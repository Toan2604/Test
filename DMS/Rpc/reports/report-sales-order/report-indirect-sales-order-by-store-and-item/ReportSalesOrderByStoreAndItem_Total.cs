using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public class ReportSalesOrderByStoreAndItem_TotalDTO : DataDTO
    {
        public decimal TotalSalesStock { get; set; }
        public decimal TotalPromotionStock { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
