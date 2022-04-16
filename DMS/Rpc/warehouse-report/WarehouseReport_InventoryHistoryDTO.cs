using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReport_InventoryHistoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public decimal OldSaleStock { get; set; }
        public decimal SaleStock { get; set; }
        public decimal OldAccountingStock { get; set; }
        public decimal AccountingStock { get; set; }
        public long AppUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public WarehouseReport_AppUserDTO AppUser { get; set; }
        public WarehouseReport_InventoryDTO Inventory { get; set; }
        public WarehouseReport_InventoryHistoryDTO() { }
        public WarehouseReport_InventoryHistoryDTO(InventoryHistory InventoryHistory)
        {
            this.Id = InventoryHistory.Id;
            this.InventoryId = InventoryHistory.InventoryId;
            this.OldSaleStock = InventoryHistory.OldSaleStock;
            this.SaleStock = InventoryHistory.SaleStock;
            this.OldAccountingStock = InventoryHistory.OldAccountingStock;
            this.AccountingStock = InventoryHistory.AccountingStock;
            this.AppUserId = InventoryHistory.AppUserId;
            this.UpdateTime = InventoryHistory.UpdateTime;
            this.AppUser = InventoryHistory.AppUser == null ? null : new WarehouseReport_AppUserDTO(InventoryHistory.AppUser);
            this.Inventory = InventoryHistory.Inventory == null ? null : new WarehouseReport_InventoryDTO(InventoryHistory.Inventory);
            this.Errors = InventoryHistory.Errors;
        }
    }

    public class WarehouseReport_InventoryHistoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter InventoryId { get; set; }
        public DecimalFilter OldSaleStock { get; set; }
        public DecimalFilter SaleStock { get; set; }
        public DecimalFilter OldAccountingStock { get; set; }
        public DecimalFilter AccountingStock { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter UpdateTime { get; set; }
        public InventoryHistoryOrder OrderBy { get; set; }
    }
}
