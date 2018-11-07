using ClassSurvey1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class VersionSurveyEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public int? Version { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> ContentCategory { get; set; }
        public ICollection<SurveyEntity> Surveys { get; set; }
        public VersionSurveyEntity() : base()
        {

        }
        public VersionSurveyEntity(VersionSurvey versionSurvey, params object[] args) : base(versionSurvey)
        {
            this.ContentCategory = JsonConvert.DeserializeObject<VersionSurveyContent>(this.Content).Values;
            foreach (var arg in args)
            {
                if(arg is ICollection<Survey> surveys)
                {
                    this.Surveys = surveys.Select(s => new SurveyEntity(s)).ToList();
                }
            }
        }
    }
    public partial class VersionSurveySearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public int? Version { get; set; }
    }
    class VersionSurveyContent
    {
        public Dictionary<string, string> Values { get; set; }
    }
}
