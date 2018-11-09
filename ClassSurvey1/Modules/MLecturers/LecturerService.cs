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
        List<LecturerEntity> Create(byte[] data, LecturerEntity lecturerEntity = null);
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
            IQueryable<Lecturer> lecturers = context.Lecturers;
            Apply(lecturers, LecturerSearchEntity);
            SkipAndTake(lecturers, LecturerSearchEntity);
            return lecturers.Include(l => l.Classes).Select(l => new LecturerEntity(l)).ToList();

        }

        public LecturerEntity Get(UserEntity userEntity, Guid LecturerId)
        {
            Lecturer Lecturer = context.Lecturers.Include(l=>l.Classes).FirstOrDefault(c => c.Id == LecturerId);
            if (Lecturer == null) throw new NotFoundException("Class Not Found");
            return new LecturerEntity(Lecturer);
            
        }

        public LecturerEntity Update(UserEntity userEntity, Guid LecturerId, LecturerEntity lecturerEntity)
        {
            Lecturer Lecturer = context.Lecturers.Include(c => c.Classes).FirstOrDefault(c => c.Id == LecturerId);
            if (Lecturer == null) throw new NotFoundException("Class Not Found");
            Lecturer updateLecturer = new Lecturer(lecturerEntity);
            Common<Lecturer>.Copy(updateLecturer, Lecturer);
            context.SaveChanges();
            List<Class> Classes = context.Classes.Where(sc => sc.LectureId == LecturerId).ToList();
            List<Class> Insert, Update, Delete;
            Common<Class>.Split(lecturerEntity.Classes.Select(c=>new Class(c)).ToList(), Classes, out Insert, out Update, out Delete);
            foreach(var c in Insert)
            {
                c.Id = Guid.NewGuid();
                c.LectureId = Lecturer.Id;
                Classes.Add(c);
            }
            foreach(var c in Update)
            {
                var curClass = Classes.FirstOrDefault(s => s.Id == c.Id);
                Common<Class>.Copy(c, curClass);
            }
            foreach(var sc in Delete)
            {
                var deleteClass = Classes.FirstOrDefault(s => sc.Id == s.Id);
                Classes.Remove(deleteClass);
            }
            context.SaveChanges();
            
            return lecturerEntity;
        }

        public bool Delete(UserEntity userEntity, Guid LecturerId)
        {
            var CurrentLecturer = context.Lecturers.FirstOrDefault(c => c.Id == LecturerId);
            if (CurrentLecturer == null) return false;
            context.Lecturers.Remove(CurrentLecturer);
            context.SaveChanges();
            return true;
        }

        public List<LecturerEntity> Create(byte[] data, LecturerEntity lecturerEntity = null)
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
                    user.Role = 1;
                    //Create User 
                    var newLecturerEntity = new LecturerEntity();
                    newLecturerEntity = lecturerExcelModel.ToEntity(newLecturerEntity);
                    var newLecturer = new Lecturer(newLecturerEntity);
                    user.Id1 = newLecturer;
                    context.Lecturers.Add(newLecturer);
                    context.SaveChanges();
                    lecturerEntities.Add(new LecturerEntity(newLecturer));
                }
            }
            return lecturerEntities;
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
            if (LecturerSearchEntity.LectureCode != null)
            {
                lecturers = lecturers.Where(l=>l.LectureCode.Equals(LecturerSearchEntity.LectureCode));
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
        public string LectureCode { get; set; }
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
            lecturerEntity.LectureCode = this.LectureCode;
            return lecturerEntity;
        }
    }
}