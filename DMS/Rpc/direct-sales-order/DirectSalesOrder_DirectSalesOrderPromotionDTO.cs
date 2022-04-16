using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_DirectSalesOrderPromotionDTO : DataDTO
    {
        public long Id { get; set; }
        public long DirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Note { get; set; }
        public long? Factor { get; set; }
        public long? InventoryCheckStateId { get; set; }
        public DirectSalesOrder_ItemDTO Item { get; set; }
        public DirectSalesOrder_InventoryCheckStateDTO InventoryCheckState { get; set; }
        public DirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public DirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public DirectSalesOrder_DirectSalesOrderPromotionDTO() { }
        public DirectSalesOrder_DirectSalesOrderPromotionDTO(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            this.Id = DirectSalesOrderPromotion.Id;
            this.DirectSalesOrderId = DirectSalesOrderPromotion.DirectSalesOrderId;
            this.ItemId = DirectSalesOrderPromotion.ItemId;
            this.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
            this.Quantity = DirectSalesOrderPromotion.Quantity;
            this.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            this.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            this.Note = DirectSalesOrderPromotion.Note;
            this.InventoryCheckStateId = DirectSalesOrderPromotion.InventoryCheckStateId;
            this.Factor = DirectSalesOrderPromotion.Factor;
            this.Item = DirectSalesOrderPromotion.Item == null ? null : new DirectSalesOrder_ItemDTO(DirectSalesOrderPromotion.Item);
            this.InventoryCheckState = DirectSalesOrderPromotion.InventoryCheckState == null ? null : new DirectSalesOrder_InventoryCheckStateDTO(DirectSalesOrderPromotion.InventoryCheckState);
            this.PrimaryUnitOfMeasure = DirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new DirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = DirectSalesOrderPromotion.UnitOfMeasure == null ? null : new DirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = DirectSalesOrderPromotion.Errors;
        }
    }

    public class DirectSalesOrder_DirectSalesOrderPromotionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter DirectSalesOrderId { get; set; }

        public IdFilter ItemId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public LongFilter Quantity { get; set; }

        public IdFilter PrimaryUnitOfMeasureId { get; set; }

        public LongFilter RequestedQuantity { get; set; }

        public StringFilter Note { get; set; }

        public DirectSalesOrderPromotionOrder OrderBy { get; set; }
    }
}