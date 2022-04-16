using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public Store_StoreGroupingDTO StoreGrouping { get; set; }

        public Store_StoreStoreGroupingMappingDTO() { }
        public Store_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new Store_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
        }
    }

    public class Store_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}