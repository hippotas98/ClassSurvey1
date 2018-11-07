using ClassSurvey1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public partial class StudentClass : Base
    {
        
        public StudentClass(StudentClassEntity studentClassEntity): base(studentClassEntity)
        {
            if(studentClassEntity.Class != null)
            {
                this.Class = new Class(studentClassEntity.Class);
            }
            if(studentClassEntity.Student != null)
            {
                this.Student = new Student(studentClassEntity.Student);
            }
            if(studentClassEntity.Surveys != null)
            {
                this.Surveys = new HashSet<Survey>();
                foreach(var survey in studentClassEntity.Surveys)
                {
                    survey.StudentClassId = this.Id;
                    Surveys.Add(new Survey(survey));
                }
            }
        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is StudentClass StudentClass)
            {
                return Id.Equals(StudentClass.Id);
            }

            return false;
        }
    }
}
