using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.DTO
{
    public class ApplicationUser : ApplicationBaseIdentity
    {
        public ApplicationUser()
        {
            Rides = new HashSet<Ride>();
            Ratings = new HashSet<Rating>();
        }

        public virtual ICollection<Ride> Rides { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}