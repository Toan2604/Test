using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_checked
{
    public class ReportStoreChecked_StoreCheckingDTO : DataDTO
    {
        public long Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreCodeDraft { get; set; }
        public string StoreName { get; set; }
        public string StoreStatusName { get; set; }
        public string StoreAddress { get; set; }
        public long SaleEmployeeId { get; set; }
        public long StoreStatusId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string eCheckIn { get; set; }
        public string eCheckOut { get; set; }
        public string Duaration { get; set; }
        public string CheckInDistance { get; set; }
        public string CheckOutDistance { get; set; }
        public long ImageCounter { get; set; }
        public bool Planned { get; set; }
        public string ePlanned => Planned == true ? "x" : "";
        public long SalesOrderCounter { get; set; }
        public string eSalesOrder => SalesOrderCounter.ToString();
        public bool IsClosed { get; set; }
        public string Closed => IsClosed == true ? "x" : "";
    }

    public class ReportStoreChecked_StoreCheckingGroupByDateDTO : DataDTO
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string DayOfWeek { get; set; }
        public List<ReportStoreChecked_StoreCheckingDTO> StoreCheckings { get; set; }
    }
}
