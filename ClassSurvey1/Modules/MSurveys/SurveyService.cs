using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules.MSurveys
{
    public interface ISurveyService : ITransientService
    {
        int Count(UserEntity userEntity, SurveySearchEntity SurveySearchEntity);
        List<SurveyEntity> List(UserEntity userEntity, SurveySearchEntity SurveySearchEntity);
        SurveyEntity Get(UserEntity userEntity, Guid SurveyId);
        SurveyEntity Update(UserEntity userEntity, Guid SurveyId, SurveyEntity SurveyEntity);
        bool Delete(UserEntity userEntity, Guid SurveyId);
        SurveyEntity Create(UserEntity userEntity, SurveyEntity SurveyEntity);
        
    }

    public class SurveyService :CommonService, ISurveyService
    {
        public int Count(UserEntity userEntity, SurveySearchEntity SurveySearchEntity)
        {
            if (SurveySearchEntity == null) SurveySearchEntity = new SurveySearchEntity();
            IQueryable<Survey> Surveys = context.Surveys;
            Apply(Surveys, SurveySearchEntity);
            return Surveys.Count();
        }
        
        public List<SurveyEntity> List(UserEntity userEntity, SurveySearchEntity SurveySearchEntity)
        {
            if (SurveySearchEntity == null) SurveySearchEntity = new SurveySearchEntity();
            IQueryable<Survey> Surveys = context.Surveys;
            Apply(Surveys, SurveySearchEntity);
            Surveys = SurveySearchEntity.SkipAndTake(Surveys);
            return Surveys.Select(l => new SurveyEntity(l)).ToList();
        }

        public SurveyEntity Get(UserEntity userEntity, Guid SurveyId)
        {
            Survey Survey = context.Surveys.FirstOrDefault(c => c.Id == SurveyId); ///add include later
            if (Survey == null) throw new NotFoundException("Survey Not Found");
            return new SurveyEntity(Survey);
        }
        public SurveyEntity Update(UserEntity userEntity, Guid SurveyId, SurveyEntity SurveyEntity)
        {
            if (SurveyValidator(SurveyEntity))
            {
                Survey Survey = context.Surveys.FirstOrDefault(c => c.Id == SurveyId); //add include later
                if (Survey == null) throw new NotFoundException("Survey Not Found");
                Survey updateSurvey = new Survey(SurveyEntity);
                updateSurvey.CopyTo(Survey);
                context.SaveChanges();
                return new SurveyEntity(Survey);
            }
            else throw new BadRequestException("Cannot update");
            
        }

        public SurveyEntity Create(UserEntity userEntity, SurveyEntity SurveyEntity)
        {
            if (SurveyValidator(SurveyEntity))
            {
                Survey Survey = new Survey(SurveyEntity);
                Survey.Id = Guid.NewGuid();
                context.Surveys.Add(Survey);
                context.SaveChanges();
                return new SurveyEntity(Survey);
            }
            else throw new BadRequestException("Cannot update");

            return null;
        }

        
        public bool Delete(UserEntity userEntity, Guid SurveyId)
        {
            var CurrentSurvey = context.Surveys.FirstOrDefault(c => c.Id == SurveyId);
            if (CurrentSurvey == null) return false;
            SurveyEntity surveyEntity = new SurveyEntity(CurrentSurvey);
            if (SurveyValidator(surveyEntity))
            {
                
                context.Surveys.Remove(CurrentSurvey);
                context.SaveChanges();
                return true;
            }

            return false;
        }
        private void Apply(IQueryable<Survey> Surveys, SurveySearchEntity SurveySearchEntity)
        {
            if (SurveySearchEntity.VersionSurveyId != null)
            {
                Surveys = Surveys.Where(vs => vs.VersionSurveyId.Equals(SurveySearchEntity.VersionSurveyId));
            }
            return;
        }
        private bool SurveyValidator(SurveyEntity SurveyEntity)
        {

            StudentClass studentClass = context.StudentClasses.Where(sc => sc.Id == SurveyEntity.StudentClassId).FirstOrDefault();
            if (studentClass == null) return false;
            Class Class = context.Classes.Where(c => c.Id == studentClass.ClassId).FirstOrDefault();
            if (Class == null) return false;
            if (DateTime.Now > Class.OpenedDate && DateTime.Now < Class.ClosedDate)
                return true;
            return false;
        }
    }
}
