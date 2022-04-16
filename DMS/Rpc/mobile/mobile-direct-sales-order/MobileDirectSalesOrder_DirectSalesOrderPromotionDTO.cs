using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_DirectSalesOrderPromotionDTO : DataDTO
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
        public MobileDirectSalesOrder_ItemDTO Item { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public MobileDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public MobileDirectSalesOrder_DirectSalesOrderPromotionDTO() { }
        public MobileDirectSalesOrder_DirectSalesOrderPromotionDTO(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            this.Id = DirectSalesOrderPromotion.Id;
            this.DirectSalesOrderId = DirectSalesOrderPromotion.DirectSalesOrderId;
            this.ItemId = DirectSalesOrderPromotion.ItemId;
            this.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
            this.Quantity = DirectSalesOrderPromotion.Quantity;
            this.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            this.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            this.Note = DirectSalesOrderPromotion.Note;
            this.Factor = DirectSalesOrderPromotion.Factor;
            this.Item = DirectSalesOrderPromotion.Item == null ? null : new MobileDirectSalesOrder_ItemDTO(DirectSalesOrderPromotion.Item);
            this.PrimaryUnitOfMeasure = DirectSalesOrderPromotion.PrimaryUnitOfMeasure == null ? null : new MobileDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = DirectSalesOrderPromotion.UnitOfMeasure == null ? null : new MobileDirectSalesOrder_UnitOfMeasureDTO(DirectSalesOrderPromotion.UnitOfMeasure);
            this.Errors = DirectSalesOrderPromotion.Errors;
        }
    }

    public class MobileDirectSalesOrder_DirectSalesOrderPromotionFilterDTO : FilterDTO
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