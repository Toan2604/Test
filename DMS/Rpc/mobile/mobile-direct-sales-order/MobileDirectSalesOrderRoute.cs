using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    [DisplayName("Đơn hàng trực tiếp trên mobile")]
    public class MobileDirectSalesOrderRoute : Root
    {
        public const string Master = Module + "/mobile/direct-sales-order-master";
        public const string Detail = Module + "/mobile/direct-sales-order-detail";
        private const string Default = Rpc + Module + "/mobile/direct-sales-order";

        public const string Get = Default + "/get";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string CountInScoped = Default + "/count-in-scoped";
        public const string ListInScoped = Default + "/list-in-scoped";
        public const string CountPending = Default + "/count-pending";
        public const string ListPending = Default + "/list-pending";

        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Send = Default + "/send";
        public const string Approve = Default + "/approve";
        public const string Reject = Default + "/reject";

        public const string SingleListBuyerStore = Default + "/single-list-buyer-store";

        public const string FilterListErpApprovalState = Default + "/filter-list-erp-approval-state";
        public const string FilterListGeneralApprovalState = Default + "/filter-list-general-approval-state";
        public const string FilterListSaleEmployee = Default + "/filter-list-sale-employee";

        public const string Print = Default + "/print";
        public const string CalculatePrice = Default + "/calculate-price";




        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(DirectSalesOrderFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(DirectSalesOrderFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {

            { "Thêm", new List<string> {
                Master, Detail,
                Get, Create, Send, Print, CalculatePrice,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee, SingleListBuyerStore,
            } },
            { "Sửa", new List<string> {
                Master, Detail,
                Get, Update, Send, Print, CalculatePrice,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee, SingleListBuyerStore,
            } },
            { "Xoá", new List<string> {
                Master, Detail,
                Get, Delete, Print, CalculatePrice,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee, SingleListBuyerStore,
            } },
            { "Phê duyệt", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print, CalculatePrice,
                CountPending, ListPending,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee, SingleListBuyerStore,
            } },
            { "Tìm kiếm tất cả", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print,
                Count, List, CountInScoped, ListInScoped,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee
            } },
            { "Tìm kiếm của tôi", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print,
                CountInScoped, ListInScoped,
                FilterListErpApprovalState, FilterListGeneralApprovalState, FilterListSaleEmployee
            } },
        };
    }
}
