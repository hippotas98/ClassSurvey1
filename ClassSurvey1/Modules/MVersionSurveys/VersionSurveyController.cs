using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using ClassSurvey1.Modules.MClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MVersionSurveys
{
    [Route("api/VersionSurveys")]
    public class VersionSurveyController : CommonController
    {
        private IVersionSurveyService VersionSurveyService;

        public VersionSurveyController(IVersionSurveyService VersionSurveyService)
        {
            this.VersionSurveyService = VersionSurveyService;
        }
        
        
        [HttpGet("Count")]
        public int Count(VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            return VersionSurveyService.Count(UserEntity, VersionSurveySearchEntity);
        }
        [HttpGet("List")]
        public List<VersionSurveyEntity> List(VersionSurveySearchEntity VersionSurveySearchEntity)
        {
            return VersionSurveyService.List(UserEntity, VersionSurveySearchEntity);
        }
        [HttpPut("{VersionSurveyId}")]
        public VersionSurveyEntity Update([FromBody] VersionSurveyEntity VersionSurveyEntity, [FromRoute]Guid VersionSurveyId)
        {
            return VersionSurveyService.Update(UserEntity, VersionSurveyId, VersionSurveyEntity);
        }
        [HttpGet("{VersionSurveyId}")]
        public VersionSurveyEntity Get([FromRoute]Guid VersionSurveyId)
        {
            return VersionSurveyService.Get(UserEntity, VersionSurveyId);
        }
        [HttpDelete("{VersionSurveyId}")]
        public bool Delete([FromRoute]Guid VersionSurveyId)
        {
            return VersionSurveyService.Delete(UserEntity, VersionSurveyId);
        }

        [HttpPost]
        public VersionSurveyEntity Create([FromBody]VersionSurveyEntity VersionSurveyEntity)
        {
            return VersionSurveyService.Create(UserEntity, VersionSurveyEntity);
        }
        
    }
}