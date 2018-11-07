using ClassSurvey1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public partial class Survey : Base
    {
        public Survey() : base() { }
        public Survey(SurveyEntity surveyEntity): base(surveyEntity)
        {
            if(surveyEntity.StudentClass != null)
            {
                this.StudentClass = new StudentClass(surveyEntity.StudentClass);
            }
            if(surveyEntity.VersionSurvey != null)
            {
                this.VersionSurvey = new VersionSurvey(surveyEntity.VersionSurvey);
            }
        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is Survey Survey)
            {
                return Id.Equals(Survey.Id);
            }

            return false;
        }
    }
}
