using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportSalesOrderGeneral_IndirectSalesOrderDTO> SalesOrders { get; set; }
    }

    public class ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public IdFilter SellerStoreId { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public DateFilter OrderDate { get; set; }
        public bool? CreatedInCheckin { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (AppUserId != null && AppUserId.HasValue) ||
            (BuyerStoreId != null && BuyerStoreId.HasValue) ||
            (SellerStoreId != null && SellerStoreId.HasValue) ||
            (StoreStatusId != null && StoreStatusId.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue) ||
            (OrderDate != null && OrderDate.HasValue);
    }
}
