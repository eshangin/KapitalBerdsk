﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            var employees = await _context.Employees
                .OnlyActive()
                .Include(item => item.EmployeePayrolls)
                .Include(item => item.PdSections)
                .Include(item => item.FundsFlows)
                .OrderBy(item => item.OrderNumber)
                .ThenByDescending(item => item.Id)
                .ToListAsync();

            var model = from item in employees
                        let accured = item.EmployeePayrolls.Sum(ep => ep.Value) + item.PdSections.Sum(s => s.Price)
                        let outgo = item.FundsFlows.Where(ff => ff.Outgo.HasValue &&
                                                                ff.OutgoType == OutgoType.Regular &&
                                                                ff.OrganizationId == null).Sum(ff => ff.Outgo.Value)
                        let accountable = item.FundsFlows.Where(ff => ff.Outgo.HasValue && ff.OutgoType == OutgoType.Accountable).Sum(ff => ff.Outgo.Value)
                        let writeOffAccountable = item.FundsFlows.Where(ff => ff.Outgo.HasValue && ff.OutgoType == OutgoType.WriteOffAccountable).Sum(ff => ff.Outgo.Value)
                        select new EmployeeListItemModel
                        {
                            FullName = item.FullName,
                            Id = item.Id,
                            Salary = item.Salary.ToDecimal(),
                            Accrued = accured,
                            Balance = accured - outgo,
                            AccountableBalance = accountable - writeOffAccountable,
                            Email = item.Email
                        };

            return View(model);
        }

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Employee emp = await _context.Employees
                .Include(item => item.EmployeePayrolls)
                .Include(item => item.FundsFlows)
                .ThenInclude(item => item.BuildingObject)
                .Include(item => item.PdSections)
                .ThenInclude(item => item.BuildingObject)
                .FirstOrDefaultAsync(item => item.Id == id);

            var model = new EmployeeDetailsModel
            {
                FullName = emp.FullName,
                Email = emp.Email,
                Salary = emp.Salary.ToDecimal(),
                AccountableBalance = emp.FundsFlows.Where(ff => ff.Outgo.HasValue && ff.OutgoType == OutgoType.Accountable).Sum(ff => ff.Outgo.Value) -
                                    emp.FundsFlows.Where(ff => ff.Outgo.HasValue && ff.OutgoType == OutgoType.WriteOffAccountable).Sum(ff => ff.Outgo.Value),
                Id = emp.Id,
                BuildingObjects = GetEmployeeBuildingObjectDetails(emp),
                MonthlyEmployeePayrolls = GetEmployeeMonthlyPayrolls(emp)
            };

            return View(model);
        }

        private IEnumerable<EmployeeDetailsModel.BuildingObjectDetail> GetEmployeeBuildingObjectDetails(Employee emp)
        {
            var accuredItems = (
                from pd in emp.PdSections
                group pd by pd.BuildingObjectId
                into item
                select new
                {
                    item.First().BuildingObjectId,
                    Accrued = item.Sum(pd => pd.Price)
                }).ToList();

            var issuedItems = (
                from ff in emp.FundsFlows
                where ff.Outgo != null &&
                        ff.OutgoType == OutgoType.Regular
                group ff by ff.BuildingObjectId into g
                select new
                {
                    g.First().BuildingObjectId,
                    Issued = g.Sum(item => item.Outgo.Value)
                }).ToList();

            var combined = new List<EmployeeDetailsModel.BuildingObjectDetail>();
            IEnumerable<BuildingObject> combinedBuildingObjects = emp.FundsFlows.Where(ff => ff.BuildingObject != null).Select(ff => ff.BuildingObject)
                .Union(emp.PdSections.Select(ps => ps.BuildingObject));
            foreach (BuildingObject buildingObject in combinedBuildingObjects.ApplyOrder())
            {
                decimal issued = issuedItems.FirstOrDefault(item => item.BuildingObjectId == buildingObject.Id)?.Issued ?? 0;
                decimal accured = accuredItems.FirstOrDefault(item => item.BuildingObjectId == buildingObject.Id)?.Accrued ?? 0;
                combined.Add(new EmployeeDetailsModel.BuildingObjectDetail()
                {
                    Name = buildingObject.Name,
                    Id = buildingObject.Id,
                    Issued = issued,
                    Accrued = accured
                });
            }

            return combined;
        }

        private IEnumerable<MonthlyEmployeePayrollModel> GetEmployeeMonthlyPayrolls(Employee emp)
        {
            var accruedPayrolls = emp.EmployeePayrolls.Select(item => new
            {
                accrued = item.Value,
                year = item.Year,
                month = item.Month
            }).ToList();

            var issuedPayrolls = (from ff in emp.FundsFlows
                                  where ff.OutgoType == OutgoType.Salary && ff.Outgo.HasValue
                                  group ff by new { ff.Date.Year, ff.Date.Month } into g
                                  let first = g.First()
                                  select new
                                  {
                                      issued = g.Sum(el => el.Outgo.Value),
                                      year = first.Date.Year,
                                      month = first.Date.Month
                                  }).ToList();

            var payrollPeriods = accruedPayrolls.Select(item => new { item.year, item.month })
                .Union(issuedPayrolls.Select(item => new { item.year, item.month }));

            var monthlyPayrolls = new List<MonthlyEmployeePayrollModel>();
            foreach (var period in payrollPeriods)
            {
                decimal accrued = accruedPayrolls.FirstOrDefault(item => item.year == period.year && item.month == period.month)?.accrued ?? 0;
                decimal issued = issuedPayrolls.FirstOrDefault(item => item.year == period.year && item.month == period.month)?.issued ?? 0;
                monthlyPayrolls.Add(new MonthlyEmployeePayrollModel
                {
                    Year = period.year,
                    Month = period.month,
                    Accrued = accrued,
                    Issued = issued
                });
            }

            return monthlyPayrolls;
        }

        // GET: Employee/Create
        public async Task<ActionResult> Create()
        {
            var model = new EmployeeModel()
            {
                IsCreateMode = true
            };

            return View(model);
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
                var emp = new Employee();
                UpdateValues(emp, model);
                await _context.Employees.AddAsync(emp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        private void UpdateValues(Employee emp, EmployeeModel model)
        {
            emp.FullName = model.FullName;
            emp.Salary = model.Salary;
            emp.Email = model.Email;
            emp.IsInactive = model.IsInactive;
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
                Email = emp.Email,
                Id = emp.Id,
                IsInactive = emp.IsInactive
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
                UpdateValues(emp, model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            List<Employee> items = await _context.Employees.ToListAsync();

            items.UpdateOrder(model.Ids);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}