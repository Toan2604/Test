using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_UnitOfMeasureGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public KpiProductGrouping_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public KpiProductGrouping_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public KpiProductGrouping_UnitOfMeasureGroupingContentDTO() { }
        public KpiProductGrouping_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            this.Id = UnitOfMeasureGroupingContent.Id;
            this.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            this.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            this.Factor = UnitOfMeasureGroupingContent.Factor;
            this.UnitOfMeasure = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? null : new KpiProductGrouping_UnitOfMeasureDTO(UnitOfMeasureGroupingContent.UnitOfMeasure);
            this.UnitOfMeasureGrouping = UnitOfMeasureGroupingContent.UnitOfMeasureGrouping == null ? null : new KpiProductGrouping_UnitOfMeasureGroupingDTO(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping);
            this.Errors = UnitOfMeasureGroupingContent.Errors;
        }
    }

    public class KpiProductGrouping_UnitOfMeasureGroupingContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter UnitOfMeasureGroupingId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public LongFilter Factor { get; set; }

        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
    }
}
