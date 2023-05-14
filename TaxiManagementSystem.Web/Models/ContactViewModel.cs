using System.ComponentModel.DataAnnotations;

namespace TaxiManagementSystem.Web.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(32767, MinimumLength = 10)]
        public string Message { get; set; }
    }
}