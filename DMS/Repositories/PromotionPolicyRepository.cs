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
    public interface IPromotionPolicyRepository
    {
        Task<int> Count(PromotionPolicyFilter PromotionPolicyFilter);
        Task<int> CountAll(PromotionPolicyFilter PromotionPolicyFilter);
        Task<List<PromotionPolicy>> List(PromotionPolicyFilter PromotionPolicyFilter);
        Task<PromotionPolicy> Get(long Id);
        Task<PromotionPromotionPolicyMapping> GetMapping(long Id, long PromotionId);
    }
    public class PromotionPolicyRepository : IPromotionPolicyRepository
    {
        private DataContext DataContext;
        public PromotionPolicyRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionPolicyDAO> DynamicFilter(IQueryable<PromotionPolicyDAO> query, PromotionPolicyFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<PromotionPolicyDAO> OrFilter(IQueryable<PromotionPolicyDAO> query, PromotionPolicyFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionPolicyDAO> initQuery = query.Where(q => false);
            foreach (PromotionPolicyFilter PromotionPolicyFilter in filter.OrFilter)
            {
                IQueryable<PromotionPolicyDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionPolicyFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionPolicyFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionPolicyFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionPolicyDAO> DynamicOrder(IQueryable<PromotionPolicyDAO> query, PromotionPolicyFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionPolicyOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionPolicyOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionPolicyOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionPolicyOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionPolicyOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionPolicyOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionPolicy>> DynamicSelect(IQueryable<PromotionPolicyDAO> query, PromotionPolicyFilter filter)
        {
            List<PromotionPolicy> PromotionPolicies = await query.Select(q => new PromotionPolicy()
            {
                Id = filter.Selects.Contains(PromotionPolicySelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionPolicySelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionPolicySelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return PromotionPolicies;
        }

        public async Task<int> Count(PromotionPolicyFilter filter)
        {
            IQueryable<PromotionPolicyDAO> PromotionPolicies = DataContext.PromotionPolicy.AsNoTracking();
            PromotionPolicies = DynamicFilter(PromotionPolicies, filter);
            PromotionPolicies = OrFilter(PromotionPolicies, filter);
            return await PromotionPolicies.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionPolicyFilter filter)
        {
            IQueryable<PromotionPolicyDAO> PromotionPolicies = DataContext.PromotionPolicy.AsNoTracking();
            PromotionPolicies = DynamicFilter(PromotionPolicies, filter);
            return await PromotionPolicies.CountWithNoLockAsync();
        }

        public async Task<List<PromotionPolicy>> List(PromotionPolicyFilter filter)
        {
            if (filter == null) return new List<PromotionPolicy>();
            IQueryable<PromotionPolicyDAO> PromotionPolicyDAOs = DataContext.PromotionPolicy.AsNoTracking();
            PromotionPolicyDAOs = DynamicFilter(PromotionPolicyDAOs, filter);
            PromotionPolicyDAOs = OrFilter(PromotionPolicyDAOs, filter);
            PromotionPolicyDAOs = DynamicOrder(PromotionPolicyDAOs, filter);
            List<PromotionPolicy> PromotionPolicies = await DynamicSelect(PromotionPolicyDAOs, filter);
            return PromotionPolicies;
        }

        public async Task<PromotionPolicy> Get(long Id)
        {
            PromotionPolicy PromotionPolicy = await DataContext.PromotionPolicy.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionPolicy()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionPolicy == null)
                return null;

            return PromotionPolicy;
        }

        public async Task<PromotionPromotionPolicyMapping> GetMapping(long Id, long PromotionId)
        {
            PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = await DataContext.PromotionPromotionPolicyMapping.AsNoTracking()
            .Where(x => x.PromotionPolicyId == Id && x.PromotionId == PromotionId)
            .Select(x => new PromotionPromotionPolicyMapping
            {
                PromotionPolicyId = x.PromotionPolicyId,
                PromotionId = x.PromotionId,
                Note = x.Note,
                StatusId = x.StatusId,
                PromotionPolicy = x.PromotionPolicy == null ? null : new PromotionPolicy
                {
                    Id = x.PromotionPolicy.Id,
                    Code = x.PromotionPolicy.Code,
                    Name = x.PromotionPolicy.Name,
                }
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionPromotionPolicyMapping == null)
                return null;
            if (PromotionPromotionPolicyMapping.PromotionPolicy != null)
            {
                if (Id == PromotionPolicyEnum.SALES_ORDER.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders = await DataContext.PromotionDirectSalesOrder.AsNoTracking()
                        .Where(x => x.PromotionId == PromotionId)
                        .Select(x => new PromotionDirectSalesOrder
                        {
                            Id = x.Id,
                            PromotionPolicyId = x.PromotionPolicyId,
                            PromotionId = x.PromotionId,
                            Note = x.Note,
                            FromValue = x.FromValue,
                            ToValue = x.ToValue,
                            PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                            DiscountPercentage = x.DiscountPercentage,
                            DiscountValue = x.DiscountValue,
                            Price = x.Price,
                            PromotionDiscountType = new PromotionDiscountType
                            {
                                Id = x.PromotionDiscountType.Id,
                                Code = x.PromotionDiscountType.Code,
                                Name = x.PromotionDiscountType.Name,
                            },
                            PromotionPolicy = new PromotionPolicy
                            {
                                Id = x.PromotionPolicy.Id,
                                Code = x.PromotionPolicy.Code,
                                Name = x.PromotionPolicy.Name,
                            },
                        }).ToListWithNoLockAsync();

                    var PromotionDirectSalesOrderIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders.Select(x => x.Id).ToList();
                    IdFilter PromotionDirectSalesOrderIdFilter = new IdFilter { In = PromotionDirectSalesOrderIds };
                    var PromotionDirectSalesOrderItemMappings = await DataContext.PromotionDirectSalesOrderItemMapping
                        .Where(x => x.PromotionDirectSalesOrderId, PromotionDirectSalesOrderIdFilter)
                        .ToListWithNoLockAsync();

                    foreach (var PromotionDirectSalesOrder in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders)
                    {
                        PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings = PromotionDirectSalesOrderItemMappings
                            .Where(x => x.PromotionDirectSalesOrderId == PromotionDirectSalesOrder.Id)
                            .Select(x => new PromotionDirectSalesOrderItemMapping
                            {
                                ItemId = x.ItemId,
                                PromotionDirectSalesOrderId = x.PromotionDirectSalesOrderId,
                                Quantity = x.Quantity,
                                Item = x.Item == null ? null : new Item
                                {
                                    Id = x.Item.Id,
                                    Code = x.Item.Code,
                                    Name = x.Item.Name,
                                }
                            }).ToList();
                    }
                }
                else if (Id == PromotionPolicyEnum.STORE.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionStores = await DataContext.PromotionStore.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionStore
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        Note = x.Note,
                        FromValue = x.FromValue,
                        ToValue = x.ToValue,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.STORE_GROUPING.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionStoreGroupings = await DataContext.PromotionStoreGrouping.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionStoreGrouping
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        Note = x.Note,
                        FromValue = x.FromValue,
                        ToValue = x.ToValue,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.STORE_TYPE.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionStoreTypes = await DataContext.PromotionStoreType.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionStoreType
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        Note = x.Note,
                        FromValue = x.FromValue,
                        ToValue = x.ToValue,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.PRODUCT.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts = await DataContext.PromotionProduct.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionProduct
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
                        Price = x.Price,
                        Product = new Product
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
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.PRODUCT_GROUPING.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings = await DataContext.PromotionProductGrouping.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionProductGrouping
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        ProductGroupingId = x.ProductGroupingId,
                        Note = x.Note,
                        FromValue = x.FromValue,
                        ToValue = x.ToValue,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        ProductGrouping = new ProductGrouping
                        {
                            Id = x.ProductGrouping.Id,
                            Code = x.ProductGrouping.Code,
                            Name = x.ProductGrouping.Name,
                            Description = x.ProductGrouping.Description,
                            ParentId = x.ProductGrouping.ParentId,
                            Path = x.ProductGrouping.Path,
                            Level = x.ProductGrouping.Level,
                        },
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.PRODUCT_TYPE.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes = await DataContext.PromotionProductType.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionProductType
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        ProductTypeId = x.ProductTypeId,
                        Note = x.Note,
                        FromValue = x.FromValue,
                        ToValue = x.ToValue,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        ProductType = new ProductType
                        {
                            Id = x.ProductType.Id,
                            Code = x.ProductType.Code,
                            Name = x.ProductType.Name,
                            Description = x.ProductType.Description,
                            StatusId = x.ProductType.StatusId,
                            Used = x.ProductType.Used,
                        },
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.COMBO.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos = await DataContext.PromotionCombo.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionCombo
                    {
                        Id = x.Id,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        Note = x.Note,
                        Name = x.Name,
                        PromotionDiscountTypeId = x.PromotionDiscountTypeId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountValue = x.DiscountValue,
                        Price = x.Price,
                        PromotionDiscountType = new PromotionDiscountType
                        {
                            Id = x.PromotionDiscountType.Id,
                            Code = x.PromotionDiscountType.Code,
                            Name = x.PromotionDiscountType.Name,
                        },
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },

                    }).ToListWithNoLockAsync();
                }
                else if (Id == PromotionPolicyEnum.SAME_PRICE.Id)
                {
                    PromotionPromotionPolicyMapping.PromotionPolicy.PromotionSamePrices = await DataContext.PromotionSamePrice.AsNoTracking()
                    .Where(x => x.PromotionId == PromotionId)
                    .Select(x => new PromotionSamePrice
                    {
                        Id = x.Id,
                        Note = x.Note,
                        PromotionPolicyId = x.PromotionPolicyId,
                        PromotionId = x.PromotionId,
                        Price = x.Price,
                        PromotionPolicy = new PromotionPolicy
                        {
                            Id = x.PromotionPolicy.Id,
                            Code = x.PromotionPolicy.Code,
                            Name = x.PromotionPolicy.Name,
                        },
                    }).ToListWithNoLockAsync();
                }
            }
            return PromotionPromotionPolicyMapping;
        }
    }
}
