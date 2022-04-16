using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MItemHistory
{
    public interface IItemHistoryService : IServiceScoped
    {
        Task<int> Count(ItemHistoryFilter ItemHistoryFilter);
        Task<List<ItemHistory>> List(ItemHistoryFilter ItemHistoryFilter);
        Task<ItemHistory> Get(long Id);
        ItemHistoryFilter ToFilter(ItemHistoryFilter ItemHistoryFilter);
    }

    public class ItemHistoryService : BaseService, IItemHistoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IItemHistoryValidator ItemHistoryValidator;

        public ItemHistoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IItemHistoryValidator ItemHistoryValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ItemHistoryValidator = ItemHistoryValidator;
        }
        public async Task<int> Count(ItemHistoryFilter ItemHistoryFilter)
        {
            try
            {
                int result = await UOW.ItemHistoryRepository.Count(ItemHistoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemHistoryService));
            }
            return 0;
        }

        public async Task<List<ItemHistory>> List(ItemHistoryFilter ItemHistoryFilter)
        {
            try
            {
                List<ItemHistory> ItemHistorys = await UOW.ItemHistoryRepository.List(ItemHistoryFilter);
                return ItemHistorys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ItemHistoryService));
            }
            return null;
        }
        public async Task<ItemHistory> Get(long Id)
        {
            ItemHistory ItemHistory = await UOW.ItemHistoryRepository.Get(Id);
            if (ItemHistory == null)
                return null;
            return ItemHistory;
        }

        public ItemHistoryFilter ToFilter(ItemHistoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ItemHistoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ItemHistoryFilter subFilter = new ItemHistoryFilter();
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
