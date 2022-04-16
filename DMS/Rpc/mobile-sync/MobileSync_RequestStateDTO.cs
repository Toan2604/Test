using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_RequestStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileSync_RequestStateDTO() { }
        public MobileSync_RequestStateDTO(RequestState RequestState)
        {
            this.Id = RequestState.Id;
            this.Code = RequestState.Code;
            this.Name = RequestState.Name;
        }
    }
}
