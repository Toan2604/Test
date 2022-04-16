using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_AppUserRoleMappingDTO : DataDTO
    {
        public long AppUserId { get; set; }
        public long RoleId { get; set; }
        public Role_AppUserDTO AppUser { get; set; }

        public Role_AppUserRoleMappingDTO() { }
        public Role_AppUserRoleMappingDTO(AppUserRoleMapping AppUserRoleMapping)
        {
            this.AppUserId = AppUserRoleMapping.AppUserId;
            this.RoleId = AppUserRoleMapping.RoleId;
            this.AppUser = AppUserRoleMapping.AppUser == null ? null : new Role_AppUserDTO(AppUserRoleMapping.AppUser);
        }
    }

    public class Role_AppUserRoleMappingFilterDTO : FilterDTO
    {

        public IdFilter AppUserId { get; set; }

        public IdFilter RoleId { get; set; }

        public AppUserRoleMappingOrder OrderBy { get; set; }
    }
}
