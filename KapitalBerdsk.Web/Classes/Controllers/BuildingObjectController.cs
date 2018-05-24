using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
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
                .ApplyOrder();

            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id);
            }
            else
            {
                query = query.OnlyActive();
            }

            return (await query.ToListAsync()).Select(item => new BuildingObjectListItemModel
            {
                Id = item.Id,
                Name = item.Name,
                PdSections = item.PdSections.ApplyOrder().Select(ps => new PdSectionModel
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
                IsCreateMode = true,
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
                var bo = new BuildingObject();
                UpdateValues(bo, model);
                await _context.BuildingObjects.AddAsync(bo);
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

        private void UpdateValues(BuildingObject entity, BuildingObjectModel model)
        {
            entity.Name = model.Name;
            entity.Status = model.Status;
            entity.Price = model.Price.Value;
            entity.ContractDateStart = model.ContractDateStart.Value;
            entity.ContractDateEnd = model.ContractDateEnd.Value;
            entity.ResponsibleEmployeeId = model.ResponsibleEmployeeId;
            entity.IsInactive = model.IsInactive;
        }

        private async Task<BuildingObject> GetBuildingObjectByName(string buildingObjectName)
        {
            return await _context.BuildingObjects.FirstOrDefaultAsync(item => item.Name == buildingObjectName);
        }

        // GET: BuildingObject/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var el = await _context.BuildingObjects
                .FirstOrDefaultAsync(item => item.Id == id);
            var model = new BuildingObjectModel
            {
                Name = el.Name,
                ContractDateEnd = el.ContractDateEnd,
                ContractDateStart = el.ContractDateStart,
                Price = el.Price,
                Status = el.Status,
                Id = el.Id,
                ResponsibleEmployeeId = el.ResponsibleEmployeeId,
                IsInactive = el.IsInactive
            };

            await FillRelatedObjects(model);

            return View(model);
        }

        private async Task FillRelatedObjects(BuildingObjectModel model)
        {
            model.PdSections = (await _context.PdSections.Include(item => item.Employee)
                                        .ApplyOrder()
                                        .Where(item => item.BuildingObjectId == model.Id)
                                        .ToListAsync())
                .Select(item => new PdSectionModel
                {
                    Name = item.Name,
                    Id = item.Id,
                    Price = item.Price,
                    EmployeeId = item.EmployeeId,
                    EmployeeName = item.OneTimeEmployeeName ?? item.Employee?.FullName
                });

            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });
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
                UpdateValues(el, model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await FillRelatedObjects(model);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            List<BuildingObject> items = await _context.BuildingObjects.ToListAsync();

            items.UpdateOrder(model.Ids);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}