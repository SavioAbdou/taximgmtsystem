using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.Web.Controllers
{
    public class TaxiController : Controller
    {
        private readonly ITaxiService _taxiService;

        public TaxiController(ITaxiService taxiService)
        {
            _taxiService = taxiService;
        }

        public ActionResult Index()
        {
            return View(_taxiService.GetByDriverId(HttpContext.User.Identity.GetUserId()));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Taxi taxi)
        {
            bool iAlreadyHaveTaxi = _taxiService.GetByDriverId(HttpContext.User.Identity.GetUserId()) != null;

            taxi.TaxiStatus = TaxiStatus.Unoccupied;
            taxi.DriverId = HttpContext.User.Identity.GetUserId();

            if (ModelState.IsValid && !iAlreadyHaveTaxi)
            {
                _taxiService.Create(taxi);
                return RedirectToAction("Index");
            }

            return View(taxi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Taxi taxi)
        {
            if (taxi == null) throw new ArgumentNullException(nameof(taxi));

            _taxiService.RemoveFromDriver(taxi);
            _taxiService.Delete(taxi);

            return RedirectToAction("Index");
        }
    }
}