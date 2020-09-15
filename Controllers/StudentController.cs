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
            for(int i =0;i<planetsList.Count;i++)
            {
                if(planetsList[i].englishName==englishName)
                {
                    testedPlanet = planetsList[i];
                }
            }

            string mass = testedPlanet.mass.massValue.ToString() + "^" + testedPlanet.mass.massExponent.ToString();
            //check radius answer
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
            int total = numberOfCorrectAnswers;
            double grade = (total / 6) * 100;
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
        public async Task<IActionResult> DisplayGeneralQuizAsync()
        {
            var search = await _solarDal.GetBody();

            //It's probably three lines of code that go here.
            //stick the english name in a view bag and get the index of over to the WhatAmI method. 

            //Need to make that two methods. 


            _context.Questions.ToList();
            var quizes = _context.Quizes.ToList();
            var qb = _context.Questionsbank.ToList();
            ViewBag.Quizes = quizes;
            return View(qb);
        }
        //[HttpGet]
        //public async Task<IActionResult> WhatAmI(string searchtype, )
        //{

        //    if (searchtype == )
        //    {

        //        string answer = "Correct!";
        //        return View("DisplayGeneralQuizAsync", answer)
        //    }
        //    else
        //    {
        //        string answer = "Incorrect";
        //        return View("DisplayGeneralQuizAsync", answer);
        //    }
        //}
    }
}

