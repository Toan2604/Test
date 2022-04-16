using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MBrand
{
    public interface IBrandValidator : IServiceScoped
    {
        Task<bool> Create(Brand Brand);
        Task<bool> Update(Brand Brand);
        Task<bool> Delete(Brand Brand);
        Task<bool> BulkDelete(List<Brand> Brands);
        Task<bool> Import(List<Brand> Brands);
    }

    public class BrandValidator : IBrandValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeExisted,
            CodeHasSpecialCharacter,
            NameEmpty,
            NameOverLength,
            DescriptionOverLength,
            StatusNotExisted,
            BrandInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public BrandValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Brand Brand)
        {
            BrandFilter BrandFilter = new BrandFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Brand.Id },
                Selects = BrandSelect.Id
            };

            int count = await UOW.BrandRepository.Count(BrandFilter);
            if (count == 0)
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Brand Brand)
        {
            if (string.IsNullOrWhiteSpace(Brand.Code))
            {
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = Brand.Code;
                if (Brand.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Brand.Code))
                {
                    Brand.AddError(nameof(BrandValidator), nameof(Brand.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                BrandFilter BrandFilter = new BrandFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = Brand.Id },
                    Code = new StringFilter { Equal = Brand.Code },
                    Selects = BrandSelect.Code
                };

                int count = await UOW.BrandRepository.Count(BrandFilter);
                if (count != 0)
                    Brand.AddError(nameof(BrandValidator), nameof(Brand.Code), ErrorCode.CodeExisted);
            }

            return Brand.IsValidated;
        }

        private async Task<bool> ValidateName(Brand Brand)
        {
            if (string.IsNullOrWhiteSpace(Brand.Name))
            {
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Name), ErrorCode.NameEmpty);
            }
            else if (Brand.Name.Length > 255)
            {
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Name), ErrorCode.NameOverLength);
            }
            return Brand.IsValidated;
        }

        private async Task<bool> ValidateDescription(Brand Brand)
        {
            if (!string.IsNullOrWhiteSpace(Brand.Description) && Brand.Description.Length > 2000)
            {
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Description), ErrorCode.DescriptionOverLength);
            }
            return Brand.IsValidated;
        }

        private async Task<bool> ValidateStatus(Brand Brand)
        {
            if (StatusEnum.ACTIVE.Id != Brand.StatusId && StatusEnum.INACTIVE.Id != Brand.StatusId)
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Status), ErrorCode.StatusNotExisted);
            return Brand.IsValidated;
        }

        private async Task<bool> ValidateBrandInUsed(Brand Brand)
        {
            ProductFilter ProductFilter = new ProductFilter
            {
                BrandId = new IdFilter { Equal = Brand.Id },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            };
            int count = await UOW.ProductRepository.Count(ProductFilter);
            if (count > 0)
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Id), ErrorCode.BrandInUsed);

            return Brand.IsValidated;
        }

        public async Task<bool> Create(Brand Brand)
        {
            await ValidateCode(Brand);
            await ValidateName(Brand);
            await ValidateDescription(Brand);
            await ValidateStatus(Brand);
            return Brand.IsValidated;
        }

        public async Task<bool> Update(Brand Brand)
        {
            if (await ValidateId(Brand))
            {
                await ValidateCode(Brand);
                await ValidateName(Brand);
                await ValidateDescription(Brand);
                await ValidateStatus(Brand);
            }
            return Brand.IsValidated;
        }

        public async Task<bool> Delete(Brand Brand)
        {
            if (await ValidateId(Brand))
            {
                await ValidateBrandInUsed(Brand);
            }
            return Brand.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Brand> Brands)
        {
            BrandFilter BrandFilter = new BrandFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { In = Brands.Select(a => a.Id).ToList() },
                Selects = BrandSelect.Id
            };

            var listInDB = await UOW.BrandRepository.List(BrandFilter);
            var listExcept = Brands.Except(listInDB);
            if (listExcept == null || listExcept.Count() == 0) return true;
            foreach (var Brand in listExcept)
            {
                Brand.AddError(nameof(BrandValidator), nameof(Brand.Id), ErrorCode.IdNotExisted);
            }
            return false;
        }

        public async Task<bool> Import(List<Brand> Brands)
        {
            var listCodeInDB = (await UOW.BrandRepository.List(new BrandFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = BrandSelect.Code
            })).Select(e => e.Code);

            foreach (var Brand in Brands)
            {
                if (listCodeInDB.Contains(Brand.Code))
                {
                    Brand.AddError(nameof(BrandValidator), nameof(Brand.Code), ErrorCode.CodeExisted);
                }

                await (ValidateName(Brand));
                await (ValidateDescription(Brand));
            }

            return Brands.Any(o => !o.IsValidated) ? false : true;
        }
    }
}
