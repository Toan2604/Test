using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_PrintDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public string sOrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string sDeliveryDate { get; set; }
        public decimal SubTotal { get; set; }
        public string SubTotalString { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public string Discount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public string Tax { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string TotalQuantityString { get; set; }
        public string TotalText { get; set; }
        public string Note { get; set; }
        public DirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public DirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public List<DirectSalesOrder_PrintContentDTO> Contents { get; set; }
        public List<DirectSalesOrder_PrintPromotionDTO> Promotions { get; set; }
        public DirectSalesOrder_PrintDTO() { }
        public DirectSalesOrder_PrintDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.PhoneNumber = DirectSalesOrder.PhoneNumber;
            this.StoreAddress = DirectSalesOrder.StoreAddress;
            this.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
            this.OrderDate = DirectSalesOrder.OrderDate;
            this.DeliveryDate = DirectSalesOrder.DeliveryDate;
            this.SubTotal = DirectSalesOrder.SubTotal;
            this.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
            this.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
            this.Total = DirectSalesOrder.Total;
            this.Note = DirectSalesOrder.Note;
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new DirectSalesOrder_StoreDTO(DirectSalesOrder.BuyerStore);
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new DirectSalesOrder_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.Contents = DirectSalesOrder.DirectSalesOrderContents?.Select(x => new DirectSalesOrder_PrintContentDTO(x)).ToList();
            this.Promotions = DirectSalesOrder.DirectSalesOrderPromotions?.Select(x => new DirectSalesOrder_PrintPromotionDTO(x)).ToList();
            this.Errors = DirectSalesOrder.Errors;
        }
    }
}
