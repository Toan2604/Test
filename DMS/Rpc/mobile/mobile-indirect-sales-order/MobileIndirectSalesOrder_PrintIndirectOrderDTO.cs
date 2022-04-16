using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_PrintIndirectOrderDTO : DataDTO
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
        public string TotalString { get; set; }
        public string TotalText { get; set; }
        public string Note { get; set; }
        public MobileIndirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public MobileIndirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public MobileIndirectSalesOrder_StoreDTO SellerStore { get; set; }
        public List<MobileIndirectSalesOrder_PrintIndirectOrderContentDTO> Contents { get; set; }
        public List<MobileIndirectSalesOrder_PrintIndirectOrderPromotionDTO> Promotions { get; set; }
        public MobileIndirectSalesOrder_PrintIndirectOrderDTO() { }
        public MobileIndirectSalesOrder_PrintIndirectOrderDTO(IndirectSalesOrder IndirectSalesOrder)
        {
            this.Id = IndirectSalesOrder.Id;
            this.Code = IndirectSalesOrder.Code;
            this.PhoneNumber = IndirectSalesOrder.PhoneNumber;
            this.StoreAddress = IndirectSalesOrder.StoreAddress;
            this.DeliveryAddress = IndirectSalesOrder.DeliveryAddress;
            this.OrderDate = IndirectSalesOrder.OrderDate;
            this.DeliveryDate = IndirectSalesOrder.DeliveryDate;
            this.SubTotal = IndirectSalesOrder.SubTotal;
            this.GeneralDiscountAmount = IndirectSalesOrder.GeneralDiscountAmount;
            this.Total = IndirectSalesOrder.Total;
            this.Note = IndirectSalesOrder.Note;
            this.BuyerStore = IndirectSalesOrder.BuyerStore == null ? null : new MobileIndirectSalesOrder_StoreDTO(IndirectSalesOrder.BuyerStore);
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new MobileIndirectSalesOrder_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.SellerStore = IndirectSalesOrder.SellerStore == null ? null : new MobileIndirectSalesOrder_StoreDTO(IndirectSalesOrder.SellerStore);
            this.Contents = IndirectSalesOrder.IndirectSalesOrderContents?.Select(x => new MobileIndirectSalesOrder_PrintIndirectOrderContentDTO(x)).ToList();
            this.Promotions = IndirectSalesOrder.IndirectSalesOrderPromotions?.Select(x => new MobileIndirectSalesOrder_PrintIndirectOrderPromotionDTO(x)).ToList();
            this.Errors = IndirectSalesOrder.Errors;
        }
    }
}
