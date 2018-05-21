using System.Linq;
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
                var pdSection = new PdSection();
                UpdateValues(pdSection, model);

                await _context.PdSections.AddAsync(pdSection);
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

        private void UpdateValues(PdSection pdSection, EditPdSectionModel model)
        {
            pdSection.Name = model.Name;
            pdSection.BuildingObjectId = model.BuildingObjectId.Value;
            pdSection.Price = model.Price.Value;
            pdSection.SetEmployee(model.UseOneTimeEmployee, model.EmployeeId, model.OneTimeEmployeeName);
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
                OneTimeEmployeeName = pdSection.OneTimeEmployeeName,
                UseOneTimeEmployee = !string.IsNullOrWhiteSpace(pdSection.OneTimeEmployeeName),
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
            model.VerifyEmployeeSelection(ModelState);

            if (ModelState.IsValid)
            {
                PdSection pdSection = await _context.PdSections.FirstOrDefaultAsync(item => item.Id == model.Id);
                UpdateValues(pdSection, model);
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

        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.PdSections
                    .Include(item => item.Employee)
                    .Select(item => new PdSectionModel
                    {
                        Name = item.Name,
                        Id = item.Id,
                        Price = item.Price,
                        EmployeeName = item.OneTimeEmployeeName ?? item.Employee.FullName
                    })
                    .FirstOrDefaultAsync(item => item.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var itemToDelete = new PdSection() { Id = id };
            _context.Entry(itemToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BuildingObjectController.Index), nameof(BuildingObjectController).Replace("Controller", string.Empty));
        }
    }
}