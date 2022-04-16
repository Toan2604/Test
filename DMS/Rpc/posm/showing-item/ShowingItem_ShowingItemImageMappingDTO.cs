using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_item
{
    public class ShowingItem_ShowingItemImageMappingDTO : DataDTO
    {
        public long ShowingItemId { get; set; }
        public long ImageId { get; set; }
        public ShowingItem_ImageDTO Image { get; set; }

        public ShowingItem_ShowingItemImageMappingDTO() { }
        public ShowingItem_ShowingItemImageMappingDTO(ShowingItemImageMapping ShowingItemImageMapping)
        {
            this.ShowingItemId = ShowingItemImageMapping.ShowingItemId;
            this.ImageId = ShowingItemImageMapping.ImageId;
            this.Image = ShowingItemImageMapping.Image == null ? null : new ShowingItem_ImageDTO(ShowingItemImageMapping.Image);
        }
    }

    public class ShowingItem_ShowingItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ShowingItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ShowingItemImageMappingOrder OrderBy { get; set; }
    }
}