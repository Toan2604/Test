using DMS.Entities;
using DMS.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public partial class ShowingOrderWithDrawController : RpcController
    {
        [Route(ShowingOrderWithDrawRoute.CountShowingItem), HttpPost]
        public async Task<ActionResult<int>> CountShowingItem([FromBody] ShowingOrderWithDraw_ShowingItemFilterDTO ShowingOrderWithDraw_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Id = ShowingOrderWithDraw_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = ShowingOrderWithDraw_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = ShowingOrderWithDraw_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = ShowingOrderWithDraw_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.UnitOfMeasureId = ShowingOrderWithDraw_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = ShowingOrderWithDraw_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = ShowingOrderWithDraw_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = ShowingOrderWithDraw_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = ShowingOrderWithDraw_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = ShowingOrderWithDraw_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.Search = ShowingOrderWithDraw_ShowingItemFilterDTO.Search;

            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            int count = await ShowingItemService.Count(ShowingItemFilter);
            return count;
        }

        [Route(ShowingOrderWithDrawRoute.ListShowingItem), HttpPost]
        public async Task<ActionResult<List<ShowingOrderWithDraw_ShowingItemDTO>>> ListShowingItem([FromBody] ShowingOrderWithDraw_ShowingItemFilterDTO ShowingOrderWithDraw_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
            ShowingItemFilter.Skip = ShowingOrderWithDraw_ShowingItemFilterDTO.Skip;
            ShowingItemFilter.Take = ShowingOrderWithDraw_ShowingItemFilterDTO.Take;
            ShowingItemFilter.OrderBy = ShowingOrderWithDraw_ShowingItemFilterDTO.OrderBy;
            ShowingItemFilter.OrderType = ShowingOrderWithDraw_ShowingItemFilterDTO.OrderType;

            ShowingItemFilter.Id = ShowingOrderWithDraw_ShowingItemFilterDTO.Id;
            ShowingItemFilter.Code = ShowingOrderWithDraw_ShowingItemFilterDTO.Code;
            ShowingItemFilter.Name = ShowingOrderWithDraw_ShowingItemFilterDTO.Name;
            ShowingItemFilter.ShowingCategoryId = ShowingOrderWithDraw_ShowingItemFilterDTO.ShowingCategoryId;
            ShowingItemFilter.ShowingWarehouseId = ShowingOrderWithDraw_ShowingItemFilterDTO.ShowingWarehouseId;
            ShowingItemFilter.UnitOfMeasureId = ShowingOrderWithDraw_ShowingItemFilterDTO.UnitOfMeasureId;
            ShowingItemFilter.SalePrice = ShowingOrderWithDraw_ShowingItemFilterDTO.SalePrice;
            ShowingItemFilter.ERPCode = ShowingOrderWithDraw_ShowingItemFilterDTO.ERPCode;
            ShowingItemFilter.Description = ShowingOrderWithDraw_ShowingItemFilterDTO.Description;
            ShowingItemFilter.StatusId = ShowingOrderWithDraw_ShowingItemFilterDTO.StatusId;
            ShowingItemFilter.RowId = ShowingOrderWithDraw_ShowingItemFilterDTO.RowId;
            ShowingItemFilter.Search = ShowingOrderWithDraw_ShowingItemFilterDTO.Search;

            ShowingItemFilter = await ShowingItemService.ToFilter(ShowingItemFilter);
            List<ShowingItem> ShowingItems = await ShowingOrderWithDrawService.ListShowingItem(ShowingItemFilter);
            List<ShowingOrderWithDraw_ShowingItemDTO> ShowingOrderWithDraw_ShowingItemDTOs = ShowingItems
                .Select(c => new ShowingOrderWithDraw_ShowingItemDTO(c)).ToList();
            return ShowingOrderWithDraw_ShowingItemDTOs;
        }

        [Route(ShowingOrderWithDrawRoute.CountStore), HttpPost]
        public async Task<long> CountStore([FromBody] ShowingOrderWithDraw_StoreFilterDTO ShowingOrderWithDraw_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Id = ShowingOrderWithDraw_StoreFilterDTO.Id;
            StoreFilter.Code = ShowingOrderWithDraw_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ShowingOrderWithDraw_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ShowingOrderWithDraw_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ShowingOrderWithDraw_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ShowingOrderWithDraw_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ShowingOrderWithDraw_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ShowingOrderWithDraw_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ShowingOrderWithDraw_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ShowingOrderWithDraw_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ShowingOrderWithDraw_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ShowingOrderWithDraw_StoreFilterDTO.WardId;
            StoreFilter.Address = ShowingOrderWithDraw_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ShowingOrderWithDraw_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ShowingOrderWithDraw_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ShowingOrderWithDraw_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ShowingOrderWithDraw_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ShowingOrderWithDraw_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ShowingOrderWithDraw_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            return await StoreService.Count(StoreFilter);
        }

        [Route(ShowingOrderWithDrawRoute.ListStore), HttpPost]
        public async Task<List<ShowingOrderWithDraw_StoreDTO>> ListStore([FromBody] ShowingOrderWithDraw_StoreFilterDTO ShowingOrderWithDraw_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = ShowingOrderWithDraw_StoreFilterDTO.Skip;
            StoreFilter.Take = ShowingOrderWithDraw_StoreFilterDTO.Take;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ShowingOrderWithDraw_StoreFilterDTO.Id;
            StoreFilter.Code = ShowingOrderWithDraw_StoreFilterDTO.Code;
            StoreFilter.CodeDraft = ShowingOrderWithDraw_StoreFilterDTO.CodeDraft;
            StoreFilter.Name = ShowingOrderWithDraw_StoreFilterDTO.Name;
            StoreFilter.ParentStoreId = ShowingOrderWithDraw_StoreFilterDTO.ParentStoreId;
            StoreFilter.OrganizationId = ShowingOrderWithDraw_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ShowingOrderWithDraw_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ShowingOrderWithDraw_StoreFilterDTO.StoreGroupingId;
            StoreFilter.Telephone = ShowingOrderWithDraw_StoreFilterDTO.Telephone;
            StoreFilter.ProvinceId = ShowingOrderWithDraw_StoreFilterDTO.ProvinceId;
            StoreFilter.DistrictId = ShowingOrderWithDraw_StoreFilterDTO.DistrictId;
            StoreFilter.WardId = ShowingOrderWithDraw_StoreFilterDTO.WardId;
            StoreFilter.Address = ShowingOrderWithDraw_StoreFilterDTO.Address;
            StoreFilter.DeliveryAddress = ShowingOrderWithDraw_StoreFilterDTO.DeliveryAddress;
            StoreFilter.Latitude = ShowingOrderWithDraw_StoreFilterDTO.Latitude;
            StoreFilter.Longitude = ShowingOrderWithDraw_StoreFilterDTO.Longitude;
            StoreFilter.OwnerName = ShowingOrderWithDraw_StoreFilterDTO.OwnerName;
            StoreFilter.OwnerPhone = ShowingOrderWithDraw_StoreFilterDTO.OwnerPhone;
            StoreFilter.OwnerEmail = ShowingOrderWithDraw_StoreFilterDTO.OwnerEmail;
            StoreFilter.StoreStatusId = new IdFilter { Equal = StoreStatusEnum.OFFICIAL.Id };
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            StoreFilter = StoreService.ToFilter(StoreFilter);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ShowingOrderWithDraw_StoreDTO> ShowingOrderWithDraw_StoreDTOs = Stores
                .Select(x => new ShowingOrderWithDraw_StoreDTO(x)).ToList();
            return ShowingOrderWithDraw_StoreDTOs;
        }
    }
}

