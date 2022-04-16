using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReport_KpiItemReportDTO : DataDTO
    {
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<KpiItemReport_KpiItemContentDTO> ItemContents { get; set; }
    }


    public class KpiItemReport_KpiItemReportFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiItemTypeId { get; set; }
        public IdFilter ItemId { get; set; }
        public DateFilter OrderDate { get; set; }
    }
}
