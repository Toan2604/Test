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
    public interface IWorkflowDirectionService : IServiceScoped
    {
        Task<int> Count(WorkflowDirectionFilter WorkflowDirectionFilter);
        Task<List<WorkflowDirection>> List(WorkflowDirectionFilter WorkflowDirectionFilter);
        Task<WorkflowDirection> Get(long Id);
        Task<WorkflowDirection> Create(WorkflowDirection WorkflowDirection);
        Task<WorkflowDirection> Update(WorkflowDirection WorkflowDirection);
        Task<WorkflowDirection> Delete(WorkflowDirection WorkflowDirection);
        Task<List<WorkflowDirection>> BulkDelete(List<WorkflowDirection> WorkflowDirections);
        Task<List<WorkflowDirection>> Import(List<WorkflowDirection> WorkflowDirections);
        WorkflowDirectionFilter ToFilter(WorkflowDirectionFilter WorkflowDirectionFilter);
    }

    public class WorkflowDirectionService : BaseService, IWorkflowDirectionService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IWorkflowDirectionValidator WorkflowDirectionValidator;

        public WorkflowDirectionService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IWorkflowDirectionValidator WorkflowDirectionValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.WorkflowDirectionValidator = WorkflowDirectionValidator;
        }
        public async Task<int> Count(WorkflowDirectionFilter WorkflowDirectionFilter)
        {
            try
            {
                int result = await UOW.WorkflowDirectionRepository.Count(WorkflowDirectionFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return 0;
        }

        public async Task<List<WorkflowDirection>> List(WorkflowDirectionFilter WorkflowDirectionFilter)
        {
            try
            {
                List<WorkflowDirection> WorkflowDirections = await UOW.WorkflowDirectionRepository.List(WorkflowDirectionFilter);
                return WorkflowDirections;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }
        public async Task<WorkflowDirection> Get(long Id)
        {
            WorkflowDirection WorkflowDirection = await UOW.WorkflowDirectionRepository.Get(Id);
            if (WorkflowDirection == null)
                return null;
            List<WorkflowParameter> WorkflowParameters = await UOW.WorkflowParameterRepository.List(new WorkflowParameterFilter
            {
                WorkflowTypeId = new IdFilter { Equal = WorkflowDirection.WorkflowDefinitionId },
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkflowParameterSelect.ALL,
            });
            WorkflowDirection.WorkflowParameters = WorkflowParameters;
            return WorkflowDirection;
        }

        public async Task<WorkflowDirection> Create(WorkflowDirection WorkflowDirection)
        {
            if (!await WorkflowDirectionValidator.Create(WorkflowDirection))
                return WorkflowDirection;

            try
            {
                WorkflowDirection.ModifierId = CurrentContext.UserId;

                await UOW.WorkflowDirectionRepository.Create(WorkflowDirection);


                Logging.CreateAuditLog(WorkflowDirection, new { }, nameof(WorkflowDirectionService));
                return await UOW.WorkflowDirectionRepository.Get(WorkflowDirection.Id);
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }

        public async Task<WorkflowDirection> Update(WorkflowDirection WorkflowDirection)
        {
            if (!await WorkflowDirectionValidator.Update(WorkflowDirection))
                return WorkflowDirection;
            try
            {
                var oldData = await UOW.WorkflowDirectionRepository.Get(WorkflowDirection.Id);
                WorkflowDirection.ModifierId = CurrentContext.UserId;

                if (oldData.Used)
                {
                    oldData.BodyMailForCreator = WorkflowDirection.BodyMailForCreator;
                    oldData.BodyMailForCurrentStep = WorkflowDirection.BodyMailForCurrentStep;
                    oldData.BodyMailForNextStep = WorkflowDirection.BodyMailForNextStep;
                    oldData.SubjectMailForCreator = WorkflowDirection.SubjectMailForCreator;
                    oldData.SubjectMailForCurrentStep = WorkflowDirection.SubjectMailForCurrentStep;
                    oldData.SubjectMailForNextStep = WorkflowDirection.SubjectMailForNextStep;
                    oldData.ModifierId = WorkflowDirection.ModifierId;
                    await UOW.WorkflowDirectionRepository.Update(oldData);
                }
                else
                {
                    await UOW.WorkflowDirectionRepository.Update(WorkflowDirection);
                }


                var newData = await UOW.WorkflowDirectionRepository.Get(WorkflowDirection.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(WorkflowDirectionService));
                return newData;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }

        public async Task<WorkflowDirection> Delete(WorkflowDirection WorkflowDirection)
        {
            if (!await WorkflowDirectionValidator.Delete(WorkflowDirection))
                return WorkflowDirection;

            try
            {
                WorkflowDirection.ModifierId = CurrentContext.UserId;

                await UOW.WorkflowDirectionRepository.Delete(WorkflowDirection);

                Logging.CreateAuditLog(new { }, WorkflowDirection, nameof(WorkflowDirectionService));
                return WorkflowDirection;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }

        public async Task<List<WorkflowDirection>> BulkDelete(List<WorkflowDirection> WorkflowDirections)
        {
            if (!await WorkflowDirectionValidator.BulkDelete(WorkflowDirections))
                return WorkflowDirections;

            try
            {
                WorkflowDirections.ForEach(x => x.ModifierId = CurrentContext.UserId);

                await UOW.WorkflowDirectionRepository.BulkDelete(WorkflowDirections);

                Logging.CreateAuditLog(new { }, WorkflowDirections, nameof(WorkflowDirectionService));
                return WorkflowDirections;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }

        public async Task<List<WorkflowDirection>> Import(List<WorkflowDirection> WorkflowDirections)
        {
            if (!await WorkflowDirectionValidator.Import(WorkflowDirections))
                return WorkflowDirections;
            try
            {
                WorkflowDirections.ForEach(x => x.ModifierId = CurrentContext.UserId);

                await UOW.WorkflowDirectionRepository.BulkMerge(WorkflowDirections);


                Logging.CreateAuditLog(WorkflowDirections, new { }, nameof(WorkflowDirectionService));
                return WorkflowDirections;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowDirectionService));
            }
            return null;
        }

        public WorkflowDirectionFilter ToFilter(WorkflowDirectionFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<WorkflowDirectionFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                WorkflowDirectionFilter subFilter = new WorkflowDirectionFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
