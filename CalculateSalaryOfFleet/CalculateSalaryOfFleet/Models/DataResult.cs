using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class DataResult
    {
        public string DriverIcNo { get; set; }
        public string DriverName { get; set; }
        public string TransportAgent { get; set; }
        public DateTime ATD_Date { get; set; }
        public int TotalJobs { get; set; }
        public int TotalDropPoint { get; set; }
        public List<DescriptionPerJob> descriptionPerJobs { get; set; }
    }
}
