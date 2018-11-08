﻿using ClassSurvey1.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules
{
    public interface ITransientService
    {
    }
    public interface IScopedService
    {
    }

    public interface IValidator<T>
    {
        bool ValidateCreate(T entity);
        bool ValidateUpdate(T entity);
        bool ValidateDelete(T entity);
    }
    public interface ICommonService
    {
        IEnumerable<T> ConvertToIEnumrable<T>(byte[] data) where T : new();
        string GetPropValueFromExcel(byte[] data, string prop);
    }
    public class CommonService : ICommonService
    {
        //protected IUnitOfWork UnitOfWork;
        protected ClassSurveyContext context;
        public CommonService()
        {
            context = new ClassSurveyContext();
        }

        //public CommonService(IUnitOfWork UnitOfWork)
        //{
        //    this.UnitOfWork = UnitOfWork;
        //}
        protected IQueryable<T> SkipAndTake<T>(IQueryable<T> source, FilterEntity FilterEntity)
        {
            string command = FilterEntity.SortType == SortType.ASC ? "OrderBy" : "OrderByDescending";
            var type = typeof(T);
            var property = type.GetProperty(FilterEntity.SortBy);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            source = source.Provider.CreateQuery<T>(resultExpression);
            source = source.Skip(FilterEntity.Skip).Take(FilterEntity.Take);
            return source;
        }
        public IEnumerable<T> ConvertToIEnumrable<T>(byte[] data) where T : new()
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                ExcelPackage excelPackage = new ExcelPackage(ms);
                int count = excelPackage.Workbook.Worksheets.Count;
                if (count > 0)
                {
                    var worksheet = excelPackage.Workbook.Worksheets[0];
                    var list = ConvertSheetToObjects<T>(worksheet);
                    return list;
                }

                return null;
            }
        }

        static int startRow(ExcelWorksheet worksheet, string prop)
        {
            var columns = worksheet.Cells.Select(cell => cell.Start.Column).Distinct();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            foreach (var row in rows)
            {
                foreach (var column in columns)
                {
                    //Console.WriteLine(row.ToString() + "," + column.ToString());
                    var value = worksheet.Cells[row, column].Value;
                    if (value != null)
                        if (value.ToString().Contains(prop))
                        {
                            return row;
                        }
                }
            }

            return -1;
        }

        public string GetPropValueFromExcel(byte[] data, string prop)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                string result = "";
                ExcelPackage excelPackage = new ExcelPackage(ms);
                var worksheet = excelPackage.Workbook.Worksheets[0];
                //Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

                var columns = worksheet.Cells.Select(cell => cell.Start.Column).Distinct();


                var rows = worksheet.Cells
                    .Select(cell => cell.Start.Row)
                    .Distinct()
                    .OrderBy(x => x);

                foreach (var row in rows)
                {
                    foreach (var column in columns)
                    {
                        //Console.WriteLine(row.ToString() + "," + column.ToString());
                        var value = worksheet.Cells[row, column].Value;
                        if (value != null)
                            if (value.ToString().Contains(prop))
                            {
                                for(int i = 1; i < 10; ++i)
                                {
                                    if(worksheet.Cells[row, column, row, column+i].Merge == false )
                                    {
                                        if(worksheet.Cells[row, column + i].Value != null)
                                            result = worksheet.Cells[row, column + i].Value.ToString();
                                        Console.WriteLine(worksheet.Cells[row, column + i].Value);
                                    }
                                        
                                    
                                }
                                return result;
                            }
                    }
                }

                return result;
            }
        }

        static IEnumerable<T> ConvertSheetToObjects<T>(ExcelWorksheet worksheet) where T : new()
        {
            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

            var columns = typeof(T)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(columnOnly))
                .Select(p => new
                {
                    Property = p,
                    Column = p.GetCustomAttributes<Column>().First().ColumnIndex //safe because if where above
                }).ToList();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            var startPos = startRow(worksheet, "STT");
            //Create the collection container
            var collection = rows.Skip(startPos)
                .Select(row =>
                {
                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        var val = worksheet.Cells[row, col.Column];
                        //If it is numeric it is a double since that is how excel stores all numbers
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }

                        if (col.Property.PropertyType == typeof(Int32))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }

                        if (col.Property.PropertyType == typeof(double))
                        {
                            col.Property.SetValue(tnew, val.GetValue<double>());
                            return;
                        }

                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }

                        //Its a string
                        col.Property.SetValue(tnew, val.GetValue<string>());
                    });

                    return tnew;
                });


            //Send it back
            return collection;
        }
    }

    public enum ERRORCODE
    {
        NONE = 0,
        ITEM_NOT_EXISTED = 1,
        ITEM_NOT_UPLOAD = 2,
        ITEM_IS_DEFAULT = 3,
        ITEM_EXSITED = 4,
    }
    public class Column : System.Attribute
    {
        public int ColumnIndex { get; set; }


        public Column(int column)
        {
            ColumnIndex = column;
        }
    }
    public class UploadClass
    {
        public IEnumerable<IFormFile> myFiles { get; set; }

        public UploadClass()
        {
        }

        public UploadClass(List<IFormFile> myFiles)
        {
            this.myFiles = myFiles;
        }
    }
}
