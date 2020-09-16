using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_SolarSystemEducationApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly SolarSystemDbContext _context;
        private readonly SolarDAL _solarDal;
        public StudentController(SolarSystemDbContext context)
        {
            _context = context;
            _solarDal = new SolarDAL();
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> TakePlanetsQuiz()
        {
            List<Body> planetsList = await CreatePlanetsListFromAPIAsync();
            List<Body> fourPlanetsList = new List<Body>();

            //Retrieve 4 random planets from the API
            Random planet = new Random();
            for (int i = 0; i < 4; i++)
            {
                int index = planet.Next(0, planetsList.Count);
                fourPlanetsList.Add(planetsList[index]);
                planetsList.RemoveAt(index);
            }

            //Select a random planet from the 4 to be the test subject and send it to the view
            int indexOfPlanetToBeTested = planet.Next(0, fourPlanetsList.Count);

            if(fourPlanetsList[indexOfPlanetToBeTested].discoveredBy.ToString().Length<1)
            {
                fourPlanetsList[indexOfPlanetToBeTested].discoveredBy = "Unknown";
            }
            if (fourPlanetsList[indexOfPlanetToBeTested].discoveryDate.ToString().Length < 1)
            {
                fourPlanetsList[indexOfPlanetToBeTested].discoveryDate = "Unknown";
            }

            ViewBag.indexOfPlanetToBeTested = indexOfPlanetToBeTested;

            //Get questionsbank from SQL and send it to the view
            _context.Questions.ToList();
            _context.Quizes.ToList();
            var questiosBankList = _context.Questionsbank.ToList();
            ViewBag.questiosBankList = questiosBankList;

            return View(fourPlanetsList);
        }

        [HttpPost]
        public async Task<IActionResult> TakePlanetsQuiz(List<string> answers, string englishName)
        {
            int numberOfCorrectAnswers = 0;
            int numberOfMoons = 0;
          
            List<Body> planetsList = await CreatePlanetsListFromAPIAsync();
            Body testedPlanet = new Body();
            
            //Retrieve the 4 planets in multipe choice
            for(int i =0;i<planetsList.Count;i++)
            {
                if(planetsList[i].englishName==englishName)
                {
                    testedPlanet = planetsList[i];
                }
            }

            //check mass answer
            string mass = testedPlanet.mass.massValue.ToString() + "^" + testedPlanet.mass.massExponent.ToString();
            
            if (answers[0] == mass)
            {
                numberOfCorrectAnswers += 1;
            }
            //check volume answer
            string planetVolume = testedPlanet.vol.volValue.ToString() + "^" + testedPlanet.vol.volExponent.ToString();
            if (answers[1] == planetVolume)
            {
                numberOfCorrectAnswers += 1;
            }
            //checking for number of moons it has
            if (testedPlanet.moons == null)
            {
                numberOfMoons = 0;
            }
            if (testedPlanet.moons != null)
            {
                numberOfMoons = testedPlanet.moons.Length;
            }

            if (answers[2] == numberOfMoons.ToString())
            {
                numberOfCorrectAnswers += 1;
            }
            ////check who discovered this planet
            if(testedPlanet.discoveredBy.Length<1)
            {
                testedPlanet.discoveredBy = "Unknown";
            }
            if (answers[3] == testedPlanet.discoveredBy)
            {
                numberOfCorrectAnswers += 1;
            }
            ////check when was the planet discovered
             if(testedPlanet.discoveryDate.Length<1)
            {
                testedPlanet.discoveryDate = "Unknown";
            }
            if (answers[4] == testedPlanet.discoveryDate)
            {
                numberOfCorrectAnswers += 1;
            }
            ////check gravity answer
            if (answers[5] == testedPlanet.gravity.ToString())
            {
                numberOfCorrectAnswers += 1;
            }
            double total = numberOfCorrectAnswers;
            double grade = (double)(total / 6) * 100;

            

            return RedirectToAction("ResultsPlanetsQuiz", new {g=grade,answered=answers, engname=englishName});
        }
        public async Task<IActionResult> ResultsPlanetsQuiz(double g, List<string> answered, string engname)
        {
            //find logged in userid 
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //create a list of students where student id matches logged in person id
            List<Students> students = _context.Students.Where(x => x.UserId == id).ToList();

            List<Questions> questions = _context.Questions.Where(x => x.QuizId == 2).ToList();

            List<Body> planetsList = await CreatePlanetsListFromAPIAsync();
            Body testedPlanet = new Body();

            //Retrieve the 4 planets in multipe choice
            for (int i = 0; i < planetsList.Count; i++)
            {
                if (planetsList[i].englishName == engname)
                {
                    testedPlanet = planetsList[i];
                }
            }

            int numberOfMoons = 0;

            if (testedPlanet.moons == null)
            {
                numberOfMoons = 0;
            }
            if (testedPlanet.moons != null)
            {
                numberOfMoons = testedPlanet.moons.Length;
            }
            if (testedPlanet.discoveredBy.Length < 1)
            {
                testedPlanet.discoveredBy = "Unknown";
            }
            if (testedPlanet.discoveryDate.Length < 1)
            {
                testedPlanet.discoveryDate = "Unknown";
            }

            List<string> correctAnswers = new List<string>() { testedPlanet.mass.massExponent.ToString() + "^" + testedPlanet.mass.massExponent.ToString(), testedPlanet.vol.volValue.ToString() + "^" + testedPlanet.vol.volExponent.ToString(), testedPlanet.moons.Count().ToString(), testedPlanet.discoveredBy.ToString(), testedPlanet.discoveryDate.ToString(), testedPlanet.gravity.ToString() };
            int studentId = students[0].Id;
            //planets quiz
            int quizId = 2;

            Grades newGrade = new Grades();

            newGrade.StudentId = studentId;
            newGrade.QuizId = quizId;
            newGrade.Grade = g;
            _context.Grades.Add(newGrade);
            _context.SaveChanges();

            ViewBag.studentname = students[0].FirstName.ToString() + students[0].LastName.ToString();
            ViewBag.grade = g;
            ViewBag.questions = questions;
            ViewBag.correctanswers = correctAnswers;
            ViewBag.englishname = engname;
            return View(answered);
        }
        public async Task<List<Body>> CreatePlanetsListFromAPIAsync()
        {
            //access API get a list of all bodies in the api
            List<Body> body = await _solarDal.GetBody();

            //create an empty list of bodies and populate it with planets
            List<Body> planetsList = new List<Body>();

            for (int i = 0; i < body.Count; i++)
            {
                if (body[i].isPlanet == true && body[i].mass != null && body[i].vol != null && body[i].gravity > 0.01)
                {
                    planetsList.Add(body[i]);
                }
            }
            return planetsList;
        }


        public async Task<IActionResult> DisplayBodies()
        {
            var allInfo = await _solarDal.GetBody();
            return View(allInfo);
        }

        public async Task<IActionResult> BodyDetails(string id)
        {
            List<Body> bodiesList = await _solarDal.GetBody();
            Body currentBody = new Body();
            foreach (Body b in bodiesList)
            {
                if (b.id == id)
                {
                    currentBody = b;
                }
            }

            return View(currentBody);

        }
        [HttpGet]
        public async Task<IActionResult> DisplayGeneralQuiz()
        {
            List<Body> search = await _solarDal.GetBody();
            Random star = new Random();
            int randomStarIndex = star.Next(1, search.Count);

            ViewBag.body = search[randomStarIndex];
            ViewBag.randomStarIndex = randomStarIndex;


            _context.Questions.ToList();
            var quizes = _context.Quizes.ToList();
            var qb = _context.Questionsbank.ToList();
            ViewBag.Quizes = quizes;
            return View(qb);
        }
        [HttpPost]
        public async Task<IActionResult> DisplayGeneralQuiz(bool searchtype, int randomStarIndex)
        {
            double grade = 0;

            List<Body> search = await _solarDal.GetBody();

            bool a = search[randomStarIndex].isPlanet;

            if (searchtype == a)
            {
                grade += 100;
                ViewBag.answer = "Correct! You got 100%";
            }
            else
            {
                grade = 0;
                ViewBag.answer = "Incorrect. You lose. ZERO!";
            }
            return RedirectToAction("Results", new { g = grade });

        }
        public IActionResult Results(double g)
        {

            //know we need this. finding user id. 
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //does the logged in person match someone in the database
            List<Students> students = _context.Students.Where(x => x.UserId == id).ToList();

            int sid = students[0].Id;
            int qid = 1; 

            Grades newGrade = new Grades();

            newGrade.StudentId = sid;
            newGrade.QuizId = qid;
            newGrade.Grade = g;
            _context.Grades.Add(newGrade);
            _context.SaveChanges(); 

            // grades = _context.Grades.Where(x => x.StudentId = Id).First();
            return View("BoolResults", g);

        }
    }
}

