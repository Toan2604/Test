using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_employee_report
{
    public class KpiGeneralEmployeeReport_ExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string KpiPeriod { get; set; }
        public string KpiYear { get; set; }
        public List<KpiGeneralEmployeeReport_CriteriaContentDTO> CriteriaContents { get; set; }

        public KpiGeneralEmployeeReport_ExportDTO() { }
    }

    public class KpiGeneralEmployeeReport_CriteriaContentDTO : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Result { get; set; }
        public decimal? Ratio { get; set; }
    }
}
