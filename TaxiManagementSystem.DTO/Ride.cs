using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.DTO
{
    [Table("Ride")]
    public class Ride : BaseEntity
    {
        public Ride()
        {
            Ratings = new HashSet<Rating>();
        }

        public double Distance { get; set; }

        [Display(Name = "Booking time")]
        public DateTime BookingTime { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public RideStatus RideStatus { get; set; }

        [Display(Name = "Is user rated?")]
        public bool IsUserRated { get; set; }

        [Display(Name = "Is driver rated?")]
        public bool IsDriverRated { get; set; }

        [Column(TypeName = "Money")]
        [Display(Name = "Estimated price")]
        public decimal EstimatedPrice { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public Guid TaxiId { get; set; }
        [ForeignKey("TaxiId")]
        public virtual Taxi Taxi { get; set; }

        public Guid SourceId { get; set; }
        [ForeignKey("SourceId")]
        [Display(Name = "Source")]
        public virtual Lebanon Source { get; set; }

        public Guid DestinationId { get; set; }
        [ForeignKey("DestinationId")]
        [Display(Name = "Destination")]
        public virtual Lebanon Destination { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}