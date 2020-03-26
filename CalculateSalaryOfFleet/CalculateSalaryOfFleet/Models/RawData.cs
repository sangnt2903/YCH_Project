using System;
using System.Collections.Generic;

namespace CalculateSalaryOfFleet.Models
{
    public partial class RawData
    {
        public int RawId { get; set; }
        public string OrderNo { get; set; }
        public string JobNo { get; set; }
        public string DeliveryCustCode { get; set; }
        public string DeliveryAddress { get; set; }
        public string ServiceLevel { get; set; }
        public string TruckId { get; set; }
        public string TruckType { get; set; }
        public string TransportAgent { get; set; }
        public DateTime? AtdcompleteDate { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
    }
}
