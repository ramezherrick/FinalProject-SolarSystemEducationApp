using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;




namespace FinalProject_SolarSystemEducationApp.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
                       


        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;

        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };
                await _roleManager.CreateAsync(identityRole);
                return RedirectToAction("index", "home");
            }
            return View(model);
        }
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> DeleteRole(EditRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            await _roleManager.DeleteAsync(role);
          
            return RedirectToAction("ListRoles");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
