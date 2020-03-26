using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalculateSalaryOfFleet.Models
{
    public partial class MoneyByBigCar
    {
        [Display(Name="Tài")]
        public int TripNo { get; set; }
        [Display(Name = "Số tiền đi thành phố")]
        public long? MoneyOfServiceLevelHcmc { get; set; }
        [Display(Name = "Số tiền đi tỉnh")]
        public long? MoneyOfServiceLevelHcmp { get; set; }
    }
}
