using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionDiscountType
{
    public interface IPromotionDiscountTypeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionDiscountType PromotionDiscountType);
        Task<bool> Update(PromotionDiscountType PromotionDiscountType);
        Task<bool> Delete(PromotionDiscountType PromotionDiscountType);
        Task<bool> BulkDelete(List<PromotionDiscountType> PromotionDiscountTypes);
        Task<bool> Import(List<PromotionDiscountType> PromotionDiscountTypes);
    }

    public class PromotionDiscountTypeValidator : IPromotionDiscountTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionDiscountTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionDiscountType PromotionDiscountType)
        {
            PromotionDiscountTypeFilter PromotionDiscountTypeFilter = new PromotionDiscountTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionDiscountType.Id },
                Selects = PromotionDiscountTypeSelect.Id
            };

            int count = await UOW.PromotionDiscountTypeRepository.Count(PromotionDiscountTypeFilter);
            if (count == 0)
                PromotionDiscountType.AddError(nameof(PromotionDiscountTypeValidator), nameof(PromotionDiscountType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionDiscountType PromotionDiscountType)
        {
            return PromotionDiscountType.IsValidated;
        }

        public async Task<bool> Update(PromotionDiscountType PromotionDiscountType)
        {
            if (await ValidateId(PromotionDiscountType))
            {
            }
            return PromotionDiscountType.IsValidated;
        }

        public async Task<bool> Delete(PromotionDiscountType PromotionDiscountType)
        {
            if (await ValidateId(PromotionDiscountType))
            {
            }
            return PromotionDiscountType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionDiscountType> PromotionDiscountTypes)
        {
            foreach (PromotionDiscountType PromotionDiscountType in PromotionDiscountTypes)
            {
                await Delete(PromotionDiscountType);
            }
            return PromotionDiscountTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionDiscountType> PromotionDiscountTypes)
        {
            return true;
        }
    }
}
