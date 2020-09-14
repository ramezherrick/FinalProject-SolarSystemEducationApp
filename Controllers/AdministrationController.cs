using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;




namespace FinalProject_SolarSystemEducationApp.Controllers
{
    
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<IActionResult> AssignAsStudent()
        {
            if (ModelState.IsValid)
            {
                //get logged in user id
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                //get entire user by user id
                var user = await _userManager.FindByIdAsync(userId);
                //get all roles associated with current user
                var role = await _userManager.GetRolesAsync(user);

                for (int i = 0; i<role.Count; i++)
                {
                    await _userManager.RemoveFromRoleAsync(user, role[i]);
                }
                
                await _userManager.AddToRoleAsync(user, "Student");
                
                return RedirectToAction("index", "home");
            }
            return View("index");
        }

        public async Task<IActionResult> AssignAsTeacher()
        {
            if (ModelState.IsValid)
            {
                //get logged in user id
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                //get entire user by user id
                var user = await _userManager.FindByIdAsync(userId);
                //get all roles associated with current user
                var role = await _userManager.GetRolesAsync(user);

                for (int i = 0; i < role.Count; i++)
                {
                    await _userManager.RemoveFromRoleAsync(user, role[i]);
                }

                await _userManager.AddToRoleAsync(user, "Teacher");

                return RedirectToAction("index", "home");
            }
            return View("index");
        }

        public async Task<IActionResult> AssignAsAdmin()
        {
            if (ModelState.IsValid)
            {
                //get logged in user id
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                //get entire user by user id
                var user = await _userManager.FindByIdAsync(userId);
                //get all roles associated with current user
                var role = await _userManager.GetRolesAsync(user);

                for (int i = 0; i < role.Count; i++)
                {
                    await _userManager.RemoveFromRoleAsync(user, role[i]);
                }

                await _userManager.AddToRoleAsync(user, "admin");

                return RedirectToAction("index", "home");
            }
            return View("index");
        }

        [Authorize(Roles = "Teacher")]
        [Authorize(Roles = "admin")]
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
