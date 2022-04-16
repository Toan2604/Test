using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_AppUserStoreMappingDTO : DataDTO
    {
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public Store_StoreDTO Store { get; set; }
        public Store_AppUserDTO AppUser { get; set; }

        public Store_AppUserStoreMappingDTO() { }
        public Store_AppUserStoreMappingDTO(AppUserStoreMapping AppUserStoreMapping)
        {
            this.AppUserId = AppUserStoreMapping.AppUserId;
            this.StoreId = AppUserStoreMapping.StoreId;
            this.Store = AppUserStoreMapping.Store == null ? null : new Store_StoreDTO(AppUserStoreMapping.Store);
            this.AppUser = AppUserStoreMapping.AppUser == null ? null : new Store_AppUserDTO(AppUserStoreMapping.AppUser);
        }
    }

    public class Store_AppUserStoreMappingFilterDTO : FilterDTO
    {

        public IdFilter AppUserId { get; set; }

        public IdFilter StoreId { get; set; }

        public AppUserStoreMappingOrder OrderBy { get; set; }
    }
}