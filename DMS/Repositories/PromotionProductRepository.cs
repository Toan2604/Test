using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IPromotionProductRepository
    {
        Task<int> Count(PromotionProductFilter PromotionProductFilter);
        Task<int> CountAll(PromotionProductFilter PromotionProductFilter);
        Task<List<PromotionProduct>> List(PromotionProductFilter PromotionProductFilter);
        Task<PromotionProduct> Get(long Id);
        Task<bool> Create(PromotionProduct PromotionProduct);
        Task<bool> Update(PromotionProduct PromotionProduct);
        Task<bool> Delete(PromotionProduct PromotionProduct);
        Task<bool> BulkMerge(List<PromotionProduct> PromotionProducts);
        Task<bool> BulkDelete(List<PromotionProduct> PromotionProducts);
    }
    public class PromotionProductRepository : IPromotionProductRepository
    {
        private DataContext DataContext;
        public PromotionProductRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionProductDAO> DynamicFilter(IQueryable<PromotionProductDAO> query, PromotionProductFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PromotionPolicyId, filter.PromotionPolicyId);
            query = query.Where(q => q.PromotionId, filter.PromotionId);
            query = query.Where(q => q.ProductId, filter.ProductId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.FromValue, filter.FromValue);
            query = query.Where(q => q.ToValue, filter.ToValue);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountValue, filter.DiscountValue);
            return query;
        }

        private IQueryable<PromotionProductDAO> OrFilter(IQueryable<PromotionProductDAO> query, PromotionProductFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionProductDAO> initQuery = query.Where(q => false);
            foreach (PromotionProductFilter PromotionProductFilter in filter.OrFilter)
            {
                IQueryable<PromotionProductDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionProductFilter.Id);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionProductFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionProductFilter.PromotionId);
                queryable = queryable.Where(q => q.ProductId, PromotionProductFilter.ProductId);
                queryable = queryable.Where(q => q.Note, PromotionProductFilter.Note);
                queryable = queryable.Where(q => q.FromValue, PromotionProductFilter.FromValue);
                queryable = queryable.Where(q => q.ToValue, PromotionProductFilter.ToValue);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionProductFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionProductFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionProductFilter.DiscountValue);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionProductDAO> DynamicOrder(IQueryable<PromotionProductDAO> query, PromotionProductFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionProductOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionProductOrder.Product:
                            query = query.OrderBy(q => q.ProductId);
                            break;
                        case PromotionProductOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionProductOrder.FromValue:
                            query = query.OrderBy(q => q.FromValue);
                            break;
                        case PromotionProductOrder.ToValue:
                            query = query.OrderBy(q => q.ToValue);
                            break;
                        case PromotionProductOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionProductOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionProductOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionProductOrder.Product:
                            query = query.OrderByDescending(q => q.ProductId);
                            break;
                        case PromotionProductOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionProductOrder.FromValue:
                            query = query.OrderByDescending(q => q.FromValue);
                            break;
                        case PromotionProductOrder.ToValue:
                            query = query.OrderByDescending(q => q.ToValue);
                            break;
                        case PromotionProductOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionProductOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionProduct>> DynamicSelect(IQueryable<PromotionProductDAO> query, PromotionProductFilter filter)
        {
            List<PromotionProduct> PromotionProducts = await query.Select(q => new PromotionProduct()
            {
                Id = filter.Selects.Contains(PromotionProductSelect.Id) ? q.Id : default(long),
                PromotionPolicyId = filter.Selects.Contains(PromotionProductSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionProductSelect.Promotion) ? q.PromotionId : default(long),
                ProductId = filter.Selects.Contains(PromotionProductSelect.Product) ? q.ProductId : default(long),
                Note = filter.Selects.Contains(PromotionProductSelect.Note) ? q.Note : default(string),
                FromValue = filter.Selects.Contains(PromotionProductSelect.FromValue) ? q.FromValue : default(decimal),
                ToValue = filter.Selects.Contains(PromotionProductSelect.ToValue) ? q.ToValue : default(decimal),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionProductSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionProductSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionProductSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                Product = filter.Selects.Contains(PromotionProductSelect.Product) && q.Product != null ? new Product
                {
                    Id = q.Product.Id,
                    Code = q.Product.Code,
                    Name = q.Product.Name,
                    Description = q.Product.Description,
                    ScanCode = q.Product.ScanCode,
                    ERPCode = q.Product.ERPCode,
                    ProductTypeId = q.Product.ProductTypeId,
                    BrandId = q.Product.BrandId,
                    UnitOfMeasureId = q.Product.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = q.Product.UnitOfMeasureGroupingId,
                    SalePrice = q.Product.SalePrice,
                    RetailPrice = q.Product.RetailPrice,
                    TaxTypeId = q.Product.TaxTypeId,
                    StatusId = q.Product.StatusId,
                    OtherName = q.Product.OtherName,
                    TechnicalName = q.Product.TechnicalName,
                    Note = q.Product.Note,
                    IsNew = q.Product.IsNew,
                    UsedVariationId = q.Product.UsedVariationId,
                    Used = q.Product.Used,
                } : null,
                Promotion = filter.Selects.Contains(PromotionProductSelect.Promotion) && q.Promotion != null ? new Promotion
                {
                    Id = q.Promotion.Id,
                    Code = q.Promotion.Code,
                    Name = q.Promotion.Name,
                    StartDate = q.Promotion.StartDate,
                    EndDate = q.Promotion.EndDate,
                    OrganizationId = q.Promotion.OrganizationId,
                    PromotionTypeId = q.Promotion.PromotionTypeId,
                    Note = q.Promotion.Note,
                    Priority = q.Promotion.Priority,
                    StatusId = q.Promotion.StatusId,
                } : null,
                PromotionDiscountType = filter.Selects.Contains(PromotionProductSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionProductSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionProducts;
        }

        public async Task<int> Count(PromotionProductFilter filter)
        {
            IQueryable<PromotionProductDAO> PromotionProducts = DataContext.PromotionProduct.AsNoTracking();
            PromotionProducts = DynamicFilter(PromotionProducts, filter);
            PromotionProducts = OrFilter(PromotionProducts, filter);
            return await PromotionProducts.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionProductFilter filter)
        {
            IQueryable<PromotionProductDAO> PromotionProducts = DataContext.PromotionProduct.AsNoTracking();
            PromotionProducts = DynamicFilter(PromotionProducts, filter);
            return await PromotionProducts.CountWithNoLockAsync();
        }

        public async Task<List<PromotionProduct>> List(PromotionProductFilter filter)
        {
            if (filter == null) return new List<PromotionProduct>();
            IQueryable<PromotionProductDAO> PromotionProductDAOs = DataContext.PromotionProduct.AsNoTracking();
            PromotionProductDAOs = DynamicFilter(PromotionProductDAOs, filter);
            PromotionProductDAOs = OrFilter(PromotionProductDAOs, filter);
            PromotionProductDAOs = DynamicOrder(PromotionProductDAOs, filter);
            List<PromotionProduct> PromotionProducts = await DynamicSelect(PromotionProductDAOs, filter);
            return PromotionProducts;
        }

        public async Task<PromotionProduct> Get(long Id)
        {
            PromotionProduct PromotionProduct = await DataContext.PromotionProduct.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionProduct()
            {
                Id = x.Id,
                PromotionPolicyId = x.PromotionPolicyId,
                PromotionId = x.PromotionId,
                ProductId = x.ProductId,
                Note = x.Note,
                FromValue = x.FromValue,
                ToValue = x.ToValue,
                PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                DiscountPercentage = x.DiscountPercentage,
                DiscountValue = x.DiscountValue,
                Product = x.Product == null ? null : new Product
                {
                    Id = x.Product.Id,
                    Code = x.Product.Code,
                    Name = x.Product.Name,
                    Description = x.Product.Description,
                    ScanCode = x.Product.ScanCode,
                    ERPCode = x.Product.ERPCode,
                    ProductTypeId = x.Product.ProductTypeId,
                    BrandId = x.Product.BrandId,
                    UnitOfMeasureId = x.Product.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = x.Product.UnitOfMeasureGroupingId,
                    SalePrice = x.Product.SalePrice,
                    RetailPrice = x.Product.RetailPrice,
                    TaxTypeId = x.Product.TaxTypeId,
                    StatusId = x.Product.StatusId,
                    OtherName = x.Product.OtherName,
                    TechnicalName = x.Product.TechnicalName,
                    Note = x.Product.Note,
                    IsNew = x.Product.IsNew,
                    UsedVariationId = x.Product.UsedVariationId,
                    Used = x.Product.Used,
                },
                Promotion = x.Promotion == null ? null : new Promotion
                {
                    Id = x.Promotion.Id,
                    Code = x.Promotion.Code,
                    Name = x.Promotion.Name,
                    StartDate = x.Promotion.StartDate,
                    EndDate = x.Promotion.EndDate,
                    OrganizationId = x.Promotion.OrganizationId,
                    PromotionTypeId = x.Promotion.PromotionTypeId,
                    Note = x.Promotion.Note,
                    Priority = x.Promotion.Priority,
                    StatusId = x.Promotion.StatusId,
                },
                PromotionDiscountType = x.PromotionDiscountType == null ? null : new PromotionDiscountType
                {
                    Id = x.PromotionDiscountType.Id,
                    Code = x.PromotionDiscountType.Code,
                    Name = x.PromotionDiscountType.Name,
                },
                PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                {
                    Id = x.PromotionPolicy.Id,
                    Code = x.PromotionPolicy.Code,
                    Name = x.PromotionPolicy.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionProduct == null)
                return null;
            PromotionProduct.PromotionProductItemMappings = await DataContext.PromotionProductItemMapping.AsNoTracking()
                .Where(x => x.PromotionProductId == PromotionProduct.Id)
                .Where(x => x.Item.DeletedAt == null)
                .Select(x => new PromotionProductItemMapping
                {
                    PromotionProductId = x.PromotionProductId,
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        ProductId = x.Item.ProductId,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ScanCode = x.Item.ScanCode,
                        SalePrice = x.Item.SalePrice,
                        RetailPrice = x.Item.RetailPrice,
                        StatusId = x.Item.StatusId,
                        Used = x.Item.Used,
                    },
                }).ToListWithNoLockAsync();

            return PromotionProduct;
        }
        public async Task<bool> Create(PromotionProduct PromotionProduct)
        {
            PromotionProductDAO PromotionProductDAO = new PromotionProductDAO();
            PromotionProductDAO.Id = PromotionProduct.Id;
            PromotionProductDAO.PromotionPolicyId = PromotionProduct.PromotionPolicyId;
            PromotionProductDAO.PromotionId = PromotionProduct.PromotionId;
            PromotionProductDAO.ProductId = PromotionProduct.ProductId;
            PromotionProductDAO.Note = PromotionProduct.Note;
            PromotionProductDAO.FromValue = PromotionProduct.FromValue;
            PromotionProductDAO.ToValue = PromotionProduct.ToValue;
            PromotionProductDAO.PromotionDiscountTypeId = PromotionProduct.PromotionDiscountTypeId;
            PromotionProductDAO.DiscountPercentage = PromotionProduct.DiscountPercentage;
            PromotionProductDAO.DiscountValue = PromotionProduct.DiscountValue;
            DataContext.PromotionProduct.Add(PromotionProductDAO);
            await DataContext.SaveChangesAsync();
            PromotionProduct.Id = PromotionProductDAO.Id;
            await SaveReference(PromotionProduct);
            return true;
        }

        public async Task<bool> Update(PromotionProduct PromotionProduct)
        {
            PromotionProductDAO PromotionProductDAO = DataContext.PromotionProduct.Where(x => x.Id == PromotionProduct.Id).FirstOrDefault();
            if (PromotionProductDAO == null)
                return false;
            PromotionProductDAO.Id = PromotionProduct.Id;
            PromotionProductDAO.PromotionPolicyId = PromotionProduct.PromotionPolicyId;
            PromotionProductDAO.PromotionId = PromotionProduct.PromotionId;
            PromotionProductDAO.ProductId = PromotionProduct.ProductId;
            PromotionProductDAO.Note = PromotionProduct.Note;
            PromotionProductDAO.FromValue = PromotionProduct.FromValue;
            PromotionProductDAO.ToValue = PromotionProduct.ToValue;
            PromotionProductDAO.PromotionDiscountTypeId = PromotionProduct.PromotionDiscountTypeId;
            PromotionProductDAO.DiscountPercentage = PromotionProduct.DiscountPercentage;
            PromotionProductDAO.DiscountValue = PromotionProduct.DiscountValue;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionProduct);
            return true;
        }

        public async Task<bool> Delete(PromotionProduct PromotionProduct)
        {
            await DataContext.PromotionProduct.Where(x => x.Id == PromotionProduct.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionProduct> PromotionProducts)
        {
            List<PromotionProductDAO> PromotionProductDAOs = new List<PromotionProductDAO>();
            foreach (PromotionProduct PromotionProduct in PromotionProducts)
            {
                PromotionProductDAO PromotionProductDAO = new PromotionProductDAO();
                PromotionProductDAO.Id = PromotionProduct.Id;
                PromotionProductDAO.PromotionPolicyId = PromotionProduct.PromotionPolicyId;
                PromotionProductDAO.PromotionId = PromotionProduct.PromotionId;
                PromotionProductDAO.ProductId = PromotionProduct.ProductId;
                PromotionProductDAO.Note = PromotionProduct.Note;
                PromotionProductDAO.FromValue = PromotionProduct.FromValue;
                PromotionProductDAO.ToValue = PromotionProduct.ToValue;
                PromotionProductDAO.PromotionDiscountTypeId = PromotionProduct.PromotionDiscountTypeId;
                PromotionProductDAO.DiscountPercentage = PromotionProduct.DiscountPercentage;
                PromotionProductDAO.DiscountValue = PromotionProduct.DiscountValue;
                PromotionProductDAOs.Add(PromotionProductDAO);
            }
            await DataContext.BulkMergeAsync(PromotionProductDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionProduct> PromotionProducts)
        {
            List<long> Ids = PromotionProducts.Select(x => x.Id).ToList();
            await DataContext.PromotionProduct
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionProduct PromotionProduct)
        {
            await DataContext.PromotionProductItemMapping
                .Where(x => x.PromotionProductId == PromotionProduct.Id)
                .DeleteFromQueryAsync();
            List<PromotionProductItemMappingDAO> PromotionProductItemMappingDAOs = new List<PromotionProductItemMappingDAO>();
            if (PromotionProduct.PromotionProductItemMappings != null)
            {
                foreach (PromotionProductItemMapping PromotionProductItemMapping in PromotionProduct.PromotionProductItemMappings)
                {
                    PromotionProductItemMappingDAO PromotionProductItemMappingDAO = new PromotionProductItemMappingDAO();
                    PromotionProductItemMappingDAO.PromotionProductId = PromotionProduct.Id;
                    PromotionProductItemMappingDAO.ItemId = PromotionProductItemMapping.ItemId;
                    PromotionProductItemMappingDAO.Quantity = PromotionProductItemMapping.Quantity;
                    PromotionProductItemMappingDAOs.Add(PromotionProductItemMappingDAO);
                }
                await DataContext.PromotionProductItemMapping.BulkMergeAsync(PromotionProductItemMappingDAOs);
            }
        }

    }
}
