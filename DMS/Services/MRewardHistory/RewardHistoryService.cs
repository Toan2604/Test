using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MRewardHistory
{
    public interface IRewardHistoryService : IServiceScoped
    {
        Task<int> Count(RewardHistoryFilter RewardHistoryFilter);
        Task<List<RewardHistory>> List(RewardHistoryFilter RewardHistoryFilter);
        Task<RewardHistory> Get(long Id);
        Task<RewardHistory> Create(RewardHistory RewardHistory);
        Task<RewardHistory> Update(RewardHistory RewardHistory);
        Task<RewardHistory> Delete(RewardHistory RewardHistory);
        Task<List<RewardHistory>> BulkDelete(List<RewardHistory> RewardHistories);
        Task<List<RewardHistory>> Import(List<RewardHistory> RewardHistories);
        Task<RewardHistoryFilter> ToFilter(RewardHistoryFilter RewardHistoryFilter);
    }

    public class RewardHistoryService : BaseService, IRewardHistoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRewardHistoryValidator RewardHistoryValidator;
        private IRabbitManager RabbitManager;

        public RewardHistoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRewardHistoryValidator RewardHistoryValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RewardHistoryValidator = RewardHistoryValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(RewardHistoryFilter RewardHistoryFilter)
        {
            try
            {
                int result = await UOW.RewardHistoryRepository.Count(RewardHistoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return 0;
        }

        public async Task<List<RewardHistory>> List(RewardHistoryFilter RewardHistoryFilter)
        {
            try
            {
                List<RewardHistory> RewardHistorys = await UOW.RewardHistoryRepository.List(RewardHistoryFilter);
                return RewardHistorys;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }
        public async Task<RewardHistory> Get(long Id)
        {
            RewardHistory RewardHistory = await UOW.RewardHistoryRepository.Get(Id);
            if (RewardHistory == null)
                return null;
            return RewardHistory;
        }

        public async Task<RewardHistory> Create(RewardHistory RewardHistory)
        {
            if (!await RewardHistoryValidator.Create(RewardHistory))
                return RewardHistory;

            try
            {
                RewardHistory.TurnCounter = (long)(RewardHistory.Revenue / 100000000);

                await UOW.RewardHistoryRepository.Create(RewardHistory);

                RewardHistory = await UOW.RewardHistoryRepository.Get(RewardHistory.Id);
                Sync(new List<RewardHistory> { RewardHistory });
                Logging.CreateAuditLog(RewardHistory, new { }, nameof(RewardHistoryService));
                return RewardHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }

        public async Task<RewardHistory> Update(RewardHistory RewardHistory)
        {
            if (!await RewardHistoryValidator.Update(RewardHistory))
                return RewardHistory;
            try
            {
                var oldData = await UOW.RewardHistoryRepository.Get(RewardHistory.Id);


                await UOW.RewardHistoryRepository.Update(RewardHistory);


                RewardHistory = await UOW.RewardHistoryRepository.Get(RewardHistory.Id);
                Sync(new List<RewardHistory> { RewardHistory });
                Logging.CreateAuditLog(RewardHistory, oldData, nameof(RewardHistoryService));
                return RewardHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }

        public async Task<RewardHistory> Delete(RewardHistory RewardHistory)
        {
            if (!await RewardHistoryValidator.Delete(RewardHistory))
                return RewardHistory;

            try
            {

                await UOW.RewardHistoryRepository.Delete(RewardHistory);
                Sync(new List<RewardHistory> { RewardHistory });
                Logging.CreateAuditLog(new { }, RewardHistory, nameof(RewardHistoryService));
                return RewardHistory;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }

        public async Task<List<RewardHistory>> BulkDelete(List<RewardHistory> RewardHistories)
        {
            if (!await RewardHistoryValidator.BulkDelete(RewardHistories))
                return RewardHistories;

            try
            {

                await UOW.RewardHistoryRepository.BulkDelete(RewardHistories);

                Logging.CreateAuditLog(new { }, RewardHistories, nameof(RewardHistoryService));
                return RewardHistories;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }

        public async Task<List<RewardHistory>> Import(List<RewardHistory> RewardHistories)
        {
            if (!await RewardHistoryValidator.Import(RewardHistories))
                return RewardHistories;
            try
            {

                await UOW.RewardHistoryRepository.BulkMerge(RewardHistories);


                Logging.CreateAuditLog(RewardHistories, new { }, nameof(RewardHistoryService));
                return RewardHistories;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryService));
            }
            return null;
        }

        public async Task<RewardHistoryFilter> ToFilter(RewardHistoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<RewardHistoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                RewardHistoryFilter subFilter = new RewardHistoryFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterBuilder.Merge(subFilter.StoreId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.TurnCounter))
                        subFilter.TurnCounter = FilterBuilder.Merge(subFilter.TurnCounter, FilterPermissionDefinition.LongFilter);
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
        private void Sync(List<RewardHistory> RewardHistories)
        {
            List<Store> Stores = RewardHistories.Select(x => new Store { Id = x.StoreId }).Distinct().ToList();
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
        }
    }
}
