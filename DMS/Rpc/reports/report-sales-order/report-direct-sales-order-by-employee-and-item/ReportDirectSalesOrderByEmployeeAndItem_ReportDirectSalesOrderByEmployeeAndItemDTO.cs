using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_employee_and_item
{
    public class ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportDirectSalesOrderByEmployeeAndItem_SaleEmployeeDTO> SaleEmployees { get; set; }
    }

    public class ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemFilterDTO : FilterDTO
    {
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ItemId { get; set; }
        public DateFilter OrderDate { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (AppUserId != null && AppUserId.HasValue) ||
            (ItemId != null && ItemId.HasValue) ||
            (OrderDate != null && OrderDate.HasValue);
    }
}
