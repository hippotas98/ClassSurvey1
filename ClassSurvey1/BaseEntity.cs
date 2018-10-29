using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace ClassSurvey1
{
    public interface ICommand { }
    public class BaseEntity
    {

        public Dictionary<string, string> Errors = new Dictionary<string, string>();

        public void AddError(string Key, string Value)
        {
            if (Errors == null) Errors = new Dictionary<string, string>();
            if (Errors.ContainsKey(Key))
                Errors[Key] += Value;
            else
                Errors.Add(Key, Value);
        }

        public string ConvertToUnsign(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            string[] signs = new string[] {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
                }
            }

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c.Equals(' ') || ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || c.Equals(',') || c.Equals('.') || ('0' <= c && c <= '9'))
                {
                    continue;
                }
                else
                {
                    str = str.Replace(c, ' ');
                }
            }
            return str;
        }

        public BaseEntity() { }
        public BaseEntity(object obj)
        {
            Common<object>.Copy(obj, this);
        }
    }


    public interface IFilterEntity
    {
        int Take { get; set; }
        int Skip { get; set; }
        string SortBy { get; set; }
        SortType? SortType { get; set; }
    }

    public class FilterEntity : IFilterEntity
    {
        public Guid? CurrentUserId;
        public int Take { get; set; }
        public int Skip { get; set; }
        public string SortBy { get; set; }
        public SortType? SortType { get; set; }
        public FilterEntity()
        {
            if (Take == 0) Take = 10;
            if (Skip == 0) Skip = 0;
            if (string.IsNullOrEmpty(SortBy)) SortBy = "Cx";
            if (!SortType.HasValue) SortType = ClassSurvey1.SortType.ASC;
        }
    }
    public enum SortType
    {
        NONE = 0,
        DESC = 1,
        ASC = 2
    }
}
