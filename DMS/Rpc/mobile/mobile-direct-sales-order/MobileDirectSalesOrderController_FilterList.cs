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
        [Route(MobileDirectSalesOrderRoute.FilterListErpApprovalState), HttpPost]
        public async Task<List<MobileDirectSalesOrder_ErpApprovalStateDTO>> FilterListErpApprovalState([FromBody] MobileDirectSalesOrder_ErpApprovalStateFilterDTO MobileDirectSalesOrder_ErpApprovalStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ErpApprovalStateFilter ErpApprovalStateFilter = new ErpApprovalStateFilter();
            ErpApprovalStateFilter.Skip = 0;
            ErpApprovalStateFilter.Take = 20;
            ErpApprovalStateFilter.OrderBy = ErpApprovalStateOrder.Id;
            ErpApprovalStateFilter.OrderType = OrderType.ASC;
            ErpApprovalStateFilter.Selects = ErpApprovalStateSelect.ALL;
            ErpApprovalStateFilter.Id = MobileDirectSalesOrder_ErpApprovalStateFilterDTO.Id;
            ErpApprovalStateFilter.Code = MobileDirectSalesOrder_ErpApprovalStateFilterDTO.Code;
            ErpApprovalStateFilter.Name = MobileDirectSalesOrder_ErpApprovalStateFilterDTO.Name;

            List<ErpApprovalState> ErpApprovalStates = await ErpApprovalStateService.List(ErpApprovalStateFilter);
            List<MobileDirectSalesOrder_ErpApprovalStateDTO> MobileDirectSalesOrder_ErpApprovalStateDTOs = ErpApprovalStates
                .Select(x => new MobileDirectSalesOrder_ErpApprovalStateDTO(x)).ToList();
            return MobileDirectSalesOrder_ErpApprovalStateDTOs;
        }

        [Route(MobileDirectSalesOrderRoute.FilterListGeneralApprovalState), HttpPost]
        public async Task<List<MobileDirectSalesOrder_GeneralApprovalStateDTO>> FilterListGeneralApprovalState([FromBody] MobileDirectSalesOrder_GeneralApprovalStateFilterDTO MobileDirectSalesOrder_GeneralApprovalStateFilterDTO)
        {
            GeneralApprovalStateFilter GeneralApprovalStateFilter = new GeneralApprovalStateFilter();
            GeneralApprovalStateFilter.Skip = MobileDirectSalesOrder_GeneralApprovalStateFilterDTO.Skip;
            GeneralApprovalStateFilter.Take = MobileDirectSalesOrder_GeneralApprovalStateFilterDTO.Take;
            GeneralApprovalStateFilter.OrderBy = GeneralApprovalStateOrder.Id;
            GeneralApprovalStateFilter.OrderType = OrderType.ASC;
            GeneralApprovalStateFilter.Selects = GeneralApprovalStateSelect.ALL;
            GeneralApprovalStateFilter.Id = MobileDirectSalesOrder_GeneralApprovalStateFilterDTO.Id;
            GeneralApprovalStateFilter.Code = MobileDirectSalesOrder_GeneralApprovalStateFilterDTO.Code;
            GeneralApprovalStateFilter.Name = MobileDirectSalesOrder_GeneralApprovalStateFilterDTO.Name;

            List<GeneralApprovalState> GeneralApprovalStates = await GeneralApprovalStateService.List(GeneralApprovalStateFilter);
            List<MobileDirectSalesOrder_GeneralApprovalStateDTO> MobileDirectSalesOrder_GeneralApprovalStateDTOs = GeneralApprovalStates
                .Select(x => new MobileDirectSalesOrder_GeneralApprovalStateDTO(x)).ToList();
            return MobileDirectSalesOrder_GeneralApprovalStateDTOs;
        }

        [Route(MobileDirectSalesOrderRoute.FilterListSaleEmployee), HttpPost]
        public async Task<List<MobileDirectSalesOrder_AppUserDTO>> FilterListSaleEmployee([FromBody] MobileDirectSalesOrder_AppUserFilterDTO MobileDirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = MobileDirectSalesOrder_AppUserFilterDTO.Id;
            AppUserFilter.Username = MobileDirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = MobileDirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = MobileDirectSalesOrder_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = MobileDirectSalesOrder_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = MobileDirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = MobileDirectSalesOrder_AppUserFilterDTO.ProvinceId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<MobileDirectSalesOrder_AppUserDTO> MobileDirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new MobileDirectSalesOrder_AppUserDTO(x)).ToList();
            return MobileDirectSalesOrder_AppUserDTOs;
        }

    }
}

