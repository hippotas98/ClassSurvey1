using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class LecturerEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public int Role { get; set; }
        public string Phone { get; set; }
        public string LecturerCode { get; set; }

        public User User { get; set; }
        public ICollection<ClassEntity> Classes { get; set; }
        public LecturerEntity() : base() { }
        public LecturerEntity(Lecturer lecturer, params object[] args) : base(lecturer)
        {
            foreach(var arg in args)
            {
                if (arg is ICollection<Class> classes)
                {
                    this.Classes = classes.Select(c => new ClassEntity(c)).ToList();
                }
            }
        }

    }
    public partial class LecturerSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public string Phone { get; set; }
        public string LecturerCode { get; set; }
    }
}
