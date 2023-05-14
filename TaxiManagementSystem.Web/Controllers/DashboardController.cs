using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRideService _rideService;
        private readonly ITaxiService _taxiService;
        private readonly IUserManagerService _userManagerService;
        private readonly ILebanonService _lebanonService;

        public DashboardController(IRideService rideService, ITaxiService taxiService, IUserManagerService userManagerService, ILebanonService lebanonService)
        {
            _rideService = rideService;
            _taxiService = taxiService;
            _userManagerService = userManagerService;
            _lebanonService = lebanonService;
        }

        public ActionResult Index()
        {
            ViewBag.TotalTaxis = _taxiService.GetAll().Count(x => x.TaxiStatus != TaxiStatus.NotInUse);
            ViewBag.TotalCustomers = _userManagerService.FindAllUsers().Count();
            ViewBag.TotalOrders = _rideService.GetAll().Count();

            List<Lebanon> cities = _lebanonService.GetAll().ToList();

            ViewBag.TotalCities = cities.Select(x => x.City).Distinct().Count();
            ViewBag.TotalDistricts = cities.Select(x => x.District).Distinct().Count();
            ViewBag.TotalAreas = cities.Select(x => x.Area).Distinct().Count();

            return View();
        }

        public ActionResult TaxiOrders()
        {
            List<TaxiOrdersViewModel> result = _rideService.GetAll()
                .GroupBy(x => new { x.CreatedDate.Date })
                .Select(x => new TaxiOrdersViewModel
                {
                    Date = x.Key.Date,
                    Count = x.Count()
                })
                .OrderBy(x=>x.Date)
                .ToList();

            return Json(result.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }
    }
}