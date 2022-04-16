using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.app_user
{
    public partial class AppUserController
    {
        [Route(AppUserRoute.FilterListOrganization), HttpPost]
        public async Task<List<AppUser_OrganizationDTO>> FilterListOrganization([FromBody] AppUser_OrganizationFilterDTO AppUser_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = AppUser_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = AppUser_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = AppUser_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = AppUser_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = AppUser_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = AppUser_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = AppUser_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = AppUser_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = AppUser_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = AppUser_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.OrFilter == null) OrganizationFilter.OrFilter = new List<OrganizationFilter>();
            if (CurrentContext.Filters != null)
            {
                foreach (var currentFilter in CurrentContext.Filters)
                {
                    OrganizationFilter subFilter = new OrganizationFilter();
                    OrganizationFilter.OrFilter.Add(subFilter);
                    List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                    foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                    {
                        if (FilterPermissionDefinition.Name == nameof(AppUserFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<AppUser_OrganizationDTO> AppUser_OrganizationDTOs = Organizations
                .Select(x => new AppUser_OrganizationDTO(x)).ToList();
            return AppUser_OrganizationDTOs;
        }

        [Route(AppUserRoute.FilterListPosition), HttpPost]
        public async Task<List<AppUser_PositionDTO>> FilterListPosition([FromBody] AppUser_PositionFilterDTO AppUser_PositionFilterDTO)
        {
            PositionFilter PositionFilter = new PositionFilter();
            PositionFilter.Skip = 0;
            PositionFilter.Take = 99999;
            PositionFilter.OrderBy = PositionOrder.Id;
            PositionFilter.OrderType = OrderType.ASC;
            PositionFilter.Selects = PositionSelect.ALL;
            PositionFilter.Id = AppUser_PositionFilterDTO.Id;
            PositionFilter.Code = AppUser_PositionFilterDTO.Code;
            PositionFilter.Name = AppUser_PositionFilterDTO.Name;
            PositionFilter.StatusId = AppUser_PositionFilterDTO.StatusId;

            List<Position> Positions = await PositionService.List(PositionFilter);
            List<AppUser_PositionDTO> AppUser_PositionDTOs = Positions
                .Select(x => new AppUser_PositionDTO(x)).ToList();
            return AppUser_PositionDTOs;
        }

        [Route(AppUserRoute.FilterListStatus), HttpPost]
        public async Task<List<AppUser_StatusDTO>> FilterListStatus([FromBody] AppUser_StatusFilterDTO AppUser_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = AppUser_StatusFilterDTO.Id;
            StatusFilter.Code = AppUser_StatusFilterDTO.Code;
            StatusFilter.Name = AppUser_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<AppUser_StatusDTO> AppUser_StatusDTOs = Statuses
                .Select(x => new AppUser_StatusDTO(x)).ToList();
            return AppUser_StatusDTOs;
        }

        [Route(AppUserRoute.SingleListOrganization), HttpPost]
        public async Task<List<AppUser_OrganizationDTO>> SingleListOrganization([FromBody] AppUser_OrganizationFilterDTO AppUser_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = AppUser_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = AppUser_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = AppUser_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = AppUser_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = AppUser_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = AppUser_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = AppUser_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = AppUser_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = AppUser_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.OrFilter == null) OrganizationFilter.OrFilter = new List<OrganizationFilter>();
            if (CurrentContext.Filters != null)
            {
                foreach (var currentFilter in CurrentContext.Filters)
                {
                    OrganizationFilter subFilter = new OrganizationFilter();
                    OrganizationFilter.OrFilter.Add(subFilter);
                    List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                    foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                    {
                        if (FilterPermissionDefinition.Name == nameof(AppUserFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<AppUser_OrganizationDTO> AppUser_OrganizationDTOs = Organizations
                .Select(x => new AppUser_OrganizationDTO(x)).ToList();
            return AppUser_OrganizationDTOs;
        }

        [Route(AppUserRoute.SingleListPosition), HttpPost]
        public async Task<List<AppUser_PositionDTO>> SingleListPosition([FromBody] AppUser_PositionFilterDTO AppUser_PositionFilterDTO)
        {
            PositionFilter PositionFilter = new PositionFilter();
            PositionFilter.Skip = 0;
            PositionFilter.Take = 99999;
            PositionFilter.OrderBy = PositionOrder.Id;
            PositionFilter.OrderType = OrderType.ASC;
            PositionFilter.Selects = PositionSelect.ALL;
            PositionFilter.Id = AppUser_PositionFilterDTO.Id;
            PositionFilter.Code = AppUser_PositionFilterDTO.Code;
            PositionFilter.Name = AppUser_PositionFilterDTO.Name;
            PositionFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<Position> Positions = await PositionService.List(PositionFilter);
            List<AppUser_PositionDTO> AppUser_PositionDTOs = Positions
                .Select(x => new AppUser_PositionDTO(x)).ToList();
            return AppUser_PositionDTOs;
        }

        [Route(AppUserRoute.SingleListSex), HttpPost]
        public async Task<List<AppUser_SexDTO>> SingleListSex([FromBody] AppUser_SexFilterDTO AppUser_SexFilterDTO)
        {
            SexFilter SexFilter = new SexFilter();
            SexFilter.Skip = 0;
            SexFilter.Take = 20;
            SexFilter.OrderBy = SexOrder.Id;
            SexFilter.OrderType = OrderType.ASC;
            SexFilter.Selects = SexSelect.ALL;
            SexFilter.Id = AppUser_SexFilterDTO.Id;
            SexFilter.Code = AppUser_SexFilterDTO.Code;
            SexFilter.Name = AppUser_SexFilterDTO.Name;

            List<Sex> Sexes = await SexService.List(SexFilter);
            List<AppUser_SexDTO> AppUser_SexDTOs = Sexes
                .Select(x => new AppUser_SexDTO(x)).ToList();
            return AppUser_SexDTOs;
        }
        [Route(AppUserRoute.SingleListStatus), HttpPost]
        public async Task<List<AppUser_StatusDTO>> SingleListStatus([FromBody] AppUser_StatusFilterDTO AppUser_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = AppUser_StatusFilterDTO.Id;
            StatusFilter.Code = AppUser_StatusFilterDTO.Code;
            StatusFilter.Name = AppUser_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<AppUser_StatusDTO> AppUser_StatusDTOs = Statuses
                .Select(x => new AppUser_StatusDTO(x)).ToList();
            return AppUser_StatusDTOs;
        }
        [Route(AppUserRoute.SingleListRole), HttpPost]
        public async Task<List<AppUser_RoleDTO>> SingleListRole([FromBody] AppUser_RoleFilterDTO AppUser_RoleFilterDTO)
        {
            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = 0;
            RoleFilter.Take = 20;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = AppUser_RoleFilterDTO.Id;
            RoleFilter.Code = AppUser_RoleFilterDTO.Code;
            RoleFilter.Name = AppUser_RoleFilterDTO.Name;
            RoleFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<Role> Roles = await RoleService.List(RoleFilter);
            List<AppUser_RoleDTO> AppUser_RoleDTOs = Roles
                .Select(x => new AppUser_RoleDTO(x)).ToList();
            return AppUser_RoleDTOs;
        }

        [Route(AppUserRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<AppUser_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] AppUser_StoreGroupingFilterDTO AppUser_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = AppUser_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = AppUser_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = AppUser_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = AppUser_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<AppUser_StoreGroupingDTO> AppUser_StoreGroupingDTOs = StoreGroupings
                .Select(x => new AppUser_StoreGroupingDTO(x)).ToList();
            return AppUser_StoreGroupingDTOs;
        }
        [Route(AppUserRoute.SingleListStoreType), HttpPost]
        public async Task<List<AppUser_StoreTypeDTO>> SingleListStoreType([FromBody] AppUser_StoreTypeFilterDTO AppUser_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = AppUser_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = AppUser_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = AppUser_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<AppUser_StoreTypeDTO> AppUser_StoreTypeDTOs = StoreTypes
                .Select(x => new AppUser_StoreTypeDTO(x)).ToList();
            return AppUser_StoreTypeDTOs;
        }

        [Route(AppUserRoute.CountRole), HttpPost]
        public async Task<long> CountRole([FromBody] AppUser_RoleFilterDTO AppUser_RoleFilterDTO)
        {
            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Id = AppUser_RoleFilterDTO.Id;
            RoleFilter.Code = AppUser_RoleFilterDTO.Code;
            RoleFilter.Name = AppUser_RoleFilterDTO.Name;
            RoleFilter.StatusId = AppUser_RoleFilterDTO.StatusId;

            return await RoleService.Count(RoleFilter);
        }

        [Route(AppUserRoute.ListRole), HttpPost]
        public async Task<List<AppUser_RoleDTO>> ListRole([FromBody] AppUser_RoleFilterDTO AppUser_RoleFilterDTO)
        {
            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = AppUser_RoleFilterDTO.Skip;
            RoleFilter.Take = AppUser_RoleFilterDTO.Take;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = AppUser_RoleFilterDTO.Id;
            RoleFilter.Code = AppUser_RoleFilterDTO.Code;
            RoleFilter.Name = AppUser_RoleFilterDTO.Name;
            RoleFilter.StatusId = AppUser_RoleFilterDTO.StatusId;

            List<Role> Roles = await RoleService.List(RoleFilter);
            List<AppUser_RoleDTO> AppUser_RoleDTOs = Roles
                .Select(x => new AppUser_RoleDTO(x)).ToList();
            return AppUser_RoleDTOs;
        }

        [Route(AppUserRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] AppUser_StoreFilterDTO AppUser_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = AppUser_StoreFilterDTO.Id;
            StoreFilter.Code = AppUser_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = AppUser_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = AppUser_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = AppUser_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = AppUser_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = AppUser_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = AppUser_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = AppUser_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = AppUser_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = AppUser_StoreFilterDTO.WardId;
            StoreFilter.Address = AppUser_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = AppUser_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = AppUser_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = AppUser_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = AppUser_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = AppUser_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = AppUser_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            return await StoreService.Count(StoreFilter);
        }

        [Route(AppUserRoute.ListStore), HttpPost]
        public async Task<List<AppUser_StoreDTO>> ListStore([FromBody] AppUser_StoreFilterDTO AppUser_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = AppUser_StoreFilterDTO.Skip;
            StoreFilter.Take = AppUser_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = AppUser_StoreFilterDTO.Id;
            StoreFilter.Code = AppUser_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = AppUser_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = AppUser_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = AppUser_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = AppUser_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = AppUser_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = AppUser_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = AppUser_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = AppUser_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = AppUser_StoreFilterDTO.WardId;
            StoreFilter.Address = AppUser_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = AppUser_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = AppUser_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = AppUser_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = AppUser_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = AppUser_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = AppUser_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<AppUser_StoreDTO> AppUser_StoreDTOs = Stores
                .Select(x => new AppUser_StoreDTO(x)).ToList();
            return AppUser_StoreDTOs;
        }
    }
}

