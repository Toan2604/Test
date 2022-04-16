using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProvince
{
    public interface IProvinceValidator : IServiceScoped
    {
        Task<bool> Create(Province Province);
        Task<bool> Update(Province Province);
        Task<bool> Delete(Province Province);
        Task<bool> BulkDelete(List<Province> Provinces);
        Task<bool> BulkMerge(List<Province> Provinces);
    }

    public class ProvinceValidator : IProvinceValidator
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

        public ProvinceValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Province Province)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Province.Id },
                Selects = ProvinceSelect.Id
            };

            int count = await UOW.ProvinceRepository.Count(ProvinceFilter);
            if (count == 0)
                Province.AddError(nameof(ProvinceValidator), nameof(Province.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Province Province)
        {
            if (string.IsNullOrEmpty(Province.Code))
            {
                Province.AddError(nameof(ProvinceValidator), nameof(Province.Code), ErrorCode.CodeEmpty);
                return false;
            }
            ProvinceFilter ProvinceFilter = new ProvinceFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { NotEqual = Province.Id },
                Code = new StringFilter { Equal = Province.Code },
                Selects = ProvinceSelect.Code
            };

            int count = await UOW.ProvinceRepository.Count(ProvinceFilter);
            if (count != 0)
                Province.AddError(nameof(ProvinceValidator), nameof(Province.Code), ErrorCode.CodeExisted);
            return count == 0;
        }

        private async Task<bool> ValidateName(Province Province)
        {
            if (string.IsNullOrEmpty(Province.Name))
            {
                Province.AddError(nameof(ProvinceValidator), nameof(Province.Name), ErrorCode.NameEmpty);
                return false;
            }
            else if (Province.Name.Length > 255)
            {
                Province.AddError(nameof(ProvinceValidator), nameof(Province.Name), ErrorCode.NameOverLength);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(Province Province)
        {
            await ValidateCode(Province);
            await ValidateName(Province);
            return Province.IsValidated;
        }

        public async Task<bool> Update(Province Province)
        {
            if (await ValidateId(Province))
            {
                await ValidateCode(Province);
                await ValidateName(Province);
            }
            return Province.IsValidated;
        }

        public async Task<bool> Delete(Province Province)
        {
            if (await ValidateId(Province))
            {
            }
            return Province.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Province> Provinces)
        {
            return true;
        }

        public async Task<bool> BulkMerge(List<Province> Provinces)
        {
            return true;
        }
    }
}
