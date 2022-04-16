using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_ERouteTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ERoute_ERouteTypeDTO() { }
        public ERoute_ERouteTypeDTO(ERouteType ERouteType)
        {
            this.Id = ERouteType.Id;
            this.Code = ERouteType.Code;
            this.Name = ERouteType.Name;
            this.Errors = ERouteType.Errors;
        }
    }

    public class ERoute_ERouteTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ERouteTypeOrder OrderBy { get; set; }
    }
}
