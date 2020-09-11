using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Teachers
    {
        public Teachers()
        {
            Classrooms = new HashSet<Classrooms>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<Classrooms> Classrooms { get; set; }
    }
}
