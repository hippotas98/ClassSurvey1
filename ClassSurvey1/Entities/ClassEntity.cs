using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class ClassEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        public int StudentNumber { get; set; }
        public Guid LecturerId { get; set; }
        public string Subject { get; set; }
        public decimal? M { get; set; }
        public decimal? M1 { get; set; }
        public decimal? M2 { get; set; }
        public decimal? Std { get; set; }
        public decimal? Std1 { get; set; }
        public decimal? Std2 { get; set; }

        public LecturerEntity Lecturer { get; set; }
        public ICollection<StudentClassEntity> StudentClasses { get; set; }
        public ClassEntity() : base() { }
        public ClassEntity(Class Class, params object[] args) : base(Class)
        {
            foreach (var arg in args)
            {
                if (arg is Lecturer lecturer)
                {
                    this.Lecturer = new LecturerEntity(lecturer);
                }
                else if (arg is ICollection<StudentClass> students)
                {
                    this.StudentClasses = students.Select(s => new StudentClassEntity(s)).ToList();
                }
            }
        }
    }
    public partial class ClassSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        public Guid LecturerId { get; set; }
        public string Subject { get; set; }
    }
}
