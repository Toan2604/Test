using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWorkflow
{
    public interface IWorkflowParameterValidator : IServiceScoped
    {
        Task<bool> Create(WorkflowParameter WorkflowParameter);
        Task<bool> Update(WorkflowParameter WorkflowParameter);
        Task<bool> Delete(WorkflowParameter WorkflowParameter);
        Task<bool> BulkDelete(List<WorkflowParameter> WorkflowParameters);
        Task<bool> Import(List<WorkflowParameter> WorkflowParameters);
    }

    public class WorkflowParameterValidator : IWorkflowParameterValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public WorkflowParameterValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(WorkflowParameter WorkflowParameter)
        {
            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = WorkflowParameter.Id },
                Selects = WorkflowParameterSelect.Id
            };

            int count = await UOW.WorkflowParameterRepository.Count(WorkflowParameterFilter);
            if (count == 0)
                WorkflowParameter.AddError(nameof(WorkflowParameterValidator), nameof(WorkflowParameter.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(WorkflowParameter WorkflowParameter)
        {
            return WorkflowParameter.IsValidated;
        }

        public async Task<bool> Update(WorkflowParameter WorkflowParameter)
        {
            if (await ValidateId(WorkflowParameter))
            {
            }
            return WorkflowParameter.IsValidated;
        }

        public async Task<bool> Delete(WorkflowParameter WorkflowParameter)
        {
            if (await ValidateId(WorkflowParameter))
            {
            }
            return WorkflowParameter.IsValidated;
        }

        public async Task<bool> BulkDelete(List<WorkflowParameter> WorkflowParameters)
        {
            return true;
        }

        public async Task<bool> Import(List<WorkflowParameter> WorkflowParameters)
        {
            return true;
        }
    }
}
