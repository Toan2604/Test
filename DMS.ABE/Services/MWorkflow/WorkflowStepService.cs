using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MWorkflow
{
    public interface IWorkflowStepService : IServiceScoped
    {
        Task<int> Count(WorkflowStepFilter WorkflowStepFilter);
        Task<List<WorkflowStep>> List(WorkflowStepFilter WorkflowStepFilter);
        Task<WorkflowStep> Get(long Id);
        Task<WorkflowStep> Create(WorkflowStep WorkflowStep);
        Task<WorkflowStep> Update(WorkflowStep WorkflowStep);
        Task<WorkflowStep> Delete(WorkflowStep WorkflowStep);
        Task<List<WorkflowStep>> BulkDelete(List<WorkflowStep> WorkflowSteps);
        Task<List<WorkflowStep>> Import(List<WorkflowStep> WorkflowSteps);
        WorkflowStepFilter ToFilter(WorkflowStepFilter WorkflowStepFilter);
    }

    public class WorkflowStepService : BaseService, IWorkflowStepService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IWorkflowStepValidator WorkflowStepValidator;
        private IRabbitManager RabbitManager;

        public WorkflowStepService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IWorkflowStepValidator WorkflowStepValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.WorkflowStepValidator = WorkflowStepValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(WorkflowStepFilter WorkflowStepFilter)
        {
            try
            {
                int result = await UOW.WorkflowStepRepository.Count(WorkflowStepFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return 0;
        }

        public async Task<List<WorkflowStep>> List(WorkflowStepFilter WorkflowStepFilter)
        {
            try
            {
                List<WorkflowStep> WorkflowSteps = await UOW.WorkflowStepRepository.List(WorkflowStepFilter);
                return WorkflowSteps;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public async Task<WorkflowStep> Get(long Id)
        {
            WorkflowStep WorkflowStep = await UOW.WorkflowStepRepository.Get(Id);
            if (WorkflowStep == null)
                return null;
            List<WorkflowParameter> WorkflowParameters = await UOW.WorkflowParameterRepository.List(new WorkflowParameterFilter
            {
                WorkflowTypeId = new IdFilter { Equal = WorkflowStep.WorkflowDefinitionId },
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkflowParameterSelect.ALL,
            });
            WorkflowStep.WorkflowParameters = WorkflowParameters;
            return WorkflowStep;
        }

        public async Task<WorkflowStep> Create(WorkflowStep WorkflowStep)
        {
            if (!await WorkflowStepValidator.Create(WorkflowStep))
                return WorkflowStep;

            try
            {
                WorkflowStep.ModifierId = CurrentContext.UserId;

                await UOW.WorkflowStepRepository.Create(WorkflowStep);

                NotifyUsed(WorkflowStep);

                Logging.CreateAuditLog(WorkflowStep, new { }, nameof(WorkflowStepService));
                return await UOW.WorkflowStepRepository.Get(WorkflowStep.Id);
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public async Task<WorkflowStep> Update(WorkflowStep WorkflowStep)
        {
            if (!await WorkflowStepValidator.Update(WorkflowStep))
                return WorkflowStep;
            try
            {
                var oldData = await UOW.WorkflowStepRepository.Get(WorkflowStep.Id);
                WorkflowStep.ModifierId = CurrentContext.UserId;

                if (oldData.Used)
                {
                    oldData.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
                    oldData.BodyMailForReject = WorkflowStep.BodyMailForReject;
                    oldData.ModifierId = WorkflowStep.ModifierId;
                    await UOW.WorkflowStepRepository.Update(oldData);
                }
                else
                {
                    await UOW.WorkflowStepRepository.Update(WorkflowStep);
                }

                NotifyUsed(WorkflowStep);

                var newData = await UOW.WorkflowStepRepository.Get(WorkflowStep.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(WorkflowStepService));
                return newData;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public async Task<WorkflowStep> Delete(WorkflowStep WorkflowStep)
        {
            if (!await WorkflowStepValidator.Delete(WorkflowStep))
                return WorkflowStep;

            try
            {
                WorkflowStep.ModifierId = CurrentContext.UserId;

                await UOW.WorkflowStepRepository.Delete(WorkflowStep);

                Logging.CreateAuditLog(new { }, WorkflowStep, nameof(WorkflowStepService));
                return WorkflowStep;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public async Task<List<WorkflowStep>> BulkDelete(List<WorkflowStep> WorkflowSteps)
        {
            if (!await WorkflowStepValidator.BulkDelete(WorkflowSteps))
                return WorkflowSteps;

            try
            {
                WorkflowSteps.ForEach(x => x.ModifierId = CurrentContext.UserId);

                await UOW.WorkflowStepRepository.BulkDelete(WorkflowSteps);

                Logging.CreateAuditLog(new { }, WorkflowSteps, nameof(WorkflowStepService));
                return WorkflowSteps;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public async Task<List<WorkflowStep>> Import(List<WorkflowStep> WorkflowSteps)
        {
            if (!await WorkflowStepValidator.Import(WorkflowSteps))
                return WorkflowSteps;
            try
            {
                WorkflowSteps.ForEach(x => x.ModifierId = CurrentContext.UserId);

                await UOW.WorkflowStepRepository.BulkMerge(WorkflowSteps);


                Logging.CreateAuditLog(WorkflowSteps, new { }, nameof(WorkflowStepService));
                return WorkflowSteps;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(WorkflowStepService));
            }
            return null;
        }

        public WorkflowStepFilter ToFilter(WorkflowStepFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<WorkflowStepFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                WorkflowStepFilter subFilter = new WorkflowStepFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.WorkflowDefinitionId))
                        subFilter.WorkflowDefinitionId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterPermissionDefinition.StringFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RoleId))
                        subFilter.RoleId = FilterPermissionDefinition.IdFilter;
                }
            }
            return filter;
        }


        private void NotifyUsed(WorkflowStep WorkflowStep)
        {
            {
                Role RoleMessage = new Role { Id = WorkflowStep.RoleId };
                RabbitManager.PublishSingle(RoleMessage, RoutingKeyEnum.RoleUsed.Code);
            }
        }
    }
}
