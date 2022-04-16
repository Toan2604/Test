using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_PrintDirectOrderDTO : DataDTO
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
        public decimal TotalTaxAmount { get; set; }
        public string SubTotalString { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public string Discount { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string TotalTaxString { get; set; }
        public string TotalText { get; set; }
        public string Note { get; set; }
        public MobileDirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public MobileDirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public MobileDirectSalesOrder_StoreDTO SellerStore { get; set; }
        public List<MobileDirectSalesOrder_PrintDirectOrderContentDTO> Contents { get; set; }
        public List<MobileDirectSalesOrder_PrintDirectOrderPromotionDTO> Promotions { get; set; }
        public MobileDirectSalesOrder_PrintDirectOrderDTO() { }
        public MobileDirectSalesOrder_PrintDirectOrderDTO(DirectSalesOrder DirectSalesOrder)
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
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new MobileDirectSalesOrder_StoreDTO(DirectSalesOrder.BuyerStore);
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new MobileDirectSalesOrder_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.Contents = DirectSalesOrder.DirectSalesOrderContents?.Select(x => new MobileDirectSalesOrder_PrintDirectOrderContentDTO(x)).ToList();
            this.Promotions = DirectSalesOrder.DirectSalesOrderPromotions?.Select(x => new MobileDirectSalesOrder_PrintDirectOrderPromotionDTO(x)).ToList();
            this.Errors = DirectSalesOrder.Errors;
        }
    }
}
