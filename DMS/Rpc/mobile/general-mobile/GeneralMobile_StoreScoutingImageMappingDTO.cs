using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreScoutingImageMappingDTO : DataDTO
    {
        public long StoreScoutingId { get; set; }
        public long ImageId { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public GeneralMobile_StoreScoutingImageMappingDTO() { }
        public GeneralMobile_StoreScoutingImageMappingDTO(StoreScoutingImageMapping StoreScoutingImageMapping)
        {
            this.StoreScoutingId = StoreScoutingImageMapping.StoreScoutingId;
            this.ImageId = StoreScoutingImageMapping.ImageId;
            this.Image = StoreScoutingImageMapping.Image == null ? null : new GeneralMobile_ImageDTO(StoreScoutingImageMapping.Image);
        }

        public class StoreScouting_StoreScoutingImageMappingFilterDTO : FilterDTO
        {
            public IdFilter StoreScoutingId { get; set; }

            public IdFilter ImageId { get; set; }

            public StoreScoutingImageMappingOrder OrderBy { get; set; }
        }
    }
}
