using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Excels
    {
        public int ExcelCode { get; set; }
        public string ExcelFileName { get; set; }
        public DateTime? ExcelUploadedDate { get; set; }
    }
}
