using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_ItemImageMappingDTO : DataDTO
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public WebDirectSalesOrder_ImageDTO Image { get; set; }
        public WebDirectSalesOrder_ItemImageMappingDTO() { }
        public WebDirectSalesOrder_ItemImageMappingDTO(ItemImageMapping ItemImageMapping)
        {
            this.ItemId = ItemImageMapping.ItemId;
            this.ImageId = ItemImageMapping.ImageId;
            this.Image = ItemImageMapping.Image == null ? null : new WebDirectSalesOrder_ImageDTO(ItemImageMapping.Image);
        }
    }
    public class WebDirectSalesOrder_ItemImageMappingFilterDTO : FilterDTO
    {
        public IdFilter ItemId { get; set; }
        public IdFilter ImageId { get; set; }
        public ItemImageMappingOrder OrderBy { get; set; }
    }
}
