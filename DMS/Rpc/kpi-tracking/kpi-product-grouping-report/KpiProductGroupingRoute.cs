using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_product_grouping_report
{
    [DisplayName("Báo cáo KPI nhóm sản phẩm")]
    public class KpiProductGroupingReportRoute : Root
    {
        public const string Parent = Module + "/kpi-tracking";
        public const string Master = Module + "/kpi-tracking/kpi-product-grouping-report/kpi-product-grouping-report-master";

        private const string Default = Rpc + Module + "/kpi-product-grouping-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string ListKpiProductGroupingCriteria = Default + "/list-kpi-product-grouping-criteria";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListKpiPeriod = Default + "/filter-list-kpi-period";
        public const string FilterListKpiYear = Default + "/filter-list-kpi-year";
        public const string FilterListKpiProductGroupingType = Default + "/filter-list-kpi-product-grouping-type";
        public const string FilterListProductGrouping = Default + "/filter-list-product-grouping";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(KpiProductGroupingReport_KpiProductGroupingReportFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                ListKpiProductGroupingCriteria,
                FilterListOrganization,FilterListAppUser,FilterListKpiYear, FilterListKpiPeriod, FilterListKpiProductGroupingType, FilterListProductGrouping} },

        };
    }
}
