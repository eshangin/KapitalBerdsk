using System;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class FundsFlowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FundsFlowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FundsFlow
        public async Task<ActionResult> Index()
        {
            var items = (await _context.FundsFlows
                .Include(item => item.Employee)
                .Include(item => item.BuildingObject)
                .OrderByDescending(item => item.Date)
                .ThenByDescending(item => item.Id)
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

            var model = new FundsFlowListModel
            {
                Items = items,
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item =>
                    new SelectListItem
                    {
                        Text = item.FullName,
                        Value = item.Id.ToString()
                    }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                })
            };


            return View(model);
        }

        // GET: FundsFlow/Create
        public async Task<ActionResult> Create()
        {
            var model = new EditFundsFlowModel
            {
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
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

            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
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

        public async Task<ActionResult> Edit(int id)
        {
            FundsFlow ff = await _context.FundsFlows.FirstOrDefaultAsync(item => item.Id == id);
            var model = new EditFundsFlowModel
            {
                Id = ff.Id,
                BuildingObjectId = ff.BuildingObjectId,
                Date = ff.Date,
                Description = ff.Description,
                EmployeeId = ff.EmployeeId,
                Income = ff.Income,
                Outgo = ff.Outgo,
                PayType = ff.PayType,
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                })
            };

            return View(model);
        }

        //// POST: Employee/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(EmployeeModel model)
        //{
        //    var objectByName = await GetEmployeeByName(model.FullName);
        //    if (objectByName != null && objectByName.Id != model.Id)
        //    {
        //        ModelState.AddModelError("", "Сотрудник с таким ФИО уже есть в системе");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        Employee emp = await _context.Employees.FirstOrDefaultAsync(item => item.Id == model.Id);
        //        emp.FullName = model.FullName;
        //        emp.Salary = model.Salary;
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View();
        //}
    }
}