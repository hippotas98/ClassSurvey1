using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentClas = new HashSet<StudentClas>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int Role { get; set; }
        public string Content { get; set; }
        public string Vnumail { get; set; }

        public User User { get; set; }
        public ICollection<StudentClas> StudentClas { get; set; }
    }
}
