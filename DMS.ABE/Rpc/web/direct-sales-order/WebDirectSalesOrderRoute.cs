using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;
using System.ComponentModel;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    [DisplayName("Đơn hàng bán trực tiếp trên web")]
    public class WebDirectSalesOrderRoute : Root
    {
        public const string Parent = WebModule + "/direct-sales-order";
        public const string Master = WebModule + "/direct-sales-order/direct-sales-order-master";
        public const string Preview = WebModule + "/direct-sales-order/direct-sales-order-preview";
        public const string Mobile = Module + ".direct-sales-order.*";

        public const string DMSMaster = DMSModule + "/sale-order/direct-sales-order/direct-sales-order-master";
        public const string DMSMobile = DMSModule + ".direct-sales-order.*";

        private const string Default = Rpc + Module + "/web/direct-sales-order";
        public const string List = Default + "/list";
        public const string Count = Default + "/count";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Send = Default + "/send"; // chỉ áp dụng cho trạng thái đơn = nháp hoặc đang tạo
        public const string Approve = Default + "/approve"; //trạng thái đơn = chờ đại lý phê duyệt
        public const string Reject = Default + "/reject";//trạng thái đơn = chờ đại lý phê duyệt
        public const string Get = Default + "/get";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string ListItem = Default + "/list-item";
        public const string CountItem = Default + "/count-item";
        public const string FilterListStoreApprovalState = Default + "/filter-list-store-approval-state";
        public const string FilterListGeneralApprovalState = Default + "/filter-list-general-approval-state";
        public const string FilterListCategory = Default + "/filter-list-category";
        public const string SingleListUnitOfMeasure = Default + "/single-list-unit-of-measure";
        public const string PrintDirectOrder = Default + "/print-direct-order";
        public const string CalculatePrice = Default + "/calculate-price";
        public const string Export = Default + "/export";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
            } },
        };
    }
}
