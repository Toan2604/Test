using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPriceListItemHistory
{
    public interface IPriceListItemHistoryService : IServiceScoped
    {
        Task<int> Count(PriceListItemHistoryFilter PriceListItemHistoryFilter);
        Task<List<PriceListItemHistory>> List(PriceListItemHistoryFilter PriceListItemHistoryFilter);
        Task<PriceListItemHistory> Get(long Id);
        Task<PriceListItemHistory> Create(PriceListItemHistory PriceListItemHistory);
        Task<PriceListItemHistory> Update(PriceListItemHistory PriceListItemHistory);
        Task<PriceListItemHistory> Delete(PriceListItemHistory PriceListItemHistory);
        Task<List<PriceListItemHistory>> BulkDelete(List<PriceListItemHistory> PriceListItemHistories);
        Task<List<PriceListItemHistory>> Import(List<PriceListItemHistory> PriceListItemHistories);
        Task<PriceListItemHistoryFilter> ToFilter(PriceListItemHistoryFilter PriceListItemHistoryFilter);
    }

    public class PriceListItemHistoryService : BaseService, IPriceListItemHistoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPriceListItemHistoryValidator PriceListItemHistoryValidator;

        public PriceListItemHistoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPriceListItemHistoryValidator PriceListItemHistoryValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PriceListItemHistoryValidator = PriceListItemHistoryValidator;
        }
        public async Task<int> Count(PriceListItemHistoryFilter PriceListItemHistoryFilter)
        {
            try
            {
                int result = await UOW.PriceListItemHistoryRepository.Count(PriceListItemHistoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return 0;
        }

        public async Task<List<PriceListItemHistory>> List(PriceListItemHistoryFilter PriceListItemHistoryFilter)
        {
            try
            {
                List<PriceListItemHistory> PriceListItemHistorys = await UOW.PriceListItemHistoryRepository.List(PriceListItemHistoryFilter);
                return PriceListItemHistorys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }
        public async Task<PriceListItemHistory> Get(long Id)
        {
            PriceListItemHistory PriceListItemHistory = await UOW.PriceListItemHistoryRepository.Get(Id);
            if (PriceListItemHistory == null)
                return null;
            return PriceListItemHistory;
        }

        public async Task<PriceListItemHistory> Create(PriceListItemHistory PriceListItemHistory)
        {
            if (!await PriceListItemHistoryValidator.Create(PriceListItemHistory))
                return PriceListItemHistory;

            try
            {

                await UOW.PriceListItemHistoryRepository.Create(PriceListItemHistory);

                PriceListItemHistory = await UOW.PriceListItemHistoryRepository.Get(PriceListItemHistory.Id);
                Logging.CreateAuditLog(PriceListItemHistory, new { }, nameof(PriceListItemHistoryService));
                return PriceListItemHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }

        public async Task<PriceListItemHistory> Update(PriceListItemHistory PriceListItemHistory)
        {
            if (!await PriceListItemHistoryValidator.Update(PriceListItemHistory))
                return PriceListItemHistory;
            try
            {
                var oldData = await UOW.PriceListItemHistoryRepository.Get(PriceListItemHistory.Id);


                await UOW.PriceListItemHistoryRepository.Update(PriceListItemHistory);


                PriceListItemHistory = await UOW.PriceListItemHistoryRepository.Get(PriceListItemHistory.Id);
                Logging.CreateAuditLog(PriceListItemHistory, oldData, nameof(PriceListItemHistoryService));
                return PriceListItemHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }

        public async Task<PriceListItemHistory> Delete(PriceListItemHistory PriceListItemHistory)
        {
            if (!await PriceListItemHistoryValidator.Delete(PriceListItemHistory))
                return PriceListItemHistory;

            try
            {

                await UOW.PriceListItemHistoryRepository.Delete(PriceListItemHistory);

                Logging.CreateAuditLog(new { }, PriceListItemHistory, nameof(PriceListItemHistoryService));
                return PriceListItemHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }

        public async Task<List<PriceListItemHistory>> BulkDelete(List<PriceListItemHistory> PriceListItemHistories)
        {
            if (!await PriceListItemHistoryValidator.BulkDelete(PriceListItemHistories))
                return PriceListItemHistories;

            try
            {

                await UOW.PriceListItemHistoryRepository.BulkDelete(PriceListItemHistories);

                Logging.CreateAuditLog(new { }, PriceListItemHistories, nameof(PriceListItemHistoryService));
                return PriceListItemHistories;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }

        public async Task<List<PriceListItemHistory>> Import(List<PriceListItemHistory> PriceListItemHistories)
        {
            if (!await PriceListItemHistoryValidator.Import(PriceListItemHistories))
                return PriceListItemHistories;
            try
            {

                await UOW.PriceListItemHistoryRepository.BulkMerge(PriceListItemHistories);


                Logging.CreateAuditLog(PriceListItemHistories, new { }, nameof(PriceListItemHistoryService));
                return PriceListItemHistories;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PriceListItemHistoryService));
            }
            return null;
        }

        public async Task<PriceListItemHistoryFilter> ToFilter(PriceListItemHistoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PriceListItemHistoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PriceListItemHistoryFilter subFilter = new PriceListItemHistoryFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.PriceListId))
                        subFilter.PriceListId = FilterBuilder.Merge(subFilter.PriceListId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ItemId))
                        subFilter.ItemId = FilterBuilder.Merge(subFilter.ItemId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OldPrice))
                        subFilter.OldPrice = FilterBuilder.Merge(subFilter.OldPrice, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.NewPrice))
                        subFilter.NewPrice = FilterBuilder.Merge(subFilter.NewPrice, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ModifierId))
                        subFilter.ModifierId = FilterBuilder.Merge(subFilter.ModifierId, FilterPermissionDefinition.IdFilter);
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
