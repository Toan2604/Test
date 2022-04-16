using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_StoreScoutingImageMappingDTO : DataDTO
    {
        public long StoreScoutingId { get; set; }
        public long ImageId { get; set; }
        public StoreScouting_ImageDTO Image { get; set; }
        public StoreScouting_StoreScoutingImageMappingDTO() { }
        public StoreScouting_StoreScoutingImageMappingDTO(StoreScoutingImageMapping StoreScoutingImageMapping)
        {
            this.StoreScoutingId = StoreScoutingImageMapping.StoreScoutingId;
            this.ImageId = StoreScoutingImageMapping.ImageId;
            this.Image = StoreScoutingImageMapping.Image == null ? null : new StoreScouting_ImageDTO(StoreScoutingImageMapping.Image);
        }

        public class Store_StoreImageMappingFilterDTO : FilterDTO
        {
            public IdFilter StoreScoutingId { get; set; }

            public IdFilter ImageId { get; set; }

            public StoreImageMappingOrder OrderBy { get; set; }
        }
    }
}
