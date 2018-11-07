using ClassSurvey1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public partial class Class : Base
    {
       
        public Class(ClassEntity classEntity) : base(classEntity)
        {
            if(classEntity.Lecture != null)
            {
                this.Lecture = new Lecturer(classEntity.Lecture);
            }
            if(classEntity.StudentClasses != null)
            {
                this.StudentClasses = new HashSet<StudentClass>();
                foreach(var studentClassEntity in classEntity.StudentClasses)
                {
                    studentClassEntity.ClassId = this.Id;
                    this.StudentClasses.Add(new StudentClass(studentClassEntity));
                }
            }
        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is Class Class)
            {
                return Id.Equals(Class.Id) && ClassCode.Equals(Class.ClassCode);
            }

            return false;
        }
    }
}
