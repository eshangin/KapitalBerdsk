using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Authorize]
    public class PdSectionController : Controller
    {
        private ApplicationDbContext _context;

        public PdSectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PdSection/Create
        public async Task<ActionResult> Create(int objectId)
        {
            var model = new EditPdSectionModel
            {
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                    Selected = item.Id == objectId
                }),
                BuildingObjectId = objectId,
                SelectedBuildingObjectId = objectId
            };
            return View(model);
        }

        // POST: PdSection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int objectId, EditPdSectionModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.PdSections.AddAsync(new PdSection
                {
                    Name = model.Name,
                    BuildingObjectId = model.BuildingObjectId.Value,
                    EmployeeId = model.EmployeeId.Value,
                    Price = model.Price.Value
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BuildingObjectController.Details), "BuildingObject", 
                    new { id = model.SelectedBuildingObjectId });
            }

            model.BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == objectId
            });
            model.Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            });

            return View(model);
        }

        // GET: PdSection/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var pdSection = await _context.PdSections.FirstOrDefaultAsync(item => item.Id == id);
            var model = new EditPdSectionModel
            {
                Id = pdSection.Id,
                Name = pdSection.Name,
                Price = pdSection.Price,
                EmployeeId = pdSection.EmployeeId,
                Employees = (await _context.Employees.OrderBy(item => item.OrderNumber).ToListAsync()).Select(item => new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                }),
                BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
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
            if (ModelState.IsValid)
            {
                var pdSection = await _context.PdSections.FirstOrDefaultAsync(item => item.Id == model.Id);
                pdSection.Name = model.Name;
                pdSection.BuildingObjectId = model.BuildingObjectId.Value;
                pdSection.EmployeeId = model.EmployeeId.Value;
                pdSection.Price = model.Price.Value;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BuildingObjectController.Details), "BuildingObject",
                    new { id = model.SelectedBuildingObjectId });
            }

            model.BuildingObjects = (await _context.BuildingObjects.ToListAsync()).Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = item.Id == model.SelectedBuildingObjectId
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