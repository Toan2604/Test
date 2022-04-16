using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_ExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<WebDirectSalesOrder_ExportContentDTO> Contents { get; set; }
    }

    public class WebDirectSalesOrder_ExportContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDateString { get; set; }
        public string DeliveryAddress { get; set; }
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
        public WebDirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public WebDirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public WebDirectSalesOrder_RequestStateDTO RequestState { get; set; }
        public WebDirectSalesOrder_GeneralApprovalStateDTO GeneralApprovalState { get; set; }
        public WebDirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public WebDirectSalesOrder_ExportContentDTO() { }
        public WebDirectSalesOrder_ExportContentDTO(WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            this.Id = WebDirectSalesOrder_DirectSalesOrderDTO.Id;
            this.Code = WebDirectSalesOrder_DirectSalesOrderDTO.Code;
            this.OrderDate = WebDirectSalesOrder_DirectSalesOrderDTO.OrderDate;
            this.DeliveryDate = WebDirectSalesOrder_DirectSalesOrderDTO.DeliveryDate;
            this.DeliveryAddress = WebDirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress;
            this.SubTotal = WebDirectSalesOrder_DirectSalesOrderDTO.SubTotal;
            this.GeneralDiscountAmount = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount;
            this.TotalTaxAmount = WebDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount;
            this.Total = WebDirectSalesOrder_DirectSalesOrderDTO.Total;
            this.BuyerStore = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore;
            this.EditedPriceStatus = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus;
            this.RequestState = WebDirectSalesOrder_DirectSalesOrderDTO.RequestState;
            this.GeneralApprovalState = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState;
            this.SaleEmployee = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee;
            this.Errors = WebDirectSalesOrder_DirectSalesOrderDTO.Errors;
        }
    }
}
