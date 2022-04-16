using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Services.MStoreBalance;
using DMS.ABE.Services.MCategory;
using DMS.ABE.Services.MProduct;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Services.MStoreUser;
using DMS.ABE.Services.MStore;

namespace DMS.ABE.Rpc.store_balance
{
    public class StoreBalanceController : SimpleController
    {
        private IStoreBalanceService StoreBalanceService;
        private IStoreService StoreService;
        private ICurrentContext CurrentContext;
        private IStoreUserService StoreUserService;
        public StoreBalanceController(
            IStoreBalanceService StoreBalanceService,
            ICurrentContext CurrentContext,
            IStoreUserService StoreUserService,
            IStoreService StoreService
            )
        {
            this.StoreBalanceService = StoreBalanceService;
            this.CurrentContext = CurrentContext;
            this.StoreUserService = StoreUserService;
            this.StoreService = StoreService;
        }

        [Route(StoreBalanceRoute.Get), HttpPost]
        public async Task<ActionResult<StoreBalance_StoreBalanceDTO>> Get()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await StoreUserService.Get(StoreUserId);
            Store Store = new Store();
            if (StoreUser != null)
            {
                Store = await StoreService.Get(StoreUser.StoreId);
            }
            StoreBalance StoreBalance = (await StoreBalanceService.List(new StoreBalanceFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreBalanceSelect.ALL,
                StoreId = new IdFilter { Equal = Store.Id }
            })).FirstOrDefault();
            if (StoreBalance == null)
            {
                return BadRequest();
            }
            StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO(StoreBalance);
            StoreBalance_StoreBalanceDTO.BalanceAmount = StoreBalance_StoreBalanceDTO.DebitAmount - StoreBalance_StoreBalanceDTO.CreditAmount;
            return StoreBalance_StoreBalanceDTO;
        }
        [Route(StoreBalanceRoute.List), HttpPost]
        public async Task<ActionResult<List<StoreBalance_StoreBalanceDTO>>> List()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await StoreUserService.Get(StoreUserId);
            Store Store = new Store();
            if (StoreUser != null)
            {
                Store = await StoreService.Get(StoreUser.StoreId);
            }
            List<StoreBalance> StoreBalances = await StoreBalanceService.List(new StoreBalanceFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreBalanceSelect.ALL,
                StoreId = new IdFilter { Equal = Store.Id }
            });
            if (StoreBalances == null)
            {
                return BadRequest("Không có công nợ");
            }
            List<StoreBalance_StoreBalanceDTO> StoreBalance_StoreBalanceDTOs = StoreBalances.Select(x => new StoreBalance_StoreBalanceDTO(x)).ToList();
            foreach (var StoreBalance_StoreBalanceDTO in StoreBalance_StoreBalanceDTOs)
            {
                StoreBalance_StoreBalanceDTO.BalanceAmount = StoreBalance_StoreBalanceDTO.DebitAmount - StoreBalance_StoreBalanceDTO.CreditAmount;
            }
            return StoreBalance_StoreBalanceDTOs;
        }
    }
}
