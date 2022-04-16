using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    [DisplayName("Bảng giá")]
    public class PriceListRoute : Root
    {
        public const string Parent = Module + "/price-list-and-promotion";
        public const string Master = Module + "/price-list-and-promotion/price-list/price-list-master";
        public const string Detail = Module + "/price-list-and-promotion/price-list/price-list-detail/*";
        public const string MasterOwner = Module + "/price-list-and-promotion/price-list-owner/price-list-owner-master";
        public const string DetailOwner = Module + "/price-list-and-promotion/price-list-owner/price-list-owner-detail/*";
        private const string Default = Rpc + Module + "/price-list";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";

        public const string CountNew = Default + "/count-new";
        public const string ListNew = Default + "/list-new";
        public const string CountPending = Default + "/count-pending";
        public const string ListPending = Default + "/list-pending";
        public const string CountCompleted = Default + "/count-completed";
        public const string ListCompleted = Default + "/list-completed";

        public const string GetDetail = Default + "/get-detail";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Send = Default + "/send";
        public const string Approve = Default + "/approve";
        public const string Reject = Default + "/reject";
        public const string Delete = Default + "/delete";
        public const string ImportItem = Default + "/import-item";
        public const string ExportItem = Default + "/export-item";
        public const string ExportTemplateItem = Default + "/export-template-item";
        public const string ImportStore = Default + "/import-store";
        public const string ExportStore = Default + "/export-store";
        public const string ExportTemplateStore = Default + "/export-template-store";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListItem = Default + "/filter-list-item";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListPriceListType = Default + "/filter-list-price-list-type";
        public const string FilterListSalesOrderType = Default + "/filter-list-sales-order-type";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListRequestState = Default + "/filter-list-request-state";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListItem = Default + "/single-list-item";
        public const string SingleListProvince = Default + "/single-list-province";
        public const string SingleListProductGrouping = Default + "/single-list-product-grouping";
        public const string SingleListProductType = Default + "/single-list-product-type";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListPriceListType = Default + "/single-list-price-list-type";
        public const string SingleListSalesOrderType = Default + "/single-list-sales-order-type";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListRequestState = Default + "/single-list-request-state";

        public const string CountItem = Default + "/count-item";
        public const string ListItem = Default + "/list-item";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public const string CountPriceListItemHistory = Default + "/count-price-list-item-history";
        public const string ListPriceListItemHistory = Default + "/list-price-list-item-history";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(PriceListFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(PriceListFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm tất cả", new List<string> {
                Parent, Master, Count, List,
                Get, GetDetail,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },
            { "Tìm kiếm của tôi", new List<string> {
                Parent, MasterOwner, Get, GetDetail, DetailOwner,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },

            { "Thêm", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                DetailOwner, Detail, Create, Send,
                SingleListItem, SingleListOrganization, SingleListPriceListType, SingleListSalesOrderType, SingleListStatus, SingleListProductGrouping, SingleListProductType, SingleListStoreGrouping, SingleListStore, SingleListStoreType, SingleListProvince, SingleListRequestState,
                Count, List, Count, List, Count, List, Count, List,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },

            { "Sửa", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                DetailOwner, Detail, Update, Send,
                SingleListItem, SingleListOrganization, SingleListPriceListType, SingleListSalesOrderType, SingleListStatus, SingleListProductGrouping, SingleListProductType, SingleListStoreGrouping, SingleListStore, SingleListStoreType, SingleListProvince, SingleListRequestState,
                Count, List, Count, List, Count, List, Count, List,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                }},

            { "Xoá", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                Delete,
                SingleListItem, SingleListOrganization, SingleListPriceListType, SingleListSalesOrderType, SingleListStatus, SingleListProductGrouping, SingleListProductType, SingleListStoreGrouping, SingleListStore, SingleListStoreType, SingleListProvince,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },

            { "Xoá nhiều", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                BulkDelete,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },

            { "Xuất excel", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                ExportItem, ExportStore,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },

            { "Nhập excel", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                ExportTemplateItem, ExportTemplateStore, ImportItem, ImportStore,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                } },
            { "Phê duyệt", new List<string> {
                Parent, Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListItem, FilterListOrganization, FilterListPriceListType, FilterListSalesOrderType, FilterListStatus, FilterListStoreGrouping, FilterListStore, FilterListStoreType, FilterListRequestState,
                DetailOwner, Detail, Approve, Reject, Send,
                SingleListItem, SingleListOrganization, SingleListPriceListType, SingleListSalesOrderType, SingleListStatus, SingleListProductGrouping, SingleListProductType, SingleListStoreGrouping, SingleListStore, SingleListStoreType, SingleListProvince, SingleListRequestState,
                Count, List, Count, List, Count, List, Count, List,
                CountItem, ListItem, CountStore, ListStore, CountPriceListItemHistory, ListPriceListItemHistory
                }},
        };
    }
}
