using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public partial class RoleController : RpcController
    {

        [Route(RoleRoute.SingleListCurrentUser), HttpPost]
        public async Task<List<GenericEnum>> SingleListCurrentUser()
        {
            return CurrentUserEnum.CurrentUserEnumList;
        }
        [Route(RoleRoute.SingleListAppUser), HttpPost]
        public async Task<List<Role_AppUserDTO>> SingleListAppUser([FromBody] Role_AppUserFilterDTO Role_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName | AppUserSelect.Email | AppUserSelect.Phone;
            AppUserFilter.Id = Role_AppUserFilterDTO.Id;
            AppUserFilter.Username = Role_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Role_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = Role_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Role_AppUserFilterDTO.Phone;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Role_AppUserDTO> Role_AppUserDTOs = AppUsers
                .Select(x => new Role_AppUserDTO(x)).ToList();
            return Role_AppUserDTOs;
        }
        [Route(RoleRoute.SingleListMenu), HttpPost]
        public async Task<List<Role_MenuDTO>> SingleListMenu([FromBody] Role_MenuFilterDTO Role_MenuFilterDTO)
        {
            MenuFilter MenuFilter = new MenuFilter();
            MenuFilter.Skip = 0;
            MenuFilter.Take = 20;
            MenuFilter.OrderBy = MenuOrder.Id;
            MenuFilter.OrderType = OrderType.ASC;
            MenuFilter.Selects = MenuSelect.ALL;
            MenuFilter.Id = Role_MenuFilterDTO.Id;
            MenuFilter.Code = Role_MenuFilterDTO.Code;
            MenuFilter.Name = Role_MenuFilterDTO.Name;
            MenuFilter.Path = Role_MenuFilterDTO.Path;

            List<Menu> Menus = await MenuService.List(MenuFilter);
            List<Role_MenuDTO> Role_MenuDTOs = Menus
                .Select(x => new Role_MenuDTO(x)).ToList();
            return Role_MenuDTOs;
        }

        [Route(RoleRoute.SingleListStatus), HttpPost]
        public async Task<List<Role_StatusDTO>> SingleListStatus([FromBody] Role_StatusFilterDTO Role_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = Role_StatusFilterDTO.Id;
            StatusFilter.Code = Role_StatusFilterDTO.Code;
            StatusFilter.Name = Role_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Role_StatusDTO> Role_StatusDTOs = Statuses
                .Select(x => new Role_StatusDTO(x)).ToList();
            return Role_StatusDTOs;
        }

        [Route(RoleRoute.SingleListBrand), HttpPost]
        public async Task<List<Role_BrandDTO>> SingleListBrand([FromBody] Role_BrandFilterDTO Role_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = 20;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.Id | BrandSelect.Code | BrandSelect.Name;
            BrandFilter.Id = Role_BrandFilterDTO.Id;
            BrandFilter.Code = Role_BrandFilterDTO.Code;
            BrandFilter.Name = Role_BrandFilterDTO.Name;

            List<Brand> Brandes = await BrandService.List(BrandFilter);
            List<Role_BrandDTO> Role_BrandDTOs = Brandes
                .Select(x => new Role_BrandDTO(x)).ToList();
            return Role_BrandDTOs;
        }

        [Route(RoleRoute.SingleListOrganization), HttpPost]
        public async Task<List<Role_OrganizationDTO>> SingleListOrganization([FromBody] Role_OrganizationFilterDTO Role_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.Id | OrganizationSelect.Code | OrganizationSelect.Name | OrganizationSelect.Parent;
            OrganizationFilter.Id = Role_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Role_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Role_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Role_OrganizationFilterDTO.ParentId;
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizationes = await OrganizationService.List(OrganizationFilter);
            List<Role_OrganizationDTO> Role_OrganizationDTOs = Organizationes
                .Select(x => new Role_OrganizationDTO(x)).ToList();
            return Role_OrganizationDTOs;
        }

        [Route(RoleRoute.SingleListProduct), HttpPost]
        public async Task<List<Role_ProductDTO>> SingleListProduct([FromBody] Role_ProductFilterDTO Role_ProductFilterDTO)
        {
            ProductFilter ProductFilter = new ProductFilter();
            ProductFilter.Skip = 0;
            ProductFilter.Take = 20;
            ProductFilter.OrderBy = ProductOrder.Id;
            ProductFilter.OrderType = OrderType.ASC;
            ProductFilter.Selects = ProductSelect.Id | ProductSelect.Code | ProductSelect.Name;
            ProductFilter.Id = Role_ProductFilterDTO.Id;
            ProductFilter.Code = Role_ProductFilterDTO.Code;
            ProductFilter.Name = Role_ProductFilterDTO.Name;

            List<Product> Productes = await ProductService.List(ProductFilter);
            List<Role_ProductDTO> Role_ProductDTOs = Productes
                .Select(x => new Role_ProductDTO(x)).ToList();
            return Role_ProductDTOs;
        }

        [Route(RoleRoute.SingleListProductGrouping), HttpPost]
        public async Task<List<Role_ProductGroupingDTO>> SingleListProductGrouping([FromBody] Role_ProductGroupingFilterDTO Role_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;
            ProductGroupingFilter.Id = Role_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = Role_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = Role_ProductGroupingFilterDTO.Name;

            List<ProductGrouping> ProductGroupinges = await ProductGroupingService.List(ProductGroupingFilter);
            List<Role_ProductGroupingDTO> Role_ProductGroupingDTOs = ProductGroupinges
                .Select(x => new Role_ProductGroupingDTO(x)).ToList();
            return Role_ProductGroupingDTOs;
        }

        [Route(RoleRoute.SingleListProductType), HttpPost]
        public async Task<List<Role_ProductTypeDTO>> SingleListProductType([FromBody] Role_ProductTypeFilterDTO Role_ProductTypeFilterDTO)
        {
            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.Id | ProductTypeSelect.Code | ProductTypeSelect.Name;
            ProductTypeFilter.Id = Role_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = Role_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = Role_ProductTypeFilterDTO.Name;

            List<ProductType> ProductTypees = await ProductTypeService.List(ProductTypeFilter);
            List<Role_ProductTypeDTO> Role_ProductTypeDTOs = ProductTypees
                .Select(x => new Role_ProductTypeDTO(x)).ToList();
            return Role_ProductTypeDTOs;
        }

        [Route(RoleRoute.SingleListStore), HttpPost]
        public async Task<List<Role_StoreDTO>> SingleListStore([FromBody] Role_StoreFilterDTO Role_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name;
            StoreFilter.Id = Role_StoreFilterDTO.Id;
            StoreFilter.Code = Role_StoreFilterDTO.Code;
            StoreFilter.Name = Role_StoreFilterDTO.Name;

            List<Store> Storees = await StoreService.List(StoreFilter);
            List<Role_StoreDTO> Role_StoreDTOs = Storees
                .Select(x => new Role_StoreDTO(x)).ToList();
            return Role_StoreDTOs;
        }

        [Route(RoleRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<Role_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] Role_StoreGroupingFilterDTO Role_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 20;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.Id | StoreGroupingSelect.Code | StoreGroupingSelect.Name;
            StoreGroupingFilter.Id = Role_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = Role_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Role_StoreGroupingFilterDTO.Name;

            List<StoreGrouping> StoreGroupinges = await StoreGroupingService.List(StoreGroupingFilter);
            List<Role_StoreGroupingDTO> Role_StoreGroupingDTOs = StoreGroupinges
                .Select(x => new Role_StoreGroupingDTO(x)).ToList();
            return Role_StoreGroupingDTOs;
        }

        [Route(RoleRoute.SingleListStoreType), HttpPost]
        public async Task<List<Role_StoreTypeDTO>> SingleListStoreType([FromBody] Role_StoreTypeFilterDTO Role_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.Id | StoreTypeSelect.Code | StoreTypeSelect.Name;
            StoreTypeFilter.Id = Role_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Role_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Role_StoreTypeFilterDTO.Name;

            List<StoreType> StoreTypees = await StoreTypeService.List(StoreTypeFilter);
            List<Role_StoreTypeDTO> Role_StoreTypeDTOs = StoreTypees
                .Select(x => new Role_StoreTypeDTO(x)).ToList();
            return Role_StoreTypeDTOs;
        }

        [Route(RoleRoute.SingleListSupplier), HttpPost]
        public async Task<List<Role_SupplierDTO>> SingleListSupplier([FromBody] Role_SupplierFilterDTO Role_SupplierFilterDTO)
        {
            SupplierFilter SupplierFilter = new SupplierFilter();
            SupplierFilter.Skip = 0;
            SupplierFilter.Take = 20;
            SupplierFilter.OrderBy = SupplierOrder.Id;
            SupplierFilter.OrderType = OrderType.ASC;
            SupplierFilter.Selects = SupplierSelect.Id | SupplierSelect.Code | SupplierSelect.Name;
            SupplierFilter.Id = Role_SupplierFilterDTO.Id;
            SupplierFilter.Code = Role_SupplierFilterDTO.Code;
            SupplierFilter.Name = Role_SupplierFilterDTO.Name;

            List<Supplier> Supplieres = await SupplierService.List(SupplierFilter);
            List<Role_SupplierDTO> Role_SupplierDTOs = Supplieres
                .Select(x => new Role_SupplierDTO(x)).ToList();
            return Role_SupplierDTOs;
        }

        [Route(RoleRoute.SingleListWarehouse), HttpPost]
        public async Task<List<Role_WarehouseDTO>> SingleListWarehouse([FromBody] Role_WarehouseFilterDTO Role_WarehouseFilterDTO)
        {
            WarehouseFilter WarehouseFilter = new WarehouseFilter();
            WarehouseFilter.Skip = 0;
            WarehouseFilter.Take = 20;
            WarehouseFilter.OrderBy = WarehouseOrder.Id;
            WarehouseFilter.OrderType = OrderType.ASC;
            WarehouseFilter.Selects = WarehouseSelect.Id | WarehouseSelect.Code | WarehouseSelect.Name;
            WarehouseFilter.Id = Role_WarehouseFilterDTO.Id;
            WarehouseFilter.Code = Role_WarehouseFilterDTO.Code;
            WarehouseFilter.Name = Role_WarehouseFilterDTO.Name;

            List<Warehouse> Warehousees = await WarehouseService.List(WarehouseFilter);
            List<Role_WarehouseDTO> Role_WarehouseDTOs = Warehousees
                .Select(x => new Role_WarehouseDTO(x)).ToList();
            return Role_WarehouseDTOs;
        }

        [Route(RoleRoute.SingleListField), HttpPost]
        public async Task<List<Role_FieldDTO>> SingleListField([FromBody] Role_FieldFilterDTO Role_FieldFilterDTO)
        {
            FieldFilter FieldFilter = new FieldFilter();
            FieldFilter.Skip = 0;
            FieldFilter.Take = 200;
            FieldFilter.OrderBy = FieldOrder.Id;
            FieldFilter.OrderType = OrderType.ASC;
            FieldFilter.Selects = FieldSelect.ALL;
            FieldFilter.Id = Role_FieldFilterDTO.Id;
            FieldFilter.MenuId = Role_FieldFilterDTO.MenuId;
            FieldFilter.Name = Role_FieldFilterDTO.Name;

            List<Field> Fieldes = await FieldService.List(FieldFilter);
            List<Role_FieldDTO> Role_FieldDTOs = Fieldes
                .Select(x => new Role_FieldDTO(x)).ToList();
            return Role_FieldDTOs;
        }

        [Route(RoleRoute.SingleListPermissionOperator), HttpPost]
        public async Task<List<Role_PermissionOperatorDTO>> SingleListPermissionOperator([FromBody] Role_PermissionOperatorFilterDTO Role_PermissionOperatorFilterDTO)
        {
            PermissionOperatorFilter PermissionOperatorFilter = new PermissionOperatorFilter();
            PermissionOperatorFilter.Skip = 0;
            PermissionOperatorFilter.Take = 200;
            PermissionOperatorFilter.Id = Role_PermissionOperatorFilterDTO.Id;
            PermissionOperatorFilter.Code = Role_PermissionOperatorFilterDTO.Code;
            PermissionOperatorFilter.Name = Role_PermissionOperatorFilterDTO.Name;
            PermissionOperatorFilter.FieldTypeId = Role_PermissionOperatorFilterDTO.FieldTypeId;

            List<PermissionOperator> PermissionOperatores = await PermissionOperatorService.List(PermissionOperatorFilter);
            List<Role_PermissionOperatorDTO> Role_PermissionOperatorDTOs = PermissionOperatores
                .Select(x => new Role_PermissionOperatorDTO(x)).ToList();
            return Role_PermissionOperatorDTOs;
        }

        [Route(RoleRoute.SingleListERouteType), HttpPost]
        public async Task<List<Role_ERouteTypeDTO>> SingleListERouteType()
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter();
            ERouteTypeFilter.Skip = 0;
            ERouteTypeFilter.Take = 20;
            ERouteTypeFilter.OrderBy = ERouteTypeOrder.Id;
            ERouteTypeFilter.OrderType = OrderType.ASC;
            ERouteTypeFilter.Selects = ERouteTypeSelect.ALL;

            List<ERouteType> ERouteTypees = await ERouteTypeService.List(ERouteTypeFilter);
            List<Role_ERouteTypeDTO> Role_ERouteTypeDTOs = ERouteTypees
                .Select(x => new Role_ERouteTypeDTO(x)).ToList();
            return Role_ERouteTypeDTOs;
        }

        [Route(RoleRoute.SingleListRequestState), HttpPost]
        public async Task<List<Role_RequestStateDTO>> SingleListRequestState()
        {
            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;

            List<RequestState> RequestStatees = await RequestStateService.List(RequestStateFilter);
            List<Role_RequestStateDTO> Role_RequestStateDTOs = RequestStatees
                .Select(x => new Role_RequestStateDTO(x)).ToList();
            return Role_RequestStateDTOs;
        }

    }
}

