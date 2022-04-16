using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MErpApprovalState
{
    public interface IErpApprovalStateValidator : IServiceScoped
    {
        Task<bool> Create(ErpApprovalState ErpApprovalState);
        Task<bool> Update(ErpApprovalState ErpApprovalState);
        Task<bool> Delete(ErpApprovalState ErpApprovalState);
        Task<bool> BulkDelete(List<ErpApprovalState> ErpApprovalStates);
        Task<bool> Import(List<ErpApprovalState> ErpApprovalStates);
    }

    public class ErpApprovalStateValidator : IErpApprovalStateValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ErpApprovalStateValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ErpApprovalState ErpApprovalState)
        {
            ErpApprovalStateFilter ErpApprovalStateFilter = new ErpApprovalStateFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ErpApprovalState.Id },
                Selects = ErpApprovalStateSelect.Id
            };

            int count = await UOW.ErpApprovalStateRepository.Count(ErpApprovalStateFilter);
            if (count == 0)
                ErpApprovalState.AddError(nameof(ErpApprovalStateValidator), nameof(ErpApprovalState.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(ErpApprovalState ErpApprovalState)
        {
            return ErpApprovalState.IsValidated;
        }

        public async Task<bool> Update(ErpApprovalState ErpApprovalState)
        {
            if (await ValidateId(ErpApprovalState))
            {
            }
            return ErpApprovalState.IsValidated;
        }

        public async Task<bool> Delete(ErpApprovalState ErpApprovalState)
        {
            if (await ValidateId(ErpApprovalState))
            {
            }
            return ErpApprovalState.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ErpApprovalState> ErpApprovalStates)
        {
            foreach (ErpApprovalState ErpApprovalState in ErpApprovalStates)
            {
                await Delete(ErpApprovalState);
            }
            return ErpApprovalStates.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ErpApprovalState> ErpApprovalStates)
        {
            return true;
        }
    }
}
