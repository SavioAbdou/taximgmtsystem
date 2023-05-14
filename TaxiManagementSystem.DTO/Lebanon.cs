using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiManagementSystem.DTO
{
    public class Lebanon : BaseEntity
    {
        public Lebanon()
        {
            Source = new HashSet<Ride>();
            Destination = new HashSet<Ride>();
        }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(40)]
        public string District { get; set; }

        [Required]
        [StringLength(20)]
        public string Area { get; set; }

        public virtual ICollection<Ride> Source { get; set; }

        public virtual ICollection<Ride> Destination { get; set; }
    }
}