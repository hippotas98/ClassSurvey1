using ClassSurvey1.Entities;
using System;


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
        public override bool Equals(Object other)
        {
            if (other == null) return false;
            if (other is Admin admin)
            {
                return Id.Equals(admin.Id);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
