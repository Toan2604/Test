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
    public interface IWorkflowParameterService : IServiceScoped
    {
        Task<int> Count(WorkflowParameterFilter WorkflowParameterFilter);
        Task<List<WorkflowParameter>> List(WorkflowParameterFilter WorkflowParameterFilter);
    }

    public class WorkflowParameterService : BaseService, IWorkflowParameterService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public WorkflowParameterService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(WorkflowParameterFilter WorkflowParameterFilter)
        {
            try
            {
                int result = await UOW.WorkflowParameterRepository.Count(WorkflowParameterFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowParameterService));
            }
            return 0;
        }

        public async Task<List<WorkflowParameter>> List(WorkflowParameterFilter WorkflowParameterFilter)
        {
            try
            {
                List<WorkflowParameter> WorkflowParameters = await UOW.WorkflowParameterRepository.List(WorkflowParameterFilter);
                return WorkflowParameters;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowParameterService));
            }
            return null;
        }
    }
}
