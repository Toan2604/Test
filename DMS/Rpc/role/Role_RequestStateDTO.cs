using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_RequestStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Role_RequestStateDTO() { }
        public Role_RequestStateDTO(RequestState RequestState)
        {
            this.Id = RequestState.Id;
            this.Code = RequestState.Code;
            this.Name = RequestState.Name;
        }
    }
}
