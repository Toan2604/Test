using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_unchecked
{
    public class ReportStoreUnchecked_ReportStoreUncheckedDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportStoreUnchecked_SaleEmployeeDTO> SaleEmployees { get; set; }
    }


    public class ReportStoreUnchecked_ReportStoreUncheckedFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ERouteId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public DateFilter Date { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }

        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (AppUserId != null && AppUserId.HasValue) ||
            (ERouteId != null && ERouteId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (Date != null && Date.HasValue);
    }

}
