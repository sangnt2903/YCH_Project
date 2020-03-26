using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class MoneyJob
    {
        public int Id { get; set; }
        public int? JobOrder { get; set; }
        public string DescriptionJobOrder { get; set; }
        public long PerformenceMoney { get; set; }
    }
}
