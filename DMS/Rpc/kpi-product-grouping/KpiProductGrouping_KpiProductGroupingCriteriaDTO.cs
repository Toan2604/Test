using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingCriteriaDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public KpiProductGrouping_KpiProductGroupingCriteriaDTO() { }

        public KpiProductGrouping_KpiProductGroupingCriteriaDTO(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            this.Id = KpiProductGroupingCriteria.Id;
            this.Code = KpiProductGroupingCriteria.Code;
            this.Name = KpiProductGroupingCriteria.Name;
            this.Errors = KpiProductGroupingCriteria.Errors;
        }
    }
}
