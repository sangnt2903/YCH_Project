using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class DeliveryCustomers
    {
        public DeliveryCustomers()
        {
            Orders = new HashSet<Orders>();
        }

        public string DeliveryCustCode { get; set; }
        public string DeliveryAddress { get; set; }
        public string ServiceLevel { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
