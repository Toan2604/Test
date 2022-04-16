using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_checker
{
    public class MonitorStoreChecker_StoreCheckingDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string DateDisplay => Date.ToString("dd-MM-yyyy");
        public long PlanCounter { get; set; }
        public long Unchecking { get; set; }
        public long SalesOrderCounter { get; set; }
        public long InternalCounter { get { return Internal == null ? 0 : Internal.Count; } }
        public long ExternalCounter { get { return External == null ? 0 : External.Count; } }
        public long ImageCounter { get { return Image == null ? 0 : Image.Count; } }
        public decimal RevenueCounter { get; set; }
        internal HashSet<long> Image { get; set; }
        internal HashSet<long> Internal { get; set; }
        internal HashSet<long> External { get; set; }
        internal bool HasValue => PlanCounter != 0 ||
            Unchecking != 0 ||
            SalesOrderCounter != 0 ||
            InternalCounter != 0 ||
            ExternalCounter != 0 ||
            ImageCounter != 0 ||
            RevenueCounter != 0;

    }
}
