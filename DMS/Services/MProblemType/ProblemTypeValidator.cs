using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProblemType
{
    public interface IProblemTypeValidator : IServiceScoped
    {
        Task<bool> Create(ProblemType ProblemType);
        Task<bool> Update(ProblemType ProblemType);
        Task<bool> Delete(ProblemType ProblemType);
        Task<bool> BulkDelete(List<ProblemType> ProblemTypes);
        Task<bool> Import(List<ProblemType> ProblemTypes);
    }

    public class ProblemTypeValidator : IProblemTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            CodeExisted,
            NameEmpty,
            NameOverLength,
            StatusNotExisted,
            ProblemTypeInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ProblemTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ProblemType ProblemType)
        {
            ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ProblemType.Id },
                Selects = ProblemTypeSelect.Id
            };

            int count = await UOW.ProblemTypeRepository.Count(ProblemTypeFilter);
            if (count == 0)
                ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateCode(ProblemType ProblemType)
        {
            if (string.IsNullOrWhiteSpace(ProblemType.Code))
            {
                ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = ProblemType.Code;
                if (ProblemType.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(ProblemType.Code))
                {
                    ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = ProblemType.Id },
                    Code = new StringFilter { Equal = ProblemType.Code },
                    Selects = ProblemTypeSelect.Code
                };

                int count = await UOW.ProblemTypeRepository.Count(ProblemTypeFilter);
                if (count != 0)
                    ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Code), ErrorCode.CodeExisted);
            }
            return ProblemType.IsValidated;
        }
        public async Task<bool> ValidateName(ProblemType ProblemType)
        {
            if (string.IsNullOrWhiteSpace(ProblemType.Name))
            {
                ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Name), ErrorCode.NameEmpty);
            }
            else if (ProblemType.Name.Length > 255)
            {
                ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Name), ErrorCode.NameOverLength);
            }
            return ProblemType.IsValidated;
        }

        public async Task<bool> ValidateStatus(ProblemType ProblemType)
        {
            if (StatusEnum.ACTIVE.Id != ProblemType.StatusId && StatusEnum.INACTIVE.Id != ProblemType.StatusId)
                ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Status), ErrorCode.StatusNotExisted);
            return ProblemType.IsValidated;
        }

        public async Task<bool> Create(ProblemType ProblemType)
        {
            await ValidateCode(ProblemType);
            await ValidateName(ProblemType);
            await ValidateStatus(ProblemType);
            return ProblemType.IsValidated;
        }

        public async Task<bool> Update(ProblemType ProblemType)
        {
            if (await ValidateId(ProblemType))
            {
                await ValidateCode(ProblemType);
                await ValidateName(ProblemType);
                await ValidateStatus(ProblemType);
            }
            return ProblemType.IsValidated;
        }

        public async Task<bool> Delete(ProblemType ProblemType)
        {
            if (await ValidateId(ProblemType))
            {
                ProblemType = await UOW.ProblemTypeRepository.Get(ProblemType.Id);
                if (ProblemType.Used)
                {
                    ProblemType.AddError(nameof(ProblemTypeValidator), nameof(ProblemType.Id), ErrorCode.ProblemTypeInUsed);
                }
            }
            return ProblemType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ProblemType> ProblemTypes)
        {
            foreach (ProblemType ProblemType in ProblemTypes)
            {
                await Delete(ProblemType);
            }
            return ProblemTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ProblemType> ProblemTypes)
        {
            return true;
        }
    }
}
