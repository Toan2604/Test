using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreStatusHistoryTypeHistoryType
{
    public interface IStoreStatusHistoryTypeService : IServiceScoped
    {
        Task<int> Count(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter);
        Task<List<StoreStatusHistoryType>> List(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter);
    }

    public class StoreStatusHistoryTypeService : BaseService, IStoreStatusHistoryTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public StoreStatusHistoryTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter)
        {
            try
            {
                int result = await UOW.StoreStatusHistoryTypeRepository.Count(StoreStatusHistoryTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreStatusHistoryTypeService));
            }
            return 0;
        }

        public async Task<List<StoreStatusHistoryType>> List(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter)
        {
            try
            {
                List<StoreStatusHistoryType> StoreStatusHistoryTypes = await UOW.StoreStatusHistoryTypeRepository.List(StoreStatusHistoryTypeFilter);
                return StoreStatusHistoryTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreStatusHistoryTypeService));
            }
            return null;
        }
    }
}
