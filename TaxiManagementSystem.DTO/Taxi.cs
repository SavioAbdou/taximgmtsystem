using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.DTO
{
    public class Taxi : BaseEntity
    {
        public Taxi()
        {
            Rides = new HashSet<Ride>();
        }

        [Required]
        public TaxiTypes Type { get; set; }

        [Required]
        public TaxiStatus TaxiStatus { get; set; }

        [StringLength(128)]
        public string DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual ApplicationDriver Driver { get; set; }

        public virtual ICollection<Ride> Rides { get; set; }
    }
}