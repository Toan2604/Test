using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public partial class ShowingOrderController : RpcController
    {
        [Route(ShowingOrderRoute.CountShowingItem), HttpPost]
        public async Task<ActionResult<int>> CountShowingItem([FromBody] ShowingOrder_ShowingItemFilterDTO ShowingOrder_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Id = ShowingOrder_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = ShowingOrder_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = ShowingOrder_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = ShowingOrder_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.UnitOfMeasureId = ShowingOrder_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = ShowingOrder_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = ShowingOrder_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = ShowingOrder_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = ShowingOrder_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = ShowingOrder_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.Search = ShowingOrder_ShowingItemFilterDTO.Search;

            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            int count = await ShowingItemService.Count(ShowingItemFilter);
            return count;
        }

        [Route(ShowingOrderRoute.ListShowingItem), HttpPost]
        public async Task<ActionResult<List<ShowingOrder_ShowingItemDTO>>> ListShowingItem([FromBody] ShowingOrder_ShowingItemFilterDTO ShowingOrder_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
            ShowingItemFilter.Skip = ShowingOrder_ShowingItemFilterDTO.Skip;
            ShowingItemFilter.Take = ShowingOrder_ShowingItemFilterDTO.Take;
            ShowingItemFilter.OrderBy = ShowingOrder_ShowingItemFilterDTO.OrderBy;
            ShowingItemFilter.OrderType = ShowingOrder_ShowingItemFilterDTO.OrderType;

            ShowingItemFilter.Id = ShowingOrder_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = ShowingOrder_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = ShowingOrder_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = ShowingOrder_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.ShowingWarehouseId = ShowingOrder_ShowingItemFilterDTO.ShowingWarehouseId;
            ShowingItemFilter.UnitOfMeasureId = ShowingOrder_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = ShowingOrder_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = ShowingOrder_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = ShowingOrder_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = ShowingOrder_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = ShowingOrder_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.Search = ShowingOrder_ShowingItemFilterDTO.Search;

            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            List<ShowingItem> ShowingItems = await ShowingOrderService.ListShowingItem(ShowingItemFilter);
            List<ShowingOrder_ShowingItemDTO> ShowingOrder_ShowingItemDTOs = ShowingItems
                .Select(c => new ShowingOrder_ShowingItemDTO(c)).ToList();
            return ShowingOrder_ShowingItemDTOs;
        }

        [Route(ShowingOrderRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] ShowingOrder_StoreFilterDTO ShowingOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = ShowingOrder_StoreFilterDTO.Id;
            StoreFilter.Code = ShowingOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ShowingOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ShowingOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ShowingOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ShowingOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ShowingOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ShowingOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ShowingOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ShowingOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ShowingOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ShowingOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = ShowingOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ShowingOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ShowingOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ShowingOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ShowingOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ShowingOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ShowingOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            return await StoreService.Count(StoreFilter);
        }

        [Route(ShowingOrderRoute.ListStore), HttpPost]
        public async Task<List<ShowingOrder_StoreDTO>> ListStore([FromBody] ShowingOrder_StoreFilterDTO ShowingOrder_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = ShowingOrder_StoreFilterDTO.Skip;
            StoreFilter.Take = ShowingOrder_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ShowingOrder_StoreFilterDTO.Id;
            StoreFilter.Code = ShowingOrder_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ShowingOrder_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ShowingOrder_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ShowingOrder_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ShowingOrder_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ShowingOrder_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ShowingOrder_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ShowingOrder_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ShowingOrder_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ShowingOrder_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ShowingOrder_StoreFilterDTO.WardId;
            StoreFilter.Address = ShowingOrder_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ShowingOrder_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ShowingOrder_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ShowingOrder_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ShowingOrder_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ShowingOrder_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ShowingOrder_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ShowingOrder_StoreDTO> ShowingOrder_StoreDTOs = Stores
                .Select(x => new ShowingOrder_StoreDTO(x)).ToList();
            return ShowingOrder_StoreDTOs;
        }
    }
}

