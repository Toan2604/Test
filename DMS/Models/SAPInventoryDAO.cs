using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SAPInventoryDAO
    {
        public string WarehouseCode { get; set; }
        public string ItemCode { get; set; }
        public double? OnHand { get; set; }
        public double? IsCommited { get; set; }
        public double? OnOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
