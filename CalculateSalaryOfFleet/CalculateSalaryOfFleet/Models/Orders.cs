using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string JobNo { get; set; }
        public string TranportAgent { get; set; }
        public string DeliveryCustCode { get; set; }
        public DateTime AtdcompleteDate { get; set; }

        public DeliveryCustomers DeliveryCustCodeNavigation { get; set; }
        public Jobs JobNoNavigation { get; set; }
    }
}
