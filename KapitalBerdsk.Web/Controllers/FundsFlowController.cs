using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data;
using KapitalBerdsk.Web.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Controllers
{
    [Authorize]
    public class FundsFlowController : Controller
    {
        private ApplicationDbContext _context;

        public FundsFlowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FundsFlow
        public async Task<ActionResult> Index()
        {
            var model = (await _context.FundsFlows
                .Include(item => item.Employee)
                .Include(item => item.BuildingObject)
                .OrderByDescending(item => item.Id)
                .ToListAsync()).Select(item => new FundsFlowListItemModel
            {
                Date = item.Date,
                Description = item.Description,
                Income = item.Income,
                Outgo = item.Outgo,
                PayType = item.PayType,
                Id = item.Id,
                EmployeeName = item.Employee.FullName,
                EmployeeId = item.EmployeeId,
                BuildingObjectName = item.BuildingObject.Name
            });

            return View(model);
        }

        // GET: FundsFlow/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FundsFlow/Create
        public async Task<ActionResult> Create()
        {
            var model = new EditFundsFlowModel
            {
                Employees = (await _context.Employees.ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
                Date = DateTime.UtcNow.AddHours(7)
            };
            return View(model);
        }

        // POST: FundsFlow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditFundsFlowModel model)
        {
            if (model.Income == null && model.Outgo == null)
            {
                ModelState.AddModelError("", "Приход или расход должны быть указаны");
            }

            if (ModelState.IsValid)
            {
                await _context.FundsFlows.AddAsync(new FundsFlow
                {
                    BuildingObjectId = model.BuildingObjectId.Value,
                    Date = model.Date.Value,
                    Description = model.Description,
                    EmployeeId = model.EmployeeId.Value,
                    PayType = model.PayType,
                    Income = model.Income,
                    Outgo = model.Outgo
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.Employees = (await _context.Employees.ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });
            model.BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            return View(model);
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