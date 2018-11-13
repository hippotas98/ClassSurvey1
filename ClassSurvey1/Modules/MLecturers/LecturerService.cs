using System;
using System.Collections.Generic;
using System.Linq;
using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassSurvey1.Modules.MLecturers
{
    public interface ILecturerService : ITransientService
    {
        int Count(UserEntity userEntity, LecturerSearchEntity LecturerSearchEntity);
        List<LecturerEntity> List(UserEntity userEntity, LecturerSearchEntity LecturerSearchEntity);
        LecturerEntity Get(UserEntity userEntity, Guid LecturerId);
        LecturerEntity Update(UserEntity userEntity, Guid LecturerId, LecturerEntity lecturerEntity);
        bool Delete(UserEntity userEntity, Guid LecturerId);
        List<LecturerEntity> CreateFromExcel(byte[] data);
        LecturerEntity Create(LecturerExcelModel LecturerExcelModel);
    }
    public class LecturerService : CommonService, ILecturerService
    {
        public IUserService UserService;

        public LecturerService(IUserService userService)
        {
            this.UserService = userService;
        }
        public int Count(UserEntity userEntity, LecturerSearchEntity LecturerSearchEntity)
        {
            if (LecturerSearchEntity == null) LecturerSearchEntity = new LecturerSearchEntity();
            IQueryable<Lecturer> lecturers = context.Lecturers;
            Apply(lecturers, LecturerSearchEntity);
            return lecturers.Count();
        }

        public List<LecturerEntity> List(UserEntity userEntity, LecturerSearchEntity LecturerSearchEntity)
        {
            if (LecturerSearchEntity == null) LecturerSearchEntity = new LecturerSearchEntity();
            IQueryable<Lecturer> lecturers = context.Lecturers.Include(l=>l.Classes);
            Apply(lecturers, LecturerSearchEntity);
            lecturers = LecturerSearchEntity.SkipAndTake(lecturers);
            return lecturers.Select(l => new LecturerEntity(l,l.Classes)).ToList();

        }

        public LecturerEntity Get(UserEntity userEntity, Guid LecturerId)
        {
            Lecturer Lecturer = context.Lecturers.Include(l=>l.Classes).FirstOrDefault(c => c.Id == LecturerId);
            if (Lecturer == null) throw new NotFoundException("Class Not Found");
            return new LecturerEntity(Lecturer,Lecturer.Classes);
            
        }

        public LecturerEntity Update(UserEntity userEntity, Guid LecturerId, LecturerEntity lecturerEntity)
        {
            Lecturer Lecturer = context.Lecturers.Include(c => c.Classes).FirstOrDefault(c => c.Id == LecturerId);
            if (Lecturer == null) throw new NotFoundException("Class Not Found");
            Lecturer updateLecturer = new Lecturer(lecturerEntity);
            updateLecturer.CopyTo(Lecturer);
            context.SaveChanges();
            List<Class> Classes = context.Classes.Where(sc => sc.LecturerId == LecturerId).ToList();
            List<Class> Insert, Update, Delete;
            List<Class> newClasses = lecturerEntity.Classes == null
                ? new List<Class>()
                : lecturerEntity.Classes.Select(c => new Class(c)).ToList();
            Common<Class>.Split(newClasses, Classes, out Insert, out Update, out Delete);
            if(Insert!=null)
                foreach(var c in Insert)
                {
                    c.Id = Guid.NewGuid();
                    c.LecturerId = Lecturer.Id;
                    Classes.Add(c);
                }
            if(Update != null)
                foreach(var c in Update)
                {
                    var curClass = Classes.FirstOrDefault(s => s.Id == c.Id);
                    Common<Class>.Copy(c, curClass);
                }
            if(Delete != null)
                foreach(var c in Delete)
                {
                    var deleteClass = Classes.FirstOrDefault(s => c.Id == s.Id);
                    Classes.Remove(deleteClass);
                }
            context.SaveChanges();
            
            return new LecturerEntity(Lecturer, Lecturer.Classes);
        }

        public bool Delete(UserEntity userEntity, Guid LecturerId)
        {
            var CurrentLecturer = context.Lecturers.FirstOrDefault(c => c.Id == LecturerId);
            if (CurrentLecturer == null) return false;
            context.Lecturers.Remove(CurrentLecturer);
            context.SaveChanges();
            return true;
        }

        public List<LecturerEntity> CreateFromExcel(byte[] data)
        {
            List<LecturerEntity> lecturerEntities = new List<LecturerEntity>();
            if (data != null)
            {
                List<LecturerExcelModel> LecturerExcelModels = ConvertToIEnumrable<LecturerExcelModel>(data).ToList();
                
                foreach (var lecturerExcelModel in LecturerExcelModels.Where(lem => lem.Name != null))
                {
                    var userEntity = new UserEntity();
                    userEntity.Password = lecturerExcelModel.Password;
                    userEntity.Username = lecturerExcelModel.UserName;
                    UserService.Create(userEntity);
                    var user = context.Users.FirstOrDefault(u => u.Name == lecturerExcelModel.UserName);
                    user.Role = 2;
                    //Create User 
                    var newLecturerEntity = new LecturerEntity();
                    newLecturerEntity = lecturerExcelModel.ToEntity(newLecturerEntity);
                    newLecturerEntity.Id = user.Id;
                    var newLecturer = new Lecturer(newLecturerEntity);
                    context.Lecturers.Add(newLecturer);
                    context.SaveChanges();
                    lecturerEntities.Add(new LecturerEntity(newLecturer));
                }
            }
            return lecturerEntities;
        }

        public LecturerEntity Create(LecturerExcelModel lecturerExcelModel)
        {
            var userEntity = new UserEntity();
            userEntity.Password = lecturerExcelModel.Password;
            userEntity.Username = lecturerExcelModel.UserName;
            var user = context.Users.FirstOrDefault(u => u.Name == lecturerExcelModel.UserName);
            user.Role = 2;
            //Create User 
            var newLecturerEntity = new LecturerEntity();
            newLecturerEntity = lecturerExcelModel.ToEntity(newLecturerEntity);
            newLecturerEntity.Id = user.Id;
            var newLecturer = new Lecturer(newLecturerEntity);
            context.Lecturers.Add(newLecturer);
            context.SaveChanges();
            return newLecturerEntity;
        }
        private void Apply(IQueryable<Lecturer> lecturers, LecturerSearchEntity LecturerSearchEntity)
        {
            if (LecturerSearchEntity.Name != null)
            {
                lecturers = lecturers.Where(l=>l.Name.Contains(LecturerSearchEntity.Name) || LecturerSearchEntity.Name.Contains(l.Name));
            }
            if (LecturerSearchEntity.Phone != null)
            {
                lecturers = lecturers.Where(l=>l.Phone.Contains(LecturerSearchEntity.Phone) || LecturerSearchEntity.Phone.Contains(l.Phone));
            }
            if (LecturerSearchEntity.Vnumail != null)
            {
                lecturers = lecturers.Where(l=>l.Vnumail.Equals(LecturerSearchEntity.Vnumail));
            }
            if (LecturerSearchEntity.LecturerCode != null)
            {
                lecturers = lecturers.Where(l=>l.LecturerCode.Equals(LecturerSearchEntity.LecturerCode));
            }
            return;
        }
    }
    public class LecturerExcelModel 
    {
        [Column(2)]
        public string UserName { get; set; }
        [Column(3)]
        public string Password { get; set; }
        [Column(4)]
        public string Name { get; set; }
        [Column(6)]
        public string LecturerCode { get; set; }
        [Column(5)]
        public string Vnumail { get; set; }

        public LecturerEntity ToEntity(LecturerEntity lecturerEntity)
        {
            if (lecturerEntity == null)
            {
                lecturerEntity.Id = Guid.NewGuid();
               
            }

            lecturerEntity.Name = this.Name;
            lecturerEntity.Vnumail = this.Vnumail;
            lecturerEntity.LecturerCode = this.LecturerCode;
            return lecturerEntity;
        }
    }
}