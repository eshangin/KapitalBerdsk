using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Controllers
{
    public class FundsFlowController : Controller
    {
        // GET: FundsFlow
        public ActionResult Index()
        {
            var model = new List<FundsFlowListItemModel>();

            return View(model);
        }

        // GET: FundsFlow/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FundsFlow/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FundsFlow/Create
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

        // GET: FundsFlow/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FundsFlow/Edit/5
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

        // GET: FundsFlow/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FundsFlow/Delete/5
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