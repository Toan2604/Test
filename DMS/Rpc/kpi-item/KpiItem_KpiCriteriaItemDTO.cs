using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_KpiCriteriaItemDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiItem_KpiCriteriaItemDTO() { }
        public KpiItem_KpiCriteriaItemDTO(KpiCriteriaItem KpiCriteriaItem)
        {

            this.Id = KpiCriteriaItem.Id;

            this.Code = KpiCriteriaItem.Code;

            this.Name = KpiCriteriaItem.Name;

            this.Errors = KpiCriteriaItem.Errors;
        }
    }
}
