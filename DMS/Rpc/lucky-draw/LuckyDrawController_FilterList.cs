using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Common;
using DMS.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using DMS.Entities;
using DMS.Services.MLuckyDraw;
using DMS.Services.MImage;
using DMS.Services.MLuckyDrawType;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStore;
using DMS.Services.MStoreType;
using DMS.Services.MLuckyDrawStructure;
using DMS.Services.MLuckyDrawWinner;
using DMS.Services.MAppUser;
using DMS.Enums;

namespace DMS.Rpc.lucky_draw
{
    public partial class LuckyDrawController 
    {
        [Route(LuckyDrawRoute.FilterListLuckyDrawType), HttpPost]
        public async Task<List<LuckyDraw_LuckyDrawTypeDTO>> FilterListLuckyDrawType([FromBody] LuckyDraw_LuckyDrawTypeFilterDTO LuckyDraw_LuckyDrawTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawTypeFilter LuckyDrawTypeFilter = new LuckyDrawTypeFilter();
            LuckyDrawTypeFilter.Skip = 0;
            LuckyDrawTypeFilter.Take = int.MaxValue;
            LuckyDrawTypeFilter.Take = 20;
            LuckyDrawTypeFilter.OrderBy = LuckyDrawTypeOrder.Id;
            LuckyDrawTypeFilter.OrderType = OrderType.ASC;
            LuckyDrawTypeFilter.Selects = LuckyDrawTypeSelect.ALL;

            List<LuckyDrawType> LuckyDrawTypes = await LuckyDrawTypeService.List(LuckyDrawTypeFilter);
            List<LuckyDraw_LuckyDrawTypeDTO> LuckyDraw_LuckyDrawTypeDTOs = LuckyDrawTypes
                .Select(x => new LuckyDraw_LuckyDrawTypeDTO(x)).ToList();
            return LuckyDraw_LuckyDrawTypeDTOs;
        }
        [Route(LuckyDrawRoute.FilterListOrganization), HttpPost]
        public async Task<List<LuckyDraw_OrganizationDTO>> FilterListOrganization([FromBody] LuckyDraw_OrganizationFilterDTO LuckyDraw_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = LuckyDraw_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = LuckyDraw_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = LuckyDraw_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = LuckyDraw_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = LuckyDraw_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = LuckyDraw_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = LuckyDraw_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = LuckyDraw_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = LuckyDraw_OrganizationFilterDTO.Address;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<LuckyDraw_OrganizationDTO> LuckyDraw_OrganizationDTOs = Organizations
                .Select(x => new LuckyDraw_OrganizationDTO(x)).ToList();
            return LuckyDraw_OrganizationDTOs;
        }
        [Route(LuckyDrawRoute.FilterListStatus), HttpPost]
        public async Task<List<LuckyDraw_StatusDTO>> FilterListStatus([FromBody] LuckyDraw_StatusFilterDTO LuckyDraw_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<LuckyDraw_StatusDTO> LuckyDraw_StatusDTOs = Statuses
                .Select(x => new LuckyDraw_StatusDTO(x)).ToList();
            return LuckyDraw_StatusDTOs;
        }
        [Route(LuckyDrawRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<LuckyDraw_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] LuckyDraw_StoreGroupingFilterDTO LuckyDraw_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = int.MaxValue;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Id = LuckyDraw_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = LuckyDraw_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = LuckyDraw_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = LuckyDraw_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = LuckyDraw_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = LuckyDraw_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<LuckyDraw_StoreGroupingDTO> LuckyDraw_StoreGroupingDTOs = StoreGroupings
                .Select(x => new LuckyDraw_StoreGroupingDTO(x)).ToList();
            return LuckyDraw_StoreGroupingDTOs;
        }
        [Route(LuckyDrawRoute.FilterListStore), HttpPost]
        public async Task<List<LuckyDraw_StoreDTO>> FilterListStore([FromBody] LuckyDraw_StoreFilterDTO LuckyDraw_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = LuckyDraw_StoreFilterDTO.Id;
            StoreFilter.Code = LuckyDraw_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = LuckyDraw_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = LuckyDraw_StoreFilterDTO.Name;
            StoreFilter.UnsignName = LuckyDraw_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = LuckyDraw_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = LuckyDraw_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = LuckyDraw_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = LuckyDraw_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = LuckyDraw_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = LuckyDraw_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = LuckyDraw_StoreFilterDTO.WardId;
            StoreFilter.Address = LuckyDraw_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = LuckyDraw_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = LuckyDraw_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = LuckyDraw_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = LuckyDraw_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = LuckyDraw_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = LuckyDraw_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = LuckyDraw_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = LuckyDraw_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = LuckyDraw_StoreFilterDTO.OwnerEmail;
            StoreFilter.CreatorId = LuckyDraw_StoreFilterDTO.CreatorId;
            StoreFilter.AppUserId = LuckyDraw_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter.StoreStatusId = LuckyDraw_StoreFilterDTO.StoreStatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<LuckyDraw_StoreDTO> LuckyDraw_StoreDTOs = Stores
                .Select(x => new LuckyDraw_StoreDTO(x)).ToList();
            return LuckyDraw_StoreDTOs;
        }
        [Route(LuckyDrawRoute.FilterListStoreType), HttpPost]
        public async Task<List<LuckyDraw_StoreTypeDTO>> FilterListStoreType([FromBody] LuckyDraw_StoreTypeFilterDTO LuckyDraw_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = LuckyDraw_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = LuckyDraw_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = LuckyDraw_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = LuckyDraw_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<LuckyDraw_StoreTypeDTO> LuckyDraw_StoreTypeDTOs = StoreTypes
                .Select(x => new LuckyDraw_StoreTypeDTO(x)).ToList();
            return LuckyDraw_StoreTypeDTOs;
        }
        [Route(LuckyDrawRoute.FilterListLuckyDrawStructure), HttpPost]
        public async Task<List<LuckyDraw_LuckyDrawStructureDTO>> FilterListLuckyDrawStructure([FromBody] LuckyDraw_LuckyDrawStructureFilterDTO LuckyDraw_LuckyDrawStructureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawStructureFilter LuckyDrawStructureFilter = new LuckyDrawStructureFilter();
            LuckyDrawStructureFilter.Skip = 0;
            LuckyDrawStructureFilter.Take = 20;
            LuckyDrawStructureFilter.OrderBy = LuckyDrawStructureOrder.Id;
            LuckyDrawStructureFilter.OrderType = OrderType.ASC;
            LuckyDrawStructureFilter.Selects = LuckyDrawStructureSelect.ALL;
            LuckyDrawStructureFilter.Id = LuckyDraw_LuckyDrawStructureFilterDTO.Id;
            LuckyDrawStructureFilter.LuckyDrawId = LuckyDraw_LuckyDrawStructureFilterDTO.LuckyDrawId;
            LuckyDrawStructureFilter.Name = LuckyDraw_LuckyDrawStructureFilterDTO.Name;
            LuckyDrawStructureFilter.Value = LuckyDraw_LuckyDrawStructureFilterDTO.Value;
            LuckyDrawStructureFilter.Quantity = LuckyDraw_LuckyDrawStructureFilterDTO.Quantity;

            List<LuckyDrawStructure> LuckyDrawStructures = await LuckyDrawStructureService.List(LuckyDrawStructureFilter);
            List<LuckyDraw_LuckyDrawStructureDTO> LuckyDraw_LuckyDrawStructureDTOs = LuckyDrawStructures
                .Select(x => new LuckyDraw_LuckyDrawStructureDTO(x)).ToList();
            return LuckyDraw_LuckyDrawStructureDTOs;
        }
        [Route(LuckyDrawRoute.FilterListLuckyDrawWinner), HttpPost]
        public async Task<List<LuckyDraw_LuckyDrawWinnerDTO>> FilterListLuckyDrawWinner([FromBody] LuckyDraw_LuckyDrawWinnerFilterDTO LuckyDraw_LuckyDrawWinnerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
            LuckyDrawWinnerFilter.Skip = 0;
            LuckyDrawWinnerFilter.Take = 20;
            LuckyDrawWinnerFilter.OrderBy = LuckyDrawWinnerOrder.Id;
            LuckyDrawWinnerFilter.OrderType = OrderType.ASC;
            LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
            LuckyDrawWinnerFilter.Id = LuckyDraw_LuckyDrawWinnerFilterDTO.Id;
            LuckyDrawWinnerFilter.LuckyDrawId = LuckyDraw_LuckyDrawWinnerFilterDTO.LuckyDrawId;
            LuckyDrawWinnerFilter.LuckyDrawStructureId = LuckyDraw_LuckyDrawWinnerFilterDTO.LuckyDrawStructureId;
            LuckyDrawWinnerFilter.LuckyDrawRegistrationId = LuckyDraw_LuckyDrawWinnerFilterDTO.LuckyDrawRegistrationId;
            LuckyDrawWinnerFilter.Time = LuckyDraw_LuckyDrawWinnerFilterDTO.Time;

            List<LuckyDrawWinner> LuckyDrawWinners = await LuckyDrawWinnerService.List(LuckyDrawWinnerFilter);
            List<LuckyDraw_LuckyDrawWinnerDTO> LuckyDraw_LuckyDrawWinnerDTOs = LuckyDrawWinners
                .Select(x => new LuckyDraw_LuckyDrawWinnerDTO(x)).ToList();
            return LuckyDraw_LuckyDrawWinnerDTOs;
        }
        [Route(LuckyDrawRoute.FilterListAppUser), HttpPost]
        public async Task<List<LuckyDraw_AppUserDTO>> FilterListAppUser([FromBody] LuckyDraw_AppUserFilterDTO LuckyDraw_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = LuckyDraw_AppUserFilterDTO.Id;
            AppUserFilter.Username = LuckyDraw_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = LuckyDraw_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = LuckyDraw_AppUserFilterDTO.Address;
            AppUserFilter.Email = LuckyDraw_AppUserFilterDTO.Email;
            AppUserFilter.Phone = LuckyDraw_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = LuckyDraw_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = LuckyDraw_AppUserFilterDTO.Birthday;
            AppUserFilter.PositionId = LuckyDraw_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = LuckyDraw_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = LuckyDraw_AppUserFilterDTO.OrganizationId;
            AppUserFilter.ProvinceId = LuckyDraw_AppUserFilterDTO.ProvinceId;
            AppUserFilter.StatusId = LuckyDraw_AppUserFilterDTO.StatusId;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<LuckyDraw_AppUserDTO> LuckyDraw_AppUserDTOs = AppUsers
                .Select(x => new LuckyDraw_AppUserDTO(x)).ToList();
            return LuckyDraw_AppUserDTOs;
        }
    }
}

