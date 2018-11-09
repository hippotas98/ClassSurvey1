using ClassSurvey1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class StudentEntity : BaseEntity
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Role { get; set; }
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
            
            foreach(var arg in args)
            {
                if(arg is ICollection<StudentClass> studentClasses)
                {
                    this.StudentClasses = studentClasses.Select(s => new StudentClassEntity(s)).ToList();
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
    }
}
