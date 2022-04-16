using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_general
{
    public class ReportStoreGeneral_StoreDetailDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string StoreStatusName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long CheckingPlannedCounter => StoreCheckingPlannedIds?.Count() ?? 0;
        public long CheckingUnPlannedCounter => StoreCheckingUnPlannedIds?.Count() ?? 0;
        public string TotalCheckingTime { get; set; }
        public DateTime? FirstChecking { get; set; }
        public string eFirstChecking => FirstChecking == DateTime.MinValue || FirstChecking == null ? "" : FirstChecking.Value.ToString("dd-MM-yyyy");
        public DateTime? LastChecking { get; set; }
        public string eLastChecking => LastChecking == DateTime.MinValue || LastChecking == null ? "" : LastChecking.Value.ToString("dd-MM-yyyy");
        public string EmployeeLastChecking { get; set; }
        public long IndirectSalesOrderCounter => SalesOrderIds?.Count() ?? 0;
        public long SKUCounter => SKUItemIds?.Count() ?? 0;
        public decimal TotalRevenue { get; set; }
        public DateTime? LastOrder { get; set; }
        public string LastOrderDisplay => LastOrder == DateTime.MinValue || LastOrder == null ? "" : LastOrder.Value.ToString("dd-MM-yyyy");
        internal HashSet<long> StoreCheckingPlannedIds { get; set; }
        internal HashSet<long> StoreCheckingUnPlannedIds { get; set; }
        internal List<long> SKUItemIds { get; set; }
        internal HashSet<long> SalesOrderIds { get; set; }
    }
}
