using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    public class KpiProductGroupingReport_ItemDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public KpiProductGroupingReport_ItemDTO(Item item)
        {
            this.Id = item.Id;
            this.Code = item.Code;
            this.Name = item.Name;
        }
    }

    public class KpiProductGroupingReport_ItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public string Search { get; set; }
    }
}
