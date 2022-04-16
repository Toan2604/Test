using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long KpiProductGroupingId { get; set; }
        public long ProductGroupingId { get; set; }
        public bool SelectAllCurrentItem { get; set; }
        public Guid RowId { get; set; }
        public KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping { get; set; }
        public KpiProductGrouping_ProductGroupingDTO ProductGrouping { get; set; }

        public Dictionary<long, long?> KpiProductGroupingContentCriteriaMappings { get; set; }
        public List<KpiProductGrouping_KpiProductGroupingContentItemMappingDTO> KpiProductGroupingContentItemMappings { get; set; }

        public KpiProductGrouping_KpiProductGroupingContentDTO() { }
        public KpiProductGrouping_KpiProductGroupingContentDTO(KpiProductGroupingContent KpiProductGroupingContent)
        {
            this.Id = KpiProductGroupingContent.Id;
            this.KpiProductGroupingId = KpiProductGroupingContent.KpiProductGroupingId;
            this.ProductGroupingId = KpiProductGroupingContent.ProductGroupingId;
            this.SelectAllCurrentItem = KpiProductGroupingContent.SelectAllCurrentItem;
            this.RowId = KpiProductGroupingContent.RowId;
            this.KpiProductGrouping = KpiProductGroupingContent.KpiProductGrouping == null ? null : new KpiProductGrouping_KpiProductGroupingDTO(KpiProductGroupingContent.KpiProductGrouping);
            this.ProductGrouping = KpiProductGroupingContent.ProductGrouping == null ? null : new KpiProductGrouping_ProductGroupingDTO(KpiProductGroupingContent.ProductGrouping);
            this.KpiProductGroupingContentCriteriaMappings = KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings?
                .ToDictionary(x => x.KpiProductGroupingCriteriaId, y => y.Value); // tra ve cho FE dictionary voi key la Id cua chi tieu, value la gia tri cua chi tieu
            this.KpiProductGroupingContentItemMappings = KpiProductGroupingContent.KpiProductGroupingContentItemMappings?
                .Select(x => new KpiProductGrouping_KpiProductGroupingContentItemMappingDTO(x))
                .ToList(); // tra ve cho FE List mapping cua content voi item
            this.Errors = KpiProductGroupingContent.Errors;
        }
    }

    public class KpiProductGroupingContent_KpiProductGroupingContentFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter KpiProductGroupingId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public GuidFilter RowId { get; set; }
        public KpiProductGroupingContentOrder OrderBy { get; set; }
    }
}
