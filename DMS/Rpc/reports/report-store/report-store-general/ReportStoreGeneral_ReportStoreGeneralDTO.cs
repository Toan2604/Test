using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_general
{
    public class ReportStoreGeneral_ReportStoreGeneralDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportStoreGeneral_StoreDetailDTO> Stores { get; set; }
    }


    public class ReportStoreGeneral_ReportStoreGeneralFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CheckIn { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (StoreId != null && StoreId.HasValue) ||
            (StoreTypeId != null && StoreTypeId.HasValue) ||
            (StoreGroupingId != null && StoreGroupingId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue) ||
            (CheckIn != null && CheckIn.HasValue);
    }

}
