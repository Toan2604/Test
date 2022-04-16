using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionComboOutItemMappingDTO : DataDTO
    {
        public long PromotionComboId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionComboOutItemMappingDTO() { }
        public Promotion_PromotionComboOutItemMappingDTO(PromotionComboOutItemMapping PromotionComboOutItemMapping)
        {
            this.PromotionComboId = PromotionComboOutItemMapping.PromotionComboId;
            this.ItemId = PromotionComboOutItemMapping.ItemId;
            this.Quantity = PromotionComboOutItemMapping.Quantity;
            this.Item = PromotionComboOutItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionComboOutItemMapping.Item);
        }
    }
}
