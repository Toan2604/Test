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

namespace DMS.Services.MStoreGrouping
{
    public interface IStoreGroupingService : IServiceScoped
    {
        Task<int> Count(StoreGroupingFilter StoreGroupingFilter);
        Task<List<StoreGrouping>> List(StoreGroupingFilter StoreGroupingFilter);
        Task<StoreGrouping> Get(long Id);
        Task<StoreGrouping> Create(StoreGrouping StoreGrouping);
        Task<StoreGrouping> Update(StoreGrouping StoreGrouping);
        Task<StoreGrouping> Delete(StoreGrouping StoreGrouping);
        Task<List<StoreGrouping>> BulkDelete(List<StoreGrouping> StoreGroupings);
        Task<List<StoreGrouping>> BulkMerge(List<StoreGrouping> StoreGroupings);
        StoreGroupingFilter ToFilter(StoreGroupingFilter StoreGroupingFilter);
    }

    public class StoreGroupingService : BaseService, IStoreGroupingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreGroupingValidator StoreGroupingValidator;
        private IRabbitManager RabbitManager;

        public StoreGroupingService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreGroupingValidator StoreGroupingValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreGroupingValidator = StoreGroupingValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreGroupingFilter StoreGroupingFilter)
        {
            try
            {
                int result = await UOW.StoreGroupingRepository.Count(StoreGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return 0;
        }

        public async Task<List<StoreGrouping>> List(StoreGroupingFilter StoreGroupingFilter)
        {
            try
            {
                List<StoreGrouping> StoreGroupings = await UOW.StoreGroupingRepository.List(StoreGroupingFilter);
                return StoreGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }
        public async Task<StoreGrouping> Get(long Id)
        {
            StoreGrouping StoreGrouping = await UOW.StoreGroupingRepository.Get(Id);
            if (StoreGrouping == null)
                return null;
            return StoreGrouping;
        }

        public async Task<StoreGrouping> Create(StoreGrouping StoreGrouping)
        {
            if (!await StoreGroupingValidator.Create(StoreGrouping))
                return StoreGrouping;

            try
            {

                await UOW.StoreGroupingRepository.Create(StoreGrouping);


                var newData = await UOW.StoreGroupingRepository.Get(StoreGrouping.Id);
                Sync(new List<StoreGrouping> { newData });
                Logging.CreateAuditLog(StoreGrouping, new { }, nameof(StoreGroupingService));
                return await UOW.StoreGroupingRepository.Get(StoreGrouping.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }

        public async Task<StoreGrouping> Update(StoreGrouping StoreGrouping)
        {
            if (!await StoreGroupingValidator.Update(StoreGrouping))
                return StoreGrouping;
            try
            {
                var oldData = await UOW.StoreGroupingRepository.Get(StoreGrouping.Id);


                await UOW.StoreGroupingRepository.Update(StoreGrouping);


                var newData = await UOW.StoreGroupingRepository.Get(StoreGrouping.Id);
                Sync(new List<StoreGrouping> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(StoreGroupingService));
                return newData;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }

        public async Task<StoreGrouping> Delete(StoreGrouping StoreGrouping)
        {
            if (!await StoreGroupingValidator.Delete(StoreGrouping))
                return StoreGrouping;

            try
            {

                await UOW.StoreGroupingRepository.Delete(StoreGrouping);


                var newData = await UOW.StoreGroupingRepository.Get(StoreGrouping.Id);
                Sync(new List<StoreGrouping> { newData });
                Logging.CreateAuditLog(newData, StoreGrouping, nameof(StoreGroupingService));
                return StoreGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }

        public async Task<List<StoreGrouping>> BulkDelete(List<StoreGrouping> StoreGroupings)
        {
            if (!await StoreGroupingValidator.BulkDelete(StoreGroupings))
                return StoreGroupings;

            try
            {

                await UOW.StoreGroupingRepository.BulkDelete(StoreGroupings);


                var Ids = StoreGroupings.Select(x => x.Id).ToList();
                StoreGroupings = await UOW.StoreGroupingRepository.List(Ids);
                Sync(StoreGroupings);

                Logging.CreateAuditLog(new { }, StoreGroupings, nameof(StoreGroupingService));
                return StoreGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }
        public async Task<List<StoreGrouping>> BulkMerge(List<StoreGrouping> StoreGroupings)
        {
            if (!await StoreGroupingValidator.Import(StoreGroupings))
                return StoreGroupings;
            try
            {
                await UOW.StoreGroupingRepository.BulkMerge(StoreGroupings);
                List<long> Ids = StoreGroupings.Select(x => x.Id).ToList();
                StoreGroupings = await UOW.StoreGroupingRepository.List(Ids);
                Sync(StoreGroupings);

                Logging.CreateAuditLog(StoreGroupings, new { }, nameof(StoreGroupingService));
                return StoreGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
            }
            return null;
        }
        public StoreGroupingFilter ToFilter(StoreGroupingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreGroupingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreGroupingFilter subFilter = new StoreGroupingFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }

        private void Sync(List<StoreGrouping> storeGroupings)
        {
            RabbitManager.PublishList(storeGroupings, RoutingKeyEnum.StoreGroupingSync.Code);
        }
    }
}
