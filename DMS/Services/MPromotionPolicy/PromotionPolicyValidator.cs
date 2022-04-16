using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionPolicy
{
    public interface IPromotionPolicyValidator : IServiceScoped
    {
        Task<bool> Create(PromotionPolicy PromotionPolicy);
        Task<bool> Update(PromotionPolicy PromotionPolicy);
        Task<bool> Delete(PromotionPolicy PromotionPolicy);
        Task<bool> BulkDelete(List<PromotionPolicy> PromotionPolicies);
        Task<bool> Import(List<PromotionPolicy> PromotionPolicies);
    }

    public class PromotionPolicyValidator : IPromotionPolicyValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionPolicyValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionPolicy PromotionPolicy)
        {
            PromotionPolicyFilter PromotionPolicyFilter = new PromotionPolicyFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionPolicy.Id },
                Selects = PromotionPolicySelect.Id
            };

            int count = await UOW.PromotionPolicyRepository.Count(PromotionPolicyFilter);
            if (count == 0)
                PromotionPolicy.AddError(nameof(PromotionPolicyValidator), nameof(PromotionPolicy.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionPolicy PromotionPolicy)
        {
            return PromotionPolicy.IsValidated;
        }

        public async Task<bool> Update(PromotionPolicy PromotionPolicy)
        {
            if (await ValidateId(PromotionPolicy))
            {
            }
            return PromotionPolicy.IsValidated;
        }

        public async Task<bool> Delete(PromotionPolicy PromotionPolicy)
        {
            if (await ValidateId(PromotionPolicy))
            {
            }
            return PromotionPolicy.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionPolicy> PromotionPolicies)
        {
            foreach (PromotionPolicy PromotionPolicy in PromotionPolicies)
            {
                await Delete(PromotionPolicy);
            }
            return PromotionPolicies.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionPolicy> PromotionPolicies)
        {
            return true;
        }
    }
}
