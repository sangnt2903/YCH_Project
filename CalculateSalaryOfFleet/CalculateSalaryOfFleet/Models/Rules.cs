using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Rules
    {
        public int RuleId { get; set; }
        public int? RuleFrom { get; set; }
        public int? RuleTo { get; set; }
        public double RuleNumber { get; set; }
        public long MoneyFirstJob { get; set; }
    }
}
