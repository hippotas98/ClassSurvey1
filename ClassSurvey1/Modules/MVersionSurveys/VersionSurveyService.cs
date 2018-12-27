using System;
using System.Collections.Generic;
using System.Linq;
using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ClassSurvey1.Modules.MVersionSurveys
{
    public interface IVersionSurveyService : ITransientService
    {
        int Count(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity);
        List<VersionSurveyEntity> List(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity);
        VersionSurveyEntity Get(UserEntity userEntity, Guid VersionSurveyId);

        VersionSurveyEntity Update(UserEntity userEntity, Guid VersionSurveyId,
            VersionSurveyEntity VersionSurveyEntity);

        bool Delete(UserEntity userEntity, Guid VersionSurveyId);
        VersionSurveyEntity Create(UserEntity userEntity, VersionSurveyEntity versionSurveyEntity);
    }

    public class VersionSurveyService : CommonService, IVersionSurveyService
    {
        public int Count(UserEntity userEntity, VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity == null) VersionSurveySearchEntity = new VersionSurveySearchEntity();
            IQueryable<VersionSurvey> VersionSurveys = context.VersionSurveys;
            VersionSurveys = Apply(VersionSurveys, VersionSurveySearchEntity);
            return VersionSurveys.Count();
        }

        public List<VersionSurveyEntity> List(UserEntity userEntity,
            VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity == null) VersionSurveySearchEntity = new VersionSurveySearchEntity();
            IQueryable<VersionSurvey> VersionSurveys = context.VersionSurveys;
            VersionSurveys = Apply(VersionSurveys, VersionSurveySearchEntity);
            //VersionSurveys = VersionSurveySearchEntity.SkipAndTake(VersionSurveys);
            return VersionSurveys.Select(l => new VersionSurveyEntity(l)).ToList();
        }

        public VersionSurveyEntity Get(UserEntity userEntity, Guid VersionSurveyId)
        {
            VersionSurvey
                VersionSurvey =
                    context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId); ///add include later
            if (VersionSurvey == null) throw new NotFoundException("VersionSurvey Not Found");
            return new VersionSurveyEntity(VersionSurvey);
        }

        public VersionSurveyEntity Update(UserEntity userEntity, Guid VersionSurveyId,
            VersionSurveyEntity VersionSurveyEntity)
        {
            if (VersionSurveyEntity.Content.Equals(String.Empty)
                || VersionSurveyEntity.Content == null
                || VersionSurveyEntity.Version == 0)
                throw new BadRequestException("Field khong duoc trong");
            if (!VersionSurveyValidator(VersionSurveyId))
            {
                throw new BadRequestException("Cannot Update Version Survey Content");
            }

            VersionSurvey
                VersionSurvey = context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId); //add include later
            if (VersionSurvey == null) throw new NotFoundException("VersionSurvey Not Found");
            VersionSurvey updateVersionSurvey = new VersionSurvey(VersionSurveyEntity);
            DateTime? createdDate = VersionSurvey.CreatedDate;
            updateVersionSurvey.CopyTo(VersionSurvey);
            VersionSurvey.ModifiedDate = DateTime.Now;
            VersionSurvey.CreatedDate = createdDate;
            context.SaveChanges();
            return new VersionSurveyEntity(VersionSurvey);
        }

        public VersionSurveyEntity Create(UserEntity userEntity, VersionSurveyEntity versionSurveyEntity)
        {
            if (versionSurveyEntity.Content.Equals(String.Empty)
                || versionSurveyEntity.Content == null
                || versionSurveyEntity.Version == 0)
                throw new BadRequestException("Field khong duoc trong");
            VersionSurvey versionSurvey = new VersionSurvey(versionSurveyEntity);
            versionSurvey.Id = Guid.NewGuid();
            versionSurvey.CreatedDate = DateTime.Now;
            context.VersionSurveys.Add(versionSurvey);
            context.SaveChanges();
            return new VersionSurveyEntity(versionSurvey);
        }

        public bool Delete(UserEntity userEntity, Guid VersionSurveyId)
        {
            try
            {
                var CurrentVersionSurvey = context.VersionSurveys.FirstOrDefault(c => c.Id == VersionSurveyId);
                if (CurrentVersionSurvey == null) return false;
                if (!VersionSurveyValidator(VersionSurveyId)) return false;
                context.VersionSurveys.Remove(CurrentVersionSurvey);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }   
        }

        private IQueryable<VersionSurvey> Apply(IQueryable<VersionSurvey> VersionSurveys,
            VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            if (VersionSurveySearchEntity.Version.HasValue)
            {
                VersionSurveys = VersionSurveys.Where(vs => vs.Version.Equals(VersionSurveySearchEntity.Version.Value));
            }

            if (VersionSurveySearchEntity.Year != null)
            {
                VersionSurveys = VersionSurveys.Where(vs =>
                    vs.CreatedDate.Value.Year.ToString().Equals(VersionSurveySearchEntity.Year));
            }

            return VersionSurveys;
        }

        private bool VersionSurveyValidator(Guid VersionId)
        {
            List<Class> classes = context.Classes.Where(c => c.VersionSurveyId == VersionId).ToList();
            foreach (var Class in classes)
            {
                try
                {
                    if (Class.OpenedDate != null && DateTime.Now >= Class.OpenedDate.Value)
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }
    }
}