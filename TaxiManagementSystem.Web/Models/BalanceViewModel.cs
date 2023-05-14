using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiManagementSystem.Web.Models
{
    public class BalanceViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
    }
}