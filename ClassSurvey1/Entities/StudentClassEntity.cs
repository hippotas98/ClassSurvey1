using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class StudentClassEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }

        public ClassEntity Class { get; set; }
        public StudentEntity Student { get; set; }
        public ICollection<SurveyEntity> Surveys { get; set; }
        public StudentClassEntity() : base() { }
        public StudentClassEntity(StudentClass studentClass, params object[] args) : base(studentClass)
        {
            foreach (var arg in args)
            {
                if (arg is Student student)
                {
                    this.Student = new StudentEntity(student);
                }
                else if (arg is Class Class)
                {
                    this.Class = new ClassEntity(Class);
                }
                else if (arg is ICollection<Survey> Surveys)
                {
                    this.Surveys = Surveys.Select(s => new SurveyEntity(s)).ToList();
                }
            }
        }
    }
    public partial class StudentClassSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
    }
}
