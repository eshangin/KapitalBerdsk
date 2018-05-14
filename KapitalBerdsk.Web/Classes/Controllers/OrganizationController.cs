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

        // GET: Organization/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Organization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}