using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public IndirectSalesOrder_ImageDTO Image { get; set; }

        public IndirectSalesOrder_ItemImageMappingDTO() { }
        public IndirectSalesOrder_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new IndirectSalesOrder_ImageDTO(ItemImageMapping.Image);
        }
    }

    public class IndirectSalesOrder_ItemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ItemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
