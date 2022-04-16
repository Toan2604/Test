using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReport_ExportDTO : DataDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<KpiItemReport_ItemExportDTO> Items { get; set; }

        public KpiItemReport_ExportDTO() { }
    }

    public class KpiItemReport_ItemExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public List<KpiItemReport_CriteriaContentDTO> CriteriaContents { get; set; }

        public KpiItemReport_ItemExportDTO() { }
    }

    public class KpiItemReport_CriteriaContentDTO : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Result { get; set; }
        public decimal? Ratio { get; set; }
    }

}
