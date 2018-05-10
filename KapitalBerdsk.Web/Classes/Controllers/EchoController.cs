using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Classes.Controllers
{
    [Produces("application/json")]
    [Route("api/Echo")]
    public class EchoController : Controller
    {
        // GET: api/Echo
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
