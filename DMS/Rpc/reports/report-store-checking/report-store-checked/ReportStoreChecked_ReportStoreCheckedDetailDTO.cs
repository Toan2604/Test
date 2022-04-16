using System;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_checked
{
    public class ReportStoreChecked_ReportStoreCheckedDetailDTO : DataDTO
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string IndirectSalesOrderCode { get; set; }
        public decimal Sales { get; set; }
        public string ImagePath { get; set; }
        public string StoreProblemCode { get; set; }
        public string CompetitorProblemCode { get; set; }
    }

    public class ReportStoreChecked_ReportStoreCheckedDetailFilterDTO : FilterDTO
    {
        public long AppUserId { get; set; }
        public DateTime Date { get; set; }
    }
}
