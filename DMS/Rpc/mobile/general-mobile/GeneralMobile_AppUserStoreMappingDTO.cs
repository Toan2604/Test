using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_AppUserStoreMappingDTO : DataDTO
    {
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }
        public GeneralMobile_AppUserDTO AppUser { get; set; }

        public GeneralMobile_AppUserStoreMappingDTO() { }
        public GeneralMobile_AppUserStoreMappingDTO(AppUserStoreMapping AppUserStoreMapping)
        {
            this.AppUserId = AppUserStoreMapping.AppUserId;
            this.StoreId = AppUserStoreMapping.StoreId;
            this.Store = AppUserStoreMapping.Store == null ? null : new GeneralMobile_StoreDTO(AppUserStoreMapping.Store);
            this.AppUser = AppUserStoreMapping.AppUser == null ? null : new GeneralMobile_AppUserDTO(AppUserStoreMapping.AppUser);
        }
    }

    public class GeneralMobile_AppUserStoreMappingFilterDTO : FilterDTO
    {

        public IdFilter AppUserId { get; set; }

        public IdFilter StoreId { get; set; }

        public AppUserStoreMappingOrder OrderBy { get; set; }
    }
}