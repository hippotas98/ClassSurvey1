using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentClasses = new HashSet<StudentClass>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int Role { get; set; }
        public string Content { get; set; }
        public string Vnumail { get; set; }

        public User User { get; set; }
        public ICollection<StudentClass> StudentClasses { get; set; }
    }
}
