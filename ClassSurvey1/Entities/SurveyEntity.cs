﻿using ClassSurvey1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class SurveyEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid StudentClassId { get; set; }
        public string Content { get; set; }
        public Guid VersionSurveyId { get; set; }
        public Dictionary<string, int> ContentValues { get; set; }
        public StudentClassEntity StudentClass { get; set; }
        public VersionSurveyEntity VersionSurvey { get; set; }
        public SurveyEntity() : base()
        {

        }
        public SurveyEntity(Survey survey, params object[] args) : base(survey)
        {
            if(this.Content != null || this.Content != "")
                this.ContentValues = JsonConvert.DeserializeObject<Dictionary<string, int> >(this.Content);
            foreach(var arg in args)
            {
                if(arg is StudentClass studentClass)
                {
                    this.StudentClass = new StudentClassEntity(studentClass);
                }
                else if(arg is VersionSurvey versionSurvey)
                {
                    this.VersionSurvey = new VersionSurveyEntity(versionSurvey);
                }
            }
        }
    }
    
    public partial class SurveySearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public Guid StudentClassId { get; set; }
        public Guid VersionSurveyId { get; set; }
    }
}
