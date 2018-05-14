using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
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
                .Include(item => item.PdSections)
                .Include(item => item.FundsFlows)
                .OrderBy(item => item.OrderNumber)
                .ToListAsync();

            var model = from item in employees
                        let accured = item.Salary.ToDecimal() + item.PdSections.Sum(s => s.Price)
                        let outgo = item.FundsFlows.Where(ff => ff.Outgo != null &&
                                                                ff.OrganizationId == null).Sum(ff => ff.Outgo.Value)
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
                .Include(item => item.FundsFlows)
                .ThenInclude(item => item.BuildingObject)
                .Include(item => item.PdSections)
                .ThenInclude(item => item.BuildingObject)
                .FirstOrDefaultAsync(item => item.Id == id);

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
                where ff.Outgo != null
                group ff by ff.BuildingObjectId into g
                select new
                {
                    g.First().BuildingObjectId,
                    Issued = g.Sum(item => item.Outgo.Value)
                }).ToList();

            var combined = new List<EmployeeDetailsModel.BuildingObjectDetail>();
            IEnumerable<BuildingObject> combinedBuildingObjects = emp.FundsFlows.Select(ff => ff.BuildingObject)
                .Union(emp.PdSections.Select(ps => ps.BuildingObject));
            foreach (BuildingObject buildingObject in combinedBuildingObjects)
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

            var model = new EmployeeDetailsModel
            {
                FullName = emp.FullName,
                Salary = emp.Salary.ToDecimal(),
                Id = emp.Id,
                BuildingObjects = combined
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

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            List<Employee> employees = await _context.Employees.ToListAsync();

            for (int i = 0; i < model.Ids.Count(); i++)
            {
                int id = model.Ids.ElementAt(i);
                Employee emp = employees.Find(item => item.Id == id);
                emp.OrderNumber = i;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        public class UpdateOrderModel
        {
            public IEnumerable<int> Ids { get; set; }
        }
    }
}