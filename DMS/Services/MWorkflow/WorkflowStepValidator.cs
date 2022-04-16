using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWorkflow
{
    public interface IWorkflowStepValidator : IServiceScoped
    {
        Task<bool> Create(WorkflowStep WorkflowStep);
        Task<bool> Update(WorkflowStep WorkflowStep);
        Task<bool> Delete(WorkflowStep WorkflowStep);
        Task<bool> BulkDelete(List<WorkflowStep> WorkflowSteps);
        Task<bool> Import(List<WorkflowStep> WorkflowSteps);
    }

    public class WorkflowStepValidator : IWorkflowStepValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            WorkflowDefinitionNotExisted,
            WorkflowDefinitionInUsed,
            WorkflowStepInUsed,
            CodeEmpty,
            CodeHasSpecialCharacter,
            CodeExisted,
            CodeOverLength,
            NameEmpty,
            NameOverLength,
            RoleNotExisted,
            RoleEmpty,
            SubjectMailForRejectOverLength
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public WorkflowStepValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(WorkflowStep WorkflowStep)
        {
            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = WorkflowStep.Id },
                Selects = WorkflowStepSelect.Id
            };

            int count = await UOW.WorkflowStepRepository.Count(WorkflowStepFilter);
            if (count == 0)
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateWorkflowDefinitionId(WorkflowStep WorkflowStep)
        {
            WorkflowDefinition WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(WorkflowStep.WorkflowDefinitionId);

            if (WorkflowDefinition == null)
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.WorkflowDefinition), ErrorCode.WorkflowDefinitionNotExisted);
            //else if (WorkflowDefinition.Used)
            //    WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.WorkflowDefinition), ErrorCode.WorkflowDefinitionInUsed);
            return WorkflowStep.IsValidated;
        }

        private async Task<bool> ValidateCode(WorkflowStep WorkflowStep)
        {
            if (string.IsNullOrWhiteSpace(WorkflowStep.Code))
            {
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Code), ErrorCode.CodeEmpty);
            }
            else if (WorkflowStep.Code.Length > 255)
            {
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Name), ErrorCode.CodeOverLength);
            }
            else
            {
                var Code = WorkflowStep.Code;
                if (WorkflowStep.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(WorkflowStep.Code))
                {
                    WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter
                {
                    Skip = 0,
                    Take = 10,
                    WorkflowDefinitionId = new IdFilter { Equal = WorkflowStep.WorkflowDefinitionId },
                    Id = new IdFilter { NotEqual = WorkflowStep.Id },
                    Code = new StringFilter { Equal = WorkflowStep.Code },
                    Selects = WorkflowStepSelect.Code
                };

                int count = await UOW.WorkflowStepRepository.Count(WorkflowStepFilter);
                if (count != 0)
                    WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Code), ErrorCode.CodeExisted);
            }

            return WorkflowStep.IsValidated;
        }

        private async Task<bool> ValidateName(WorkflowStep WorkflowStep)
        {
            if (string.IsNullOrWhiteSpace(WorkflowStep.Name))
            {
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Name), ErrorCode.NameEmpty);
            }
            else if (WorkflowStep.Name.Length > 255)
            {
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Name), ErrorCode.NameOverLength);
            }
            return WorkflowStep.IsValidated;
        }

        private async Task<bool> ValidateRole(WorkflowStep WorkflowStep)
        {
            if (WorkflowStep.RoleId == 0)
            {
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Role), ErrorCode.RoleEmpty);
            }
            else
            {
                RoleFilter RoleFilter = new RoleFilter
                {
                    Id = new IdFilter { Equal = WorkflowStep.RoleId }
                };

                var count = await UOW.RoleRepository.Count(RoleFilter);
                if (count == 0)
                    WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Role), ErrorCode.RoleNotExisted);
            }

            return WorkflowStep.IsValidated;
        }

        private async Task<bool> ValidateSubjectMail(WorkflowStep WorkflowStep)
        {
            if (!string.IsNullOrWhiteSpace(WorkflowStep.SubjectMailForReject) && WorkflowStep.SubjectMailForReject.Length > 255)
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.SubjectMailForReject), ErrorCode.SubjectMailForRejectOverLength);
            return WorkflowStep.IsValidated;
        }

        private async Task<bool> ValidateWorkflowStepInUsed(WorkflowStep WorkflowStep)
        {
            RequestWorkflowStepMappingFilter RequestWorkflowStepMappingFilter = new RequestWorkflowStepMappingFilter
            {
                WorkflowStepId = new IdFilter { Equal = WorkflowStep.Id }
            };

            var count = await UOW.RequestWorkflowStepMappingRepository.Count(RequestWorkflowStepMappingFilter);
            if (count != 0)
                WorkflowStep.AddError(nameof(WorkflowStepValidator), nameof(WorkflowStep.Id), ErrorCode.WorkflowStepInUsed);
            return WorkflowStep.IsValidated;
        }

        public async Task<bool> Create(WorkflowStep WorkflowStep)
        {
            await ValidateWorkflowDefinitionId(WorkflowStep);
            await ValidateCode(WorkflowStep);
            await ValidateName(WorkflowStep);
            await ValidateRole(WorkflowStep);
            await ValidateSubjectMail(WorkflowStep);
            return WorkflowStep.IsValidated;
        }

        public async Task<bool> Update(WorkflowStep WorkflowStep)
        {
            if (await ValidateId(WorkflowStep))
            {
                await ValidateWorkflowDefinitionId(WorkflowStep);
                await ValidateCode(WorkflowStep);
                await ValidateName(WorkflowStep);
                await ValidateRole(WorkflowStep);
                await ValidateSubjectMail(WorkflowStep);
            }
            return WorkflowStep.IsValidated;
        }

        public async Task<bool> Delete(WorkflowStep WorkflowStep)
        {
            if (await ValidateId(WorkflowStep))
            {
                await ValidateWorkflowStepInUsed(WorkflowStep);
            }
            return WorkflowStep.IsValidated;
        }

        public async Task<bool> BulkDelete(List<WorkflowStep> WorkflowSteps)
        {
            foreach (WorkflowStep WorkflowStep in WorkflowSteps)
            {
                await Delete(WorkflowStep);
            }
            return WorkflowSteps.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<WorkflowStep> WorkflowSteps)
        {
            return true;
        }
    }
}
