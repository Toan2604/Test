using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_KpiCriteriaGeneralDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public List<long> EmployeeIds { get; set; }


        public KpiGeneral_KpiCriteriaGeneralDTO() { }
        public KpiGeneral_KpiCriteriaGeneralDTO(KpiCriteriaGeneral KpiCriteriaGeneral)
        {

            this.Id = KpiCriteriaGeneral.Id;

            this.Code = KpiCriteriaGeneral.Code;

            this.Name = KpiCriteriaGeneral.Name;

            this.Errors = KpiCriteriaGeneral.Errors;
        }
    }

    public class KpiGeneral_KpiCriteriaGeneralFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiCriteriaGeneralOrder OrderBy { get; set; }
    }
}