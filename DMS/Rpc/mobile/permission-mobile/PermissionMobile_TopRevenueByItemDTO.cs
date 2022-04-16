using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_TopRevenueByItemDTO : DataDTO
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PermissionMobile_TopRevenueByItemFilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter EmployeeId { get; set; }
    }
}
