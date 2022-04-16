using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductType
{
    public interface IPromotionProductTypeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionProductType PromotionProductType);
        Task<bool> Update(PromotionProductType PromotionProductType);
        Task<bool> Delete(PromotionProductType PromotionProductType);
        Task<bool> BulkDelete(List<PromotionProductType> PromotionProductTypes);
        Task<bool> Import(List<PromotionProductType> PromotionProductTypes);
    }

    public class PromotionProductTypeValidator : IPromotionProductTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionProductTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionProductType PromotionProductType)
        {
            PromotionProductTypeFilter PromotionProductTypeFilter = new PromotionProductTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionProductType.Id },
                Selects = PromotionProductTypeSelect.Id
            };

            int count = await UOW.PromotionProductTypeRepository.Count(PromotionProductTypeFilter);
            if (count == 0)
                PromotionProductType.AddError(nameof(PromotionProductTypeValidator), nameof(PromotionProductType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionProductType PromotionProductType)
        {
            return PromotionProductType.IsValidated;
        }

        public async Task<bool> Update(PromotionProductType PromotionProductType)
        {
            if (await ValidateId(PromotionProductType))
            {
            }
            return PromotionProductType.IsValidated;
        }

        public async Task<bool> Delete(PromotionProductType PromotionProductType)
        {
            if (await ValidateId(PromotionProductType))
            {
            }
            return PromotionProductType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionProductType> PromotionProductTypes)
        {
            foreach (PromotionProductType PromotionProductType in PromotionProductTypes)
            {
                await Delete(PromotionProductType);
            }
            return PromotionProductTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionProductType> PromotionProductTypes)
        {
            return true;
        }
    }
}
