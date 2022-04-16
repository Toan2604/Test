using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_EditedPriceStatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileSync_EditedPriceStatusDTO() { }
        public MobileSync_EditedPriceStatusDTO(EditedPriceStatus EditedPriceStatus)
        {
            this.Id = EditedPriceStatus.Id;
            this.Code = EditedPriceStatus.Code;
            this.Name = EditedPriceStatus.Name;
        }
    }
}