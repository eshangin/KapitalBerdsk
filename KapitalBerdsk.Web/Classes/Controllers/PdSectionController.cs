using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.BuildingObjects;
using KapitalBerdsk.Web.Classes.Commands.Common;
using KapitalBerdsk.Web.Classes.Commands.Employees;
using KapitalBerdsk.Web.Classes.Commands.PdSections;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Extensions;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class PdSectionController : Controller
    {
        private readonly IMediator _mediator;

        public PdSectionController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: PdSection/Create
        public async Task<ActionResult> Create(int objectId)
        {
            var model = new EditPdSectionModel
            {
                Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                    Selected = item.Id == objectId
                }),
                BuildingObjectId = objectId,
                SelectedBuildingObjectId = objectId,
                IsCreateMode = true
            };
            return View(model);
        }

        // POST: PdSection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int objectId, EditPdSectionModel model)
        {
            model.VerifyEmployeeSelection(ModelState);

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SavePdSectionCommand()
                {
                    BuildingObjectId = model.BuildingObjectId.Value,
                    EmployeeId = model.EmployeeId,
                    Name = model.Name,
                    OneTimeEmployeeName = model.OneTimeEmployeeName,
                    Price = model.Price.Value,
                    UseOneTimeEmployee = model.UseOneTimeEmployee
                });

                return RedirectToAction(nameof(BuildingObjectController.Details), "BuildingObject", 
                    new { id = model.SelectedBuildingObjectId });
            }

            model.BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == objectId
            });
            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        // GET: PdSection/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            PdSection pdSection = await _mediator.Send(new GetPdSectionByIdQuery(id));
            var model = new EditPdSectionModel
            {
                Id = pdSection.Id,
                Name = pdSection.Name,
                Price = pdSection.Price,
                EmployeeId = pdSection.EmployeeId,
                OneTimeEmployeeName = pdSection.OneTimeEmployeeName,
                UseOneTimeEmployee = !string.IsNullOrWhiteSpace(pdSection.OneTimeEmployeeName),
                Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                    Selected = item.Id == pdSection.BuildingObjectId
                }),
                BuildingObjectId = pdSection.BuildingObjectId,
                SelectedBuildingObjectId = pdSection.BuildingObjectId
            };
            return View(model);
        }

        // POST: PdSection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPdSectionModel model)
        {
            model.VerifyEmployeeSelection(ModelState);

            if (ModelState.IsValid)
            {
                await _mediator.Send(new SavePdSectionCommand()
                {
                    Id = model.Id,
                    BuildingObjectId = model.BuildingObjectId.Value,
                    EmployeeId = model.EmployeeId,
                    Name = model.Name,
                    OneTimeEmployeeName = model.OneTimeEmployeeName,
                    Price = model.Price.Value,
                    UseOneTimeEmployee = model.UseOneTimeEmployee
                });

                return RedirectToAction(nameof(BuildingObjectController.Details), "BuildingObject",
                    new { id = model.SelectedBuildingObjectId });
            }

            model.BuildingObjects = (await _mediator.Send(new ListBuildingObjectsQuery())).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == model.SelectedBuildingObjectId
            });
            model.Employees = (await _mediator.Send(new ListEmployeesQuery())).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            PdSectionModel model = null;
            PdSection pdSection = await _mediator.Send(new GetPdSectionByIdQuery(id)
            {
                IncludeEmployee = true
            });

            if (pdSection != null)
            {
                model = new PdSectionModel
                {
                    Name = pdSection.Name,
                    Id = pdSection.Id,
                    Price = pdSection.Price,
                    EmployeeName = pdSection.OneTimeEmployeeName ?? pdSection.Employee.FullName
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            await _mediator.Send(new DetetePdSectionCommand(id));

            return RedirectToAction(nameof(BuildingObjectController.Index), nameof(BuildingObjectController).Replace("Controller", string.Empty));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            await _mediator.Send(new UpdatePdSectionsOrderCommand(model.Ids));

            return Ok();
        }
    }
}