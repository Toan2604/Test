using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<DirectSalesOrder_ExportContentDTO> Contents { get; set; }
    }

    public class DirectSalesOrder_ExportContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDateString { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtString { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryDateString { get; set; }
        public decimal SubTotal { get; set; }
        public string SubTotalString { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public string Discount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string EditPrice { get; set; }
        public DirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public DirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public DirectSalesOrder_GeneralApprovalStateDTO GeneralApprovalState { get; set; }
        public DirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public DirectSalesOrder_ExportContentDTO() { }
        public DirectSalesOrder_ExportContentDTO(DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            this.Id = DirectSalesOrder_DirectSalesOrderDTO.Id;
            this.Code = DirectSalesOrder_DirectSalesOrderDTO.Code;
            this.OrderDate = DirectSalesOrder_DirectSalesOrderDTO.OrderDate;
            this.DeliveryDate = DirectSalesOrder_DirectSalesOrderDTO.DeliveryDate;
            this.CreatedAt = DirectSalesOrder_DirectSalesOrderDTO.CreatedAt;
            this.SubTotal = DirectSalesOrder_DirectSalesOrderDTO.SubTotal;
            this.GeneralDiscountAmount = DirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount;
            this.TotalTaxAmount = DirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount;
            this.Total = DirectSalesOrder_DirectSalesOrderDTO.Total;
            this.BuyerStore = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore;
            this.EditedPriceStatus = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus;
            this.GeneralApprovalState = DirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState;
            this.SaleEmployee = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee;
            this.Errors = DirectSalesOrder_DirectSalesOrderDTO.Errors;
        }
    }
}
