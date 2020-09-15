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
        private readonly SolarDAL _solarDAL;

        public TeacherController(SolarSystemDbContext context, SolarDAL solarDAL)
        {
            _context = context;
            _solarDAL = solarDAL; 
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

        public IActionResult DisplayQB()
        {
            _context.Questions.ToList();
            var quizes = _context.Quizes.ToList();
            var qb = _context.Questionsbank.ToList();

            ViewBag.Quizes = quizes;

            return View(qb);
        }

        //ADD QUESTION TO Questions table and ASSIGN QuizId & QuestionID to Questionsbank
        [HttpGet]
        public IActionResult AddQuestionToQB()
        {
            var quizes = _context.Quizes.ToList();
            ViewBag.questions = _context.Questions.ToList();
            return View(quizes);
        }

        [HttpPost]
        public IActionResult AddQuestionToQB(Questions newQuestion, int quizId)
        {
            if (ModelState.IsValid)
            {
                //Add new question to DB
                _context.Questions.Add(newQuestion);
                _context.SaveChanges();

                //create relation between quizes and questions
                var newQB = new Questionsbank();
                newQB.QuestionId = newQuestion.Id;
                newQB.QuizId = quizId;

                //Add new relationship to DB
                _context.Questionsbank.Add(newQB);
                _context.SaveChanges();
            }

            return RedirectToAction("AddQuestionToQB");
        }

        public async Task<IActionResult> RemoveQuestion(int id)
        {
            //Remove constraint from question bank 1st
            List<Questionsbank> lqb = _context.Questionsbank.ToList();
            Questionsbank qbRemove = new Questionsbank();

            foreach (Questionsbank qb in lqb)
            {
                if (qb.QuestionId == id)
                {
                    qbRemove = qb;
                }
            }
            
            if (qbRemove != null)
            {
                _context.Questionsbank.Remove(qbRemove);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }

            //remove question from question table 2nd
            //Questions currentQuestion = _context.Questions.Find(id);
            Questions currentQuestion = await _context.Questions.FindAsync(id);

            if (currentQuestion != null)
            {
                _context.Questions.Remove(currentQuestion);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddQuestionToQB");
        }

        [HttpGet]
        public IActionResult CreateClassroom()
        {
            var teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        [HttpPost]
        public IActionResult CreateClassroom(Classrooms newclassroom)
        {
            
            if(ModelState.IsValid)
            {
                _context.Classrooms.Add(newclassroom);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
