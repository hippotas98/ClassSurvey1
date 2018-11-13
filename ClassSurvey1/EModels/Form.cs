﻿using ClassSurvey1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassForm1.Entities;

namespace ClassSurvey1.Models
{
    public partial class Form : Base
    {
        public Form() : base() { }
        public Form(FormEntity FormEntity): base(FormEntity)
        {
            if(FormEntity.StudentClass != null)
            {
                this.StudentClass = new StudentClass(FormEntity.StudentClass);
            }
            
        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is Form Form)
            {
                return Id.Equals(Form.Id);
            }

            return false;
        }
    }
}
