using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_PrintIndirectOrderPromotionDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityString { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string RequestedQuantityString { get; set; }
        public long? Factor { get; set; }
        public MobileIndirectSalesOrder_ItemDTO Item { get; set; }
        public MobileIndirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public MobileIndirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public MobileIndirectSalesOrder_PrintIndirectOrderPromotionDTO() { }
        public MobileIndirectSalesOrder_PrintIndirectOrderPromotionDTO(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            this.Id = IndirectSalesOrderPromotion.Id;
            this.Quantity = IndirectSalesOrderPromotion.Quantity;
            this.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
            this.Factor = IndirectSalesOrderPromotion.Factor;
            this.Item = IndirectSalesOrderPromotion.Item == null ? null : new MobileIndirectSalesOrder_ItemDTO(IndirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new MobileIndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderPromotion.UnitOfMeasure == null ? null : new MobileIndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = IndirectSalesOrderPromotion.Errors;
        }
    }
}
