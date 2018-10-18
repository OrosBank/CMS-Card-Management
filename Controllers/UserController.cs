using Cards.DatabaseLink;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext _db;
        private readonly bool _accountController;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext db, IAccountController accountController)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _db = db;
            _accountController = accountController.CheckSessionState();
        }

        [HttpGet]
        public IActionResult Index()
        {

            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            ViewBag.Uername = TempData["UserName"];
            ViewBag.Success = TempData["Success"];

            List<UserListViewModel> model = new List<UserListViewModel>();
            model = userManager.Users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email,
                RoleName = _db.Roles.Where(r => r.Id == _db.UserRoles.Where(ur => ur.UserId == u.Id).FirstOrDefault().RoleId).Select(s => s.Name).FirstOrDefault(),
                BranchName = _db.Branches.Where(B => B.Id == u.BranchId).Select(s => s.BranchName).FirstOrDefault()
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();

            ViewBag.RoleList = _db.Roles.ToList();
            ViewBag.branch = _db.Branches.ToList();


            return PartialView("_AddUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,
                    BranchId = model.BranchId
                };

                var userExist = _db.Users.Where(u => u.UserName.Equals(model.UserName)).FirstOrDefault();

                if(userExist != null)
                {
                    TempData["UserName"] = "Username Already Exist";
                    return RedirectToAction("Index");
                }

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == model.ApplicationRoleId).FirstOrDefault();
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            TempData["Success"] = "User Created Successfully!!!";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            EditUserViewModel model = new EditUserViewModel();
            ViewBag.branch = _db.Branches.ToList();

            ViewBag.RoleList = _db.Roles.ToList();
            if (id > 0)
            {
                //ApplicationUser user = await userManager.FindByIdAsync(id);
                ApplicationUser user = _db.Users.Where(r => r.Id == id).FirstOrDefault();
                if (user != null)
                {
                    model.Name = user.Name;
                    model.Email = user.Email;
                    //model.ApplicationRoleId = roleManager.Roles.Single(r => r.Name == userManager.GetRolesAsync(user).Result.Single()).Id;
                    model.ApplicationRoleId = _db.Roles.Where(r => r.Id == (_db.UserRoles.Where(u => u.UserId == user.Id).Select(s => s.RoleId).FirstOrDefault())).Select(s => s.Id).FirstOrDefault();
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ApplicationUser user = await userManager.FindByIdAsync(id);
                ApplicationUser user = _db.Users.Where(r => r.Id == model.Id).FirstOrDefault();
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.BranchId = model.BranchId;
                    string existingRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                    if(existingRole != null)
                    {
                        int existingRoleId = roleManager.Roles.Single(r => r.Name == existingRole).Id;
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            if (existingRoleId != model.ApplicationRoleId)
                            {
                                IdentityResult roleResult = await userManager.RemoveFromRoleAsync(user, existingRole);
                                if (roleResult.Succeeded)
                                {
                                    //ApplicationRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                                    ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == model.ApplicationRoleId).FirstOrDefault();
                                    if (applicationRole != null)
                                    {
                                        IdentityResult newRoleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                                        if (newRoleResult.Succeeded)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        ApplicationRole applicationRole = _db.Roles.Where(r => r.Id == model.ApplicationRoleId).FirstOrDefault();
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                            if (roleResult.Succeeded)
                            {
                                //TempData["Success"] = "User Created Successfully!!!";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            string name = string.Empty;
            if (id > 0)
            {
                //ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                ApplicationUser applicationUser = _db.Users.Where(r => r.Id == id).FirstOrDefault();
                if (applicationUser != null)
                {
                    name = applicationUser.Name;
                }
            }
            return PartialView("_DeleteUser", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id, FormCollection form)
        {
            if (id > 0)
            {
                //ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                ApplicationUser applicationUser = _db.Users.Where(r => r.Id == id).FirstOrDefault();
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser); 
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}
