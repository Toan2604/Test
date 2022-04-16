using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public class ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO : DataDTO
    {
        internal long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportDirectSalesOrderByStoreAndItem_StoreDTO> Stores { get; set; }
    }

    public class ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (OrderDate != null && OrderDate.HasValue) ||
            (StoreId != null && StoreId.HasValue) ||
            (StoreTypeId != null && StoreTypeId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (ItemId != null && ItemId.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue) ||
            (StoreGroupingId != null && StoreGroupingId.HasValue);
    }
}
