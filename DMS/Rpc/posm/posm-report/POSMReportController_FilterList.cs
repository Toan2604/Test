using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public partial class POSMReportController : RpcController
    {
        [Route(POSMReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<POSMReport_OrganizationDTO>> FilterListOrganization([FromBody] POSMReport_OrganizationFilterDTO POSMReport_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = POSMReport_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = POSMReport_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = POSMReport_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = POSMReport_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = POSMReport_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = POSMReport_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = POSMReport_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = POSMReport_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = POSMReport_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = POSMReport_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<POSMReport_OrganizationDTO> POSMReport_OrganizationDTOs = Organizations
                .Select(x => new POSMReport_OrganizationDTO(x)).ToList();
            return POSMReport_OrganizationDTOs;
        }
        [Route(POSMReportRoute.FilterListShowingItem), HttpPost]
        public async Task<List<POSMReport_ShowingItemDTO>> FilterListShowingItem([FromBody] POSMReport_ShowingItemFilterDTO POSMReport_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Skip = 0;
            ShowingItemFilter.Take = 20;
            ShowingItemFilter.OrderBy = ShowingItemOrder.Id;
            ShowingItemFilter.OrderType = OrderType.ASC;
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
            ShowingItemFilter.Id = POSMReport_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = POSMReport_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = POSMReport_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = POSMReport_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.UnitOfMeasureId = POSMReport_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = POSMReport_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = POSMReport_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = POSMReport_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = POSMReport_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = POSMReport_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.Search = POSMReport_ShowingItemFilterDTO.Search;

            List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);
            List<POSMReport_ShowingItemDTO> POSMReport_ShowingItemDTOs = ShowingItems
                .Select(x => new POSMReport_ShowingItemDTO(x)).ToList();
            return POSMReport_ShowingItemDTOs;
        }
        [Route(POSMReportRoute.FilterListStore), HttpPost]
        public async Task<List<POSMReport_StoreDTO>> FilterListStore([FromBody] POSMReport_StoreFilterDTO POSMReport_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = POSMReport_StoreFilterDTO.Id;
            StoreFilter.Code = POSMReport_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = POSMReport_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = POSMReport_StoreFilterDTO.Name;
            StoreFilter.UnsignName = POSMReport_StoreFilterDTO.UnsignName;
            StoreFilter.ParentStoreId = POSMReport_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = POSMReport_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = POSMReport_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = POSMReport_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = POSMReport_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = POSMReport_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = POSMReport_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = POSMReport_StoreFilterDTO.WardId;
            StoreFilter.Address = POSMReport_StoreFilterDTO.Address;
            StoreFilter.UnsignAddress = POSMReport_StoreFilterDTO.UnsignAddress;
            StoreFilter.DeliveryAddress = POSMReport_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = POSMReport_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = POSMReport_StoreFilterDTO.Longitude;
            StoreFilter.DeliveryLatitude = POSMReport_StoreFilterDTO.DeliveryLatitude;
            StoreFilter.DeliveryLongitude = POSMReport_StoreFilterDTO.DeliveryLongitude;
            StoreFilter.OwnerName = POSMReport_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = POSMReport_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = POSMReport_StoreFilterDTO.OwnerEmail;
            StoreFilter.CreatorId = POSMReport_StoreFilterDTO.CreatorId;
            StoreFilter.AppUserId = POSMReport_StoreFilterDTO.AppUserId;
            StoreFilter.StatusId = POSMReport_StoreFilterDTO.StatusId;
            StoreFilter.StoreStatusId = POSMReport_StoreFilterDTO.StoreStatusId;

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            List<Store> Stores = await StoreService.List(StoreFilter);
            List<POSMReport_StoreDTO> POSMReport_StoreDTOs = Stores
                .Select(x => new POSMReport_StoreDTO(x)).ToList();
            return POSMReport_StoreDTOs;
        }

        [Route(POSMReportRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<POSMReport_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] POSMReport_StoreGroupingFilterDTO POSMReport_StoreGroupingFilterDTO)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = POSMReport_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = POSMReport_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = POSMReport_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = POSMReport_StoreGroupingFilterDTO.Path;

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<POSMReport_StoreGroupingDTO> POSMReport_StoreGroupingDTOs = StoreGroupings
                .Select(x => new POSMReport_StoreGroupingDTO(x)).ToList();
            return POSMReport_StoreGroupingDTOs;
        }

        [Route(POSMReportRoute.FilterListStoreType), HttpPost]
        public async Task<List<POSMReport_StoreTypeDTO>> FilterListStoreType([FromBody] POSMReport_StoreTypeFilterDTO POSMReport_StoreTypeFilterDTO)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = POSMReport_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = POSMReport_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = POSMReport_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = null;

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);

            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<POSMReport_StoreTypeDTO> POSMReport_StoreTypeDTOs = StoreTypes
                .Select(x => new POSMReport_StoreTypeDTO(x)).ToList();
            return POSMReport_StoreTypeDTOs;
        }
    }
}

