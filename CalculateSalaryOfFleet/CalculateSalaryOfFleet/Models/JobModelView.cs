using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class JobModelView
    {
        public string JobNo { get; set; }
        public string TruckId { get; set; }
        public string TruckType { get; set; }
        public string ServiceLevel { get; set; }
        public DateTime ATD_Date { get; set; }
        public int NumberOfDropPoint { get; set; }
        public double NumberOfTrips { get; set; }
        public long Money { get; set; }
    }
}
