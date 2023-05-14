using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using TaxiManagementSystem.DTO.Enums;

namespace TaxiManagementSystem.DTO
{
    public abstract class ApplicationBaseIdentity : IdentityUser
    {
        protected ApplicationBaseIdentity()
        {
            CreatedDate = DateTime.Now;
        }

        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public Gender Gender { get; set; }
        public byte[] ProfilePic { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}