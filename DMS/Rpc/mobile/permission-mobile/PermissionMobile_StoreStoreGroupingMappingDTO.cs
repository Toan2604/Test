using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public PermissionMobile_StoreGroupingDTO StoreGrouping { get; set; }
        public PermissionMobile_StoreDTO Store { get; set; }

        public PermissionMobile_StoreStoreGroupingMappingDTO() { }
        public PermissionMobile_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new PermissionMobile_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
            this.Store = StoreStoreGroupingMapping.Store == null ? null : new PermissionMobile_StoreDTO(StoreStoreGroupingMapping.Store);
        }
    }

    public class PermissionMobile_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}
