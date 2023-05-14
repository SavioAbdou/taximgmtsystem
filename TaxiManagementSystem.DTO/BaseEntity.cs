using System;
using System.ComponentModel.DataAnnotations;

namespace TaxiManagementSystem.DTO
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            CreateUser = "Anonymous";
        }

        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CreateUser { get; set; }

        public DateTime? LastModificationDate { get; set; }

        [StringLength(50)]
        public string LastModificationUser { get; set; }
    }
}