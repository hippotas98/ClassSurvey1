using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using ClassSurvey1.Modules.MClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            foreach(var Id in SurveyEntity.classGuids)
            {
                Class Class = context.Classes.FirstOrDefault(c => c.Id == Id);
                Class.OpenedDate = SurveyEntity.openedDate;
                Class.ClosedDate = SurveyEntity.closedDate;
                //var StudentClasses = context.StudentClasses.Where(sc => sc.ClassId == Id).ToList();
                Class.VersionSurveyId = SurveyEntity.versionSurveyId;
                
            }
            context.SaveChanges();
        }
        
    }
}
