using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductItemMappingDTO : DataDTO
    {
        public long PromotionProductId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionProductItemMappingDTO() { }
        public Promotion_PromotionProductItemMappingDTO(PromotionProductItemMapping PromotionProductItemMapping)
        {
            this.PromotionProductId = PromotionProductItemMapping.PromotionProductId;
            this.ItemId = PromotionProductItemMapping.ItemId;
            this.Quantity = PromotionProductItemMapping.Quantity;
            this.Item = PromotionProductItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionProductItemMapping.Item);
        }
    }
}
