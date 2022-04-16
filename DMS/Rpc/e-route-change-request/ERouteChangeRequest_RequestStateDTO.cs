using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequest_RequestStateDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public ERouteChangeRequest_RequestStateDTO() { }
        public ERouteChangeRequest_RequestStateDTO(RequestState RequestState)
        {

            this.Id = RequestState.Id;

            this.Code = RequestState.Code;

            this.Name = RequestState.Name;

            this.Errors = RequestState.Errors;
        }
    }

    public class ERouteChangeRequest_RequestStateFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public RequestStateOrder OrderBy { get; set; }
    }
}