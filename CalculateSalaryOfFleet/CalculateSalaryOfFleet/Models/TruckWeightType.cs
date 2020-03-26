using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalculateSalaryOfFleet.Models
{
    public partial class TruckWeightType
    {
        [Display(Name ="Mã loại vận tải")]
        public int TruckWeightTypeId { get; set; }
        [Display(Name = "Loại vận tải")]
        public string TruckWeightType1 { get; set; }
        [Display(Name = "Tải trọng từ")]
        public double? WeightFrom { get; set; }
        [Display(Name = "Tải trọng tới")]
        public double? WeightTo { get; set; }
    }
}
