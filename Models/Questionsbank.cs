using System;
using System.Collections.Generic;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public partial class Questionsbank
    {
        public int Id { get; set; }
        public int? QuizId { get; set; }
        public int? QuestionId { get; set; }

        public virtual Questions Question { get; set; }
        public virtual Quizes Quiz { get; set; }
    }
}
