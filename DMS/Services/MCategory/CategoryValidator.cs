using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MCategory
{
    public interface ICategoryValidator : IServiceScoped
    {
        Task<bool> Create(Category Category);
        Task<bool> Update(Category Category);
        Task<bool> Delete(Category Category);
        Task<bool> BulkDelete(List<Category> Categories);
        Task<bool> Import(List<Category> Categories);
    }

    public class CategoryValidator : ICategoryValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public CategoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Category Category)
        {
            CategoryFilter CategoryFilter = new CategoryFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Category.Id },
                Selects = CategorySelect.Id
            };

            int count = await UOW.CategoryRepository.Count(CategoryFilter);
            if (count == 0)
                Category.AddError(nameof(CategoryValidator), nameof(Category.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(Category Category)
        {
            return Category.IsValidated;
        }

        public async Task<bool> Update(Category Category)
        {
            if (await ValidateId(Category))
            {
            }
            return Category.IsValidated;
        }

        public async Task<bool> Delete(Category Category)
        {
            if (await ValidateId(Category))
            {
            }
            return Category.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Category> Categories)
        {
            foreach (Category Category in Categories)
            {
                await Delete(Category);
            }
            return Categories.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<Category> Categories)
        {
            return true;
        }
    }
}
