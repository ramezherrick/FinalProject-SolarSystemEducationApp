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
        private readonly SolarDAL _solarDAL;

        public StudentController(SolarSystemDbContext context, SolarDAL solarDAL)
        {
            _context = context;
            _solarDAL = solarDAL;
        }
        public async Task<IActionResult> DisplayGeneralQuizAsync()
        {
            var search = await _solarDAL.GetBody(); 

           //It's probably three lines of code that go here.
           //stick the english name in a view bag and get the index of over to the WhatAmI method. 

            //Need to make that two methods. 


            _context.Questions.ToList();
            var quizes = _context.Quizes.ToList();
            var qb = _context.Questionsbank.ToList();
            ViewBag.Quizes = quizes;
            return View(qb);
        }
        [HttpGet]
        public async Task<IActionResult> WhatAmI(string searchtype, )
        {

            if(searchtype == )
            {

                string answer = "Correct!";
                return View("DisplayGeneralQuizAsync", answer)
            }
            else
            {
                string answer = "Incorrect";
                return View("DisplayGeneralQuizAsync", answer); 
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
