using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_IndirectSalesOrderPromotionDTO : DataDTO
    {
        public long Id { get; set; }
        public long IndirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Note { get; set; }
        public long? Factor { get; set; }
        public IndirectSalesOrder_ItemDTO Item { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public IndirectSalesOrder_IndirectSalesOrderPromotionDTO() { }
        public IndirectSalesOrder_IndirectSalesOrderPromotionDTO(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            this.Id = IndirectSalesOrderPromotion.Id;
            this.IndirectSalesOrderId = IndirectSalesOrderPromotion.IndirectSalesOrderId;
            this.ItemId = IndirectSalesOrderPromotion.ItemId;
            this.UnitOfMeasureId = IndirectSalesOrderPromotion.UnitOfMeasureId;
            this.Quantity = IndirectSalesOrderPromotion.Quantity;
            this.PrimaryUnitOfMeasureId = IndirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            this.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
            this.Note = IndirectSalesOrderPromotion.Note;
            this.Factor = IndirectSalesOrderPromotion.Factor;
            this.Item = IndirectSalesOrderPromotion.Item == null ? null : new IndirectSalesOrder_ItemDTO(IndirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderPromotion.UnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = IndirectSalesOrderPromotion.Errors;
        }
    }

    public class IndirectSalesOrder_IndirectSalesOrderPromotionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter IndirectSalesOrderId { get; set; }

        public IdFilter ItemId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public LongFilter Quantity { get; set; }

        public IdFilter PrimaryUnitOfMeasureId { get; set; }

        public LongFilter RequestedQuantity { get; set; }

        public StringFilter Note { get; set; }

        public IndirectSalesOrderPromotionOrder OrderBy { get; set; }
    }
}