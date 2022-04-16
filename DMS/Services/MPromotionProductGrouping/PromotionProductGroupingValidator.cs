using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductGrouping
{
    public interface IPromotionProductGroupingValidator : IServiceScoped
    {
        Task<bool> Create(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> Update(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> Delete(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings);
        Task<bool> Import(List<PromotionProductGrouping> PromotionProductGroupings);
    }

    public class PromotionProductGroupingValidator : IPromotionProductGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionProductGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionProductGrouping PromotionProductGrouping)
        {
            PromotionProductGroupingFilter PromotionProductGroupingFilter = new PromotionProductGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionProductGrouping.Id },
                Selects = PromotionProductGroupingSelect.Id
            };

            int count = await UOW.PromotionProductGroupingRepository.Count(PromotionProductGroupingFilter);
            if (count == 0)
                PromotionProductGrouping.AddError(nameof(PromotionProductGroupingValidator), nameof(PromotionProductGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionProductGrouping PromotionProductGrouping)
        {
            return PromotionProductGrouping.IsValidated;
        }

        public async Task<bool> Update(PromotionProductGrouping PromotionProductGrouping)
        {
            if (await ValidateId(PromotionProductGrouping))
            {
            }
            return PromotionProductGrouping.IsValidated;
        }

        public async Task<bool> Delete(PromotionProductGrouping PromotionProductGrouping)
        {
            if (await ValidateId(PromotionProductGrouping))
            {
            }
            return PromotionProductGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            foreach (PromotionProductGrouping PromotionProductGrouping in PromotionProductGroupings)
            {
                await Delete(PromotionProductGrouping);
            }
            return PromotionProductGroupings.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            return true;
        }
    }
}
