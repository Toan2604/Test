using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReport_KpiCriteriaItemDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiItemReport_KpiCriteriaItemDTO() { }
        public KpiItemReport_KpiCriteriaItemDTO(KpiCriteriaItem KpiCriteriaItem)
        {

            this.Id = KpiCriteriaItem.Id;

            this.Code = KpiCriteriaItem.Code;

            this.Name = KpiCriteriaItem.Name;

            this.Errors = KpiCriteriaItem.Errors;
        }
    }

    public class KpiItemReport_KpiCriteriaItemFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiCriteriaItemOrder OrderBy { get; set; }
    }
}
