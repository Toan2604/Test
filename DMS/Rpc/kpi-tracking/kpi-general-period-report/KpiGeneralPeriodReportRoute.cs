using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    [DisplayName("Báo cáo KPI theo kỳ")]
    public class KpiGeneralPeriodReportRoute : Root
    {
        public const string Parent = Module + "/kpi-tracking";
        public const string Master = Module + "/kpi-tracking/kpi-general-period-report/kpi-general-period-report-master";

        private const string Default = Rpc + Module + "/kpi-general-period-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string ListKpiCriteriaGeneral = Default + "/list-kpi-criteria-general";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListKpiPeriod = Default + "/filter-list-kpi-period";
        public const string FilterListKpiYear = Default + "/filter-list-kpi-year";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                ListKpiCriteriaGeneral,
                FilterListOrganization,FilterListAppUser,FilterListKpiYear, FilterListKpiPeriod} },

        };
    }
}
