using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public partial class DirectSalesOrderController : SimpleController
    {
        [Route(DirectSalesOrderRoute.SingleListAppUser), HttpPost]
        public async Task<List<DirectSalesOrder_AppUserDTO>> SingleListAppUser([FromBody] DirectSalesOrder_AppUserFilterDTO DirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = DirectSalesOrder_AppUserFilterDTO.Skip;
            AppUserFilter.Take = DirectSalesOrder_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Username = DirectSalesOrder_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = DirectSalesOrder_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = DirectSalesOrder_AppUserFilterDTO.Address;
            AppUserFilter.Email = DirectSalesOrder_AppUserFilterDTO.Email;
            AppUserFilter.Phone = DirectSalesOrder_AppUserFilterDTO.Phone;
            AppUserFilter.SexId = DirectSalesOrder_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = DirectSalesOrder_AppUserFilterDTO.Birthday;
            AppUserFilter.Department = DirectSalesOrder_AppUserFilterDTO.Department;
            AppUserFilter.StatusId = DirectSalesOrder_AppUserFilterDTO.StatusId;

            AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<DirectSalesOrder_AppUserDTO> DirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new DirectSalesOrder_AppUserDTO(x)).ToList();
            return DirectSalesOrder_AppUserDTOs;
        }

        [Route(DirectSalesOrderRoute.ListAppUser), HttpPost]
        public async Task<List<DirectSalesOrder_AppUserDTO>> ListAppUser([FromBody] DirectSalesOrder_AppUserFilterDTO DirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            AppUserFilter AppUserFilter = new AppUserFilter
            {
                Skip = DirectSalesOrder_AppUserFilterDTO.Skip,
                Take = DirectSalesOrder_AppUserFilterDTO.Take,
                OrderBy = AppUserOrder.Id,
                OrderType = OrderType.ASC,
                Selects = AppUserSelect.ALL,
                Username = DirectSalesOrder_AppUserFilterDTO.Username,
                DisplayName = DirectSalesOrder_AppUserFilterDTO.DisplayName,
                StatusId = DirectSalesOrder_AppUserFilterDTO.StatusId
            };

            AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<DirectSalesOrder_AppUserDTO> DirectSalesOrder_AppUserDTOs = AppUsers
                .Select(x => new DirectSalesOrder_AppUserDTO(x)).ToList();
            return DirectSalesOrder_AppUserDTOs;
        }

        [Route(DirectSalesOrderRoute.CountAppUser), HttpPost]
        public async Task<int> CountAppUser([FromBody] DirectSalesOrder_AppUserFilterDTO DirectSalesOrder_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            AppUserFilter AppUserFilter = new AppUserFilter
            {
                Skip = DirectSalesOrder_AppUserFilterDTO.Skip,
                Take = DirectSalesOrder_AppUserFilterDTO.Take,
                OrderBy = AppUserOrder.Id,
                OrderType = OrderType.ASC,
                Selects = AppUserSelect.ALL,
                Username = DirectSalesOrder_AppUserFilterDTO.Username,
                DisplayName = DirectSalesOrder_AppUserFilterDTO.DisplayName,
                StatusId = DirectSalesOrder_AppUserFilterDTO.StatusId
            };

            AppUserFilter = await ChangeAppUserFilterByCurrentAccount(AppUserFilter);
            int Count = await AppUserService.Count(AppUserFilter);
            return Count;
        }
        [Route(DirectSalesOrderRoute.ListErpApprovalState), HttpPost]
        public async Task<List<DirectSalesOrder_ErpApprovalStateDTO>> ListErpApprovalState([FromBody] DirectSalesOrder_ErpApprovalStateFilterDTO DirectSalesOrder_ErpApprovalStateFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ErpApprovalStateFilter ErpApprovalStateFilter = new ErpApprovalStateFilter();
            ErpApprovalStateFilter.Skip = 0;
            ErpApprovalStateFilter.Take = 20;
            ErpApprovalStateFilter.OrderBy = ErpApprovalStateOrder.Id;
            ErpApprovalStateFilter.OrderType = OrderType.ASC;
            ErpApprovalStateFilter.Selects = ErpApprovalStateSelect.ALL;
            ErpApprovalStateFilter.Id = DirectSalesOrder_ErpApprovalStateFilterDTO.Id;
            ErpApprovalStateFilter.Code = DirectSalesOrder_ErpApprovalStateFilterDTO.Code;
            ErpApprovalStateFilter.Name = DirectSalesOrder_ErpApprovalStateFilterDTO.Name;

            List<ErpApprovalState> ErpApprovalStates = await ErpApprovalStateService.List(ErpApprovalStateFilter);
            List<DirectSalesOrder_ErpApprovalStateDTO> DirectSalesOrder_ErpApprovalStateDTOs = ErpApprovalStates
                .Select(x => new DirectSalesOrder_ErpApprovalStateDTO(x)).ToList();
            return DirectSalesOrder_ErpApprovalStateDTOs;
        }

        private async Task<AppUserFilter> ChangeAppUserFilterByCurrentAccount(AppUserFilter AppUserFilter)
        {
            long StoreUserId = CurrentContext.StoreUserId;
            List<StoreUser> StoreUsers = await StoreUserService.List(new StoreUserFilter
            {
                Id = new IdFilter { Equal = StoreUserId },
                Selects = StoreUserSelect.Id | StoreUserSelect.Store
            });
            StoreUser StoreUser = StoreUsers.FirstOrDefault();
            List<Store> Stores = await StoreService.List(new StoreFilter
            {
                Id = new IdFilter { Equal = StoreUser.Store.Id },
                Selects = StoreSelect.Id | StoreSelect.StoreAppUserMappings | StoreSelect.Organization
            }); // lay ra pham vi di tuyen + ou cua cua hang
            Store Store = Stores.FirstOrDefault();
            if (Store.StoreAppUserMappings.Count == 0)
            {
                AppUserFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
            } // neu khong co pham vi di tuyen, filter AU theo Id OU
            else
            {
                List<long> AppUserIds = Store.StoreAppUserMappings.Select(x => x.AppUserId).ToList();
                AppUserFilter.Id = new IdFilter
                {
                    In = AppUserIds
                };
            } // neu co pham vi di tuyen, lay toan bo appUser theo pham vi di tuyen
            return AppUserFilter;
        }

    }
}

