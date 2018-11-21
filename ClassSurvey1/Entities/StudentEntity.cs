﻿using ClassSurvey1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassSurvey1.Modules;

namespace ClassSurvey1.Entities
{
    public partial class StudentEntity : BaseEntity
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public int? Role { get; set; }
        public string Content { get; set; }
        public string Vnumail { get; set; }
        public string Class { get; set; }
        public User User { get; set; }
        public ICollection<StudentClassEntity> StudentClasses { get; set; }
        public StudentEntity() : base()
        {

        }
        public StudentEntity(Student student, params object [] args) : base(student)
        {
            //this.Username = this.Code;
            foreach(var arg in args)
            {
                if(arg is ICollection<StudentClass> studentClasses)
                {
                    this.StudentClasses = studentClasses.Select(s => new StudentClassEntity(s)).ToList();
                }

                if (arg is User User)
                {
                    this.Username = User.Username;
                }
            }
        }
    }
    class StudentContent //tam thua
    {
        public string Class { get; set; }
    }
    public partial class StudentSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
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
