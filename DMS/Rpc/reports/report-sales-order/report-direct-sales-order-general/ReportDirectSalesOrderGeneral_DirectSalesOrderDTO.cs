using System;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    public class ReportDirectSalesOrderGeneral_DirectSalesOrderDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string BuyerStoreCode { get; set; }
        public string BuyerStoreCodeDraft { get; set; }
        public string BuyerStoreName { get; set; }
        public string BuyerStoreStatusName { get; set; }
        public string BuyerStoreProvinceName { get; set; }
        public string BuyerStoreDistrictName { get; set; }
        public string SaleEmployeeName { get; set; }
        public string SaleEmployeeUsername { get; set; }
        public DateTime OrderDate { get; set; }
        public string eOrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string eCreatedAt { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxValue { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal PromotionValue { get; set; }
        public decimal TotalAfterPromotion { get; set; }
        public long? StoreCheckingId { get; set; }
        public bool? CreatedInCheckin { get; set; }
        public string CreatedInCheckinString { get; set; }

    }
}
