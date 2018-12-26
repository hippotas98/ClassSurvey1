﻿using ClassSurvey1.Entities;
using System;

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
        public override bool Equals(Object other)
        {
            if (other == null) return false;
            if (other is Form Form)
            {
                return Id.Equals(Form.Id);
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
