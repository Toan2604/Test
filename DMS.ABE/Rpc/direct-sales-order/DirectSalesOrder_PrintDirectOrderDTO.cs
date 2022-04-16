using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_PrintDirectOrderDTO : DataDTO
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
        public decimal Total { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public string TotalString { get; set; }
        public string TotalText { get; set; }
        public string TotalTaxString { get; set; }
        public string Note { get; set; }
        public DirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public DirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public DirectSalesOrder_StoreDTO SellerStore { get; set; }
        public List<DirectSalesOrder_PrintDirectOrderContentDTO> Contents { get; set; }
        public List<DirectSalesOrder_PrintDirectOrderPromotionDTO> Promotions { get; set; }
        public DirectSalesOrder_PrintDirectOrderDTO() { }
        public DirectSalesOrder_PrintDirectOrderDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.PhoneNumber = DirectSalesOrder.PhoneNumber;
            this.StoreAddress = DirectSalesOrder.StoreAddress;
            this.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
            this.OrderDate = DirectSalesOrder.OrderDate;
            this.DeliveryDate = DirectSalesOrder.DeliveryDate;
            this.SubTotal = DirectSalesOrder.SubTotal;
            this.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
            this.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
            this.Total = DirectSalesOrder.Total;
            this.Note = DirectSalesOrder.Note;
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new DirectSalesOrder_StoreDTO(DirectSalesOrder.BuyerStore);
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new DirectSalesOrder_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.Contents = DirectSalesOrder.DirectSalesOrderContents?.Select(x => new DirectSalesOrder_PrintDirectOrderContentDTO(x)).ToList();
            this.Promotions = DirectSalesOrder.DirectSalesOrderPromotions?.Select(x => new DirectSalesOrder_PrintDirectOrderPromotionDTO(x)).ToList();
            this.Errors = DirectSalesOrder.Errors;
        }
    }
}
