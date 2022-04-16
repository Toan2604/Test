using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionPolicy
{
    public interface IPromotionPolicyService : IServiceScoped
    {
        Task<int> Count(PromotionPolicyFilter PromotionPolicyFilter);
        Task<List<PromotionPolicy>> List(PromotionPolicyFilter PromotionPolicyFilter);
        Task<PromotionPromotionPolicyMapping> GetMapping(long Id, long PromotionId);
    }

    public class PromotionPolicyService : BaseService, IPromotionPolicyService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionPolicyValidator PromotionPolicyValidator;

        public PromotionPolicyService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionPolicyValidator PromotionPolicyValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionPolicyValidator = PromotionPolicyValidator;
        }
        public async Task<int> Count(PromotionPolicyFilter PromotionPolicyFilter)
        {
            try
            {
                int result = await UOW.PromotionPolicyRepository.Count(PromotionPolicyFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionPolicyService));
            }
            return 0;
        }

        public async Task<List<PromotionPolicy>> List(PromotionPolicyFilter PromotionPolicyFilter)
        {
            try
            {
                List<PromotionPolicy> PromotionPolicys = await UOW.PromotionPolicyRepository.List(PromotionPolicyFilter);
                return PromotionPolicys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionPolicyService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> GetMapping(long Id, long PromotionId)
        {
            try
            {
                PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(Id, PromotionId);
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionPolicyService));
            }
            return null;
        }
    }
}
