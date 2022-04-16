using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MShowingCategory
{
    public interface IShowingCategoryValidator : IServiceScoped
    {
        Task<bool> Create(ShowingCategory ShowingCategory);
        Task<bool> Update(ShowingCategory ShowingCategory);
        Task<bool> Delete(ShowingCategory ShowingCategory);
        Task<bool> BulkDelete(List<ShowingCategory> Categories);
        Task<bool> Import(List<ShowingCategory> Categories);
    }

    public class ShowingCategoryValidator : IShowingCategoryValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            CodeExisted,
            NameEmpty,
            NameOverLength,
            ParentNotExisted,
            ShowingCategoryInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ShowingCategoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ShowingCategory ShowingCategory)
        {
            ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ShowingCategory.Id },
                Selects = ShowingCategorySelect.Id
            };

            int count = await UOW.ShowingCategoryRepository.Count(ShowingCategoryFilter);
            if (count == 0)
                ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(ShowingCategory ShowingCategory)
        {
            if (string.IsNullOrWhiteSpace(ShowingCategory.Code))
            {
                ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = ShowingCategory.Code;
                if (ShowingCategory.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(ShowingCategory.Code))
                {
                    ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = ShowingCategory.Id },
                        Code = new StringFilter { Equal = ShowingCategory.Code },
                        Selects = ShowingCategorySelect.Code
                    };

                    int count = await UOW.ShowingCategoryRepository.Count(ShowingCategoryFilter);
                    if (count != 0)
                        ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Code), ErrorCode.CodeExisted);
                }
            }
            return ShowingCategory.IsValidated;
        }

        private async Task<bool> ValidateName(ShowingCategory ShowingCategory)
        {
            if (string.IsNullOrWhiteSpace(ShowingCategory.Name))
            {
                ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Name), ErrorCode.NameEmpty);
            }
            else if (ShowingCategory.Name.Length > 255)
            {
                ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Name), ErrorCode.NameOverLength);
            }
            return ShowingCategory.IsValidated;
        }

        private async Task<bool> ValidateParent(ShowingCategory ShowingCategory)
        {
            if (ShowingCategory.ParentId.HasValue)
            {
                ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = ShowingCategory.ParentId },
                    Selects = ShowingCategorySelect.Id
                };

                int count = await UOW.ShowingCategoryRepository.Count(ShowingCategoryFilter);
                if (count == 0)
                    ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.ParentId), ErrorCode.ParentNotExisted);
            }

            return ShowingCategory.IsValidated;
        }

        public async Task<bool> Create(ShowingCategory ShowingCategory)
        {
            await ValidateCode(ShowingCategory);
            await ValidateName(ShowingCategory);
            if (ShowingCategory.ParentId.HasValue)
                await ValidateParent(ShowingCategory);
            return ShowingCategory.IsValidated;
        }

        public async Task<bool> Update(ShowingCategory ShowingCategory)
        {
            if (await ValidateId(ShowingCategory))
            {
                await ValidateCode(ShowingCategory);
                await ValidateName(ShowingCategory);
                await ValidateParent(ShowingCategory);
            }
            return ShowingCategory.IsValidated;
        }

        public async Task<bool> Delete(ShowingCategory ShowingCategory)
        {
            if (await ValidateId(ShowingCategory))
            {
                ShowingCategoryFilter ShowingCategoryFilter = new ShowingCategoryFilter
                {
                    ParentId = new IdFilter { Equal = ShowingCategory.Id },
                };

                var count = await UOW.ShowingCategoryRepository.Count(ShowingCategoryFilter);
                if (count > 0)
                    ShowingCategory.AddError(nameof(ShowingCategoryValidator), nameof(ShowingCategory.Id), ErrorCode.ShowingCategoryInUsed);
            }
            return ShowingCategory.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ShowingCategory> ShowingCategorys)
        {
            foreach (ShowingCategory ShowingCategory in ShowingCategorys)
            {
                await Delete(ShowingCategory);
            }
            return ShowingCategorys.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<ShowingCategory> ShowingCategorys)
        {
            return true;
        }
    }
}
