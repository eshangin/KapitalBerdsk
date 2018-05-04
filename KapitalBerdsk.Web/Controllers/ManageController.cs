using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KapitalBerdsk.Web.Models;
using KapitalBerdsk.Web.Models.ManageViewModels;
using KapitalBerdsk.Web.Services;

namespace KapitalBerdsk.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ManageIndexViewModel
            {
                ChangePasswordViewModel = new ChangePasswordViewModel(),
                UserInvitationViewModel = new UserInvitationViewModel(),
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

            var changePasswordResult = await _userManager.UpdateAsync(user);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Пароль был изменен.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvitation(ManageIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ChangePassword), model);
            }

            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
            //    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            //    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                _logger.LogInformation($"Created new account with password for email {model.UserInvitationViewModel.Email}");
                  StatusMessage = "Приглашение отослано";
            //    //return RedirectToLocal(returnUrl);
            //}
            //else
            //{
            //    AddErrors(result);
            //      return View(model);
            //}

            return RedirectToAction(nameof(ChangePassword));
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        #endregion
    }
}
