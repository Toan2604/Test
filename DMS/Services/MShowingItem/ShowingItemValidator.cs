using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MShowingItem
{
    public interface IShowingItemValidator : IServiceScoped
    {
        Task<bool> Create(ShowingItem ShowingItem);
        Task<bool> Update(ShowingItem ShowingItem);
        Task<bool> Delete(ShowingItem ShowingItem);
        Task<bool> BulkDelete(List<ShowingItem> ShowingItems);
        Task<bool> Import(List<ShowingItem> ShowingItems);
    }

    public class ShowingItemValidator : IShowingItemValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            CodeRuleNotExisted,
            CodeHasSpecialCharacter,
            CodeEmpty,
            NameEmpty,
            NameExisted,
            NameOverLength,
            UnitOfMeasureNotExisted,
            UnitOfMeasureEmpty,
            SalePriceEmpty,
            StatusEmpty,
            ShowingCategoryEmpty,
            ShowingCategoryNotExisted,
            ShowingCategoryInvalid,
            ShowingItemInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ShowingItemValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ShowingItem ShowingItem)
        {
            ShowingItemFilter ShowingItemFilter = new ShowingItemFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ShowingItem.Id },
                Selects = ShowingItemSelect.Id
            };

            int count = await UOW.ShowingItemRepository.Count(ShowingItemFilter);
            if (count == 0)
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(ShowingItem ShowingItem)
        {
            var oldData = await UOW.ShowingItemRepository.Get(ShowingItem.Id);
            if (oldData != null && oldData.Used)
            {
                if (oldData.Code != ShowingItem.Code)
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Id), ErrorCode.ShowingItemInUsed);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ShowingItem.Code))
                {
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Code), ErrorCode.CodeEmpty);
                }
                else
                {
                    var Code = ShowingItem.Code;
                    if (ShowingItem.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(ShowingItem.Code))
                    {
                        ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Code), ErrorCode.CodeHasSpecialCharacter);
                    }

                    ShowingItemFilter ShowingItemFilter = new ShowingItemFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = ShowingItem.Id },
                        Code = new StringFilter { Equal = ShowingItem.Code },
                        Selects = ShowingItemSelect.Code
                    };

                    int count = await UOW.ShowingItemRepository.Count(ShowingItemFilter);
                    if (count != 0)
                        ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Code), ErrorCode.CodeExisted);
                }
            }

            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateName(ShowingItem ShowingItem)
        {
            var oldData = await UOW.ShowingItemRepository.Get(ShowingItem.Id);
            if (oldData != null && oldData.Used)
            {
                if (oldData.Name != ShowingItem.Name)
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Id), ErrorCode.ShowingItemInUsed);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ShowingItem.Name))
                {
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Name), ErrorCode.NameEmpty);
                }
                else if (ShowingItem.Name.Length > 3000)
                {
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Name), ErrorCode.NameOverLength);
                }
            }

            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateCategory(ShowingItem ShowingItem)
        {
            if (ShowingItem.ShowingCategoryId == 0)
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.ShowingCategory), ErrorCode.ShowingCategoryEmpty);
            else
            {
                ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = ShowingItem.ShowingCategoryId },
                    Selects = ShowingCategorySelect.Id
                };

                int count = await UOW.ShowingCategoryRepository.Count(ShowingCategoryFilter);
                if (count == 0)
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.ShowingCategory), ErrorCode.ShowingCategoryNotExisted);
            }

            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateUnitOfMeasure(ShowingItem ShowingItem)
        {
            if (ShowingItem.UnitOfMeasureId == 0)
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.UnitOfMeasure), ErrorCode.UnitOfMeasureEmpty);
            else
            {
                UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = ShowingItem.UnitOfMeasureId },
                    Selects = UnitOfMeasureSelect.Id
                };

                int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
                if (count == 0)
                    ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
            }

            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateStatusId(ShowingItem ShowingItem)
        {
            if (StatusEnum.ACTIVE.Id != ShowingItem.StatusId && StatusEnum.INACTIVE.Id != ShowingItem.StatusId)
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Status), ErrorCode.StatusEmpty);
            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateSalePrice(ShowingItem ShowingItem)
        {
            if (ShowingItem.SalePrice == 0)
            {
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.SalePrice), ErrorCode.SalePriceEmpty);
            }
            return ShowingItem.IsValidated;
        }

        private async Task<bool> ValidateUsed(ShowingItem ShowingItem)
        {
            var oldData = await UOW.ShowingItemRepository.Get(ShowingItem.Id);
            if (oldData.Used)
                ShowingItem.AddError(nameof(ShowingItemValidator), nameof(ShowingItem.Used), ErrorCode.ShowingItemInUsed);
            return ShowingItem.IsValidated;
        }

        public async Task<bool> Create(ShowingItem ShowingItem)
        {
            await ValidateCode(ShowingItem);
            await ValidateName(ShowingItem);
            await ValidateCategory(ShowingItem);
            await ValidateUnitOfMeasure(ShowingItem);
            await ValidateStatusId(ShowingItem);
            await ValidateSalePrice(ShowingItem);
            return ShowingItem.IsValidated;
        }

        public async Task<bool> Update(ShowingItem ShowingItem)
        {
            if (await ValidateId(ShowingItem))
            {
                await ValidateCode(ShowingItem);
                await ValidateName(ShowingItem);
                await ValidateCategory(ShowingItem);
                await ValidateUnitOfMeasure(ShowingItem);
                await ValidateStatusId(ShowingItem);
                await ValidateSalePrice(ShowingItem);
            }
            return ShowingItem.IsValidated;
        }

        public async Task<bool> Delete(ShowingItem ShowingItem)
        {
            if (await ValidateId(ShowingItem))
            {
                await ValidateUsed(ShowingItem);// validate Used
            }
            return ShowingItem.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ShowingItem> ShowingItems)
        {
            foreach (ShowingItem ShowingItem in ShowingItems)
            {
                await Delete(ShowingItem);
            }
            return ShowingItems.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ShowingItem> ShowingItems)
        {
            return true;
        }
    }
}
