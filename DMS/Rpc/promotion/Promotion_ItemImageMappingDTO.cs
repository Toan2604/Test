using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public Promotion_ImageDTO Image { get; set; }

        public Promotion_ItemImageMappingDTO() { }
        public Promotion_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new Promotion_ImageDTO(ItemImageMapping.Image);
        }
    }

    public class Promotion_ItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
