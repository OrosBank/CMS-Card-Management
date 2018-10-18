using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cards.Helpers;
using Cards.Models;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Cards.Helpers.Helper;

namespace Cards.Controllers
{
    public class CardIssuanceController : Controller
    {

        private readonly ICardRepository _cardRequestRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly bool _accountController;

        public CardIssuanceController(ICardRepository cardRequestRepo, IHttpContextAccessor httpContextAccessor, IAccountController accountController)
        {
            _cardRequestRepo = cardRequestRepo;
            _httpContextAccessor = httpContextAccessor;
            //var result = new AccountController().CheckSessionState();
            _accountController = accountController.CheckSessionState();

            
        }

        public IActionResult GetCustomerDetails(string AccountNumber)
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                List<Gender> gender = new List<Gender>();

               var cvm = Activity.GetCustomer(AccountNumber);

                if(cvm == null)
                {
                    ViewBag.GetCustomerFeedBack = "Get Customer API is offline or Customer does not exist.";
                }

                ViewBag.customer = cvm;
                ViewData["customerVm"] = cvm;

                ViewBag.CardProduct = _cardRequestRepo.GetAllCardProducts();
                ViewBag.branch = _cardRequestRepo.GetAllBranches();

                ViewBag.Gender = new List<Gender>
                {
                    new Gender {Name = "Male"},
                    new Gender {Name = "Female"}
                };
                //Activity.GenFiles();
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                _session.SetString("CustomerAPIFeedback", ex.Message);
            }

            return View("Create");
        }

        public ActionResult Verify(int id)
        {
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                var cvm = _cardRequestRepo.GetCardRequestById(id);

                if (cvm == null)
                {
                    TempData["nullRequest"] = "You can only Edit Request that Originate from your branch";

                    //ViewBag.nullRequest

                    return RedirectToAction("Index");
                }

                ViewBag.customer = cvm;
                ViewData["customerVm"] = cvm;

                ViewBag.CardProduct = _cardRequestRepo.GetAllCardProducts();
                ViewBag.branch = _cardRequestRepo.GetAllBranches();

                ViewBag.Gender = new List<Gender>
                {
                    new Gender {Name = "Male"},
                    new Gender {Name = "Female"}
                };
                return View(cvm);
            }
        }

        [HttpPost]
        public ActionResult Verify(int id, string verify, string decline)
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }
               ViewBag.message =  _cardRequestRepo.VerifyCard(id, verify, decline);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {

                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }

        // GET: CardIssuance
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            ViewData["ProductSortParm"] = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";
            ViewData["StatusSortParm"] = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            ViewData["BranchSortParm"] = String.IsNullOrEmpty(sortOrder) ? "branch_desc" : "";
            ViewData["PickUpBranchSortParm"] = String.IsNullOrEmpty(sortOrder) ? "pickup_desc" : "";
            ViewData["EntryDateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["ExpiryDateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            //var students = from s in _context.Students
            //               select s;
            var requests = _cardRequestRepo.GetAllCardRequests().OrderByDescending(o => o.EntryDate);

            if (!String.IsNullOrEmpty(searchString))
            {
                requests = _cardRequestRepo.GetCardRequestBySearch(searchString, "").OrderByDescending(o => o.EntryDate);
            }
            switch (sortOrder)
            {
                case "product_desc":
                    requests = requests.OrderByDescending(s => s.Product).OrderByDescending(o => o.EntryDate);
                    break;
                case "status_desc":
                    requests = requests.OrderByDescending(s => s.CardStatus).OrderByDescending(o => o.EntryDate);
                    break;
                case "branch_desc":
                    requests = requests.OrderByDescending(s => s.Branch).OrderByDescending(o => o.EntryDate);
                    break;
                case "pickup_desc":
                    requests = requests.OrderByDescending(s => s.PickUpBranch).OrderByDescending(o => o.EntryDate);
                    break;
                case "Date":
                    requests = requests.OrderBy(s => s.EntryDate).OrderByDescending(o => o.EntryDate);
                    break;
                case "date_desc":
                    requests = requests.OrderByDescending(s => s.ExpiryDate).OrderByDescending(o => o.EntryDate);
                    break;
                default:
                    requests = requests.OrderBy(s => s.Id).OrderByDescending(o => o.EntryDate);
                    break;
            }
            ViewBag.RequestExist = TempData["RequestExist"];
            ViewBag.CreatedRequest = TempData["CreatedRequest"];
            ViewBag.CustomerCreated = TempData["CustomerCreated"];
            ViewBag.nullRequest = TempData["nullRequest"];



            //return View(requests);
            //return View(requests.ToList());
            int pageSize = 10;
            return View(PaginatedList<IssuanceDisplayViewModel>.CreateAsync(requests.AsQueryable(), page ?? 1, pageSize));
        }

        // GET: CardIssuance/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: CardIssuance/Create
        public IActionResult Create()
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            return View();
        }

        // POST: CardIssuance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection data)
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }
                _cardRequestRepo.AddCustomer(data);

                TempData["RequestExist"] = HttpContext.Session.GetString("RequestExist");
                TempData["CreatedRequest"] = HttpContext.Session.GetString("CreatedRequest");
                TempData["CustomerCreated"] = HttpContext.Session.GetString("CustomerCreated");
                

                return RedirectToAction(nameof(Index));
               
            }
            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }

            
        }

        // GET: CardIssuance/Edit/5
        public ActionResult Edit(int id)
        { 

            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                var cvm = _cardRequestRepo.GetCardRequestById(id);


                if(cvm == null)
                {
                    TempData["nullRequest"] = "You can only Edit Request that Originate from your branch or has not been verified";

                    //ViewBag.nullRequest
                    return RedirectToAction("Index");
                }

                ViewBag.customer = cvm;
                ViewData["customerVm"] = cvm;

                ViewBag.CardProduct = _cardRequestRepo.GetAllCardProducts();
                ViewBag.branch = _cardRequestRepo.GetAllBranches();

                ViewBag.Gender = new List<Gender>
                {
                    new Gender {Name = "Male"},
                    new Gender {Name = "Female"}
                };
                return View(cvm);
            }
        }

        // POST: CardIssuance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                // TODO: Add update logic here
                ViewBag.message = _cardRequestRepo.EditCardRequest(id, collection);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {

                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }


        [HttpGet]
        public ActionResult ReturnedCards()
        {
            ViewBag.batch = _cardRequestRepo.CardBranchBatch();

            return View();
        }

        public ActionResult Returned(int Id)
        {
            string Returned = "Returned";

            ViewBag.feedback = _cardRequestRepo.CardBranchBatchUpdate(Id, Returned, "");

            ViewBag.batch = _cardRequestRepo.CardBranchBatch();

            return View("ReturnedCards");
        }

        public ActionResult ReleaseToCustomer(int Id)
        {
            ViewBag.feedback = _cardRequestRepo.ReleaseCard(Id);

            ViewBag.batch = _cardRequestRepo.CardBranchBatch();

            return View("Index");
        }

        // GET: CardIssuance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CardIssuance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {

                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }
    }
}