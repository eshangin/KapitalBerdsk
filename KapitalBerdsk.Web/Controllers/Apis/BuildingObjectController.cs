using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Apis.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BuildingObjectController : Controller
    {
        private ApplicationDbContext _context;

        public BuildingObjectController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = nameof(ClosingContracts))]
        public async Task<IActionResult> ClosingContracts()
        {
            var today = DateTime.UtcNow.AddHours(7).Date;
            var tillDate = today.AddDays(7);

            var items = await (from bo in _context.BuildingObjects
                               where !bo.IsClosed &&
                                   bo.ContractDateEnd >= today &&
                                   bo.ContractDateEnd <= tillDate
                               orderby bo.ContractDateEnd
                               select new
                               {
                                   bo.Name,
                                   bo.ContractDateEnd
                               }).ToListAsync();

            return Ok(items);
        }
    }
}
