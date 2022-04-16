using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public MobileIndirectSalesOrder_ImageDTO Image { get; set; }

        public MobileIndirectSalesOrder_ItemImageMappingDTO() { }
        public MobileIndirectSalesOrder_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new MobileIndirectSalesOrder_ImageDTO(ItemImageMapping.Image);
        }
    }

    public class MobileIndirectSalesOrder_ItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
