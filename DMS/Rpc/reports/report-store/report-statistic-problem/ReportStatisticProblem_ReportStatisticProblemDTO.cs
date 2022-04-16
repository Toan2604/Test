using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_statistic_problem
{
    public class ReportStatisticProblem_ReportStatisticProblemDTO : DataDTO
    {
        internal long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportStatisticProblem_StoreDTO> Stores { get; set; }
    }

    public class ReportStatisticProblem_ReportStatisticProblemFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter Date { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (Date != null && Date.HasValue) ||
            (StoreId != null && StoreId.HasValue) ||
            (StoreTypeId != null && StoreTypeId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (StoreGroupingId != null && StoreGroupingId.HasValue);
    }
}
