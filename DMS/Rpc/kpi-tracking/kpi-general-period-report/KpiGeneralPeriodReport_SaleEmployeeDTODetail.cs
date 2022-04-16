using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReport_SaleEmployeeDetailDTO : DataDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string OrganizationName { get; set; }
        public long SaleEmployeeId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public decimal? Value { get; set; }


        //public List<MonitorStoreChecker_StoreCheckingDTO> StoreCheckings { get; set; }
    }
}
