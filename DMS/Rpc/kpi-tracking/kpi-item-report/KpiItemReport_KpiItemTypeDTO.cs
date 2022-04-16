using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReport_KpiItemTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiItemReport_KpiItemTypeDTO() { }
        public KpiItemReport_KpiItemTypeDTO(KpiItemType KpiItemType)
        {

            this.Id = KpiItemType.Id;

            this.Code = KpiItemType.Code;

            this.Name = KpiItemType.Name;

            this.Errors = KpiItemType.Errors;
        }
    }

    public class KpiItemReport_KpiItemTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiItemTypeOrder OrderBy { get; set; }
    }
}