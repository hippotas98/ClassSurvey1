﻿using ClassSurvey1.Models;
using System;


namespace ClassSurvey1.Entities
{
    public partial class AdminEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public int? Role { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public User User { get; set; }
        public AdminEntity() : base()
        {
        }
        public AdminEntity(Admin admin, params object[] args) : base(admin)
        {
            
        }
    }
    public partial class AdminSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
    }
    public class AdminDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }

        public AdminEntity ToEntity(AdminEntity adminEntity)
        {
            adminEntity.Name = this.Name;
            adminEntity.Vnumail = this.Vnumail;
            adminEntity.Username = this.Username;
            return adminEntity;
        }
    }
}
