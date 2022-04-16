using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    [DisplayName("Tuyến")]
    public class ERouteRoute : Root
    {
        public const string Parent = Module + "/route";
        public const string Master = Module + "/route/e-route/e-route-master";
        public const string Detail = Module + "/route/e-route/e-route-detail/*";
        public const string MasterOwner = Module + "/route/e-route-owner/e-route-owner-master";
        public const string DetailOwner = Module + "/route/e-route-owner/e-route-owner-detail/*";
        private const string Default = Rpc + Module + "/e-route";
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
        public const string BulkDelete = Default + "/bulk-delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListERouteType = Default + "/filter-list-eroute-type";
        public const string FilterListRequestState = Default + "/filter-list-request-state";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListStoreStatus = Default + "/filter-list-store-status";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListERouteType = Default + "/single-list-eroute-type";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListRequestState = Default + "/single-list-request-state";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListStatus = Default + "/single-list-status";

        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ERouteFilter.StoreId), FieldTypeEnum.ID.Id },
            { nameof(ERouteFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ERouteFilter.ERouteTypeId), FieldTypeEnum.ID.Id },
            { nameof(ERouteFilter.RequestStateId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreTypeId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };


        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm tất cả", new List<string> {
                Parent,
                Master, Count, List, Get, GetDetail,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore, FilterListStoreStatus } },
            { "Tìm kiếm của tôi", new List<string> {
                Parent,
                Get, MasterOwner, DetailOwner, GetDetail,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore, } },

            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                DetailOwner, Detail, Create, Send,
                SingleListAppUser, SingleListERouteType, SingleListRequestState, SingleListStatus,  SingleListStore, SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },

            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                DetailOwner, Detail, Update, Send,
                SingleListAppUser, SingleListERouteType, SingleListRequestState, SingleListStatus,  SingleListStore,  SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },

            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                Detail, Delete,  } },

            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                BulkDelete } },
            { "Phê duyệt", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                DetailOwner, Detail, Send, Approve, Reject,
                SingleListAppUser, SingleListERouteType, SingleListRequestState, SingleListStatus,  SingleListStore,  SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },
             { "Nhập Excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                Import, ExportTemplate,
                SingleListAppUser, SingleListERouteType, SingleListRequestState, SingleListStatus,  SingleListStore,  SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },
             { "Xuất Excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                CountNew, ListNew, CountPending, ListPending, CountCompleted, ListCompleted,
                FilterListAppUser, FilterListOrganization, FilterListERouteType, FilterListRequestState, FilterListStatus,  FilterListStore,
                Export,
                SingleListAppUser, SingleListERouteType, SingleListRequestState, SingleListStatus,  SingleListStore,  SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },
        };
    }
}
