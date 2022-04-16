using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequestRoute : Root
    {
        public const string Parent = Module + "/route";
        public const string Master = Module + "/route/e-route-change-request/e-route-change-request-master";
        public const string Detail = Module + "/route/e-route-change-request/e-route-change-request-detail/*";
        private const string Default = Rpc + Module + "/e-route-change-request";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string GetDraft = Default + "/get-draft";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListERoute = Default + "/filter-list-e-route";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListRequestState = Default + "/filter-list-request-state";
        public const string FilterListERouteType = Default + "/filter-list-eroute-type";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListERoute = Default + "/single-list-e-route";
        public const string SingleListERouteChangeRequestContent = Default + "/single-list-e-route-change-request-content";
        public const string SingleListERouteType = Default + "/single-list-eroute-type";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListRequestState = Default + "/single-list-request-state";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListStore = Default + "/single-list-store";

        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ERouteChangeRequestFilter.ERouteId), FieldTypeEnum.ID.Id },
            { nameof(ERouteChangeRequestFilter.CreatorId), FieldTypeEnum.ID.Id },
            { nameof(ERouteChangeRequestFilter.RequestStateId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreGroupingId), FieldTypeEnum.ID.Id },
            { nameof(StoreFilter.StoreTypeId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListERoute, FilterListRequestState,  FilterListStore,FilterListERouteType, } },

            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListERoute, FilterListRequestState,  FilterListStore, FilterListERouteType,
                Detail, Create, GetDraft,
                SingleListAppUser, SingleListERoute, SingleListRequestState,  SingleListStore, SingleListERouteChangeRequestContent, SingleListERouteType, SingleListOrganization, SingleListStoreType,
                CountStore, ListStore, } },

            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListERoute, FilterListRequestState,  FilterListStore, FilterListERouteType,
                Detail, Update,
                SingleListAppUser, SingleListERoute, SingleListRequestState,  SingleListStore, SingleListERouteChangeRequestContent, SingleListERouteType,  SingleListOrganization, SingleListStoreType,
                CountStore, ListStore,  } },

            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListERoute, FilterListRequestState,  FilterListStore, FilterListERouteType,
                Detail, Delete, } },

            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListERoute, FilterListRequestState,  FilterListStore, FilterListERouteType,
                BulkDelete } },

        };
    }
}
