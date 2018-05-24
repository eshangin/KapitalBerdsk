using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<OrganizationListItemModel> model = await GetListItems();

            return View(model);
        }

        private async Task<IEnumerable<OrganizationListItemModel>> GetListItems(int? id = null)
        {
            var query = _context.Organizations.Include(item => item.FundsFlows).Select(item => item);

            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            else
            {
                query = query.OnlyActive();
            }

            var model = (await query.ToListAsync()).Select(item => new OrganizationListItemModel
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
                var org = new Organization();
                UpdateValues(org, model);
                await _context.Organizations.AddAsync(org);
                await _context.SaveChangesAsync();

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
            var el = await _context.Organizations
                .FirstOrDefaultAsync(item => item.Id == id);
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
                var el = await _context.Organizations.FirstOrDefaultAsync(item => item.Id == model.Id);
                UpdateValues(el, model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}