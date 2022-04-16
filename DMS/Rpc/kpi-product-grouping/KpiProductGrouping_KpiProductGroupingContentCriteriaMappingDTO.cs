using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingContentCriteriaMappingDTO : DataDTO
    {
        public long SaleEmployeeId { get; set; }
        public long ProductGroupingId { get; set; }
        public long KpiProductGroupingContentId { get; set; }
        public long KpiProductGroupingCriteriaId { get; set; }
        public long? Value { get; set; }
    }
}
