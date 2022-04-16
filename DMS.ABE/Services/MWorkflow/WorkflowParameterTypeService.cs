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
    public interface IWorkflowParameterTypeService : IServiceScoped
    {
        Task<int> Count(WorkflowParameterTypeFilter WorkflowParameterTypeFilter);
        Task<List<WorkflowParameterType>> List(WorkflowParameterTypeFilter WorkflowParameterTypeFilter);
    }

    public class WorkflowParameterTypeService : BaseService, IWorkflowParameterTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public WorkflowParameterTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(WorkflowParameterTypeFilter WorkflowParameterTypeFilter)
        {
            try
            {
                int result = await UOW.WorkflowParameterTypeRepository.Count(WorkflowParameterTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowParameterTypeService));
            }
            return 0;
        }

        public async Task<List<WorkflowParameterType>> List(WorkflowParameterTypeFilter WorkflowParameterTypeFilter)
        {
            try
            {
                List<WorkflowParameterType> WorkflowParameterTypes = await UOW.WorkflowParameterTypeRepository.List(WorkflowParameterTypeFilter);
                return WorkflowParameterTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowParameterTypeService));
            }
            return null;
        }
    }
}
