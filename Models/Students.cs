using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Students
    {
        public Students()
        {
            Grades = new HashSet<Grades>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double? AverageGrade { get; set; }
        public string UserId { get; set; }
        public int? ClassroomId { get; set; }

        public virtual Classrooms Classroom { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<Grades> Grades { get; set; }
    }
}
