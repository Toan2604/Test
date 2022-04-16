using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ExportDetailDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long RequestStateId { get; set; }
        public string Code { get; set; }
        public string OrderDate { get; set; }
        public string CreatedAt { get; set; }
        public string ApprovedAt { get; set; }
        public string DeliveryDate { get; set; }
        public string BuyerStoreCode { get; set; }
        public string BuyerStoreName { get; set; }
        public string BuyerStoreAddress { get; set; }
        public string BuyerStoreTypeName { get; set; }
        public string BuyerStoreGroupingName { get; set; }
        public string BuyerStoreProvinceName { get; set; }
        public string BuyerStoreDistrictName { get; set; }
        public string ERouteCode { get; set; }
        public string ERouteName { get; set; }
        public string MonitorUserName { get; set; }
        public string MonitorName { get; set; }
        public string SalesEmployeeUserName { get; set; }
        public string SalesEmployeeName { get; set; }
        public string OrganizationName { get; set; }
        public string Note { get; set; }
        public string GeneralApprovalStateName { get; set; }
        public string ErpApprovalStateName { get; set; }
        public string InventoryCheckStateName { get; set; }
        public string StoreBalanceCheckStateName { get; set; }
        public string DirectSalesOrderSourceTypeCheck { get; set; }
        public string EditedPriceStatusName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public List<DirectSalesOrder_ExportDetailContentDTO> Contents { get; set; }
    }

    public class DirectSalesOrder_ExportDetailContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long RequestStateId { get; set; }
        public string Code { get; set; }
        public string OrderDate { get; set; }
        public string CreatedAt { get; set; }
        public string ApprovedAt { get; set; }
        public string DeliveryDate { get; set; }
        public string BuyerStoreCode { get; set; }
        public string BuyerStoreName { get; set; }
        public string BuyerStoreAddress { get; set; }
        public string BuyerStoreProvinceName { get; set; }
        public string BuyerStoreDistrictName { get; set; }
        public string BuyerStoreTypeName { get; set; }
        public string BuyerStoreGroupingName { get; set; }
        public string ProductGroupings { get; set; }
        public string ERouteCode { get; set; }
        public string ERouteName { get; set; }
        public string MonitorUserName { get; set; }
        public string MonitorName { get; set; }
        public string SalesEmployeeUserName { get; set; }
        public string SalesEmployeeName { get; set; }
        public string OrganizationName { get; set; }
        public string Note { get; set; }
        public string GeneralApprovalStateName { get; set; }
        public string ErpApprovalStateName { get; set; }
        public string InventoryCheckStateName { get; set; }
        public string StoreBalanceCheckStateName { get; set; }
        public string DirectSalesOrderSourceTypeCheck { get; set; }
        public string EditedPriceStatusName { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemInventoryCheckStateName { get; set; }
        public string ItemOrderType { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
