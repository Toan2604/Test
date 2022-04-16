using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_KpiYearDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiGeneral_KpiYearDTO() { }
        public KpiGeneral_KpiYearDTO(KpiYear KpiYear)
        {

            this.Id = KpiYear.Id;

            this.Code = KpiYear.Code;

            this.Name = KpiYear.Name;

            this.Errors = KpiYear.Errors;
        }
    }

    public class KpiGeneral_KpiYearFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiYearOrder OrderBy { get; set; }
    }
}