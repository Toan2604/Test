using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_InventoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        //public List<Warehouse_InventoryHistoryDTO> InventoryHistories { get; set; }

        public Sync_InventoryDTO() { }
        public Sync_InventoryDTO(Inventory Inventory)
        {
            this.Id = Inventory.Id;
            this.WarehouseId = Inventory.WarehouseId;
            this.ItemId = Inventory.ItemId;
            this.SaleStock = Inventory.SaleStock;
            this.AccountingStock = Inventory.AccountingStock;
            //this.InventoryHistories = Inventory.InventoryHistories?.Select(i => new Warehouse_InventoryHistoryDTO(i)).ToList();
            this.Errors = Inventory.Errors;
        }
    }

    public class Sync_InventoryFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter WarehouseId { get; set; }

        public IdFilter ItemId { get; set; }

        public DecimalFilter SaleStock { get; set; }

        public DecimalFilter AccountingStock { get; set; }

        public InventoryOrder OrderBy { get; set; }
    }
}