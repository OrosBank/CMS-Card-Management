using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cards.Helpers;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList;
using PagedList.Core;
using static Cards.Helpers.Helper;

namespace Cards.Controllers
{
    public class EBankingController : Controller
    {
        private readonly IEBankingRepository _eBankingRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardRepository _cardRequestRepo;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly bool _accountController;

        public EBankingController(IEBankingRepository eBankingRepo, IHttpContextAccessor httpContextAccessor, ICardRepository cardRequestRepo, IAccountController accountController)
        {
            _eBankingRepo = eBankingRepo;
            _httpContextAccessor = httpContextAccessor;
            _cardRequestRepo = cardRequestRepo;
            //var result = new AccountController().CheckSessionState();
            _accountController = accountController.CheckSessionState();
        }

        // GET: EBanking
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string branch, string cardStatus, string pinStatus, string fromDate, string toDate, int? page)
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
            ViewBag.AllBranches = _eBankingRepo.GetAllBranches();

            var requests = _eBankingRepo.GetAllCardReqs().OrderBy(o => o.Id);

            if (!String.IsNullOrEmpty(searchString) || branch != null || cardStatus != null || pinStatus != null || fromDate != null || toDate != null)
            {
                requests = _eBankingRepo.GetCardRequestBySearchString(searchString, branch, cardStatus, pinStatus, fromDate, toDate).OrderBy(o => o.Id);
            }
            switch (sortOrder)
            {
                case "product_desc":
                    requests = requests.OrderByDescending(s => s.Product);
                    break;
                case "status_desc":
                    requests = requests.OrderByDescending(s => s.CardStatus);
                    break;
                case "branch_desc":
                    requests = requests.OrderByDescending(s => s.Branch);
                    break;
                case "pickup_desc":
                    requests = requests.OrderByDescending(s => s.PickUpBranch);
                    break;
                case "Date":
                    requests = requests.OrderBy(s => s.EntryDate);
                    break;
                case "date_desc":
                    requests = requests.OrderByDescending(s => s.ExpiryDate);
                    break;
                default:
                    requests = requests.OrderBy(s => s.Id);
                    break;
            }

            var str = JsonConvert.SerializeObject(requests);
            _session.SetString("requests", str);

            ViewBag.CardStatus = _eBankingRepo.GetCardStatus();
            ViewBag.PinStatus = _eBankingRepo.GetPinStatus();

            int pageSize = 10;
            return View(PaginatedList<IssuanceDisplayViewModel>.CreateAsync(requests.AsQueryable(), page ?? 1, pageSize));
            //return View(requests);
        }

        public ActionResult DownloadCards(string sortOrder, string currentFilter, string searchString, int product, int? page)
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
            ViewBag.AllProducts = _eBankingRepo.GetAllCardProducts();

            var requests = _eBankingRepo.GetAllCardReqs().OrderBy(o => o.Id);

            if (product > 0)
            {
                requests = _eBankingRepo.GetCardRequestByProduct(product).OrderByDescending(o => o.Id);
            }
            switch (sortOrder)
            {
                case "product_desc":
                    requests = requests.OrderByDescending(s => s.Product);
                    break;
                case "status_desc":
                    requests = requests.OrderByDescending(s => s.CardStatus);
                    break;
                case "branch_desc":
                    requests = requests.OrderByDescending(s => s.Branch);
                    break;
                case "pickup_desc":
                    requests = requests.OrderByDescending(s => s.PickUpBranch);
                    break;
                case "Date":
                    requests = requests.OrderBy(s => s.EntryDate);
                    break;
                case "date_desc":
                    requests = requests.OrderByDescending(s => s.ExpiryDate);
                    break;
                default:
                    requests = requests.OrderBy(s => s.Id);
                    break;
            }

            var str = JsonConvert.SerializeObject(requests);
            _session.SetString("ProcessRequests", str);
            _session.SetInt32("product", product);
            ViewBag.RequestnotFound = TempData["RequestnotFound"];

            ViewBag.CardStatus = _eBankingRepo.GetCardStatus();
            ViewBag.PinStatus = _eBankingRepo.GetPinStatus();

            int pageSize = 10;
            return View(PaginatedList<IssuanceDisplayViewModel>.CreateAsync(requests.AsQueryable(), page ?? 1, pageSize));
            //return View(requests);
        }


        // GET: EBanking/Details/5
        public ActionResult ProcessSingle(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            var cvm = _cardRequestRepo.GetCardRequestById(id);

            if(cvm == null)
            {
                TempData["RequestnotFound"] = "Request not Found";
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

        [HttpPost]
        public ActionResult ProcessSingle(int Id, string authorize, string decline, IFormCollection form)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            _eBankingRepo.AuthorizeSingle(Id, authorize, decline, form);

            return RedirectToAction(nameof(Index));
        }

        // GET: EBanking/Create
        //public ActionResult Process()
        //{
        //    return View();
        //}



        // POST: EBanking/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Process()
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                var str = _session.GetString("ProcessRequests");
                var product = (int)_session.GetInt32("product");

                if (!String.IsNullOrEmpty(str))
                {

                    var obj = JsonConvert.DeserializeObject<IEnumerable<IssuanceDisplayViewModel>>(str);

                    _eBankingRepo.ProcessCards(obj, product);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }



        public ActionResult AuthorizeAllCard()
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                var str = _session.GetString("requests");

                if (!String.IsNullOrEmpty(str))
                {

                    var obj = JsonConvert.DeserializeObject<IEnumerable<IssuanceDisplayViewModel>>(str);

                    _eBankingRepo.AuthorizeAllCard(obj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
                return View();
            }
        }

        public ActionResult DeclineAllCard()
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }

                var str = _session.GetString("requests");

                if (!String.IsNullOrEmpty(str))
                {

                    var obj = JsonConvert.DeserializeObject<IEnumerable<IssuanceDisplayViewModel>>(str);

                    _eBankingRepo.AuthorizeAllCard(obj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }
        //[HttpPost]
        public ActionResult ReIssueCardSinge(int id)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }

        // POST: EBanking/Edit/5
        //[HttpPost]
        public ActionResult ReIssuePinSinge(int id)
        {
            try
            {
                if (!_accountController)
                {
                    return RedirectToAction("SignOff", "Account");
                }
                // TODO: Add update logic here

                _eBankingRepo.Request(id);

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
        public ActionResult UpdateReturnedCards()
        {
            ViewBag.batch = _eBankingRepo.CardBatch();

            return View();
        }

        public ActionResult Returned(int Id)
        {
            string Returned = "Returned";

            ViewBag.feedback = _eBankingRepo.CardBatchUpdate(Id, Returned, "");

            ViewBag.batch = _eBankingRepo.CardBatch();

            return View("UpdateReturnedCards");
        }

        public ActionResult Dispatched(int Id)
        {
            string Dispatched = "Dispatched";

            ViewBag.feedback = _eBankingRepo.CardBatchUpdate(Id, "", Dispatched);

            ViewBag.batch = _eBankingRepo.CardBatch();

            return View("UpdateReturnedCards");
        }

        // GET: EBanking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EBanking/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }
    }
}