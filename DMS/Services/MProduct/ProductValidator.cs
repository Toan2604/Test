using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IProductValidator : IServiceScoped
    {
        Task<bool> Create(Product Product);
        Task<bool> Update(Product Product);
        Task<bool> Delete(Product Product);
        Task<bool> BulkDelete(List<Product> Products);
        Task<bool> BulkMergeNewProduct(List<Product> Products);
        Task<bool> Import(List<Product> Products);
    }

    public class ProductValidator : IProductValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            CodeHasSpecialCharacter,
            CodeEmpty,
            NameEmpty,
            NameExisted,
            NameOverLength,
            ProductTypeNotExisted,
            ProductTypeEmpty,
            SupplierNotExisted,
            BrandNotExisted,
            TaxTypeNotExisted,
            TaxTypeEmpty,
            UnitOfMeasureNotExisted,
            UnitOfMeasureEmpty,
            UnitOfMeasureGroupingNotExisted,
            UnitOfMeasureGroupingInvalid,
            SalePriceInvalid,
            SalePriceEmpty,
            RetailPriceInvalid,
            StatusEmpty,
            ERPCodeHasSpecialCharacter,
            ScanCodeHasSpecialCharacter,
            VariationGroupingsEmpty,
            VariationsEmpty,
            ItemsEmpty,
            ProductInUsed,
            UsedVariationNotExisted,
            CategoryNotExisted,
            CategoryEmpty,

        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ProductValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        private async Task<bool> ValidateId(Product Product)
        {
            ProductFilter ProductFilter = new ProductFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Product.Id },
                Selects = ProductSelect.Id
            };

            int count = await UOW.ProductRepository.Count(ProductFilter);
            if (count == 0)
                Product.AddError(nameof(ProductValidator), nameof(Product.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        private async Task<bool> ValidateName(Product Product)
        {
            var oldData = await UOW.ProductRepository.Get(Product.Id);
            if (oldData != null && oldData.Used)
            {
                if (oldData.Name != Product.Name)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Id), ErrorCode.ProductInUsed);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Product.Name))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Name), ErrorCode.NameEmpty);
                }
                else if (Product.Name.Length > 3000)
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Name), ErrorCode.NameOverLength);
                }
            }

            return Product.IsValidated;
        }
        private async Task<bool> ValidateCode(Product Product)
        {
            var oldData = await UOW.ProductRepository.Get(Product.Id);
            if (oldData != null && oldData.Used)
            {
                if (oldData.Code != Product.Code)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Id), ErrorCode.ProductInUsed);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Product.Code))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Code), ErrorCode.CodeEmpty);
                }
                else
                {
                    var Code = Product.Code;
                    if (Product.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Product.Code))
                    {
                        Product.AddError(nameof(ProductValidator), nameof(Product.Code), ErrorCode.CodeHasSpecialCharacter);
                    }

                    ProductFilter ProductFilter = new ProductFilter
                    {
                        Skip = 0,
                        Take = 10,
                        Id = new IdFilter { NotEqual = Product.Id },
                        Code = new StringFilter { Equal = Product.Code },
                        Selects = ProductSelect.Code
                    };

                    int count = await UOW.ProductRepository.Count(ProductFilter);
                    if (count != 0)
                        Product.AddError(nameof(ProductValidator), nameof(Product.Code), ErrorCode.CodeExisted);
                }
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateCategory(Product Product)
        {
            if (Product.CategoryId == 0)
                Product.AddError(nameof(ProductValidator), nameof(Product.Category), ErrorCode.CategoryEmpty);
            else
            {
                CategoryFilter CategoryFilter = new CategoryFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.CategoryId },
                    Selects = CategorySelect.Id
                };

                int count = await UOW.CategoryRepository.Count(CategoryFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Category), ErrorCode.CategoryNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateProductType(Product Product)
        {
            if (Product.ProductTypeId == 0)
                Product.AddError(nameof(ProductValidator), nameof(Product.ProductType), ErrorCode.ProductTypeEmpty);
            else
            {
                ProductTypeFilter ProductTypeFilter = new ProductTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.ProductTypeId },
                    Selects = ProductTypeSelect.Id
                };

                int count = await UOW.ProductTypeRepository.Count(ProductTypeFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.ProductType), ErrorCode.ProductTypeNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateSupplier(Product Product)
        {
            if (Product.SupplierId.HasValue)
            {
                SupplierFilter SupplierFilter = new SupplierFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.SupplierId },
                    Selects = SupplierSelect.Id
                };

                int count = await UOW.SupplierRepository.Count(SupplierFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Supplier), ErrorCode.SupplierNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateBrand(Product Product)
        {
            if (Product.BrandId.HasValue)
            {
                BrandFilter BrandFilter = new BrandFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.BrandId },
                    Selects = BrandSelect.Id
                };

                int count = await UOW.BrandRepository.Count(BrandFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Brand), ErrorCode.BrandNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateERPCode(Product Product)
        {
            if (!string.IsNullOrWhiteSpace(Product.ERPCode))
            {
                var ERPCode = Product.ERPCode;
                if (Product.ERPCode.Contains(" ") || !ERPCode.ChangeToEnglishChar().Equals(Product.ERPCode))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.ERPCode), ErrorCode.ERPCodeHasSpecialCharacter);
                }
            }
            return Product.IsValidated;
        }

        private async Task<bool> ValidateScanCode(Product Product)
        {
            if (!string.IsNullOrWhiteSpace(Product.ScanCode))
            {
                var ScanCode = Product.ScanCode;
                if (Product.ScanCode.Contains(" ") || !ScanCode.ChangeToEnglishChar().Equals(Product.ScanCode))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.ScanCode), ErrorCode.ScanCodeHasSpecialCharacter);
                }
            }
            return Product.IsValidated;
        }

        private async Task<bool> ValidateTaxType(Product Product)
        {
            if (Product.TaxTypeId == 0)
            {
                Product.AddError(nameof(ProductValidator), nameof(Product.TaxType), ErrorCode.TaxTypeEmpty);
            }
            else
            {
                TaxTypeFilter TaxTypeFilter = new TaxTypeFilter
                {
                    Skip = 0,
                    Take = 10,
                    Selects = TaxTypeSelect.Id,
                    Id = new IdFilter { Equal = Product.TaxTypeId }
                };

                int count = await UOW.TaxTypeRepository.Count(TaxTypeFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.TaxType), ErrorCode.TaxTypeNotExisted);
            }
            return Product.IsValidated;
        }

        private async Task<bool> ValidateUnitOfMeasure(Product Product)
        {
            if (Product.UnitOfMeasureId == 0)
                Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasure), ErrorCode.UnitOfMeasureEmpty);
            else
            {
                UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.UnitOfMeasureId },
                    Selects = UnitOfMeasureSelect.Id
                };

                int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateUnitOfMeasureGrouping(Product Product)
        {
            if (Product.UnitOfMeasureGroupingId.HasValue)
            {
                UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter = new UnitOfMeasureGroupingFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Product.UnitOfMeasureGroupingId.Value },
                    UnitOfMeasureId = new IdFilter { Equal = Product.UnitOfMeasureId },
                    Selects = UnitOfMeasureGroupingSelect.Id
                };

                int count = await UOW.UnitOfMeasureGroupingRepository.Count(UnitOfMeasureGroupingFilter);
                if (count == 0)
                    Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasureGrouping), ErrorCode.UnitOfMeasureGroupingNotExisted);
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateStatusId(Product Product)
        {
            if (StatusEnum.ACTIVE.Id != Product.StatusId && StatusEnum.INACTIVE.Id != Product.StatusId)
                Product.AddError(nameof(ProductValidator), nameof(Product.Status), ErrorCode.StatusEmpty);
            return Product.IsValidated;
        }

        private async Task<bool> ValidateUsedVariation(Product Product)
        {
            if (UsedVariationEnum.USED.Id != Product.UsedVariationId && UsedVariationEnum.NOTUSED.Id != Product.UsedVariationId)
                Product.AddError(nameof(ProductValidator), nameof(Product.UsedVariation), ErrorCode.UsedVariationNotExisted);
            else if (UsedVariationEnum.USED.Id == Product.UsedVariationId)
            {
                if (Product.VariationGroupings == null)
                    Product.AddError(nameof(ProductValidator), nameof(Product.VariationGroupings), ErrorCode.VariationGroupingsEmpty);
                else
                {
                    foreach (var VariationGrouping in Product.VariationGroupings)
                    {
                        if (VariationGrouping.Variations == null || !VariationGrouping.Variations.Any())
                            VariationGrouping.AddError(nameof(ProductValidator), nameof(VariationGrouping.Variations), ErrorCode.VariationsEmpty);
                        else
                        {
                            foreach (var Variation in VariationGrouping.Variations)
                            {
                                var Code = Variation.Code;
                                if (Variation.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Variation.Code))
                                {
                                    Variation.AddError(nameof(ProductValidator), nameof(Variation.Code), ErrorCode.CodeHasSpecialCharacter);
                                }
                            }
                        }
                    }
                }

                if (Product.Items == null || !Product.Items.Any())
                    Product.AddError(nameof(ProductValidator), nameof(Product.Items), ErrorCode.ItemsEmpty);
                foreach (var item in Product.Items)
                {
                    if (item.SalePrice == 0)
                    {
                        item.AddError(nameof(ProductValidator), nameof(item.SalePrice), ErrorCode.SalePriceEmpty);
                    }
                }
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateItem(Product Product)
        {
            return Product.IsValidated;
            if (Product.Items != null)
            {
                foreach (var item in Product.Items)
                {
                    ItemFilter ItemFilter = new ItemFilter
                    {
                        Id = new IdFilter { NotEqual = item.Id },
                        Code = new StringFilter { Equal = item.Code },
                        ProductId = new IdFilter { Equal = Product.Id }
                    };

                    int count = await UOW.ItemRepository.Count(ItemFilter);
                    if (count > 0)
                    {
                        item.AddError(nameof(ProductValidator), nameof(Item.Code), ErrorCode.CodeExisted);
                    }

                    ItemFilter = new ItemFilter
                    {
                        Id = new IdFilter { NotEqual = item.Id },
                        Name = new StringFilter { Equal = item.Name },
                        ProductId = new IdFilter { Equal = Product.Id }
                    };

                    count = await UOW.ItemRepository.Count(ItemFilter);
                    if (count > 0)
                    {
                        item.AddError(nameof(ProductValidator), nameof(Item.Name), ErrorCode.NameExisted);
                    }
                }
            }

            return Product.IsValidated;
        }

        private async Task<bool> ValidateSalePrice(Product Product)
        {
            if (Product.SalePrice < 0)
            {
                Product.AddError(nameof(ProductValidator), nameof(Product.SalePrice), ErrorCode.SalePriceInvalid);
            }

            if (Product.Items != null)
            {
                foreach (var item in Product.Items)
                {
                    if (item.SalePrice < 0)
                    {
                        item.AddError(nameof(ProductValidator), nameof(item.SalePrice), ErrorCode.SalePriceInvalid);
                    }
                }
            }
            return Product.IsValidated;
        }

        private async Task<bool> ValidateRetailPrice(Product Product)
        {
            if (Product.RetailPrice.HasValue)
            {
                if (Product.RetailPrice < 0)
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.RetailPrice), ErrorCode.RetailPriceInvalid);
                }
            }


            if (Product.Items != null)
            {
                foreach (var item in Product.Items)
                {
                    if (item.RetailPrice.HasValue)
                    {
                        if (item.RetailPrice < 0)
                        {
                            item.AddError(nameof(ProductValidator), nameof(item.RetailPrice), ErrorCode.RetailPriceInvalid);
                        }
                    }

                }
            }
            return Product.IsValidated;
        }

        private async Task<bool> CanDelete(Product Product)
        {
            IndirectSalesOrderContentFilter IndirectSalesOrderContentFilter = new IndirectSalesOrderContentFilter
            {
                ProductId = new IdFilter { Equal = Product.Id }
            };
            var count1 = await UOW.IndirectSalesOrderContentRepository.Count(IndirectSalesOrderContentFilter);
            IndirectSalesOrderPromotionFilter IndirectSalesOrderPromotionFilter = new IndirectSalesOrderPromotionFilter
            {
                ProductId = new IdFilter { Equal = Product.Id }
            };
            var count2 = await UOW.IndirectSalesOrderPromotionRepository.Count(IndirectSalesOrderPromotionFilter);
            DirectSalesOrderContentFilter DirectSalesOrderContentFilter = new DirectSalesOrderContentFilter
            {
                ProductId = new IdFilter { Equal = Product.Id }
            };
            var count3 = await UOW.DirectSalesOrderContentRepository.Count(DirectSalesOrderContentFilter);
            DirectSalesOrderPromotionFilter DirectSalesOrderPromotionFilter = new DirectSalesOrderPromotionFilter
            {
                ProductId = new IdFilter { Equal = Product.Id }
            };
            var count4 = await UOW.DirectSalesOrderPromotionRepository.Count(DirectSalesOrderPromotionFilter);

            if (count1 == 0 && count2 == 0 && count3 == 0 && count4 == 0)
                return Product.IsValidated;
            else
                Product.AddError(nameof(ProductValidator), nameof(Product.Id), ErrorCode.ProductInUsed);
            return Product.IsValidated;
        }
        public async Task<bool> Create(Product Product)
        {
            await ValidateCode(Product);
            await ValidateName(Product);
            await ValidateCategory(Product);
            await ValidateProductType(Product);
            await ValidateSupplier(Product);
            await ValidateBrand(Product);
            await ValidateTaxType(Product);
            await ValidateUnitOfMeasure(Product);
            await ValidateERPCode(Product);
            await ValidateScanCode(Product);
            await ValidateUnitOfMeasureGrouping(Product);
            await ValidateStatusId(Product);
            await ValidateItem(Product);
            await ValidateUsedVariation(Product);
            await ValidateSalePrice(Product);
            await ValidateRetailPrice(Product);
            return Product.IsValidated;
        }
        public async Task<bool> BulkMergeNewProduct(List<Product> Products)
        {
            foreach (Product Product in Products)
            {
                await ValidateId(Product);
            }
            return Products.All(st => st.IsValidated);
        }

        public async Task<bool> Update(Product Product)
        {
            if (await ValidateId(Product))
            {
                await ValidateCode(Product);
                await ValidateName(Product);
                await ValidateCategory(Product);
                await ValidateProductType(Product);
                await ValidateSupplier(Product);
                await ValidateBrand(Product);
                await ValidateTaxType(Product);
                await ValidateERPCode(Product);
                await ValidateScanCode(Product);
                await ValidateUnitOfMeasure(Product);
                await ValidateUnitOfMeasureGrouping(Product);
                await ValidateStatusId(Product);
                await ValidateItem(Product);
                await ValidateUsedVariation(Product);
                await ValidateSalePrice(Product);
                await ValidateRetailPrice(Product);
            }
            return Product.IsValidated;
        }

        public async Task<bool> Delete(Product Product)
        {
            if (await ValidateId(Product))
            {
                if (Product.Used)
                    Product.AddError(nameof(ProductValidator), nameof(Product.Id), ErrorCode.ProductInUsed);
            }
            return Product.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Product> Products)
        {
            foreach (Product Product in Products)
            {
                await Delete(Product);
            }
            return Products.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<Product> Products)
        {
            var listCodeInDB = (await UOW.ProductRepository.List(new ProductFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listCategoryCodeInDB = (await UOW.CategoryRepository.List(new CategoryFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = CategorySelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listProductTypeCodeInDB = (await UOW.ProductTypeRepository.List(new ProductTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductTypeSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listSupplierCodeInDB = (await UOW.SupplierRepository.List(new SupplierFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = SupplierSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listBrandCodeInDB = (await UOW.BrandRepository.List(new BrandFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = BrandSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listTaxTypeCodeInDB = (await UOW.TaxTypeRepository.List(new TaxTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = TaxTypeSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var listUsedVariationCodeInDB = (await UOW.UsedVariationRepository.List(new UsedVariationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UsedVariationSelect.Code
            })).Select(e => e.Code.ToLower()).ToList();
            var UOMs = await UOW.UnitOfMeasureRepository.List(new UnitOfMeasureFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureSelect.Code | UnitOfMeasureSelect.Id
            });
            var listUOMCodeInDB = UOMs.Select(e => e.Code.ToLower()).ToList();
            var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Code | UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure
            });
            var listUOMGroupingCodeInDB = UOMGs.Select(e => e.Code.ToLower()).ToList();

            foreach (var Product in Products)
            {
                var Code = Product.Code;
                if (Product.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Product.Code))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Code), ErrorCode.CodeHasSpecialCharacter);
                }
                else if (listCodeInDB.Contains(Product.Code))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Code), ErrorCode.CodeExisted);
                }
                if (!listCategoryCodeInDB.Contains(Product.Category.Code.ToLower()))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.Category), ErrorCode.CategoryNotExisted);
                }
                if (!listProductTypeCodeInDB.Contains(Product.ProductType.Code.ToLower()))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.ProductType), ErrorCode.ProductTypeNotExisted);
                }
                if (Product.Supplier != null)
                {
                    if (!listSupplierCodeInDB.Contains(Product.Supplier.Code.ToLower()))
                    {
                        Product.AddError(nameof(ProductValidator), nameof(Product.Supplier), ErrorCode.SupplierNotExisted);
                    }
                }
                if (Product.Brand != null)
                {
                    if (!listBrandCodeInDB.Contains(Product.Brand.Code.ToLower()))
                    {
                        Product.AddError(nameof(ProductValidator), nameof(Product.Brand), ErrorCode.BrandNotExisted);
                    }
                }
                if (!listTaxTypeCodeInDB.Contains(Product.TaxType.Code.ToLower()))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.TaxType), ErrorCode.TaxTypeNotExisted);
                }
                if (!listUOMCodeInDB.Contains(Product.UnitOfMeasure.Code.ToLower()))
                {
                    Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
                }
                if (Product.UnitOfMeasureGrouping != null)
                {
                    if (!listUOMGroupingCodeInDB.Contains(Product.UnitOfMeasureGrouping.Code.ToLower()))
                    {
                        Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasureGrouping), ErrorCode.UnitOfMeasureGroupingNotExisted);
                    }
                    var UOMG = UOMGs.Where(x => x.Code == Product.UnitOfMeasureGrouping.Code && x.UnitOfMeasureId == Product.UnitOfMeasureId).FirstOrDefault();
                    if (UOMG == null)
                    {
                        Product.AddError(nameof(ProductValidator), nameof(Product.UnitOfMeasureGrouping), ErrorCode.UnitOfMeasureGroupingInvalid);
                    }
                }

                await ValidateName(Product);
                await ValidateERPCode(Product);
                await ValidateScanCode(Product);
                await ValidateSalePrice(Product);
                await ValidateRetailPrice(Product);
                if (UsedVariationEnum.USED.Id != Product.UsedVariationId && UsedVariationEnum.NOTUSED.Id != Product.UsedVariationId)
                    Product.AddError(nameof(ProductValidator), nameof(Product.UsedVariation), ErrorCode.UsedVariationNotExisted);
                if (Product.VariationGroupings != null)
                {
                    foreach (var VariationGrouping in Product.VariationGroupings)
                    {
                        if (VariationGrouping.Variations == null || !VariationGrouping.Variations.Any())
                            VariationGrouping.AddError(nameof(ProductValidator), nameof(VariationGrouping.Variations), ErrorCode.VariationsEmpty);
                        else
                        {
                            foreach (var Variation in VariationGrouping.Variations)
                            {
                                var vgCode = Variation.Code;
                                if (Variation.Code.Contains(" ") || !vgCode.ChangeToEnglishChar().Equals(Variation.Code))
                                {
                                    Variation.AddError(nameof(ProductValidator), nameof(Variation.Code), ErrorCode.CodeHasSpecialCharacter);
                                }
                            }
                        }
                    }
                }
                await ValidateStatusId(Product);
            }
            return Products.Any(s => !s.IsValidated) ? false : true;
        }
    }
}
