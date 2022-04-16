using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    [DisplayName("Báo cáo KPI sản phẩm")]
    public class KpiItemReportRoute : Root
    {
        public const string Parent = Module + "/kpi-tracking";
        public const string Master = Module + "/kpi-tracking/kpi-item-report/kpi-item-report-master";

        private const string Default = Rpc + Module + "/kpi-item-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string ListKpiCriteriaItem = Default + "/list-kpi-criteria-item";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListItem = Default + "/filter-list-item";
        public const string FilterListKpiPeriod = Default + "/filter-list-kpi-period";
        public const string FilterListKpiYear = Default + "/filter-list-kpi-year";
        public const string FilterListKpiItemType = Default + "/filter-list-kpi-item-type";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(KpiItemReport_KpiItemReportFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(KpiItemReport_KpiItemReportFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                ListKpiCriteriaItem,
                FilterListOrganization,FilterListAppUser,FilterListKpiYear, FilterListKpiPeriod, FilterListItem, FilterListKpiItemType} },

        };
    }
}
