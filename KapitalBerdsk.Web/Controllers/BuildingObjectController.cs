using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data;
using KapitalBerdsk.Web.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Controllers
{
    public class BuildingObjectController : Controller
    {
        private ApplicationDbContext _context;

        public BuildingObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildingObject
        public async Task<ActionResult> Index()
        {
            var model = (await _context.BuildingObjects.ToListAsync()).Select(item => new BuildingObjectModel
            {
                Id = item.Id,
                Name = item.Name
            });

            return View(model);
        }

        // GET: BuildingObject/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BuildingObject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuildingObject/Create
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

        // GET: BuildingObject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BuildingObject/Edit/5
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

        // GET: BuildingObject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BuildingObject/Delete/5
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