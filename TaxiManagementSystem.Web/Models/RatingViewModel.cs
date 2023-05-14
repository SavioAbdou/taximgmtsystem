using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaxiManagementSystem.DTO;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.Web.Models
{
    public class RatingViewModel
    {
        public Ride Ride { get; set; }

        [Display(Name = "Rating value")]
        [Range(1, 5, ErrorMessage = "Range must be between 1 and 5!")]
        public int Value { get; set; }

        public List<Ride> MyRides { get; set; }
        public List<Rating> Ratings { get; set; }
        public double MyRating { get; set; }
    }
}