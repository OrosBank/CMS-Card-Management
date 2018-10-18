using Cards.DatabaseLink;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Controllers
{
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext _db;
        private readonly bool _accountController;

        public ApplicationRoleController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext db, IAccountController accountController)
        {
            this.roleManager = roleManager;
            _db = db;
            //var result = new AccountController().CheckSessionState();
            _accountController = accountController.CheckSessionState();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            IEnumerable<ApplicationRoleListViewModel> model = new List<ApplicationRoleListViewModel>();

            ViewBag.RoleExist = TempData["RoleExist"];
            ViewBag.RoleCreated = TempData["RoleCreated"];

            ViewBag.model = _db.Roles.Select( item => new ApplicationRoleListViewModel
            {
                Id = item.Id,
                RoleName = item.Name,
                Description = item.Description,
                NumberOfUsers = _db.UserRoles.Where(ur => ur.RoleId == item.Id).Count()


            }).ToList();

            return View();
        }

        public IActionResult Create()
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRoleViewModel applicationRoleView)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            //return View(name);
            if (ModelState.IsValid)
            {
                var roleExist = _db.Users.Where(u => u.UserName.Equals(applicationRoleView.RoleName)).FirstOrDefault();

                if (roleExist != null)
                {
                    TempData["RoleExist"] = "Role Already Exist";
                    return RedirectToAction("Index");
                }

                // Use ApplicationRole, not IdentityRole:
                var role = new ApplicationRole(applicationRoleView.RoleName);
                var roleresult = await roleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    //ModelState.AddModelError("", roleresult.Errors.First());
                    TempData["RoleCreated"] = "Role Created Successfuly!!!";
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public IActionResult AddEditApplicationRole(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            ApplicationRoleViewModel model = new ApplicationRoleViewModel();

            if (id > 0)
            {
                //ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == id).FirstOrDefault();
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return PartialView("_AddEditApplicationRole", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEditApplicationRole(int id, ApplicationRoleViewModel model)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            if (ModelState.IsValid)
            {
                int Id = id;
                bool isExist = Convert.ToBoolean(id);
               //ApplicationRole applicationRole = isExist ? await roleManager.FindByIdAsync(id) :

               ApplicationRole applicationRole = isExist ? _db.Roles.Where(r => r.Id == Id).FirstOrDefault() :
               new ApplicationRole
               {                   
                   CreatedDate = DateTime.UtcNow                   
               };

                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = isExist?  await roleManager.UpdateAsync(applicationRole)
                                                    : await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Index");
        }



        [HttpGet]
        public IActionResult DeleteApplicationRole(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            string name = string.Empty;
            if (id > 0)
            {
                // ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == id).FirstOrDefault();
                if (applicationRole != null)
                {
                    name = applicationRole.Name;
                }
            }
            return PartialView("_DeleteApplicationRole", name);
        }

        [HttpPost]
        public IActionResult DeleteApplicationRole(int id, IFormCollection form)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            if (id > 0)
            {
                //ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == id).FirstOrDefault();
                if (applicationRole != null)
                {
                    IdentityResult roleRuslt = roleManager.DeleteAsync(applicationRole).Result;
                    if (roleRuslt.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}
