using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    public class ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportDirectSalesOrderGeneral_DirectSalesOrderDTO> SalesOrders { get; set; }
    }

    public class ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public bool? CreatedInCheckin { get; set; }
      
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (AppUserId != null && AppUserId.HasValue) ||
            (BuyerStoreId != null && BuyerStoreId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue) ||
            (OrderDate != null && OrderDate.HasValue);
    }
}
