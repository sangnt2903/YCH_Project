using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class ResultModelView
    {
        public string DriverIcNo { get; set; }
        public string DriverName { get; set; }
        public int TotalJobsInMonth { get; set; }
        public int TotalDropPointOnTotalJobs { get; set; }
        public double TotalTrip { get; set; }

    }
}
