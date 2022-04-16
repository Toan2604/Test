using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProblem
{
    public interface IProblemValidator : IServiceScoped
    {
        Task<bool> Create(Problem Problem);
        Task<bool> Update(Problem Problem);
        Task<bool> Delete(Problem Problem);
        Task<bool> BulkDelete(List<Problem> Problems);
        Task<bool> Import(List<Problem> Problems);
    }

    public class ProblemValidator : IProblemValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            ProblemTypeEmpty,
            ProblemTypeNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ProblemValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Problem Problem)
        {
            ProblemFilter ProblemFilter = new ProblemFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Problem.Id },
                Selects = ProblemSelect.Id
            };

            int count = await UOW.ProblemRepository.Count(ProblemFilter);
            if (count == 0)
                Problem.AddError(nameof(ProblemValidator), nameof(Problem.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateProblemType(Problem Problem)
        {
            if (Problem.ProblemTypeId == 0)
                Problem.AddError(nameof(ProblemValidator), nameof(Problem.ProblemType), ErrorCode.ProblemTypeEmpty);
            else
            {
                ProblemTypeFilter ProblemTypeFilter = new ProblemTypeFilter
                {
                    Id = new IdFilter { Equal = Problem.ProblemTypeId },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                int count = await UOW.ProblemTypeRepository.Count(ProblemTypeFilter);
                if (count == 0)
                    Problem.AddError(nameof(ProblemValidator), nameof(Problem.ProblemType), ErrorCode.ProblemTypeNotExisted);
            }
            return Problem.IsValidated;
        }

        public async Task<bool> Create(Problem Problem)
        {
            await ValidateProblemType(Problem);
            return Problem.IsValidated;
        }

        public async Task<bool> Update(Problem Problem)
        {
            if (await ValidateId(Problem))
            {
            }
            return Problem.IsValidated;
        }

        public async Task<bool> Delete(Problem Problem)
        {
            if (await ValidateId(Problem))
            {
            }
            return Problem.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Problem> Problems)
        {
            return true;
        }

        public async Task<bool> Import(List<Problem> Problems)
        {
            return true;
        }
    }
}
