using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionComboInItemMappingDTO : DataDTO
    {
        public long PromotionComboId { get; set; }
        public long ItemId { get; set; }
        public long From { get; set; }
        public long? To { get; set; }
        public Promotion_ItemDTO Item { get; set; }
        public Promotion_PromotionComboInItemMappingDTO() { }
        public Promotion_PromotionComboInItemMappingDTO(PromotionComboInItemMapping PromotionProductItemMapping)
        {
            this.PromotionComboId = PromotionProductItemMapping.PromotionComboId;
            this.ItemId = PromotionProductItemMapping.ItemId;
            this.From = PromotionProductItemMapping.From;
            this.To = PromotionProductItemMapping.To;
            this.Item = PromotionProductItemMapping.Item == null ? null : new Promotion_ItemDTO(PromotionProductItemMapping.Item);
        }
    }
}
