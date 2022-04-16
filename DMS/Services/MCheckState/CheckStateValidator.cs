using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS;
using DMS.Common;
using DMS.Enums;
using DMS.Entities;
using DMS.Repositories;

namespace DMS.Services.MCheckState
{
    public interface ICheckStateValidator : IServiceScoped
    {
        Task Get(CheckState CheckState);
        Task<bool> Create(CheckState CheckState);
        Task<bool> Update(CheckState CheckState);
        Task<bool> Delete(CheckState CheckState);
        Task<bool> BulkDelete(List<CheckState> CheckStates);
        Task<bool> Import(List<CheckState> CheckStates);
    }

    public class CheckStateValidator : ICheckStateValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private CheckStateMessage CheckStateMessage;

        public CheckStateValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.CheckStateMessage = new CheckStateMessage();
        }

        public async Task Get(CheckState CheckState)
        {
        }

        public async Task<bool> Create(CheckState CheckState)
        {
            await ValidateCode(CheckState);
            await ValidateName(CheckState);
            return CheckState.IsValidated;
        }

        public async Task<bool> Update(CheckState CheckState)
        {
            if (await ValidateId(CheckState))
            {
                await ValidateCode(CheckState);
                await ValidateName(CheckState);
            }
            return CheckState.IsValidated;
        }

        public async Task<bool> Delete(CheckState CheckState)
        {
            if (await ValidateId(CheckState))
            {
            }
            return CheckState.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<CheckState> CheckStates)
        {
            foreach (CheckState CheckState in CheckStates)
            {
                await Delete(CheckState);
            }
            return CheckStates.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<CheckState> CheckStates)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(CheckState CheckState)
        {
            CheckStateFilter CheckStateFilter = new CheckStateFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = CheckState.Id },
                Selects = CheckStateSelect.Id
            };

            int count = await UOW.CheckStateRepository.CountAll(CheckStateFilter);
            if (count == 0)
                CheckState.AddError(nameof(CheckStateValidator), nameof(CheckState.Id), CheckStateMessage.Error.IdNotExisted, CheckStateMessage);
            return CheckState.IsValidated;
        }

        private async Task<bool> ValidateCode(CheckState CheckState)
        {
            if(string.IsNullOrEmpty(CheckState.Code))
            {
                CheckState.AddError(nameof(CheckStateValidator), nameof(CheckState.Code), CheckStateMessage.Error.CodeEmpty, CheckStateMessage);
            }
            else if(CheckState.Code.Count() > 500)
            {
                CheckState.AddError(nameof(CheckStateValidator), nameof(CheckState.Code), CheckStateMessage.Error.CodeOverLength, CheckStateMessage);
            }
            return CheckState.IsValidated;
        }
        private async Task<bool> ValidateName(CheckState CheckState)
        {
            if(string.IsNullOrEmpty(CheckState.Name))
            {
                CheckState.AddError(nameof(CheckStateValidator), nameof(CheckState.Name), CheckStateMessage.Error.NameEmpty, CheckStateMessage);
            }
            else if(CheckState.Name.Count() > 500)
            {
                CheckState.AddError(nameof(CheckStateValidator), nameof(CheckState.Name), CheckStateMessage.Error.NameOverLength, CheckStateMessage);
            }
            return CheckState.IsValidated;
        }
    }
}
