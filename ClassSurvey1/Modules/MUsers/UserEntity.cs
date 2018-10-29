
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules
{
    public class UserEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public UserEntity() { }

        //public UserEntity(User User)
        //{
        //    this.Id = User.Id;
        //    this.Username = User.Username;
        //    this.Password = User.Password;
        //    ROLES Roles = ROLES.USER;
        //    if (User.Admin != null) Roles = Roles | ROLES.ADMIN;
        //    if (User.Student != null) Roles |= ROLES.STUDENT;
        //    if (User.Lecturer != null) Roles |= ROLES.LECTURER;
        //    if (User.HrEmployee != null) Roles |= ROLES.HrEmployee;
        //    this.Roles = Roles.ToString().Replace(" ", "").Split(",").ToList();
        //}
        
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
}
