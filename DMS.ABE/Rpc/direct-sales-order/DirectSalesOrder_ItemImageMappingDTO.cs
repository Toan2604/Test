using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public DirectSalesOrder_ImageDTO Image { get; set; }

        public DirectSalesOrder_ItemImageMappingDTO() { }
        public DirectSalesOrder_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new DirectSalesOrder_ImageDTO(ItemImageMapping.Image);
        }
    }

    public class DirectSalesOrder_ItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
