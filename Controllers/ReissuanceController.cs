using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cards.Helpers;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers
{
    public class ReissuanceController : Controller
    {

        private readonly IReissuanceRepository _pinRequestRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly bool _accountController;

        public ReissuanceController(IReissuanceRepository pinRequestRepo, IHttpContextAccessor httpContextAccessor, IAccountController accountController)
        {
            _pinRequestRepo = pinRequestRepo;
            _httpContextAccessor = httpContextAccessor;
            //var result = new AccountController().CheckSessionState();
            _accountController = accountController.CheckSessionState();

        }
        // GET: Reissuance
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
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
                var requests = _pinRequestRepo.GetRequests(searchString).OrderBy(o => o.Id);

                if (!String.IsNullOrEmpty(searchString))
                {
                   requests = _pinRequestRepo.GetRequests(searchString).OrderBy(o => o.Id);
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
                ViewBag.RequestExist = TempData["RequestExist"];
                ViewBag.CreatedRequest = TempData["CreatedRequest"];
                ViewBag.CustomerCreated = TempData["CustomerCreated"];
                ViewBag.PinMessage = TempData["message"];
                ViewBag.CardMessage = TempData["cardmessage"];




                int pageSize = 10;
                return View(PaginatedList<IssuanceDisplayViewModel>.CreateAsync(requests.AsQueryable(), page ?? 1, pageSize));
            }
            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }
            return View();
        }

        // GET: Reissuance/Details/5
        public ActionResult Pin(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            TempData["message"] = _pinRequestRepo.PinReIssuance(id); // Display message on Index

            return RedirectToAction("Index");
        }
        public ActionResult Card(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            TempData["cardmessage"] = _pinRequestRepo.CardReIssuance(id); // Display message on Index

            return RedirectToAction("Index");
        }

        // GET: Reissuance/Create
        public ActionResult Create()
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            return View();
        }

        // POST: Reissuance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());

                return View();
            }
        }

        // GET: Reissuance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reissuance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reissuance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reissuance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}