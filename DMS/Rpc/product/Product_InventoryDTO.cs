using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_InventoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Product_ItemDTO Item { get; set; }

        public Product_InventoryDTO() { }
        public Product_InventoryDTO(Inventory Inventory)
        {
            this.Id = Inventory.Id;
            this.WarehouseId = Inventory.WarehouseId;
            this.ItemId = Inventory.ItemId;
            this.SaleStock = Inventory.SaleStock;
            this.AccountingStock = Inventory.AccountingStock;
            this.UpdatedAt = Inventory.UpdatedAt;
            this.Item = Inventory.Item == null ? null : new Product_ItemDTO(Inventory.Item);
            this.Errors = Inventory.Errors;
        }
    }

    public class Product_InventoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter WarehouseId { get; set; }
        public IdFilter ItemId { get; set; }
        public DecimalFilter SaleStock { get; set; }
        public DecimalFilter AccountingStock { get; set; }
        public InventoryOrder OrderBy { get; set; }
    }
}
