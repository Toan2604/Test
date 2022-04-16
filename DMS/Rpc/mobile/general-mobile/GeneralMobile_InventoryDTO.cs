using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_InventoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        public GeneralMobile_WarehouseDTO Warehouse { get; set; }

        public GeneralMobile_InventoryDTO() { }
        public GeneralMobile_InventoryDTO(Inventory Inventory)
        {
            this.Id = Inventory.Id;
            this.WarehouseId = Inventory.WarehouseId;
            this.ItemId = Inventory.ItemId;
            this.SaleStock = Inventory.SaleStock;
            this.AccountingStock = Inventory.AccountingStock;
            this.Warehouse = Inventory.Warehouse == null ? null : new GeneralMobile_WarehouseDTO(Inventory.Warehouse);
            this.Errors = Inventory.Errors;
        }
    }

    public class GeneralMobile_InventoryFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter WarehouseId { get; set; }

        public IdFilter ItemId { get; set; }

        public DecimalFilter SaleStock { get; set; }

        public DecimalFilter AccountingStock { get; set; }

        public InventoryOrder OrderBy { get; set; }
    }
}