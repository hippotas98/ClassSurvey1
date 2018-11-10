﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public class PollEntity : BaseEntity 
    {
        public DateTime openedDate { get; set; }
        public DateTime closedDate { get; set; }
        public List<Guid> classGuids { get; set; }
    }
}
