using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalculateSalaryOfFleet.Models
{
    public partial class DesLocateException
    {
        [Display(Name = "Mã khu vực")]
        public int DesLocationNo { get; set; }
        [Display(Name = "Tên khu vực không dấu")]
        public string DeslocationName { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string DeslocationFullName { get; set; }
    }
}
