using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MErpApprovalState
{
    public interface IErpApprovalStateService : IServiceScoped
    {
        Task<int> Count(ErpApprovalStateFilter ErpApprovalStateFilter);
        Task<List<ErpApprovalState>> List(ErpApprovalStateFilter ErpApprovalStateFilter);
    }

    public class ErpApprovalStateService : BaseService, IErpApprovalStateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IErpApprovalStateValidator ErpApprovalStateValidator;

        public ErpApprovalStateService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IErpApprovalStateValidator ErpApprovalStateValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ErpApprovalStateValidator = ErpApprovalStateValidator;
        }
        public async Task<int> Count(ErpApprovalStateFilter ErpApprovalStateFilter)
        {
            try
            {
                int result = await UOW.ErpApprovalStateRepository.Count(ErpApprovalStateFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(ErpApprovalStateService));
            }
            return 0;
        }

        public async Task<List<ErpApprovalState>> List(ErpApprovalStateFilter ErpApprovalStateFilter)
        {
            try
            {
                List<ErpApprovalState> ErpApprovalStates = await UOW.ErpApprovalStateRepository.List(ErpApprovalStateFilter);
                return ErpApprovalStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ErpApprovalStateService));
            }
            return null;
        }
    }
}
