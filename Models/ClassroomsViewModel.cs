using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_SolarSystemEducationApp.Models
{
    public class ClassroomsViewModel
    {
        public List<Classrooms> Classroom { get; set; }
        public double? QuizAverage1 { get; set; }
        public double? QuizAverage2 { get; set; }
        public double? QuizAverage3 { get; set; }
        public List<Grades> AverageQG { get; set; }

    } 
}
