using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data;
using KapitalBerdsk.Web.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Controllers
{
    public class PdSectionController : Controller
    {
        private ApplicationDbContext _context;

        public PdSectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PdSection/Create
        public async Task<ActionResult> Create(int objectId)
        {
            var model = new EditPdSectionModel
            {
                Employees = (await _context.Employees.ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                    Selected = item.Id == objectId
                }),
                BuildingObjectId = objectId,
                SelectedBuildingObjectId = objectId
            };
            return View(model);
        }

        // POST: PdSection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int objectId, EditPdSectionModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.PdSections.AddAsync(new PdSection
                {
                    Name = model.Name,
                    BuildingObjectId = model.BuildingObjectId.Value,
                    EmployeeId = model.EmployeeId.Value,
                    Price = model.Price.Value
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BuildingObjectController.Details), "BuildingObject", 
                    new { id = model.SelectedBuildingObjectId });
            }

            model.BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == objectId
            });
            model.Employees = (await _context.Employees.ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        // GET: PdSection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PdSection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(BuildingObjectController.Index), nameof(BuildingObjectController));
            }
            catch
            {
                return View();
            }
        }
    }
}