using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateSalaryOfFleet.Models
{
    public class FilterModel
    {
        public string TransportAgent { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
