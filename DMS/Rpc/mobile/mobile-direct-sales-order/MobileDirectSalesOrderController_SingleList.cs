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

namespace DMS.Rpc.mobile.direct_sales_order
{
    public partial class MobileDirectSalesOrderController
    {
        [Route(MobileDirectSalesOrderRoute.SingleListBuyerStore), HttpPost]
        public async Task<List<MobileDirectSalesOrder_StoreDTO>> SingleListBuyerStore([FromBody] MobileDirectSalesOrder_StoreFilterDTO MobileDirectSalesOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Search = MobileDirectSalesOrder_StoreFilterDTO.Search;
            StoreFilter.Skip = MobileDirectSalesOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = MobileDirectSalesOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.DESC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = MobileDirectSalesOrder_StoreFilterDTO.Id;
            StoreFilter.Code = MobileDirectSalesOrder_StoreFilterDTO.Code;
            StoreFilter.Name = MobileDirectSalesOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = MobileDirectSalesOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.StoreTypeId = MobileDirectSalesOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = MobileDirectSalesOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StoreCheckingStatusId = MobileDirectSalesOrder_StoreFilterDTO.StoreCheckingStatusId;
            StoreFilter.Telephone = MobileDirectSalesOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = MobileDirectSalesOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = MobileDirectSalesOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = MobileDirectSalesOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = MobileDirectSalesOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = MobileDirectSalesOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = MobileDirectSalesOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = MobileDirectSalesOrder_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = MobileDirectSalesOrder_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = MobileDirectSalesOrder_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = MobileDirectSalesOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = MobileDirectSalesOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = MobileDirectSalesOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreDraftTypeId = MobileDirectSalesOrder_StoreFilterDTO.StoreDraftTypeId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<Store> Stores = await StoreCheckingService.ListStore(StoreFilter, MobileDirectSalesOrder_StoreFilterDTO.ERouteId);
            List<MobileDirectSalesOrder_StoreDTO> MobileDirectSalesOrder_StoreDTOs = Stores
                .Select(x => new MobileDirectSalesOrder_StoreDTO(x)).ToList();
            return MobileDirectSalesOrder_StoreDTOs;
        }
    }
}

