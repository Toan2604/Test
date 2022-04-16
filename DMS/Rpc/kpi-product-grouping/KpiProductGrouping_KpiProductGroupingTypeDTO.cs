using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiProductGrouping_KpiProductGroupingTypeDTO() { }
        public KpiProductGrouping_KpiProductGroupingTypeDTO(KpiProductGroupingType KpiProductGroupingType)
        {

            this.Id = KpiProductGroupingType.Id;

            this.Code = KpiProductGroupingType.Code;

            this.Name = KpiProductGroupingType.Name;

            this.Errors = KpiProductGroupingType.Errors;
        }
    }

    public class KpiProductGrouping_KpiProductGroupingTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiProductGroupingTypeOrder OrderBy { get; set; }
    }
}