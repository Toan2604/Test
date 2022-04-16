using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRewardStatus
{
    public interface IRewardStatusValidator : IServiceScoped
    {
        Task<bool> Create(RewardStatus RewardStatus);
        Task<bool> Update(RewardStatus RewardStatus);
        Task<bool> Delete(RewardStatus RewardStatus);
        Task<bool> BulkDelete(List<RewardStatus> RewardStatuses);
        Task<bool> Import(List<RewardStatus> RewardStatuses);
    }

    public class RewardStatusValidator : IRewardStatusValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public RewardStatusValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(RewardStatus RewardStatus)
        {
            RewardStatusFilter RewardStatusFilter = new RewardStatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = RewardStatus.Id },
                Selects = RewardStatusSelect.Id
            };

            int count = await UOW.RewardStatusRepository.Count(RewardStatusFilter);
            if (count == 0)
                RewardStatus.AddError(nameof(RewardStatusValidator), nameof(RewardStatus.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(RewardStatus RewardStatus)
        {
            return RewardStatus.IsValidated;
        }

        public async Task<bool> Update(RewardStatus RewardStatus)
        {
            if (await ValidateId(RewardStatus))
            {
            }
            return RewardStatus.IsValidated;
        }

        public async Task<bool> Delete(RewardStatus RewardStatus)
        {
            if (await ValidateId(RewardStatus))
            {
            }
            return RewardStatus.IsValidated;
        }

        public async Task<bool> BulkDelete(List<RewardStatus> RewardStatuses)
        {
            foreach (RewardStatus RewardStatus in RewardStatuses)
            {
                await Delete(RewardStatus);
            }
            return RewardStatuses.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<RewardStatus> RewardStatuses)
        {
            return true;
        }
    }
}
