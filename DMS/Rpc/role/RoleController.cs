using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MAppUser;
using DMS.Services.MBrand;
using DMS.Services.MERouteType;
using DMS.Services.MField;
using DMS.Services.MMenu;
using DMS.Services.MOrganization;
using DMS.Services.MPermissionOperator;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MRole;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using DMS.Services.MSupplier;
using DMS.Services.MWarehouse;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public partial class RoleController : RpcController
    {
        private IAppUserService AppUserService;
        private IMenuService MenuService;
        private IRoleService RoleService;
        private IPermissionService PermissionService;
        private IStatusService StatusService;
        private IBrandService BrandService;
        private IOrganizationService OrganizationService;
        private IProductService ProductService;
        private IProductGroupingService ProductGroupingService;
        private IProductTypeService ProductTypeService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private ISupplierService SupplierService;
        private IWarehouseService WarehouseService;
        private IFieldService FieldService;
        private IPermissionOperatorService PermissionOperatorService;
        private IERouteTypeService ERouteTypeService;
        private IRequestStateService RequestStateService;
        private ICurrentContext CurrentContext;


        public RoleController(
            IAppUserService AppUserService,
            IMenuService MenuService,
            IRoleService RoleService,
            IPermissionService PermissionService,
            IBrandService BrandService,
            IOrganizationService OrganizationService,
            IProductService ProductService,
            IProductGroupingService ProductGroupingService,
            IProductTypeService ProductTypeService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            ISupplierService SupplierService,
            IWarehouseService WarehouseService,
            IFieldService FieldService,
            IPermissionOperatorService PermissionOperatorService,
            IERouteTypeService ERouteTypeService,
            IRequestStateService RequestStateService,
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.MenuService = MenuService;
            this.RoleService = RoleService;
            this.PermissionService = PermissionService;
            this.BrandService = BrandService;
            this.OrganizationService = OrganizationService;
            this.ProductService = ProductService;
            this.ProductGroupingService = ProductGroupingService;
            this.ProductTypeService = ProductTypeService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.SupplierService = SupplierService;
            this.WarehouseService = WarehouseService;
            this.FieldService = FieldService;
            this.StatusService = StatusService;
            this.PermissionOperatorService = PermissionOperatorService;
            this.ERouteTypeService = ERouteTypeService;
            this.RequestStateService = RequestStateService;
            this.CurrentContext = CurrentContext;
        }

        [Route(RoleRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Role_RoleFilterDTO Role_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = ConvertFilterDTOToFilterEntity(Role_RoleFilterDTO);
            RoleFilter = RoleService.ToFilter(RoleFilter);
            int count = await RoleService.Count(RoleFilter);
            return count;
        }

        [Route(RoleRoute.List), HttpPost]
        public async Task<ActionResult<List<Role_RoleDTO>>> List([FromBody] Role_RoleFilterDTO Role_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = ConvertFilterDTOToFilterEntity(Role_RoleFilterDTO);
            RoleFilter = RoleService.ToFilter(RoleFilter);
            List<Role> Roles = await RoleService.List(RoleFilter);
            List<Role_RoleDTO> Role_RoleDTOs = Roles
                .Select(c => new Role_RoleDTO(c)).ToList();
            return Role_RoleDTOs;
        }

        [Route(RoleRoute.Get), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> Get([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            Role Role = await RoleService.Get(Role_RoleDTO.Id);
            return new Role_RoleDTO(Role);
        }

        [Route(RoleRoute.Clone), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> Clone([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            var Role = await RoleService.Clone(Role_RoleDTO.Id);
            Role_RoleDTO = new Role_RoleDTO(Role);
            if (Role.IsValidated)
                return Role_RoleDTO;
            else
                return BadRequest(Role_RoleDTO);
        }

        [Route(RoleRoute.Create), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> Create([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            Role Role = ConvertDTOToEntity(Role_RoleDTO);
            Role = await RoleService.Create(Role);
            Role_RoleDTO = new Role_RoleDTO(Role);
            if (Role.IsValidated)
                return Role_RoleDTO;
            else
                return BadRequest(Role_RoleDTO);
        }

        [Route(RoleRoute.Update), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> Update([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            Role Role = ConvertDTOToEntity(Role_RoleDTO);
            Role = await RoleService.Update(Role);
            Role_RoleDTO = new Role_RoleDTO(Role);
            if (Role.IsValidated)
                return Role_RoleDTO;
            else
                return BadRequest(Role_RoleDTO);
        }

        [Route(RoleRoute.Delete), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> Delete([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            Role Role = ConvertDTOToEntity(Role_RoleDTO);
            Role = await RoleService.Delete(Role);
            Role_RoleDTO = new Role_RoleDTO(Role);
            if (Role.IsValidated)
                return Role_RoleDTO;
            else
                return BadRequest(Role_RoleDTO);
        }

        [Route(RoleRoute.AssignAppUser), HttpPost]
        public async Task<ActionResult<Role_RoleDTO>> AssignAppUser([FromBody] Role_RoleDTO Role_RoleDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Role_RoleDTO.Id))
                return Forbid();

            Role Role = ConvertDTOToEntity(Role_RoleDTO);
            Role = await RoleService.AssignAppUser(Role);
            Role_RoleDTO = new Role_RoleDTO(Role);
            if (Role.IsValidated)
                return Role_RoleDTO;
            else
                return BadRequest(Role_RoleDTO);
        }

        private async Task<bool> HasPermission(long Id)
        {
            RoleFilter RoleFilter = new RoleFilter();
            if (Id == 0)
            {

            }
            else
            {
                RoleFilter.Id = new IdFilter { Equal = Id };
                RoleFilter = RoleService.ToFilter(RoleFilter);
                int count = await RoleService.Count(RoleFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        public Role ConvertDTOToEntity(Role_RoleDTO Role_RoleDTO)
        {
            Role_RoleDTO.TrimString();
            Role Role = new Role();
            Role.Id = Role_RoleDTO.Id;
            Role.Code = Role_RoleDTO.Code;
            Role.Name = Role_RoleDTO.Name;
            Role.StatusId = Role_RoleDTO.StatusId;
            Role.AppUserRoleMappings = Role_RoleDTO.AppUserRoleMappings?
                .Select(x => new AppUserRoleMapping
                {
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId
                }).ToList();
            Role.BaseLanguage = CurrentContext.Language;
            return Role;
        }

        public RoleFilter ConvertFilterDTOToFilterEntity(Role_RoleFilterDTO Role_RoleFilterDTO)
        {
            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Skip = Role_RoleFilterDTO.Skip;
            RoleFilter.Take = Role_RoleFilterDTO.Take;
            RoleFilter.OrderBy = Role_RoleFilterDTO.OrderBy;
            RoleFilter.OrderType = Role_RoleFilterDTO.OrderType;

            RoleFilter.Id = Role_RoleFilterDTO.Id;
            RoleFilter.Code = Role_RoleFilterDTO.Code;
            RoleFilter.Name = Role_RoleFilterDTO.Name;
            RoleFilter.StatusId = Role_RoleFilterDTO.StatusId;
            return RoleFilter;
        }

        [Route(RoleRoute.GetMenu), HttpPost]
        public async Task<ActionResult<Role_MenuDTO>> GetMenu([FromBody] Role_MenuDTO Role_MenuDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Menu Menu = await MenuService.Get(Role_MenuDTO.Id);
            return new Role_MenuDTO(Menu);
        }

        [Route(RoleRoute.CountPermission), HttpPost]
        public async Task<long> CountPermission([FromBody] Role_PermissionFilterDTO Role_PermissionFilterDTO)
        {
            PermissionFilter PermissionFilter = new PermissionFilter();
            PermissionFilter.Id = Role_PermissionFilterDTO.Id;
            PermissionFilter.Code = Role_PermissionFilterDTO.Code;
            PermissionFilter.Name = Role_PermissionFilterDTO.Name;
            PermissionFilter.RoleId = Role_PermissionFilterDTO.RoleId;
            PermissionFilter.MenuId = Role_PermissionFilterDTO.MenuId;

            return await PermissionService.Count(PermissionFilter);
        }
        [Route(RoleRoute.ListPermission), HttpPost]
        public async Task<List<Role_PermissionDTO>> ListPermission([FromBody] Role_PermissionFilterDTO Role_PermissionFilterDTO)
        {
            PermissionFilter PermissionFilter = new PermissionFilter();
            PermissionFilter.Skip = Role_PermissionFilterDTO.Skip;
            PermissionFilter.Take = Role_PermissionFilterDTO.Take;
            PermissionFilter.OrderBy = PermissionOrder.Id;
            PermissionFilter.OrderType = OrderType.ASC;
            PermissionFilter.Selects = PermissionSelect.ALL;
            PermissionFilter.Id = Role_PermissionFilterDTO.Id;
            PermissionFilter.Code = Role_PermissionFilterDTO.Code;
            PermissionFilter.Name = Role_PermissionFilterDTO.Name;
            PermissionFilter.RoleId = Role_PermissionFilterDTO.RoleId;
            PermissionFilter.MenuId = Role_PermissionFilterDTO.MenuId;

            List<Permission> Permissions = await PermissionService.List(PermissionFilter);
            List<Role_PermissionDTO> Role_PermissionDTOs = Permissions
                .Select(x => new Role_PermissionDTO(x)).ToList();
            return Role_PermissionDTOs;
        }
        [Route(RoleRoute.GetPermission), HttpPost]
        public async Task<ActionResult<Role_PermissionDTO>> GetPermission([FromBody] Role_PermissionDTO Role_PermissionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Permission Permission = await PermissionService.Get(Role_PermissionDTO.Id);
            return new Role_PermissionDTO(Permission);
        }
        [Route(RoleRoute.CreatePermission), HttpPost]
        public async Task<ActionResult<Role_PermissionDTO>> CreatePermission([FromBody] Role_PermissionDTO Role_PermissionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Permission Permission = ConvertPermissionDTOToBO(Role_PermissionDTO);
            Permission = await PermissionService.Create(Permission);
            Role_PermissionDTO = new Role_PermissionDTO(Permission);
            if (Permission.IsValidated)
                return Role_PermissionDTO;
            else
                return BadRequest(Role_PermissionDTO);
        }
        [Route(RoleRoute.UpdatePermission), HttpPost]
        public async Task<ActionResult<Role_PermissionDTO>> UpdatePermission([FromBody] Role_PermissionDTO Role_PermissionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Permission Permission = ConvertPermissionDTOToBO(Role_PermissionDTO);
            Permission = await PermissionService.Update(Permission);
            Role_PermissionDTO = new Role_PermissionDTO(Permission);
            if (Permission.IsValidated)
                return Role_PermissionDTO;
            else
                return BadRequest(Role_PermissionDTO);
        }
        [Route(RoleRoute.DeletePermission), HttpPost]
        public async Task<ActionResult<Role_PermissionDTO>> DeletePermission([FromBody] Role_PermissionDTO Role_PermissionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            Permission Permission = ConvertPermissionDTOToBO(Role_PermissionDTO);
            Permission = await PermissionService.Delete(Permission);
            Role_PermissionDTO = new Role_PermissionDTO(Permission);
            if (Permission.IsValidated)
                return Role_PermissionDTO;
            else
                return BadRequest(Role_PermissionDTO);
        }

        public Permission ConvertPermissionDTOToBO(Role_PermissionDTO Role_PermissionDTO)
        {
            Permission Permission = new Permission
            {
                Id = Role_PermissionDTO.Id,
                Code = Role_PermissionDTO.Code,
                Name = Role_PermissionDTO.Name,
                RoleId = Role_PermissionDTO.RoleId,
                MenuId = Role_PermissionDTO.MenuId,
                StatusId = Role_PermissionDTO.StatusId,
                Menu = Role_PermissionDTO.Menu == null ? null : new Menu
                {
                    Id = Role_PermissionDTO.Menu.Id,
                    Code = Role_PermissionDTO.Menu.Code,
                    Name = Role_PermissionDTO.Menu.Name,
                    Path = Role_PermissionDTO.Menu.Path,
                    IsDeleted = Role_PermissionDTO.Menu.IsDeleted,
                    Fields = Role_PermissionDTO.Menu.Fields?.Select(f => new Field
                    {
                        Id = f.Id,
                        Name = f.Name,
                        FieldTypeId = f.FieldTypeId,
                    }).ToList(),
                    Actions = Role_PermissionDTO.Menu.Actions?.Select(p => new Entities.Action
                    {
                        Id = p.Id,
                        Name = p.Name,
                    }).ToList(),
                },
                PermissionContents = Role_PermissionDTO.PermissionContents?.Select(pf => new PermissionContent
                {
                    Id = pf.Id,
                    PermissionOperatorId = pf.PermissionOperatorId,
                    FieldId = pf.FieldId,
                    Value = pf.Value,
                    Field = pf.Field == null ? null : new Field
                    {
                        Id = pf.Field.Id,
                        Name = pf.Field.Name,
                        FieldTypeId = pf.Field.FieldTypeId,
                    },
                    PermissionOperator = pf.PermissionOperator == null ? null : new PermissionOperator
                    {
                        Id = pf.PermissionOperator.Id,
                        Name = pf.PermissionOperator.Name,
                        Code = pf.PermissionOperator.Code,
                        FieldTypeId = pf.PermissionOperator.FieldTypeId,
                    }
                }).ToList(),
                PermissionActionMappings = Role_PermissionDTO.PermissionActionMappings?.Select(pp => new PermissionActionMapping
                {
                    ActionId = pp.ActionId,
                }).ToList(),
            };
            Permission.BaseLanguage = CurrentContext.Language;
            return Permission;
        }

        [Route(RoleRoute.CountAppUser), HttpPost]
        public async Task<long> CountAppUser([FromBody] Role_AppUserFilterDTO Role_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Id = Role_AppUserFilterDTO.Id;
            AppUserFilter.Username = Role_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Role_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = Role_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Role_AppUserFilterDTO.Phone;
            AppUserFilter.OrganizationId = Role_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await AppUserService.Count(AppUserFilter);
        }
        [Route(RoleRoute.ListAppUser), HttpPost]
        public async Task<List<Role_AppUserDTO>> ListAppUser([FromBody] Role_AppUserFilterDTO Role_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = Role_AppUserFilterDTO.Skip;
            AppUserFilter.Take = Role_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Role_AppUserFilterDTO.Id;
            AppUserFilter.Username = Role_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Role_AppUserFilterDTO.DisplayName;
            AppUserFilter.Email = Role_AppUserFilterDTO.Email;
            AppUserFilter.OrganizationId = Role_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Phone = Role_AppUserFilterDTO.Phone;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Role_AppUserDTO> Role_AppUserDTOs = AppUsers
                .Select(x => new Role_AppUserDTO(x)).ToList();
            return Role_AppUserDTOs;
        }
    }
}

