using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_TopRevenueBySalesEmployeeDTO : DataDTO
    {
        public long SaleEmployeeId { get; set; }
        public string SaleEmployeeName { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PermissionMobile_FilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
        public DateFilter OrderDate { get; set; }
        public IdFilter EmployeeId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter ProvinceId { get; set; }
    }
}
