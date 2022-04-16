using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MWorkflow
{
    public interface IWorkflowOperatorService : IServiceScoped
    {
        Task<int> Count(WorkflowOperatorFilter WorkflowOperatorFilter);
        Task<List<WorkflowOperator>> List(WorkflowOperatorFilter WorkflowOperatorFilter);
    }

    public class WorkflowOperatorService : BaseService, IWorkflowOperatorService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public WorkflowOperatorService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(WorkflowOperatorFilter WorkflowOperatorFilter)
        {
            try
            {
                int result = await UOW.WorkflowOperatorRepository.Count(WorkflowOperatorFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowOperatorService));
            }
            return 0;
        }

        public async Task<List<WorkflowOperator>> List(WorkflowOperatorFilter WorkflowOperatorFilter)
        {
            try
            {
                List<WorkflowOperator> WorkflowOperators = await UOW.WorkflowOperatorRepository.List(WorkflowOperatorFilter);
                return WorkflowOperators;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowOperatorService));
            }
            return null;
        }
    }
}
