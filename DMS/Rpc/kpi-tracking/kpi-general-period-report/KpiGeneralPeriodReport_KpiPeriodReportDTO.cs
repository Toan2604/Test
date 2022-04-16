using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<KpiGeneralPeriodReport_SaleEmployeeDTO> SaleEmployees { get; set; }
    }

    public class KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public DateFilter OrderDate { get; set; }
    }
}
