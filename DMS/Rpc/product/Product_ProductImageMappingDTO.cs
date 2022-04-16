using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_ProductImageMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ImageId { get; set; }
        public Product_ImageDTO Image { get; set; }

        public Product_ProductImageMappingDTO() { }
        public Product_ProductImageMappingDTO(ProductImageMapping ProductImageMapping)
        {
            this.ProductId = ProductImageMapping.ProductId;
            this.ImageId = ProductImageMapping.ImageId;
            this.Image = ProductImageMapping.Image == null ? null : new Product_ImageDTO(ProductImageMapping.Image);
        }
    }

    public class Product_ProductImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ProductId { get; set; }

        public IdFilter ImageId { get; set; }

        public ProductImageMappingOrder OrderBy { get; set; }
    }
}