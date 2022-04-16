using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_ExportDetailDTO : DataDTO
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
        public string SellerStoreCode { get; set; }
        public string SellerStoreName { get; set; }
        public string ERouteCode { get; set; }
        public string ERouteName { get; set; }
        public string MonitorUserName { get; set; }
        public string MonitorName { get; set; }
        public string SalesEmployeeUserName { get; set; }
        public string SalesEmployeeName { get; set; }
        public string OrganizationName { get; set; }
        public string Note { get; set; }
        public string RequestStateName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public string EditedPriceStatusName { get; set; }
        public string CreatedInCheckin { get; set; }
        public List<IndirectSalesOrder_ExportDetailContentDTO> Contents { get; set; }
    }

    public class IndirectSalesOrder_ExportDetailContentDTO : DataDTO
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
        public string SellerStoreCode { get; set; }
        public string SellerStoreName { get; set; }
        public string ProductGroupings { get; set; }
        public string ERouteCode { get; set; }
        public string ERouteName { get; set; }
        public string MonitorUserName { get; set; }
        public string MonitorName { get; set; }
        public string SalesEmployeeUserName { get; set; }
        public string SalesEmployeeName { get; set; }
        public string OrganizationName { get; set; }
        public string Note { get; set; }
        public string RequestStateName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemOrderType { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string EditedPriceStatusName { get; set; }
        public string CreatedInCheckin { get; set; }
    }
}
