using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_employee_report
{
    public class KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO() { }
        public KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO(KpiCriteriaGeneral KpiCriteriaGeneral)
        {

            this.Id = KpiCriteriaGeneral.Id;

            this.Code = KpiCriteriaGeneral.Code;

            this.Name = KpiCriteriaGeneral.Name;

            this.Errors = KpiCriteriaGeneral.Errors;
        }
    }

    public class KpiGeneralEmployeeReport_KpiCriteriaGeneralFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiCriteriaGeneralOrder OrderBy { get; set; }
    }

}
