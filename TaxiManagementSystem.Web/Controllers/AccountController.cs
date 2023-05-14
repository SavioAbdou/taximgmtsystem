using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EasyGCaptchaMVC.Configuration;
using EasyGCaptchaMVC.Model;
using EasyGCaptchaMVC.Worker;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;
using TaxiManagementSystem.Web.Models;
using System.Linq;

namespace TaxiManagementSystem.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IRoleManagerService _roleManagerService;

        public AccountController(IUserManagerService userManagerService, IRoleManagerService roleManagerService)
        {
            _userManagerService = userManagerService;
            _roleManagerService = roleManagerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationBaseIdentity user = await _userManagerService.FindAsync(model.UserName, model.Password);

            if (user != null)
            {
                await SignInAsync(user, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", @"Invalid username or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            GCaptchaSettingsProvider.Instance.Theme = Theme.Dark;
            GCaptchaSettingsProvider.Instance.Size = Size.Invisible;
            GCaptchaSettingsProvider.Instance.PrivateKey = "6LfmHFMUAAAAAFLUFGNlegdrmJ2WdueVl1857mC6";
            GCaptchaSettingsProvider.Instance.PublicKey = "6LfmHFMUAAAAAGm5-L_vW8ges-I2RZvmoLRyJfx4";

            ViewBag.RoleList = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x != Roles.Admin).Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = x.ToString()
            });

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [EasyGCaptcha]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase upload, EasyGCaptchaResult easyGCaptchaResult)
        {
            if (ModelState.IsValid && easyGCaptchaResult.Success)
            {
                ApplicationBaseIdentity user;
                IdentityRole role;

                switch (model.Role)
                {
                    case Roles.Admin:
                        user = new ApplicationAdmin
                        {

                        };
                        if ((role = _roleManagerService.FindRoleByName("admin")) == null)
                        {
                            _roleManagerService.CreateRole("Admin");
                            role = _roleManagerService.FindRoleByName("admin");
                        }
                        break;
                    case Roles.Driver:
                        user = new ApplicationDriver
                        {

                        };
                        if ((role = _roleManagerService.FindRoleByName("driver")) == null)
                        {
                            _roleManagerService.CreateRole("Driver");
                            role = _roleManagerService.FindRoleByName("driver");
                        }
                        break;
                    case Roles.User:
                        user = new ApplicationUser
                        {

                        };
                        if ((role = _roleManagerService.FindRoleByName("user")) == null)
                        {
                            _roleManagerService.CreateRole("User");
                            role = _roleManagerService.FindRoleByName("user");
                        }
                        break;
                    default:
                        throw new ArgumentNullException(nameof(user));
                }

                user.Gender = model.Gender;
                user.ProfilePic = upload?.ContentLength > 0
                    ? new BinaryReader(upload.InputStream).ReadBytes(upload.ContentLength)
                    : null;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = Request["areaCode"] + model.PhoneNumber;

                var result = await _userManagerService.CreateAsync(user, model.Password);
                await _userManagerService.AssignToRole(user.Id, role.Name);

                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            ViewBag.RoleList = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x != Roles.Admin).Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = x.ToString()
            });

            return View(model);
        }

        public ActionResult GetBalance()
        {
            return Content(_userManagerService.FindById(HttpContext.User.Identity.GetUserId()).Balance.ToString());
        }

        #region Helpers

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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private Guid getGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }

        #endregion
    }
}