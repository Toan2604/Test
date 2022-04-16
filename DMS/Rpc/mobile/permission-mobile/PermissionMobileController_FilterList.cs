using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public partial class PermissionMobileController
    {
        [Route(PermissionMobileRoute.CountAppUser), HttpPost]
        public async Task<int> CountAppUser([FromBody] PermissionMobile_AppUserFilterDTO PermissionMobile_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Id = PermissionMobile_AppUserFilterDTO.Id;
            AppUserFilter.Username = PermissionMobile_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = PermissionMobile_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = PermissionMobile_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            int count = await AppUserService.Count(AppUserFilter);
            return count;
        }

        [Route(PermissionMobileRoute.ListAppUser), HttpPost]
        public async Task<List<PermissionMobile_AppUserDTO>> ListAppUser([FromBody] PermissionMobile_AppUserFilterDTO PermissionMobile_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = PermissionMobile_AppUserFilterDTO.Skip;
            AppUserFilter.Take = PermissionMobile_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = PermissionMobile_AppUserFilterDTO.Id;
            AppUserFilter.Username = PermissionMobile_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = PermissionMobile_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = PermissionMobile_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<PermissionMobile_AppUserDTO> PermissionMobile_AppUserDTOs = AppUsers
                .Select(x => new PermissionMobile_AppUserDTO(x)).ToList();
            return PermissionMobile_AppUserDTOs;
        }

        [Route(PermissionMobileRoute.FilterListStore), HttpPost]
        public async Task<List<PermissionMobile_StoreDTO>> FilterListStore([FromBody] PermissionMobile_StoreFilterDTO PermissionMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = PermissionMobile_StoreFilterDTO.Skip;
            StoreFilter.Take = PermissionMobile_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;

            StoreFilter.AppUserId = PermissionMobile_StoreFilterDTO.AppUserId;
            StoreFilter.Name = PermissionMobile_StoreFilterDTO.Name;
            StoreFilter.Id = new IdFilter();

            if (StoreFilter.AppUserId != null && StoreFilter.AppUserId.Equal != null)
                StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<PermissionMobile_StoreDTO> PermissionMobile_StoreDTOs = Stores
                .Select(x => new PermissionMobile_StoreDTO(x)).ToList();
            return PermissionMobile_StoreDTOs;
        }

        [Route(PermissionMobileRoute.FilterCountStore), HttpPost]
        public async Task<int> FilterCountStore([FromBody] PermissionMobile_StoreFilterDTO PermissionMobile_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.AppUserId = PermissionMobile_StoreFilterDTO.AppUserId;
            StoreFilter.Name = PermissionMobile_StoreFilterDTO.Name;
            StoreFilter.Id = new IdFilter();

            if (StoreFilter.AppUserId != null && StoreFilter.AppUserId.Equal != null)
                StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            int count = await StoreService.Count(StoreFilter);
            return count;
        }

        [Route(PermissionMobileRoute.FilterListStoreType), HttpPost]
        public async Task<List<PermissionMobile_StoreTypeDTO>> FilterListStoreType([FromBody] PermissionMobile_StoreTypeFilterDTO PermissionMobile_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = PermissionMobile_StoreTypeFilterDTO.Skip;
            StoreTypeFilter.Take = PermissionMobile_StoreTypeFilterDTO.Take;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = PermissionMobile_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = PermissionMobile_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = PermissionMobile_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = PermissionMobile_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<PermissionMobile_StoreTypeDTO> PermissionMobile_StoreTypeDTOs = StoreTypes
                .Select(x => new PermissionMobile_StoreTypeDTO(x)).ToList();
            return PermissionMobile_StoreTypeDTOs;
        }

        [Route(PermissionMobileRoute.FilterCountStoreType), HttpPost]
        public async Task<int> FilterCountStoreType([FromBody] PermissionMobile_StoreTypeFilterDTO PermissionMobile_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Id = PermissionMobile_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = PermissionMobile_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = PermissionMobile_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = PermissionMobile_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            int count = await StoreTypeService.Count(StoreTypeFilter);
            return count;
        }

        [Route(PermissionMobileRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<PermissionMobile_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] PermissionMobile_StoreGroupingFilterDTO PermissionMobile_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = PermissionMobile_StoreGroupingFilterDTO.Skip;
            StoreGroupingFilter.Take = PermissionMobile_StoreGroupingFilterDTO.Take;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = PermissionMobile_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = PermissionMobile_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = PermissionMobile_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = PermissionMobile_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = PermissionMobile_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = PermissionMobile_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<PermissionMobile_StoreGroupingDTO> PermissionMobile_StoreGroupingDTOs = StoreGroupings
                .Select(x => new PermissionMobile_StoreGroupingDTO(x)).ToList();
            return PermissionMobile_StoreGroupingDTOs;
        }

        [Route(PermissionMobileRoute.FilterCountStoreGrouping), HttpPost]
        public async Task<int> FilterCountStoreGrouping([FromBody] PermissionMobile_StoreGroupingFilterDTO PermissionMobile_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Id = PermissionMobile_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = PermissionMobile_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = PermissionMobile_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = PermissionMobile_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = PermissionMobile_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = PermissionMobile_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            int count = await StoreGroupingService.Count(StoreGroupingFilter);
            return count;
        }

        [Route(PermissionMobileRoute.FilterListProvince), HttpPost]
        public async Task<List<PermissionMobile_ProvinceDTO>> FilterListProvince([FromBody] PermissionMobile_ProvinceFilterDTO PermissionMobile_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = PermissionMobile_ProvinceFilterDTO.Skip;
            ProvinceFilter.Take = PermissionMobile_ProvinceFilterDTO.Take;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = PermissionMobile_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = PermissionMobile_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = PermissionMobile_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<PermissionMobile_ProvinceDTO> PermissionMobile_ProvinceDTOs = Provinces
                .Select(x => new PermissionMobile_ProvinceDTO(x)).ToList();
            return PermissionMobile_ProvinceDTOs;
        }

        [Route(PermissionMobileRoute.FilterCountProvince), HttpPost]
        public async Task<int> FilterCountProvince([FromBody] PermissionMobile_ProvinceFilterDTO PermissionMobile_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Id = PermissionMobile_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = PermissionMobile_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = PermissionMobile_ProvinceFilterDTO.StatusId;

            int count = await ProvinceService.Count(ProvinceFilter);
            return count;
        }
    }
}
