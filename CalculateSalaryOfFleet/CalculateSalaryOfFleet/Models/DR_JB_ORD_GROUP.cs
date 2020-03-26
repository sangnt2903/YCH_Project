using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class DR_JB_ORD_GROUP
    {
        public string DriverICNo { get; set; }
        public string DriverName { get; set; }
        public string TransportAgent { get; set; }
        public DateTime ATD_CompleteDate { get; set; }
        public string JobNo { get; set; }
        public int TotalDropPointInJob { get; set; }
    }
}
