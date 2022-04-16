using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    public class KpiProductGroupingReport_KpiYearDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public KpiProductGroupingReport_KpiYearDTO() { }
        public KpiProductGroupingReport_KpiYearDTO(KpiYear KpiYear)
        {

            this.Id = KpiYear.Id;

            this.Code = KpiYear.Code;

            this.Name = KpiYear.Name;

            this.Errors = KpiYear.Errors;
        }
    }

    public class KpiProductGroupingReport_KpiYearFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiYearOrder OrderBy { get; set; }
    }
}