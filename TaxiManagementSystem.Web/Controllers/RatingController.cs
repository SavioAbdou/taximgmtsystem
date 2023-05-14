using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly IRideService _rideService;
        private readonly ITaxiService _taxiService;

        public RatingController(IRatingService ratingService, IRideService rideService, ITaxiService taxiService)
        {
            _ratingService = ratingService;
            _rideService = rideService;
            _taxiService = taxiService;
        }

        public ActionResult Index()
        {
            IPrincipal myUser = HttpContext.User;
            RatingViewModel model = new RatingViewModel
            {
                MyRides = new List<Ride>()
            };
            if (myUser.IsInRole("Driver"))
            {
                Taxi myTaxi = _taxiService.GetByDriverId(myUser.Identity.GetUserId());

                if (myTaxi != null)
                {
                    model.MyRides = _rideService
                        .GetAllByMyTaxi(myTaxi.Id, false, x => x.Source, x => x.Destination, x => x.Taxi.Driver,
                            x => x.User, x => x.Ratings).ToList();
                }

                model.Ratings = _ratingService.GetAll().Where(x => !string.IsNullOrEmpty(x.DriverId) && x.DriverId.Equals(myUser.Identity.GetUserId())).ToList();
            }
            else
            {
                model.MyRides = _rideService.GetAllMine(myUser.Identity.GetUserId(), false, x => x.Source, x => x.Destination, x => x.Taxi.Driver, x => x.User, x => x.Ratings).ToList();

                model.Ratings = _ratingService.GetAll().Where(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(myUser.Identity.GetUserId())).ToList();
            }

            double result = (model.Ratings.Sum(x => x.Value) * 1.0) / model.Ratings.Count;

            model.MyRating = result;

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Rate(RatingViewModel viewModel)
        {
            Ride ride = _rideService.FindById(viewModel.Ride.Id);

            Rating result = new Rating
            {
                Value = viewModel.Value,
                RideId = viewModel.Ride.Id
            };

            if (User.IsInRole("Driver"))//the driver is doing the rating now, so user will be rated
            {
                result.UserId = viewModel.Ride.User.Id;
                ride.IsUserRated = true;
            }
            else
            {
                result.DriverId = viewModel.Ride.Taxi.DriverId;
                ride.IsDriverRated = true;
            }

            _ratingService.Create(result);
            _rideService.Update(ride);

            return RedirectToAction("Index");
        }
    }
}