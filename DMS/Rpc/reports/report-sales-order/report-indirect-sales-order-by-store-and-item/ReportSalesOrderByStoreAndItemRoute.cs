using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    [DisplayName("Báo cáo đơn hàng gián tiếp theo đại lý và sản phẩm")]
    public class ReportSalesOrderByStoreAndItemRoute : Root
    {
        public const string Parent = Module + "/indirect-sales-order-report";
        public const string Master = Module + "/indirect-sales-order-report/indirect-sales-order-by-store-and-items-report-master";

        private const string Default = Rpc + Module + "/indirect-sales-order-by-store-and-items-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Total = Default + "/total";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListItem = Default + "/filter-list-item";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListProductGrouping = Default + "/filter-list-product-grouping";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListOrganization, FilterListStore, FilterListStoreType, FilterListStoreGrouping, FilterListStoreStatus, 
                FilterListItem, FilterListProductGrouping, FilterListProvince, FilterListDistrict } },

        };
    }
}
