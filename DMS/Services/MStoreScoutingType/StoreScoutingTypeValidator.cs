using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreScoutingType
{
    public interface IStoreScoutingTypeValidator : IServiceScoped
    {
        Task<bool> Create(StoreScoutingType StoreScoutingType);
        Task<bool> Update(StoreScoutingType StoreScoutingType);
        Task<bool> Delete(StoreScoutingType StoreScoutingType);
        Task<bool> BulkDelete(List<StoreScoutingType> StoreScoutingTypes);
        Task<bool> Import(List<StoreScoutingType> StoreScoutingTypes);
    }

    public class StoreScoutingTypeValidator : IStoreScoutingTypeValidator
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
            StoreScoutingTypeInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreScoutingTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreScoutingType StoreScoutingType)
        {
            StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreScoutingType.Id },
                Selects = StoreScoutingTypeSelect.Id
            };

            int count = await UOW.StoreScoutingTypeRepository.Count(StoreScoutingTypeFilter);
            if (count == 0)
                StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateCode(StoreScoutingType StoreScoutingType)
        {
            if (string.IsNullOrWhiteSpace(StoreScoutingType.Code))
            {
                StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                if (!StoreScoutingType.Code.ChangeToEnglishChar().Equals(StoreScoutingType.Code))
                {
                    StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                StoreScoutingTypeFilter StoreScoutingTypeFilter = new StoreScoutingTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = StoreScoutingType.Id },
                    Code = new StringFilter { Equal = StoreScoutingType.Code },
                    Selects = StoreScoutingTypeSelect.Code
                };

                int count = await UOW.StoreScoutingTypeRepository.Count(StoreScoutingTypeFilter);
                if (count != 0)
                    StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Code), ErrorCode.CodeExisted);
            }
            return StoreScoutingType.IsValidated;
        }
        public async Task<bool> ValidateName(StoreScoutingType StoreScoutingType)
        {
            if (string.IsNullOrWhiteSpace(StoreScoutingType.Name))
            {
                StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Name), ErrorCode.NameEmpty);
            }
            else if (StoreScoutingType.Name.Length > 255)
            {
                StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Name), ErrorCode.NameOverLength);
            }
            return StoreScoutingType.IsValidated;
        }

        public async Task<bool> ValidateStatus(StoreScoutingType StoreScoutingType)
        {
            if (StatusEnum.ACTIVE.Id != StoreScoutingType.StatusId && StatusEnum.INACTIVE.Id != StoreScoutingType.StatusId)
                StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Status), ErrorCode.StatusNotExisted);
            return StoreScoutingType.IsValidated;
        }

        public async Task<bool> Create(StoreScoutingType StoreScoutingType)
        {
            await ValidateCode(StoreScoutingType);
            await ValidateName(StoreScoutingType);
            await ValidateStatus(StoreScoutingType);
            return StoreScoutingType.IsValidated;
        }

        public async Task<bool> Update(StoreScoutingType StoreScoutingType)
        {
            if (await ValidateId(StoreScoutingType))
            {
                await ValidateCode(StoreScoutingType);
                await ValidateName(StoreScoutingType);
                await ValidateStatus(StoreScoutingType);
            }
            return StoreScoutingType.IsValidated;
        }

        public async Task<bool> Delete(StoreScoutingType StoreScoutingType)
        {
            if (await ValidateId(StoreScoutingType))
            {
                StoreScoutingType = await UOW.StoreScoutingTypeRepository.Get(StoreScoutingType.Id);
                if (StoreScoutingType.Used)
                {
                    StoreScoutingType.AddError(nameof(StoreScoutingTypeValidator), nameof(StoreScoutingType.Id), ErrorCode.StoreScoutingTypeInUsed);
                }
            }
            return StoreScoutingType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreScoutingType> StoreScoutingTypes)
        {
            foreach (StoreScoutingType StoreScoutingType in StoreScoutingTypes)
            {
                await Delete(StoreScoutingType);
            }
            return StoreScoutingTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<StoreScoutingType> StoreScoutingTypes)
        {
            return true;
        }
    }
}
