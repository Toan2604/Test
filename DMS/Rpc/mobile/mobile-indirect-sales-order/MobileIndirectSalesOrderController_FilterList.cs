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

        [Route(MobileIndirectSalesOrderRoute.FilterListRequestState), HttpPost]
        public async Task<List<MobileIndirectSalesOrder_RequestStateDTO>> FilterListRequestState([FromBody] MobileIndirectSalesOrder_RequestStateFilterDTO MobileIndirectSalesOrder_RequestStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RequestStateFilter RequestStateFilter = new RequestStateFilter();
            RequestStateFilter.Skip = 0;
            RequestStateFilter.Take = 20;
            RequestStateFilter.OrderBy = RequestStateOrder.Id;
            RequestStateFilter.OrderType = OrderType.ASC;
            RequestStateFilter.Selects = RequestStateSelect.ALL;
            RequestStateFilter.Id = MobileIndirectSalesOrder_RequestStateFilterDTO.Id;
            RequestStateFilter.Code = MobileIndirectSalesOrder_RequestStateFilterDTO.Code;
            RequestStateFilter.Name = MobileIndirectSalesOrder_RequestStateFilterDTO.Name;

            List<RequestState> RequestStates = await RequestStateService.List(RequestStateFilter);
            List<MobileIndirectSalesOrder_RequestStateDTO> MobileIndirectSalesOrder_RequestStateDTOs = RequestStates
                .Select(x => new MobileIndirectSalesOrder_RequestStateDTO(x)).ToList();
            return MobileIndirectSalesOrder_RequestStateDTOs;
        }

        [Route(MobileIndirectSalesOrderRoute.FilterListSaleEmployee), HttpPost]
        public async Task<List<MobileIndirectSalesOrder_AppUserDTO>> FilterListSaleEmployee([FromBody] MobileIndirectSalesOrder_AppUserFilterDTO MobileIndirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = MobileIndirectSalesOrder_AppUserFilterDTO.Id;
            AppUserFilter.Username = MobileIndirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = MobileIndirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = MobileIndirectSalesOrder_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = MobileIndirectSalesOrder_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = MobileIndirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = MobileIndirectSalesOrder_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MobileIndirectSalesOrder_AppUserDTO> MobileIndirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new MobileIndirectSalesOrder_AppUserDTO(x)).ToList();
            return MobileIndirectSalesOrder_AppUserDTOs;
        }
    }
}

