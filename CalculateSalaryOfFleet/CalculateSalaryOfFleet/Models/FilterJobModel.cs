using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class FilterJobModel
    {
        public string driverIcNo { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
