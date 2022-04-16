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
        [Route(LuckyDrawRoute.CountStoreGrouping), HttpPost]
        public async Task<long> CountStoreGrouping([FromBody] LuckyDraw_StoreGroupingFilterDTO LuckyDraw_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Id = LuckyDraw_StoreGroupingFilterDTO.Id;
            StoreGroupingFilter.Code = LuckyDraw_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = LuckyDraw_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.ParentId = LuckyDraw_StoreGroupingFilterDTO.ParentId;
            StoreGroupingFilter.Path = LuckyDraw_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.Level = LuckyDraw_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreGroupingService.Count(StoreGroupingFilter);
        }

        [Route(LuckyDrawRoute.ListStoreGrouping), HttpPost]
        public async Task<List<LuckyDraw_StoreGroupingDTO>> ListStoreGrouping([FromBody] LuckyDraw_StoreGroupingFilterDTO LuckyDraw_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = LuckyDraw_StoreGroupingFilterDTO.Skip;
            StoreGroupingFilter.Take = LuckyDraw_StoreGroupingFilterDTO.Take;
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
        [Route(LuckyDrawRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] LuckyDraw_StoreFilterDTO LuckyDraw_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
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

            return await StoreService.Count(StoreFilter);
        }

        [Route(LuckyDrawRoute.ListStore), HttpPost]
        public async Task<List<LuckyDraw_StoreDTO>> ListStore([FromBody] LuckyDraw_StoreFilterDTO LuckyDraw_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = LuckyDraw_StoreFilterDTO.Skip;
            StoreFilter.Take = LuckyDraw_StoreFilterDTO.Take;
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
        [Route(LuckyDrawRoute.CountStoreType), HttpPost]
        public async Task<long> CountStoreType([FromBody] LuckyDraw_StoreTypeFilterDTO LuckyDraw_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Id = LuckyDraw_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = LuckyDraw_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = LuckyDraw_StoreTypeFilterDTO.Name;
            StoreTypeFilter.ColorId = LuckyDraw_StoreTypeFilterDTO.ColorId;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            return await StoreTypeService.Count(StoreTypeFilter);
        }

        [Route(LuckyDrawRoute.ListStoreType), HttpPost]
        public async Task<List<LuckyDraw_StoreTypeDTO>> ListStoreType([FromBody] LuckyDraw_StoreTypeFilterDTO LuckyDraw_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = LuckyDraw_StoreTypeFilterDTO.Skip;
            StoreTypeFilter.Take = LuckyDraw_StoreTypeFilterDTO.Take;
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
    }
}

