using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_DirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long BuyerStoreId { get; set; }
        public decimal? PromotionValue { get; set; }
        public decimal TotalAfterTax { get; set; }
        public decimal Total { get; set; }
        public PromotionCode_StoreDTO BuyerStore { get; set; }
        public PromotionCode_DirectSalesOrderDTO() { }
        public PromotionCode_DirectSalesOrderDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
            this.PromotionValue = DirectSalesOrder.PromotionValue;
            this.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
            this.Total = Math.Round(DirectSalesOrder.Total, 0);
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new PromotionCode_StoreDTO(DirectSalesOrder.BuyerStore);
            this.Errors = DirectSalesOrder.Errors;
        }
    }
}
