using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.BuildingObjects;
using KapitalBerdsk.Web.Classes.Commands.Employees;
using KapitalBerdsk.Web.Classes.Commands.FundsFlows;
using KapitalBerdsk.Web.Classes.Commands.Organizations;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using KapitalBerdsk.Web.Classes.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class FundsFlowController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediator;

        public FundsFlowController(
            IDateTimeService dateTimeService,
            IMediator mediator)
        {
            _dateTimeService = dateTimeService;
            _mediator = mediator;
        }

        // GET: FundsFlow
        public async Task<ActionResult> Index()
        {
            var flows = await _mediator.Send(new ListFundsFlowsQuery()
            {
                UserId = User.IsInRole(Constants.Roles.Admin) ? null : GetCurrentUserId(),
                IncludeBuildingObject = true,
                IncludeOrganization = true,
                IncludeEmployee = true
            });

            var items = flows.Select(item => new FundsFlowListItemModel
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
                BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
                {
                    Text = item.Name
                }),
                Organizations = (await _mediator.Send(new ListOrganizationsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                })
            };

            var oneTimeEmployees = await _mediator.Send(new ListOneTimeEmployeesQuery());

            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => item.FullName)
                .Union(oneTimeEmployees).Select(employeeName =>
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
                Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
                Organizations = (await _mediator.Send(new ListOrganizationsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
                Date = _dateTimeService.LocalDate,
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
                await _mediator.Send(new SaveFundsFlowCommand()
                {
                    Date = model.Date.Value,
                    BuildingObjectId = model.BuildingObjectId,
                    Description = model.Description,
                    OrganizationId = model.OrganizationId,
                    Income = model.Income,
                    Outgo = model.Outgo,
                    OutgoType = model.OutgoType,
                    PayType = model.PayType,
                    UseOneTimeEmployee = model.UseOneTimeEmployee,
                    EmployeeId = model.EmployeeId,
                    OneTimeEmployeeName = model.OneTimeEmployeeName
                });

                return RedirectToAction(nameof(Index));
            }

            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });
            model.BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });
            model.Organizations = (await _mediator.Send(new ListOrganizationsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            FundsFlow ff = await _mediator.Send(new GetFundsFlowByIdQuery(id));
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
                Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }),
                Organizations = (await _mediator.Send(new ListOrganizationsQuery())).Select(item => new SelectListItem
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
                await _mediator.Send(new SaveFundsFlowCommand()
                {
                    Id = model.Id,
                    Date = model.Date.Value,
                    BuildingObjectId = model.BuildingObjectId,
                    Description = model.Description,
                    OrganizationId = model.OrganizationId,
                    Income = model.Income,
                    Outgo = model.Outgo,
                    OutgoType = model.OutgoType,
                    PayType = model.PayType,
                    UseOneTimeEmployee = model.UseOneTimeEmployee,
                    EmployeeId = model.EmployeeId,
                    OneTimeEmployeeName = model.OneTimeEmployeeName
                });

                return RedirectToAction(nameof(Index));
            }

            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });
            model.BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });
            model.Organizations = (await _mediator.Send(new ListOrganizationsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            FundsFlow flow = await _mediator.Send(new GetFundsFlowByIdQuery(id)
            {
                IncludeBuildingObject = true,
                IncludeEmployee = true,
                IncludeOrganization = true
            });

            FundsFlowListItemModel model = null;

            if (flow != null)
            {
                model = new FundsFlowListItemModel()
                {
                    Date = flow.Date,
                    Description = flow.Description,
                    Income = flow.Income,
                    Outgo = flow.Outgo,
                    PayType = flow.PayType,
                    Id = flow.Id,
                    EmployeeName = flow.Employee == null ? flow.OneTimeEmployeeName : flow.Employee.FullName,
                    EmployeeId = flow.EmployeeId,
                    OrganizationName = flow.Organization == null ? null : flow.Organization.Name,
                    OrganizationId = flow.OrganizationId,
                    BuildingObjectName = flow.BuildingObject == null ? null : flow.BuildingObject.Name,
                    BuildingObjectId = flow.BuildingObjectId
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            await _mediator.Send(new DeteteFundsFlowCommand(id));

            return RedirectToAction(nameof(Index));
        }
    }
}