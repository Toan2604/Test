using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionSamePrice
{
    public interface IPromotionSamePriceValidator : IServiceScoped
    {
        Task<bool> Create(PromotionSamePrice PromotionSamePrice);
        Task<bool> Update(PromotionSamePrice PromotionSamePrice);
        Task<bool> Delete(PromotionSamePrice PromotionSamePrice);
        Task<bool> BulkDelete(List<PromotionSamePrice> PromotionSamePrices);
        Task<bool> Import(List<PromotionSamePrice> PromotionSamePrices);
    }

    public class PromotionSamePriceValidator : IPromotionSamePriceValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionSamePriceValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionSamePrice PromotionSamePrice)
        {
            PromotionSamePriceFilter PromotionSamePriceFilter = new PromotionSamePriceFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionSamePrice.Id },
                Selects = PromotionSamePriceSelect.Id
            };

            int count = await UOW.PromotionSamePriceRepository.Count(PromotionSamePriceFilter);
            if (count == 0)
                PromotionSamePrice.AddError(nameof(PromotionSamePriceValidator), nameof(PromotionSamePrice.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionSamePrice PromotionSamePrice)
        {
            return PromotionSamePrice.IsValidated;
        }

        public async Task<bool> Update(PromotionSamePrice PromotionSamePrice)
        {
            if (await ValidateId(PromotionSamePrice))
            {
            }
            return PromotionSamePrice.IsValidated;
        }

        public async Task<bool> Delete(PromotionSamePrice PromotionSamePrice)
        {
            if (await ValidateId(PromotionSamePrice))
            {
            }
            return PromotionSamePrice.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionSamePrice> PromotionSamePrices)
        {
            foreach (PromotionSamePrice PromotionSamePrice in PromotionSamePrices)
            {
                await Delete(PromotionSamePrice);
            }
            return PromotionSamePrices.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionSamePrice> PromotionSamePrices)
        {
            return true;
        }
    }
}
