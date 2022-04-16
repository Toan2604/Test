using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionType
{
    public interface IPromotionTypeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionType PromotionType);
        Task<bool> Update(PromotionType PromotionType);
        Task<bool> Delete(PromotionType PromotionType);
        Task<bool> BulkDelete(List<PromotionType> PromotionTypes);
        Task<bool> Import(List<PromotionType> PromotionTypes);
    }

    public class PromotionTypeValidator : IPromotionTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionType PromotionType)
        {
            PromotionTypeFilter PromotionTypeFilter = new PromotionTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionType.Id },
                Selects = PromotionTypeSelect.Id
            };

            int count = await UOW.PromotionTypeRepository.Count(PromotionTypeFilter);
            if (count == 0)
                PromotionType.AddError(nameof(PromotionTypeValidator), nameof(PromotionType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionType PromotionType)
        {
            return PromotionType.IsValidated;
        }

        public async Task<bool> Update(PromotionType PromotionType)
        {
            if (await ValidateId(PromotionType))
            {
            }
            return PromotionType.IsValidated;
        }

        public async Task<bool> Delete(PromotionType PromotionType)
        {
            if (await ValidateId(PromotionType))
            {
            }
            return PromotionType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionType> PromotionTypes)
        {
            foreach (PromotionType PromotionType in PromotionTypes)
            {
                await Delete(PromotionType);
            }
            return PromotionTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionType> PromotionTypes)
        {
            return true;
        }
    }
}
