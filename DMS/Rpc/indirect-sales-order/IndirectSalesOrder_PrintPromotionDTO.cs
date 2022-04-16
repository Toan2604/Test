using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_PrintPromotionDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityString { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string RequestedQuantityString { get; set; }
        public long? Factor { get; set; }
        public IndirectSalesOrder_ItemDTO Item { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public IndirectSalesOrder_PrintPromotionDTO() { }
        public IndirectSalesOrder_PrintPromotionDTO(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            this.Id = IndirectSalesOrderPromotion.Id;
            this.Quantity = IndirectSalesOrderPromotion.Quantity;
            this.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
            this.Factor = IndirectSalesOrderPromotion.Factor;
            this.Item = IndirectSalesOrderPromotion.Item == null ? null : new IndirectSalesOrder_ItemDTO(IndirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderPromotion.UnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = IndirectSalesOrderPromotion.Errors;
        }
    }
}
