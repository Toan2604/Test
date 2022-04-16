using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public MobileDirectSalesOrder_ImageDTO Image { get; set; }

        public MobileDirectSalesOrder_ItemImageMappingDTO() { }
        public MobileDirectSalesOrder_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new MobileDirectSalesOrder_ImageDTO(ItemImageMapping.Image);
        }
    }

    public class MobileDirectSalesOrder_ItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
