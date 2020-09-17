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
    }
}
