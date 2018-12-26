using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using System;
using System.Linq;

namespace ClassSurvey1.Modules.MSurveys
{
    public interface ISurveyService : ITransientService
    {
        void CreateOrUpdate(UserEntity userEntity, SurveyEntity SurveyEntity);
        
    }
    public class SurveyService : CommonService, ISurveyService
    {
        public void CreateOrUpdate(UserEntity userEntity, SurveyEntity SurveyEntity)
        {
            if(!SurveyValidator(SurveyEntity)) 
                throw new BadRequestException("Cannot create or update surveys");
            foreach(var Id in SurveyEntity.ClassGuids)
            {
                Class Class = context.Classes.FirstOrDefault(c => c.Id == Id);
                if(Class == null) 
                    throw new BadRequestException("Class Not Found");
                
                if(SurveyEntity.OpenedDate != DateTime.MinValue)
                    Class.OpenedDate = DateTime.Parse(SurveyEntity.OpenedDate.ToString());
                if(SurveyEntity.ClosedDate != DateTime.MinValue)
                    Class.ClosedDate = DateTime.Parse(SurveyEntity.ClosedDate.ToString());
                Class.Semester = GetSemester(Class.OpenedDate.Value);
                //var StudentClasses = context.StudentClasses.Where(sc => sc.ClassId == Id).ToList();
                if (context.VersionSurveys.FirstOrDefault(vs => vs.Id == SurveyEntity.VersionSurveyId) == null)
                    throw new BadRequestException("Version Survey Not Found");
                Class.VersionSurveyId = SurveyEntity.VersionSurveyId;
                
            }
            context.SaveChanges();
        }

        private string GetSemester(DateTime openedDate)
        {
            int year = openedDate.Year;
            int month = openedDate.Month;
            if ((month >= 1 && month <= 5) || month == 12)
            {
                if(month == 12) return "HK1 " + year + "-" + (year+1);
                return "HK1 " + (year-1) + "-" + (year);
            }
            else if (month >= 6 && month <= 8)
            {
                return "HK2 " + (year-1) + "-" + (year);
            }
            else
            {
                return "Ki he " + (year-1) + "-" + (year);
            }
        }

        private bool SurveyValidator(SurveyEntity surveyEntity)
        {
            if (surveyEntity.OpenedDate == DateTime.MinValue) return false;
            if (surveyEntity.ClosedDate == DateTime.MinValue) return false;
            if (surveyEntity.OpenedDate > surveyEntity.ClosedDate) return false;
            if (surveyEntity.VersionSurveyId == Guid.Empty) return false;          
            if (surveyEntity.ClassGuids.Count == 0) return false;
            return true;
        }
    }
}
