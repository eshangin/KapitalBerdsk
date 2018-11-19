using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.Employees;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            var employees = await _mediator.Send(new ListEmployeesQuery()
            {
                IncludeEmployeePayrolls = true,
                IncludeFundsFlows = true,
                IncludePdSections = true,
                OnlyActive = true
            });

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
            Employee emp = await _mediator.Send(new GetEmployeeByIdQuery(id)
            {
                IncludeEmployeePayrolls = true,
                IncludeFundsFlows = true,
                IncludeFundsFlowsBuildingObject = true,
                IncludePdSections = true,
                IncludePdSectionsBuildingObject = true
            });

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
            if (await _mediator.Send(new GetEmployeeByNameQuery(model.FullName)) != null)
            {
                ModelState.AddModelError("", "Сотрудник с таким ФИО уже есть в системе");
            }

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveEmployeeCommand()
                {
                    FullName = model.FullName,
                    Salary = model.Salary,
                    Email = model.Email,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Employee emp = await _mediator.Send(new GetEmployeeByIdQuery(id));
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
            var objectByName = await _mediator.Send(new GetEmployeeByNameQuery(model.FullName));
            if (objectByName != null && objectByName.Id != model.Id)
            {
                ModelState.AddModelError("", "Сотрудник с таким ФИО уже есть в системе");
            }

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveEmployeeCommand()
                {
                    Id = model.Id,
                    FullName = model.FullName,
                    Salary = model.Salary,
                    Email = model.Email,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            await _mediator.Send(new UpdateEmployeesOrderCommand(model.Ids));

            return Ok();
        }
    }
}