using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public partial class ERouteController
    {
        [Route(ERouteRoute.FilterListAppUser), HttpPost]
        public async Task<List<ERoute_AppUserDTO>> FilterListAppUser([FromBody] ERoute_AppUserFilterDTO ERoute_AppUserFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ERoute_AppUserFilterDTO.Id;
            AppUserFilter.Username = ERoute_AppUserFilterDTO.Username;
            AppUserFilter.Password = ERoute_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = ERoute_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ERoute_AppUserFilterDTO.Address;
            AppUserFilter.Email = ERoute_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ERoute_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = ERoute_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ERoute_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = ERoute_AppUserFilterDTO.OrganizationId; //new IdFilter { Equal = CurrentUser.OrganizationId };
            AppUserFilter.SexId = ERoute_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = ERoute_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = ERoute_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = ERoute_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ERoute_AppUserDTO> ERoute_AppUserDTOs = AppUsers
                .Select(x => new ERoute_AppUserDTO(x)).ToList();
            return ERoute_AppUserDTOs;
        }
        [Route(ERouteRoute.FilterListStore), HttpPost]
        public async Task<List<ERoute_StoreDTO>> FilterListStore([FromBody] ERoute_StoreFilterDTO ERoute_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERoute_StoreFilterDTO.Id;
            StoreFilter.Code = ERoute_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERoute_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERoute_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERoute_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId };
            StoreFilter.StoreTypeId = ERoute_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ERoute_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ERoute_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERoute_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERoute_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERoute_StoreFilterDTO.WardId;
            StoreFilter.Address = ERoute_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERoute_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERoute_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERoute_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = ERoute_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = ERoute_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = ERoute_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERoute_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERoute_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = ERoute_StoreFilterDTO.StatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ERoute_StoreDTO> ERoute_StoreDTOs = Stores
                .Select(x => new ERoute_StoreDTO(x)).ToList();
            return ERoute_StoreDTOs;
        }
        [Route(ERouteRoute.FilterListERouteType), HttpPost]
        public async Task<List<ERoute_ERouteTypeDTO>> FilterListERouteType([FromBody] ERoute_ERouteTypeFilterDTO ERoute_ERouteTypeFilterDTO)
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter();
            ERouteTypeFilter.Skip = 0;
            ERouteTypeFilter.Take = 20;
            ERouteTypeFilter.OrderBy = ERouteTypeOrder.Id;
            ERouteTypeFilter.OrderType = OrderType.ASC;
            ERouteTypeFilter.Selects = ERouteTypeSelect.ALL;
            ERouteTypeFilter.Id = ERoute_ERouteTypeFilterDTO.Id;
            ERouteTypeFilter.Code = ERoute_ERouteTypeFilterDTO.Code;
            ERouteTypeFilter.Name = ERoute_ERouteTypeFilterDTO.Name;

            List<ERouteType> ERouteTypes = await ERouteTypeService.List(ERouteTypeFilter);
            List<ERoute_ERouteTypeDTO> ERoute_ERouteTypeDTOs = ERouteTypes
                .Select(x => new ERoute_ERouteTypeDTO(x)).ToList();
            return ERoute_ERouteTypeDTOs;
        }
        [Route(ERouteRoute.FilterListStatus), HttpPost]
        public async Task<List<ERoute_StatusDTO>> FilterListStatus([FromBody] ERoute_StatusFilterDTO ERoute_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<ERoute_StatusDTO> ERoute_StatusDTOs = Statuses
                .Select(x => new ERoute_StatusDTO(x)).ToList();
            return ERoute_StatusDTOs;
        }

        [Route(ERouteRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ERoute_StoreStatusDTO>> FilterListStoreStatus([FromBody] ERoute_StoreStatusFilterDTO ERoute_StoreStatusFilterDTO)
        {
            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = int.MaxValue;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ERoute_StoreStatusDTO> ERoute_StoreStatusDTOs = StoreStatuses
                .Select(s => new ERoute_StoreStatusDTO(s)).ToList();
            return ERoute_StoreStatusDTOs;
        }

        [Route(ERouteRoute.FilterListOrganization), HttpPost]
        public async Task<List<ERoute_OrganizationDTO>> FilterListOrganization([FromBody] ERoute_OrganizationFilterDTO ERoute_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = ERoute_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ERoute_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ERoute_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = ERoute_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = ERoute_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = ERoute_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = null;
            OrganizationFilter.Phone = ERoute_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = ERoute_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = ERoute_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ERoute_OrganizationDTO> ERoute_OrganizationDTOs = Organizations
                .Select(x => new ERoute_OrganizationDTO(x)).ToList();
            return ERoute_OrganizationDTOs;
        }

        [Route(ERouteRoute.FilterListRequestState), HttpPost]
        public async Task<List<ERoute_RequestStateDTO>> FilterListRequestState([FromBody] ERoute_RequestStateFilterDTO ERoute_RequestStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = ERoute_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = ERoute_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = ERoute_RequestStateFilterDTO.Name;

            List<RequestState> RequestStatees = await RequestStateService.List(RequestStateFilter);
            List<ERoute_RequestStateDTO> ERoute_RequestStateDTOs = RequestStatees
                .Select(x => new ERoute_RequestStateDTO(x)).ToList();
            return ERoute_RequestStateDTOs;
        }

        [Route(ERouteRoute.SingleListAppUser), HttpPost]
        public async Task<List<ERoute_AppUserDTO>> SingleListAppUser([FromBody] ERoute_AppUserFilterDTO ERoute_AppUserFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ERoute_AppUserFilterDTO.Id;
            AppUserFilter.Username = ERoute_AppUserFilterDTO.Username;
            AppUserFilter.Password = ERoute_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = ERoute_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ERoute_AppUserFilterDTO.Address;
            AppUserFilter.Email = ERoute_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ERoute_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = ERoute_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ERoute_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId };
            AppUserFilter.SexId = ERoute_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            AppUserFilter.Birthday = ERoute_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = ERoute_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ERoute_AppUserDTO> ERoute_AppUserDTOs = AppUsers
                .Select(x => new ERoute_AppUserDTO(x)).ToList();
            return ERoute_AppUserDTOs;
        }
        [Route(ERouteRoute.SingleListERouteType), HttpPost]
        public async Task<List<ERoute_ERouteTypeDTO>> SingleListERouteType([FromBody] ERoute_ERouteTypeFilterDTO ERoute_ERouteTypeFilterDTO)
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter();
            ERouteTypeFilter.Skip = 0;
            ERouteTypeFilter.Take = 20;
            ERouteTypeFilter.OrderBy = ERouteTypeOrder.Id;
            ERouteTypeFilter.OrderType = OrderType.ASC;
            ERouteTypeFilter.Selects = ERouteTypeSelect.ALL;
            ERouteTypeFilter.Id = ERoute_ERouteTypeFilterDTO.Id;
            ERouteTypeFilter.Code = ERoute_ERouteTypeFilterDTO.Code;
            ERouteTypeFilter.Name = ERoute_ERouteTypeFilterDTO.Name;

            List<ERouteType> ERouteTypes = await ERouteTypeService.List(ERouteTypeFilter);
            List<ERoute_ERouteTypeDTO> ERoute_ERouteTypeDTOs = ERouteTypes
                .Select(x => new ERoute_ERouteTypeDTO(x)).ToList();
            return ERoute_ERouteTypeDTOs;
        }
        [Route(ERouteRoute.SingleListOrganization), HttpPost]
        public async Task<List<ERoute_OrganizationDTO>> SingleListOrganization([FromBody] ERoute_OrganizationFilterDTO ERoute_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = ERoute_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ERoute_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ERoute_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = ERoute_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = ERoute_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = ERoute_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = ERoute_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = ERoute_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = ERoute_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ERoute_OrganizationDTO> ERoute_OrganizationDTOs = Organizations
                .Select(x => new ERoute_OrganizationDTO(x)).ToList();
            return ERoute_OrganizationDTOs;
        }
        [Route(ERouteRoute.SingleListRequestState), HttpPost]
        public async Task<List<ERoute_RequestStateDTO>> SingleListRequestState([FromBody] ERoute_RequestStateFilterDTO ERoute_RequestStateFilterDTO)
        {
            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = ERoute_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = ERoute_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = ERoute_RequestStateFilterDTO.Name;

            List<RequestState> RequestStates = await RequestStateService.List(RequestStateFilter);
            List<ERoute_RequestStateDTO> ERoute_RequestStateDTOs = RequestStates
                .Select(x => new ERoute_RequestStateDTO(x)).ToList();
            return ERoute_RequestStateDTOs;
        }
        [Route(ERouteRoute.SingleListStore), HttpPost]
        public async Task<List<ERoute_StoreDTO>> SingleListStore([FromBody] ERoute_StoreFilterDTO ERoute_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERoute_StoreFilterDTO.Id;
            StoreFilter.Code = ERoute_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERoute_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERoute_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERoute_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = new IdFilter { Equal = CurrentUser.OrganizationId };
            StoreFilter.StoreTypeId = ERoute_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ERoute_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ERoute_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERoute_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERoute_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERoute_StoreFilterDTO.WardId;
            StoreFilter.Address = ERoute_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERoute_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERoute_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERoute_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = ERoute_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = ERoute_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = ERoute_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERoute_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERoute_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ERoute_StoreDTO> ERoute_StoreDTOs = Stores
                .Select(x => new ERoute_StoreDTO(x)).ToList();
            return ERoute_StoreDTOs;

        }
        [Route(ERouteRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<ERoute_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] ERoute_StoreGroupingFilterDTO ERoute_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = ERoute_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = ERoute_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = ERoute_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = ERoute_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<ERoute_StoreGroupingDTO> ERoute_StoreGroupingDTOs = StoreGroupings
                .Select(x => new ERoute_StoreGroupingDTO(x)).ToList();
            return ERoute_StoreGroupingDTOs;
        }
        [Route(ERouteRoute.SingleListStoreType), HttpPost]
        public async Task<List<ERoute_StoreTypeDTO>> SingleListStoreType([FromBody] ERoute_StoreTypeFilterDTO ERoute_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ERoute_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ERoute_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ERoute_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<ERoute_StoreTypeDTO> ERoute_StoreTypeDTOs = StoreTypes
                .Select(x => new ERoute_StoreTypeDTO(x)).ToList();
            return ERoute_StoreTypeDTOs;
        }
        [Route(ERouteRoute.SingleListStatus), HttpPost]
        public async Task<List<ERoute_StatusDTO>> SingleListStatus([FromBody] ERoute_StatusFilterDTO ERoute_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<ERoute_StatusDTO> ERoute_StatusDTOs = Statuses
                .Select(x => new ERoute_StatusDTO(x)).ToList();
            return ERoute_StatusDTOs;
        }

        [Route(ERouteRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] ERoute_StoreFilterDTO ERoute_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = ERoute_StoreFilterDTO.Id;
            StoreFilter.Code = ERoute_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERoute_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERoute_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERoute_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERoute_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERoute_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = ERoute_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERoute_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERoute_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERoute_StoreFilterDTO.WardId;
            StoreFilter.Address = ERoute_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERoute_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERoute_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERoute_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ERoute_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERoute_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERoute_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = ERoute_StoreFilterDTO.SaleEmployeeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = ERoute_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            if (StoreFilter.Id.In != null)
            {
                var StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
                StoreFilter.Id.In = StoreFilter.Id.In.Intersect(StoreIds).ToList();
            }
            else
            {
                StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            }
            int count = await StoreService.Count(StoreFilter);
            return count;
        }

        [Route(ERouteRoute.ListStore), HttpPost]
        public async Task<List<ERoute_StoreDTO>> ListStore([FromBody] ERoute_StoreFilterDTO ERoute_StoreFilterDTO)
        {
            var CurrentUser = await AppUserService.Get(CurrentContext.UserId);
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = ERoute_StoreFilterDTO.Skip;
            StoreFilter.Take = ERoute_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERoute_StoreFilterDTO.Id;
            StoreFilter.Code = ERoute_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERoute_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERoute_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERoute_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERoute_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERoute_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = ERoute_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERoute_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERoute_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERoute_StoreFilterDTO.WardId;
            StoreFilter.Address = ERoute_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERoute_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERoute_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERoute_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ERoute_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERoute_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERoute_StoreFilterDTO.OwnerEmail;
            StoreFilter.AppUserId = ERoute_StoreFilterDTO.SaleEmployeeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = ERoute_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            if (StoreFilter.Id.In != null)
            {
                var StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
                StoreFilter.Id.In = StoreFilter.Id.In.Intersect(StoreIds).ToList();
            }
            else
            {
                StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            }

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ERoute_StoreDTO> ERoute_StoreDTOs = Stores
                .Select(x => new ERoute_StoreDTO(x)).ToList();
            return ERoute_StoreDTOs;
        }
    }
}
