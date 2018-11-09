using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using Microsoft.EntityFrameworkCore;
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
    }

    public class StudentExcelModel
    {
        [Column(2)] public string Code { get; set; }
    }
}