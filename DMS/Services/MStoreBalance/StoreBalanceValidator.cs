using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS;
using DMS.Common;
using DMS.Enums;
using DMS.Entities;
using DMS.Repositories;

namespace DMS.Services.MStoreBalance
{
    public interface IStoreBalanceValidator : IServiceScoped
    {
        Task Get(StoreBalance StoreBalance);
        Task<bool> Create(StoreBalance StoreBalance);
        Task<bool> Update(StoreBalance StoreBalance);
        Task<bool> Delete(StoreBalance StoreBalance);
        Task<bool> BulkDelete(List<StoreBalance> StoreBalances);
        Task<bool> Import(List<StoreBalance> StoreBalances);
    }

    public class StoreBalanceValidator : IStoreBalanceValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private StoreBalanceMessage StoreBalanceMessage;

        public StoreBalanceValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.StoreBalanceMessage = new StoreBalanceMessage();
        }

        public async Task Get(StoreBalance StoreBalance)
        {
        }

        public async Task<bool> Create(StoreBalance StoreBalance)
        {
            await ValidateCreditAmount(StoreBalance);
            await ValidateDebitAmount(StoreBalance);
            await ValidateOrganization(StoreBalance);
            await ValidateStore(StoreBalance);
            await ValidateStoreBalance(StoreBalance);
            return StoreBalance.IsValidated;
        }

        public async Task<bool> Update(StoreBalance StoreBalance)
        {
            if (await ValidateId(StoreBalance))
            {
                await ValidateCreditAmount(StoreBalance);
                await ValidateDebitAmount(StoreBalance);
                await ValidateOrganization(StoreBalance);
                await ValidateStore(StoreBalance);
            }
            return StoreBalance.IsValidated;
        }

        public async Task<bool> Delete(StoreBalance StoreBalance)
        {
            if (await ValidateId(StoreBalance))
            {
            }
            return StoreBalance.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<StoreBalance> StoreBalances)
        {
            foreach (StoreBalance StoreBalance in StoreBalances)
            {
                await Delete(StoreBalance);
            }
            return StoreBalances.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<StoreBalance> StoreBalances)
        {
            foreach (var StoreBalance in StoreBalances)
            {
                await ValidateCreditAmount(StoreBalance);
                await ValidateDebitAmount(StoreBalance);
                await ValidateOrganization(StoreBalance);
                await ValidateStore(StoreBalance);
            }
            return StoreBalances.All(x => x.IsValidated) ? true : false;
        }

        public async Task<bool> ValidateId(StoreBalance StoreBalance)
        {
            StoreBalanceFilter StoreBalanceFilter = new StoreBalanceFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreBalance.Id },
                Selects = StoreBalanceSelect.Id
            };

            int count = await UOW.StoreBalanceRepository.CountAll(StoreBalanceFilter);
            if (count == 0)
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Id), StoreBalanceMessage.Error.IdNotExisted, StoreBalanceMessage);
            return count == 1;
        }

        public async Task<bool> ValidateCreditAmount(StoreBalance StoreBalance)
        {
            if (StoreBalance.CreditAmount == null)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.CreditAmount), StoreBalanceMessage.Error.CreditAmountEmpty);
            }
            if (StoreBalance.CreditAmount < 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.CreditAmount), StoreBalanceMessage.Error.CreditAmountInvalid);
            }
            return StoreBalance.IsValidated;
        }
        public async Task<bool> ValidateDebitAmount(StoreBalance StoreBalance)
        {
            if (StoreBalance.DebitAmount == null)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.DebitAmount), StoreBalanceMessage.Error.DebitAmountEmpty);
            }
            if (StoreBalance.DebitAmount < 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.DebitAmount), StoreBalanceMessage.Error.DebitAmountInvalid);
            }
            return StoreBalance.IsValidated;
        }
        public async Task<bool> ValidateOrganization(StoreBalance StoreBalance)
        {       
            if(StoreBalance.OrganizationId == 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Organization), StoreBalanceMessage.Error.OrganizationEmpty, StoreBalanceMessage);
                return false;
            }

            int count = await UOW.OrganizationRepository.CountAll(new OrganizationFilter
            {
                Id = new IdFilter{ Equal =  StoreBalance.OrganizationId },
            });
            if(count == 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Organization), StoreBalanceMessage.Error.OrganizationNotExisted, StoreBalanceMessage);
                return false;
            }
            if (StoreBalance.Id > 0)
            {
                var oldData = await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
                if (oldData.OrganizationId != StoreBalance.OrganizationId)
                {
                    StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Organization), StoreBalanceMessage.Error.UpdateNotAllowed, StoreBalanceMessage);
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> ValidateStore(StoreBalance StoreBalance)
        {
            if (StoreBalance.Id > 0)
            {
                var oldData = await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
                if (oldData.StoreId != StoreBalance.StoreId)
                {
                    StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Store), StoreBalanceMessage.Error.UpdateNotAllowed, StoreBalanceMessage);
                    return false;
                }
            }
            if (StoreBalance.StoreId == 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Store), StoreBalanceMessage.Error.StoreEmpty, StoreBalanceMessage);
                return false;
            }

            int count = await UOW.StoreRepository.CountAll(new StoreFilter
            {
                Id = new IdFilter{ Equal =  StoreBalance.StoreId },
            });
            if(count == 0)
            {
                StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Store), StoreBalanceMessage.Error.StoreNotExisted, StoreBalanceMessage);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateStoreBalance(StoreBalance StoreBalance)
        {
            if (StoreBalance.StoreId > 0 && StoreBalance.OrganizationId > 0)
            {
                int count = await UOW.StoreBalanceRepository.CountAll(new StoreBalanceFilter
                {
                    StoreId = new IdFilter { Equal = StoreBalance.StoreId },
                    OrganizationId = new IdFilter { Equal = StoreBalance.OrganizationId },
                });
                if (count > 0)
                {
                    StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Store), StoreBalanceMessage.Error.StoreBalanceExisted, StoreBalanceMessage);
                    StoreBalance.AddError(nameof(StoreBalanceValidator), nameof(StoreBalance.Organization), StoreBalanceMessage.Error.StoreBalanceExisted, StoreBalanceMessage);
                    return false;
                }
            }
            return true;
        }
    }
}
