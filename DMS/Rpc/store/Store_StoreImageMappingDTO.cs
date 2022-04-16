using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreImageMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long ImageId { get; set; }
        public Store_ImageDTO Image { get; set; }
        public Store_StoreImageMappingDTO() { }
        public Store_StoreImageMappingDTO(StoreImageMapping StoreImageMapping)
        {
            this.StoreId = StoreImageMapping.StoreId;
            this.ImageId = StoreImageMapping.ImageId;
            this.Image = StoreImageMapping.Image == null ? null : new Store_ImageDTO(StoreImageMapping.Image);
        }

        public class Store_StoreImageMappingFilterDTO : FilterDTO
        {
            public IdFilter StoreId { get; set; }

            public IdFilter ImageId { get; set; }

            public StoreImageMappingOrder OrderBy { get; set; }
        }
    }
}
