using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Trucks
    {
        public Trucks()
        {
            Jobs = new HashSet<Jobs>();
        }

        public string TruckId { get; set; }
        public string TruckType { get; set; }

        public TruckSize TruckTypeNavigation { get; set; }
        public ICollection<Jobs> Jobs { get; set; }
    }
}
