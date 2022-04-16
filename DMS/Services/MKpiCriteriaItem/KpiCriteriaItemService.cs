using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiCriteriaItem
{
    public interface IKpiCriteriaItemService : IServiceScoped
    {
        Task<int> Count(KpiCriteriaItemFilter KpiCriteriaItemFilter);
        Task<List<KpiCriteriaItem>> List(KpiCriteriaItemFilter KpiCriteriaItemFilter);
        Task<KpiCriteriaItem> Get(long Id);
        Task<KpiCriteriaItem> Create(KpiCriteriaItem KpiCriteriaItem);
        Task<KpiCriteriaItem> Update(KpiCriteriaItem KpiCriteriaItem);
        Task<KpiCriteriaItem> Delete(KpiCriteriaItem KpiCriteriaItem);
        Task<List<KpiCriteriaItem>> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems);
        Task<List<KpiCriteriaItem>> Import(List<KpiCriteriaItem> KpiCriteriaItems);
        KpiCriteriaItemFilter ToFilter(KpiCriteriaItemFilter KpiCriteriaItemFilter);
    }

    public class KpiCriteriaItemService : BaseService, IKpiCriteriaItemService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiCriteriaItemValidator KpiCriteriaItemValidator;

        public KpiCriteriaItemService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiCriteriaItemValidator KpiCriteriaItemValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiCriteriaItemValidator = KpiCriteriaItemValidator;
        }
        public async Task<int> Count(KpiCriteriaItemFilter KpiCriteriaItemFilter)
        {
            try
            {
                int result = await UOW.KpiCriteriaItemRepository.Count(KpiCriteriaItemFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return 0;
        }

        public async Task<List<KpiCriteriaItem>> List(KpiCriteriaItemFilter KpiCriteriaItemFilter)
        {
            try
            {
                List<KpiCriteriaItem> KpiCriteriaItems = await UOW.KpiCriteriaItemRepository.List(KpiCriteriaItemFilter);
                return KpiCriteriaItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }
        public async Task<KpiCriteriaItem> Get(long Id)
        {
            KpiCriteriaItem KpiCriteriaItem = await UOW.KpiCriteriaItemRepository.Get(Id);
            if (KpiCriteriaItem == null)
                return null;
            return KpiCriteriaItem;
        }

        public async Task<KpiCriteriaItem> Create(KpiCriteriaItem KpiCriteriaItem)
        {
            if (!await KpiCriteriaItemValidator.Create(KpiCriteriaItem))
                return KpiCriteriaItem;

            try
            {
                await UOW.KpiCriteriaItemRepository.Create(KpiCriteriaItem);
                Logging.CreateAuditLog(KpiCriteriaItem, new { }, nameof(KpiCriteriaItemService));
                return await UOW.KpiCriteriaItemRepository.Get(KpiCriteriaItem.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }

        public async Task<KpiCriteriaItem> Update(KpiCriteriaItem KpiCriteriaItem)
        {
            if (!await KpiCriteriaItemValidator.Update(KpiCriteriaItem))
                return KpiCriteriaItem;
            try
            {
                var oldData = await UOW.KpiCriteriaItemRepository.Get(KpiCriteriaItem.Id);
                await UOW.KpiCriteriaItemRepository.Update(KpiCriteriaItem);
                var newData = await UOW.KpiCriteriaItemRepository.Get(KpiCriteriaItem.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(KpiCriteriaItemService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }

        public async Task<KpiCriteriaItem> Delete(KpiCriteriaItem KpiCriteriaItem)
        {
            if (!await KpiCriteriaItemValidator.Delete(KpiCriteriaItem))
                return KpiCriteriaItem;

            try
            {
                await UOW.KpiCriteriaItemRepository.Delete(KpiCriteriaItem);
                Logging.CreateAuditLog(new { }, KpiCriteriaItem, nameof(KpiCriteriaItemService));
                return KpiCriteriaItem;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }

        public async Task<List<KpiCriteriaItem>> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            if (!await KpiCriteriaItemValidator.BulkDelete(KpiCriteriaItems))
                return KpiCriteriaItems;
            try
            {

                await UOW.KpiCriteriaItemRepository.BulkDelete(KpiCriteriaItems);
                Logging.CreateAuditLog(new { }, KpiCriteriaItems, nameof(KpiCriteriaItemService));
                return KpiCriteriaItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }

        public async Task<List<KpiCriteriaItem>> Import(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            if (!await KpiCriteriaItemValidator.Import(KpiCriteriaItems))
                return KpiCriteriaItems;
            try
            {

                await UOW.KpiCriteriaItemRepository.BulkMerge(KpiCriteriaItems);
                await UOW.KpiCriteriaItemRepository.BulkMerge(KpiCriteriaItems);
                Logging.CreateAuditLog(KpiCriteriaItems, new { }, nameof(KpiCriteriaItemService));
                return KpiCriteriaItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaItemService));
            }
            return null;
        }

        public KpiCriteriaItemFilter ToFilter(KpiCriteriaItemFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiCriteriaItemFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiCriteriaItemFilter subFilter = new KpiCriteriaItemFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
