using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_checked
{
    public class ReportStoreChecked_SaleEmployeeDTO : DataDTO
    {
        public long STT { get; set; }
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public long OrganizationId { get; set; }
        public List<ReportStoreChecked_StoreCheckingGroupByDateDTO> StoreCheckingGroupByDates { get; set; }
    }
}
