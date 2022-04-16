using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    [DisplayName("Đơn hàng gián tiếp trên mobile")]
    public class MobileIndirectSalesOrderRoute : Root
    {
        public const string Master = Module + "/mobile/indirect-sales-order-master";
        public const string Detail = Module + "/mobile/indirect-sales-order-detail";
        private const string Default = Rpc + Module + "/mobile/indirect-sales-order";

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
        public const string SingleListSellerStore = Default + "/single-list-seller-store";

        public const string FilterListRequestState = Default + "/filter-list-request-state";
        public const string FilterListSaleEmployee = Default + "/filter-list-sale-employee";

        public const string Print = Default + "/print";


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
                Get, Create, Send, Print, 
                FilterListRequestState, FilterListSaleEmployee, SingleListBuyerStore, SingleListSellerStore
            } },
            { "Sửa", new List<string> {
                Master, Detail,
                Get, Update, Send, Print, 
                FilterListRequestState, FilterListSaleEmployee, SingleListBuyerStore, SingleListSellerStore
            } },
            { "Xoá", new List<string> {
                Master, Detail,
                Get, Delete, Print, 
                FilterListRequestState, FilterListSaleEmployee, SingleListBuyerStore, SingleListSellerStore
            } },
            { "Phê duyệt", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print, 
                CountPending, ListPending,
                FilterListRequestState, FilterListSaleEmployee, SingleListBuyerStore, SingleListSellerStore
            } },
            { "Tìm kiếm tất cả", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print,
                Count, List, ListInScoped, CountInScoped,
                FilterListRequestState, FilterListSaleEmployee
            } },
            { "Tìm kiếm của tôi", new List<string> {
                Master, Detail,
                Get, Approve, Reject, Print,
                ListInScoped, CountInScoped,
                FilterListRequestState, FilterListSaleEmployee
            } },
        };
    }
}
