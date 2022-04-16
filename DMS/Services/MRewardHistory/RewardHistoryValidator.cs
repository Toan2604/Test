using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRewardHistory
{
    public interface IRewardHistoryValidator : IServiceScoped
    {
        Task<bool> Create(RewardHistory RewardHistory);
        Task<bool> Update(RewardHistory RewardHistory);
        Task<bool> Delete(RewardHistory RewardHistory);
        Task<bool> BulkDelete(List<RewardHistory> RewardHistories);
        Task<bool> Import(List<RewardHistory> RewardHistories);
    }

    public class RewardHistoryValidator : IRewardHistoryValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            RevenueEmpty,
            RevenueNotEnough,
            LuckyNumberEmpty,
            LuckyNumberGroupingEmpty
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public RewardHistoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(RewardHistory RewardHistory)
        {
            RewardHistoryFilter RewardHistoryFilter = new RewardHistoryFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = RewardHistory.Id },
                Selects = RewardHistorySelect.Id
            };

            int count = await UOW.RewardHistoryRepository.Count(RewardHistoryFilter);
            if (count == 0)
                RewardHistory.AddError(nameof(RewardHistoryValidator), nameof(RewardHistory.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateRevenue(RewardHistory RewardHistory)
        {
            if (RewardHistory.Revenue <= 0)
            {
                RewardHistory.AddError(nameof(RewardHistoryValidator), nameof(RewardHistory.Revenue), ErrorCode.RevenueEmpty);
            }
            else if (RewardHistory.Revenue < 100000000)
            {
                RewardHistory.AddError(nameof(RewardHistoryValidator), nameof(RewardHistory.Revenue), ErrorCode.RevenueNotEnough);
            }
            return RewardHistory.IsValidated;
        }

        private async Task<bool> ValidateLuckyNumber(RewardHistory RewardHistory)
        {
            var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
            var Organizations = await UOW.OrganizationRepository.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Id | OrganizationSelect.Path
            });
            var OrganizationIds = Organizations.Where(x => CurrentUser.Organization.Path.StartsWith(x.Path)).Select(x => x.Id).ToList();

            List<LuckyNumberGrouping> LuckyNumberGroupings = await UOW.LuckyNumberGroupingRepository.List(new LuckyNumberGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = LuckyNumberGroupingSelect.Id,
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            var LuckyNumberGroupingIds = LuckyNumberGroupings.Select(x => x.Id).ToList();
            if (LuckyNumberGroupingIds.Count == 0)
            {
                RewardHistory.AddError(nameof(RewardHistoryValidator), nameof(RewardHistory.Id), ErrorCode.LuckyNumberGroupingEmpty);
            }
            else
            {
                var count = await UOW.LuckyNumberRepository.Count(new LuckyNumberFilter
                {
                    LuckyNumberGroupingId = new IdFilter { In = LuckyNumberGroupingIds },
                    RewardStatusId = new IdFilter { Equal = RewardStatusEnum.ACTIVE.Id }
                });

                if (count == 0)
                {
                    RewardHistory.AddError(nameof(RewardHistoryValidator), nameof(RewardHistory.Id), ErrorCode.LuckyNumberEmpty);
                }
            }

            return RewardHistory.IsValidated;
        }

        public async Task<bool> Create(RewardHistory RewardHistory)
        {
            await ValidateRevenue(RewardHistory);
            await ValidateLuckyNumber(RewardHistory);
            return RewardHistory.IsValidated;
        }

        public async Task<bool> Update(RewardHistory RewardHistory)
        {
            if (await ValidateId(RewardHistory))
            {
            }
            return RewardHistory.IsValidated;
        }

        public async Task<bool> Delete(RewardHistory RewardHistory)
        {
            if (await ValidateId(RewardHistory))
            {
            }
            return RewardHistory.IsValidated;
        }

        public async Task<bool> BulkDelete(List<RewardHistory> RewardHistories)
        {
            foreach (RewardHistory RewardHistory in RewardHistories)
            {
                await Delete(RewardHistory);
            }
            return RewardHistories.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<RewardHistory> RewardHistories)
        {
            return true;
        }
    }
}
