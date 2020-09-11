using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Questions
    {
        public Questions()
        {
            Questionsbank = new HashSet<Questionsbank>();
        }

        public int Id { get; set; }
        public int? QuizId { get; set; }
        public string Question { get; set; }

        public virtual Quizes Quiz { get; set; }
        public virtual ICollection<Questionsbank> Questionsbank { get; set; }
    }
}
