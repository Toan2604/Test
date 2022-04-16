using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProduct
{
    public interface IPromotionProductValidator : IServiceScoped
    {
        Task<bool> Create(PromotionProduct PromotionProduct);
        Task<bool> Update(PromotionProduct PromotionProduct);
        Task<bool> Delete(PromotionProduct PromotionProduct);
        Task<bool> BulkDelete(List<PromotionProduct> PromotionProducts);
        Task<bool> Import(List<PromotionProduct> PromotionProducts);
    }

    public class PromotionProductValidator : IPromotionProductValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionProductValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionProduct PromotionProduct)
        {
            PromotionProductFilter PromotionProductFilter = new PromotionProductFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionProduct.Id },
                Selects = PromotionProductSelect.Id
            };

            int count = await UOW.PromotionProductRepository.Count(PromotionProductFilter);
            if (count == 0)
                PromotionProduct.AddError(nameof(PromotionProductValidator), nameof(PromotionProduct.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionProduct PromotionProduct)
        {
            return PromotionProduct.IsValidated;
        }

        public async Task<bool> Update(PromotionProduct PromotionProduct)
        {
            if (await ValidateId(PromotionProduct))
            {
            }
            return PromotionProduct.IsValidated;
        }

        public async Task<bool> Delete(PromotionProduct PromotionProduct)
        {
            if (await ValidateId(PromotionProduct))
            {
            }
            return PromotionProduct.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionProduct> PromotionProducts)
        {
            foreach (PromotionProduct PromotionProduct in PromotionProducts)
            {
                await Delete(PromotionProduct);
            }
            return PromotionProducts.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionProduct> PromotionProducts)
        {
            return true;
        }
    }
}
