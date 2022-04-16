using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store_grouping
{
    public class StoreGroupingController : RpcController
    {
        private IStoreService StoreService;
        private IStatusService StatusService;
        private IStoreGroupingService StoreGroupingService;
        private ICurrentContext CurrentContext;
        public StoreGroupingController(
            IStoreService StoreService,
            IStatusService StatusService,
            IStoreGroupingService StoreGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.StoreService = StoreService;
            this.StatusService = StatusService;
            this.StoreGroupingService = StoreGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(StoreGroupingRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] StoreGrouping_StoreGroupingFilterDTO StoreGrouping_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = ConvertFilterDTOToFilterEntity(StoreGrouping_StoreGroupingFilterDTO);
            StoreGroupingFilter = StoreGroupingService.ToFilter(StoreGroupingFilter);
            int count = await StoreGroupingService.Count(StoreGroupingFilter);
            return count;
        }

        [Route(StoreGroupingRoute.List), HttpPost]
        public async Task<ActionResult<List<StoreGrouping_StoreGroupingDTO>>> List([FromBody] StoreGrouping_StoreGroupingFilterDTO StoreGrouping_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = ConvertFilterDTOToFilterEntity(StoreGrouping_StoreGroupingFilterDTO);
            StoreGroupingFilter = StoreGroupingService.ToFilter(StoreGroupingFilter);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<StoreGrouping_StoreGroupingDTO> StoreGrouping_StoreGroupingDTOs = StoreGroupings
                .Select(c => new StoreGrouping_StoreGroupingDTO(c)).ToList();
            return StoreGrouping_StoreGroupingDTOs;
        }

        [Route(StoreGroupingRoute.Get), HttpPost]
        public async Task<ActionResult<StoreGrouping_StoreGroupingDTO>> Get([FromBody] StoreGrouping_StoreGroupingDTO StoreGrouping_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreGrouping_StoreGroupingDTO.Id))
                return Forbid();

            StoreGrouping StoreGrouping = await StoreGroupingService.Get(StoreGrouping_StoreGroupingDTO.Id);
            return new StoreGrouping_StoreGroupingDTO(StoreGrouping);
        }

        [Route(StoreGroupingRoute.Create), HttpPost]
        public async Task<ActionResult<StoreGrouping_StoreGroupingDTO>> Create([FromBody] StoreGrouping_StoreGroupingDTO StoreGrouping_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreGrouping_StoreGroupingDTO.Id))
                return Forbid();

            StoreGrouping StoreGrouping = ConvertDTOToEntity(StoreGrouping_StoreGroupingDTO);
            StoreGrouping = await StoreGroupingService.Create(StoreGrouping);
            StoreGrouping_StoreGroupingDTO = new StoreGrouping_StoreGroupingDTO(StoreGrouping);
            if (StoreGrouping.IsValidated)
                return StoreGrouping_StoreGroupingDTO;
            else
                return BadRequest(StoreGrouping_StoreGroupingDTO);
        }

        [Route(StoreGroupingRoute.Update), HttpPost]
        public async Task<ActionResult<StoreGrouping_StoreGroupingDTO>> Update([FromBody] StoreGrouping_StoreGroupingDTO StoreGrouping_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreGrouping_StoreGroupingDTO.Id))
                return Forbid();

            StoreGrouping StoreGrouping = ConvertDTOToEntity(StoreGrouping_StoreGroupingDTO);
            StoreGrouping = await StoreGroupingService.Update(StoreGrouping);
            StoreGrouping_StoreGroupingDTO = new StoreGrouping_StoreGroupingDTO(StoreGrouping);
            if (StoreGrouping.IsValidated)
                return StoreGrouping_StoreGroupingDTO;
            else
                return BadRequest(StoreGrouping_StoreGroupingDTO);
        }

        [Route(StoreGroupingRoute.Delete), HttpPost]
        public async Task<ActionResult<StoreGrouping_StoreGroupingDTO>> Delete([FromBody] StoreGrouping_StoreGroupingDTO StoreGrouping_StoreGroupingDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(StoreGrouping_StoreGroupingDTO.Id))
                return Forbid();

            StoreGrouping StoreGrouping = ConvertDTOToEntity(StoreGrouping_StoreGroupingDTO);
            StoreGrouping = await StoreGroupingService.Delete(StoreGrouping);
            StoreGrouping_StoreGroupingDTO = new StoreGrouping_StoreGroupingDTO(StoreGrouping);
            if (StoreGrouping.IsValidated)
                return StoreGrouping_StoreGroupingDTO;
            else
                return BadRequest(StoreGrouping_StoreGroupingDTO);
        }

        [Route(StoreGroupingRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Id = new IdFilter { In = Ids };
            StoreGroupingFilter.Selects = StoreGroupingSelect.Id;
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            StoreGroupings = await StoreGroupingService.BulkDelete(StoreGroupings);
            if (StoreGroupings.Any(x => !x.IsValidated))
                return BadRequest(StoreGroupings.Where(x => !x.IsValidated));
            return true;
        }

        private async Task<bool> HasPermission(long Id)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter = StoreGroupingService.ToFilter(StoreGroupingFilter);
            if (Id == 0)
            {

            }
            else
            {
                StoreGroupingFilter.Id = new IdFilter { Equal = Id };
                int count = await StoreGroupingService.Count(StoreGroupingFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private StoreGrouping ConvertDTOToEntity(StoreGrouping_StoreGroupingDTO StoreGrouping_StoreGroupingDTO)
        {
            StoreGrouping_StoreGroupingDTO.TrimString();
            StoreGrouping StoreGrouping = new StoreGrouping();
            StoreGrouping.Id = StoreGrouping_StoreGroupingDTO.Id;
            StoreGrouping.Code = StoreGrouping_StoreGroupingDTO.Code;
            StoreGrouping.Name = StoreGrouping_StoreGroupingDTO.Name;
            StoreGrouping.ParentId = StoreGrouping_StoreGroupingDTO.ParentId;
            StoreGrouping.Path = StoreGrouping_StoreGroupingDTO.Path;
            StoreGrouping.Level = StoreGrouping_StoreGroupingDTO.Level;
            StoreGrouping.StatusId = StoreGrouping_StoreGroupingDTO.StatusId;
            StoreGrouping.Parent = StoreGrouping_StoreGroupingDTO.Parent == null ? null : new StoreGrouping
            {
                Id = StoreGrouping_StoreGroupingDTO.Parent.Id,
                Code = StoreGrouping_StoreGroupingDTO.Parent.Code,
                Name = StoreGrouping_StoreGroupingDTO.Parent.Name,
                Path = StoreGrouping_StoreGroupingDTO.Parent.Path,
                Level = StoreGrouping_StoreGroupingDTO.Parent.Level,
                ParentId = StoreGrouping_StoreGroupingDTO.Parent.ParentId,
            };
            StoreGrouping.Status = StoreGrouping_StoreGroupingDTO.Status == null ? null : new Status
            {
                Id = StoreGrouping_StoreGroupingDTO.Status.Id,
                Code = StoreGrouping_StoreGroupingDTO.Status.Code,
                Name = StoreGrouping_StoreGroupingDTO.Status.Name,
            };
            StoreGrouping.Stores = StoreGrouping_StoreGroupingDTO.Stores?
                .Select(x => new Store
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ParentStoreId = x.ParentStoreId,
                    OrganizationId = x.OrganizationId,
                    StoreTypeId = x.StoreTypeId,
                    Telephone = x.Telephone,
                    ProvinceId = x.ProvinceId,
                    DistrictId = x.DistrictId,
                    WardId = x.WardId,
                    Address = x.Address,
                    DeliveryAddress = x.DeliveryAddress,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DeliveryLatitude = x.DeliveryLatitude,
                    DeliveryLongitude = x.DeliveryLongitude,
                    OwnerName = x.OwnerName,
                    OwnerPhone = x.OwnerPhone,
                    OwnerEmail = x.OwnerEmail,
                    TaxCode = x.TaxCode,
                    LegalEntity = x.LegalEntity,
                    StatusId = x.StatusId,
                    District = new District
                    {
                        Id = x.District.Id,
                        Name = x.District.Name,
                        Priority = x.District.Priority,
                        ProvinceId = x.District.ProvinceId,
                        StatusId = x.District.StatusId,
                    },
                    Organization = new Organization
                    {
                        Id = x.Organization.Id,
                        Code = x.Organization.Code,
                        Name = x.Organization.Name,
                        ParentId = x.Organization.ParentId,
                        Path = x.Organization.Path,
                        Level = x.Organization.Level,
                        StatusId = x.Organization.StatusId,
                        Phone = x.Organization.Phone,
                        Address = x.Organization.Address,
                        Email = x.Organization.Email,
                    },
                    ParentStore = new Store
                    {
                        Id = x.ParentStore.Id,
                        Code = x.ParentStore.Code,
                        Name = x.ParentStore.Name,
                        ParentStoreId = x.ParentStore.ParentStoreId,
                        OrganizationId = x.ParentStore.OrganizationId,
                        StoreTypeId = x.ParentStore.StoreTypeId,
                        Telephone = x.ParentStore.Telephone,
                        ProvinceId = x.ParentStore.ProvinceId,
                        DistrictId = x.ParentStore.DistrictId,
                        WardId = x.ParentStore.WardId,
                        Address = x.ParentStore.Address,
                        DeliveryAddress = x.ParentStore.DeliveryAddress,
                        Latitude = x.ParentStore.Latitude,
                        Longitude = x.ParentStore.Longitude,
                        DeliveryLatitude = x.ParentStore.DeliveryLatitude,
                        DeliveryLongitude = x.ParentStore.DeliveryLongitude,
                        OwnerName = x.ParentStore.OwnerName,
                        OwnerPhone = x.ParentStore.OwnerPhone,
                        OwnerEmail = x.ParentStore.OwnerEmail,
                        StatusId = x.ParentStore.StatusId,
                    },
                    Province = new Province
                    {
                        Id = x.Province.Id,
                        Name = x.Province.Name,
                        Priority = x.Province.Priority,
                        StatusId = x.Province.StatusId,
                    },
                    Status = new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    StoreType = new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        StatusId = x.StoreType.StatusId,
                    },
                    Ward = new Ward
                    {
                        Id = x.Ward.Id,
                        Name = x.Ward.Name,
                        Priority = x.Ward.Priority,
                        DistrictId = x.Ward.DistrictId,
                        StatusId = x.Ward.StatusId,
                    },
                }).ToList();
            StoreGrouping.BaseLanguage = CurrentContext.Language;
            return StoreGrouping;
        }

        private StoreGroupingFilter ConvertFilterDTOToFilterEntity(StoreGrouping_StoreGroupingFilterDTO StoreGrouping_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGrouping_StoreGroupingFilterDTO.OrderBy;
            StoreGroupingFilter.OrderType = StoreGrouping_StoreGroupingFilterDTO.OrderType;

            StoreGroupingFilter.Id = StoreGrouping_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = StoreGrouping_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = StoreGrouping_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = StoreGrouping_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = StoreGrouping_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = StoreGrouping_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = StoreGrouping_StoreGroupingFilterDTO.StatusId;
            return StoreGroupingFilter;
        }

        [Route(StoreGroupingRoute.SingleListParentStoreGrouping), HttpPost]
        public async Task<List<StoreGrouping_StoreGroupingDTO>> SingleListParentStoreGrouping([FromBody] StoreGrouping_StoreGroupingFilterDTO StoreGrouping_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = StoreGrouping_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = StoreGrouping_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = StoreGrouping_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = StoreGrouping_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = StoreGrouping_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = StoreGrouping_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<StoreGrouping_StoreGroupingDTO> StoreGrouping_StoreGroupingDTOs = StoreGroupings
                .Select(x => new StoreGrouping_StoreGroupingDTO(x)).ToList();
            return StoreGrouping_StoreGroupingDTOs;
        }

        [Route(StoreGroupingRoute.SingleListStatus), HttpPost]
        public async Task<List<StoreGrouping_StatusDTO>> SingleListStatus([FromBody] StoreGrouping_StatusFilterDTO StoreGrouping_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<StoreGrouping_StatusDTO> StoreGrouping_StatusDTOs = Statuses
                .Select(x => new StoreGrouping_StatusDTO(x)).ToList();
            return StoreGrouping_StatusDTOs;
        }

        [Route(StoreGroupingRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] StoreGrouping_StoreFilterDTO StoreGrouping_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = StoreGrouping_StoreFilterDTO.Id;
            StoreFilter.Code = StoreGrouping_StoreFilterDTO.Code;
            StoreFilter.Name = StoreGrouping_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = StoreGrouping_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = StoreGrouping_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = StoreGrouping_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = StoreGrouping_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = StoreGrouping_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = StoreGrouping_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = StoreGrouping_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = StoreGrouping_StoreFilterDTO.WardId;
            StoreFilter.Address = StoreGrouping_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = StoreGrouping_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = StoreGrouping_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = StoreGrouping_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = StoreGrouping_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = StoreGrouping_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = StoreGrouping_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = StoreGrouping_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = StoreGrouping_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            return await StoreService.Count(StoreFilter);
        }

        [Route(StoreGroupingRoute.ListStore), HttpPost]
        public async Task<List<StoreGrouping_StoreDTO>> ListStore([FromBody] StoreGrouping_StoreFilterDTO StoreGrouping_StoreFilterDTO)
        {
            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = StoreGrouping_StoreFilterDTO.Skip;
            StoreFilter.Take = StoreGrouping_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = StoreGrouping_StoreFilterDTO.Id;
            StoreFilter.Code = StoreGrouping_StoreFilterDTO.Code;
            StoreFilter.Name = StoreGrouping_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = StoreGrouping_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = StoreGrouping_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = StoreGrouping_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = StoreGrouping_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = StoreGrouping_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = StoreGrouping_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = StoreGrouping_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = StoreGrouping_StoreFilterDTO.WardId;
            StoreFilter.Address = StoreGrouping_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = StoreGrouping_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = StoreGrouping_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = StoreGrouping_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = StoreGrouping_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = StoreGrouping_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = StoreGrouping_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = StoreGrouping_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = StoreGrouping_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<StoreGrouping_StoreDTO> StoreGrouping_StoreDTOs = Stores
                .Select(x => new StoreGrouping_StoreDTO(x)).ToList();
            return StoreGrouping_StoreDTOs;
        }
    }
}

