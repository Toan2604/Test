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
using DMS.Services.MStoreBalance;
using DMS.Services.MOrganization;
using DMS.Services.MStore;

namespace DMS.Rpc.store_balance
{
    public partial class StoreBalanceController 
    {
        [Route(StoreBalanceRoute.FilterListOrganization), HttpPost]
        public async Task<List<StoreBalance_OrganizationDTO>> FilterListOrganization([FromBody] StoreBalance_OrganizationFilterDTO StoreBalance_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = StoreBalance_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = StoreBalance_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = StoreBalance_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = StoreBalance_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = StoreBalance_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = StoreBalance_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = StoreBalance_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = StoreBalance_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = StoreBalance_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = StoreBalance_OrganizationFilterDTO.Address;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<StoreBalance_OrganizationDTO> StoreBalance_OrganizationDTOs = Organizations
                .Select(x => new StoreBalance_OrganizationDTO(x)).ToList();
            return StoreBalance_OrganizationDTOs;
        }
        [Route(StoreBalanceRoute.FilterListStore), HttpPost]
        public async Task<List<StoreBalance_StoreDTO>> FilterListStore([FromBody] StoreBalance_StoreFilterDTO StoreBalance_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = int.MaxValue;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = StoreBalance_StoreFilterDTO.Id;
            StoreFilter.Code = StoreBalance_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = StoreBalance_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = StoreBalance_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = StoreBalance_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = StoreBalance_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = StoreBalance_StoreFilterDTO.StoreTypeId;
            StoreFilter.Telephone = StoreBalance_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = StoreBalance_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = StoreBalance_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = StoreBalance_StoreFilterDTO.WardId;
            StoreFilter.Address = StoreBalance_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = StoreBalance_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = StoreBalance_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = StoreBalance_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = StoreBalance_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = StoreBalance_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = StoreBalance_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = StoreBalance_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = StoreBalance_StoreFilterDTO.OwnerEmail;
            StoreFilter.StatusId = StoreBalance_StoreFilterDTO.StatusId;

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<StoreBalance_StoreDTO> StoreBalance_StoreDTOs = Stores
                .Select(x => new StoreBalance_StoreDTO(x)).ToList();
            return StoreBalance_StoreDTOs;
        }
    }
}

