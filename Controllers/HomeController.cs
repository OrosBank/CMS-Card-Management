using Cards.Controllers;
using Cards.DatabaseLink;
using Cards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.DatabaseLink
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly bool _accountController;
        private readonly ApplicationDbContext _appDbContext;

        public HomeController(UserManager<ApplicationUser> userManager, IAccountController accountController, ApplicationDbContext appDbContext)
        {
            this.userManager = userManager;
            _appDbContext = appDbContext;
            _accountController = accountController.CheckSessionState();
        }
        //[Authorize(Roles = "User")]
        public IActionResult Index()
        {
            //string userName =  userManager.GetUserName(User);
            //return View("Index",userName);

            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            ViewBag.newCards = _appDbContext.CardIssuances.Where(c => c.CardStatusId == 1).ToList().Count();
            ViewBag.declinedCards = _appDbContext.CardIssuances.Where(c => c.CardStatusId == 4).ToList().Count();
            ViewBag.processedCards = _appDbContext.CardIssuances.Where(c => c.CardStatusId >= 6 ).ToList().Count();
            ViewBag.unverifiedCards = _appDbContext.CardIssuances.Where(c => c.CardStatusId == 2).ToList().Count();
            ViewBag.masterUSDCards = _appDbContext.CardIssuances.Where(c => c.ProductId == 2).ToList().Count();
            ViewBag.masterNGNCards = _appDbContext.CardIssuances.Where(c => c.ProductId == 1).ToList().Count();
            ViewBag.verveNGNCards = _appDbContext.CardIssuances.Where(c => c.ProductId == 3).ToList().Count();
            ViewBag.visaNGNCards = _appDbContext.CardIssuances.Where(c => c.ProductId == 4).ToList().Count();



            //foreach (var line in data.GroupBy(info => info.metric)
            //            .Select(group => new {
            //                Metric = group.Key,
            //                Count = group.Count()
            //            })
            //            .OrderBy(x => x.Metric)

            return View();
        }       
    }
}
