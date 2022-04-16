using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_statistic_problem
{
    [DisplayName("Báo cáo thống kê vấn đề")]
    public class ReportStatisticProblemRoute : Root
    {
        public const string Parent = Module + "/store-report";
        public const string Master = Module + "/store-report/statistic-problem-report-master";

        private const string Default = Rpc + Module + "/statistic-problem-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Total = Default + "/total";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(ReportStatisticProblem_ReportStatisticProblemFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListOrganization, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListStoreStatus } },

        };
    }
}
