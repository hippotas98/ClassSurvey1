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
            lecturers = Apply(lecturers, LecturerSearchEntity);
            return lecturers.Count();
        }

        public List<LecturerEntity> List(UserEntity userEntity, LecturerSearchEntity LecturerSearchEntity)
        {
            if (LecturerSearchEntity == null) LecturerSearchEntity = new LecturerSearchEntity();
            IQueryable<Lecturer> lecturers = context.Lecturers.Include(l=>l.Classes);
            lecturers =  Apply(lecturers, LecturerSearchEntity);
            //lecturers = LecturerSearchEntity.SkipAndTake(lecturers);
            return lecturers.Select(l => new LecturerEntity(l, l.Classes)).ToList();
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
            if (Lecturer == null) throw new NotFoundException("Lecturer Not Found");
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
            
            var User = context.Users.FirstOrDefault(u => u.Id == LecturerId);
            if (CurrentLecturer == null) return false;
            if (User == null) return false;
            
            context.Lecturers.Remove(CurrentLecturer);
            context.SaveChanges();
            context.Users.Remove(User);
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
                    userEntity.Username = lecturerExcelModel.Username.Trim();
                    UserService.Create(userEntity);
                    
                    var users = context.Users.Where(u => u.Username == userEntity.Username).ToList();
                    if (users.Count > 1)
                        throw new BadRequestException("Trung giang vien co username: " + userEntity.Username);
                    var user = users.FirstOrDefault();
                    user.Role = 4;
                    context.SaveChanges();
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
            userEntity.Username = lecturerExcelModel.Username.Trim();
            UserService.Create(userEntity);
            var users = context.Users.Where(u => u.Username == userEntity.Username).ToList();
            if(users.Count > 1) throw new BadRequestException("Trung giang vien");
            var user = users.FirstOrDefault();
            user.Role = 4;
            context.SaveChanges();
            //Create User 
            var newLecturerEntity = new LecturerEntity();
            newLecturerEntity = lecturerExcelModel.ToEntity(newLecturerEntity);
            newLecturerEntity.Id = user.Id;
            var newLecturer = new Lecturer(newLecturerEntity);
            context.Lecturers.Add(newLecturer);
            context.SaveChanges();
            return newLecturerEntity;
        }
        private IQueryable<Lecturer>  Apply(IQueryable<Lecturer> lecturers, LecturerSearchEntity LecturerSearchEntity)
        {
            if (LecturerSearchEntity.Name != null)
            {
                lecturers = lecturers.Where(l=>l.Name.ToLower().Contains(LecturerSearchEntity.Name.ToLower()) 
                                               || LecturerSearchEntity.Name.ToLower().Contains(l.Name.ToLower()));
            }
            if (LecturerSearchEntity.Username != null)
            {
                lecturers = lecturers.Where(l=>l.Username.ToLower().Contains(LecturerSearchEntity.Username.ToLower()) 
                                               || LecturerSearchEntity.Username.ToLower().Contains(l.Username.ToLower()));
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
            return lecturers;
        }
    }
    
}