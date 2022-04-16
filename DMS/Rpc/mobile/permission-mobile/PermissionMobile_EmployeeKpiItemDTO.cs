using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_EmployeeKpiItemReportDTO
    {
        internal long ItemId { get; set; }
        public string ItemName { get; set; } // tên Item được áp dụng KPI

        public List<PermissionMobile_EmployeeKpiItem> CurrentKpiItems { get; set; }
    }

    public class PermissionMobile_EmployeeKpiItem
    {
        public long ItemId { get; set; }
        public string KpiCriteriaItemName { get; set; }
        public decimal? PlannedValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public decimal? Percentage { get; set; } // giá trị thực hiện 

    }

    public class PermissionMobile_EmployeeKpiItemReportFilterDTO : FilterDTO
    {
        public IdFilter EmployeeId { get; set; }
    }
}
