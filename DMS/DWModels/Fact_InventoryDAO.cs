using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_InventoryDAO
    {
        public long InventoryId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
    }
}
