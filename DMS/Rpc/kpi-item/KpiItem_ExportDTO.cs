using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_ExportDTO : DataDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string KpiItemTypeName { get; set; }
        public List<KpiItem_ExportContentDTO> Contents { get; set; }
    }

    public class KpiItem_ExportContentDTO : DataDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string KpiItemTypeName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public List<KpiItem_ExportCriteriaContent> CriteriaContents { get; set; }
    }

    public class KpiItem_ExportCriteriaContent : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public long? Value { get; set; }
    }
}
