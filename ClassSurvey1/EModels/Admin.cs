using ClassSurvey1.Entities;
using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Models
{
    public partial class Admin : Base
    {
        public Admin() : base() { }
        public Admin(AdminEntity adminEntity) : base(adminEntity)
        {

        }
        public override bool Equals(Base other)
        {
            if (other == null) return false;
            if (other is Admin admin)
            {
                return Id.Equals(admin.Id);
            }

            return false;
        }
    }
}
