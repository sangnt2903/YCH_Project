using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalculateSalaryOfFleet.Models
{
    public partial class TypeJob
    {
        [Display(Name ="Mã shipzone")]
        public int TypeJobNo { get; set; }
        [Display(Name = "Loại shipzone")]
        public string TypeJob1 { get; set; }
        [Display(Name = "Mô tả")]
        public string TypeJobDescription { get; set; }
    }
}
