﻿using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassSurvey1.Modules;

namespace ClassSurvey1.Entities
{
    public partial class LecturerEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public int? Role { get; set; }
        public string Phone { get; set; }
        public string LecturerCode { get; set; }
        public string Username { get; set; }
        public User User { get; set; }
        public ICollection<ClassEntity> Classes { get; set; }
        public LecturerEntity() : base() { }
        public LecturerEntity(Lecturer lecturer, params object[] args) : base(lecturer)
        {
            foreach(var arg in args)
            {
                if (arg is ICollection<Class> classes)
                {
                    this.Classes = classes.Select(c => new ClassEntity(c)).ToList();
                }

//                if (arg is User User)
//                {
//                    this.Username = User.Username;
//                }
                
            }
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

            lecturerEntity.Username = this.UserName;
            lecturerEntity.Name = this.Name;
            lecturerEntity.Vnumail = this.Vnumail;
            lecturerEntity.LecturerCode = this.LecturerCode;
            return lecturerEntity;
        }
    }
    public partial class LecturerSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Vnumail { get; set; }
        public string Phone { get; set; }
        public string LecturerCode { get; set; }
    }
}
