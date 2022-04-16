using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreApprovalState
{
    public interface IStoreApprovalStateService : IServiceScoped
    {
        Task<int> Count(StoreApprovalStateFilter StoreApprovalStateFilter);
        Task<List<StoreApprovalState>> List(StoreApprovalStateFilter StoreApprovalStateFilter);
    }

    public class StoreApprovalStateService : BaseService, IStoreApprovalStateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreApprovalStateValidator StoreApprovalStateValidator;

        public StoreApprovalStateService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreApprovalStateValidator StoreApprovalStateValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreApprovalStateValidator = StoreApprovalStateValidator;
        }
        public async Task<int> Count(StoreApprovalStateFilter StoreApprovalStateFilter)
        {
            try
            {
                int result = await UOW.StoreApprovalStateRepository.Count(StoreApprovalStateFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(StoreApprovalStateService));
            }
            return 0;
        }

        public async Task<List<StoreApprovalState>> List(StoreApprovalStateFilter StoreApprovalStateFilter)
        {
            try
            {
                List<StoreApprovalState> StoreApprovalStates = await UOW.StoreApprovalStateRepository.List(StoreApprovalStateFilter);
                return StoreApprovalStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreApprovalStateService));
            }
            return null;
        }
    }
}
