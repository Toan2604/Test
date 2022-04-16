using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public class ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO : DataDTO
    {
        internal long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportSalesOrderByStoreAndItem_StoreDTO> Stores { get; set; }
    }

    public class ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (OrderDate != null && OrderDate.HasValue) ||
            (StoreId != null && StoreId.HasValue) ||
            (StoreTypeId != null && StoreTypeId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (ItemId != null && ItemId.HasValue) ||
            (StoreGroupingId != null && StoreGroupingId.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue);
    }
}
