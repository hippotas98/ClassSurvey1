using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using ClassSurvey1.Modules.MClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MSurveys
{
    [Route("api/Surveys")]
    public class SurveyController : CommonController
    {
        private ISurveyService SurveyService;

        public SurveyController(ISurveyService SurveyService)
        {
            this.SurveyService = SurveyService;
        }


        [HttpGet("Count")]
        public int Count([FromBody]SurveySearchEntity SurveySearchEntity)
        {
            return SurveyService.Count(UserEntity, SurveySearchEntity);
        }
        [HttpGet("List")]
        public List<SurveyEntity> List([FromBody]SurveySearchEntity SurveySearchEntity)
        {
            return SurveyService.List(UserEntity, SurveySearchEntity);
        }
        [HttpPut("{SurveyId}")]
        public SurveyEntity Update([FromBody] SurveyEntity SurveyEntity, [FromRoute]Guid SurveyId)
        {
            return SurveyService.Update(UserEntity, SurveyId, SurveyEntity);
        }
        [HttpGet("{SurveyId}")]
        public SurveyEntity Get([FromRoute]Guid SurveyId)
        {
            return SurveyService.Get(UserEntity, SurveyId);
        }
        [HttpDelete("{SurveyId}")]
        public bool Delete([FromRoute]Guid SurveyId)
        {
            return SurveyService.Delete(UserEntity, SurveyId);
        }

        [HttpPost]
        public SurveyEntity Create([FromBody]SurveyEntity SurveyEntity)
        {
            return SurveyService.Create(UserEntity, SurveyEntity);
        }

    }
}