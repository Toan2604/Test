using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IProductGroupingRepository
    {
        Task<int> Count(ProductGroupingFilter ProductGroupingFilter);
        Task<List<ProductGrouping>> List(ProductGroupingFilter ProductGroupingFilter);
        Task<ProductGrouping> Get(long Id);
    }
    public class ProductGroupingRepository : IProductGroupingRepository
    {
        private DataContext DataContext;
        public ProductGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProductGroupingDAO> DynamicFilter(IQueryable<ProductGroupingDAO> query, ProductGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ParentId, filter.ParentId);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Description, filter.Description);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<ProductGroupingDAO> OrFilter(IQueryable<ProductGroupingDAO> query, ProductGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProductGroupingDAO> initQuery = query.Where(q => false);
            foreach (ProductGroupingFilter ProductGroupingFilter in filter.OrFilter)
            {
                IQueryable<ProductGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProductGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, ProductGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, ProductGroupingFilter.Name);
                queryable = queryable.Where(q => q.ParentId, ProductGroupingFilter.ParentId);
                queryable = queryable.Where(q => q.Path, ProductGroupingFilter.Path);
                queryable = queryable.Where(q => q.Description, ProductGroupingFilter.Description);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProductGroupingDAO> DynamicOrder(IQueryable<ProductGroupingDAO> query, ProductGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProductGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProductGroupingOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProductGroupingOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProductGroupingOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case ProductGroupingOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case ProductGroupingOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProductGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProductGroupingOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProductGroupingOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProductGroupingOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case ProductGroupingOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case ProductGroupingOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ProductGrouping>> DynamicSelect(IQueryable<ProductGroupingDAO> query, ProductGroupingFilter filter)
        {
            List<ProductGrouping> ProductGroupings = await query.Select(q => new ProductGrouping()
            {
                Id = filter.Selects.Contains(ProductGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProductGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProductGroupingSelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(ProductGroupingSelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(ProductGroupingSelect.Path) ? q.Path : default(string),
                Description = filter.Selects.Contains(ProductGroupingSelect.Description) ? q.Description : default(string),
                Parent = filter.Selects.Contains(ProductGroupingSelect.Parent) && q.Parent != null ? new ProductGrouping
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Description = q.Parent.Description,
                } : null,
            }).ToListAsync();
            return ProductGroupings;
        }

        public async Task<int> Count(ProductGroupingFilter filter)
        {
            IQueryable<ProductGroupingDAO> ProductGroupings = DataContext.ProductGrouping;
            ProductGroupings = DynamicFilter(ProductGroupings, filter);
            return await ProductGroupings.CountAsync();
        }

        public async Task<List<ProductGrouping>> List(ProductGroupingFilter filter)
        {
            if (filter == null) return new List<ProductGrouping>();
            IQueryable<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping;
            ProductGroupingDAOs = DynamicFilter(ProductGroupingDAOs, filter);
            ProductGroupingDAOs = DynamicOrder(ProductGroupingDAOs, filter);
            List<ProductGrouping> ProductGroupings = await DynamicSelect(ProductGroupingDAOs, filter);
            return ProductGroupings;
        }

        public async Task<ProductGrouping> Get(long Id)
        {
            ProductGrouping ProductGrouping = await DataContext.ProductGrouping.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new ProductGrouping()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Path = x.Path,
                    Description = x.Description,
                    Parent = x.Parent == null ? null : new ProductGrouping
                    {
                        Id = x.Parent.Id,
                        Code = x.Parent.Code,
                        Name = x.Parent.Name,
                        ParentId = x.Parent.ParentId,
                        Path = x.Parent.Path,
                        Description = x.Parent.Description,
                    },
                }).FirstOrDefaultAsync();

            if (ProductGrouping == null)
                return null;
            ProductGrouping.ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping.Include(x => x.Product)
                .Where(x => x.ProductGrouping.Id == Id && x.Product.DeletedAt == null)
                .Select(x => new ProductProductGroupingMapping
                {
                    ProductId = x.ProductId,
                    ProductGroupingId = x.ProductGroupingId,
                    Product = new Product
                    {
                        Id = x.Product.Id,
                        Code = x.Product.Code,
                        Name = x.Product.Name,
                        Description = x.Product.Description,
                        ScanCode = x.Product.ScanCode,
                        ProductTypeId = x.Product.ProductTypeId,
                        BrandId = x.Product.BrandId,
                        UnitOfMeasureId = x.Product.UnitOfMeasureId,
                        UnitOfMeasureGroupingId = x.Product.UnitOfMeasureGroupingId,
                        SalePrice = x.Product.SalePrice,
                        RetailPrice = x.Product.RetailPrice,
                        TaxTypeId = x.Product.TaxTypeId,
                        StatusId = x.Product.StatusId,
                        IsNew = x.Product.IsNew,
                        Brand = x.Product.Brand == null ? null : new Brand
                        {
                            Id = x.Product.Brand.Id,
                            Code = x.Product.Brand.Code,
                            Name = x.Product.Brand.Name,
                            Description = x.Product.Brand.Description,
                            StatusId = x.Product.Brand.StatusId,
                        },
                        ProductType = x.Product.ProductType == null ? null : new ProductType
                        {
                            Id = x.Product.ProductType.Id,
                            Code = x.Product.ProductType.Code,
                            Name = x.Product.ProductType.Name,
                            Description = x.Product.ProductType.Description,
                            StatusId = x.Product.ProductType.StatusId,
                        },
                        Status = x.Product.Status == null ? null : new Status
                        {
                            Id = x.Product.Status.Id,
                            Code = x.Product.Status.Code,
                            Name = x.Product.Status.Name,
                        },
                        TaxType = x.Product.TaxType == null ? null : new TaxType
                        {
                            Id = x.Product.TaxType.Id,
                            Code = x.Product.TaxType.Code,
                            Name = x.Product.TaxType.Name,
                            Percentage = x.Product.TaxType.Percentage,
                            StatusId = x.Product.TaxType.StatusId,
                        },
                        UnitOfMeasure = x.Product.UnitOfMeasure == null ? null : new UnitOfMeasure
                        {
                            Id = x.Product.UnitOfMeasure.Id,
                            Code = x.Product.UnitOfMeasure.Code,
                            Name = x.Product.UnitOfMeasure.Name,
                            Description = x.Product.UnitOfMeasure.Description,
                            StatusId = x.Product.UnitOfMeasure.StatusId,
                        },
                        UnitOfMeasureGrouping = x.Product.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                        {
                            Id = x.Product.UnitOfMeasureGrouping.Id,
                            Name = x.Product.UnitOfMeasureGrouping.Name,
                            UnitOfMeasureId = x.Product.UnitOfMeasureGrouping.UnitOfMeasureId,
                            StatusId = x.Product.UnitOfMeasureGrouping.StatusId
                        },
                    },
                }).ToListAsync();

            return ProductGrouping;
        }
    }
}
