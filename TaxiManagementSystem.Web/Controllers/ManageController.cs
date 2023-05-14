using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Repository;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IUserManagerService _userManagerService;

        public ManageController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been successfully changed"
                    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been successfully set"
                        : message == ManageMessageId.SetTwoFactorSuccess ? "Two factor authentication has been set"
                            : message == ManageMessageId.Error ? "Somthing went wrong"
                                : message == ManageMessageId.AddPhoneSuccess ? "Telephone number has been successfully added"
                                    : message == ManageMessageId.RemovePhoneSuccess ? "Telephone number has been successfully removed"
                                        : "";

            var userId = User.Identity.GetUserId();

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await _userManagerService.GetPhoneNumberAsync(userId),
                //TwoFactor = await _userManagerService.GetTwoFactorEnabledAsync(userId),
                Logins = await _userManagerService.GetLoginsAsync(userId),
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                EmailAddress = await _userManagerService.GetEmailAsync(userId),
                //EmailNotifications = HasEmailNotifications()
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userManagerService.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManagerService.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManagerService.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await _userManagerService.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            return View(model);
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationBaseIdentity user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManagerService.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManagerService.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        private bool HasPhoneNumber()
        {
            var user = _userManagerService.FindById(User.Identity.GetUserId());
            return user?.PhoneNumber != null;
        }

        //private bool HasEmailNotifications()
        //{
        //    var user = _userManagerService.FindById(User.Identity.GetUserId());
        //    if (user?.EmailNotifications != null)
        //    {
        //        return user.EmailNotifications;
        //    }
        //    return false;
        //}

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        
    }
}