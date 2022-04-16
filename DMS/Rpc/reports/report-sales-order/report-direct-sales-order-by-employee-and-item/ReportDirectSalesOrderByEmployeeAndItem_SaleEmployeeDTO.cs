using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_employee_and_item
{
    public class ReportDirectSalesOrderByEmployeeAndItem_SaleEmployeeDTO : DataDTO
    {
        public long STT { get; set; }
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<ReportDirectSalesOrderByEmployeeAndItem_ItemDTO> Items { get; set; }
    }
}
