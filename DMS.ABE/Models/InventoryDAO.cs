using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class InventoryDAO
    {
        public InventoryDAO()
        {
            InventoryHistories = new HashSet<InventoryHistoryDAO>();
        }

        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ItemDAO Item { get; set; }
        public virtual WarehouseDAO Warehouse { get; set; }
        public virtual ICollection<InventoryHistoryDAO> InventoryHistories { get; set; }
    }
}
