using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class StudentClas
    {
        public StudentClas()
        {
            Surveys = new HashSet<Survey>();
        }

        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }

        public Clas Class { get; set; }
        public Student Student { get; set; }
        public ICollection<Survey> Surveys { get; set; }
    }
}
