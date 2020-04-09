using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class TruckWeightType
    {
        public int TruckWeightTypeId { get; set; }
        public string TruckWeightType1 { get; set; }
        public double? WeightFrom { get; set; }
        public double? WeightTo { get; set; }
    }
}
