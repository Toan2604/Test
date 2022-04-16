using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_RevenueGrowthDTO : DataDTO
    {
        public List<PermissionMobile_RevenueGrowthByMonthDTO> IndirectRevenueGrowthByMonths { get; set; }
        public List<PermissionMobile_RevenueGrowthByQuarterDTO> IndirectRevenueGrowthByQuaters { get; set; }
        public List<PermissionMobile_RevenueGrowthByYearDTO> IndirectRevenueGrowthByYears { get; set; }
        public List<PermissionMobile_RevenueGrowthByMonthDTO> DirectRevenueGrowthByMonths { get; set; }
        public List<PermissionMobile_RevenueGrowthByQuarterDTO> DirectRevenueGrowthByQuaters { get; set; }
        public List<PermissionMobile_RevenueGrowthByYearDTO> DirectRevenueGrowthByYears { get; set; }
    }

    public class PermissionMobile_RevenueGrowthByMonthDTO : DataDTO
    {
        public long Day { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PermissionMobile_RevenueGrowthByQuarterDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PermissionMobile_RevenueGrowthByYearDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PermissionMobile_RevenueGrowthFilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter EmployeeId { get; set; }
    }
}
