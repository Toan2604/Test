using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    [DisplayName("Báo cáo tổng hợp bán hàng đơn gián tiếp")]
    public class ReportSalesOrderGeneralRoute : Root
    {
        public const string Parent = Module + "/indirect-sales-order-report";
        public const string Master = Module + "/indirect-sales-order-report/indirect-sales-order-general-report-master";

        private const string Default = Rpc + Module + "/indirect-sales-order-general-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Total = Default + "/total";
        public const string Export = Default + "/export";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderGeneral_ReportSalesOrderGeneralFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListAppUser, FilterListOrganization, FilterListStore, FilterListStoreStatus, FilterListStoreType,
                FilterListProvince, FilterListDistrict} },

        };
    }
}
