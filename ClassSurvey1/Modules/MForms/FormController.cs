﻿using System;
using System.Collections.Generic;
using ClassSurvey1.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MForms
{
    [Route("api/Forms")]
    public class FormController : CommonController
    {
        private IFormService FormService;

        public FormController(IFormService FormService)
        {
            this.FormService = FormService;
        }


        [HttpGet("Count")]
        public int Count(FormSearchEntity FormSearchEntity)
        {
            return FormService.Count(UserEntity, FormSearchEntity);
        }
        [HttpGet("List")]
        public List<FormEntity> List(FormSearchEntity FormSearchEntity)
        {
            return FormService.List(UserEntity, FormSearchEntity);
        }
        [HttpPut("{FormId}")]
        public FormEntity Update([FromBody] FormEntity FormEntity, [FromRoute]Guid FormId)
        {
            return FormService.Update(UserEntity, FormId, FormEntity);
        }
        [HttpGet("{FormId}")]
        public FormEntity Get([FromRoute]Guid FormId)
        {
            return FormService.Get(UserEntity, FormId);
        }
        [HttpDelete("{FormId}")]
        public bool Delete([FromRoute]Guid FormId)
        {
            return FormService.Delete(UserEntity, FormId);
        }

        [HttpPost]
        public FormEntity Create([FromBody]FormEntity FormEntity)
        {
            return FormService.Create(UserEntity, FormEntity);
        }

    }
}