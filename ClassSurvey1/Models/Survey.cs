using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class Survey
    {
        public Guid Id { get; set; }
        public Guid StudentClassId { get; set; }
        public string Content { get; set; }
        public Guid VersionSurveyId { get; set; }

        public StudentClass StudentClass { get; set; }
        public VersionSurvey VersionSurvey { get; set; }
    }
}
