using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_employee_and_item
{
    [DisplayName("Báo cáo đơn hàng trực tiếp theo nhân viên và sản phẩm")]
    public class ReportDirectSalesOrderByEmployeeAndItemRoute : Root
    {
        public const string Parent = Module + "/direct-sales-order-report";
        public const string Master = Module + "/direct-sales-order-report/direct-sales-order-by-employee-and-items-report-master";

        private const string Default = Rpc + Module + "/direct-sales-order-by-employee-and-items-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Total = Default + "/total";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListItem = Default + "/filter-list-item";
        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListProductGrouping = Default + "/filter-list-product-grouping";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Total, Export,
                FilterListOrganization, FilterListItem, FilterListAppUser, FilterListProductGrouping } },

        };
    }
}
