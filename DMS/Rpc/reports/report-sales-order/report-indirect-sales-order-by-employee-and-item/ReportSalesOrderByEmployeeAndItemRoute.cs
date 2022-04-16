using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_employee_and_item
{
    [DisplayName("Báo cáo đơn hàng gián tiếp theo nhân viên và sản phẩm")]
    public class ReportSalesOrderByEmployeeAndItemRoute : Root
    {
        public const string Parent = Module + "/indirect-sales-order-report";
        public const string Master = Module + "/indirect-sales-order-report/indirect-sales-order-by-employee-and-items-report-master";

        private const string Default = Rpc + Module + "/indirect-sales-order-by-employee-and-items-report";
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
            { nameof(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
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
