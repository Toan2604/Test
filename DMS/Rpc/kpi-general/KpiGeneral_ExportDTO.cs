using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_ExportDTO : DataDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<KpiGeneral_ExportCriterialDTO> CriteriaContents { get; set; }

    }

    public class KpiGeneral_ExportCriterialDTO : DataDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CriteriaName { get; set; }

        public decimal? M1Value { get; set; }
        public decimal? M2Value { get; set; }
        public decimal? M3Value { get; set; }
        public decimal? M4Value { get; set; }
        public decimal? M5Value { get; set; }
        public decimal? M6Value { get; set; }
        public decimal? M7Value { get; set; }
        public decimal? M8Value { get; set; }
        public decimal? M9Value { get; set; }
        public decimal? M10Value { get; set; }
        public decimal? M11Value { get; set; }
        public decimal? M12Value { get; set; }
               
        public decimal? Q1Value { get; set; }
        public decimal? Q2Value { get; set; }
        public decimal? Q3Value { get; set; }
        public decimal? Q4Value { get; set; }
               
        public decimal? YValue { get; set; }
    }

}
