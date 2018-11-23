using System;
using System.Collections.Generic;
using System.Linq;
using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassSurvey1.Modules.MAdmins
{
    public interface IAdminService : ITransientService
    {
        AdminEntity Create(UserEntity userEntity, AdminDto AdminDto);
        AdminEntity Update(UserEntity userEntity, Guid AdminId, AdminEntity AdminEntity);
        bool Delete(UserEntity userEntity, Guid AdminId);
        int Count(UserEntity userEntity, AdminSearchEntity AdminSearchEntity);
        List<AdminEntity> List(UserEntity userEntity, AdminSearchEntity AdminSearchEntity);
        AdminEntity Get(UserEntity userEntity, Guid AdminId);
    }
    public class AdminService : CommonService, IAdminService
    {
        private readonly IUserService UserService;

        public AdminService(IUserService userService)
        {
            this.UserService = userService;
        }
        public int Count(UserEntity userEntity, AdminSearchEntity AdminSearchEntity)
        {
            if (AdminSearchEntity == null) AdminSearchEntity = new AdminSearchEntity();
            IQueryable<Admin> Admins = context.Admins;
            Apply(Admins, AdminSearchEntity);
            return Admins.Count();
        }
        
        public List<AdminEntity> List(UserEntity userEntity, AdminSearchEntity AdminSearchEntity)
        {
            if (AdminSearchEntity == null) AdminSearchEntity = new AdminSearchEntity();
            IQueryable<Admin> Admins = context.Admins;
            Apply(Admins, AdminSearchEntity);
            //Admins = AdminSearchEntity.SkipAndTake(Admins);
           
//            List<User> Users = new List<User>();
//            foreach (var admin in Admins)
//            {
//                var admin_user = context.Users.FirstOrDefault(u => u.Id == admin.Id);
//                Users.Add(admin_user);
//            }
//            
//            return Admins.Join(Users, ad => ad.Id, u => u.Id, (ad, u) => new AdminEntity(ad, u))
//                .ToList();
            return Admins.Select(ad => new AdminEntity(ad)).ToList();
        }

        public AdminEntity Get(UserEntity userEntity, Guid AdminId)
        {
            Admin Admin = context.Admins.FirstOrDefault(c => c.Id == AdminId); ///add include later
            //User User = context.Users.FirstOrDefault(u => u.Id == AdminId);                                                           
            if (Admin == null) throw new NotFoundException("Admin Not Found");
            return new AdminEntity(Admin);
        }

        public AdminEntity Update(UserEntity userEntity, Guid AdminId, AdminEntity AdminEntity)
        {
            Admin Admin = context.Admins.FirstOrDefault(c => c.Id == AdminId); //add include later
            if (Admin == null) throw new NotFoundException("Admin Not Found");
            Admin updateAdmin = new Admin(AdminEntity);
            updateAdmin.CopyTo(Admin);
            context.SaveChanges();
            return new AdminEntity(Admin);
        }

        public AdminEntity Create(UserEntity userEntity, AdminDto AdminDto)
        {
            UserEntity newUserEntity = new UserEntity();
            newUserEntity.Password = AdminDto.Password;
            newUserEntity.Username = AdminDto.Username;
            UserService.Create(newUserEntity);
            var users = context.Users.Where(u => u.Username == AdminDto.Username).ToList();
            if(users.Count > 1) throw new BadRequestException("Admin bi trung username " + AdminDto.Username);
            var user = users.FirstOrDefault();
            user.Role = 2;
            context.SaveChanges();
            AdminEntity adminEntity = new AdminEntity();
            adminEntity = AdminDto.ToEntity(adminEntity);
            Admin Admin = new Admin(adminEntity);
            Admin.Id = user.Id;
            context.Admins.Add(Admin);
            context.SaveChanges();
            return new AdminEntity(Admin);   
        }


        public bool Delete(UserEntity userEntity, Guid AdminId)
        {
            var CurrentAdmin = context.Admins.FirstOrDefault(c => c.Id == AdminId);
            if (CurrentAdmin == null) return false;
            var user = context.Users.FirstOrDefault(u => u.Id == AdminId);
            
            context.Admins.Remove(CurrentAdmin);
            context.SaveChanges();
            context.Users.Remove(user);
            context.SaveChanges();
            return true;
        }
        private void Apply(IQueryable<Admin> Admins, AdminSearchEntity AdminSearchEntity)
        {
            if (AdminSearchEntity.Name != null)
            {
                Admins = Admins.Where(ad => ad.Name.ToLower().Contains(AdminSearchEntity.Name.ToLower()) ||
                                            AdminSearchEntity.Name.ToLower().Contains(ad.Name.ToLower()));
            }

            if (AdminSearchEntity.Vnumail != null)
            {
                Admins = Admins.Where(ad => ad.Vnumail == AdminSearchEntity.Vnumail);
            }
            return;
        }
    }

    
}