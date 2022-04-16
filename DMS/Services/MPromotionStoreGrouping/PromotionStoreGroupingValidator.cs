using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionStoreGrouping
{
    public interface IPromotionStoreGroupingValidator : IServiceScoped
    {
        Task<bool> Create(PromotionStoreGrouping PromotionStoreGrouping);
        Task<bool> Update(PromotionStoreGrouping PromotionStoreGrouping);
        Task<bool> Delete(PromotionStoreGrouping PromotionStoreGrouping);
        Task<bool> BulkDelete(List<PromotionStoreGrouping> PromotionStoreGroupings);
        Task<bool> Import(List<PromotionStoreGrouping> PromotionStoreGroupings);
    }

    public class PromotionStoreGroupingValidator : IPromotionStoreGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionStoreGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionStoreGrouping PromotionStoreGrouping)
        {
            PromotionStoreGroupingFilter PromotionStoreGroupingFilter = new PromotionStoreGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionStoreGrouping.Id },
                Selects = PromotionStoreGroupingSelect.Id
            };

            int count = await UOW.PromotionStoreGroupingRepository.Count(PromotionStoreGroupingFilter);
            if (count == 0)
                PromotionStoreGrouping.AddError(nameof(PromotionStoreGroupingValidator), nameof(PromotionStoreGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionStoreGrouping PromotionStoreGrouping)
        {
            return PromotionStoreGrouping.IsValidated;
        }

        public async Task<bool> Update(PromotionStoreGrouping PromotionStoreGrouping)
        {
            if (await ValidateId(PromotionStoreGrouping))
            {
            }
            return PromotionStoreGrouping.IsValidated;
        }

        public async Task<bool> Delete(PromotionStoreGrouping PromotionStoreGrouping)
        {
            if (await ValidateId(PromotionStoreGrouping))
            {
            }
            return PromotionStoreGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionStoreGrouping> PromotionStoreGroupings)
        {
            foreach (PromotionStoreGrouping PromotionStoreGrouping in PromotionStoreGroupings)
            {
                await Delete(PromotionStoreGrouping);
            }
            return PromotionStoreGroupings.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionStoreGrouping> PromotionStoreGroupings)
        {
            return true;
        }
    }
}
