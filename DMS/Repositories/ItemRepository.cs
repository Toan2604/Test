using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IItemRepository
    {
        Task<int> Count(ItemFilter ItemFilter);
        Task<int> CountAll(ItemFilter ItemFilter);
        Task<List<Item>> List(ItemFilter ItemFilter);
        Task<Item> Get(long Id);
    }
    public class ItemRepository : IItemRepository
    {
        private DataContext DataContext;
        public ItemRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ItemDAO> DynamicFilter(IQueryable<ItemDAO> query, ItemFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.DeletedAt == null && q.Product.DeletedAt == null);
            if (filter.Search != null)
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
                    queryForOtherName = queryForOtherName.Where(x => x.Product.OtherName.ToLower().Contains(Token));
                }
                query = queryForCode.Union(queryForName).Union(queryForOtherName);
                query = query.Distinct();
            }
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ProductId, filter.ProductId);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.ERPCode, filter.ERPCode);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Product.OtherName, filter.OtherName);
            query = query.Where(q => q.ScanCode, filter.ScanCode);
            query = query.Where(q => q.RetailPrice, filter.RetailPrice);
            query = query.Where(q => q.Product.ProductTypeId, filter.ProductTypeId);
            query = query.Where(q => q.Product.BrandId, filter.BrandId);
            query = query.Where(q => q.Product.IsNew, filter.IsNew);
            if (filter.ProductGroupingId != null && filter.ProductGroupingId.HasValue)
            {
                if (filter.ProductGroupingId.Equal != null)
                {
                    ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                        .Where(o => o.Id == filter.ProductGroupingId.Equal.Value).FirstOrDefaultWithNoLock();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where pg.Path.StartsWith(ProductGroupingDAO.Path) && pg.Id == ProductGroupingDAO.Id
                            select q;
                }
                if (filter.ProductGroupingId.NotEqual != null)
                {
                    ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                        .Where(o => o.Id == filter.ProductGroupingId.NotEqual.Value).FirstOrDefaultWithNoLock();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where !pg.Path.StartsWith(ProductGroupingDAO.Path)
                            select q;
                }
                if (filter.ProductGroupingId.In != null)
                {
                    List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                        .Where(o => o.DeletedAt == null).ToListWithNoLock();
                    List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => filter.ProductGroupingId.In.Contains(o.Id)).ToList();
                    List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> ProductGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where ProductGroupingIds.Contains(pg.Id)
                            select q;
                }
                if (filter.ProductGroupingId.NotIn != null)
                {
                    List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                        .Where(o => o.DeletedAt == null).ToListWithNoLock();
                    List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => filter.ProductGroupingId.NotIn.Contains(o.Id)).ToList();
                    List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> ProductGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                            join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                            where !ProductGroupingIds.Contains(pg.Id)
                            select q;
                }
            }

            if (filter.CategoryId != null && filter.CategoryId.HasValue)
            {
                if (filter.CategoryId.Equal != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.Equal.Value).FirstOrDefault();
                    query = from q in query
                            join pr in DataContext.Product on q.ProductId equals pr.Id
                            join cat in DataContext.Category on pr.CategoryId equals cat.Id
                            where cat.Path.Equals(CategoryDAO.Path)
                            select q;
                }
                if (filter.CategoryId.NotEqual != null)
                {
                    CategoryDAO CategoryDAO = DataContext.Category
                        .Where(o => o.Id == filter.CategoryId.NotEqual.Value).FirstOrDefault();
                    query = from q in query
                            join pr in DataContext.Product on q.ProductId equals pr.Id
                            join cat in DataContext.Category on pr.CategoryId equals cat.Id
                            where !cat.Path.Equals(CategoryDAO.Path)
                            select q;
                }
                if (filter.CategoryId.In != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.In.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.Product.CategoryId, IdFilter);
                }
                if (filter.CategoryId.NotIn != null)
                {
                    List<CategoryDAO> CategoryDAOs = DataContext.Category
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<CategoryDAO> Parents = CategoryDAOs.Where(o => filter.CategoryId.NotIn.Contains(o.Id)).ToList();
                    List<CategoryDAO> Branches = CategoryDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.Product.CategoryId, IdFilter);
                }

            }


            if (filter.StatusId != null && filter.StatusId.HasValue)
            {
                var UsedVariationItems = query.Where(x => x.Product.UsedVariationId == UsedVariationEnum.USED.Id);
                UsedVariationItems = UsedVariationItems.Where(q => q.StatusId, filter.StatusId);

                var NotUsedVariationItems = query.Where(x => x.Product.UsedVariationId == UsedVariationEnum.NOTUSED.Id);
                NotUsedVariationItems = NotUsedVariationItems.Where(x => x.Product.StatusId, filter.StatusId);

                query = UsedVariationItems.Union(NotUsedVariationItems);
            }

            query = query.Distinct();
            return query;
        }

        private IQueryable<ItemDAO> OrFilter(IQueryable<ItemDAO> query, ItemFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ItemDAO> initQuery = query.Where(q => false);
            foreach (ItemFilter ItemFilter in filter.OrFilter)
            {
                IQueryable<ItemDAO> queryable = query;
                queryable = queryable.Where(q => q.SalePrice, ItemFilter.SalePrice);
                queryable = queryable.Where(q => q.Product.ProductTypeId, ItemFilter.ProductTypeId);
                if (ItemFilter.ProductGroupingId != null && ItemFilter.ProductGroupingId.HasValue)
                {
                    if (ItemFilter.ProductGroupingId.Equal != null)
                    {
                        ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                            .Where(o => o.Id == ItemFilter.ProductGroupingId.Equal.Value).FirstOrDefaultWithNoLock();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where pg.Path.StartsWith(ProductGroupingDAO.Path)
                                    select q;
                    }
                    if (ItemFilter.ProductGroupingId.NotEqual != null)
                    {
                        ProductGroupingDAO ProductGroupingDAO = DataContext.ProductGrouping
                            .Where(o => o.Id == ItemFilter.ProductGroupingId.NotEqual.Value).FirstOrDefaultWithNoLock();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where !pg.Path.StartsWith(ProductGroupingDAO.Path)
                                    select q;
                    }
                    if (ItemFilter.ProductGroupingId.In != null)
                    {
                        List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                            .Where(o => o.DeletedAt == null).ToListWithNoLock();
                        List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => ItemFilter.ProductGroupingId.In.Contains(o.Id)).ToList();
                        List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> ProductGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where ProductGroupingIds.Contains(pg.Id)
                                    select q;
                    }
                    if (ItemFilter.ProductGroupingId.NotIn != null)
                    {
                        List<ProductGroupingDAO> ProductGroupingDAOs = DataContext.ProductGrouping
                            .Where(o => o.DeletedAt == null).ToListWithNoLock();
                        List<ProductGroupingDAO> Parents = ProductGroupingDAOs.Where(o => ItemFilter.ProductGroupingId.NotIn.Contains(o.Id)).ToList();
                        List<ProductGroupingDAO> Branches = ProductGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> ProductGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ppg in DataContext.ProductProductGroupingMapping on q.ProductId equals ppg.ProductId
                                    join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                    where !ProductGroupingIds.Contains(pg.Id)
                                    select q;
                    }
                }
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ItemDAO> DynamicOrder(IQueryable<ItemDAO> query, ItemFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ItemOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ItemOrder.Product:
                            query = query.OrderBy(q => q.ProductId);
                            break;
                        case ItemOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ItemOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ItemOrder.ScanCode:
                            query = query.OrderBy(q => q.ScanCode);
                            break;
                        case ItemOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ItemOrder.RetailPrice:
                            query = query.OrderBy(q => q.RetailPrice);
                            break;
                        case ItemOrder.Score:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ItemOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ItemOrder.Product:
                            query = query.OrderByDescending(q => q.ProductId);
                            break;
                        case ItemOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ItemOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ItemOrder.ScanCode:
                            query = query.OrderByDescending(q => q.ScanCode);
                            break;
                        case ItemOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ItemOrder.RetailPrice:
                            query = query.OrderByDescending(q => q.RetailPrice);
                            break;
                        case ItemOrder.Score:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Item>> DynamicSelect(IQueryable<ItemDAO> query, ItemFilter filter)
        {
            List<Item> Items = await query.Select(q => new Item()
            {
                Id = filter.Selects.Contains(ItemSelect.Id) ? q.Id : default(long),
                ProductId = filter.Selects.Contains(ItemSelect.ProductId) ? q.ProductId : default(long),
                Code = filter.Selects.Contains(ItemSelect.Code) ? q.Code : default(string),
                ERPCode = filter.Selects.Contains(ItemSelect.ERPCode) ? q.ERPCode : default(string),
                Name = filter.Selects.Contains(ItemSelect.Name) ? q.Name : default(string),
                ScanCode = filter.Selects.Contains(ItemSelect.ScanCode) ? q.ScanCode : default(string),
                SalePrice = filter.Selects.Contains(ItemSelect.SalePrice) ? q.SalePrice : default(decimal),
                RetailPrice = filter.Selects.Contains(ItemSelect.RetailPrice) ? q.RetailPrice : default(decimal?),
                StatusId = filter.Selects.Contains(ItemSelect.Status) ? q.StatusId : default(long),
                Product = filter.Selects.Contains(ItemSelect.Product) && q.Product != null ? new Product
                {
                    Id = q.Product.Id,
                    Code = q.Product.Code,
                    Name = q.Product.Name,
                    Description = q.Product.Description,
                    ScanCode = q.Product.ScanCode,
                    ProductTypeId = q.Product.ProductTypeId,
                    BrandId = q.Product.BrandId,
                    UnitOfMeasureId = q.Product.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = q.Product.UnitOfMeasureGroupingId,
                    SalePrice = q.Product.SalePrice,
                    RetailPrice = q.Product.RetailPrice,
                    TaxTypeId = q.Product.TaxTypeId,
                    StatusId = q.Product.StatusId,
                    IsNew = q.Product.IsNew,
                    CategoryId = q.Product.CategoryId,
                    Category = new Category
                    {
                        Id = q.Product.Category.Id,
                        Code = q.Product.Category.Code,
                        Name = q.Product.Category.Name,
                        Path = q.Product.Category.Path,
                        ParentId = q.Product.Category.ParentId,
                        StatusId = q.Product.Category.StatusId,
                        Level = q.Product.Category.Level
                    },
                    ProductType = new ProductType
                    {
                        Id = q.Product.ProductType.Id,
                        Code = q.Product.ProductType.Code,
                        Name = q.Product.ProductType.Name,
                        Description = q.Product.ProductType.Description,
                        StatusId = q.Product.ProductType.StatusId,
                        UpdatedAt = q.Product.ProductType.UpdatedAt,
                    },
                    TaxType = new TaxType
                    {
                        Id = q.Product.TaxType.Id,
                        Code = q.Product.TaxType.Code,
                        Name = q.Product.TaxType.Name,
                        Percentage = q.Product.TaxType.Percentage,
                        StatusId = q.Product.TaxType.StatusId,
                    },
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = q.Product.UnitOfMeasure.Id,
                        Code = q.Product.UnitOfMeasure.Code,
                        Name = q.Product.UnitOfMeasure.Name,
                    },
                    Brand = q.Product.BrandId == null ? null : new Brand
                    {
                        Id = q.Product.Brand.Id,
                        Code = q.Product.Brand.Code,
                        Name = q.Product.Brand.Name,
                        StatusId = q.Product.Brand.StatusId,
                    },
                    UnitOfMeasureGrouping = new UnitOfMeasureGrouping
                    {
                        Id = q.Product.UnitOfMeasureGrouping.Id,
                        Code = q.Product.UnitOfMeasureGrouping.Code,
                        Name = q.Product.UnitOfMeasureGrouping.Name
                    },
                } : null,
                Status = filter.Selects.Contains(ItemSelect.Status) && q.Status == null ? null : new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                },
                Used = q.Used,
            }).ToListWithNoLockAsync();
            var Ids = Items.Select(x => x.Id).ToList();
            var ProductIds = Items.Select(x => x.ProductId).ToList();
            IdFilter ItemIdFilter = new IdFilter { In = Ids };
            IdFilter ProductIdFilter = new IdFilter { In = ProductIds };
            var ProductImageMappings =await DataContext.ProductImageMapping
                .Include(x => x.Image)
                .Where(x => ProductIdFilter.In.Contains(x.ProductId))
                .ToListWithNoLockAsync();
            var ItemImageMappings = await DataContext.ItemImageMapping.Include(x => x.Image)
                .Where(x => x.ItemId, ItemIdFilter)
                .ToListWithNoLockAsync();
            var ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping
                .Include(x => x.ProductGrouping)
                .Where(x => x.ProductId, ProductIdFilter).ToListWithNoLockAsync();

            foreach (var Item in Items)
            {
                Item.ItemImageMappings = new List<ItemImageMapping>();
                var ItemImageMappingDAOs = ItemImageMappings.Where(x => x.ItemId == Item.Id).ToList();
                if (ItemImageMappingDAOs != null && ItemImageMappingDAOs.Any()) 
                {
                    List<ItemImageMapping> itemImageMappings = ItemImageMappingDAOs.Select(x => new ItemImageMapping
                    {
                        ImageId = x.ImageId,
                        ItemId = x.ItemId,
                        Image = x.Image == null ? null : new Image
                        {
                            Id = x.Image.Id,
                            Name = x.Image.Name,
                            Url = x.Image.Url,
                            ThumbnailUrl = x.Image.ThumbnailUrl
                        }
                    }).ToList();
                    Item.ItemImageMappings.AddRange(itemImageMappings);
                }
                if (Item.ItemImageMappings.Count == 0)
                {
                    if (ProductImageMappings != null && ProductImageMappings.Any())
                    {
                        List<ItemImageMapping> itemImageMappings = ProductImageMappings.Where(x => x.ProductId == Item.ProductId).Select(x => new ItemImageMapping
                        {
                            ImageId = x.ImageId,
                            ItemId = Item.Id,
                            Image = x.Image == null ? null : new Image
                            {
                                Id = x.Image.Id,
                                Name = x.Image.Name,
                                Url = x.Image.Url,
                                ThumbnailUrl = x.Image.ThumbnailUrl
                            }
                        }).ToList();
                        Item.ItemImageMappings.AddRange(itemImageMappings);
                    }
                }
                if (Item.Product != null)
                {
                    Item.Product.ProductProductGroupingMappings = new List<ProductProductGroupingMapping>();
                    Item.Product.ProductProductGroupingMappings = ProductProductGroupingMappings.Where(x => x.ProductId == Item.ProductId).Select(p => new ProductProductGroupingMapping
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
                    }).ToList();
                }
            }
            return Items;
        }

        public async Task<int> Count(ItemFilter filter)
        {
            IQueryable<ItemDAO> Items = DataContext.Item;
            Items = DynamicFilter(Items, filter);
            Items = OrFilter(Items, filter);
            return await Items.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ItemFilter filter)
        {
            IQueryable<ItemDAO> Items = DataContext.Item;
            Items = DynamicFilter(Items, filter);
            return await Items.CountWithNoLockAsync();
        }

        public async Task<List<Item>> List(ItemFilter filter)
        {
            if (filter == null) return new List<Item>();
            IQueryable<ItemDAO> ItemDAOs = DataContext.Item;
            ItemDAOs = DynamicFilter(ItemDAOs, filter);
            ItemDAOs = OrFilter(ItemDAOs, filter);
            ItemDAOs = DynamicOrder(ItemDAOs, filter);
            List<Item> Items = await DynamicSelect(ItemDAOs, filter);
            return Items;
        }

        public async Task<Item> Get(long Id)
        {
            Item Item = await DataContext.Item.Where(x => x.Id == Id).AsNoTracking().Select(x => new Item()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Code = x.Code,
                Name = x.Name,
                ScanCode = x.ScanCode,
                SalePrice = x.SalePrice,
                RetailPrice = x.RetailPrice,
                Used = x.Used,
                Product = x.Product == null ? null : new Product
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
                    ProductType = x.Product.ProductType == null ? null : new ProductType
                    {
                        Id = x.Product.ProductType.Id,
                        Code = x.Product.ProductType.Code,
                        Name = x.Product.ProductType.Name,
                        Description = x.Product.ProductType.Description,
                        StatusId = x.Product.ProductType.StatusId,
                        UpdatedAt = x.Product.ProductType.UpdatedAt,
                    },
                    Category = x.Product.Category == null ? null : new Category
                    {
                        Id = x.Product.Category.Id,
                        Code = x.Product.Category.Code,
                        Name = x.Product.Category.Name,
                        Path = x.Product.Category.Path,
                        ParentId = x.Product.Category.ParentId,
                        StatusId = x.Product.Category.StatusId,
                        Level = x.Product.Category.Level
                    },
                    TaxType = x.Product.TaxType == null ? null : new TaxType
                    {
                        Id = x.Product.TaxType.Id,
                        Code = x.Product.TaxType.Code,
                        StatusId = x.Product.TaxType.StatusId,
                        Name = x.Product.TaxType.Name,
                        Percentage = x.Product.TaxType.Percentage,
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
                        Code = x.Product.UnitOfMeasureGrouping.Code,
                        Name = x.Product.UnitOfMeasureGrouping.Name,
                        Description = x.Product.UnitOfMeasureGrouping.Description,
                        StatusId = x.Product.UnitOfMeasureGrouping.StatusId,
                        UnitOfMeasureId = x.Product.UnitOfMeasureGrouping.UnitOfMeasureId,
                    },
                    Brand = x.Product.Brand == null ? null : new Brand
                    {
                        Id = x.Product.Brand.Id,
                        Code = x.Product.Brand.Code,
                        Name = x.Product.Brand.Name,
                    },
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Item == null)
                return null;
            Item.Product.ProductProductGroupingMappings = await DataContext.ProductProductGroupingMapping.Where(x => x.ProductId == Item.ProductId).Select(x => new ProductProductGroupingMapping
            {
                ProductId = x.ProductId,
                ProductGroupingId = x.ProductGroupingId,
                ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                {
                    Id = x.ProductGrouping.Id,
                    Code = x.ProductGrouping.Code,
                    Name = x.ProductGrouping.Name,
                }
            }).ToListWithNoLockAsync();

            if (Item.Product.UnitOfMeasureGroupingId.HasValue)
            {
                List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await DataContext.UnitOfMeasureGroupingContent
                    .Where(x => x.UnitOfMeasureGroupingId == Item.Product.UnitOfMeasureGroupingId.Value)
                    .Select(x => new UnitOfMeasureGroupingContent
                    {
                        Id = x.Id,
                        Factor = x.Factor,
                        UnitOfMeasureId = x.UnitOfMeasureId,
                        UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                        {
                            Id = x.UnitOfMeasure.Id,
                            Code = x.UnitOfMeasure.Code,
                            Name = x.UnitOfMeasure.Name,
                        },
                    }).ToListWithNoLockAsync();
                Item.Product.UnitOfMeasureGrouping.UnitOfMeasureGroupingContents = UnitOfMeasureGroupingContents;
            }

            Item.ItemImageMappings = await DataContext.ItemImageMapping
                .Where(x => x.ItemId == Item.Id)
                .Select(x => new ItemImageMapping
                {
                    ItemId = x.ItemId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    },
                }).ToListWithNoLockAsync();
            if (Item.ItemImageMappings.Count == 0)
            {
                var ProductImageMappingDAOs = await DataContext.ProductImageMapping.Include(x => x.Image).Where(x => x.ProductId == Item.ProductId).ToListWithNoLockAsync();
                foreach (ProductImageMappingDAO ProductImageMappingDAO in ProductImageMappingDAOs)
                {
                    ItemImageMapping ItemImageMapping = new ItemImageMapping
                    {
                        ImageId = ProductImageMappingDAO.ImageId,
                        ItemId = Item.Id,
                        Image = ProductImageMappingDAO.Image == null ? null : new Image
                        {
                            Id = ProductImageMappingDAO.Image.Id,
                            Name = ProductImageMappingDAO.Image.Name,
                            Url = ProductImageMappingDAO.Image.Url,
                            ThumbnailUrl = ProductImageMappingDAO.Image.ThumbnailUrl,
                        }
                    };
                    Item.ItemImageMappings.Add(ItemImageMapping);
                }
            }
            return Item;
        }
    }
}
