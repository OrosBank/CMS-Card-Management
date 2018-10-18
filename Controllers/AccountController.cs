using Cards.DatabaseLink;
using Cards.Helpers;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;


namespace Cards.Controllers
{
    public class AccountController : Controller, IAccountController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = _appDbContext.Users.Where(u => u.Email == model.UserName).FirstOrDefault();

            try
            {

                if (ModelState.IsValid)
                {
                    var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        //var roleName = _appDbContext.Roles.Where(r => r.Id == (_appDbContext.UserRoles.Where(u => u.UserId == user.Id).Select(s => s.RoleId).FirstOrDefault())).Select(s => s.Name).FirstOrDefault();

                        var _user = await _userManager.FindByEmailAsync(user.Email);
                        // Get the roles for the user
                        var roles = await _userManager.GetRolesAsync(_user);
                        _session.SetString("email", user.Email);

                        _session.SetInt32("UserId", user.Id);
                        _session.SetInt32("BranchId", user.BranchId);
                        _session.SetString("role", roles[0]);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
            }

            catch(Exception ex)
            {
                Activity activity = new Activity();
                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                _session.SetString("CustomerAPIFeedback", ex.Message);

                return RedirectToAction("Login");
            }          
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOff()
        {          
            await signInManager.SignOutAsync();
            _session.Clear();
            return RedirectToAction("Login");
        }

       public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public bool CheckSessionState()
        {
            bool state = false;
            state = _session.GetInt32("UserId") == null ? false : true;

            if(!state)
            {
                return state;
            }

            return state;
        }
    }
}
