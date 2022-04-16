using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    [DisplayName("Báo cáo tổng hợp bán hàng đơn trực tiếp")]
    public class ReportDirectSalesOrderGeneralRoute : Root
    {
        public const string Parent = Module + "/direct-sales-order-report";
        public const string Master = Module + "/direct-sales-order-report/direct-sales-order-general-report-master";

        private const string Default = Rpc + Module + "/direct-sales-order-general-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Total = Default + "/total";
        public const string Export = Default + "/export";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListDistrict = Default + "/filter-list-district";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralFilterDTO.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListAppUser, FilterListOrganization, FilterListStore, FilterListStoreStatus,
                FilterListProvince, FilterListDistrict} },

        };
    }
}
