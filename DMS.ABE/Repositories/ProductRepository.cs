using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IProductRepository
    {
        Task<int> Count(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<Product> Get(long Id);
        Task<bool> BulkInsertNewProduct(List<Product> Products);
        Task<bool> BulkDeleteNewProduct(List<Product> Products);
    }
    public class ProductRepository : IProductRepository
    {
        private DataContext DataContext;
        public ProductRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProductDAO> DynamicFilter(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.ScanCode, filter.ScanCode);
            query = query.Where(q => q.ProductTypeId, filter.ProductTypeId);
            query = query.Where(q => q.BrandId, filter.BrandId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.UnitOfMeasureGroupingId, filter.UnitOfMeasureGroupingId);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.RetailPrice, filter.RetailPrice);
            query = query.Where(q => q.TaxTypeId, filter.TaxTypeId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.OtherName, filter.OtherName);
            query = query.Where(q => q.TechnicalName, filter.TechnicalName);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.UsedVariationId, filter.UsedVariationId);
            if (filter.ProductGroupingId != null)
            {
                if (filter.ProductGroupingId.Equal != null)
                {
                    ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                        .Where(o => o.Id == filter.ProductGroupingId.Equal.Value).FirstOrDefault();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where pg.Path.StartsWith(ProductGroupingDAO.Path)
                            select q;
                }
                if (filter.ProductGroupingId.NotEqual != null)
                {
                    ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                        .Where(o => o.Id == filter.ProductGroupingId.NotEqual.Value).FirstOrDefault();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where !pg.Path.StartsWith(ProductGroupingDAO.Path)
                            select q;
                }
                if (filter.ProductGroupingId.In != null)
                {
                    List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                        .Where(o => o.DeletedAt == null).ToList();
                    List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => filter.ProductGroupingId.In.Contains(o.Id)).ToList();
                    List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> ProductGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where ProductGroupingIds.Contains(pg.Id)
                            select q;
                }
                if (filter.ProductGroupingId.NotIn != null)
                {
                    List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                        .Where(o => o.DeletedAt == null).ToList();
                    List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => filter.ProductGroupingId.NotIn.Contains(o.Id)).ToList();
                    List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> ProductGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where !ProductGroupingIds.Contains(pg.Id)
                            select q;
                }
            } // filter theo productGrouping
            if (filter.CategoryId != null)
            {
                if (filter.CategoryId.Equal != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Category.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.CategoryId.NotEqual != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Category.Path.StartsWith(CategoryDAO.Path));
                }
                if (filter.CategoryId.In != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.In.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    query = query.Where(q => Ids.Contains(q.CategoryId));
                }
                if (filter.CategoryId.NotIn != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.NotIn.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    query = query.Where(q => !Ids.Contains(q.CategoryId));
                }
            } // filter theo category
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                List<string> Tokens = filter.Search.Split(" ").Select(x => x.ToLower()).ToList();
                var queryForCode = query;
                var queryForName = query;
                var queryForOtherName = query;
                foreach (string Token in Tokens)
                {
                    if (string.IsNullOrWhiteSpace(Token))
                        continue;
                    queryForCode = queryForCode.Where(x => x.Code.ToLower().Contains(Token));
                    queryForName = queryForName.Where(x => x.Name.ToLower().Contains(Token));
                    queryForOtherName = queryForOtherName.Where(x => x.OtherName.ToLower().Contains(Token));
                }
                query = queryForCode.Union(queryForName).Union(queryForOtherName);
                query = query.Distinct();
            }
            query = query.Where(q => q.IsNew, filter.IsNew);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<ProductDAO> OrFilter(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProductDAO> initQuery = query.Where(q => false);
            foreach (ProductFilter ProductFilter in filter.OrFilter)
            {
                IQueryable<ProductDAO> queryable = query;
                    queryable = queryable.Where(q => q.ProductTypeId, ProductFilter.ProductTypeId);
                if (ProductFilter.ProductGroupingId != null)
                {
                    if (ProductFilter.ProductGroupingId.Equal != null)
                    {
                        ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                            .Where(o => o.Id == ProductFilter.ProductGroupingId.Equal.Value).FirstOrDefault();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where pg.Path.StartsWith(ProductGroupingDAO.Path)
                                    select q;
                    }
                    if (ProductFilter.ProductGroupingId.NotEqual != null)
                    {
                        ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                            .Where(o => o.Id == ProductFilter.ProductGroupingId.NotEqual.Value).FirstOrDefault();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where !pg.Path.StartsWith(ProductGroupingDAO.Path)
                                    select q;
                    }
                    if (ProductFilter.ProductGroupingId.In != null)
                    {
                        List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                            .Where(o => o.DeletedAt == null).ToList();
                        List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => ProductFilter.ProductGroupingId.In.Contains(o.Id)).ToList();
                        List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> ProductGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where ProductGroupingIds.Contains(pg.Id)
                                    select q;
                    }
                    if (ProductFilter.ProductGroupingId.NotIn != null)
                    {
                        List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                            .Where(o => o.DeletedAt == null).ToList();
                        List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => ProductFilter.ProductGroupingId.NotIn.Contains(o.Id)).ToList();
                        List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> ProductGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.Id equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where !ProductGroupingIds.Contains(pg.Id)
                                    select q;
                    }
                }

                    queryable = queryable.Where(q => q.IsNew, ProductFilter.IsNew);
                    queryable = queryable.Where(q => q.UsedVariationId, ProductFilter.UsedVariationId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProductDAO> DynamicOrder(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProductOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProductOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            query = query.OrderBy(q => q.ScanCode);
                            break;
                        case ProductOrder.Category:
                            query = query.OrderBy(q => q.Category.Name);
                            break;
                        case ProductOrder.ProductType:
                            query = query.OrderBy(q => q.ProductType.Name);
                            break;
                        case ProductOrder.Brand:
                            query = query.OrderBy(q => q.Brand.Name);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasure.Name);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            query = query.OrderBy(q => q.UnitOfMeasureGrouping.Name);
                            break;
                        case ProductOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            query = query.OrderBy(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            query = query.OrderBy(q => q.TaxType.Code);
                            break;
                        case ProductOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            query = query.OrderBy(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            query = query.OrderBy(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case ProductOrder.UsedVariation:
                            query = query.OrderBy(q => q.UsedVariationId);
                            break;
                        case ProductOrder.SalesOrderCounter:
                            query = query.OrderBy(q => q.SalesOrderCounter);
                            break;
                        case ProductOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                        default:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProductOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProductOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            query = query.OrderByDescending(q => q.ScanCode);
                            break;
                        case ProductOrder.Category:
                            query = query.OrderByDescending(q => q.Category.Name);
                            break;
                        case ProductOrder.ProductType:
                            query = query.OrderByDescending(q => q.ProductType.Name);
                            break;
                        case ProductOrder.Brand:
                            query = query.OrderByDescending(q => q.Brand.Name);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasure.Name);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            query = query.OrderByDescending(q => q.UnitOfMeasureGrouping.Name);
                            break;
                        case ProductOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            query = query.OrderByDescending(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            query = query.OrderByDescending(q => q.TaxTypeId);
                            break;
                        case ProductOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            query = query.OrderByDescending(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            query = query.OrderByDescending(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case ProductOrder.UsedVariation:
                            query = query.OrderByDescending(q => q.UsedVariationId);
                            break;
                        case ProductOrder.SalesOrderCounter:
                            query = query.OrderByDescending(q => q.SalesOrderCounter);
                            break;
                        case ProductOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Product>> DynamicSelect(IQueryable<ProductDAO> query, ProductFilter filter)
        {
            List<Product> Products = await query.Select(q => new Product()
            {
                Id = filter.Selects.Contains(ProductSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProductSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProductSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(ProductSelect.Description) ? q.Description : default(string),
                ScanCode = filter.Selects.Contains(ProductSelect.ScanCode) ? q.ScanCode : default(string),
                ERPCode = filter.Selects.Contains(ProductSelect.ERPCode) ? q.ERPCode : default(string),
                CategoryId = filter.Selects.Contains(ProductSelect.Category) ? q.CategoryId : default(long),
                ProductTypeId = filter.Selects.Contains(ProductSelect.ProductType) ? q.ProductTypeId : default(long),
                BrandId = filter.Selects.Contains(ProductSelect.Brand) ? q.BrandId : default(long?),
                UnitOfMeasureId = filter.Selects.Contains(ProductSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                UnitOfMeasureGroupingId = filter.Selects.Contains(ProductSelect.UnitOfMeasureGrouping) ? q.UnitOfMeasureGroupingId : default(long?),
                SalePrice = filter.Selects.Contains(ProductSelect.SalePrice) ? q.SalePrice : default(long),
                RetailPrice = filter.Selects.Contains(ProductSelect.RetailPrice) ? q.RetailPrice : default(long?),
                TaxTypeId = filter.Selects.Contains(ProductSelect.TaxType) ? q.TaxTypeId : default(long),
                StatusId = filter.Selects.Contains(ProductSelect.Status) ? q.StatusId : default(long),
                OtherName = filter.Selects.Contains(ProductSelect.OtherName) ? q.OtherName : default(string),
                TechnicalName = filter.Selects.Contains(ProductSelect.TechnicalName) ? q.TechnicalName : default(string),
                Note = filter.Selects.Contains(ProductSelect.Note) ? q.Note : default(string),
                IsNew = filter.Selects.Contains(ProductSelect.IsNew) ? q.IsNew : default(bool),
                UsedVariationId = filter.Selects.Contains(ProductSelect.UsedVariation) ? q.UsedVariationId : default(long),
                Brand = filter.Selects.Contains(ProductSelect.Brand) && q.Brand != null ? new Brand
                {
                    Id = q.Brand.Id,
                    Code = q.Brand.Code,
                    Name = q.Brand.Name,
                    Description = q.Brand.Description,
                    StatusId = q.Brand.StatusId,
                } : null,
                Category = filter.Selects.Contains(ProductSelect.Category) && q.Category != null ? new Category
                {
                    Id = q.Category.Id,
                    Code = q.Category.Code,
                    Name = q.Category.Name,
                    Path = q.Category.Path,
                    ParentId = q.Category.ParentId,
                    StatusId = q.Category.StatusId,
                    Level = q.Category.Level
                } : null,
                ProductType = filter.Selects.Contains(ProductSelect.ProductType) && q.ProductType != null ? new ProductType
                {
                    Id = q.ProductType.Id,
                    Code = q.ProductType.Code,
                    Name = q.ProductType.Name,
                    Description = q.ProductType.Description,
                    StatusId = q.ProductType.StatusId,
                } : null,
                Status = filter.Selects.Contains(ProductSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                TaxType = filter.Selects.Contains(ProductSelect.TaxType) && q.TaxType != null ? new TaxType
                {
                    Id = q.TaxType.Id,
                    Code = q.TaxType.Code,
                    Name = q.TaxType.Name,
                    Percentage = q.TaxType.Percentage,
                    StatusId = q.TaxType.StatusId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(ProductSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
                UnitOfMeasureGrouping = filter.Selects.Contains(ProductSelect.UnitOfMeasureGrouping) && q.UnitOfMeasureGrouping != null ? new UnitOfMeasureGrouping
                {
                    Id = q.UnitOfMeasureGrouping.Id,
                    Name = q.UnitOfMeasureGrouping.Name,
                    UnitOfMeasureId = q.UnitOfMeasureGrouping.UnitOfMeasureId,
                    StatusId = q.UnitOfMeasureGrouping.StatusId,
                } : null,
                UsedVariation = filter.Selects.Contains(ProductSelect.UsedVariation) && q.UsedVariation != null ? new UsedVariation
                {
                    Id = q.UsedVariation.Id,
                    Code = q.UsedVariation.Code,
                    Name = q.UsedVariation.Name,
                } : null,
                Used = q.Used,
                RowId = q.RowId,
            }).ToListAsync();



            //Lấy ra 1 cái ảnh cho list product
            var Ids = Products.Select(x => x.Id).ToList();
            IdFilter ProductIdFilter = new IdFilter { In = Ids };
            var ProductImageMappings = DataContext.ProductImageMapping.Include(x => x.Image).Where(x => Ids.Contains(x.ProductId)).ToList();
            foreach (var Product in Products)
            {
                Product.ProductImageMappings = new List<ProductImageMapping>();
                var ProductImageMappingDAOs = ProductImageMappings.Where(x => x.ProductId == Product.Id).ToList();
                if (ProductImageMappingDAOs != null)
                {
                    foreach (ProductImageMappingDAO ProductImageMappingDAO in ProductImageMappingDAOs)
                    {
                        ProductImageMapping ProductImageMapping = new ProductImageMapping
                        {
                            ImageId = ProductImageMappingDAO.ImageId,
                            ProductId = ProductImageMappingDAO.ProductId,
                            Image = new Image
                            {
                                Id = ProductImageMappingDAO.Image.Id,
                                Name = ProductImageMappingDAO.Image.Name,
                                Url = ProductImageMappingDAO.Image.Url,
                                ThumbnailUrl = ProductImageMappingDAO.Image.ThumbnailUrl,
                            }
                        };
                        Product.ProductImageMappings.Add(ProductImageMapping);
                    }
                }
            }
            if (filter.Selects.Contains(ProductSelect.ProductProductGroupingMapping))
            {
                List<ProductProductGroupingMapping> ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping.AsNoTracking()
                    .Where(x => x.ProductId, ProductIdFilter)
                    .Select(p => new ProductProductGroupingMapping
                    {
                        ProductId = p.ProductId,
                        ProductGroupingId = p.ProductGroupingId,
                        ProductGrouping = new ProductGrouping
                        {
                            Id = p.ProductGrouping.Id,
                            Code = p.ProductGrouping.Code,
                            Name = p.ProductGrouping.Name,
                            ParentId = p.ProductGrouping.ParentId,
                            Path = p.ProductGrouping.Path,
                            Description = p.ProductGrouping.Description,
                        },
                    })
                    .ToListAsync();
                foreach (Product Product in Products)
                {
                    Product.ProductProductGroupingMappings = ProductProductGroupingMappings
                        .Where(x => x.ProductId == Product.Id)
                        .ToList();
                }
            }
            if (filter.Selects.Contains(ProductSelect.VariationGrouping))
            {
                List<VariationGrouping> VariationGroupings = await DataContext.VariationGrouping.AsNoTracking()
                    .Where(x => x.ProductId, ProductIdFilter)
                    .Select(v => new VariationGrouping
                    {
                        ProductId = v.ProductId,
                        Name = v.Name,
                        Id = v.Id,
                        RowId = v.RowId,
                    })
                    .ToListAsync();
                foreach (Product Product in Products)
                {
                    Product.VariationGroupings = VariationGroupings
                        .Where(x => x.ProductId == Product.Id)
                        .ToList();
                }
                var VariationGroupingIds = VariationGroupings
                            .Select(x => x.Id)
                            .Distinct()
                            .ToList();
                IdFilter VariationGroupingIdFilter = new IdFilter { In = VariationGroupingIds };
                if (VariationGroupingIds != null)
                {
                    List<Variation> Variations = await DataContext.Variation.AsNoTracking()
                        .Where(x => x.VariationGroupingId, VariationGroupingIdFilter)
                        .Select(x => new Variation
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            VariationGroupingId = x.VariationGroupingId,
                            UpdatedAt = x.UpdatedAt,
                            CreatedAt = x.CreatedAt,
                            DeletedAt = x.DeletedAt
                        }).ToListAsync();
                    foreach (VariationGrouping VariationGrouping in VariationGroupings)
                    {
                        VariationGrouping.Variations = Variations
                            .Where(x => x.VariationGroupingId == VariationGrouping.Id)
                            .ToList();
                    }
                }
            }
            return Products;
        }

        public async Task<int> Count(ProductFilter filter)
        {
            IQueryable<ProductDAO> Products = DataContext.Product;
            Products = DynamicFilter(Products, filter);
            return await Products.CountAsync();
        }

        public async Task<List<Product>> List(ProductFilter filter)
        {
            if (filter == null) return new List<Product>();
            IQueryable<ProductDAO> ProductDAOs = DataContext.Product.AsNoTracking();
            ProductDAOs = DynamicFilter(ProductDAOs, filter);
            ProductDAOs = DynamicOrder(ProductDAOs, filter);
            List<Product> Products = await DynamicSelect(ProductDAOs, filter);
            return Products;
        }

        public async Task<Product> Get(long Id)
        {
            Product Product = await DataContext.Product.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Product()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ERPCode = x.ERPCode,
                    TechnicalName = x.TechnicalName,
                    OtherName = x.OtherName,
                    Description = x.Description,
                    ScanCode = x.ScanCode,
                    CategoryId = x.CategoryId,
                    ProductTypeId = x.ProductTypeId,
                    BrandId = x.BrandId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                    SalePrice = x.SalePrice,
                    RetailPrice = x.RetailPrice,
                    TaxTypeId = x.TaxTypeId,
                    StatusId = x.StatusId,
                    IsNew = x.IsNew,
                    UsedVariationId = x.UsedVariationId,
                    Used = x.Used,
                    RowId = x.RowId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.CreatedAt,
                    DeletedAt = x.DeletedAt,
                    Note = x.Note,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                        Description = x.Brand.Description,
                        StatusId = x.Brand.StatusId,
                    },
                    Category = x.Category == null ? null : new Category
                    {
                        Id = x.Category.Id,
                        Code = x.Category.Code,
                        Name = x.Category.Name,
                        Path = x.Category.Path,
                        ParentId = x.Category.ParentId,
                        StatusId = x.Category.StatusId,
                        Level = x.Category.Level
                    },
                    ProductType = x.ProductType == null ? null : new ProductType
                    {
                        Id = x.ProductType.Id,
                        Code = x.ProductType.Code,
                        Name = x.ProductType.Name,
                        Description = x.ProductType.Description,
                        StatusId = x.ProductType.StatusId,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    TaxType = x.TaxType == null ? null : new TaxType
                    {
                        Id = x.TaxType.Id,
                        Code = x.TaxType.Code,
                        Name = x.TaxType.Name,
                        Percentage = x.TaxType.Percentage,
                        StatusId = x.TaxType.StatusId,
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                    UnitOfMeasureGrouping = x.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                    {
                        Id = x.UnitOfMeasureGrouping.Id,
                        Name = x.UnitOfMeasureGrouping.Name,
                        UnitOfMeasureId = x.UnitOfMeasureGrouping.UnitOfMeasureId,
                        StatusId = x.UnitOfMeasureGrouping.StatusId,

                    },
                    UsedVariation = x.UsedVariation == null ? null : new UsedVariation
                    {
                        Id = x.UsedVariation.Id,
                        Code = x.UsedVariation.Code,
                        Name = x.UsedVariation.Name,
                    }
                }).FirstOrDefaultAsync();

            if (Product == null)
                return null;
            if (Product.UnitOfMeasureGrouping != null)
            {
                Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents = await DataContext.UnitOfMeasureGroupingContent
                    .Where(uomgc => uomgc.UnitOfMeasureGroupingId == Product.UnitOfMeasureGroupingId.Value)
                    .Select(uomgc => new UnitOfMeasureGroupingContent
                    {
                        Id = uomgc.Id,
                        Factor = uomgc.Factor,
                        UnitOfMeasureId = uomgc.UnitOfMeasureId,
                        UnitOfMeasure = new UnitOfMeasure
                        {
                            Id = uomgc.UnitOfMeasure.Id,
                            Code = uomgc.UnitOfMeasure.Code,
                            Name = uomgc.UnitOfMeasure.Name,
                            Description = uomgc.UnitOfMeasure.Description,
                            StatusId = uomgc.UnitOfMeasure.StatusId,
                        }
                    }).ToListAsync();
            }

            Product.Items = await DataContext.Item.AsNoTracking()
                .Where(x => x.ProductId == Product.Id)
                .Where(x => x.DeletedAt == null)
                .Select(x => new Item
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Code = x.Code,
                    Name = x.Name,
                    ScanCode = x.ScanCode,
                    SalePrice = x.SalePrice,
                    RetailPrice = x.RetailPrice,
                    StatusId = x.StatusId,
                    Used = x.Used,
                    ItemImageMappings = new List<ItemImageMapping>()
                }).ToListAsync();
            Product.ProductImageMappings = await DataContext.ProductImageMapping.AsNoTracking()
                .Where(x => x.ProductId == Product.Id)
                .Select(x => new ProductImageMapping
                {
                    ProductId = x.ProductId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                }).ToListAsync();
            Product.ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping.AsNoTracking()
                .Where(x => x.ProductId == Product.Id)
                .Where(x => x.ProductGrouping.DeletedAt == null)
                .Select(x => new ProductProductGroupingMapping
                {
                    ProductId = x.ProductId,
                    ProductGroupingId = x.ProductGroupingId,
                    ProductGrouping = new ProductGrouping
                    {
                        Id = x.ProductGrouping.Id,
                        Code = x.ProductGrouping.Code,
                        Name = x.ProductGrouping.Name,
                        ParentId = x.ProductGrouping.ParentId,
                        Path = x.ProductGrouping.Path,
                        Description = x.ProductGrouping.Description,
                    },
                }).ToListAsync();
            Product.VariationGroupings = await DataContext.VariationGrouping.Include(x => x.Variations)
                .Where(x => x.ProductId == Product.Id)
                .Where(x => x.DeletedAt == null)
                .Select(x => new VariationGrouping
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductId = x.ProductId,
                    Variations = x.Variations.Select(v => new Variation
                    {
                        Id = v.Id,
                        Code = v.Code,
                        Name = v.Name,
                        VariationGroupingId = v.VariationGroupingId,
                    }).ToList(),
                }).ToListAsync();

            var ItemIds = Product.Items.Select(x => x.Id).ToList();
            List<ItemImageMapping> ItemImageMappings = await DataContext.ItemImageMapping.Where(x => ItemIds.Contains(x.ItemId)).Select(x => new ItemImageMapping
            {
                ImageId = x.ImageId,
                ItemId = x.ItemId,
                Image = new Image
                {
                    Id = x.Image.Id,
                    Url = x.Image.Url,
                    Name = x.Image.Name,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                }
            }).ToListAsync();

            foreach (var item in Product.Items)
            {
                item.ItemImageMappings = ItemImageMappings.Where(x => x.ItemId == item.Id).ToList();
            }

            List<ItemHistory> ItemHistories = await DataContext.ItemHistory.Where(x => ItemIds.Contains(x.ItemId)).Select(x => new ItemHistory
            {
                Id = x.Id,
                ItemId = x.ItemId,
                ModifierId = x.ModifierId,
                NewPrice = x.NewPrice,
                OldPrice = x.OldPrice,
                Time = x.Time,
            }).ToListAsync();

            foreach (var item in Product.Items)
            {
                item.ItemHistories = ItemHistories.Where(x => x.ItemId == item.Id).ToList();
            }
            return Product;
        }

        public async Task<bool> BulkInsertNewProduct(List<Product> Products)
        {
            var ProductIds = Products.Select(x => x.Id).ToList();
            await DataContext.Product.Where(x => ProductIds.Contains(x.Id)).UpdateFromQueryAsync(x => new ProductDAO
            {
                IsNew = true,
                UpdatedAt = StaticParams.DateTimeNow
            });

            return true;
        }

        public async Task<bool> BulkDeleteNewProduct(List<Product> Products)
        {
            var ProductIds = Products.Select(x => x.Id).ToList();
            await DataContext.Product.Where(x => ProductIds.Contains(x.Id)).UpdateFromQueryAsync(x => new ProductDAO
            {
                IsNew = false
            });

            return true;
        }
    }
}
