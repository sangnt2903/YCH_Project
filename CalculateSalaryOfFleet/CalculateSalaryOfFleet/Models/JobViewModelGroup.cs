using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class JobViewModelGroup
    {
        public string JobNo { get; set; }
        public string TruckId { get; set; }
        public string TruckType { get; set; }
        public string DriverIcNo { get; set; }
        public DateTime ATD_Date { get; set; }
        public string DeliveryCustCode { get; set; }
        public string ServiceLevel { get; set; }
    }
}
