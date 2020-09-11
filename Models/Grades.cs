using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Grades
    {
        public int Id { get; set; }
        public double? Grade { get; set; }
        public int? StudentId { get; set; }
        public int? QuizId { get; set; }

        public virtual Quizes Quiz { get; set; }
        public virtual Students Student { get; set; }
    }
}
