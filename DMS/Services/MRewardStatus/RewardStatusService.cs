using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRewardStatus
{
    public interface IRewardStatusService : IServiceScoped
    {
        Task<int> Count(RewardStatusFilter RewardStatusFilter);
        Task<List<RewardStatus>> List(RewardStatusFilter RewardStatusFilter);
    }

    public class RewardStatusService : BaseService, IRewardStatusService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRewardStatusValidator RewardStatusValidator;

        public RewardStatusService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRewardStatusValidator RewardStatusValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RewardStatusValidator = RewardStatusValidator;
        }
        public async Task<int> Count(RewardStatusFilter RewardStatusFilter)
        {
            try
            {
                int result = await UOW.RewardStatusRepository.Count(RewardStatusFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardStatusService));
            }
            return 0;
        }

        public async Task<List<RewardStatus>> List(RewardStatusFilter RewardStatusFilter)
        {
            try
            {
                List<RewardStatus> RewardStatuss = await UOW.RewardStatusRepository.List(RewardStatusFilter);
                return RewardStatuss;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardStatusService));
            }
            return null;
        }
    }
}
