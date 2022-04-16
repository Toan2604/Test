using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_RequestStateDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public ERoute_RequestStateDTO() { }
        public ERoute_RequestStateDTO(RequestState RequestState)
        {

            this.Id = RequestState.Id;

            this.Code = RequestState.Code;

            this.Name = RequestState.Name;

            this.Errors = RequestState.Errors;
        }
    }

    public class ERoute_RequestStateFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public RequestStateOrder OrderBy { get; set; }
    }
}