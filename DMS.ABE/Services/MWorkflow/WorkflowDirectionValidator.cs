using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MWorkflow
{
    public interface IWorkflowDirectionValidator : IServiceScoped
    {
        Task<bool> Create(WorkflowDirection WorkflowDirection);
        Task<bool> Update(WorkflowDirection WorkflowDirection);
        Task<bool> Delete(WorkflowDirection WorkflowDirection);
        Task<bool> BulkDelete(List<WorkflowDirection> WorkflowDirections);
        Task<bool> Import(List<WorkflowDirection> WorkflowDirections);
    }

    public class WorkflowDirectionValidator : IWorkflowDirectionValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            IdInUsed,
            FromStepNotExisted,
            FromStepEmpty,
            ToStepNotExisted,
            ToStepEmpty,
            ToStepNotSameFromStep,
            SubjectMailForCreatorOverLength,
            SubjectMailForCurrentOverLength,
            SubjectMailForNextStepOverLength,
            WorkflowDefinitionEmpty,
            WorkflowDefinitionNotExisted,
            WorkflowDirectionExisted,
            DirectionDuplicate,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public WorkflowDirectionValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(WorkflowDirection WorkflowDirection)
        {
            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = WorkflowDirection.Id },
                Selects = WorkflowDirectionSelect.Id
            };

            int count = await UOW.WorkflowDirectionRepository.Count(WorkflowDirectionFilter);
            if (count == 0)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateWorkflowDefinition(WorkflowDirection WorkflowDirection)
        {
            if (WorkflowDirection.WorkflowDefinitionId == 0)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.WorkflowDefinition), ErrorCode.WorkflowDefinitionEmpty);
            else
            {
                WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter
                {
                    Id = new IdFilter { Equal = WorkflowDirection.WorkflowDefinitionId }
                };

                int count = await UOW.WorkflowDefinitionRepository.Count(WorkflowDefinitionFilter);
                if (count == 0)
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.WorkflowDefinition), ErrorCode.WorkflowDefinitionNotExisted);
            }
            return WorkflowDirection.IsValidated;
        }

        private async Task<bool> ValidateStep(WorkflowDirection WorkflowDirection)
        {
            if (WorkflowDirection.FromStepId == 0)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.FromStep), ErrorCode.FromStepEmpty);
            else
            {
                WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = WorkflowDirection.FromStepId },
                    WorkflowDefinitionId = new IdFilter { Equal = WorkflowDirection.WorkflowDefinitionId },
                    Selects = WorkflowStepSelect.Id
                };
                int count = await UOW.WorkflowStepRepository.Count(WorkflowStepFilter);
                if (count == 0)
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.FromStep), ErrorCode.FromStepNotExisted);
            }
            if (WorkflowDirection.ToStepId == 0)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.ToStep), ErrorCode.ToStepEmpty);
            else
            {
                WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = WorkflowDirection.ToStepId },
                    WorkflowDefinitionId = new IdFilter { Equal = WorkflowDirection.WorkflowDefinitionId },
                    Selects = WorkflowStepSelect.Id
                };
                int count = await UOW.WorkflowStepRepository.Count(WorkflowStepFilter);
                if (count == 0)
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.ToStep), ErrorCode.ToStepNotExisted);
                if (WorkflowDirection.ToStepId == WorkflowDirection.FromStepId)
                {
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.ToStep), ErrorCode.ToStepNotSameFromStep);
                }
                //else
                //{
                //    var WFoldData = await UOW.WorkflowDefinitionRepository.Get(WorkflowDirection.WorkflowDefinitionId);
                //    if (WFoldData != null)
                //    {
                //        var countDirection = WFoldData.WorkflowDirections
                //            .Where(x =>
                //                x.Id != WorkflowDirection.Id &&
                //                x.FromStepId == WorkflowDirection.FromStepId &&
                //                x.ToStepId == WorkflowDirection.ToStepId &&
                //                x.StatusId == StatusEnum.ACTIVE.Id)
                //            .Count();
                //        if (countDirection != 0)
                //        {
                //            WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.Id), ErrorCode.WorkflowDirectionExisted);
                //        }
                //    }
                //}
            }
            return WorkflowDirection.IsValidated;
        }

        private async Task<bool> ValidateSubjectMail(WorkflowDirection WorkflowDirection)
        {
            if (!string.IsNullOrWhiteSpace(WorkflowDirection.SubjectMailForCreator) && WorkflowDirection.SubjectMailForCreator.Length > 255)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.SubjectMailForCreator), ErrorCode.SubjectMailForCreatorOverLength);
            if (!string.IsNullOrWhiteSpace(WorkflowDirection.SubjectMailForCreator) && WorkflowDirection.SubjectMailForCurrentStep.Length > 255)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.SubjectMailForCreator), ErrorCode.SubjectMailForCurrentOverLength);
            if (!string.IsNullOrWhiteSpace(WorkflowDirection.SubjectMailForNextStep) && WorkflowDirection.SubjectMailForNextStep.Length > 255)
                WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.SubjectMailForNextStep), ErrorCode.SubjectMailForNextStepOverLength);
            return WorkflowDirection.IsValidated;
        }

        private async Task<bool> ValidateDuplicateDirection(WorkflowDirection WorkflowDirection)
        {
            if (WorkflowDirection.WorkflowDefinitionId != 0)
            {
                WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter()
                {
                    Id = new IdFilter { NotEqual = WorkflowDirection.Id },
                    WorkflowDefinitionId = new IdFilter { Equal = WorkflowDirection.WorkflowDefinitionId },
                    FromStepId = new IdFilter { Equal = WorkflowDirection.FromStepId },
                    ToStepId = new IdFilter { Equal = WorkflowDirection.ToStepId },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };

                int count = await UOW.WorkflowDirectionRepository.Count(WorkflowDirectionFilter);
                if (count > 0)
                {
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.ToStep), ErrorCode.DirectionDuplicate);
                }
            }
            return WorkflowDirection.IsValidated;
        }

        public async Task<bool> Create(WorkflowDirection WorkflowDirection)
        {
            await ValidateWorkflowDefinition(WorkflowDirection);
            await ValidateStep(WorkflowDirection);
            await ValidateSubjectMail(WorkflowDirection);
            //await ValidateDuplicateDirection(WorkflowDirection);
            return WorkflowDirection.IsValidated;
        }

        public async Task<bool> Update(WorkflowDirection WorkflowDirection)
        {
            if (await ValidateId(WorkflowDirection))
            {
                await ValidateWorkflowDefinition(WorkflowDirection);
                await ValidateStep(WorkflowDirection);
                await ValidateSubjectMail(WorkflowDirection);
                //await ValidateDuplicateDirection(WorkflowDirection);
            }
            return WorkflowDirection.IsValidated;
        }

        public async Task<bool> Delete(WorkflowDirection WorkflowDirection)
        {
            if (await ValidateId(WorkflowDirection))
            {
                WorkflowDefinition WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(WorkflowDirection.WorkflowDefinitionId);
                if (WorkflowDefinition.Used)
                    WorkflowDirection.AddError(nameof(WorkflowDirectionValidator), nameof(WorkflowDirection.Id), ErrorCode.IdInUsed);
            }
            return WorkflowDirection.IsValidated;
        }

        public async Task<bool> BulkDelete(List<WorkflowDirection> WorkflowDirections)
        {
            foreach (WorkflowDirection WorkflowDirection in WorkflowDirections)
            {
                await Delete(WorkflowDirection);
            }
            return WorkflowDirections.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<WorkflowDirection> WorkflowDirections)
        {
            return true;
        }
    }
}
