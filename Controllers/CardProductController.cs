using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cards.Helpers;
using Cards.Models;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers
{
    public class CardProductController : Controller
    {
        private readonly ICardProductRepository _cardProductRepo;
        private readonly bool _accountController;

        public CardProductController(ICardProductRepository cardProductRepo, IAccountController accountController)
        {
            _cardProductRepo = cardProductRepo;
            //var result = new AccountController().CheckSessionState();
            _accountController = accountController.CheckSessionState();
        }

        // GET: CardProduct
        public IActionResult Index()
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            var products = _cardProductRepo.GetAllCardProducts().OrderBy(p => p.Id);

            return View(products);
        }

        // GET: CardProduct/Details/5
        public ActionResult Details(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            return View();
        }

        // GET: CardProduct/Create
        public IActionResult Create()
        {
            ViewBag.FileType = _cardProductRepo.GetAllFileTypes();
            ViewBag.CardType = _cardProductRepo.GetAllCardType();


            return View();
        }

        // POST: CardProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CardProduct cardProduct)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            try
            {
                if(ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    _cardProductRepo.AddProduct(cardProduct);
                    return RedirectToAction(nameof(Index));

                }
                return View();

            }
            catch
            {
                return View();
            }
        }

        // GET: CardProduct/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

            return View();
        }

        // POST: CardProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (!_accountController)
            {
                return RedirectToAction("SignOff", "Account");
            }

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

        // GET: CardProduct/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CardProduct/Delete/5
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