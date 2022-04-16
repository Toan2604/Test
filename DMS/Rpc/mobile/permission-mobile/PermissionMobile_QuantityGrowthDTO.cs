using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_QuantityGrowthDTO : DataDTO
    {
        public List<PermissionMobile_QuantityGrowthByMonthDTO> IndirectSalesOrderQuantityGrowthByMonths { get; set; }
        public List<PermissionMobile_QuantityGrowthByQuarterDTO> IndirectSalesOrderQuantityGrowthByQuaters { get; set; }
        public List<PermissionMobile_QuantityGrowthByYearDTO> IndirectSalesOrderQuantityGrowthByYears { get; set; }

        public List<PermissionMobile_QuantityGrowthByMonthDTO> DirectSalesOrderQuantityGrowthByMonths { get; set; }
        public List<PermissionMobile_QuantityGrowthByQuarterDTO> DirectSalesOrderQuantityGrowthByQuaters { get; set; }
        public List<PermissionMobile_QuantityGrowthByYearDTO> DirectSalesOrderQuantityGrowthByYears { get; set; }
    }

    public class PermissionMobile_QuantityGrowthByMonthDTO : DataDTO
    {
        public long Day { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }
    }

    public class PermissionMobile_QuantityGrowthByQuarterDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }

    }

    public class PermissionMobile_QuantityGrowthByYearDTO : DataDTO
    {
        public long Month { get; set; }
        public decimal IndirectSalesOrderCounter { get; set; }
        public decimal DirectSalesOrderCounter { get; set; }
    }

    public class PermissionMobile_QuantityGrowthFilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter EmployeeId { get; set; }
    }
}
