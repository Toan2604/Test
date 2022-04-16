using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_PrintDirectOrderPromotionDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityString { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string RequestedQuantityString { get; set; }
        public long? Factor { get; set; }
        public MobileDirectSalesOrder_ItemDTO Item { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public MobileDirectSalesOrder_PrintDirectOrderPromotionDTO() { }
        public MobileDirectSalesOrder_PrintDirectOrderPromotionDTO(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            this.Id = DirectSalesOrderPromotion.Id;
            this.Quantity = DirectSalesOrderPromotion.Quantity;
            this.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            this.Factor = DirectSalesOrderPromotion.Factor;
            this.Item = DirectSalesOrderPromotion.Item == null ? null : new MobileDirectSalesOrder_ItemDTO(DirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = DirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new MobileDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = DirectSalesOrderPromotion.UnitOfMeasure == null ? null : new MobileDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = DirectSalesOrderPromotion.Errors;
        }
    }
}
