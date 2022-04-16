using DMS.Entities;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingContentItemMappingDTO
    {
        public long KpiProductGroupingContentId { get; set; }
        public long ItemId { get; set; }
        public KpiProductGrouping_KpiProductGroupingContentDTO KpiProductGroupingContent { get; set; }
        public KpiProductGrouping_ItemDTO Item { get; set; }

        public KpiProductGrouping_KpiProductGroupingContentItemMappingDTO() { }
        public KpiProductGrouping_KpiProductGroupingContentItemMappingDTO(KpiProductGroupingContentItemMapping KpiProductGroupingContentItemMapping)
        {
            this.KpiProductGroupingContentId = KpiProductGroupingContentItemMapping.KpiProductGroupingContentId;
            this.ItemId = KpiProductGroupingContentItemMapping.ItemId;
            this.KpiProductGroupingContent = KpiProductGroupingContentItemMapping.KpiProductGroupingContent == null ? null : new KpiProductGrouping_KpiProductGroupingContentDTO(KpiProductGroupingContentItemMapping.KpiProductGroupingContent);
            this.Item = KpiProductGroupingContentItemMapping.Item == null ? null : new KpiProductGrouping_ItemDTO(KpiProductGroupingContentItemMapping.Item);
        }
    }
}
