using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Classrooms
    {
        public Classrooms()
        {
            Students = new HashSet<Students>();
        }

        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public string ClassName { get; set; }
        public double? ClassAvg { get; set; }

        public virtual Teachers Teacher { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
