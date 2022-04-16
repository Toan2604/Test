using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWorkflow
{
    public interface IWorkflowDefinitionValidator : IServiceScoped
    {
        Task<bool> Create(WorkflowDefinition WorkflowDefinition);
        Task<bool> Update(WorkflowDefinition WorkflowDefinition);
        Task<bool> Clone(WorkflowDefinition WorkflowDefinition);
        Task<bool> CloneStep(WorkflowDefinition WorkflowDefinition);
        Task<bool> Delete(WorkflowDefinition WorkflowDefinition);
        Task<bool> BulkDelete(List<WorkflowDefinition> WorkflowDefinitions);
        Task<bool> Import(List<WorkflowDefinition> WorkflowDefinitions);
    }

    public class WorkflowDefinitionValidator : IWorkflowDefinitionValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            WorkflowDefinitionInUsed,
            CodeEmpty,
            CodeOverLength,
            ContainStepCodeOrNameOverLength,
            CodeHasSpecialCharacter,
            CodeExisted,
            NameEmpty,
            NameOverLength,
            WorkflowTypeNotExisted,
            WorkflowTypeEmpty,
            StatusNotExisted,
            EndDateInvalid,
            OrganizationNotExisted,
            StartDateEmpty,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public WorkflowDefinitionValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(WorkflowDefinition WorkflowDefinition)
        {
            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = WorkflowDefinition.Id },
                Selects = WorkflowDefinitionSelect.Id
            };

            int count = await UOW.WorkflowDefinitionRepository.Count(WorkflowDefinitionFilter);
            if (count == 0)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(WorkflowDefinition WorkflowDefinition)
        {
            var Code = WorkflowDefinition.Code;

            if (string.IsNullOrWhiteSpace(Code))
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Code), ErrorCode.CodeEmpty);
                return WorkflowDefinition.IsValidated;
            } // check code is null or empty

            if (Code.Length > 50)
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Code), ErrorCode.CodeOverLength);
                return WorkflowDefinition.IsValidated;
            } // check code length is longer than maximum in database

            if (Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Code))
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Code), ErrorCode.CodeHasSpecialCharacter);
                return WorkflowDefinition.IsValidated;
            } // check code contain empty space and special character

            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { NotEqual = WorkflowDefinition.Id },
                Code = new StringFilter { Equal = Code },
                Selects = WorkflowDefinitionSelect.Code
            };

            int count = await UOW.WorkflowDefinitionRepository.Count(WorkflowDefinitionFilter);
            if (count != 0)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Code), ErrorCode.CodeExisted); // check code existed

            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> ValidateName(WorkflowDefinition WorkflowDefinition)
        {
            if (string.IsNullOrWhiteSpace(WorkflowDefinition.Name))
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Name), ErrorCode.NameEmpty);
            }
            else if (WorkflowDefinition.Name.Length > 500)
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Name), ErrorCode.NameOverLength);
            }
            return WorkflowDefinition.IsValidated;
        }


        private async Task<bool> ValidateWorkflowType(WorkflowDefinition WorkflowDefinition)
        {
            if (WorkflowDefinition.WorkflowTypeId == 0)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.WorkflowType), ErrorCode.WorkflowTypeEmpty);
            else if (!WorkflowTypeEnum.WorkflowTypeEnumList.Any(x => x.Id == WorkflowDefinition.WorkflowTypeId))
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.WorkflowType), ErrorCode.WorkflowTypeNotExisted);
            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> ValidateStatusId(WorkflowDefinition WorkflowDefinition)
        {
            if (StatusEnum.ACTIVE.Id != WorkflowDefinition.StatusId && StatusEnum.INACTIVE.Id != WorkflowDefinition.StatusId)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Status), ErrorCode.StatusNotExisted);
            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> ValidateOrganizationId(WorkflowDefinition WorkflowDefinition)
        {
            Organization Organization = await UOW.OrganizationRepository.Get(WorkflowDefinition.OrganizationId);
            if (Organization == null)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Organization), ErrorCode.OrganizationNotExisted);
            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> WorkflowDefinitionInUsed(WorkflowDefinition WorkflowDefinition)
        {
            RequestWorkflowDefinitionMappingFilter RequestWorkflowDefinitionMappingFilter = new RequestWorkflowDefinitionMappingFilter
            {
                Skip = 0,
                Take = 1,
                WorkflowDefinitionId = new IdFilter { Equal = WorkflowDefinition.Id }
            };

            var count = await UOW.RequestWorkflowDefinitionMappingRepository.Count(RequestWorkflowDefinitionMappingFilter);
            if (count != 0)
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.Id), ErrorCode.WorkflowDefinitionInUsed);
            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> ValidateDate(WorkflowDefinition WorkflowDefinition)
        {
            if (WorkflowDefinition.StartDate.HasValue)
            {
                if (WorkflowDefinition.EndDate.HasValue)
                {
                    if (WorkflowDefinition.EndDate.Value < StaticParams.DateTimeNow || WorkflowDefinition.EndDate.Value <= WorkflowDefinition.StartDate)
                    {
                        WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.EndDate), ErrorCode.EndDateInvalid);
                    }
                }
            }
            if (!WorkflowDefinition.StartDate.HasValue)
            {
                WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.StartDate), ErrorCode.StartDateEmpty);
            }
            return WorkflowDefinition.IsValidated;
        }

        private async Task<bool> ValidateWorkflowStep(WorkflowDefinition WorkflowDefinition)
        {
            var hasError = false;
            foreach (var step in WorkflowDefinition.WorkflowSteps)
            {
                if (step.Name.Length > 500 || step.Code.Length > 50) hasError = true;
            }
            if (hasError) WorkflowDefinition.AddError(nameof(WorkflowDefinitionValidator), nameof(WorkflowDefinition.WorkflowSteps), ErrorCode.ContainStepCodeOrNameOverLength);
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> Create(WorkflowDefinition WorkflowDefinition)
        {
            await ValidateCode(WorkflowDefinition);
            await ValidateName(WorkflowDefinition);
            await ValidateWorkflowType(WorkflowDefinition);
            await ValidateDate(WorkflowDefinition);
            await ValidateStatusId(WorkflowDefinition);
            await ValidateOrganizationId(WorkflowDefinition);
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> Update(WorkflowDefinition WorkflowDefinition)
        {
            if (await ValidateId(WorkflowDefinition))
            {
                await ValidateCode(WorkflowDefinition);
                await ValidateName(WorkflowDefinition);
                await ValidateWorkflowType(WorkflowDefinition);
                await ValidateDate(WorkflowDefinition);
                await ValidateStatusId(WorkflowDefinition);
                await ValidateOrganizationId(WorkflowDefinition);
            }
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> Clone(WorkflowDefinition WorkflowDefinition)
        {
            await ValidateCode(WorkflowDefinition);
            await ValidateName(WorkflowDefinition);
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> CloneStep(WorkflowDefinition WorkflowDefinition)
        {
            if (await ValidateId(WorkflowDefinition))
            {
                await ValidateWorkflowStep(WorkflowDefinition); // validate Step Name, Code
            }
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> Delete(WorkflowDefinition WorkflowDefinition)
        {
            if (await ValidateId(WorkflowDefinition))
            {
                await WorkflowDefinitionInUsed(WorkflowDefinition);
            }
            return WorkflowDefinition.IsValidated;
        }

        public async Task<bool> BulkDelete(List<WorkflowDefinition> WorkflowDefinitions)
        {
            foreach (WorkflowDefinition WorkflowDefinition in WorkflowDefinitions)
            {
                await Delete(WorkflowDefinition);
            }
            return WorkflowDefinitions.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<WorkflowDefinition> WorkflowDefinitions)
        {
            return true;
        }
    }
}
