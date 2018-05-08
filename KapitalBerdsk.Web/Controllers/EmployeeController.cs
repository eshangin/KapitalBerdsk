using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data;
using KapitalBerdsk.Web.Extensions;
using KapitalBerdsk.Web.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(item => item.PdSections)
                .Include(item => item.FundsFlows)
                .ToListAsync();

            var model = from item in employees
                        let accured = item.Salary.ToDecimal() + item.PdSections.Sum(s => s.Price)
                        let outgo = item.FundsFlows.Where(ff => ff.Outgo.HasValue).Sum(ff => ff.Outgo.Value)
                        select new EmployeeListItemModel
                        {
                            FullName = item.FullName,
                            Id = item.Id,
                            Salary = item.Salary.ToDecimal(),
                            Accrued = accured,
                            Balance = accured - outgo
                        };

            return View(model);
        }

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Employee emp = await _context.Employees
                .Include(item => item.PdSections)
                .ThenInclude(item => item.BuildingObject)
                .FirstOrDefaultAsync(item => item.Id == id);

            var model = new EmployeeDetailsModel
            {
                FullName = emp.FullName,
                Salary = emp.Salary.ToDecimal(),
                Id = emp.Id,
                BuildingObjects = emp.PdSections.GroupBy(item => item.BuildingObjectId)
                                        .Select(item => new EmployeeDetailsModel.BuildingObjectDetail
                                        {
                                            Name = item.First().BuildingObject.Name,
                                            Id = item.First().BuildingObject.Id,
                                            Total = item.Sum(pd => pd.Price)
                                        })
            };

            return View(model);
        }

        // GET: Employee/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeModel model)
        {
            if ((await GetEmployeeByName(model.FullName)) != null)
            {
                ModelState.AddModelError("", "Сотрудник с таким ФИО уже есть в системе");
            }

            if (ModelState.IsValid)
            {
                await _context.Employees.AddAsync(new Employee
                {
                    FullName = model.FullName,
                    Salary = model.Salary
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        private async Task<Employee> GetEmployeeByName(string name)
        {
            return await _context.Employees.FirstOrDefaultAsync(item => item.FullName == name);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Employee emp = await _context.Employees.FirstOrDefaultAsync(item => item.Id == id);
            var model = new EmployeeModel
            {
                FullName = emp.FullName,
                Salary = emp.Salary,
                Id = emp.Id
            };
            return View(model);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeModel model)
        {
            var objectByName = await GetEmployeeByName(model.FullName);
            if (objectByName != null && objectByName.Id != model.Id)
            {
                ModelState.AddModelError("", "Сотрудник с таким ФИО уже есть в системе");
            }

            if (ModelState.IsValid)
            {
                Employee emp = await _context.Employees.FirstOrDefaultAsync(item => item.Id == model.Id);
                emp.FullName = model.FullName;
                emp.Salary = model.Salary;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
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