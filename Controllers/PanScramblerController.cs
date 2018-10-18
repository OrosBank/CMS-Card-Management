using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers
{
    public class PanScramblerController : Controller
    {
        private readonly IPanScramblerRepository _panScramber;

        public PanScramblerController(IPanScramblerRepository panScrambler)
        {
            _panScramber = panScrambler;
        }

        // GET: PanScrambler
        public ActionResult Index()
        {

            ViewBag.pans = _panScramber.GetAllPanDetails().OrderByDescending(o => o.Id);
            return View();
        }

        // GET: PanScrambler/Details/5
        public ActionResult FileUpload(IFormFile file)
        {

            //string[] _file = file.FileName.Split('.');
            //string fileType = _file[1];

            if (file != null)
            {
                //file = Request.

                ViewBag.feedback = _panScramber.UploadPanFile(file);
                
            }

            else
            {
                ViewBag.errorFeedback = "Check Uploaded file and try again...";
            }

            ViewBag.pans = _panScramber.GetAllPanDetails().OrderByDescending(o => o.Id);
            return View("Index");
        }

        // GET: PanScrambler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PanScrambler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PanScrambler/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PanScrambler/Edit/5
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

        // GET: PanScrambler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PanScrambler/Delete/5
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