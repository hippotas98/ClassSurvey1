using ClassSurvey1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Entities
{
    public partial class AdminEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public int Role { get; set; }
        public string Phone { get; set; }

        public User User { get; set; }
        public AdminEntity() : base()
        {
        }
        public AdminEntity(Admin admin) : base(admin)
        {
            
        }
    }
    public partial class AdminSearchEntity : FilterEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Vnumail { get; set; }
        public string Phone { get; set; }
    }
}
