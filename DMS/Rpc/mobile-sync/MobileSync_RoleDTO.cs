using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_RoleDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileSync_RoleDTO() { }
        public MobileSync_RoleDTO(Role Role)
        {
            this.Id = Role.Id;
            this.Code = Role.Code;
            this.Name = Role.Name;
        }
    }
}
