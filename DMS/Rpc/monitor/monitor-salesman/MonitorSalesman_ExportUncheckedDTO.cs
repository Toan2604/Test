using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_salesman
{
    public class MonitorSalesman_ExportEmployeeUncheckedDTO : DataDTO
    {
        public long AppUserId { get; set; }
        public string DisplayName { get; set; }
        public List<MonitorSalesman_ExportUncheckedDTO> Contents { get; set; }
    }
    public class MonitorSalesman_ExportUncheckedDTO : DataDTO
    {
        public long STT { get; set; }
        public long AppUserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string StoreCode { get; set; }
        public string StoreCodeDraft { get; set; }
        public string StoreName { get; set; }
        public string StorePhone { get; set; }
        public string StoreAddress { get; set; }
        public string StoreTypeName { get; set; }
        public string StoreStatusName { get; set; }
    }
}
