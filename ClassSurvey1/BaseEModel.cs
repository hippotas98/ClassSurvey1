using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public abstract class Base
    {
        public Base() { }
        public Base(object obj)
        {
            Common<object>.Copy(obj, this);
        }
        public abstract bool Equals(Base other);
    }
}
