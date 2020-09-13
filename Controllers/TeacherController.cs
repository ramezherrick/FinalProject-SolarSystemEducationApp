using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_SolarSystemEducationApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly SolarSystemDbContext _context;

        public TeacherController(SolarSystemDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Add questions to the database in Questionsbank table
        [HttpGet]
        public  IActionResult AddQuestionsToDb()
        {
            var quizes = _context.Quizes.ToList();
            ViewBag.questions = _context.Questions.ToList();
            return View(quizes);
        }
        [HttpPost]
        public IActionResult AddQuestionsToDb(Questions newQuestion)
        {
            if(ModelState.IsValid)
            {
                _context.Questions.Add(newQuestion);
                _context.SaveChanges();
            }
            return RedirectToAction("AddQuestionsToDb");
        }
        //Add quizes to the database in Quizes table
        [HttpGet]
        public IActionResult AddQuizesToDb()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddQuizesToDb(Quizes newQuiz)
        {
            if (ModelState.IsValid)
            {
                _context.Quizes.Add(newQuiz);
                _context.SaveChanges();
            }
            return View();
        }
        //Add "Test" to the database in Questionsbank table
        [HttpGet]
        public IActionResult AddQuestionsbankToDb()
        {

            var questionsbank = _context.Questionsbank;
            return View(questionsbank);
        }
    }
}
