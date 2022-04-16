using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_ExportTemplateDTO : DataDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string KpiProductGroupingTypeName { get; set; }
        public List<KpiProductGrouping_ExportTemplateContentDTO> Contents { get; set; }
    }

    public class KpiProductGrouping_ExportTemplateContentDTO : DataDTO
    {
        public string ProductGroupingCode { get; set; }
        public string ProductGroupingName { get; set; }
        public long? IndirectQuantity { get; set; }
        public decimal? IndirectRevenue { get; set; }
        public long? IndirectCounter { get; set; }
        public long? IndirectStoreCounter { get; set; }
        public long? DirectQuantity { get; set; }
        public decimal? DirectRevenue { get; set; }
        public long? DirectCounter { get; set; }
        public long? DirectStoreCounter { get; set; }
    }
}
