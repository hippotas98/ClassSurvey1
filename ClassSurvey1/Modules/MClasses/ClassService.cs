using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules.MClasses
{
    public interface IClassService : ITransientService
    {
        int Count(UserEntity userEntity, ClassSearchEntity classSearchEntity);
        List<ClassEntity> List(UserEntity userEntity, ClassSearchEntity classSearchEntity);
        ClassEntity Get(UserEntity userEntity, Guid ClassId);
        ClassEntity Update(UserEntity userEntity, Guid ClassId, ClassEntity classEntity);
        bool Delete(UserEntity userEntity, Guid ClassId);
        ClassEntity Create(byte[] data);
    }

    public class ClassService : CommonService, IClassService
    {
        public ClassService() : base()
        {
        }

        public int Count(UserEntity userEntity, ClassSearchEntity classSearchEntity)
        {
            if (classSearchEntity == null) classSearchEntity = new ClassSearchEntity();
            IQueryable<Class> classes = context.Classes;
            Apply(classes, classSearchEntity);
            return classes.Count();
        }

        public List<ClassEntity> List(UserEntity userEntity, ClassSearchEntity classSearchEntity)
        {
            if (classSearchEntity == null) classSearchEntity = new ClassSearchEntity();
            IQueryable<Class> classes = context.Classes;
            Apply(classes, classSearchEntity);
            classes = classSearchEntity.SkipAndTake(classes);
            return classes.Select(c => new ClassEntity(c)).ToList();
        }

        public ClassEntity Get(UserEntity userEntity, Guid ClassId)
        {
            Class Class = context.Classes.Include(c => c.StudentClasses).FirstOrDefault(c => c.Id == ClassId);
            if (Class == null) throw new NotFoundException("Class Not Found");
            if (//Class.OpenedDate != null && Class.ClosedDate != null && 
                //DateTime.UtcNow > Class.OpenedDate  && DateTime.UtcNow > Class.ClosedDate &&
                Class.M == null)
            {
                Average();
                StandardDeviation();
                Average1();
                StandardDeviation1();
                Average2();
                StandardDeviation2();
            }
            return new ClassEntity(Class);
        }

        public ClassEntity Update(UserEntity userEntity, Guid ClassId, ClassEntity classEntity)
        {
            Class Class = context.Classes.Include(c => c.StudentClasses).FirstOrDefault(c => c.Id == ClassId);
            if (Class == null) throw new NotFoundException("Class Not Found");
            Class updateClass = new Class(classEntity);
            updateClass.CopyTo(Class);
            context.SaveChanges();
            List<StudentClass> studentClasses = context.StudentClasses.Where(sc => sc.ClassId == ClassId).ToList();
            List<StudentClass> Insert, Update, Delete;
            List<StudentClass> newStudentClasses = classEntity.StudentClasses == null
                ? new List<StudentClass>()
                : classEntity.StudentClasses.Select(sc => new StudentClass(sc)).ToList();
            Common<StudentClass>.Split(newStudentClasses, studentClasses, out Insert, out Update, out Delete);
            if (Insert != null)
                foreach (var sc in Insert)
                {
                    sc.Id = Guid.NewGuid();
                    sc.ClassId = Class.Id;
                    studentClasses.Add(sc);
                }

            if (Update != null)
                foreach (var sc in Update)
                {
                    var curStudentClass = studentClasses.FirstOrDefault(s => s.Id == sc.Id);
                    Common<StudentClass>.Copy(sc, curStudentClass);
                }

            if (Delete != null)
                foreach (var sc in Delete)
                {
                    var deleteStudentClass = studentClasses.FirstOrDefault(s => sc.Id == s.Id);
                    studentClasses.Remove(deleteStudentClass);
                }

            context.SaveChanges();

            return classEntity;
        }

        public bool Delete(UserEntity userEntity, Guid ClassId)
        {
            var CurrentClass = context.Classes.FirstOrDefault(c => c.Id == ClassId);
            if (CurrentClass == null) return false;
            var CurrentStudentClasses = context.StudentClasses.Where(sc => sc.ClassId == ClassId).ToList();
            if (CurrentStudentClasses != null)
            {
                foreach (var sc in CurrentStudentClasses)
                {
                    context.StudentClasses.Remove(sc);
                }

                context.SaveChanges();
            }

            context.Classes.Remove(CurrentClass);
            context.SaveChanges();
            return true;
        }

        public ClassEntity Create(byte[] data)
        {
            Class newClass;
            newClass = new Class();
            newClass.Id = Guid.NewGuid();
            if (data != null)
            {
                List<StudentExcelModel> studentModelEntities = ConvertToIEnumrable<StudentExcelModel>(data).ToList();
                string lecturerCode = GetPropValueFromExcel(data, "Mã cán bộ");
                if (lecturerCode == "") throw new BadRequestException("Cannot Get Ma can bo");
                //newClass.LectureId = new Guid(Id);
                var lecturer = context.Lecturers.FirstOrDefault(l => l.LecturerCode == lecturerCode);
                newClass.LecturerId = lecturer.Id;
                //newClass.Lecture = lecturer;
                newClass.Subject = GetPropValueFromExcel(data, "Môn học");
                newClass.ClassCode = GetPropValueFromExcel(data, "Lớp môn học");
                var Students = context.Students;
                foreach (var studentModel in studentModelEntities.Where(sme => sme.Code != null))
                {
                    var student = Students.FirstOrDefault(s => s.Code.ToString().Equals(studentModel.Code));
                    var StudentClass = new StudentClass();
                    StudentClass.Id = Guid.NewGuid();
                    //StudentClass.StudentId = student.Id;
                    StudentClass.ClassId = newClass.Id;
                    //StudentClass.Class = newClass;
                    StudentClass.Student = student;
                    context.StudentClasses.Add(StudentClass);
                }

                context.SaveChanges();
            }

            context.Classes.Add(newClass);
            context.SaveChanges();
            return new ClassEntity(newClass);
        }

        private void Apply(IQueryable<Class> classes, ClassSearchEntity classSearchEntity)
        {
            if (classSearchEntity.ClassCode != null)
            {
                classes = classes.Where(c => c.ClassCode == classSearchEntity.ClassCode);
            }

            if (classSearchEntity.LecturerId != null)
            {
                classes = classes.Where(c => c.LecturerId == classSearchEntity.LecturerId);
            }

            if (classSearchEntity.Subject != null)
            {
                classes = classes.Where(c =>
                    c.Subject.Contains(classSearchEntity.Subject) || classSearchEntity.Subject.Contains(c.Subject));
            }

            return;
        }
        private void Average()
        {
            List<Class> classes = context.Classes.ToList();
            //List<Dictionary<string, float>> averages = new List<Dictionary<string, float>>();
            foreach(var Class in classes)
            {
                
                List<StudentClass> studentClasses = context.StudentClasses.Where(sc => sc.ClassId == Class.Id).ToList(); //Get All Student Classes Which have same ClassId
                List<Dictionary<string, int>> surveyResults = new List<Dictionary<string, int>>(); //Get Survey Result from all student classes that we found before
                Dictionary<string, float> Ms = new Dictionary<string, float>(); //Get results with Key and M
                foreach(var studentClass in studentClasses)
                {
                    Survey survey = context.Surveys.FirstOrDefault(s => s.StudentClassId == studentClass.Id);
                    surveyResults.Add(JsonConvert.DeserializeObject<Dictionary<string, int>>(survey.Content));
                }
                var keys = surveyResults[0].Keys; //Get all keys from survey results
                foreach(var key in keys)
                {
                    List<int> values = surveyResults.Select(sr => sr[key]).ToList(); //find values that have the same key
                    int sum = 0;
                    values.ForEach(v => sum += v);
                    float average = sum / values.Count();
                    Ms.Add(key, average);
                    
                }
                Class.M = JsonConvert.SerializeObject(Ms);
                //averages.Add(Ms);
            }
            //return averages;
        }
        private void StandardDeviation()
        {
            List<Class> classes = context.Classes.ToList();
            //List<Dictionary<string, double>> deviations = new List<Dictionary<string, double>>();
            foreach (var Class in classes)
            {

                List<StudentClass> studentClasses = context.StudentClasses.Where(sc => sc.ClassId == Class.Id).ToList(); 
                List<Dictionary<string, int>> surveyResults = new List<Dictionary<string, int>>();
                Dictionary<string, double> Stds = new Dictionary<string, double>();
                foreach (var studentClass in studentClasses)
                {
                    Survey survey = context.Surveys.FirstOrDefault(s => s.StudentClassId == studentClass.Id);
                    surveyResults.Add(JsonConvert.DeserializeObject<Dictionary<string, int>>(survey.Content));
                }
                var keys = surveyResults[0].Keys;
                foreach (var key in keys)
                {
                    List<int> values = surveyResults.Select(sr => sr[key]).ToList();
                    float sum = 0;
                    values.ForEach(v => sum += v);
                    float average = sum / values.Count();
                    double variance = 0;
                    values.ForEach(v => variance += Math.Pow((v-average),2));
                    double deviation = Math.Sqrt(variance);
                    Stds.Add(key, deviation);
                }
                Class.Std = JsonConvert.SerializeObject(Stds);
                //deviations.Add(Stds);
            }
            //return deviations;
        }
        private void Average1()
        {
            List<Class> classes = context.Classes.ToList();
            foreach (var Class in classes)
            {
                string subject = Class.Subject;
                Guid LecturerId = Class.LecturerId;
                Dictionary<string, float> Ms = JsonConvert.DeserializeObject<Dictionary<string, float>>(Class.M);
                Dictionary<string, float> M1s = new Dictionary<string, float>();
                List<string> keys = Ms.Keys.ToList();
                foreach(var key in keys)
                {
                    List<float> values = new List<float>();
                    foreach(var otherClass in classes.Where(c=>c.LecturerId!=LecturerId  && c.Subject == subject))
                    {
                        Dictionary<string, float> M = JsonConvert.DeserializeObject<Dictionary<string, float>>(otherClass.M);
                        values.Add(M[key]);
                    }
                    float sum = 0;
                    values.ForEach(v => sum += v);
                    float M1 = sum / values.Count();
                    M1s.Add(key, M1);
                }
                Class.M1 = JsonConvert.SerializeObject(M1s);
            }
        }
        private void StandardDeviation1()
        {
            List<Class> classes = context.Classes.ToList();
            foreach (var Class in classes)
            {
                string subject = Class.Subject;
                Guid LecturerId = Class.LecturerId;
                Dictionary<string, float> Stds = JsonConvert.DeserializeObject<Dictionary<string, float>>(Class.Std);
                Dictionary<string, double> Std1s = new Dictionary<string, double>();
                List<string> keys = Stds.Keys.ToList();
                foreach (var key in keys)
                {
                    List<float> values = new List<float>();
                    foreach (var otherClass in classes.Where(c => c.LecturerId != LecturerId && c.Subject == subject))
                    {
                        Dictionary<string, float> Std = JsonConvert.DeserializeObject<Dictionary<string, float>>(otherClass.Std);
                        values.Add(Std[key]);
                    }
                    float sum = 0;
                    values.ForEach(v => sum += v);
                    float M1 = sum / values.Count();
                    double variance = 0;
                    values.ForEach(v => variance += Math.Pow((v - M1), 2));
                    double Std1 = Math.Sqrt(variance);
                    Std1s.Add(key, Std1);
                }
                Class.M1 = JsonConvert.SerializeObject(Std1s);
            }
        }
        private void Average2()
        {
            List<Class> classes = context.Classes.ToList();
            foreach (var Class in classes)
            {
                string subject = Class.Subject;
                Guid LecturerId = Class.LecturerId;
                Dictionary<string, float> Ms = JsonConvert.DeserializeObject<Dictionary<string, float>>(Class.M);
                Dictionary<string, float> M2s = new Dictionary<string, float>();
                List<string> keys = Ms.Keys.ToList();
                foreach (var key in keys)
                {
                    List<float> values = new List<float>();
                    foreach (var otherClass in classes.Where(c => c.LecturerId == LecturerId && c.Subject != subject))
                    {
                        Dictionary<string, float> M = JsonConvert.DeserializeObject<Dictionary<string, float>>(otherClass.M);
                        values.Add(M[key]);
                    }
                    float sum = 0;
                    values.ForEach(v => sum += v);
                    float M2 = sum / values.Count();
                    M2s.Add(key, M2);
                }
                Class.M2 = JsonConvert.SerializeObject(M2s);
            }
        }
        private void StandardDeviation2()
        {
            List<Class> classes = context.Classes.ToList();
            foreach (var Class in classes)
            {
                string subject = Class.Subject;
                Guid LecturerId = Class.LecturerId;
                Dictionary<string, float> Stds = JsonConvert.DeserializeObject<Dictionary<string, float>>(Class.Std);
                Dictionary<string, double> Std2s = new Dictionary<string, double>();
                List<string> keys = Stds.Keys.ToList();
                foreach (var key in keys)
                {
                    List<float> values = new List<float>();
                    foreach (var otherClass in classes.Where(c => c.LecturerId == LecturerId && c.Subject != subject))
                    {
                        Dictionary<string, float> Std = JsonConvert.DeserializeObject<Dictionary<string, float>>(otherClass.Std);
                        values.Add(Std[key]);
                    }
                    float sum = 0;
                    values.ForEach(v => sum += v);
                    float M2 = sum / values.Count();
                    double variance = 0;
                    values.ForEach(v => variance += Math.Pow((v - M2), 2));
                    double Std2 = Math.Sqrt(variance);
                    Std2s.Add(key, Std2);
                }
                Class.M2 = JsonConvert.SerializeObject(Std2s);
            }
        }
    }

    public class StudentExcelModel
    {
        [Column(2)] public string Code { get; set; }
    }
}