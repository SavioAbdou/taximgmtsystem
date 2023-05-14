using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiManagementSystem.DTO
{
    public class Rating : BaseEntity
    {
        [Display(Name = "Rating value")]
        [Range(1, 5, ErrorMessage = "Range must be between 1 and 5!")]
        public int Value { get; set; }

        [StringLength(128)]
        public string DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual ApplicationDriver Driver { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public Guid RideId { get; set; }

        [ForeignKey("RideId")]
        public virtual Ride Ride { get; set; }
    }
}