using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionStoreType
{
    public interface IPromotionStoreTypeValidator : IServiceScoped
    {
        Task<bool> Create(PromotionStoreType PromotionStoreType);
        Task<bool> Update(PromotionStoreType PromotionStoreType);
        Task<bool> Delete(PromotionStoreType PromotionStoreType);
        Task<bool> BulkDelete(List<PromotionStoreType> PromotionStoreTypes);
        Task<bool> Import(List<PromotionStoreType> PromotionStoreTypes);
    }

    public class PromotionStoreTypeValidator : IPromotionStoreTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionStoreTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionStoreType PromotionStoreType)
        {
            PromotionStoreTypeFilter PromotionStoreTypeFilter = new PromotionStoreTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionStoreType.Id },
                Selects = PromotionStoreTypeSelect.Id
            };

            int count = await UOW.PromotionStoreTypeRepository.Count(PromotionStoreTypeFilter);
            if (count == 0)
                PromotionStoreType.AddError(nameof(PromotionStoreTypeValidator), nameof(PromotionStoreType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionStoreType PromotionStoreType)
        {
            return PromotionStoreType.IsValidated;
        }

        public async Task<bool> Update(PromotionStoreType PromotionStoreType)
        {
            if (await ValidateId(PromotionStoreType))
            {
            }
            return PromotionStoreType.IsValidated;
        }

        public async Task<bool> Delete(PromotionStoreType PromotionStoreType)
        {
            if (await ValidateId(PromotionStoreType))
            {
            }
            return PromotionStoreType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionStoreType> PromotionStoreTypes)
        {
            foreach (PromotionStoreType PromotionStoreType in PromotionStoreTypes)
            {
                await Delete(PromotionStoreType);
            }
            return PromotionStoreTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionStoreType> PromotionStoreTypes)
        {
            return true;
        }
    }
}
