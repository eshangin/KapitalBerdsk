using System;
using System.Collections.Generic;
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
    public class BuildingObjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildingObject
        public async Task<ActionResult> Index()
        {
            IEnumerable<BuildingObjectListItemModel> model = await GetBuildingObjectListItems();

            return View(model);
        }

        private async Task<IEnumerable<BuildingObjectListItemModel>> GetBuildingObjectListItems(int? id = null)
        {
            IQueryable<BuildingObject> query = _context.BuildingObjects
                .Include(item => item.PdSections)
                .ThenInclude(item => item.Employee)
                .Include(item => item.FundsFlows)
                .Include(item => item.ResponsibleEmployee)
                .OrderByDescending(item => item.Id);

            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id);
            }

            return (await query.ToListAsync()).Select(item => new BuildingObjectListItemModel
            {
                Id = item.Id,
                Name = item.Name,
                PdSections = item.PdSections.Select(ps => new PdSectionModel
                {
                    Name = ps.Name,
                    Id = ps.Id,
                    Price = ps.Price,
                    EmployeeId = ps.EmployeeId,
                    EmployeeName = ps.EmployeeId.HasValue ? ps.Employee.FullName : ps.OneTimeEmployeeName
                }),
                ContractDateStart = item.ContractDateStart,
                ContractDateEnd = item.ContractDateEnd,
                CostPrice = item.PdSections.Sum(ps => ps.Price),
                Price = item.Price,
                RealPrice = item.FundsFlows.Where(ff => ff.Outgo.HasValue).Sum(ff => ff.Outgo.Value),
                PaidByCustomer = item.FundsFlows.Where(ff => ff.Income.HasValue).Sum(ff => ff.Income.Value),
                Status = item.Status,
                ResponsibleEmployeeId = item.ResponsibleEmployeeId,
                ResponsibleEmployeeName = item.ResponsibleEmployee?.FullName
            });
        }

        // GET: BuildingObject/Details/5
        public async Task<ActionResult> Details(int id)
        {
            BuildingObjectListItemModel model = (await GetBuildingObjectListItems(id)).Single();

            return View(model);
        }

        // GET: BuildingObject/Create
        public async Task<ActionResult> Create()
        {
            var model = new BuildingObjectModel()
            {
                ContractDateStart = DateTime.UtcNow.AddHours(7),
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item =>
                    new SelectListItem
                    {
                        Text = item.FullName,
                        Value = item.Id.ToString()
                    })
            };

            return View(model);
        }

        // POST: BuildingObject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BuildingObjectModel model)
        {
            if ((await GetBuildingObjectByName(model.Name)) != null)
            {
                ModelState.AddModelError("", "Объект с таким именем уже существует");
            }

            if (ModelState.IsValid)
            {
                await _context.BuildingObjects.AddAsync(new BuildingObject
                {
                    Name = model.Name,
                    ContractDateEnd = model.ContractDateEnd.Value,
                    ContractDateStart = model.ContractDateStart.Value,
                    Status = model.Status,
                    Price = model.Price.Value,
                    ResponsibleEmployeeId = model.ResponsibleEmployeeId
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        private async Task<BuildingObject> GetBuildingObjectByName(string buildingObjectName)
        {
            return await _context.BuildingObjects.FirstOrDefaultAsync(item => item.Name == buildingObjectName);
        }

        // GET: BuildingObject/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var el = await _context.BuildingObjects
                .Include(item => item.PdSections)
                .ThenInclude(item => item.Employee)
                .FirstOrDefaultAsync(item => item.Id == id);
            var model = new BuildingObjectModel
            {
                Name = el.Name,
                ContractDateEnd = el.ContractDateEnd,
                ContractDateStart = el.ContractDateStart,
                Price = el.Price,
                Status = el.Status,
                Id = el.Id,
                PdSections = el.PdSections.Select(item => new PdSectionModel
                {
                    Name = item.Name,
                    Id = item.Id,
                    Price = item.Price,
                    EmployeeId = item.EmployeeId,
                    EmployeeName = item.Employee.FullName
                }),
                ResponsibleEmployeeId = el.ResponsibleEmployeeId,
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item =>
                    new SelectListItem
                    {
                        Text = item.FullName,
                        Value = item.Id.ToString()
                    })
            };
            return View(model);
        }

        // POST: BuildingObject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BuildingObjectModel model)
        {
            BuildingObject objectByName = await GetBuildingObjectByName(model.Name);
            if (objectByName != null && objectByName.Id != model.Id)
            {
                ModelState.AddModelError("", "Объект с таким именем уже существует");
            }

            if (ModelState.IsValid)
            {
                BuildingObject el = await _context.BuildingObjects.FirstOrDefaultAsync(item => item.Id == model.Id);
                el.Name = model.Name;
                el.Status = model.Status;
                el.Price = model.Price.Value;
                el.ContractDateStart = model.ContractDateStart.Value;
                el.ContractDateEnd = model.ContractDateEnd.Value;
                el.ResponsibleEmployeeId = model.ResponsibleEmployeeId;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.PdSections = (await _context.PdSections.Include(item => item.Employee).Where(item => item.BuildingObjectId == model.Id).ToListAsync())
                .Select(item => new PdSectionModel
                {
                    Name = item.Name,
                    Id = item.Id,
                    Price = item.Price,
                    EmployeeName = item.Employee.FullName
                });

            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }
    }
}