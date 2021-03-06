﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject_SolarSystemEducationApp.Areas.Identity.Pages.Account;
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
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, SolarSystemDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Welcome()
        {
            return View(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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

                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Student");
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

                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Teacher");
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

                await _userManager.AddToRoleAsync(user, "Admin");

                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Administration");
            }
            return View("index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(EditRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            await _roleManager.DeleteAsync(role);

            return RedirectToAction("ListRoles");
        }

        //When someone new registers, we need to assign him/her into a role that is chosen by a user and a password if
        //the role is an Admin or a Teacher
        [HttpGet]
        public IActionResult AssignARole(string msg)
        {
            var roles = _context.AspNetRoles.ToList();
            ViewBag.classrooms = _context.Classrooms.ToList();
            ViewBag.ErrorMessage = msg;
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
            else if(password == "5678" && role == "Admin")
            {
                return RedirectToAction("AssignAsAdmin");
            }
            else if (password == null || password == "" && role == "Student")
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
            else
            {
                string message = "Please enter valid information.";
                return RedirectToAction("AssignARole", new { msg = message });
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            PrincipleViewModel principle = new PrincipleViewModel();

            //Displaying the Deans list and the students who needs support
            principle.studentsWithDifficulty = _context.Students.Where(x => x.AverageGrade < 60).OrderBy(x => x.AverageGrade).ToList();
            principle.studentsOnDeansList = _context.Students.Where(x => x.AverageGrade > 80).OrderByDescending(x => x.AverageGrade).ToList();


            principle.classrooms = _context.Classrooms.Where(x => x.Teacher != null).ToList();
            principle.students = _context.Students.ToList();
            principle.teachers = _context.Teachers.ToList();
            principle.grades = _context.Grades.ToList();
            principle.roles = _context.AspNetRoles.ToList();
            principle.userRoles = _context.AspNetUserRoles.ToList();
            principle.users = _context.AspNetUsers.ToList();

            return View(principle);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DisplayStudents()
        {
            PrincipleViewModel principal = new PrincipleViewModel();
            principal.students = _context.Students.ToList();
            principal.classrooms = _context.Classrooms.ToList();
            return View(principal);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStudent(int id)
        {
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.students = _context.Students.Where(x => x.Id == id).ToList();
            
            principle.grades = _context.Grades.Where(x => x.StudentId == principle.students[0].Id).ToList();

            foreach (Grades g in principle.grades)
            {
                if (g != null)
                {
                    _context.Grades.Remove(g);
                    _context.SaveChanges();
                }
            }
            foreach (Students s in principle.students)
            {
                if (s != null)
                {
                    _context.Students.Remove(s);
                    _context.SaveChanges();
                }

                principle.users = _context.AspNetUsers.Where(x => x.Id == s.UserId).ToList();

                foreach (AspNetUsers a in principle.users)
                {
                    if (a != null)
                    {
                        _context.AspNetUsers.Remove(a);
                        _context.SaveChanges();
                    }
                }
            }
     
            return RedirectToAction("DisplayStudents");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DisplayUsers()
        {
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.users = _context.AspNetUsers.ToList();
            principle.roles = _context.AspNetRoles.ToList();
            principle.userRoles = _context.AspNetUserRoles.ToList();
            principle.students = _context.Students.ToList();
            principle.teachers = _context.Teachers.ToList();
            return View(principle);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string id)
        {
            var foundUser = _context.AspNetUsers.Find(id);
            if (foundUser != null)
            {
                _context.AspNetUsers.Remove(foundUser);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayUsers");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DisplayTeachers()
        {
            var teacherList = _context.Teachers.ToList();
            return View(teacherList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTeacher(int id)
        {
            //In order to delte the teacher, you need to delete the class room, students, grades first
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.teachers = _context.Teachers.Where(x => x.Id == id).ToList();
            principle.classrooms = _context.Classrooms.Where(x => x.TeacherId == id).ToList();
            foreach (Classrooms c in principle.classrooms)
            {
                DeleteStudentsGradesUsers(c.Id);
                DeleteClassroom(c.Id);
            }

            var foundTeacher = _context.Teachers.Find(id);
            if (foundTeacher != null)
            {
                _context.Teachers.Remove(foundTeacher);
                _context.SaveChanges();

                //deleting the user
                principle.users = _context.AspNetUsers.Where(x => x.Id == foundTeacher.UserId).ToList();
                foreach (AspNetUsers a in principle.users)
                {
                    if (a != null)
                    {
                        _context.AspNetUsers.Remove(a);
                        _context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("DisplayTeachers");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DisplayGrades()
        {
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.grades = _context.Grades.ToList();
            principle.classrooms = _context.Classrooms.ToList();
            principle.students = _context.Students.ToList();
            principle.quizes = _context.Quizes.ToList();
            principle.teachers = _context.Teachers.ToList();
            return View(principle);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult HumanResources()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DisplayClassrooms()
        {
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.grades = _context.Grades.ToList();
            principle.classrooms = _context.Classrooms.ToList();
            principle.students = _context.Students.ToList();
            principle.quizes = _context.Quizes.ToList();
            principle.teachers = _context.Teachers.ToList();
            return View(principle);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteClassroom(int id)
        {
            //In order to delte the classroom, you need to delete the students in the classroom first
            //In order to delte the classroom, you need to delete the students grades in the classroom first
            //Finally delete the user

            DeleteStudentsGradesUsers(id);
            var foundClassroom = _context.Classrooms.Find(id);

            if (foundClassroom != null)
            {
                _context.Classrooms.Remove(foundClassroom);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayClassrooms");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteGrade(int id)
        {
            var foundGrade = _context.Grades.Find(id);

            if (foundGrade != null)
            {
                _context.Grades.Remove(foundGrade);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayGrades");
        }
        public void DeleteStudentsGradesUsers(int id)
        {
            PrincipleViewModel principle = new PrincipleViewModel();
            principle.students = _context.Students.Where(x => x.ClassroomId == id).ToList();

            for (int i = 0; i < principle.students.Count; i++)
            {
                principle.grades = _context.Grades.Where(x => x.StudentId == principle.students[i].Id).ToList();

                foreach (Grades g in principle.grades)
                {
                    if (g != null)
                    {
                        _context.Grades.Remove(g);
                        _context.SaveChanges();
                    }
                }
            }
            foreach (Students s in principle.students)
            {
                if (s != null)
                {
                    _context.Students.Remove(s);
                    _context.SaveChanges();
                }

                principle.users = _context.AspNetUsers.Where(x => x.Id == s.UserId).ToList();

                foreach (AspNetUsers a in principle.users)
                {
                    if (a != null)
                    {
                        _context.AspNetUsers.Remove(a);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
