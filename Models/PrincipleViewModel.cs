using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public class PrincipleViewModel
    {
        public List<Students> studentsWithDifficulty { get; set; }
        public List<Students> studentsOnDeansList { get; set; }

   

        public List<Classrooms> classrooms { get; set; }

        public List<Students> students { get; set; }
        public List<Teachers> teachers { get; set; }

        public List<AspNetUsers> users { get; set; }

        public List<AspNetUserRoles> userRoles { get; set; }

        public List<Grades> grades { get; set; }

        public List<AspNetRoles> roles { get; set; }
    }
}
