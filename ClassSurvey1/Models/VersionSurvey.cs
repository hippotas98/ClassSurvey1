﻿using System;
using System.Collections.Generic;

namespace ClassSurvey1.Models
{
    public partial class VersionSurvey
    {
        public VersionSurvey()
        {
            Classes = new HashSet<Class>();
        }

        public Guid Id { get; set; }
        public int? Version { get; set; }
        public string Content { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}
