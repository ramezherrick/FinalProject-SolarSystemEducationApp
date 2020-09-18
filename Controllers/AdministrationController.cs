using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Models;
using Microsoft.AspNetCore.Authentication;
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
        private readonly SolarSystemDbContext _context;


        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, SolarSystemDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
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

                for (int i = 0; i < role.Count; i++)
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

                return RedirectToAction("Index", "Home");
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

        [Authorize(Roles = "admin")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRole(EditRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            await _roleManager.DeleteAsync(role);

            return RedirectToAction("ListRoles");
        }

        //When someone new registers, we need to assign him/her into a role that is chosen by a user and a password if
        //the role is an Admin or a Teacher
        [HttpGet]
        public IActionResult AssignARole()
        {
            var roles = _context.AspNetRoles.ToList();
            ViewBag.classrooms = _context.Classrooms.ToList();
            return View(roles);
        }
        [HttpPost]
        public IActionResult AssignARole(string FirstName, string LastName, string role, string password, int classId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (password == "1234" && role == "Teacher")
            {
                //1-creating a new teacher object
                //2- get Fname and Lname from the userform
                //3-saves the new teacher to SQL
                Teachers teacher = new Teachers();
                teacher.FirstName = FirstName;
                teacher.LastName = LastName;
                teacher.UserId = userId;

                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return RedirectToAction("AssignAsTeacher");

            }
            else if(password == "5678" && role == "admin")
            {
                return RedirectToAction("AssignAsAdmin");
            }
            else
            {
                //1-creating a new student object
                //2- get Fname and Lname and ClassroomId from the userform
                //3-saves the new student to SQL
                Students student = new Students();
                student.FirstName = FirstName;
                student.LastName = LastName;
                student.UserId = userId;
                student.ClassroomId = classId;

                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("AssignAsStudent");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayStudents()
        {
            var studentList = _context.Students.ToList();
            return View(studentList);
        }
        public IActionResult DisplayUsers()
        {
            var usersList = _context.AspNetUsers.ToList();
            return View(usersList);
        }
        public IActionResult DisplayTeachers()
        {
            var teacherList = _context.Teachers.ToList();
            return View(teacherList);
        }
        public IActionResult DisplayGrades()
        {
            var gradesList = _context.Grades.ToList();
            return View(gradesList);
        }
        public IActionResult DisplayClassrooms()
        {
            var classroomsList = _context.Classrooms.ToList();
            return View(classroomsList);
        }
        public IActionResult DeleteStudent( int id)
        {
            var foundStudent = _context.Students.Find(id);
            if (foundStudent != null)
            {
                _context.Students.Remove(foundStudent);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteTeacher(int id)
        {
            var foundTeacher = _context.Teachers.Find(id);
            if (foundTeacher != null)
            {
                _context.Teachers.Remove(foundTeacher);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteUser(string id)
        {
            var foundUser = _context.AspNetUsers.Find(id);

            if(foundUser !=null)
            {
                _context.AspNetUsers.Remove(foundUser);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteClassroom(int id)
        {
            var foundClassroom = _context.Classrooms.Find(id);

            if (foundClassroom != null)
            {
                _context.Classrooms.Remove(foundClassroom);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public IActionResult DeleteGrade(int id)
        {
            var foundGrade = _context.Grades.Find(id);

            if (foundGrade != null)
            {
                _context.Grades.Remove(foundGrade);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
