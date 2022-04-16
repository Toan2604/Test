using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public class ReportDirectSalesOrderByStoreAndItem_TotalDTO : DataDTO
    {
        public decimal TotalSalesStock { get; set; }
        public decimal TotalPromotionStock { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
