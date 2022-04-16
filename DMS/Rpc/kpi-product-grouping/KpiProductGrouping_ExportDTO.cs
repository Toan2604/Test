using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_ExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<KpiProductGrouping_KpiProductGroupingExportDTO> Kpis { get; set; }
    }

    public class KpiProductGrouping_KpiProductGroupingExportDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string KpiProductGroupingTypeName { get; set; }
        public List<KpiProductGrouping_ProductGroupingExportDTO> ProductGroupings { get; set; }
    }

    public class KpiProductGrouping_ProductGroupingExportDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public long ItemCount { get; set; }
        public List<KpiProductGrouping_ExportItemDTO> Item_Lines { get; set; }
    }

    public class KpiProductGrouping_ExportItemDTO
    {
        public long STT { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string KpiProductGroupingTypeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public long? ItemCount { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public List<KpiProductGrouping_ExportCriteriaContent> CriteriaContents { get; set; }
    }

    public class KpiProductGrouping_ExportCriteriaContent : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public long? Value { get; set; }
    }
}

