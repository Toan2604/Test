using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public Sync_StoreGroupingDTO StoreGrouping { get; set; }

        public Sync_StoreStoreGroupingMappingDTO() { }
        public Sync_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new Sync_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
        }
    }

    public class Sync_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}