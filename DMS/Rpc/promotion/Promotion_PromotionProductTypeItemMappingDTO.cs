using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionProductTypeItemMappingDTO : DataDTO
    {
        public long PromotionProductTypeId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionProductTypeItemMappingDTO() { }
        public Promotion_PromotionProductTypeItemMappingDTO(PromotionProductTypeItemMapping PromotionProductItemMapping)
        {
            this.PromotionProductTypeId = PromotionProductItemMapping.PromotionProductTypeId;
            this.ItemId = PromotionProductItemMapping.ItemId;
            this.Quantity = PromotionProductItemMapping.Quantity;
            this.Item = PromotionProductItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionProductItemMapping.Item);
        }
    }
}
