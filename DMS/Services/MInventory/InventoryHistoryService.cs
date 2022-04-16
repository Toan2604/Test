using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MInventory
{
    public interface IInventoryHistoryService : IServiceScoped
    {
        Task<int> Count(InventoryHistoryFilter InventoryHistoryFilter);
        Task<List<InventoryHistory>> List(InventoryHistoryFilter InventoryHistoryFilter);
        Task<InventoryHistory> Get(long Id);
        InventoryHistoryFilter ToFilter(InventoryHistoryFilter InventoryHistoryFilter);
    }
    public class InventoryHistoryService : BaseService, IInventoryHistoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public InventoryHistoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(InventoryHistoryFilter InventoryHistoryFilter)
        {
            try
            {
                int result = await UOW.InventoryHistoryRepository.Count(InventoryHistoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryHistoryService));
            }
            return 0;
        }

        public async Task<List<InventoryHistory>> List(InventoryHistoryFilter InventoryHistoryFilter)
        {
            try
            {
                List<InventoryHistory> InventoryHistorys = await UOW.InventoryHistoryRepository.List(InventoryHistoryFilter);
                return InventoryHistorys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(InventoryHistoryService));
            }
            return null;
        }
        public async Task<InventoryHistory> Get(long Id)
        {
            InventoryHistory InventoryHistory = await UOW.InventoryHistoryRepository.Get(Id);
            if (InventoryHistory == null)
                return null;
            return InventoryHistory;
        }

        public InventoryHistoryFilter ToFilter(InventoryHistoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<InventoryHistoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                InventoryHistoryFilter subFilter = new InventoryHistoryFilter();
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
