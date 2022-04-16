using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreType
{
    public interface IStoreTypeValidator : IServiceScoped
    {
        Task<bool> Create(StoreType StoreType);
        Task<bool> Update(StoreType StoreType);
        Task<bool> Delete(StoreType StoreType);
        Task<bool> BulkDelete(List<StoreType> StoreTypes);
        Task<bool> Import(List<StoreType> StoreTypes);
    }

    public class StoreTypeValidator : IStoreTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            StoreTypeInUsed,
            CodeExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            NameEmpty,
            NameOverLength,
            StatusNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> ValidateId(StoreType StoreType)
        {
            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreType.Id },
                Selects = StoreTypeSelect.Id
            };

            int count = await UOW.StoreTypeRepository.Count(StoreTypeFilter);
            if (count == 0)
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        public async Task<bool> ValidateCode(StoreType StoreType)
        {
            if (string.IsNullOrWhiteSpace(StoreType.Code))
            {
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = StoreType.Code;
                if (StoreType.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(StoreType.Code))
                {
                    StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                StoreTypeFilter StoreTypeFilter = new StoreTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = StoreType.Id },
                    Code = new StringFilter { Equal = StoreType.Code },
                    Selects = StoreTypeSelect.Code
                };

                int count = await UOW.StoreTypeRepository.Count(StoreTypeFilter);
                if (count != 0)
                    StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Code), ErrorCode.CodeExisted);
            }
            return StoreType.IsValidated;
        }
        public async Task<bool> ValidateName(StoreType StoreType)
        {
            if (string.IsNullOrWhiteSpace(StoreType.Name))
            {
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Name), ErrorCode.NameEmpty);
            }
            else if (StoreType.Name.Length > 255)
            {
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Name), ErrorCode.NameOverLength);
            }
            return StoreType.IsValidated;
        }

        public async Task<bool> ValidateStatus(StoreType StoreType)
        {
            if (StatusEnum.ACTIVE.Id != StoreType.StatusId && StatusEnum.INACTIVE.Id != StoreType.StatusId)
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Status), ErrorCode.StatusNotExisted);
            return StoreType.IsValidated;
        }

        private async Task<bool> ValidateStoreTypeInUsed(StoreType StoreType)
        {
            StoreFilter storeFilter = new StoreFilter
            {
                StoreTypeId = new IdFilter { Equal = StoreType.Id },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            };
            int count = await UOW.StoreRepository.Count(storeFilter);
            if (count > 0)
                StoreType.AddError(nameof(StoreTypeValidator), nameof(StoreType.Id), ErrorCode.StoreTypeInUsed);

            return StoreType.IsValidated;
        }
        public async Task<bool> Create(StoreType StoreType)
        {
            await ValidateCode(StoreType);
            await ValidateName(StoreType);
            await ValidateStatus(StoreType);
            return StoreType.IsValidated;
        }

        public async Task<bool> Update(StoreType StoreType)
        {
            if (await ValidateId(StoreType))
            {
                await ValidateCode(StoreType);
                await ValidateName(StoreType);
                await ValidateStatus(StoreType);
            }
            return StoreType.IsValidated;
        }

        public async Task<bool> Delete(StoreType StoreType)
        {
            if (await ValidateId(StoreType))
            {
                await ValidateStoreTypeInUsed(StoreType);
            }
            return StoreType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreType> StoreTypes)
        {
            foreach (StoreType StoreType in StoreTypes)
            {
                await Delete(StoreType);
            }
            return StoreTypes.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<StoreType> StoreTypes)
        {
            return true;
        }
    }
}
