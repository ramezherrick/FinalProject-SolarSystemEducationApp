using System;
using System.Collections.Generic;
using System.Linq;
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

            //Get A random planet from the API and send it to the view via Viewbag
            Random planet = new Random();
            int randomPlanetIndex = planet.Next(1, planetsList.Count);

            ViewBag.body = planetsList[randomPlanetIndex];
            ViewBag.randomPlanetIndex = randomPlanetIndex;

            //Get questionsbank from SQL and send it to the view
            _context.Questions.ToList();
            _context.Quizes.ToList();
            var questiosBankList = _context.Questionsbank.ToList();
            return View(questiosBankList);
        }

        [HttpPost]
        public async Task<IActionResult> TakePlanetsQuiz(int randomPlanetIndex, List<string>answers)
        {
            int numberOfCorrectAnswers = 0;
            int numberOfMoons = 0;

            List<Body> planetsList = await CreatePlanetsListFromAPIAsync();

            string mass = planetsList[randomPlanetIndex].mass.massValue.ToString() + "^" + planetsList[randomPlanetIndex].mass.massExponent.ToString();
            //check radius answer
            if (answers[0]==mass)
            {
                numberOfCorrectAnswers += 1;
            }
            //check volume answer
            string planetVolume = planetsList[randomPlanetIndex].vol.volValue.ToString() + "^" + planetsList[randomPlanetIndex].vol.volExponent.ToString();
            if (answers[1]==planetVolume)
            {
                numberOfCorrectAnswers += 1;
            }
            //checking for number of moons it has
            if (planetsList[randomPlanetIndex].moons==null)
            {
                numberOfMoons = 0;
            }
            if (planetsList[randomPlanetIndex].moons != null)
            {
                numberOfMoons = planetsList[randomPlanetIndex].moons.Count();
            }

            if (answers[2] == numberOfMoons.ToString())
            {
                numberOfCorrectAnswers += 1;
            }
            //check who discovered this planet
            if(answers[3]== planetsList[randomPlanetIndex].discoveredBy)
            {
                numberOfCorrectAnswers += 1;
            }
            //check when was the planet discovered
            if(answers[4]==planetsList[randomPlanetIndex].discoveryDate)
            {
                numberOfCorrectAnswers += 1;
            }
            //check gravity answer
            if(answers[5]== planetsList[randomPlanetIndex].gravity.ToString())
            {
                numberOfCorrectAnswers += 1;
            }

            return RedirectToAction("Index");

        }
        public async Task<List<Body>> CreatePlanetsListFromAPIAsync()
        {
            //access API get a list of all bodies in the api
            List<Body> body = await _solarDal.GetBody();

            //create an empty list of bodies and populate it with planets
            List<Body> planetsList = new List<Body>();

            for (int i = 0; i < body.Count; i++)
            {
                if (body[i].isPlanet == true)
                {
                    planetsList.Add(body[i]);
                }
            }
            return planetsList;
        }
    }
}
