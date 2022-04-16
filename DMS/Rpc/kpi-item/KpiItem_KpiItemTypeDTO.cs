using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_KpiItemTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiItem_KpiItemTypeDTO() { }
        public KpiItem_KpiItemTypeDTO(KpiItemType KpiItemType)
        {

            this.Id = KpiItemType.Id;

            this.Code = KpiItemType.Code;

            this.Name = KpiItemType.Name;

            this.Errors = KpiItemType.Errors;
        }
    }

    public class KpiItem_KpiItemTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiItemTypeOrder OrderBy { get; set; }
    }
}