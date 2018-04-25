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
        public async Task<ActionResult> Details(int id)
        {
            var el = await _context.BuildingObjects
                    .Include(item => item.PdSections)
                    .ThenInclude(item => item.Employee)
                    .FirstOrDefaultAsync(item => item.Id == id);
            var model = new BuildingObjectModel
            {
                Name = el.Name,
                Id = el.Id,
                PdSections = el.PdSections.Select(item => new PdSectionModel
                {
                    Name = item.Name,
                    Id = item.Id,
                    Price = item.Price,
                    EmployeeName = item.Employee.FullName
                })
            };
            return View(model);
        }

        // GET: BuildingObject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuildingObject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BuildingObjectModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.BuildingObjects.AddAsync(new BuildingObject
                {
                    Name = model.Name
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: BuildingObject/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var el = await _context.BuildingObjects.FirstOrDefaultAsync(item => item.Id == id);
            var model = new BuildingObjectModel
            {
                Name = el.Name,
                Id = el.Id
            };
            return View(model);
        }

        // POST: BuildingObject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BuildingObjectModel model)
        {
            if (ModelState.IsValid)
            {
                BuildingObject el = await _context.BuildingObjects.FirstOrDefaultAsync(item => item.Id == model.Id);
                el.Name = model.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
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