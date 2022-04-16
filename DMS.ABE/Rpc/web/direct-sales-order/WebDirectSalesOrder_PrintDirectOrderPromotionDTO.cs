using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_PrintDirectOrderPromotionDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityString { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string RequestedQuantityString { get; set; }
        public long? Factor { get; set; }
        public WebDirectSalesOrder_ItemDTO Item { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public WebDirectSalesOrder_PrintDirectOrderPromotionDTO() { }
        public WebDirectSalesOrder_PrintDirectOrderPromotionDTO(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            this.Id = DirectSalesOrderPromotion.Id;
            this.Quantity = DirectSalesOrderPromotion.Quantity;
            this.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            this.Factor = DirectSalesOrderPromotion.Factor;
            this.Item = DirectSalesOrderPromotion.Item == null ? null : new WebDirectSalesOrder_ItemDTO(DirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = DirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new WebDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = DirectSalesOrderPromotion.UnitOfMeasure == null ? null : new WebDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = DirectSalesOrderPromotion.Errors;
        }
    }
}
