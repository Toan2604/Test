using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductGroupingItemMappingDTO : DataDTO
    {
        public long PromotionProductGroupingId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionProductGroupingItemMappingDTO() { }
        public Promotion_PromotionProductGroupingItemMappingDTO(PromotionProductGroupingItemMapping PromotionProductItemMapping)
        {
            this.PromotionProductGroupingId = PromotionProductItemMapping.PromotionProductGroupingId;
            this.ItemId = PromotionProductItemMapping.ItemId;
            this.Quantity = PromotionProductItemMapping.Quantity;
            this.Item = PromotionProductItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionProductItemMapping.Item);
        }
    }
}
