using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MWard
{
    public interface IWardValidator : IServiceScoped
    {
        Task<bool> Create(Ward Ward);
        Task<bool> Update(Ward Ward);
        Task<bool> Delete(Ward Ward);
        Task<bool> BulkDelete(List<Ward> Wards);
        Task<bool> Import(List<Ward> Wards);
    }

    public class WardValidator : IWardValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            NameEmpty,
            CodeExisted,
            NameOverLength
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public WardValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Ward Ward)
        {
            WardFilter WardFilter = new WardFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Ward.Id },
                Selects = WardSelect.Id
            };

            int count = await UOW.WardRepository.Count(WardFilter);
            if (count == 0)
                Ward.AddError(nameof(WardValidator), nameof(Ward.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        private async Task<bool> ValidateCode(Ward Ward)
        {
            if (string.IsNullOrEmpty(Ward.Code))
            {
                Ward.AddError(nameof(WardValidator), nameof(Ward.Code), ErrorCode.CodeEmpty);
                return false;
            }
            WardFilter WardFilter = new WardFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { NotEqual = Ward.Id },
                Code = new StringFilter { Equal = Ward.Code },
                Selects = WardSelect.Code
            };

            int count = await UOW.WardRepository.Count(WardFilter);
            if (count != 0)
                Ward.AddError(nameof(WardValidator), nameof(Ward.Code), ErrorCode.CodeExisted);
            return count == 0;
        }

        private async Task<bool> ValidateName(Ward Ward)
        {
            if (string.IsNullOrEmpty(Ward.Name))
            {
                Ward.AddError(nameof(WardValidator), nameof(Ward.Name), ErrorCode.NameEmpty);
                return false;
            }
            else if (Ward.Name.Length > 255)
            {
                Ward.AddError(nameof(WardValidator), nameof(Ward.Name), ErrorCode.NameOverLength);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(Ward Ward)
        {
            await ValidateCode(Ward);
            await ValidateName(Ward);
            return Ward.IsValidated;
        }

        public async Task<bool> Update(Ward Ward)
        {
            if (await ValidateId(Ward))
            {
                await ValidateCode(Ward);
                await ValidateName(Ward);
            }
            return Ward.IsValidated;
        }


        public async Task<bool> Delete(Ward Ward)
        {
            if (await ValidateId(Ward))
            {
            }
            return Ward.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Ward> Wards)
        {
            return true;
        }

        public async Task<bool> Import(List<Ward> Wards)
        {
            return true;
        }
    }
}
