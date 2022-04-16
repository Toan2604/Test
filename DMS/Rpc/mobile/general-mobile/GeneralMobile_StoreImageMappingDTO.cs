using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreImageMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long ImageId { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public GeneralMobile_StoreImageMappingDTO() { }
        public GeneralMobile_StoreImageMappingDTO(StoreImageMapping StoreImageMapping)
        {
            this.StoreId = StoreImageMapping.StoreId;
            this.ImageId = StoreImageMapping.ImageId;
            this.Image = StoreImageMapping.Image == null ? null : new GeneralMobile_ImageDTO(StoreImageMapping.Image);
        }

        public class Store_StoreImageMappingFilterDTO : FilterDTO
        {
            public IdFilter StoreId { get; set; }

            public IdFilter ImageId { get; set; }

            public StoreImageMappingOrder OrderBy { get; set; }
        }
    }
}
