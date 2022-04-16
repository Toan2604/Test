using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public partial class StoreController
    {
        [Route(StoreRoute.FilterListAppUser), HttpPost]
        public async Task<List<Store_AppUserDTO>> FilterListAppUser([FromBody] Store_AppUserFilterDTO Store_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Store_AppUserFilterDTO.Id;
            AppUserFilter.Username = Store_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Store_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Store_AppUserFilterDTO.Address;
            AppUserFilter.Email = Store_AppUserFilterDTO.Email;
            AppUserFilter.StatusId = new IdFilter { Equal = 1 };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Store_AppUserDTO> Store_AppUserDTOs = AppUsers
                .Select(x => new Store_AppUserDTO(x)).ToList();
            return Store_AppUserDTOs;
        }

        [Route(StoreRoute.FilterListDistrict), HttpPost]
        public async Task<List<Store_DistrictDTO>> FilterListDistrict([FromBody] Store_DistrictFilterDTO Store_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = Store_DistrictFilterDTO.Id;
            DistrictFilter.Name = Store_DistrictFilterDTO.Name;
            DistrictFilter.Priority = Store_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = Store_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = Store_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<Store_DistrictDTO> Store_DistrictDTOs = Districts
                .Select(x => new Store_DistrictDTO(x)).ToList();
            return Store_DistrictDTOs;
        }

        [Route(StoreRoute.FilterListOrganization), HttpPost]
        public async Task<List<Store_OrganizationDTO>> FilterListOrganization([FromBody] Store_OrganizationFilterDTO Store_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Store_OrganizationDTO> Store_OrganizationDTOs = Organizations
                .Select(x => new Store_OrganizationDTO(x)).ToList();
            return Store_OrganizationDTOs;
        }

        [Route(StoreRoute.FilterListProvince), HttpPost]
        public async Task<List<Store_ProvinceDTO>> FilterListProvince([FromBody] Store_ProvinceFilterDTO Store_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = Store_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = Store_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = Store_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<Store_ProvinceDTO> Store_ProvinceDTOs = Provinces
                .Select(x => new Store_ProvinceDTO(x)).ToList();
            return Store_ProvinceDTOs;
        }

        [Route(StoreRoute.FilterListParentStore), HttpPost]
        public async Task<List<Store_StoreDTO>> FilterListParentStore([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = Store_StoreFilterDTO.Id;
            StoreFilter.Code = Store_StoreFilterDTO.Code;
            StoreFilter.Name = Store_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = Store_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Store_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Store_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Store_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Store_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Store_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Store_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Store_StoreFilterDTO.WardId;
            StoreFilter.Address = Store_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = Store_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Store_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Store_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = Store_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Store_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Store_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = Store_StoreFilterDTO.StatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Store_StoreDTO> Store_StoreDTOs = Stores
                .Select(x => new Store_StoreDTO(x)).ToList();
            return Store_StoreDTOs;
        }

        [Route(StoreRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<Store_StoreStatusDTO>> FilterListStoreStatus([FromBody] Store_StoreStatusFilterDTO Store_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = Store_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = Store_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = Store_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<Store_StoreStatusDTO> Store_StoreStatusDTOs = StoreStatuses
                .Select(x => new Store_StoreStatusDTO(x)).ToList();
            return Store_StoreStatusDTOs;
        }

        [Route(StoreRoute.FilterListWard), HttpPost]
        public async Task<List<Store_WardDTO>> FilterListWard([FromBody] Store_WardFilterDTO Store_WardFilterDTO)
        {
            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = 0;
            WardFilter.Take = 20;
            WardFilter.OrderBy = WardOrder.Priority;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = Store_WardFilterDTO.Id;
            WardFilter.Name = Store_WardFilterDTO.Name;
            WardFilter.DistrictId = Store_WardFilterDTO.DistrictId;
            WardFilter.StatusId = Store_WardFilterDTO.StatusId;
            List<Ward> Wards = await WardService.List(WardFilter);
            List<Store_WardDTO> Store_WardDTOs = Wards
                .Select(x => new Store_WardDTO(x)).ToList();
            return Store_WardDTOs;
        }
        [Route(StoreRoute.FilterListStatus), HttpPost]
        public async Task<List<Store_StatusDTO>> FilterListStatus([FromBody] Store_StatusFilterDTO Store_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Store_StatusDTO> Store_StatusDTOs = Statuses
                .Select(x => new Store_StatusDTO(x)).ToList();
            return Store_StatusDTOs;
        }

        [Route(StoreRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<Store_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] Store_StoreGroupingFilterDTO Store_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = Store_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Store_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = Store_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = Store_StoreGroupingFilterDTO.Path;
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<Store_StoreGroupingDTO> Store_StoreGroupingDTOs = StoreGroupings
                .Select(x => new Store_StoreGroupingDTO(x)).ToList();
            return Store_StoreGroupingDTOs;
        }

        [Route(StoreRoute.FilterListStoreType), HttpPost]
        public async Task<List<Store_StoreTypeDTO>> FilterListStoreType([FromBody] Store_StoreTypeFilterDTO Store_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = Store_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Store_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Store_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = null;

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<Store_StoreTypeDTO> Store_StoreTypeDTOs = StoreTypes
                .Select(x => new Store_StoreTypeDTO(x)).ToList();
            return Store_StoreTypeDTOs;
        }

        [Route(StoreRoute.FilterListStoreUserStatus), HttpPost]
        public async Task<List<GenericEnum>> FilterListStoreUserStatus()
        {
            return StoreUserStatusEnum.StoreUserStatusEnumList;
        }

        [Route(StoreRoute.SingleListAppUser), HttpPost]
        public async Task<List<Store_AppUserDTO>> SingleListAppUser([FromBody] Store_AppUserFilterDTO Store_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Store_AppUserFilterDTO.Id;
            AppUserFilter.Username = Store_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Store_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Store_AppUserFilterDTO.Address;
            AppUserFilter.Email = Store_AppUserFilterDTO.Email;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Store_AppUserDTO> Store_AppUserDTOs = AppUsers
                .Select(x => new Store_AppUserDTO(x)).ToList();
            return Store_AppUserDTOs;
        }

        [Route(StoreRoute.SingleListOrganization), HttpPost]
        public async Task<List<Store_OrganizationDTO>> SingleListOrganization()
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;
            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Store_OrganizationDTO> Store_OrganizationDTOs = Organizations
                .Select(x => new Store_OrganizationDTO(x)).ToList();
            return Store_OrganizationDTOs;
        }
        [Route(StoreRoute.SingleListParentStore), HttpPost]
        public async Task<List<Store_StoreDTO>> SingleListParentStore([FromBody] Store_StoreFilterDTO Store_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = Store_StoreFilterDTO.Id;
            StoreFilter.Code = Store_StoreFilterDTO.Code;
            StoreFilter.Name = Store_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = Store_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = Store_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = Store_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = Store_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = Store_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = Store_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = Store_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = Store_StoreFilterDTO.WardId;
            StoreFilter.Address = Store_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = Store_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = Store_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = Store_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = Store_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = Store_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = Store_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<Store_StoreDTO> Store_StoreDTOs = Stores
                .Select(x => new Store_StoreDTO(x)).ToList();
            return Store_StoreDTOs;
        }
        [Route(StoreRoute.SingleListProvince), HttpPost]
        public async Task<List<Store_ProvinceDTO>> SingleListProvince([FromBody] Store_ProvinceFilterDTO Store_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = Store_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = Store_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<Store_ProvinceDTO> Store_ProvinceDTOs = Provinces
                .Select(x => new Store_ProvinceDTO(x)).ToList();
            return Store_ProvinceDTOs;
        }
        [Route(StoreRoute.SingleListDistrict), HttpPost]
        public async Task<List<Store_DistrictDTO>> SingleListDistrict([FromBody] Store_DistrictFilterDTO Store_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = Store_DistrictFilterDTO.Id;
            DistrictFilter.Name = Store_DistrictFilterDTO.Name;
            DistrictFilter.Priority = Store_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = Store_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<Store_DistrictDTO> Store_DistrictDTOs = Districts
                .Select(x => new Store_DistrictDTO(x)).ToList();
            return Store_DistrictDTOs;
        }

        [Route(StoreRoute.SingleListStoreStatus), HttpPost]
        public async Task<List<Store_StoreStatusDTO>> SingleListStoreStatus([FromBody] Store_StoreStatusFilterDTO Store_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = new IdFilter { NotEqual = StoreStatusEnum.ALL.Id };
            StoreStatusFilter.Code = Store_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = Store_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<Store_StoreStatusDTO> Store_StoreStatusDTOs = StoreStatuses
                .Select(x => new Store_StoreStatusDTO(x)).ToList();
            return Store_StoreStatusDTOs;
        }

        [Route(StoreRoute.SingleListEstimatedRevenue), HttpPost]
        public async Task<List<Store_EstimatedRevenueDTO>> SingleListEstimatedRevenue([FromBody] Store_EstimatedRevenueFilterDTO Store_EstimatedRevenueFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            EstimatedRevenueFilter EstimatedRevenueFilter = new EstimatedRevenueFilter();
            EstimatedRevenueFilter.Skip = 0;
            EstimatedRevenueFilter.Take = 20;
            EstimatedRevenueFilter.OrderBy = EstimatedRevenueOrder.Id;
            EstimatedRevenueFilter.OrderType = OrderType.ASC;
            EstimatedRevenueFilter.Selects = EstimatedRevenueSelect.ALL;
            EstimatedRevenueFilter.Id = Store_EstimatedRevenueFilterDTO.Id;
            EstimatedRevenueFilter.Code = Store_EstimatedRevenueFilterDTO.Code;
            EstimatedRevenueFilter.Name = Store_EstimatedRevenueFilterDTO.Name;

            List<EstimatedRevenue> EstimatedRevenuees = await EstimatedRevenueService.List(EstimatedRevenueFilter);
            List<Store_EstimatedRevenueDTO> Store_EstimatedRevenueDTOs = EstimatedRevenuees
                .Select(x => new Store_EstimatedRevenueDTO(x)).ToList();
            return Store_EstimatedRevenueDTOs;
        }

        [Route(StoreRoute.SingleListWard), HttpPost]
        public async Task<List<Store_WardDTO>> SingleListWard([FromBody] Store_WardFilterDTO Store_WardFilterDTO)
        {
            WardFilter WardFilter = new WardFilter();
            WardFilter.Skip = 0;
            WardFilter.Take = 20;
            WardFilter.OrderBy = WardOrder.Priority;
            WardFilter.OrderType = OrderType.ASC;
            WardFilter.Selects = WardSelect.ALL;
            WardFilter.Id = Store_WardFilterDTO.Id;
            WardFilter.Name = Store_WardFilterDTO.Name;
            WardFilter.DistrictId = Store_WardFilterDTO.DistrictId;
            WardFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            List<Ward> Wards = await WardService.List(WardFilter);
            List<Store_WardDTO> Store_WardDTOs = Wards
                .Select(x => new Store_WardDTO(x)).ToList();
            return Store_WardDTOs;
        }
        [Route(StoreRoute.SingleListStatus), HttpPost]
        public async Task<List<Store_StatusDTO>> SingleListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Store_StatusDTO> Store_StatusDTOs = Statuses
                .Select(x => new Store_StatusDTO(x)).ToList();
            return Store_StatusDTOs;
        }
        [Route(StoreRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<Store_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] Store_StoreGroupingFilterDTO Store_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = Store_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = Store_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = Store_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = Store_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<Store_StoreGroupingDTO> Store_StoreGroupingDTOs = StoreGroupings
                .Select(x => new Store_StoreGroupingDTO(x)).ToList();
            return Store_StoreGroupingDTOs;
        }
        [Route(StoreRoute.SingleListStoreType), HttpPost]
        public async Task<List<Store_StoreTypeDTO>> SingleListStoreType([FromBody] Store_StoreTypeFilterDTO Store_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = Store_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = Store_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = Store_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<Store_StoreTypeDTO> Store_StoreTypeDTOs = StoreTypes
                .Select(x => new Store_StoreTypeDTO(x)).ToList();
            return Store_StoreTypeDTOs;
        }

        [Route(StoreRoute.SingleListBrand), HttpPost]
        public async Task<List<Store_BrandDTO>> SingleListBrand([FromBody] Store_BrandFilterDTO Store_BrandFilterDTO)
        {
            BrandFilter BrandFilter = new BrandFilter();
            BrandFilter.Skip = 0;
            BrandFilter.Take = 20;
            BrandFilter.OrderBy = BrandOrder.Id;
            BrandFilter.OrderType = OrderType.ASC;
            BrandFilter.Selects = BrandSelect.ALL;
            BrandFilter.Id = Store_BrandFilterDTO.Id;
            BrandFilter.Code = Store_BrandFilterDTO.Code;
            BrandFilter.Name = Store_BrandFilterDTO.Name;
            BrandFilter.Description = Store_BrandFilterDTO.Description;
            BrandFilter.UpdateTime = Store_BrandFilterDTO.UpdateTime;

            List<Brand> Brands = await BrandService.List(BrandFilter);
            List<Store_BrandDTO> Store_BrandDTOs = Brands
                .Select(x => new Store_BrandDTO(x)).ToList();
            return Store_BrandDTOs;
        }

        [Route(StoreRoute.CountProductGrouping), HttpPost]
        public async Task<long> CountProductGrouping([FromBody] Store_ProductGroupingFilterDTO Store_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Id = Store_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = Store_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = Store_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.ParentId = Store_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = Store_ProductGroupingFilterDTO.Path;
            ProductGroupingFilter.Description = Store_ProductGroupingFilterDTO.Description;
            return await ProductGroupingService.Count(ProductGroupingFilter);
        }

        [Route(StoreRoute.ListProductGrouping), HttpPost]
        public async Task<List<Store_ProductGroupingDTO>> ListProductGrouping([FromBody] Store_ProductGroupingFilterDTO Store_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = Store_ProductGroupingFilterDTO.Skip;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.ALL;
            ProductGroupingFilter.Id = Store_ProductGroupingFilterDTO.Id;
            ProductGroupingFilter.Code = Store_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = Store_ProductGroupingFilterDTO.Name;
            ProductGroupingFilter.ParentId = Store_ProductGroupingFilterDTO.ParentId;
            ProductGroupingFilter.Path = Store_ProductGroupingFilterDTO.Path;
            ProductGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<Store_ProductGroupingDTO> Store_ProductGroupingDTOs = ProductGroupings
                .Select(x => new Store_ProductGroupingDTO(x)).ToList();
            return Store_ProductGroupingDTOs;
        }
        [Route(StoreRoute.CountShowingCategory), HttpPost]
        public async Task<long> CountShowingCategory([FromBody] Store_ShowingCategoryFilterDTO Store_ShowingCategoryFilterDTO)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Id = Store_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = Store_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = Store_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = Store_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = Store_ShowingCategoryFilterDTO.Path;
            return await ShowingCategoryService.Count(ShowingCategoryFilter);
        }

        [Route(StoreRoute.ListShowingCategory), HttpPost]
        public async Task<List<Store_ShowingCategoryDTO>> ListShowingCategory([FromBody] Store_ShowingCategoryFilterDTO Store_ShowingCategoryFilterDTO)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter();
            ShowingCategoryFilter.Skip = Store_ShowingCategoryFilterDTO.Skip;
            ShowingCategoryFilter.Take = int.MaxValue;
            ShowingCategoryFilter.OrderBy = ShowingCategoryOrder.Id;
            ShowingCategoryFilter.OrderType = OrderType.ASC;
            ShowingCategoryFilter.Selects = ShowingCategorySelect.ALL;
            ShowingCategoryFilter.Id = Store_ShowingCategoryFilterDTO.Id;
            ShowingCategoryFilter.Code = Store_ShowingCategoryFilterDTO.Code;
            ShowingCategoryFilter.Name = Store_ShowingCategoryFilterDTO.Name;
            ShowingCategoryFilter.ParentId = Store_ShowingCategoryFilterDTO.ParentId;
            ShowingCategoryFilter.Path = Store_ShowingCategoryFilterDTO.Path;

            List<ShowingCategory> ShowingCategorys = await ShowingCategoryService.List(ShowingCategoryFilter);
            List<Store_ShowingCategoryDTO> Store_ShowingCategoryDTOs = ShowingCategorys
                .Select(x => new Store_ShowingCategoryDTO(x)).ToList();
            return Store_ShowingCategoryDTOs;
        }


    }
}
