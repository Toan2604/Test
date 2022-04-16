using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MTaxType
{
    public interface ITaxTypeValidator : IServiceScoped
    {
        Task<bool> Create(TaxType TaxType);
        Task<bool> Update(TaxType TaxType);
        Task<bool> Delete(TaxType TaxType);
        Task<bool> BulkDelete(List<TaxType> TaxTypes);
        Task<bool> Import(List<TaxType> TaxTypes);
    }

    public class TaxTypeValidator : ITaxTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            TaxTypeInUsed,
            CodeHasSpecialCharacter,
            CodeEmpty,
            CodeExisted,
            NameEmpty,
            NameOverLength
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public TaxTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(TaxType TaxType)
        {
            TaxTypeFilter TaxTypeFilter = new TaxTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = TaxType.Id },
                Selects = TaxTypeSelect.Id
            };

            int count = await UOW.TaxTypeRepository.Count(TaxTypeFilter);
            if (count == 0)
                TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateTaxTypeInUsed(TaxType TaxType)
        {
            ProductFilter ProductFilter = new ProductFilter
            {
                TaxTypeId = new IdFilter { Equal = TaxType.Id },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            };
            int count = await UOW.ProductRepository.Count(ProductFilter);
            if (count > 0)
                TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Id), ErrorCode.TaxTypeInUsed);

            return TaxType.IsValidated;
        }

        private async Task<bool> ValidateCode(TaxType TaxType)
        {
            if (string.IsNullOrEmpty(TaxType.Code))
            {
                TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Code), ErrorCode.CodeEmpty);
                return false;
            }
            else
            {
                var Code = TaxType.Code;
                if (TaxType.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(TaxType.Code))
                {
                    TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    TaxTypeFilter TaxTypeFilter = new TaxTypeFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = TaxType.Id },
                        Code = new StringFilter { Equal = TaxType.Code },
                        Selects = TaxTypeSelect.Code
                    };

                    int count = await UOW.TaxTypeRepository.Count(TaxTypeFilter);
                    if (count != 0)
                        TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Code), ErrorCode.CodeExisted);
                }

            }

            return TaxType.IsValidated;
        }

        private async Task<bool> ValidateName(TaxType TaxType)
        {
            if (string.IsNullOrEmpty(TaxType.Name))
            {
                TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Name), ErrorCode.NameEmpty);
                return false;
            }
            else if (TaxType.Name.Length > 255)
            {
                TaxType.AddError(nameof(TaxTypeValidator), nameof(TaxType.Name), ErrorCode.NameOverLength);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(TaxType TaxType)
        {
            await ValidateCode(TaxType);
            await ValidateName(TaxType);
            return TaxType.IsValidated;
        }

        public async Task<bool> Update(TaxType TaxType)
        {
            if (await ValidateId(TaxType))
            {
                await ValidateCode(TaxType);
                await ValidateName(TaxType);
            }
            return TaxType.IsValidated;
        }

        public async Task<bool> Delete(TaxType TaxType)
        {
            if (await ValidateId(TaxType))
            {
                await ValidateTaxTypeInUsed(TaxType);
            }
            return TaxType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<TaxType> TaxTypes)
        {
            foreach (TaxType TaxType in TaxTypes)
            {
                await Delete(TaxType);
            }
            return TaxTypes.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<TaxType> TaxTypes)
        {
            return true;
        }
    }
}
