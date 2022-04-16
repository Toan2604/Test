using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_ExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<IndirectSalesOrder_ExportContentDTO> Contents { get; set; }
    }

    public class IndirectSalesOrder_ExportContentDTO : DataDTO
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
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string EditPrice { get; set; }
        public IndirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public IndirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public IndirectSalesOrder_RequestStateDTO RequestState { get; set; }
        public IndirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public IndirectSalesOrder_StoreDTO SellerStore { get; set; }
        public IndirectSalesOrder_ExportContentDTO() { }
        public IndirectSalesOrder_ExportContentDTO(IndirectSalesOrder_IndirectSalesOrderDTO IndirectSalesOrder_IndirectSalesOrderDTO)
        {
            this.Id = IndirectSalesOrder_IndirectSalesOrderDTO.Id;
            this.Code = IndirectSalesOrder_IndirectSalesOrderDTO.Code;
            this.OrderDate = IndirectSalesOrder_IndirectSalesOrderDTO.OrderDate;
            this.CreatedAt = IndirectSalesOrder_IndirectSalesOrderDTO.CreatedAt;
            this.DeliveryDate = IndirectSalesOrder_IndirectSalesOrderDTO.DeliveryDate;
            this.SubTotal = IndirectSalesOrder_IndirectSalesOrderDTO.SubTotal;
            this.GeneralDiscountAmount = IndirectSalesOrder_IndirectSalesOrderDTO.GeneralDiscountAmount;
            this.Total = IndirectSalesOrder_IndirectSalesOrderDTO.Total;
            this.BuyerStore = IndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore;
            this.EditedPriceStatus = IndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatus;
            this.RequestState = IndirectSalesOrder_IndirectSalesOrderDTO.RequestState;
            this.SaleEmployee = IndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee;
            this.SellerStore = IndirectSalesOrder_IndirectSalesOrderDTO.SellerStore;
            this.Errors = IndirectSalesOrder_IndirectSalesOrderDTO.Errors;
        }
    }
}
