using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.Web.Models
{
    public class TransactionHistoryViewModel
    {
        public int? CustomerRating { get; set; }
        public int? DriverRating { get; set; }
        public Ride Ride { get; set; }
    }
}