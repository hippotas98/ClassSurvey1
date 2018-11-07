using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class Class
    {
        public Class()
        {
            StudentClasses = new HashSet<StudentClass>();
        }

        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        public int StudentNumber { get; set; }
        public Guid LectureId { get; set; }
        public string Subject { get; set; }
        public decimal? M { get; set; }
        public decimal? M1 { get; set; }
        public decimal? M2 { get; set; }
        public decimal? Std { get; set; }
        public decimal? Std1 { get; set; }
        public decimal? Std2 { get; set; }

        public Lecturer Lecture { get; set; }
        public ICollection<StudentClass> StudentClasses { get; set; }
    }
}
