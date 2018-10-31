using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class VersionSurvey
    {
        public VersionSurvey()
        {
            Surveys = new HashSet<Survey>();
        }

        public Guid Id { get; set; }
        public int? Version { get; set; }
        public string Content { get; set; }

        public ICollection<Survey> Surveys { get; set; }
    }
}
