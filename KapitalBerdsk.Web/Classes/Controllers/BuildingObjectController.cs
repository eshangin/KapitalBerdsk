using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.BuildingObjects;
using KapitalBerdsk.Web.Classes.Commands.Employees;
using KapitalBerdsk.Web.Classes.Commands.PdSections;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using KapitalBerdsk.Web.Classes.Services;
using MediatR;
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
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediator;

        public BuildingObjectController(
            IDateTimeService dateTimeService,
            IMediator mediator)
        {
            _dateTimeService = dateTimeService;
            _mediator = mediator;
        }

        // GET: BuildingObject
        public async Task<ActionResult> Index()
        {
            IEnumerable<BuildingObjectListItemModel> model = await GetBuildingObjectListItems();

            return View(model);
        }

        private async Task<IEnumerable<BuildingObjectListItemModel>> GetBuildingObjectListItems(int? id = null)
        {
            var buildingObjects = new List<BuildingObject>();

            if (id.HasValue)
            {
                buildingObjects.Add(await _mediator.Send(new GetBuildingObjectByIdQuery(id.Value)
                {
                    IncludeFundsFlows = true,
                    IncludePdSections = true,
                    IncludePdSectionsEmployee = true,
                    IncludeResponsibleEmployee = true
                }));
            }
            else
            {
                buildingObjects.AddRange(await _mediator.Send(new ListBuildingObjectsQuery()
                {
                    IncludeFundsFlows = true,
                    IncludePdSections = true,
                    IncludePdSectionsEmployee = true,
                    IncludeResponsibleEmployee = true,
                    OnlyActive = true
                }));
            }

            return buildingObjects.Select(item => new BuildingObjectListItemModel
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
                ContractDateStart = _dateTimeService.LocalDate,
                IsCreateMode = true,
                Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item =>
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
            if (await _mediator.Send(new GetBuildingObjectByNameQuery(model.Name)) != null)
            {
                ModelState.AddModelError("", "Объект с таким именем уже существует");
            }

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveBuildingObjectCommand()
                {
                    Name = model.Name,
                    Status = model.Status,
                    Price = model.Price.Value,
                    ContractDateStart = model.ContractDateStart.Value,
                    ContractDateEnd = model.ContractDateEnd.Value,
                    ResponsibleEmployeeId = model.ResponsibleEmployeeId,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        // GET: BuildingObject/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            BuildingObject el = await _mediator.Send(new GetBuildingObjectByIdQuery(id));
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
            model.PdSections = (await _mediator.Send(new ListPdSectionsQuery()
            {
                BuildingObjectId = model.Id,
                IncludeEmployee = true
            })).Select(item => new PdSectionModel
            {
                Name = item.Name,
                Id = item.Id,
                Price = item.Price,
                EmployeeId = item.EmployeeId,
                EmployeeName = item.OneTimeEmployeeName ?? item.Employee?.FullName
            });

            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
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
            BuildingObject objectByName = await _mediator.Send(new GetBuildingObjectByNameQuery(model.Name));
            if (objectByName != null && objectByName.Id != model.Id)
            {
                ModelState.AddModelError("", "Объект с таким именем уже существует");
            }

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveBuildingObjectCommand()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Status = model.Status,
                    Price = model.Price.Value,
                    ContractDateStart = model.ContractDateStart.Value,
                    ContractDateEnd = model.ContractDateEnd.Value,
                    ResponsibleEmployeeId = model.ResponsibleEmployeeId,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            await FillRelatedObjects(model);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            await _mediator.Send(new UpdateBuildingObjectsOrderCommand(model.Ids));

            return Ok();
        }
    }
}