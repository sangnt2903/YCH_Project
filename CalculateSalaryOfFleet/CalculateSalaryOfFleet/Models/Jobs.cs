using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Jobs
    {
        public Jobs()
        {
            Orders = new HashSet<Orders>();
        }

        public string JobNo { get; set; }
        public string DriverIcno { get; set; }
        public string TruckId { get; set; }

        public Drivers DriverIcnoNavigation { get; set; }
        public Trucks Truck { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
