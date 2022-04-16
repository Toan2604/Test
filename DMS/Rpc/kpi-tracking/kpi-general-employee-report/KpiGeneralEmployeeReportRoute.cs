using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_employee_report
{
    [DisplayName("Báo cáo KPI theo nhân viên")]
    public class KpiGeneralEmployeeReportRoute : Root
    {
        public const string Parent = Module + "/kpi-tracking";
        public const string Master = Module + "/kpi-tracking/kpi-general-employee-report/kpi-general-employee-report-master";

        private const string Default = Rpc + Module + "/kpi-general-employee-report";
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
            { nameof(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Export,
                FilterListOrganization,FilterListAppUser,FilterListKpiYear, FilterListKpiPeriod,
                ListKpiCriteriaGeneral} },

        };
    }
}
