using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Quizes
    {
        public Quizes()
        {
            Grades = new HashSet<Grades>();
            Questions = new HashSet<Questions>();
            Questionsbank = new HashSet<Questionsbank>();
        }

        public int Id { get; set; }
        public string QuizType { get; set; }

        public virtual ICollection<Grades> Grades { get; set; }
        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<Questionsbank> Questionsbank { get; set; }
    }
}
