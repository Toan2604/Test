using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneral_TotalDTO : DataDTO
    {
        public decimal TotalDiscount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
