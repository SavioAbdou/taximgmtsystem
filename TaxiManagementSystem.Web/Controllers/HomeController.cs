using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EasyGCaptchaMVC.Configuration;
using EasyGCaptchaMVC.Model;
using EasyGCaptchaMVC.Worker;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;
using TaxiManagementSystem.Web.Helpers;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaxiService _taxiService;
        private readonly IRoleManagerService _roleManagerService;
        private readonly IUserManagerService _userManagerService;

        public HomeController(ITaxiService taxiService, IRoleManagerService roleManagerService, IUserManagerService userManagerService)
        {
            _taxiService = taxiService;
            _roleManagerService = roleManagerService;
            _userManagerService = userManagerService;
        }

        public ActionResult Index()
        {
            FirstRunInitialize();

            return View();
        }

        public ActionResult Contact()
        {
            GCaptchaSettingsProvider.Instance.Theme = Theme.Dark;
            GCaptchaSettingsProvider.Instance.Size = Size.Invisible;
            GCaptchaSettingsProvider.Instance.PrivateKey = "6LfmHFMUAAAAAFLUFGNlegdrmJ2WdueVl1857mC6";
            GCaptchaSettingsProvider.Instance.PublicKey = "6LfmHFMUAAAAAGm5-L_vW8ges-I2RZvmoLRyJfx4";

            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EasyGCaptcha]
        public ActionResult Contact(ContactViewModel viewModel, EasyGCaptchaResult easyGCaptchaResult)
        {
            TempData["error"] = null;
            TempData["success"] = null;

            if (!ModelState.IsValid || !easyGCaptchaResult.Success)
            {
                TempData["error"] = "Couldn't send email, because of invalid values on the form!";
                return View(viewModel);
            }

            try
            {
                Utility.SendEmail(viewModel);

                TempData["success"] = "Successfully sent email";
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (ex.InnerException != null)
                {
                    error += "<br/>" + ex.InnerException.Message;
                }
                TempData["error"] = error;
            }

            return RedirectToAction("Contact");
        }

        private void FirstRunInitialize()
        {
            string name = Server.MapPath("~/App_Data/firstrun.dat");

            FileInfo info = new FileInfo(name);
            if (!info.Exists)
            {
                using (StreamWriter writer = info.CreateText())
                {
                    writer.WriteLine("First-run initialization complete");

                }
            }
            else return;


            if (_taxiService.GetAll(null).FirstOrDefault(x => x.Id == default(Guid)) == null)//here we create a fake taxi with 0000000 Id
            {
                _taxiService.Create(new Taxi
                {
                    Id = default(Guid),
                    TaxiStatus = TaxiStatus.NotInUse,
                    Type = TaxiTypes.NotInUse
                });
            }

            if (_roleManagerService.FindRoleByName("Admin") == null)
            {
                _roleManagerService.CreateRole("Admin");
            }

            if (_roleManagerService.FindRoleByName("Driver") == null)
            {
                _roleManagerService.CreateRole("Driver");
            }

            if (_roleManagerService.FindRoleByName("User") == null)
            {
                _roleManagerService.CreateRole("User");
            }

            Task.Run(async () => await CreateTestUsers());
        }

        private async Task CreateTestUsers()
        {
            if (_userManagerService.FindByUsername("admin") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationAdmin
                {
                    UserName = "admin",
                    Email = "admin@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("admin").Id, "Admin");
            }

            if (_userManagerService.FindByUsername("user1") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("user1").Id, "User");
            }

            if (_userManagerService.FindByUsername("driver1") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver1",
                    Email = "driver1@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver1").Id, "Driver");
            }

            if (_userManagerService.FindByUsername("driver2") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver2",
                    Email = "driver2@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver2").Id, "Driver");
            }

            if (_userManagerService.FindByUsername("driver3") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver3",
                    Email = "driver3@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver3").Id, "Driver");
            }

            if (_userManagerService.FindByUsername("driver4") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver4",
                    Email = "driver4@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver4").Id, "Driver");
            }

            if (_userManagerService.FindByUsername("driver5") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver5",
                    Email = "driver5@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver5").Id, "Driver");
            }

            if (_userManagerService.FindByUsername("driver6") == null)
            {
                await _userManagerService.CreateAsync(new ApplicationDriver
                {
                    UserName = "driver6",
                    Email = "driver6@taxi.com",
                    Gender = Gender.Male,
                    PhoneNumber = "12345678"
                }, "Aa12345");

                await _userManagerService.AssignToRole(_userManagerService.FindByUsername("driver6").Id, "Driver");
            }

            await CreateTaxis();
        }

        private async Task CreateTaxis()
        {
            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver1").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver1").Id,
                    Type = TaxiTypes.Combi,
                    TaxiStatus = TaxiStatus.Unoccupied
                });

            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver2").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver2").Id,
                    Type = TaxiTypes.Combi,
                    TaxiStatus = TaxiStatus.Unoccupied
                });

            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver3").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver3").Id,
                    Type = TaxiTypes.Small,
                    TaxiStatus = TaxiStatus.Unoccupied
                });

            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver4").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver4").Id,
                    Type = TaxiTypes.Small,
                    TaxiStatus = TaxiStatus.Unoccupied
                });

            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver5").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver5").Id,
                    Type = TaxiTypes.Van,
                    TaxiStatus = TaxiStatus.Unoccupied
                });

            if (_taxiService.GetByDriverId(_userManagerService.FindByUsername("driver6").Id) == null)
                _taxiService.Create(new Taxi
                {
                    DriverId = _userManagerService.FindByUsername("driver6").Id,
                    Type = TaxiTypes.Van,
                    TaxiStatus = TaxiStatus.Unoccupied
                });
        }
    }
}