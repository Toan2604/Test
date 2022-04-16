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
    public interface IWorkflowTypeService : IServiceScoped
    {
        Task<int> Count(WorkflowTypeFilter WorkflowTypeFilter);
        Task<List<WorkflowType>> List(WorkflowTypeFilter WorkflowTypeFilter);
        Task<WorkflowType> Get(long Id);
    }

    public class WorkflowTypeService : BaseService, IWorkflowTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public WorkflowTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(WorkflowTypeFilter WorkflowTypeFilter)
        {
            try
            {
                int result = await UOW.WorkflowTypeRepository.Count(WorkflowTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowTypeService));
            }
            return 0;
        }

        public async Task<List<WorkflowType>> List(WorkflowTypeFilter WorkflowTypeFilter)
        {
            try
            {
                List<WorkflowType> WorkflowTypes = await UOW.WorkflowTypeRepository.List(WorkflowTypeFilter);
                return WorkflowTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowTypeService));
            }
            return null;
        }
        public async Task<WorkflowType> Get(long Id)
        {
            WorkflowType WorkflowType = await UOW.WorkflowTypeRepository.Get(Id);
            if (WorkflowType == null)
                return null;
            return WorkflowType;
        }

    }
}
