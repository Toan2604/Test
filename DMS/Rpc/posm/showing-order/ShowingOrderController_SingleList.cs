using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public partial class ShowingOrderController : RpcController
    {
        [Route(ShowingOrderRoute.SingleListAppUser), HttpPost]
        public async Task<List<ShowingOrder_AppUserDTO>> SingleListAppUser([FromBody] ShowingOrder_AppUserFilterDTO ShowingOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = ShowingOrder_AppUserFilterDTO.Id;
            AppUserFilter.Username = ShowingOrder_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ShowingOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = ShowingOrder_AppUserFilterDTO.Address;
            AppUserFilter.Email = ShowingOrder_AppUserFilterDTO.Email;
            AppUserFilter.Phone = ShowingOrder_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = ShowingOrder_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = ShowingOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.PositionId = ShowingOrder_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = ShowingOrder_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = ShowingOrder_AppUserFilterDTO.OrganizationId;
            AppUserFilter.ProvinceId = ShowingOrder_AppUserFilterDTO.ProvinceId;
            AppUserFilter.StatusId = ShowingOrder_AppUserFilterDTO.StatusId;
            AppUserFilter.StatusId = new IdFilter { Equal = 1 };
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ShowingOrder_AppUserDTO> ShowingOrder_AppUserDTOs = AppUsers
                .Select(x => new ShowingOrder_AppUserDTO(x)).ToList();
            return ShowingOrder_AppUserDTOs;
        }
        [Route(ShowingOrderRoute.SingleListOrganization), HttpPost]
        public async Task<List<ShowingOrder_OrganizationDTO>> SingleListOrganization([FromBody] ShowingOrder_OrganizationFilterDTO ShowingOrder_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = ShowingOrder_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ShowingOrder_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ShowingOrder_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = ShowingOrder_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = ShowingOrder_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = ShowingOrder_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = ShowingOrder_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = ShowingOrder_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = ShowingOrder_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = ShowingOrder_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;
            OrganizationFilter.StatusId = new IdFilter { Equal = 1 };
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ShowingOrder_OrganizationDTO> ShowingOrder_OrganizationDTOs = Organizations
                .Select(x => new ShowingOrder_OrganizationDTO(x)).ToList();
            return ShowingOrder_OrganizationDTOs;
        }

        [Route(ShowingOrderRoute.SingleListStatus), HttpPost]
        public async Task<List<ShowingOrder_StatusDTO>> SingleListStatus([FromBody] ShowingOrder_StatusFilterDTO ShowingOrder_StatusFilterDTO)
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
            List<ShowingOrder_StatusDTO> ShowingOrder_StatusDTOs = Statuses
                .Select(x => new ShowingOrder_StatusDTO(x)).ToList();
            return ShowingOrder_StatusDTOs;
        }
        [Route(ShowingOrderRoute.SingleListShowingItem), HttpPost]
        public async Task<List<ShowingOrder_ShowingItemDTO>> SingleListShowingItem([FromBody] ShowingOrder_ShowingItemFilterDTO ShowingOrder_ShowingItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter();
            ShowingItemFilter.Skip = 0;
            ShowingItemFilter.Take = 20;
            ShowingItemFilter.OrderBy = ShowingItemOrder.Id;
            ShowingItemFilter.OrderType = OrderType.ASC;
            ShowingItemFilter.Selects = ShowingItemSelect.ALL;
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
            ShowingItemFilter.StatusId = new IdFilter { Equal = 1 };
            List<ShowingItem> ShowingItems = await ShowingItemService.List(ShowingItemFilter);
            List<ShowingOrder_ShowingItemDTO> ShowingOrder_ShowingItemDTOs = ShowingItems
                .Select(x => new ShowingOrder_ShowingItemDTO(x)).ToList();
            return ShowingOrder_ShowingItemDTOs;
        }
        [Route(ShowingOrderRoute.SingleListUnitOfMeasure), HttpPost]
        public async Task<List<ShowingOrder_UnitOfMeasureDTO>> SingleListUnitOfMeasure([FromBody] ShowingOrder_UnitOfMeasureFilterDTO ShowingOrder_UnitOfMeasureFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter();
            UnitOfMeasureFilter.Skip = 0;
            UnitOfMeasureFilter.Take = 20;
            UnitOfMeasureFilter.OrderBy = UnitOfMeasureOrder.Id;
            UnitOfMeasureFilter.OrderType = OrderType.ASC;
            UnitOfMeasureFilter.Selects = UnitOfMeasureSelect.ALL;
            UnitOfMeasureFilter.Id = ShowingOrder_UnitOfMeasureFilterDTO.Id;
            UnitOfMeasureFilter.Code = ShowingOrder_UnitOfMeasureFilterDTO.Code;
            UnitOfMeasureFilter.Name = ShowingOrder_UnitOfMeasureFilterDTO.Name;
            UnitOfMeasureFilter.Description = ShowingOrder_UnitOfMeasureFilterDTO.Description;
            UnitOfMeasureFilter.StatusId = ShowingOrder_UnitOfMeasureFilterDTO.StatusId;
            UnitOfMeasureFilter.StatusId = new IdFilter { Equal = 1 };
            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            List<ShowingOrder_UnitOfMeasureDTO> ShowingOrder_UnitOfMeasureDTOs = UnitOfMeasures
                .Select(x => new ShowingOrder_UnitOfMeasureDTO(x)).ToList();
            return ShowingOrder_UnitOfMeasureDTOs;
        }
    }
}

