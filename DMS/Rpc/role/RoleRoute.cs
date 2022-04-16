using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    [DisplayName("Vai trò")]
    public class RoleRoute : Root
    {
        public const string Parent = Module + "/account";
        public const string Master = Module + "/account/role/role-master";
        public const string Detail = Module + "/account/role/role-detail/*";
        private const string Default = Rpc + Module + "/role";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Clone = Default + "/clone";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string AssignAppUser = Default + "/assign-app-user";
        public const string GetMenu = Default + "/get-menu";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListCurrentUser = Default + "/single-list-current-user";

        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListMenu = Default + "/single-list-menu";
        public const string SingleListBrand = Default + "/single-list-brand";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListProduct = Default + "/single-list-product";
        public const string SingleListProductGrouping = Default + "/single-list-product-grouping";
        public const string SingleListProductType = Default + "/single-list-product-type";
        public const string SingleListReseller = Default + "/single-list-reseller";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListSupplier = Default + "/single-list-supplier";
        public const string SingleListWarehouse = Default + "/single-list-warehouse";
        public const string SingleListField = Default + "/single-list-field";
        public const string SingleListPermissionOperator = Default + "/single-list-permission-operator";
        public const string SingleListERouteType = Default + "/single-list-e-route-type";
        public const string SingleListRequestState = Default + "/single-list-request-state";

        public const string CountAppUser = Default + "/count-app-user";
        public const string ListAppUser = Default + "/list-app-user";

        public const string CountPermission = Default + "/count-permission";
        public const string ListPermission = Default + "/list-permission";
        public const string GetPermission = Default + "/get-permission";
        public const string CreatePermission = Default + "/create-permission";
        public const string UpdatePermission = Default + "/update-permission";
        public const string DeletePermission = Default + "/delete-permission";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(RoleFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(RoleFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(RoleFilter.StatusId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get, Clone,
                SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator, SingleListCurrentUser } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get, Clone, CountPermission, ListPermission, GetPermission, CreatePermission, UpdatePermission, DeletePermission,
                SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator, SingleListERouteType, SingleListRequestState,
                Detail, Create, GetMenu,
                SingleListStatus ,SingleListCurrentUser} },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get, Clone, CountPermission, ListPermission, GetPermission, CreatePermission, UpdatePermission, DeletePermission,
                SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator, SingleListERouteType,
                SingleListRequestState, SingleListCurrentUser,
                Detail, Update, GetMenu,
                 } },
             { "Gán người dùng", new List<string> {
                 Parent,
                Master, Count, List, Get, Clone,
                CountAppUser, ListAppUser,
                SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator, SingleListCurrentUser,
                Detail, AssignAppUser,
                } },
             { "Tạo nhanh quyền", new List<string> {
                 Parent,
                Master, Count, List, Get, Clone,
                Detail, CreatePermission, GetMenu, Master, Count, List, Get, CountPermission, ListPermission, GetPermission, CreatePermission, UpdatePermission, DeletePermission,
                 SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator, SingleListCurrentUser,} },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get, Clone,
                SingleListAppUser, SingleListStatus, SingleListMenu, SingleListBrand, SingleListOrganization, SingleListProduct, SingleListProductGrouping, SingleListProductType, SingleListReseller,
                SingleListStore, SingleListStoreGrouping, SingleListStoreType, SingleListSupplier, SingleListWarehouse, SingleListField, SingleListPermissionOperator,SingleListCurrentUser,
                Detail, Delete,
                 } },
        };
    }
}
