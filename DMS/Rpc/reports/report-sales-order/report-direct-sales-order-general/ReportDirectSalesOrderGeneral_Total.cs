using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    public class ReportDirectSalesOrderGeneral_TotalDTO : DataDTO
    {
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal PromotionValue { get; set; }
        public decimal TotalAfterPromotion { get; set; }
    }
}
