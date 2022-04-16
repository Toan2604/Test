using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item
{
    [DisplayName("Báo cáo đơn hàng trực tiếp theo đại lý và sản phẩm")]
    public class ReportDirectSalesOrderByStoreAndItemRoute : Root
    {
        public const string Parent = Module + "/direct-sales-order-report";
        public const string Master = Module + "/direct-sales-order-report/direct-sales-order-by-store-and-items-report-master";

        private const string Default = Rpc + Module + "/direct-sales-order-by-store-and-items-report";
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
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListOrganization, FilterListItem, FilterListStore, FilterListStoreType, FilterListStoreGrouping, 
                FilterListStoreStatus, FilterListProductGrouping, FilterListProvince, FilterListDistrict } },

        };
    }
}
