using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public partial class ShowingOrderWithDrawController : RpcController
    {
        [Route(ShowingOrderWithDrawRoute.SingleListAppUser), HttpPost]
        public async Task<List<ShowingOrderWithDraw_AppUserDTO>> SingleListAppUser([FromBody] ShowingOrderWithDraw_AppUserFilterDTO ShowingOrderWithDraw_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ShowingOrderWithDraw_AppUserFilterDTO.Id;
            AppUserFilter.Username = ShowingOrderWithDraw_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ShowingOrderWithDraw_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ShowingOrderWithDraw_AppUserFilterDTO.Address;
            AppUserFilter.Email = ShowingOrderWithDraw_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ShowingOrderWithDraw_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = ShowingOrderWithDraw_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = ShowingOrderWithDraw_AppUserFilterDTO.Birthday;
            AppUserFilter.PositionId = ShowingOrderWithDraw_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ShowingOrderWithDraw_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = ShowingOrderWithDraw_AppUserFilterDTO.OrganizationId;
            AppUserFilter.ProvinceId = ShowingOrderWithDraw_AppUserFilterDTO.ProvinceId;
            AppUserFilter.StatusId = ShowingOrderWithDraw_AppUserFilterDTO.StatusId;
            AppUserFilter.StatusId = new IdFilter { Equal = 1 };
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ShowingOrderWithDraw_AppUserDTO> ShowingOrderWithDraw_AppUserDTOs = AppUsers
                .Select(x => new ShowingOrderWithDraw_AppUserDTO(x)).ToList();
            return ShowingOrderWithDraw_AppUserDTOs;
        }
        [Route(ShowingOrderWithDrawRoute.SingleListOrganization), HttpPost]
        public async Task<List<ShowingOrderWithDraw_OrganizationDTO>> SingleListOrganization([FromBody] ShowingOrderWithDraw_OrganizationFilterDTO ShowingOrderWithDraw_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = ShowingOrderWithDraw_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ShowingOrderWithDraw_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ShowingOrderWithDraw_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = ShowingOrderWithDraw_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = ShowingOrderWithDraw_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = ShowingOrderWithDraw_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = ShowingOrderWithDraw_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = ShowingOrderWithDraw_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = ShowingOrderWithDraw_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = ShowingOrderWithDraw_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;
            OrganizationFilter.StatusId = new IdFilter { Equal = 1 };
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ShowingOrderWithDraw_OrganizationDTO> ShowingOrderWithDraw_OrganizationDTOs = Organizations
                .Select(x => new ShowingOrderWithDraw_OrganizationDTO(x)).ToList();
            return ShowingOrderWithDraw_OrganizationDTOs;
        }

        [Route(ShowingOrderWithDrawRoute.SingleListStatus), HttpPost]
        public async Task<List<ShowingOrderWithDraw_StatusDTO>> SingleListStatus([FromBody] ShowingOrderWithDraw_StatusFilterDTO ShowingOrderWithDraw_StatusFilterDTO)
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
            List<ShowingOrderWithDraw_StatusDTO> ShowingOrderWithDraw_StatusDTOs = Statuses
                .Select(x => new ShowingOrderWithDraw_StatusDTO(x)).ToList();
            return ShowingOrderWithDraw_StatusDTOs;
        }
        [Route(ShowingOrderWithDrawRoute.SingleListShowingItem), HttpPost]
        public async Task<List<ShowingOrderWithDraw_ShowingItemDTO>> SingleListShowingItem([FromBody] ShowingOrderWithDraw_ShowingItemFilterDTO ShowingOrderWithDraw_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Skip = 0;
            ShowingItemFilter.Take = 20;
            ShowingItemFilter.OrderBy = ShowingItemOrder.Id;
            ShowingItemFilter.OrderType = OrderType.ASC;
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
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
            ShowingItemFilter.StatusId = new IdFilter { Equal = 1 };
            List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);
            List<ShowingOrderWithDraw_ShowingItemDTO> ShowingOrderWithDraw_ShowingItemDTOs = ShowingItems
                .Select(x => new ShowingOrderWithDraw_ShowingItemDTO(x)).ToList();
            return ShowingOrderWithDraw_ShowingItemDTOs;
        }
        [Route(ShowingOrderWithDrawRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<ShowingOrderWithDraw_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] ShowingOrderWithDraw_UnitOfMeasureFilterDTO ShowingOrderWithDraw_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter();
            UnitOfMeasureFilter.Skip = 0;
            UnitOfMeasureFilter.Take = 20;
            UnitOfMeasureFilter.OrderBy = UnitOfMeasureOrder.Id;
            UnitOfMeasureFilter.OrderType = OrderType.ASC;
            UnitOfMeasureFilter.Selects = UnitOfMeasureSelect.ALL;
            UnitOfMeasureFilter.Id = ShowingOrderWithDraw_UnitOfMeasureFilterDTO.Id;
            UnitOfMeasureFilter.Code = ShowingOrderWithDraw_UnitOfMeasureFilterDTO.Code;
            UnitOfMeasureFilter.Name = ShowingOrderWithDraw_UnitOfMeasureFilterDTO.Name;
            UnitOfMeasureFilter.Description = ShowingOrderWithDraw_UnitOfMeasureFilterDTO.Description;
            UnitOfMeasureFilter.StatusId = ShowingOrderWithDraw_UnitOfMeasureFilterDTO.StatusId;
            UnitOfMeasureFilter.StatusId = new IdFilter { Equal = 1 };
            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            List<ShowingOrderWithDraw_UnitOfMeasureDTO> ShowingOrderWithDraw_UnitOfMeasureDTOs = UnitOfMeasures
                .Select(x => new ShowingOrderWithDraw_UnitOfMeasureDTO(x)).ToList();
            return ShowingOrderWithDraw_UnitOfMeasureDTOs;
        }
    }
}

