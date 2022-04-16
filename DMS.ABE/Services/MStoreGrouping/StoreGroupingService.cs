//using DMS.ABE.Common; using TrueSight.Common;
//using DMS.ABE.Entities;
//using DMS.ABE.Helpers;
//using DMS.ABE.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace DMS.ABE.Services.MStoreGrouping
//{
//    public interface IStoreGroupingService : IServiceScoped
//    {
//        Task<int> Count(StoreGroupingFilter StoreGroupingFilter);
//        Task<List<StoreGrouping>> List(StoreGroupingFilter StoreGroupingFilter);
//        Task<StoreGrouping> Get(long Id);
//        StoreGroupingFilter ToFilter(StoreGroupingFilter StoreGroupingFilter);
//    }

//    public class StoreGroupingService : BaseService, IStoreGroupingService
//    {
//        private IUOW UOW;
//        private ILogging Logging;
//        private ICurrentContext CurrentContext;

//        public StoreGroupingService(
//            IUOW UOW,
//            ILogging Logging,
//            ICurrentContext CurrentContext
//        )
//        {
//            this.UOW = UOW;
//            this.Logging = Logging;
//            this.CurrentContext = CurrentContext;
//        }
//        public async Task<int> Count(StoreGroupingFilter StoreGroupingFilter)
//        {
//            try
//            {
//                int result = await UOW.StoreGroupingRepository.Count(StoreGroupingFilter);
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
//            }
//            return 0;
//        }

//        public async Task<List<StoreGrouping>> List(StoreGroupingFilter StoreGroupingFilter)
//        {
//            try
//            {
//                List<StoreGrouping> StoreGroupings = await UOW.StoreGroupingRepository.List(StoreGroupingFilter);
//                return StoreGroupings;
//            }
//            catch (Exception ex)
//            {
//                Logging.CreateSystemLog(ex, nameof(StoreGroupingService));
//            }
//            return null;
//        }
//        public async Task<StoreGrouping> Get(long Id)
//        {
//            StoreGrouping StoreGrouping = await UOW.StoreGroupingRepository.Get(Id);
//            if (StoreGrouping == null)
//                return null;
//            return StoreGrouping;
//        }

//        public StoreGroupingFilter ToFilter(StoreGroupingFilter filter)
//        {
//            if (filter.OrFilter == null) filter.OrFilter = new List<StoreGroupingFilter>();
//            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
//            foreach (var currentFilter in CurrentContext.Filters)
//            {
//                StoreGroupingFilter subFilter = new StoreGroupingFilter();
//                filter.OrFilter.Add(subFilter);
//                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
//                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
//                {
//                }
//            }
//            return filter;
//        }
//    }
//}
