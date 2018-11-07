using ClassSurvey1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public partial class VersionSurvey : Base
    {
        //public VersionSurvey() : base() { }
        public VersionSurvey(VersionSurveyEntity versionSurveyEntity) : base(versionSurveyEntity)
        {
            if(versionSurveyEntity.Surveys != null)
            {
                this.Surveys = new HashSet<Survey>();
                foreach(var surveyEntity in versionSurveyEntity.Surveys)
                {
                    surveyEntity.VersionSurveyId = this.Id;
                    this.Surveys.Add(new Survey(surveyEntity));
                }
            }
        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is VersionSurvey VersionSurvey)
            {
                return Id.Equals(VersionSurvey.Id);
            }

            return false;
        }
    }
}
