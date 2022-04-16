using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public LuckyDraw_StoreGroupingDTO StoreGrouping { get; set; }

        public LuckyDraw_StoreStoreGroupingMappingDTO() { }
        public LuckyDraw_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new LuckyDraw_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
        }
    }

    public class LuckyDraw_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}