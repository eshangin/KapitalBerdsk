using System;
using System.Diagnostics;
using KapitalBerdsk.Web.Classes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Classes.Controllers
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
