using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class DR_JB_ORD
    {
        public string DriverICNo { get; set; }
        public string DriverName { get; set; }
        public DateTime ATD_CompleteDate { get; set; }
        public string JobNo { get; set; }
        public string DeliveryCustCode { get; set; }
        public string TransportAgent { get; set; }
    }
}
