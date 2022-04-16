using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_StoreExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ERoute_ProvinceDTO Province { get; set; }
        public ERoute_DistrictDTO District { get; set; }
        public ERoute_WardDTO Ward { get; set; }
        public ERoute_StoreStatusDTO StoreStatus { get; set; }
        public ERoute_StoreExportDTO() { }
        public ERoute_StoreExportDTO(Store Store)
        {
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.Errors = Store.Errors;
            this.District = Store.District == null ? null : new ERoute_DistrictDTO(Store.District);
            this.Province = Store.Province == null ? null : new ERoute_ProvinceDTO(Store.Province);
            this.Ward = Store.Ward == null ? null : new ERoute_WardDTO(Store.Ward);
            this.StoreStatus = Store.StoreStatus == null ? null : new ERoute_StoreStatusDTO(Store.StoreStatus);
        }
    }

}
