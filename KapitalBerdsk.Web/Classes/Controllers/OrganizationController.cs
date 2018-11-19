using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.Organizations;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IMediator _mediator;

        public OrganizationController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<OrganizationListItemModel> model = await GetListItems();

            return View(model);
        }

        private async Task<IEnumerable<OrganizationListItemModel>> GetListItems(int? id = null)
        {
            var orgs = new List<Organization>();

            if (id.HasValue)
            {
                orgs.Add(await _mediator.Send(new GetOrganizationByIdQuery(id.Value)
                {
                    IncludeFundsFlows = true
                }));
            }
            else
            {
                orgs.AddRange(await _mediator.Send(new ListOrganizationsQuery()
                {
                    IncludeFundsFlows = true,
                    OnlyActive = true
                }));
            }

            var model = orgs.Select(item => new OrganizationListItemModel
            {
                Name = item.Name,
                Id = item.Id,
                Income = item.FundsFlows.Sum(ff => ff.Income ?? 0),
                Outgo = item.FundsFlows.Sum(ff => ff.Outgo ?? 0)
            });

            return model;
        }

        public async Task<ActionResult> Details(int id)
        {
            OrganizationListItemModel model = (await GetListItems(id)).First();

            return View(model);
        }

        public ActionResult Create()
        {
            var model = new OrganizationModel()
            {
                IsCreateMode = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationModel model)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveOrganizationCommand()
                {
                    Name = model.Name,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private void UpdateValues(Organization entity, OrganizationModel model)
        {
            entity.Name = model.Name;
            entity.IsInactive = model.IsInactive;
        }

        public async Task<ActionResult> Edit(int id)
        {
            var el = await _mediator.Send(new GetOrganizationByIdQuery(id));
            var model = new OrganizationModel()
            {
                Name = el.Name,
                Id = el.Id,
                IsInactive = el.IsInactive
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrganizationModel model)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new SaveOrganizationCommand()
                {
                    Id = model.Id,
                    Name = model.Name,
                    IsInactive = model.IsInactive
                });

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}