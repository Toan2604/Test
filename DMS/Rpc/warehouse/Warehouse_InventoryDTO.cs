using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_InventoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        public Warehouse_ItemDTO Item { get; set; }
        public List<Warehouse_InventoryHistoryDTO> InventoryHistories { get; set; }

        public Warehouse_InventoryDTO() { }
        public Warehouse_InventoryDTO(Inventory Inventory)
        {
            this.Id = Inventory.Id;
            this.WarehouseId = Inventory.WarehouseId;
            this.ItemId = Inventory.ItemId;
            this.SaleStock = Inventory.SaleStock;
            this.AccountingStock = Inventory.AccountingStock;
            this.Item = Inventory.Item == null ? null : new Warehouse_ItemDTO(Inventory.Item);
            this.InventoryHistories = Inventory.InventoryHistories?.Select(i => new Warehouse_InventoryHistoryDTO(i)).ToList();
            this.Errors = Inventory.Errors;
        }
    }

    public class Warehouse_InventoryFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter WarehouseId { get; set; }

        public IdFilter ItemId { get; set; }

        public DecimalFilter SaleStock { get; set; }

        public DecimalFilter AccountingStock { get; set; }

        public InventoryOrder OrderBy { get; set; }
    }
}