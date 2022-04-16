using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MUnitOfMeasureGrouping
{
    public interface IUnitOfMeasureGroupingValidator : IServiceScoped
    {
        Task<bool> Create(UnitOfMeasureGrouping UnitOfMeasureGrouping);
        Task<bool> Update(UnitOfMeasureGrouping UnitOfMeasureGrouping);
        Task<bool> Delete(UnitOfMeasureGrouping UnitOfMeasureGrouping);
        Task<bool> BulkDelete(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
        Task<bool> Import(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
    }

    public class UnitOfMeasureGroupingValidator : IUnitOfMeasureGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            NameEmpty,
            NameOverLength,
            StatusNotExisted,
            UnitOfMeasureEmpty,
            UnitOfMeasureNotExisted,
            UnitOfMeasureGroupingInUsed,
            FactorWrongValue,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public UnitOfMeasureGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter = new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = UnitOfMeasureGrouping.Id },
                Selects = UnitOfMeasureGroupingSelect.Id
            };

            int count = await UOW.UnitOfMeasureGroupingRepository.Count(UnitOfMeasureGroupingFilter);
            if (count == 0)
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateCode(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            if (string.IsNullOrWhiteSpace(UnitOfMeasureGrouping.Code))
            {
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = UnitOfMeasureGrouping.Code;
                if (UnitOfMeasureGrouping.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(UnitOfMeasureGrouping.Code))
                {
                    UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter = new UnitOfMeasureGroupingFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = UnitOfMeasureGrouping.Id },
                    Code = new StringFilter { Equal = UnitOfMeasureGrouping.Code },
                    Selects = UnitOfMeasureGroupingSelect.Code
                };

                int count = await UOW.UnitOfMeasureGroupingRepository.Count(UnitOfMeasureGroupingFilter);
                if (count != 0)
                    UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Code), ErrorCode.CodeExisted);
            }
            return UnitOfMeasureGrouping.IsValidated;
        }
        public async Task<bool> ValidateName(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            if (string.IsNullOrWhiteSpace(UnitOfMeasureGrouping.Name))
            {
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Name), ErrorCode.NameEmpty);
            }
            else if (UnitOfMeasureGrouping.Name.Length > 255)
            {
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Name), ErrorCode.NameOverLength);
            }
            return UnitOfMeasureGrouping.IsValidated;
        }

        public async Task<bool> ValidateStatus(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            if (StatusEnum.ACTIVE.Id != UnitOfMeasureGrouping.StatusId && StatusEnum.INACTIVE.Id != UnitOfMeasureGrouping.StatusId)
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Status), ErrorCode.StatusNotExisted);
            return UnitOfMeasureGrouping.IsValidated;
        }
        private async Task<bool> ValidateUnitOfMeasureId(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {

            if (UnitOfMeasureGrouping.UnitOfMeasureId == 0)
            {
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.UnitOfMeasureId), ErrorCode.UnitOfMeasureEmpty);
            }
            else
            {
                var oldData = await UOW.UnitOfMeasureGroupingRepository.Get(UnitOfMeasureGrouping.Id);
                if (oldData != null && oldData.Used)
                {
                    if (oldData.UnitOfMeasureId != UnitOfMeasureGrouping.UnitOfMeasureId)
                        UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Id), ErrorCode.UnitOfMeasureGroupingInUsed);
                }
                else
                {
                    UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { Equal = UnitOfMeasureGrouping.UnitOfMeasureId },
                        StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                        Selects = UnitOfMeasureSelect.Id
                    };

                    int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
                    if (count == 0)
                        UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.UnitOfMeasureId), ErrorCode.UnitOfMeasureNotExisted);
                }
            }
            return UnitOfMeasureGrouping.IsValidated;
        }

        private async Task<bool> ValidateUnitOfMeasureGroupingInUsed(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            var old = await UOW.UnitOfMeasureGroupingRepository.Get(UnitOfMeasureGrouping.Id);
            if (old != null && old.Used)
            {
                UnitOfMeasureGrouping.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGrouping.Id), ErrorCode.UnitOfMeasureGroupingInUsed);
            }

            return UnitOfMeasureGrouping.IsValidated;
        }

        private async Task<bool> ValidateUnitOfMeasureGroupingContent(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            List<long> UOMIds = UnitOfMeasureGrouping.UnitOfMeasureGroupingContents.Select(x => x.UnitOfMeasureId).ToList();
            List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(new UnitOfMeasureFilter
            {
                Id = new IdFilter { In = UOMIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureSelect.Id,
            });
            foreach (UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent in UnitOfMeasureGrouping.UnitOfMeasureGroupingContents)
            {
                if (UnitOfMeasureGroupingContent.Factor < 1)
                    UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGroupingContent.Factor), ErrorCode.FactorWrongValue);
                if (UnitOfMeasureGroupingContent.UnitOfMeasureId == 0)
                    UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasureId), ErrorCode.UnitOfMeasureEmpty);
                else
                {
                    UnitOfMeasure UOM = UnitOfMeasures.Where(x => x.Id == UnitOfMeasureGroupingContent.UnitOfMeasureId).FirstOrDefault();
                    if (UOM == null)
                        UnitOfMeasureGroupingContent.AddError(nameof(UnitOfMeasureGroupingValidator), nameof(UnitOfMeasureGroupingContent.UnitOfMeasureId), ErrorCode.UnitOfMeasureNotExisted);
                }
            }

            return UnitOfMeasureGrouping.IsValidated;
        }

        public async Task<bool> Create(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            await ValidateCode(UnitOfMeasureGrouping);
            await ValidateName(UnitOfMeasureGrouping);
            await ValidateUnitOfMeasureId(UnitOfMeasureGrouping);
            await ValidateStatus(UnitOfMeasureGrouping);
            return UnitOfMeasureGrouping.IsValidated;
        }

        public async Task<bool> Update(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            if (await ValidateId(UnitOfMeasureGrouping))
            {
                await ValidateCode(UnitOfMeasureGrouping);
                await ValidateName(UnitOfMeasureGrouping);
                await ValidateUnitOfMeasureId(UnitOfMeasureGrouping);
                await ValidateStatus(UnitOfMeasureGrouping);
            }
            return UnitOfMeasureGrouping.IsValidated;
        }

        public async Task<bool> Delete(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            if (await ValidateId(UnitOfMeasureGrouping))
            {
                await ValidateUnitOfMeasureGroupingInUsed(UnitOfMeasureGrouping);
            }
            return UnitOfMeasureGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            return true;
        }

        public async Task<bool> Import(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            return true;
        }
    }
}
