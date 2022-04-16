using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MNation
{
    public interface INationValidator : IServiceScoped
    {
        Task<bool> Create(Nation Nation);
        Task<bool> Update(Nation Nation);
        Task<bool> Delete(Nation Nation);
        Task<bool> BulkDelete(List<Nation> Nations);
        Task<bool> BulkMerge(List<Nation> Nations);
    }

    public class NationValidator : INationValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            NameEmpty,
            CodeExisted,
            NameOverLength
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public NationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Nation Nation)
        {
            NationFilter NationFilter = new NationFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Nation.Id },
                Selects = NationSelect.Id
            };

            int count = await UOW.NationRepository.Count(NationFilter);
            if (count == 0)
                Nation.AddError(nameof(NationValidator), nameof(Nation.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Nation Nation)
        {
            if (string.IsNullOrEmpty(Nation.Code))
            {
                Nation.AddError(nameof(NationValidator), nameof(Nation.Code), ErrorCode.CodeEmpty);
                return false;
            }
            NationFilter NationFilter = new NationFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { NotEqual = Nation.Id },
                Code = new StringFilter { Equal = Nation.Code },
                Selects = NationSelect.Code
            };

            int count = await UOW.NationRepository.Count(NationFilter);
            if (count != 0)
                Nation.AddError(nameof(NationValidator), nameof(Nation.Code), ErrorCode.CodeExisted);
            return count == 0;
        }

        private async Task<bool> ValidateName(Nation Nation)
        {
            if (string.IsNullOrEmpty(Nation.Name))
            {
                Nation.AddError(nameof(NationValidator), nameof(Nation.Name), ErrorCode.NameEmpty);
                return false;
            }
            else if (Nation.Name.Length > 255)
            {
                Nation.AddError(nameof(NationValidator), nameof(Nation.Name), ErrorCode.NameOverLength);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(Nation Nation)
        {
            await ValidateCode(Nation);
            await ValidateName(Nation);
            return Nation.IsValidated;
        }

        public async Task<bool> Update(Nation Nation)
        {
            if (await ValidateId(Nation))
            {
                await ValidateCode(Nation);
                await ValidateName(Nation);
            }
            return Nation.IsValidated;
        }

        public async Task<bool> Delete(Nation Nation)
        {
            if (await ValidateId(Nation))
            {
            }
            return Nation.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Nation> Nations)
        {
            return true;
        }

        public async Task<bool> BulkMerge(List<Nation> Nations)
        {
            return true;
        }
    }
}
