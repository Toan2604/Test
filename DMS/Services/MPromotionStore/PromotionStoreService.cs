using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionStore
{
    public interface IPromotionStoreService : IServiceScoped
    {
        Task<int> Count(PromotionStoreFilter PromotionStoreFilter);
        Task<List<PromotionStore>> List(PromotionStoreFilter PromotionStoreFilter);
        Task<PromotionStore> Get(long Id);
        Task<PromotionStore> Create(PromotionStore PromotionStore);
        Task<PromotionStore> Update(PromotionStore PromotionStore);
        Task<PromotionStore> Delete(PromotionStore PromotionStore);
        Task<List<PromotionStore>> BulkDelete(List<PromotionStore> PromotionStores);
        Task<List<PromotionStore>> Import(List<PromotionStore> PromotionStores);
        Task<PromotionStoreFilter> ToFilter(PromotionStoreFilter PromotionStoreFilter);
    }

    public class PromotionStoreService : BaseService, IPromotionStoreService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionStoreValidator PromotionStoreValidator;

        public PromotionStoreService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionStoreValidator PromotionStoreValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionStoreValidator = PromotionStoreValidator;
        }
        public async Task<int> Count(PromotionStoreFilter PromotionStoreFilter)
        {
            try
            {
                int result = await UOW.PromotionStoreRepository.Count(PromotionStoreFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return 0;
        }

        public async Task<List<PromotionStore>> List(PromotionStoreFilter PromotionStoreFilter)
        {
            try
            {
                List<PromotionStore> PromotionStores = await UOW.PromotionStoreRepository.List(PromotionStoreFilter);
                return PromotionStores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }
        public async Task<PromotionStore> Get(long Id)
        {
            PromotionStore PromotionStore = await UOW.PromotionStoreRepository.Get(Id);
            if (PromotionStore == null)
                return null;
            return PromotionStore;
        }

        public async Task<PromotionStore> Create(PromotionStore PromotionStore)
        {
            if (!await PromotionStoreValidator.Create(PromotionStore))
                return PromotionStore;

            try
            {

                await UOW.PromotionStoreRepository.Create(PromotionStore);

                PromotionStore = await UOW.PromotionStoreRepository.Get(PromotionStore.Id);
                Logging.CreateAuditLog(PromotionStore, new { }, nameof(PromotionStoreService));
                return PromotionStore;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }

        public async Task<PromotionStore> Update(PromotionStore PromotionStore)
        {
            if (!await PromotionStoreValidator.Update(PromotionStore))
                return PromotionStore;
            try
            {
                var oldData = await UOW.PromotionStoreRepository.Get(PromotionStore.Id);


                await UOW.PromotionStoreRepository.Update(PromotionStore);


                PromotionStore = await UOW.PromotionStoreRepository.Get(PromotionStore.Id);
                Logging.CreateAuditLog(PromotionStore, oldData, nameof(PromotionStoreService));
                return PromotionStore;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }

        public async Task<PromotionStore> Delete(PromotionStore PromotionStore)
        {
            if (!await PromotionStoreValidator.Delete(PromotionStore))
                return PromotionStore;

            try
            {

                await UOW.PromotionStoreRepository.Delete(PromotionStore);

                Logging.CreateAuditLog(new { }, PromotionStore, nameof(PromotionStoreService));
                return PromotionStore;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }

        public async Task<List<PromotionStore>> BulkDelete(List<PromotionStore> PromotionStores)
        {
            if (!await PromotionStoreValidator.BulkDelete(PromotionStores))
                return PromotionStores;

            try
            {

                await UOW.PromotionStoreRepository.BulkDelete(PromotionStores);

                Logging.CreateAuditLog(new { }, PromotionStores, nameof(PromotionStoreService));
                return PromotionStores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }

        public async Task<List<PromotionStore>> Import(List<PromotionStore> PromotionStores)
        {
            if (!await PromotionStoreValidator.Import(PromotionStores))
                return PromotionStores;
            try
            {

                await UOW.PromotionStoreRepository.BulkMerge(PromotionStores);


                Logging.CreateAuditLog(PromotionStores, new { }, nameof(PromotionStoreService));
                return PromotionStores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionStoreService));
            }
            return null;
        }

        public async Task<PromotionStoreFilter> ToFilter(PromotionStoreFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionStoreFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionStoreFilter subFilter = new PromotionStoreFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionPolicyId))
                        subFilter.PromotionPolicyId = FilterBuilder.Merge(subFilter.PromotionPolicyId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionId))
                        subFilter.PromotionId = FilterBuilder.Merge(subFilter.PromotionId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Note))






                        subFilter.Note = FilterBuilder.Merge(subFilter.Note, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.FromValue))


                        subFilter.FromValue = FilterBuilder.Merge(subFilter.FromValue, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.ToValue))


                        subFilter.ToValue = FilterBuilder.Merge(subFilter.ToValue, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionDiscountTypeId))
                        subFilter.PromotionDiscountTypeId = FilterBuilder.Merge(subFilter.PromotionDiscountTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.DiscountPercentage))


                        subFilter.DiscountPercentage = FilterBuilder.Merge(subFilter.DiscountPercentage, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.DiscountValue))


                        subFilter.DiscountValue = FilterBuilder.Merge(subFilter.DiscountValue, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
    }
}
