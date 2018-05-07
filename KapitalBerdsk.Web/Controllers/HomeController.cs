using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KapitalBerdsk.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace KapitalBerdsk.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(FundsFlowController.Index),
                    nameof(FundsFlowController).Replace("controller", string.Empty, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), 
                    nameof(AccountController).Replace("controller", string.Empty, StringComparison.OrdinalIgnoreCase));
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
