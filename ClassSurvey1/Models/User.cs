using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public string Password { get; set; }

        public Lecturer Id1 { get; set; }
        public Student Id2 { get; set; }
        public Admin IdNavigation { get; set; }
    }
}
