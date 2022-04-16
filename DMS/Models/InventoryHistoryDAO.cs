using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class InventoryHistoryDAO
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public decimal OldSaleStock { get; set; }
        public decimal SaleStock { get; set; }
        public decimal OldAccountingStock { get; set; }
        public decimal AccountingStock { get; set; }
        public long AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual InventoryDAO Inventory { get; set; }
    }
}
