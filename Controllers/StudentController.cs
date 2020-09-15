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
        private readonly SolarDAL _solarDAL;

        public StudentController()
        {
            _solarDAL = new SolarDAL();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayBodies()
        {
            var allInfo = await _solarDAL.GetBody();
            return View(allInfo);
        }

        public async Task<IActionResult> BodyDetails(string id)
        {
            List<Body> bodiesList = await _solarDAL.GetBody();
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
    }
}
