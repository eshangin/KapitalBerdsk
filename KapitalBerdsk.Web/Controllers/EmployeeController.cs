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
            var employees = await _context.Employees.Include(item => item.PdSections).ToListAsync();
            var model = employees.Select(item => new EmployeeListItemModel
            {
                FirstName = item.FirstName,
                Id = item.Id,
                LastName = item.LastName,
                Salary = item.Salary,
                Accrued = item.Salary + item.PdSections.Sum(s => s.Price)
            });

            return View(model);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            if (ModelState.IsValid)
            {
                await _context.Employees.AddAsync(new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Salary = model.Salary
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Employee emp = await _context.Employees.FirstOrDefaultAsync(item => item.Id == id);
            var model = new EmployeeModel
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
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
            if (ModelState.IsValid)
            {
                Employee emp = await _context.Employees.FirstOrDefaultAsync(item => item.Id == model.Id);
                emp.FirstName = model.FirstName;
                emp.LastName = model.LastName;
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