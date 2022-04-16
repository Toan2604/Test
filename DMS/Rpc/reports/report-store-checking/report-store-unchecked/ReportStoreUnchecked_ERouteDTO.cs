using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_unchecked
{
    public class ReportStoreUnChecked_ERouteDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ReportStoreUnChecked_ERouteDTO() { }
        public ReportStoreUnChecked_ERouteDTO(ERoute ERoute)
        {
            this.Id = ERoute.Id;
            this.Code = ERoute.Code;
            this.Name = ERoute.Name;
        }
    }

    public class ReportStoreUnChecked_ERouteFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
    }
}
