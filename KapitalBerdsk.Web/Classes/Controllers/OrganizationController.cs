using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
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
            var model = (await _context.Organizations.ToListAsync()).Select(item => new OrganizationModel
            {
                Name = item.Name,
                Id = item.Id
            });

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.Organizations.AddAsync(new Organization()
                {
                    Name = model.Name
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var el = await _context.Organizations
                .FirstOrDefaultAsync(item => item.Id == id);
            var model = new OrganizationModel()
            {
                Name = el.Name,
                Id = el.Id
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
                el.Name = model.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}