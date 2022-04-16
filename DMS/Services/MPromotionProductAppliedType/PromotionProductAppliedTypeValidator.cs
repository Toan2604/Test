using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductAppliedType
{
    public interface IPromotionProductAppliedTypeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionProductAppliedType PromotionProductAppliedType);
        Task<bool> Update(PromotionProductAppliedType PromotionProductAppliedType);
        Task<bool> Delete(PromotionProductAppliedType PromotionProductAppliedType);
        Task<bool> BulkDelete(List<PromotionProductAppliedType> PromotionProductAppliedTypes);
        Task<bool> Import(List<PromotionProductAppliedType> PromotionProductAppliedTypes);
    }

    public class PromotionProductAppliedTypeValidator : IPromotionProductAppliedTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionProductAppliedTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionProductAppliedType PromotionProductAppliedType)
        {
            PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter = new PromotionProductAppliedTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionProductAppliedType.Id },
                Selects = PromotionProductAppliedTypeSelect.Id
            };

            int count = await UOW.PromotionProductAppliedTypeRepository.Count(PromotionProductAppliedTypeFilter);
            if (count == 0)
                PromotionProductAppliedType.AddError(nameof(PromotionProductAppliedTypeValidator), nameof(PromotionProductAppliedType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionProductAppliedType PromotionProductAppliedType)
        {
            return PromotionProductAppliedType.IsValidated;
        }

        public async Task<bool> Update(PromotionProductAppliedType PromotionProductAppliedType)
        {
            if (await ValidateId(PromotionProductAppliedType))
            {
            }
            return PromotionProductAppliedType.IsValidated;
        }

        public async Task<bool> Delete(PromotionProductAppliedType PromotionProductAppliedType)
        {
            if (await ValidateId(PromotionProductAppliedType))
            {
            }
            return PromotionProductAppliedType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionProductAppliedType> PromotionProductAppliedTypes)
        {
            foreach (PromotionProductAppliedType PromotionProductAppliedType in PromotionProductAppliedTypes)
            {
                await Delete(PromotionProductAppliedType);
            }
            return PromotionProductAppliedTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionProductAppliedType> PromotionProductAppliedTypes)
        {
            return true;
        }
    }
}
