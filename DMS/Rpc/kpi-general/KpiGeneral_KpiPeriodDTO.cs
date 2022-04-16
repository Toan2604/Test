using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_KpiPeriodDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiGeneral_KpiPeriodDTO() { }
        public KpiGeneral_KpiPeriodDTO(KpiPeriod KpiPeriod)
        {

            this.Id = KpiPeriod.Id;

            this.Code = KpiPeriod.Code;

            this.Name = KpiPeriod.Name;

            this.Errors = KpiPeriod.Errors;
        }
    }

    public class KpiGeneral_KpiPeriodlFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiPeriodOrder OrderBy { get; set; }
    }
}