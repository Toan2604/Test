using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    public class ReportStoreStateChange_ReportStoreStateChangeDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public long Total { get; set; }
        public List<ReportStoreStateChange_ReportStoreStateChangeDetailDTO> Details { get; set; }
    }

    public class ReportStoreStateChange_ReportStoreStateChangeFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter PreviousStoreStatusId { get; set; }
        public StringFilter StoreAddress { get; set; }
        public DateFilter CreatedAt { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
    }
}
