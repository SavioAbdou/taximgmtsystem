using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Mvc;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    [Authorize]
    public class RideController : Controller
    {
        private readonly IRideService _rideService;
        private readonly ITaxiService _taxiService;
        private readonly ILebanonService _lebanonService;
        private readonly IUserManagerService _userManagerService;
        private readonly IRatingService _ratingService;

        public RideController(IRideService rideService, ITaxiService taxiService, ILebanonService lebanonService, IUserManagerService userManagerService, IRatingService ratingService)
        {
            _rideService = rideService;
            _taxiService = taxiService;
            _lebanonService = lebanonService;
            _userManagerService = userManagerService;
            _ratingService = ratingService;
        }

        public ActionResult Index()
        {
            IPrincipal myUser = HttpContext.User;
            string myUserId = myUser.Identity.GetUserId();

            List<Ride> rides = new List<Ride>();
            ViewBag.HaveTaxi = false;

            if (myUser.IsInRole("Driver")) //I'm a driver
            {
                Taxi myTaxi = _taxiService.GetByDriverId(myUserId);
                if (myTaxi != null)
                {
                    rides = _rideService.GetAllByMyTaxi(myTaxi.Id, false, x => x.Destination, x => x.Source, x => x.Taxi,
                        x => x.User).ToList();
                    ViewBag.HaveTaxi = true;
                }
            }//I'm a customer
            else
            {
                rides = _rideService.GetAllMine(myUserId, false, x => x.Destination, x => x.Source, x => x.Taxi,
                    x => x.User).ToList();
            }

            return View(rides);
        }

        public ActionResult Accept(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ride ride = _rideService.FindById(id.Value);
            if (ride == null)
            {
                return HttpNotFound();
            }
            ride.RideStatus = RideStatus.Accepted;
            _rideService.Update(ride);

            Taxi taxi = _taxiService.GetById(ride.TaxiId);
            taxi.TaxiStatus = TaxiStatus.Occupied;
            _taxiService.Update(taxi);

            return RedirectToAction("Index");
        }

        public ActionResult Complete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ride ride = _rideService.FindById(id.Value);
            if (ride == null)
            {
                return HttpNotFound();
            }
            ride.RideStatus = RideStatus.Completed;
            _rideService.Update(ride);

            Taxi taxi = _taxiService.GetById(ride.TaxiId);
            taxi.TaxiStatus = TaxiStatus.Unoccupied;
            _taxiService.Update(taxi);

            ApplicationDriver driver = _userManagerService.FindById(taxi.DriverId) as ApplicationDriver;
            if (driver != null)
            {
                driver.Balance += ride.EstimatedPrice;
                _userManagerService.SetBalance(driver.Id, driver.Balance);
            }
            ApplicationUser customer = _userManagerService.FindById(ride.UserId) as ApplicationUser;
            if (customer != null)
            {
                customer.Balance -= ride.EstimatedPrice;
                _userManagerService.SetBalance(customer.Id, customer.Balance);
            }


            return RedirectToAction("Index");
        }

        public ActionResult Reject(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ride ride = _rideService.FindById(id.Value);
            if (ride == null)
            {
                return HttpNotFound();
            }
            ride.RideStatus = RideStatus.Rejected;
            _rideService.Update(ride);

            Taxi taxi = _taxiService.GetById(ride.TaxiId);
            taxi.TaxiStatus = TaxiStatus.Unoccupied;
            _taxiService.Update(taxi);

            return RedirectToAction("Index");
        }

        public ActionResult Cancel(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ride ride = _rideService.FindById(id.Value);
            if (ride == null)
            {
                return HttpNotFound();
            }

            Taxi taxi = _taxiService.GetById(ride.TaxiId);
            taxi.TaxiStatus = TaxiStatus.Unoccupied;
            _taxiService.Update(taxi);

            ride.RideStatus = RideStatus.Cancelled;
            ride.TaxiId = default(Guid);
            _rideService.Update(ride);

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            IEnumerable<Lebanon> locations = _lebanonService.GetAll().OrderBy(x => x.City).ThenBy(x => x.District)
                .ThenBy(x => x.Area);

            ViewBag.DestinationId = new SelectList(locations.Select(x => new
            {
                x.Id,
                x.City,
                x.District,
                x.Area,
                Name = string.Format($"{x.City}, {x.District}, {x.Area}")
            }), "Id", "Name");

            ViewBag.SourceId = new SelectList(locations.Select(x => new
            {
                x.Id,
                x.City,
                x.District,
                x.Area,
                Name = string.Format($"{x.City}, {x.District}, {x.Area}")
            }), "Id", "Name");

            var taxiTypes = _taxiService.GetAll().Where(x => x.TaxiStatus == TaxiStatus.Unoccupied).GroupBy(x => x.Type).Select(x => x.First());

            ViewBag.TaxiId = new SelectList(taxiTypes, "Id", "Type");

            ViewBag.UserId = new SelectList(_userManagerService.FindAllDrivers(), "Id", "Email");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ride ride)
        {
            ModelState.Remove("Distance");
            ModelState.Remove("EstimatedPrice");
            if (ModelState.IsValid)
            {
                ride.RideStatus = RideStatus.New;
                ride.UserId = HttpContext.User.Identity.GetUserId();
                ride.Distance = double.Parse(Request["Distance"]);
                ride.EstimatedPrice = decimal.Parse(Request["EstimatedPrice"]);
                _rideService.Create(ride);

                return RedirectToAction("Index");
            }

            IEnumerable<Lebanon> locations = _lebanonService.GetAll().OrderBy(x => x.City).ThenBy(x => x.District)
                .ThenBy(x => x.Area);

            ViewBag.DestinationId = new SelectList(locations.Select(x => new
            {
                x.Id,
                x.City,
                x.District,
                x.Area,
                Name = string.Format($"{x.City}, {x.District}, {x.Area}")
            }), "Id", "Name");

            ViewBag.SourceId = new SelectList(locations.Select(x => new
            {
                x.Id,
                x.City,
                x.District,
                x.Area,
                Name = string.Format($"{x.City}, {x.District}, {x.Area}")
            }), "Id", "Name");

            ViewBag.TaxiId = new SelectList(_taxiService.GetAll().Where(x => x.TaxiStatus == TaxiStatus.Unoccupied), "Id", "Type");

            ViewBag.UserId = new SelectList(_userManagerService.FindAllDrivers(), "Id", "Email");

            return View(ride);
        }

        public ActionResult TransactionHistory()
        {
            IPrincipal myUser = HttpContext.User;
            string myUserId = myUser.Identity.GetUserId();

            Taxi myTaxi = _taxiService.GetByDriverId(myUserId);
            if (myTaxi != null)
            {
                List<Rating> ratings = _ratingService.GetAll().ToList();

                List<Ride> rides =
                    _rideService.GetAllByMyTaxi(myTaxi.Id, false,
                    ride => ride.Destination,
                    ride => ride.Source,
                    ride => ride.User
                    )
                    .Where(x => x.RideStatus == RideStatus.Completed).ToList();

                List<TransactionHistoryViewModel> result = new List<TransactionHistoryViewModel>();

                foreach (Ride ride in rides)
                {
                    Rating customerRating =
                        ratings.FirstOrDefault(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(ride.UserId) && x.RideId == ride.Id);
                    Rating driverRating =
                        ratings.FirstOrDefault(x => !string.IsNullOrEmpty(x.DriverId) && x.DriverId.Equals(myUserId) && x.RideId == ride.Id);

                    result.Add(new TransactionHistoryViewModel
                    {
                        Ride = ride,
                        CustomerRating = customerRating?.Value,
                        DriverRating = driverRating?.Value
                    });
                }

                return View(result);
            }


            return View(new List<TransactionHistoryViewModel>());
        }

        public ActionResult DriversRide()
        {
            ViewBag.Drivers = _userManagerService.FindAllDrivers().OrderBy(x => x.UserName);
            return View();
        }

        public ActionResult DriverRide(string id)
        {
            Taxi taxi = _taxiService.GetByDriverId(id);

            if (taxi == null) return RedirectToAction("DriversRide");

            ViewBag.Driver = _userManagerService.FindById(id);

            ViewBag.DriverRides = _rideService
                .GetAll(false, x => x.Source, x => x.Destination, x => x.Taxi)
                .Where(x => x.TaxiId == taxi.Id).OrderByDescending(x => x.BookingTime);

            return View();
        }
    }
}
