using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    public class KpiProductGroupingReport_KpiProductGroupingCriteriaDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiProductGroupingReport_KpiProductGroupingCriteriaDTO() { }
        public KpiProductGroupingReport_KpiProductGroupingCriteriaDTO(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {

            this.Id = KpiProductGroupingCriteria.Id;

            this.Code = KpiProductGroupingCriteria.Code;

            this.Name = KpiProductGroupingCriteria.Name;

            this.Errors = KpiProductGroupingCriteria.Errors;
        }
    }

    public class KpiProductGroupingReport_KpiProductGroupingCriteriaFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiProductGroupingCriteriaOrder OrderBy { get; set; }
    }
}
