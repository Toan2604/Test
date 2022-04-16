using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionStore
{
    public interface IPromotionStoreValidator : IServiceScoped
    {
        Task<bool> Create(PromotionStore PromotionStore);
        Task<bool> Update(PromotionStore PromotionStore);
        Task<bool> Delete(PromotionStore PromotionStore);
        Task<bool> BulkDelete(List<PromotionStore> PromotionStores);
        Task<bool> Import(List<PromotionStore> PromotionStores);
    }

    public class PromotionStoreValidator : IPromotionStoreValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PromotionStoreValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PromotionStore PromotionStore)
        {
            PromotionStoreFilter PromotionStoreFilter = new PromotionStoreFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PromotionStore.Id },
                Selects = PromotionStoreSelect.Id
            };

            int count = await UOW.PromotionStoreRepository.Count(PromotionStoreFilter);
            if (count == 0)
                PromotionStore.AddError(nameof(PromotionStoreValidator), nameof(PromotionStore.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PromotionStore PromotionStore)
        {
            return PromotionStore.IsValidated;
        }

        public async Task<bool> Update(PromotionStore PromotionStore)
        {
            if (await ValidateId(PromotionStore))
            {
            }
            return PromotionStore.IsValidated;
        }

        public async Task<bool> Delete(PromotionStore PromotionStore)
        {
            if (await ValidateId(PromotionStore))
            {
            }
            return PromotionStore.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PromotionStore> PromotionStores)
        {
            foreach (PromotionStore PromotionStore in PromotionStores)
            {
                await Delete(PromotionStore);
            }
            return PromotionStores.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PromotionStore> PromotionStores)
        {
            return true;
        }
    }
}
