using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWorkflow
{
    public interface IRequestStateService : IServiceScoped
    {
        Task<int> Count(RequestStateFilter RequestStateFilter);
        Task<List<RequestState>> List(RequestStateFilter RequestStateFilter);
    }

    public class RequestStateService : BaseService, IRequestStateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public RequestStateService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(RequestStateFilter RequestStateFilter)
        {
            try
            {
                int result = await UOW.RequestStateRepository.Count(RequestStateFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDefinitionService));
            }
            return 0;
        }

        public async Task<List<RequestState>> List(RequestStateFilter RequestStateFilter)
        {
            try
            {
                List<RequestState> RequestStates = await UOW.RequestStateRepository.List(RequestStateFilter);
                return RequestStates;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDefinitionService));
            }
            return null;
        }
    }
}
