using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRewardHistoryContent
{
    public interface IRewardHistoryContentValidator : IServiceScoped
    {
        Task<bool> Create(RewardHistoryContent RewardHistoryContent);
        Task<bool> Update(RewardHistoryContent RewardHistoryContent);
        Task<bool> Delete(RewardHistoryContent RewardHistoryContent);
        Task<bool> BulkDelete(List<RewardHistoryContent> RewardHistoryContents);
        Task<bool> Import(List<RewardHistoryContent> RewardHistoryContents);
    }

    public class RewardHistoryContentValidator : IRewardHistoryContentValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public RewardHistoryContentValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(RewardHistoryContent RewardHistoryContent)
        {
            RewardHistoryContentFilter RewardHistoryContentFilter = new RewardHistoryContentFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = RewardHistoryContent.Id },
                Selects = RewardHistoryContentSelect.Id
            };

            int count = await UOW.RewardHistoryContentRepository.Count(RewardHistoryContentFilter);
            if (count == 0)
                RewardHistoryContent.AddError(nameof(RewardHistoryContentValidator), nameof(RewardHistoryContent.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(RewardHistoryContent RewardHistoryContent)
        {
            return RewardHistoryContent.IsValidated;
        }

        public async Task<bool> Update(RewardHistoryContent RewardHistoryContent)
        {
            if (await ValidateId(RewardHistoryContent))
            {
            }
            return RewardHistoryContent.IsValidated;
        }

        public async Task<bool> Delete(RewardHistoryContent RewardHistoryContent)
        {
            if (await ValidateId(RewardHistoryContent))
            {
            }
            return RewardHistoryContent.IsValidated;
        }

        public async Task<bool> BulkDelete(List<RewardHistoryContent> RewardHistoryContents)
        {
            foreach (RewardHistoryContent RewardHistoryContent in RewardHistoryContents)
            {
                await Delete(RewardHistoryContent);
            }
            return RewardHistoryContents.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<RewardHistoryContent> RewardHistoryContents)
        {
            return true;
        }
    }
}
