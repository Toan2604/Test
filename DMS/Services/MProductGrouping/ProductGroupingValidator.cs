using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProductGrouping
{
    public interface IProductGroupingValidator : IServiceScoped
    {
        Task<bool> Create(ProductGrouping ProductGrouping);
        Task<bool> Update(ProductGrouping ProductGrouping);
        Task<bool> Delete(ProductGrouping ProductGrouping);
        Task<bool> BulkDelete(List<ProductGrouping> ProductGroupings);
        Task<bool> Import(List<ProductGrouping> ProductGroupings);
    }

    public class ProductGroupingValidator : IProductGroupingValidator
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
            ProductGroupingInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ProductGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ProductGrouping ProductGrouping)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ProductGrouping.Id },
                Selects = ProductGroupingSelect.Id
            };

            int count = await UOW.ProductGroupingRepository.Count(ProductGroupingFilter);
            if (count == 0)
                ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(ProductGrouping ProductGrouping)
        {
            if (string.IsNullOrWhiteSpace(ProductGrouping.Code))
            {
                ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = ProductGrouping.Code;
                if (ProductGrouping.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(ProductGrouping.Code))
                {
                    ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else
                {
                    ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = ProductGrouping.Id },
                        Code = new StringFilter { Equal = ProductGrouping.Code },
                        Selects = ProductGroupingSelect.Code
                    };

                    int count = await UOW.ProductGroupingRepository.Count(ProductGroupingFilter);
                    if (count != 0)
                        ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Code), ErrorCode.CodeExisted);
                }
            }
            return ProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateName(ProductGrouping ProductGrouping)
        {
            if (string.IsNullOrWhiteSpace(ProductGrouping.Name))
            {
                ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Name), ErrorCode.NameEmpty);
            }
            else if (ProductGrouping.Name.Length > 255)
            {
                ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Name), ErrorCode.NameOverLength);
            }
            return ProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateParent(ProductGrouping ProductGrouping)
        {
            if (ProductGrouping.ParentId.HasValue)
            {
                ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = ProductGrouping.ParentId },
                    Selects = ProductGroupingSelect.Id
                };

                int count = await UOW.ProductGroupingRepository.Count(ProductGroupingFilter);
                if (count == 0)
                    ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.ParentId), ErrorCode.ParentNotExisted);
            }

            return ProductGrouping.IsValidated;
        }

        public async Task<bool> Create(ProductGrouping ProductGrouping)
        {
            await ValidateCode(ProductGrouping);
            await ValidateName(ProductGrouping);
            if (ProductGrouping.ParentId.HasValue)
                await ValidateParent(ProductGrouping);
            return ProductGrouping.IsValidated;
        }

        public async Task<bool> Update(ProductGrouping ProductGrouping)
        {
            if (await ValidateId(ProductGrouping))
            {
                await ValidateCode(ProductGrouping);
                await ValidateName(ProductGrouping);
                await ValidateParent(ProductGrouping);
            }
            return ProductGrouping.IsValidated;
        }

        public async Task<bool> Delete(ProductGrouping ProductGrouping)
        {
            if (await ValidateId(ProductGrouping))
            {
                ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter
                {
                    ParentId = new IdFilter { Equal = ProductGrouping.Id },
                };

                var count = await UOW.ProductGroupingRepository.Count(ProductGroupingFilter);
                if (count > 0)
                    ProductGrouping.AddError(nameof(ProductGroupingValidator), nameof(ProductGrouping.Id), ErrorCode.ProductGroupingInUsed);
            }
            return ProductGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ProductGrouping> ProductGroupings)
        {
            foreach (ProductGrouping ProductGrouping in ProductGroupings)
            {
                await Delete(ProductGrouping);
            }
            return ProductGroupings.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<ProductGrouping> ProductGroupings)
        {
            return true;
        }
    }
}
