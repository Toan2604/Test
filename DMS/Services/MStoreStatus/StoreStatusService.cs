using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreStatus
{
    public interface IStoreStatusService : IServiceScoped
    {
        Task<int> Count(StoreStatusFilter StoreStatusFilter);
        Task<List<StoreStatus>> List(StoreStatusFilter StoreStatusFilter);
    }

    public class StoreStatusService : BaseService, IStoreStatusService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreStatusValidator StoreStatusValidator;

        public StoreStatusService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreStatusValidator StoreStatusValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreStatusValidator = StoreStatusValidator;
        }
        public async Task<int> Count(StoreStatusFilter StoreStatusFilter)
        {
            try
            {
                int result = await UOW.StoreStatusRepository.Count(StoreStatusFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(StoreStatusService));
            }
            return 0;
        }

        public async Task<List<StoreStatus>> List(StoreStatusFilter StoreStatusFilter)
        {
            try
            {
                List<StoreStatus> StoreStatuss = await UOW.StoreStatusRepository.List(StoreStatusFilter);
                return StoreStatuss;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreStatusService));
            }
            return null;
        }
    }
}
