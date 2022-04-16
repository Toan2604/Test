using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionDirectSalesOrderItemMappingDTO : DataDTO
    {
        public long PromotionDirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionDirectSalesOrderItemMappingDTO() { }
        public Promotion_PromotionDirectSalesOrderItemMappingDTO(PromotionDirectSalesOrderItemMapping PromotionDirectSalesOrderItemMapping)
        {
            this.PromotionDirectSalesOrderId = PromotionDirectSalesOrderItemMapping.PromotionDirectSalesOrderId;
            this.ItemId = PromotionDirectSalesOrderItemMapping.ItemId;
            this.Quantity = PromotionDirectSalesOrderItemMapping.Quantity;
            this.Item = PromotionDirectSalesOrderItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionDirectSalesOrderItemMapping.Item);
        }
    }
}
