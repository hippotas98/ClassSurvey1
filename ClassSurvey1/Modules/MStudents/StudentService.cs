using System;
using System.Collections.Generic;
using System.Linq;
using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassSurvey1.Modules.MStudents
{
    public interface IStudentService : ITransientService
    {
        int Count(UserEntity userEntity, StudentSearchEntity StudentSearchEntity);
        List<StudentEntity> List(UserEntity userEntity, StudentSearchEntity StudentSearchEntity);
        StudentEntity Get(UserEntity userEntity, Guid StudentId);
        StudentEntity Update(UserEntity userEntity, Guid StudentId, StudentEntity StudentEntity);
        bool Delete(UserEntity userEntity, Guid StudentId);
        List<StudentEntity> CreateFromExcel(byte[] data);
        StudentEntity Create(StudentExcelModel StudentExcelModel);
        List<ClassEntity> GetClasses(Guid StudentId);
        
    }

    public class StudentService : CommonService, IStudentService
    {
        public IUserService UserService;

        public StudentService(IUserService userService)
        {
            this.UserService = userService;
        }

        public int Count(UserEntity userEntity, StudentSearchEntity StudentSearchEntity)
        {
            if (StudentSearchEntity == null) StudentSearchEntity = new StudentSearchEntity();
            IQueryable<Student> Students = context.Students;
            Apply(Students, StudentSearchEntity);
            return Students.Count();
        }

        public List<StudentEntity> List(UserEntity userEntity, StudentSearchEntity StudentSearchEntity)
        {
            if (StudentSearchEntity == null) StudentSearchEntity = new StudentSearchEntity();
            IQueryable<Student> Students = context.Students;
            Apply(Students, StudentSearchEntity);
            Students = StudentSearchEntity.SkipAndTake(Students);
            return Students.Select(l => new StudentEntity(l)).ToList();
        }
        public List<ClassEntity> GetClasses(Guid StudentId)
        {
            List<StudentClass> studentClasses = context.StudentClasses.Where(sc=>sc.StudentId == StudentId).ToList();
            if(studentClasses == null) throw new NotFoundException("Class Not Found");
            List<ClassEntity> result = new List<ClassEntity>();
            foreach (var sc in studentClasses)
            {
                result.Add(new ClassEntity(context.Classes.Include(c=>c.StudentClasses).FirstOrDefault(c => c.Id == sc.ClassId)));
            }

            return result;
        }
        
        public StudentEntity Get(UserEntity userEntity, Guid StudentId)
        {
            Student Student = context.Students.FirstOrDefault(c => c.Id == StudentId); ///add include later
            if (Student == null) throw new NotFoundException("Student Not Found");
            return new StudentEntity(Student);
        }

        public StudentEntity Update(UserEntity userEntity, Guid StudentId, StudentEntity StudentEntity)
        {
            Student Student = context.Students.FirstOrDefault(c => c.Id == StudentId); //add include later
            if (Student == null) throw new NotFoundException("Student Not Found");
            Student updateStudent = new Student(StudentEntity);
            updateStudent.CopyTo(Student);
            context.SaveChanges();
            List<StudentClass> studentClasses = context.StudentClasses.Where(sc => sc.StudentId == StudentId).ToList();
            List<StudentClass> Insert, Update, Delete;
            List<StudentClass> newStudentClasses = StudentEntity.StudentClasses == null
                ? new List<StudentClass>()
                : StudentEntity.StudentClasses.Select(sc => new StudentClass(sc)).ToList();
            Common<StudentClass>.Split(newStudentClasses, studentClasses, out Insert, out Update, out Delete);
            if (Insert != null)
                foreach (var sc in Insert)
                {
                    sc.Id = Guid.NewGuid();
                    sc.StudentId = Student.Id;
                    studentClasses.Add(sc);
                }

            if (Update != null)
                foreach (var sc in Update)
                {
                    var curClass = studentClasses.FirstOrDefault(s => s.Id == sc.Id);
                    Common<StudentClass>.Copy(sc, curClass);
                }

            if (Delete != null)
                foreach (var sc in Delete)
                {
                    var deleteClass = studentClasses.FirstOrDefault(s => sc.Id == s.Id);
                    studentClasses.Remove(deleteClass);
                }

            context.SaveChanges();

            return new StudentEntity(Student);
        }

        public bool Delete(UserEntity userEntity, Guid StudentId)
        {
            var CurrentStudent = context.Students.FirstOrDefault(c => c.Id == StudentId);
            if (CurrentStudent == null) return false;
            context.Students.Remove(CurrentStudent);
            context.SaveChanges();
            return true;
        }

        public List<StudentEntity> CreateFromExcel(byte[] data)
        {
            List<StudentEntity> StudentEntities = new List<StudentEntity>();
            if (data != null)
            {
                List<StudentExcelModel> StudentExcelModels =
                    ConvertToIEnumrable<StudentExcelModel>(data).ToList();

                foreach (var StudentExcelModel in StudentExcelModels.Where(sem => sem.Name != null))
                {
                    var userEntity = new UserEntity();
                    userEntity.Password = StudentExcelModel.Password;
                    userEntity.Username = StudentExcelModel.UserName;
                    UserService.Create(userEntity);
                    var user = context.Users.FirstOrDefault(u => u.Name == StudentExcelModel.UserName);
                    user.Role = 4;
                    //Create User 
                    var newStudentEntity = new StudentEntity();
                    newStudentEntity = StudentExcelModel.ToEntity(newStudentEntity);
                    newStudentEntity.Id = user.Id;
                    var newStudent = new Student(newStudentEntity);
                    context.Students.Add(newStudent);
                    context.SaveChanges();
                    StudentEntities.Add(new StudentEntity(newStudent));
                }
            }

            return StudentEntities;
        }

        public StudentEntity Create(StudentExcelModel StudentExcelModel)
        {
            var userEntity = new UserEntity();
            userEntity.Password = StudentExcelModel.Password;
            userEntity.Username = StudentExcelModel.UserName;
            UserService.Create(userEntity);
            var user = context.Users.FirstOrDefault(u => u.Name == StudentExcelModel.UserName);
            user.Role = 4;
            //Create User 
            var newStudentEntity = new StudentEntity();
            newStudentEntity = StudentExcelModel.ToEntity(newStudentEntity);
            newStudentEntity.Id = user.Id;
            var newStudent = new Student(newStudentEntity);
            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudentEntity;
        }

        private void Apply(IQueryable<Student> Students, StudentSearchEntity StudentSearchEntity)
        {
            if (StudentSearchEntity.Name != null)
            {
                Students = Students.Where(l =>
                    l.Name.Contains(StudentSearchEntity.Name) || StudentSearchEntity.Name.Contains(l.Name));
            }

            if (StudentSearchEntity.Code != null)
            {
                Students = Students.Where(l =>
                    l.Code == StudentSearchEntity.Code);
            }

            return;
        }
    }

    public class StudentExcelModel
    {
        [Column(2)] public string UserName { get; set; }
        [Column(3)] public string Password { get; set; }
        [Column(4)] public string Name { get; set; }
        //[Column(2)] public string StudentCode { get; set; }
        [Column(5)] public string Vnumail { get; set; }
        [Column(6)] public string Class { get; set; }

        public StudentEntity ToEntity(StudentEntity StudentEntity)
        {
            if (StudentEntity == null)
            {
                StudentEntity.Id = Guid.NewGuid();
            }

            StudentEntity.Name = this.Name;
            StudentEntity.Vnumail = this.Vnumail;
            StudentEntity.Code = this.UserName;
            StudentEntity.Class = this.Class;
            return StudentEntity;
        }
    }
}