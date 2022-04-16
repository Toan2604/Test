using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MAppUser;
using DMS.Services.MERoute;
using DMS.Services.MERouteChangeRequest;
using DMS.Services.MERouteType;
using DMS.Services.MOrganization;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.e_route_change_request
{
    public class ERouteChangeRequestController : RpcController
    {
        private IAppUserService AppUserService;
        private IERouteService ERouteService;
        private IERouteTypeService ERouteTypeService;
        private IOrganizationService OrganizationService;
        private IRequestStateService RequestStateService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IERouteChangeRequestService ERouteChangeRequestService;
        private ICurrentContext CurrentContext;
        public ERouteChangeRequestController(
            IAppUserService AppUserService,
            IERouteService ERouteService,
            IERouteTypeService ERouteTypeService,
            IOrganizationService OrganizationService,
            IRequestStateService RequestStateService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IERouteChangeRequestService ERouteChangeRequestService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.ERouteService = ERouteService;
            this.ERouteTypeService = ERouteTypeService;
            this.OrganizationService = OrganizationService;
            this.RequestStateService = RequestStateService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.ERouteChangeRequestService = ERouteChangeRequestService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ERouteChangeRequestRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] ERouteChangeRequest_ERouteChangeRequestFilterDTO ERouteChangeRequest_ERouteChangeRequestFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteChangeRequestFilter ERouteChangeRequestFilter = ConvertFilterDTOToFilterEntity(ERouteChangeRequest_ERouteChangeRequestFilterDTO);
            ERouteChangeRequestFilter = ERouteChangeRequestService.ToFilter(ERouteChangeRequestFilter);
            int count = await ERouteChangeRequestService.Count(ERouteChangeRequestFilter);
            return count;
        }

        [Route(ERouteChangeRequestRoute.List), HttpPost]
        public async Task<ActionResult<List<ERouteChangeRequest_ERouteChangeRequestDTO>>> List([FromBody] ERouteChangeRequest_ERouteChangeRequestFilterDTO ERouteChangeRequest_ERouteChangeRequestFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteChangeRequestFilter ERouteChangeRequestFilter = ConvertFilterDTOToFilterEntity(ERouteChangeRequest_ERouteChangeRequestFilterDTO);
            ERouteChangeRequestFilter = ERouteChangeRequestService.ToFilter(ERouteChangeRequestFilter);
            List<ERouteChangeRequest> ERouteChangeRequests = await ERouteChangeRequestService.List(ERouteChangeRequestFilter);
            List<ERouteChangeRequest_ERouteChangeRequestDTO> ERouteChangeRequest_ERouteChangeRequestDTOs = ERouteChangeRequests
                .Select(c => new ERouteChangeRequest_ERouteChangeRequestDTO(c)).ToList();
            return ERouteChangeRequest_ERouteChangeRequestDTOs;
        }

        [Route(ERouteChangeRequestRoute.Get), HttpPost]
        public async Task<ActionResult<ERouteChangeRequest_ERouteChangeRequestDTO>> Get([FromBody] ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERouteChangeRequest_ERouteChangeRequestDTO.Id))
                return Forbid();

            ERouteChangeRequest ERouteChangeRequest = await ERouteChangeRequestService.Get(ERouteChangeRequest_ERouteChangeRequestDTO.Id);
            return new ERouteChangeRequest_ERouteChangeRequestDTO(ERouteChangeRequest);
        }
        [Route(ERouteChangeRequestRoute.GetDraft), HttpPost]
        public async Task<ActionResult<ERouteChangeRequest_ERouteChangeRequestDTO>> GetDraft([FromBody] ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERoute ERoute = await ERouteService.Get(ERouteChangeRequest_ERouteChangeRequestDTO.ERouteId);
            if (ERoute == null)
                return null;
            ERouteChangeRequest_ERouteChangeRequestDTO = new ERouteChangeRequest_ERouteChangeRequestDTO
            {
                ERouteId = ERoute.Id,
                CreatorId = CurrentContext.UserId,
                RequestStateId = RequestStateEnum.NEW.Id,
                ERoute = new ERouteChangeRequest_ERouteDTO(ERoute),
                ERouteChangeRequestContents = ERoute.ERouteContents?.Select(x => new ERouteChangeRequest_ERouteChangeRequestContentDTO
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    OrderNumber = x.OrderNumber,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday,
                    Saturday = x.Saturday,
                    Sunday = x.Sunday,
                    Week1 = x.Week1,
                    Week2 = x.Week2,
                    Week3 = x.Week3,
                    Week4 = x.Week4,
                    Store = x.Store == null ? null : new ERouteChangeRequest_StoreDTO
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        StatusId = x.Store.StatusId,
                    },
                }).ToList()
            };
            return ERouteChangeRequest_ERouteChangeRequestDTO;
        }

        [Route(ERouteChangeRequestRoute.Create), HttpPost]
        public async Task<ActionResult<ERouteChangeRequest_ERouteChangeRequestDTO>> Create([FromBody] ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERouteChangeRequest_ERouteChangeRequestDTO.Id))
                return Forbid();

            ERouteChangeRequest ERouteChangeRequest = ConvertDTOToEntity(ERouteChangeRequest_ERouteChangeRequestDTO);
            ERouteChangeRequest = await ERouteChangeRequestService.Create(ERouteChangeRequest);
            ERouteChangeRequest_ERouteChangeRequestDTO = new ERouteChangeRequest_ERouteChangeRequestDTO(ERouteChangeRequest);
            if (ERouteChangeRequest.IsValidated)
                return ERouteChangeRequest_ERouteChangeRequestDTO;
            else
                return BadRequest(ERouteChangeRequest_ERouteChangeRequestDTO);
        }

        [Route(ERouteChangeRequestRoute.Update), HttpPost]
        public async Task<ActionResult<ERouteChangeRequest_ERouteChangeRequestDTO>> Update([FromBody] ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERouteChangeRequest_ERouteChangeRequestDTO.Id))
                return Forbid();

            ERouteChangeRequest ERouteChangeRequest = ConvertDTOToEntity(ERouteChangeRequest_ERouteChangeRequestDTO);
            ERouteChangeRequest = await ERouteChangeRequestService.Update(ERouteChangeRequest);
            ERouteChangeRequest_ERouteChangeRequestDTO = new ERouteChangeRequest_ERouteChangeRequestDTO(ERouteChangeRequest);
            if (ERouteChangeRequest.IsValidated)
                return ERouteChangeRequest_ERouteChangeRequestDTO;
            else
                return BadRequest(ERouteChangeRequest_ERouteChangeRequestDTO);
        }

        [Route(ERouteChangeRequestRoute.Delete), HttpPost]
        public async Task<ActionResult<ERouteChangeRequest_ERouteChangeRequestDTO>> Delete([FromBody] ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(ERouteChangeRequest_ERouteChangeRequestDTO.Id))
                return Forbid();

            ERouteChangeRequest ERouteChangeRequest = ConvertDTOToEntity(ERouteChangeRequest_ERouteChangeRequestDTO);
            ERouteChangeRequest = await ERouteChangeRequestService.Delete(ERouteChangeRequest);
            ERouteChangeRequest_ERouteChangeRequestDTO = new ERouteChangeRequest_ERouteChangeRequestDTO(ERouteChangeRequest);
            if (ERouteChangeRequest.IsValidated)
                return ERouteChangeRequest_ERouteChangeRequestDTO;
            else
                return BadRequest(ERouteChangeRequest_ERouteChangeRequestDTO);
        }

        [Route(ERouteChangeRequestRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ERouteChangeRequestFilter ERouteChangeRequestFilter = new ERouteChangeRequestFilter();
            ERouteChangeRequestFilter = ERouteChangeRequestService.ToFilter(ERouteChangeRequestFilter);
            ERouteChangeRequestFilter.Id = new IdFilter { In = Ids };
            ERouteChangeRequestFilter.Selects = ERouteChangeRequestSelect.Id | ERouteChangeRequestSelect.RequestState;
            ERouteChangeRequestFilter.Skip = 0;
            ERouteChangeRequestFilter.Take = int.MaxValue;

            List<ERouteChangeRequest> ERouteChangeRequests = await ERouteChangeRequestService.List(ERouteChangeRequestFilter);
            ERouteChangeRequests = await ERouteChangeRequestService.BulkDelete(ERouteChangeRequests);
            if (ERouteChangeRequests.Any(x => !x.IsValidated))
                return BadRequest(ERouteChangeRequests.Where(x => !x.IsValidated));
            return true;
        }

        private async Task<bool> HasPermission(long Id)
        {
            ERouteChangeRequestFilter ERouteChangeRequestFilter = new ERouteChangeRequestFilter();
            ERouteChangeRequestFilter = ERouteChangeRequestService.ToFilter(ERouteChangeRequestFilter);
            if (Id == 0)
            {

            }
            else
            {
                ERouteChangeRequestFilter.Id = new IdFilter { Equal = Id };
                int count = await ERouteChangeRequestService.Count(ERouteChangeRequestFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private ERouteChangeRequest ConvertDTOToEntity(ERouteChangeRequest_ERouteChangeRequestDTO ERouteChangeRequest_ERouteChangeRequestDTO)
        {
            ERouteChangeRequest_ERouteChangeRequestDTO.TrimString();
            ERouteChangeRequest ERouteChangeRequest = new ERouteChangeRequest();
            ERouteChangeRequest.Id = ERouteChangeRequest_ERouteChangeRequestDTO.Id;
            ERouteChangeRequest.ERouteId = ERouteChangeRequest_ERouteChangeRequestDTO.ERouteId;
            ERouteChangeRequest.CreatorId = ERouteChangeRequest_ERouteChangeRequestDTO.CreatorId;
            ERouteChangeRequest.RequestStateId = ERouteChangeRequest_ERouteChangeRequestDTO.RequestStateId;
            ERouteChangeRequest.Creator = ERouteChangeRequest_ERouteChangeRequestDTO.Creator == null ? null : new AppUser
            {
                Id = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Id,
                Username = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Username,
                DisplayName = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.DisplayName,
                Address = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Address,
                Email = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Email,
                Phone = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Phone,
                PositionId = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.PositionId,
                Department = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Department,
                OrganizationId = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.OrganizationId,
                SexId = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.SexId,
                StatusId = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.StatusId,
                Avatar = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Avatar,
                Birthday = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.Birthday,
                ProvinceId = ERouteChangeRequest_ERouteChangeRequestDTO.Creator.ProvinceId,
            };
            ERouteChangeRequest.ERoute = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute == null ? null : new ERoute
            {
                Id = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.Id,
                Code = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.Code,
                Name = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.Name,
                SaleEmployeeId = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.SaleEmployeeId,
                StartDate = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.StartDate,
                EndDate = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.EndDate,
                ERouteTypeId = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.ERouteTypeId,
                RequestStateId = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.RequestStateId,
                StatusId = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.StatusId,
                CreatorId = ERouteChangeRequest_ERouteChangeRequestDTO.ERoute.CreatorId,
            };
            ERouteChangeRequest.RequestState = ERouteChangeRequest_ERouteChangeRequestDTO.RequestState == null ? null : new RequestState
            {
                Id = ERouteChangeRequest_ERouteChangeRequestDTO.RequestState.Id,
                Code = ERouteChangeRequest_ERouteChangeRequestDTO.RequestState.Code,
                Name = ERouteChangeRequest_ERouteChangeRequestDTO.RequestState.Name,
            };
            ERouteChangeRequest.ERouteChangeRequestContents = ERouteChangeRequest_ERouteChangeRequestDTO.ERouteChangeRequestContents?
                .Select(x => new ERouteChangeRequestContent
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    OrderNumber = x.OrderNumber,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday,
                    Saturday = x.Saturday,
                    Sunday = x.Sunday,
                    Week1 = x.Week1,
                    Week2 = x.Week2,
                    Week3 = x.Week3,
                    Week4 = x.Week4,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        StatusId = x.Store.StatusId,
                    },
                }).ToList();
            ERouteChangeRequest.BaseLanguage = CurrentContext.Language;
            return ERouteChangeRequest;
        }

        private ERouteChangeRequestFilter ConvertFilterDTOToFilterEntity(ERouteChangeRequest_ERouteChangeRequestFilterDTO ERouteChangeRequest_ERouteChangeRequestFilterDTO)
        {
            ERouteChangeRequestFilter ERouteChangeRequestFilter = new ERouteChangeRequestFilter();
            ERouteChangeRequestFilter.Selects = ERouteChangeRequestSelect.ALL;
            ERouteChangeRequestFilter.Skip = ERouteChangeRequest_ERouteChangeRequestFilterDTO.Skip;
            ERouteChangeRequestFilter.Take = ERouteChangeRequest_ERouteChangeRequestFilterDTO.Take;
            ERouteChangeRequestFilter.OrderBy = ERouteChangeRequest_ERouteChangeRequestFilterDTO.OrderBy;
            ERouteChangeRequestFilter.OrderType = ERouteChangeRequest_ERouteChangeRequestFilterDTO.OrderType;

            ERouteChangeRequestFilter.Id = ERouteChangeRequest_ERouteChangeRequestFilterDTO.Id;
            ERouteChangeRequestFilter.Code = ERouteChangeRequest_ERouteChangeRequestFilterDTO.Code;
            ERouteChangeRequestFilter.Name = ERouteChangeRequest_ERouteChangeRequestFilterDTO.Name;
            ERouteChangeRequestFilter.ERouteTypeId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.ERouteTypeId;
            ERouteChangeRequestFilter.StartDate = ERouteChangeRequest_ERouteChangeRequestFilterDTO.StartDate;
            ERouteChangeRequestFilter.EndDate = ERouteChangeRequest_ERouteChangeRequestFilterDTO.EndDate;
            ERouteChangeRequestFilter.SaleEmployeeId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.SaleEmployeeId;
            ERouteChangeRequestFilter.EndDate = ERouteChangeRequest_ERouteChangeRequestFilterDTO.EndDate;
            ERouteChangeRequestFilter.ERouteId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.ERouteId;
            ERouteChangeRequestFilter.CreatorId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.CreatorId;
            ERouteChangeRequestFilter.StoreId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.StoreId;
            ERouteChangeRequestFilter.RequestStateId = ERouteChangeRequest_ERouteChangeRequestFilterDTO.RequestStateId;
            ERouteChangeRequestFilter.CreatedAt = ERouteChangeRequest_ERouteChangeRequestFilterDTO.CreatedAt;
            ERouteChangeRequestFilter.UpdatedAt = ERouteChangeRequest_ERouteChangeRequestFilterDTO.UpdatedAt;
            return ERouteChangeRequestFilter;
        }

        [Route(ERouteChangeRequestRoute.FilterListAppUser), HttpPost]
        public async Task<List<ERouteChangeRequest_AppUserDTO>> FilterListAppUser([FromBody] ERouteChangeRequest_AppUserFilterDTO ERouteChangeRequest_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ERouteChangeRequest_AppUserFilterDTO.Id;
            AppUserFilter.Username = ERouteChangeRequest_AppUserFilterDTO.Username;
            AppUserFilter.Password = ERouteChangeRequest_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = ERouteChangeRequest_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ERouteChangeRequest_AppUserFilterDTO.Address;
            AppUserFilter.Email = ERouteChangeRequest_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ERouteChangeRequest_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = ERouteChangeRequest_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ERouteChangeRequest_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = ERouteChangeRequest_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = ERouteChangeRequest_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = ERouteChangeRequest_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = ERouteChangeRequest_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = ERouteChangeRequest_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ERouteChangeRequest_AppUserDTO> ERouteChangeRequest_AppUserDTOs = AppUsers
                .Select(x => new ERouteChangeRequest_AppUserDTO(x)).ToList();
            return ERouteChangeRequest_AppUserDTOs;
        }
        [Route(ERouteChangeRequestRoute.FilterListERoute), HttpPost]
        public async Task<List<ERouteChangeRequest_ERouteDTO>> FilterListERoute([FromBody] ERouteChangeRequest_ERouteFilterDTO ERouteChangeRequest_ERouteFilterDTO)
        {
            ERouteFilter ERouteFilter = new ERouteFilter();
            ERouteFilter.Skip = 0;
            ERouteFilter.Take = 20;
            ERouteFilter.OrderBy = ERouteOrder.Id;
            ERouteFilter.OrderType = OrderType.ASC;
            ERouteFilter.Selects = ERouteSelect.ALL;
            ERouteFilter.Id = ERouteChangeRequest_ERouteFilterDTO.Id;
            ERouteFilter.Code = ERouteChangeRequest_ERouteFilterDTO.Code;
            ERouteFilter.Name = ERouteChangeRequest_ERouteFilterDTO.Name;
            ERouteFilter.AppUserId = ERouteChangeRequest_ERouteFilterDTO.SaleEmployeeId;
            ERouteFilter.StartDate = ERouteChangeRequest_ERouteFilterDTO.StartDate;
            ERouteFilter.EndDate = ERouteChangeRequest_ERouteFilterDTO.EndDate;
            ERouteFilter.ERouteTypeId = ERouteChangeRequest_ERouteFilterDTO.ERouteTypeId;
            ERouteFilter.RequestStateId = ERouteChangeRequest_ERouteFilterDTO.RequestStateId;
            ERouteFilter.StatusId = ERouteChangeRequest_ERouteFilterDTO.StatusId;
            ERouteFilter.CreatorId = ERouteChangeRequest_ERouteFilterDTO.CreatorId;

            List<ERoute> ERoutes = await ERouteService.List(ERouteFilter);
            List<ERouteChangeRequest_ERouteDTO> ERouteChangeRequest_ERouteDTOs = ERoutes
                .Select(x => new ERouteChangeRequest_ERouteDTO(x)).ToList();
            return ERouteChangeRequest_ERouteDTOs;
        }
        [Route(ERouteChangeRequestRoute.FilterListStore), HttpPost]
        public async Task<List<ERouteChangeRequest_StoreDTO>> FilterListStore([FromBody] ERouteChangeRequest_StoreFilterDTO ERouteChangeRequest_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERouteChangeRequest_StoreFilterDTO.Id;
            StoreFilter.Code = ERouteChangeRequest_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERouteChangeRequest_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERouteChangeRequest_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERouteChangeRequest_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERouteChangeRequest_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERouteChangeRequest_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ERouteChangeRequest_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ERouteChangeRequest_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERouteChangeRequest_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERouteChangeRequest_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERouteChangeRequest_StoreFilterDTO.WardId;
            StoreFilter.Address = ERouteChangeRequest_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERouteChangeRequest_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERouteChangeRequest_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERouteChangeRequest_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = ERouteChangeRequest_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = ERouteChangeRequest_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = ERouteChangeRequest_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERouteChangeRequest_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERouteChangeRequest_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = ERouteChangeRequest_StoreFilterDTO.StatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ERouteChangeRequest_StoreDTO> ERouteChangeRequest_StoreDTOs = Stores
                .Select(x => new ERouteChangeRequest_StoreDTO(x)).ToList();
            return ERouteChangeRequest_StoreDTOs;
        }
        [Route(ERouteChangeRequestRoute.FilterListERouteType), HttpPost]
        public async Task<List<ERouteChangeRequest_ERouteTypeDTO>> FilterListERouteType([FromBody] ERouteChangeRequest_ERouteTypeFilterDTO ERouteChangeRequest_ERouteTypeFilterDTO)
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter();
            ERouteTypeFilter.Skip = 0;
            ERouteTypeFilter.Take = 20;
            ERouteTypeFilter.OrderBy = ERouteTypeOrder.Id;
            ERouteTypeFilter.OrderType = OrderType.ASC;
            ERouteTypeFilter.Selects = ERouteTypeSelect.ALL;
            ERouteTypeFilter.Id = ERouteChangeRequest_ERouteTypeFilterDTO.Id;
            ERouteTypeFilter.Code = ERouteChangeRequest_ERouteTypeFilterDTO.Code;
            ERouteTypeFilter.Name = ERouteChangeRequest_ERouteTypeFilterDTO.Name;

            List<ERouteType> ERouteTypes = await ERouteTypeService.List(ERouteTypeFilter);
            List<ERouteChangeRequest_ERouteTypeDTO> ERouteChangeRequest_ERouteTypeDTOs = ERouteTypes
                .Select(x => new ERouteChangeRequest_ERouteTypeDTO(x)).ToList();
            return ERouteChangeRequest_ERouteTypeDTOs;
        }


        [Route(ERouteChangeRequestRoute.SingleListAppUser), HttpPost]
        public async Task<List<ERouteChangeRequest_AppUserDTO>> SingleListAppUser([FromBody] ERouteChangeRequest_AppUserFilterDTO ERouteChangeRequest_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ERouteChangeRequest_AppUserFilterDTO.Id;
            AppUserFilter.Username = ERouteChangeRequest_AppUserFilterDTO.Username;
            AppUserFilter.Password = ERouteChangeRequest_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = ERouteChangeRequest_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ERouteChangeRequest_AppUserFilterDTO.Address;
            AppUserFilter.Email = ERouteChangeRequest_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ERouteChangeRequest_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = ERouteChangeRequest_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ERouteChangeRequest_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = ERouteChangeRequest_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = ERouteChangeRequest_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            AppUserFilter.Birthday = ERouteChangeRequest_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = ERouteChangeRequest_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ERouteChangeRequest_AppUserDTO> ERouteChangeRequest_AppUserDTOs = AppUsers
                .Select(x => new ERouteChangeRequest_AppUserDTO(x)).ToList();
            return ERouteChangeRequest_AppUserDTOs;
        }
        [Route(ERouteChangeRequestRoute.SingleListERoute), HttpPost]
        public async Task<List<ERouteChangeRequest_ERouteDTO>> SingleListERoute([FromBody] ERouteChangeRequest_ERouteFilterDTO ERouteChangeRequest_ERouteFilterDTO)
        {
            ERouteFilter ERouteFilter = new ERouteFilter();
            ERouteFilter.Skip = 0;
            ERouteFilter.Take = 20;
            ERouteFilter.OrderBy = ERouteOrder.Id;
            ERouteFilter.OrderType = OrderType.ASC;
            ERouteFilter.Selects = ERouteSelect.ALL;
            ERouteFilter.Id = ERouteChangeRequest_ERouteFilterDTO.Id;
            ERouteFilter.Code = ERouteChangeRequest_ERouteFilterDTO.Code;
            ERouteFilter.Name = ERouteChangeRequest_ERouteFilterDTO.Name;
            ERouteFilter.AppUserId = ERouteChangeRequest_ERouteFilterDTO.SaleEmployeeId;
            ERouteFilter.StartDate = ERouteChangeRequest_ERouteFilterDTO.StartDate;
            ERouteFilter.EndDate = ERouteChangeRequest_ERouteFilterDTO.EndDate;
            ERouteFilter.ERouteTypeId = ERouteChangeRequest_ERouteFilterDTO.ERouteTypeId;
            ERouteFilter.RequestStateId = ERouteChangeRequest_ERouteFilterDTO.RequestStateId;
            ERouteFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            ERouteFilter.CreatorId = ERouteChangeRequest_ERouteFilterDTO.CreatorId;

            List<ERoute> ERoutes = await ERouteService.List(ERouteFilter);
            List<ERouteChangeRequest_ERouteDTO> ERouteChangeRequest_ERouteDTOs = ERoutes
                .Select(x => new ERouteChangeRequest_ERouteDTO(x)).ToList();
            return ERouteChangeRequest_ERouteDTOs;
        }
        [Route(ERouteChangeRequestRoute.SingleListERouteType), HttpPost]
        public async Task<List<ERouteChangeRequest_ERouteTypeDTO>> SingleListERouteType([FromBody] ERouteChangeRequest_ERouteTypeFilterDTO ERouteChangeRequest_ERouteTypeFilterDTO)
        {
            ERouteTypeFilter ERouteTypeFilter = new ERouteTypeFilter();
            ERouteTypeFilter.Skip = 0;
            ERouteTypeFilter.Take = 20;
            ERouteTypeFilter.OrderBy = ERouteTypeOrder.Id;
            ERouteTypeFilter.OrderType = OrderType.ASC;
            ERouteTypeFilter.Selects = ERouteTypeSelect.ALL;
            ERouteTypeFilter.Id = ERouteChangeRequest_ERouteTypeFilterDTO.Id;
            ERouteTypeFilter.Code = ERouteChangeRequest_ERouteTypeFilterDTO.Code;
            ERouteTypeFilter.Name = ERouteChangeRequest_ERouteTypeFilterDTO.Name;

            List<ERouteType> ERouteTypes = await ERouteTypeService.List(ERouteTypeFilter);
            List<ERouteChangeRequest_ERouteTypeDTO> ERouteChangeRequest_ERouteTypeDTOs = ERouteTypes
                .Select(x => new ERouteChangeRequest_ERouteTypeDTO(x)).ToList();
            return ERouteChangeRequest_ERouteTypeDTOs;
        }
        [Route(ERouteChangeRequestRoute.SingleListOrganization), HttpPost]
        public async Task<List<ERouteChangeRequest_OrganizationDTO>> SingleListOrganization([FromBody] ERouteChangeRequest_OrganizationFilterDTO ERouteChangeRequest_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = ERouteChangeRequest_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ERouteChangeRequest_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ERouteChangeRequest_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = ERouteChangeRequest_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = ERouteChangeRequest_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = ERouteChangeRequest_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = ERouteChangeRequest_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = ERouteChangeRequest_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = ERouteChangeRequest_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ERouteChangeRequest_OrganizationDTO> ERouteChangeRequest_OrganizationDTOs = Organizations
                .Select(x => new ERouteChangeRequest_OrganizationDTO(x)).ToList();
            return ERouteChangeRequest_OrganizationDTOs;
        }
        [Route(ERouteChangeRequestRoute.SingleListRequestState), HttpPost]
        public async Task<List<ERouteChangeRequest_RequestStateDTO>> SingleListRequestState([FromBody] ERouteChangeRequest_RequestStateFilterDTO ERouteChangeRequest_RequestStateFilterDTO)
        {
            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = ERouteChangeRequest_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = ERouteChangeRequest_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = ERouteChangeRequest_RequestStateFilterDTO.Name;

            List<RequestState> RequestStates = await RequestStateService.List(RequestStateFilter);
            List<ERouteChangeRequest_RequestStateDTO> ERouteChangeRequest_RequestStateDTOs = RequestStates
                .Select(x => new ERouteChangeRequest_RequestStateDTO(x)).ToList();
            return ERouteChangeRequest_RequestStateDTOs;
        }

        [Route(ERouteChangeRequestRoute.SingleListStoreGrouping), HttpPost]
        public async Task<List<ERouteChangeRequest_StoreGroupingDTO>> SingleListStoreGrouping([FromBody] ERouteChangeRequest_StoreGroupingFilterDTO ERouteChangeRequest_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = ERouteChangeRequest_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = ERouteChangeRequest_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = ERouteChangeRequest_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = ERouteChangeRequest_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<ERouteChangeRequest_StoreGroupingDTO> ERouteChangeRequest_StoreGroupingDTOs = StoreGroupings
                .Select(x => new ERouteChangeRequest_StoreGroupingDTO(x)).ToList();
            return ERouteChangeRequest_StoreGroupingDTOs;
        }
        [Route(ERouteChangeRequestRoute.SingleListStoreType), HttpPost]
        public async Task<List<ERouteChangeRequest_StoreTypeDTO>> SingleListStoreType([FromBody] ERouteChangeRequest_StoreTypeFilterDTO ERouteChangeRequest_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ERouteChangeRequest_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ERouteChangeRequest_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ERouteChangeRequest_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<ERouteChangeRequest_StoreTypeDTO> ERouteChangeRequest_StoreTypeDTOs = StoreTypes
                .Select(x => new ERouteChangeRequest_StoreTypeDTO(x)).ToList();
            return ERouteChangeRequest_StoreTypeDTOs;
        }

        [Route(ERouteChangeRequestRoute.SingleListStore), HttpPost]
        public async Task<List<ERouteChangeRequest_StoreDTO>> SingleListStore([FromBody] ERouteChangeRequest_StoreFilterDTO ERouteChangeRequest_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERouteChangeRequest_StoreFilterDTO.Id;
            StoreFilter.Code = ERouteChangeRequest_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERouteChangeRequest_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERouteChangeRequest_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERouteChangeRequest_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERouteChangeRequest_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERouteChangeRequest_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ERouteChangeRequest_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ERouteChangeRequest_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERouteChangeRequest_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERouteChangeRequest_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERouteChangeRequest_StoreFilterDTO.WardId;
            StoreFilter.Address = ERouteChangeRequest_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERouteChangeRequest_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERouteChangeRequest_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERouteChangeRequest_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = ERouteChangeRequest_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = ERouteChangeRequest_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = ERouteChangeRequest_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERouteChangeRequest_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERouteChangeRequest_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ERouteChangeRequest_StoreDTO> ERouteChangeRequest_StoreDTOs = Stores
                .Select(x => new ERouteChangeRequest_StoreDTO(x)).ToList();
            return ERouteChangeRequest_StoreDTOs;
        }

        [Route(ERouteChangeRequestRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] ERouteChangeRequest_StoreFilterDTO ERouteChangeRequest_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = ERouteChangeRequest_StoreFilterDTO.Id;
            StoreFilter.Code = ERouteChangeRequest_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERouteChangeRequest_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERouteChangeRequest_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERouteChangeRequest_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERouteChangeRequest_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERouteChangeRequest_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = ERouteChangeRequest_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERouteChangeRequest_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERouteChangeRequest_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERouteChangeRequest_StoreFilterDTO.WardId;
            StoreFilter.Address = ERouteChangeRequest_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERouteChangeRequest_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERouteChangeRequest_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERouteChangeRequest_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ERouteChangeRequest_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERouteChangeRequest_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERouteChangeRequest_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            return await StoreService.Count(StoreFilter);
        }

        [Route(ERouteChangeRequestRoute.ListStore), HttpPost]
        public async Task<List<ERouteChangeRequest_StoreDTO>> ListStore([FromBody] ERouteChangeRequest_StoreFilterDTO ERouteChangeRequest_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = ERouteChangeRequest_StoreFilterDTO.Skip;
            StoreFilter.Take = ERouteChangeRequest_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ERouteChangeRequest_StoreFilterDTO.Id;
            StoreFilter.Code = ERouteChangeRequest_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ERouteChangeRequest_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ERouteChangeRequest_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ERouteChangeRequest_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ERouteChangeRequest_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ERouteChangeRequest_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = ERouteChangeRequest_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ERouteChangeRequest_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ERouteChangeRequest_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ERouteChangeRequest_StoreFilterDTO.WardId;
            StoreFilter.Address = ERouteChangeRequest_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ERouteChangeRequest_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ERouteChangeRequest_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ERouteChangeRequest_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ERouteChangeRequest_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ERouteChangeRequest_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ERouteChangeRequest_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await ERouteChangeRequestService.ListStore(StoreFilter);
            List<ERouteChangeRequest_StoreDTO> ERouteChangeRequest_StoreDTOs = Stores
                .Select(x => new ERouteChangeRequest_StoreDTO(x)).ToList();
            return ERouteChangeRequest_StoreDTOs;
        }
    }
}

