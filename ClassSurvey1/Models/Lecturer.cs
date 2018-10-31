using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            Clas = new HashSet<Clas>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public int Role { get; set; }
        public string Phone { get; set; }
        public string LectureCode { get; set; }

        public User User { get; set; }
        public ICollection<Clas> Clas { get; set; }
    }
}
