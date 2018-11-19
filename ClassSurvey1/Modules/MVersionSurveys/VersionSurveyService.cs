using System;
using System.Collections.Generic;
using System.Linq;
using ClassSurvey1.Entities;
using ClassSurvey1.Models;

namespace ClassSurvey1.Modules.MVersionSurveys
{
    public interface IVersionSurveyService : ITransientService
    {
        int Count(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity);
        List<VersionSurveyEntity> List(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity);
        VersionSurveyEntity Get(UserEntity userEntity, Guid VersionSurveyId);
        VersionSurveyEntity Update(UserEntity userEntity, Guid VersionSurveyId, VersionSurveyEntity VersionSurveyEntity);
        bool Delete(UserEntity userEntity, Guid VersionSurveyId);
        VersionSurveyEntity Create(UserEntity userEntity, VersionSurveyEntity versionSurveyEntity);
    }
    public class VersionSurveyService : CommonService, IVersionSurveyService
    {
        public int Count(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity == null) VersionSurveySearchEntity = new VersionSurveySearchEntity();
            IQueryable<VersionSurvey> VersionSurveys = context.VersionSurveys;
            Apply(VersionSurveys, VersionSurveySearchEntity);
            return VersionSurveys.Count();
        }

        public List<VersionSurveyEntity> List(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity == null) VersionSurveySearchEntity = new VersionSurveySearchEntity();
            IQueryable<VersionSurvey> VersionSurveys = context.VersionSurveys;
            Apply(VersionSurveys, VersionSurveySearchEntity);
            //VersionSurveys = VersionSurveySearchEntity.SkipAndTake(VersionSurveys);
            return VersionSurveys.Select(l => new VersionSurveyEntity(l)).ToList();
        }

        public VersionSurveyEntity Get(UserEntity userEntity, Guid VersionSurveyId)
        {
            VersionSurvey VersionSurvey = context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId); ///add include later
            if (VersionSurvey == null) throw new NotFoundException("VersionSurvey Not Found");
            return new VersionSurveyEntity(VersionSurvey);
        }
        public VersionSurveyEntity Update(UserEntity userEntity, Guid VersionSurveyId, VersionSurveyEntity VersionSurveyEntity)
        {
            VersionSurvey VersionSurvey = context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId); //add include later
            if (VersionSurvey == null) throw new NotFoundException("VersionSurvey Not Found");
            VersionSurvey updateVersionSurvey = new VersionSurvey(VersionSurveyEntity);
            updateVersionSurvey.CopyTo(VersionSurvey);
            context.SaveChanges();
            return VersionSurveyEntity;
        }

        public VersionSurveyEntity Create(UserEntity userEntity, VersionSurveyEntity versionSurveyEntity)
        {
            VersionSurvey versionSurvey = new VersionSurvey(versionSurveyEntity);
            versionSurvey.Id = Guid.NewGuid();
            context.VersionSurveys.Add(versionSurvey);
            context.SaveChanges();
            return new VersionSurveyEntity(versionSurvey);
        }
        public bool Delete(UserEntity userEntity, Guid VersionSurveyId)
        {
            var CurrentVersionSurvey = context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId);
            if (CurrentVersionSurvey == null) return false;
            context.VersionSurveys.Remove(CurrentVersionSurvey);
            context.SaveChanges();
            return true;
        }
        private void Apply(IQueryable<VersionSurvey> VersionSurveys, VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity.Version != null)
            {
                VersionSurveys = VersionSurveys.Where(vs => vs.Version.Equals(VersionSurveySearchEntity.Version));
            }
            return;
        }
    }
}