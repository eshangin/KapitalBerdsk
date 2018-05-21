using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Extensions;
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
            IQueryable<FundsFlow> query = from ff in _context.FundsFlows
                                                        .Include(item => item.Employee)
                                                        .Include(item => item.BuildingObject)
                                                        .Include(item => item.Organization)
                                          orderby ff.Date descending, ff.Id descending
                                          select ff;

            if (!User.IsInRole(Constants.Roles.Admin))
            {
                string userId = GetCurrentUserId();
                query = query.Where(ff => ff.CreatedById == userId);
            }

            var items = (await query.ToListAsync()).Select(item => new FundsFlowListItemModel
                {
                    Date = item.Date,
                    Description = item.Description,
                    Income = item.Income,
                    Outgo = item.Outgo,
                    OutgoType = item.OutgoType,
                    PayType = item.PayType,
                    Id = item.Id,
                    EmployeeName = item.Employee?.FullName ?? item.OneTimeEmployeeName,
                    EmployeeId = item.EmployeeId,
                    OrganizationName = item.Organization?.Name,
                    OrganizationId = item.OrganizationId,
                    BuildingObjectName = item.BuildingObject?.Name,
                    BuildingObjectId = item.BuildingObjectId
                });

            var model = new FundsFlowListModel
            {
                Items = items,
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.Name
                }),
                Organizations = (await _context.Organizations.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                })
            };

            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => item.FullName)
                .Union(_context.FundsFlows.Where(ff => !string.IsNullOrWhiteSpace(ff.OneTimeEmployeeName)).Select(ff => ff.OneTimeEmployeeName)).Select(employeeName =>
                    new SelectListItem
                    {
                        Text = employeeName
                    });

            return View(model);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                Organizations = (await _context.Organizations.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
                Date = DateTime.UtcNow.AddHours(7),
                IsCreateMode = true
            };
            return View(model);
        }

        // POST: FundsFlow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditFundsFlowModel model)
        {
            model.VerifyEmployeeSelection(ModelState);

            if (model.Income == null && model.Outgo == null)
            {
                ModelState.AddModelError("", "Приход или расход должны быть указаны");
            }
            if (string.IsNullOrWhiteSpace(model.OneTimeEmployeeName) && model.EmployeeId == null && model.OrganizationId == null)
            {
                ModelState.AddModelError("", "Сотрудник и/или организация должны быть указаны");
            }

            if (ModelState.IsValid)
            {
                var ff = new FundsFlow();
                UpdateValues(ff, model);
                await _context.FundsFlows.AddAsync(ff);
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
            model.Organizations = (await _context.Organizations.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            return View(model);
        }

        private void UpdateValues(FundsFlow ff, EditFundsFlowModel model)
        {
            ff.Date = model.Date.Value;
            ff.BuildingObjectId = model.BuildingObjectId;
            ff.Description = model.Description;
            ff.OrganizationId = model.OrganizationId;
            ff.Income = model.Income;
            ff.Outgo = model.Outgo;
            ff.OutgoType = model.OutgoType;
            ff.PayType = model.PayType;
            ff.SetEmployee(model.UseOneTimeEmployee, model.EmployeeId, model.OneTimeEmployeeName);
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
                OneTimeEmployeeName = ff.OneTimeEmployeeName,
                UseOneTimeEmployee = !string.IsNullOrWhiteSpace(ff.OneTimeEmployeeName),
                OrganizationId = ff.OrganizationId,
                Income = ff.Income,
                Outgo = ff.Outgo,
                OutgoType = ff.OutgoType,
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
                }),
                Organizations = (await _context.Organizations.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFundsFlowModel model)
        {
            model.VerifyEmployeeSelection(ModelState);

            if (model.Income == null && model.Outgo == null)
            {
                ModelState.AddModelError("", "Приход или расход должны быть указаны");
            }
            if (string.IsNullOrWhiteSpace(model.OneTimeEmployeeName) && model.EmployeeId == null && model.OrganizationId == null)
            {
                ModelState.AddModelError("", "Сотрудник и/или организация должны быть указаны");
            }

            if (ModelState.IsValid)
            {
                FundsFlow ff = await _context.FundsFlows.FirstOrDefaultAsync(item => item.Id == model.Id);
                UpdateValues(ff, model);
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
            model.Organizations = (await _context.Organizations.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.FundsFlows
                .Include(item => item.Employee)
                .Include(item => item.BuildingObject)
                .Include(item => item.Organization)
                .OrderByDescending(item => item.Date)
                .ThenByDescending(item => item.Id)
                .Where(item => item.Id == id)
                .Select(item => new FundsFlowListItemModel
                {
                    Date = item.Date,
                    Description = item.Description,
                    Income = item.Income,
                    Outgo = item.Outgo,
                    PayType = item.PayType,
                    Id = item.Id,
                    EmployeeName = item.Employee == null ? item.OneTimeEmployeeName : item.Employee.FullName,
                    EmployeeId = item.EmployeeId,
                    OrganizationName = item.Organization == null ? null : item.Organization.Name,
                    OrganizationId = item.OrganizationId,
                    BuildingObjectName = item.BuildingObject == null ? null : item.BuildingObject.Name,
                    BuildingObjectId = item.BuildingObjectId
                }).FirstOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var itemToDelete = new FundsFlow() { Id = id };
            _context.Entry(itemToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}