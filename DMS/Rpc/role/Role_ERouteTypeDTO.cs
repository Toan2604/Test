using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_ERouteTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Role_ERouteTypeDTO() { }
        public Role_ERouteTypeDTO(ERouteType ERouteType)
        {
            this.Id = ERouteType.Id;
            this.Code = ERouteType.Code;
            this.Name = ERouteType.Name;
        }
    }
}
