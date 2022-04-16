using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreGrouping
{
    public interface IStoreGroupingValidator : IServiceScoped
    {
        Task<bool> Create(StoreGrouping StoreGrouping);
        Task<bool> Update(StoreGrouping StoreGrouping);
        Task<bool> Delete(StoreGrouping StoreGrouping);
        Task<bool> BulkDelete(List<StoreGrouping> StoreGroupings);
        Task<bool> Import(List<StoreGrouping> StoreGroupings);
    }

    public class StoreGroupingValidator : IStoreGroupingValidator
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
            ParentNotExisted,
            StoreGroupingInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreGrouping StoreGrouping)
        {
            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreGrouping.Id },
                Selects = StoreGroupingSelect.Id
            };

            int count = await UOW.StoreGroupingRepository.Count(StoreGroupingFilter);
            if (count == 0)
                StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(StoreGrouping StoreGrouping)
        {
            if (string.IsNullOrWhiteSpace(StoreGrouping.Code))
            {
                StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = StoreGrouping.Code;
                if (StoreGrouping.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(StoreGrouping.Code))
                {
                    StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = StoreGrouping.Id },
                        Code = new StringFilter { Equal = StoreGrouping.Code },
                        Selects = StoreGroupingSelect.Code
                    };

                    int count = await UOW.StoreGroupingRepository.Count(StoreGroupingFilter);
                    if (count != 0)
                        StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Code), ErrorCode.CodeExisted);
                }
            }
            return StoreGrouping.IsValidated;
        }

        private async Task<bool> ValidateName(StoreGrouping StoreGrouping)
        {
            if (string.IsNullOrWhiteSpace(StoreGrouping.Name))
            {
                StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Name), ErrorCode.NameEmpty);
            }
            else if (StoreGrouping.Name.Length > 255)
            {
                StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Name), ErrorCode.NameOverLength);
            }
            return StoreGrouping.IsValidated;
        }

        private async Task<bool> ValidateParent(StoreGrouping StoreGrouping)
        {
            if (StoreGrouping.ParentId.HasValue)
            {
                StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = StoreGrouping.ParentId },
                    Selects = StoreGroupingSelect.Id
                };

                int count = await UOW.StoreGroupingRepository.Count(StoreGroupingFilter);
                if (count == 0)
                    StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.ParentId), ErrorCode.ParentNotExisted);
            }

            return StoreGrouping.IsValidated;
        }

        public async Task<bool> Create(StoreGrouping StoreGrouping)
        {
            await ValidateCode(StoreGrouping);
            await ValidateName(StoreGrouping);
            if (StoreGrouping.ParentId.HasValue)
            {
                await ValidateParent(StoreGrouping);
            }
            return StoreGrouping.IsValidated;
        }

        public async Task<bool> Update(StoreGrouping StoreGrouping)
        {
            if (await ValidateId(StoreGrouping))
            {
                await ValidateCode(StoreGrouping);
                await ValidateName(StoreGrouping);
                await ValidateParent(StoreGrouping);
            }
            return StoreGrouping.IsValidated;
        }

        public async Task<bool> Delete(StoreGrouping StoreGrouping)
        {
            if (await ValidateId(StoreGrouping))
            {
                StoreFilter StoreFilter = new StoreFilter
                {
                    StoreGroupingId = new IdFilter { Equal = StoreGrouping.Id },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                };

                var count = await UOW.StoreRepository.Count(StoreFilter);
                if (count > 0)
                    StoreGrouping.AddError(nameof(StoreGroupingValidator), nameof(StoreGrouping.Id), ErrorCode.StoreGroupingInUsed);
            }
            return StoreGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreGrouping> StoreGroupings)
        {
            foreach (StoreGrouping StoreGrouping in StoreGroupings)
            {
                await Delete(StoreGrouping);
            }
            return StoreGroupings.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<StoreGrouping> StoreGroupings)
        {
            return true;
        }
    }
}
