using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public partial class MobileIndirectSalesOrderController
    {
        [Route(MobileIndirectSalesOrderRoute.SingleListBuyerStore), HttpPost]
        public async Task<List<MobileIndirectSalesOrder_StoreDTO>> SingleListBuyerStore([FromBody] MobileIndirectSalesOrder_StoreFilterDTO MobileIndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = MobileIndirectSalesOrder_StoreFilterDTO.Search;
            StoreFilter.Skip = MobileIndirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = MobileIndirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = MobileIndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = MobileIndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.Name = MobileIndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = MobileIndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = MobileIndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = MobileIndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = MobileIndirectSalesOrder_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = MobileIndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = MobileIndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = MobileIndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = MobileIndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = MobileIndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = MobileIndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = MobileIndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = MobileIndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = MobileIndirectSalesOrder_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = MobileIndirectSalesOrder_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = MobileIndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = MobileIndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = MobileIndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreDraftTypeId = MobileIndirectSalesOrder_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStore(StoreFilter, MobileIndirectSalesOrder_StoreFilterDTO.ERouteId);
            List<MobileIndirectSalesOrder_StoreDTO> MobileIndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new MobileIndirectSalesOrder_StoreDTO(x)).ToList();
            return MobileIndirectSalesOrder_StoreDTOs;
        }

        [Route(MobileIndirectSalesOrderRoute.SingleListSellerStore), HttpPost]
        public async Task<List<MobileIndirectSalesOrder_StoreDTO>> SingleListSellerStore([FromBody] MobileIndirectSalesOrder_StoreFilterDTO MobileIndirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = MobileIndirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = MobileIndirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = MobileIndirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = MobileIndirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = MobileIndirectSalesOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = MobileIndirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = MobileIndirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = MobileIndirectSalesOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = MobileIndirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = MobileIndirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = MobileIndirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = MobileIndirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = MobileIndirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = MobileIndirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = MobileIndirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = MobileIndirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = MobileIndirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = MobileIndirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = MobileIndirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = MobileIndirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = MobileIndirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<MobileIndirectSalesOrder_StoreDTO> MobileIndirectSalesOrder_StoreDTOs = Stores
                .Select(x => new MobileIndirectSalesOrder_StoreDTO(x)).ToList();
            return MobileIndirectSalesOrder_StoreDTOs;
        }
    }
}

