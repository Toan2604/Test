using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_ERouteTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ERouteChangeRequest_ERouteTypeDTO() { }
        public ERouteChangeRequest_ERouteTypeDTO(ERouteType ERouteType)
        {
            this.Id = ERouteType.Id;
            this.Code = ERouteType.Code;
            this.Name = ERouteType.Name;
            this.Errors = ERouteType.Errors;
        }
    }

    public class ERouteChangeRequest_ERouteTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ERouteTypeOrder OrderBy { get; set; }
    }
}
