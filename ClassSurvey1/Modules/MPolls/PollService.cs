using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using ClassSurvey1.Modules.MClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules.MPolls
{
    public interface IPollService : ITransientService
    {
        void CreateOrUpdatePoll(UserEntity userEntity, PollEntity pollEntity);
        
    }
    public class PollService : CommonService, IPollService
    {
        public void CreateOrUpdatePoll(UserEntity userEntity, PollEntity pollEntity)
        {
            foreach(var Id in pollEntity.classGuids)
            {
                Class Class = context.Classes.FirstOrDefault(c => c.Id == Id);
                Class.OpenedDate = pollEntity.openedDate;
                Class.ClosedDate = pollEntity.closedDate;
                
            }
            context.SaveChanges();
        }
        
    }
}
