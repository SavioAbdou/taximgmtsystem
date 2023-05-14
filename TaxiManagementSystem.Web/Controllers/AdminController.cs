using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.Web.Helpers;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILebanonService _lebanonService;
        private readonly IUserManagerService _userManagerService;

        public AdminController(ILebanonService lebanonService, IUserManagerService userManagerService)
        {
            _lebanonService = lebanonService;
            _userManagerService = userManagerService;
        }

        public ActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ImportExcel")]
        public ActionResult ImportExcelPost()
        {
            if (Request.Files["FileUpload1"]?.ContentLength > 0)
            {
                string extension = Path.GetExtension(Request.Files["FileUpload1"]?.FileName)?.ToLower();
                string query = null;
                string connString = "";

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                //string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                string path1 = string.Format("{0}\\{1}", Server.MapPath("~/Content/Uploads"), Path.GetFileName(Request.Files["FileUpload1"].FileName));
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                }
                if (validFileTypes.Contains(extension))
                {
                    DataTable dt = null;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    Request.Files["FileUpload1"].SaveAs(path1);
                    if (extension == ".csv")
                    {
                        dt = Utility.ConvertCSVtoDataTable(path1);
                        ViewBag.Data = dt;
                    }

                    else if (extension?.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        ViewBag.Data = dt;
                    }
                    else if (extension?.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        ViewBag.Data = dt;
                    }

                    if (dt != null)
                    {
                        List<Lebanon> citiesInDatabase = _lebanonService.GetAll(null).ToList();

                        foreach (DataRow row in dt.Rows)
                        {
                            if (string.IsNullOrEmpty(row.ItemArray[0].ToString()))
                            {
                                continue;
                            }
                            //check if already exists in database:

                            Lebanon temp =
                                citiesInDatabase.FirstOrDefault(
                                    x => x.City.Trim().ToLower().Equals(row["city name"].ToString().Trim().ToLower()) &&
                                         x.District.Trim().ToLower()
                                             .Equals(row["district name"].ToString().Trim().ToLower()) &&
                                         x.Area.Trim().ToLower().Equals(row["area name"].ToString().Trim().ToLower())
                                );


                            if (temp == null)//if not exists in database, we create a new record
                            {
                                _lebanonService.Create(new Lebanon
                                {
                                    City = row["city name"].ToString(),
                                    District = row["district name"].ToString(),
                                    Area = row["area name"].ToString()
                                });
                            }
                            else if (temp.IsDeleted)//if exists but the IsDeleted value is true, we set it to false and update it
                            {
                                temp.IsDeleted = false;
                                _lebanonService.Update(temp);
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Balance()
        {
            List<BalanceViewModel> users = new List<BalanceViewModel>();
            foreach (ApplicationUser user in _userManagerService.FindAllUsers())
            {
                users.Add(new BalanceViewModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Balance = user.Balance,
                    Email = user.Email,
                    UserName = user.UserName
                });
            }

            ViewBag.Users = users;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Balance(BalanceViewModel model)
        {
            TempData["error"] = null;
            TempData["success"] = null;

            if (string.IsNullOrEmpty(model?.Id))
            {
                model = new BalanceViewModel
                {
                    Id = Request["user.Id"],
                    PhoneNumber = Request["user.PhoneNumber"],
                    Balance = decimal.Parse(Request["user.Balance"]),
                    Email = Request["user.Email"],
                    UserName = Request["user.UserName"]
                };
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Users = _userManagerService.FindAllUsers();
                TempData["error"] = "Couldn't update balance, because of invalid values on the form!";
                return View(model);
            }

            try
            {
                _userManagerService.SetBalance(model.Id, model.Balance);

                TempData["success"] = "Balance successfully updated";
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
            

            return RedirectToAction("Balance");
        }
    }
}