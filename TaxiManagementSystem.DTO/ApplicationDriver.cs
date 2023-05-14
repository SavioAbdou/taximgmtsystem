using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.DTO
{
    public class ApplicationDriver : ApplicationBaseIdentity
    {
        public ApplicationDriver()
        {
            Taxis = new HashSet<Taxi>();
            Ratings = new HashSet<Rating>();
        }

        public int DriverRating { get; set; }

        public virtual ICollection<Taxi> Taxis { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}