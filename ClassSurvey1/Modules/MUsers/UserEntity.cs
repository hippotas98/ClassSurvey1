
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassSurvey1.Models;

namespace ClassSurvey1.Modules
{
    public class UserEntity : BaseEntity
    {
        public Guid Id { get; set; }
        [Column(1)]
        public string Username { get; set; }
        [Column(2)]
        public string Password { get; set; }
        [Column(3)]
        public List<string> Roles { get; set; }
        public UserEntity() { }

        public UserEntity(User User) : base(User)
        {
            ROLES Roles = ROLES.USER;
            if (User.IdNavigation!= null) Roles = Roles | ROLES.ADMIN;
            if (User.Id2 != null) Roles |= ROLES.STUDENT;
            if (User.Id1 != null) Roles |= ROLES.LECTURER;
            
            this.Roles = Roles.ToString().Replace(" ", "").Split(",").ToList();
        }
        
    }

    public class AdminEntity
    {
        public Guid Id;
        public string Fullname;
        public string Organization;
        public AdminEntity() { }

        //public AdminEntity(Admin Admin)
        //{
        //    this.Id = Admin.Id;
        //    this.Fullname = Admin.Fullname;
        //    this.Organization = Admin.Organization;
        //}
    }

    public enum ROLES
    {
        USER = 0,
        ADMIN = 1,
        LECTURER = 2,
        STUDENT = 4,
    }
}
