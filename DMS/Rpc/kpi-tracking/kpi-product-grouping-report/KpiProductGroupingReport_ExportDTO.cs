using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    public class KpiProductGroupingReport_ExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<KpiProductGroupingReport_KpiSaleEmployeeReportDTO> Employees { get; set; }

        public KpiProductGroupingReport_ExportDTO() { }
    }

    public class KpiProductGroupingReport_KpiSaleEmployeeReportDTO : DataDTO
    {
        public long STT { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ProductGroupingCode { get; set; }
        public string ProductGroupingName { get; set; }
        public List<KpiProductGroupingReport_KpiProductGroupingContentExportDTO> CriteriaContents { get; set; }

        public KpiProductGroupingReport_KpiSaleEmployeeReportDTO() { }
       
    }

    public class KpiProductGroupingReport_KpiProductGroupingContentExportDTO : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Result { get; set; }
        public decimal? Ratio { get; set; }
    }
}
