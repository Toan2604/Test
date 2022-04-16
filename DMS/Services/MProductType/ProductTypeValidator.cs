using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProductType
{
    public interface IProductTypeValidator : IServiceScoped
    {
        Task<bool> Create(ProductType ProductType);
        Task<bool> Update(ProductType ProductType);
        Task<bool> Delete(ProductType ProductType);
        Task<bool> BulkDelete(List<ProductType> ProductTypes);
        Task<bool> Import(List<ProductType> ProductTypes);
    }

    public class ProductTypeValidator : IProductTypeValidator
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
            ProductTypeInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ProductTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ProductType ProductType)
        {
            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ProductType.Id },
                Selects = ProductTypeSelect.Id
            };

            int count = await UOW.ProductTypeRepository.Count(ProductTypeFilter);
            if (count == 0)
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateCode(ProductType ProductType)
        {
            if (string.IsNullOrWhiteSpace(ProductType.Code))
            {
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = ProductType.Code;
                if (ProductType.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(ProductType.Code))
                {
                    ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                ProductTypeFilter ProductTypeFilter = new ProductTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = ProductType.Id },
                    Code = new StringFilter { Equal = ProductType.Code },
                    Selects = ProductTypeSelect.Code
                };

                int count = await UOW.ProductTypeRepository.Count(ProductTypeFilter);
                if (count != 0)
                    ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Code), ErrorCode.CodeExisted);
            }
            return ProductType.IsValidated;
        }
        public async Task<bool> ValidateName(ProductType ProductType)
        {
            if (string.IsNullOrWhiteSpace(ProductType.Name))
            {
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Name), ErrorCode.NameEmpty);
            }
            else if (ProductType.Name.Length > 255)
            {
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Name), ErrorCode.NameOverLength);
            }
            return ProductType.IsValidated;
        }

        public async Task<bool> ValidateStatus(ProductType ProductType)
        {
            if (StatusEnum.ACTIVE.Id != ProductType.StatusId && StatusEnum.INACTIVE.Id != ProductType.StatusId)
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Status), ErrorCode.StatusNotExisted);
            return ProductType.IsValidated;
        }

        private async Task<bool> ValidateProductTypeInUsed(ProductType ProductType)
        {
            ProductFilter ProductFilter = new ProductFilter
            {
                ProductTypeId = new IdFilter { Equal = ProductType.Id },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            };
            int count = await UOW.ProductRepository.Count(ProductFilter);
            if (count > 0)
                ProductType.AddError(nameof(ProductTypeValidator), nameof(ProductType.Id), ErrorCode.ProductTypeInUsed);

            return ProductType.IsValidated;
        }
        public async Task<bool> Create(ProductType ProductType)
        {
            await ValidateCode(ProductType);
            await ValidateName(ProductType);
            await ValidateStatus(ProductType);
            return ProductType.IsValidated;
        }

        public async Task<bool> Update(ProductType ProductType)
        {
            if (await ValidateId(ProductType))
            {
                await ValidateCode(ProductType);
                await ValidateName(ProductType);
                await ValidateStatus(ProductType);
            }
            return ProductType.IsValidated;
        }

        public async Task<bool> Delete(ProductType ProductType)
        {
            if (await ValidateId(ProductType))
            {
                await ValidateProductTypeInUsed(ProductType);
            }
            return ProductType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ProductType> ProductTypes)
        {
            foreach (ProductType ProductType in ProductTypes)
            {
                await Delete(ProductType);
            }
            return ProductTypes.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<ProductType> ProductTypes)
        {
            return true;
        }
    }
}
