using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MStoreApprovalState
{
    public interface IStoreApprovalStateValidator : IServiceScoped
    {
        Task<bool> Create(StoreApprovalState StoreApprovalState);
        Task<bool> Update(StoreApprovalState StoreApprovalState);
        Task<bool> Delete(StoreApprovalState StoreApprovalState);
        Task<bool> BulkDelete(List<StoreApprovalState> StoreApprovalStates);
        Task<bool> Import(List<StoreApprovalState> StoreApprovalStates);
    }

    public class StoreApprovalStateValidator : IStoreApprovalStateValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreApprovalStateValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreApprovalState StoreApprovalState)
        {
            StoreApprovalStateFilter StoreApprovalStateFilter = new StoreApprovalStateFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreApprovalState.Id },
                Selects = StoreApprovalStateSelect.Id
            };

            int count = await UOW.StoreApprovalStateRepository.Count(StoreApprovalStateFilter);
            if (count == 0)
                StoreApprovalState.AddError(nameof(StoreApprovalStateValidator), nameof(StoreApprovalState.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(StoreApprovalState StoreApprovalState)
        {
            return StoreApprovalState.IsValidated;
        }

        public async Task<bool> Update(StoreApprovalState StoreApprovalState)
        {
            if (await ValidateId(StoreApprovalState))
            {
            }
            return StoreApprovalState.IsValidated;
        }

        public async Task<bool> Delete(StoreApprovalState StoreApprovalState)
        {
            if (await ValidateId(StoreApprovalState))
            {
            }
            return StoreApprovalState.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreApprovalState> StoreApprovalStates)
        {
            foreach (StoreApprovalState StoreApprovalState in StoreApprovalStates)
            {
                await Delete(StoreApprovalState);
            }
            return StoreApprovalStates.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<StoreApprovalState> StoreApprovalStates)
        {
            return true;
        }
    }
}
