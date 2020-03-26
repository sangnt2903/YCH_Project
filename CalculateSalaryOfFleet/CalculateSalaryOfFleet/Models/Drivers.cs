using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Drivers
    {
        public Drivers()
        {
            Jobs = new HashSet<Jobs>();
        }

        public string DriverIcno { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }

        public ICollection<Jobs> Jobs { get; set; }
    }
}
