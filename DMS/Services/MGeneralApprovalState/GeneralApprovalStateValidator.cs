using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MGeneralApprovalState
{
    public interface IGeneralApprovalStateValidator : IServiceScoped
    {
        Task<bool> Create(GeneralApprovalState GeneralApprovalState);
        Task<bool> Update(GeneralApprovalState GeneralApprovalState);
        Task<bool> Delete(GeneralApprovalState GeneralApprovalState);
        Task<bool> BulkDelete(List<GeneralApprovalState> GeneralApprovalStates);
        Task<bool> Import(List<GeneralApprovalState> GeneralApprovalStates);
    }

    public class GeneralApprovalStateValidator : IGeneralApprovalStateValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public GeneralApprovalStateValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(GeneralApprovalState GeneralApprovalState)
        {
            GeneralApprovalStateFilter GeneralApprovalStateFilter = new GeneralApprovalStateFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = GeneralApprovalState.Id },
                Selects = GeneralApprovalStateSelect.Id
            };

            int count = await UOW.GeneralApprovalStateRepository.Count(GeneralApprovalStateFilter);
            if (count == 0)
                GeneralApprovalState.AddError(nameof(GeneralApprovalStateValidator), nameof(GeneralApprovalState.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(GeneralApprovalState GeneralApprovalState)
        {
            return GeneralApprovalState.IsValidated;
        }

        public async Task<bool> Update(GeneralApprovalState GeneralApprovalState)
        {
            if (await ValidateId(GeneralApprovalState))
            {
            }
            return GeneralApprovalState.IsValidated;
        }

        public async Task<bool> Delete(GeneralApprovalState GeneralApprovalState)
        {
            if (await ValidateId(GeneralApprovalState))
            {
            }
            return GeneralApprovalState.IsValidated;
        }

        public async Task<bool> BulkDelete(List<GeneralApprovalState> GeneralApprovalStates)
        {
            foreach (GeneralApprovalState GeneralApprovalState in GeneralApprovalStates)
            {
                await Delete(GeneralApprovalState);
            }
            return GeneralApprovalStates.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<GeneralApprovalState> GeneralApprovalStates)
        {
            return true;
        }
    }
}
