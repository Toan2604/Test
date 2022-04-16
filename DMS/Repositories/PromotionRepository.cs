using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IPromotionRepository
    {
        Task<int> Count(PromotionFilter PromotionFilter);
        Task<int> CountAll(PromotionFilter PromotionFilter);
        Task<List<Promotion>> List(PromotionFilter PromotionFilter);
        Task<Promotion> Get(long Id);
        Task<bool> Create(Promotion Promotion);
        Task<bool> Update(Promotion Promotion);
        Task<bool> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<bool> UpdateStore(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<bool> UpdateStoreGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<bool> UpdateStoreType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<bool> Delete(Promotion Promotion);
        Task<bool> BulkMerge(List<Promotion> Promotions);
        Task<bool> BulkDelete(List<Promotion> Promotions);
    }
    public class PromotionRepository : IPromotionRepository
    {
        private DataContext DataContext;
        public PromotionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionDAO> DynamicFilter(IQueryable<PromotionDAO> query, PromotionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.EndDate == null).Union(query.Where(q => q.EndDate, filter.EndDate));
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.PromotionTypeId, filter.PromotionTypeId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<PromotionDAO> OrFilter(IQueryable<PromotionDAO> query, PromotionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionDAO> initQuery = query.Where(q => false);
            foreach (PromotionFilter PromotionFilter in filter.OrFilter)
            {
                IQueryable<PromotionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionFilter.Name);
                queryable = queryable.Where(q => q.StartDate, PromotionFilter.StartDate);
                queryable = queryable.Where(q => q.EndDate, PromotionFilter.EndDate);
                queryable = queryable.Where(q => q.OrganizationId, PromotionFilter.OrganizationId);
                queryable = queryable.Where(q => q.PromotionTypeId, PromotionFilter.PromotionTypeId);
                queryable = queryable.Where(q => q.Note, PromotionFilter.Note);
                queryable = queryable.Where(q => q.Priority, PromotionFilter.Priority);
                queryable = queryable.Where(q => q.StatusId, PromotionFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionDAO> DynamicOrder(IQueryable<PromotionDAO> query, PromotionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case PromotionOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case PromotionOrder.EndDate:
                            query = query.OrderBy(q => q.EndDate);
                            break;
                        case PromotionOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case PromotionOrder.PromotionType:
                            query = query.OrderBy(q => q.PromotionTypeId);
                            break;
                        case PromotionOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionOrder.Priority:
                            query = query.OrderBy(q => q.Priority);
                            break;
                        case PromotionOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case PromotionOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case PromotionOrder.EndDate:
                            query = query.OrderByDescending(q => q.EndDate);
                            break;
                        case PromotionOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case PromotionOrder.PromotionType:
                            query = query.OrderByDescending(q => q.PromotionTypeId);
                            break;
                        case PromotionOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority);
                            break;
                        case PromotionOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Promotion>> DynamicSelect(IQueryable<PromotionDAO> query, PromotionFilter filter)
        {
            List<Promotion> Promotions = await query.Select(q => new Promotion()
            {
                Id = filter.Selects.Contains(PromotionSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionSelect.Name) ? q.Name : default(string),
                StartDate = filter.Selects.Contains(PromotionSelect.StartDate) ? q.StartDate : default(DateTime),
                EndDate = filter.Selects.Contains(PromotionSelect.EndDate) ? q.EndDate : default(DateTime?),
                OrganizationId = filter.Selects.Contains(PromotionSelect.Organization) ? q.OrganizationId : default(long),
                PromotionTypeId = filter.Selects.Contains(PromotionSelect.PromotionType) ? q.PromotionTypeId : default(long),
                Note = filter.Selects.Contains(PromotionSelect.Note) ? q.Note : default(string),
                Priority = filter.Selects.Contains(PromotionSelect.Priority) ? q.Priority : default(long),
                StatusId = filter.Selects.Contains(PromotionSelect.Status) ? q.StatusId : default(long),
                Organization = filter.Selects.Contains(PromotionSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                } : null,
                PromotionType = filter.Selects.Contains(PromotionSelect.PromotionType) && q.PromotionType != null ? new PromotionType
                {
                    Id = q.PromotionType.Id,
                    Code = q.PromotionType.Code,
                    Name = q.PromotionType.Name,
                } : null,
                Status = filter.Selects.Contains(PromotionSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return Promotions;
        }

        public async Task<int> Count(PromotionFilter filter)
        {
            IQueryable<PromotionDAO> Promotions = DataContext.Promotion.AsNoTracking();
            Promotions = OrFilter(Promotions, filter);
            return await Promotions.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionFilter filter)
        {
            IQueryable<PromotionDAO> Promotions = DataContext.Promotion.AsNoTracking();
            Promotions = DynamicFilter(Promotions, filter);
            return await Promotions.CountWithNoLockAsync();
        }

        public async Task<List<Promotion>> List(PromotionFilter filter)
        {
            if (filter == null) return new List<Promotion>();
            IQueryable<PromotionDAO> PromotionDAOs = DataContext.Promotion.AsNoTracking();
            PromotionDAOs = DynamicFilter(PromotionDAOs, filter);
            PromotionDAOs = OrFilter(PromotionDAOs, filter);
            PromotionDAOs = DynamicOrder(PromotionDAOs, filter);
            List<Promotion> Promotions = await DynamicSelect(PromotionDAOs, filter);
            return Promotions;
        }

        public async Task<Promotion> Get(long Id)
        {
            Promotion Promotion = await DataContext.Promotion.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Promotion()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                OrganizationId = x.OrganizationId,
                PromotionTypeId = x.PromotionTypeId,
                Note = x.Note,
                Priority = x.Priority,
                StatusId = x.StatusId,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                },
                PromotionType = x.PromotionType == null ? null : new PromotionType
                {
                    Id = x.PromotionType.Id,
                    Code = x.PromotionType.Code,
                    Name = x.PromotionType.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Promotion == null)
                return null;
            Promotion.PromotionCombos = await DataContext.PromotionCombo.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
                .Select(x => new PromotionCombo
                {
                    Id = x.Id,
                    Note = x.Note,
                    PromotionPolicyId = x.PromotionPolicyId,
                    PromotionId = x.PromotionId,
                    PromotionPolicy = new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                }).ToListWithNoLockAsync();
            Promotion.PromotionDirectSalesOrders = await DataContext.PromotionDirectSalesOrder.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionProductGroupings = await DataContext.PromotionProductGrouping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionProductTypes = await DataContext.PromotionProductType.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionProducts = await DataContext.PromotionProduct.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionPromotionPolicyMappings = await DataContext.PromotionPromotionPolicyMapping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
                .Select(x => new PromotionPromotionPolicyMapping
                {
                    PromotionId = x.PromotionId,
                    PromotionPolicyId = x.PromotionPolicyId,
                    Note = x.Note,
                    StatusId = x.StatusId,
                    PromotionPolicy = new PromotionPolicy
                    {
                        Id = x.PromotionPolicy.Id,
                        Code = x.PromotionPolicy.Code,
                        Name = x.PromotionPolicy.Name,
                    },
                    Status = new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).ToListWithNoLockAsync();
            Promotion.PromotionSamePrices = await DataContext.PromotionSamePrice.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionStoreGroupingMappings = await DataContext.PromotionStoreGroupingMapping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
                .Where(x => x.StoreGrouping.DeletedAt == null)
                .Select(x => new PromotionStoreGroupingMapping
                {
                    PromotionId = x.PromotionId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        StatusId = x.StoreGrouping.StatusId,
                    },
                }).ToListWithNoLockAsync();
            Promotion.PromotionStoreGroupings = await DataContext.PromotionStoreGrouping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionStoreMappings = await DataContext.PromotionStoreMapping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
                .Where(x => x.Store.DeletedAt == null)
                .Select(x => new PromotionStoreMapping
                {
                    PromotionId = x.PromotionId,
                    StoreId = x.StoreId,
                    Store = new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        Name = x.Store.Name,
                        UnsignName = x.Store.UnsignName,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        UnsignAddress = x.Store.UnsignAddress,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        StatusId = x.Store.StatusId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                    },
                }).ToListWithNoLockAsync();
            Promotion.PromotionStoreTypeMappings = await DataContext.PromotionStoreTypeMapping.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
                .Where(x => x.StoreType.DeletedAt == null)
                .Select(x => new PromotionStoreTypeMapping
                {
                    PromotionId = x.PromotionId,
                    StoreTypeId = x.StoreTypeId,
                    StoreType = new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Used = x.StoreType.Used,
                    },
                }).ToListWithNoLockAsync();
            Promotion.PromotionStoreTypes = await DataContext.PromotionStoreType.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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
            Promotion.PromotionStores = await DataContext.PromotionStore.AsNoTracking()
                .Where(x => x.PromotionId == Promotion.Id)
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

            return Promotion;
        }
        public async Task<bool> Create(Promotion Promotion)
        {
            PromotionDAO PromotionDAO = new PromotionDAO();
            PromotionDAO.Id = Promotion.Id;
            PromotionDAO.Code = Promotion.Code;
            PromotionDAO.Name = Promotion.Name;
            PromotionDAO.StartDate = Promotion.StartDate;
            PromotionDAO.EndDate = Promotion.EndDate;
            PromotionDAO.OrganizationId = Promotion.OrganizationId;
            PromotionDAO.PromotionTypeId = Promotion.PromotionTypeId;
            PromotionDAO.Note = Promotion.Note;
            PromotionDAO.Priority = Promotion.Priority;
            PromotionDAO.StatusId = Promotion.StatusId;
            PromotionDAO.CreatedAt = StaticParams.DateTimeNow;
            PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.Promotion.Add(PromotionDAO);
            await DataContext.SaveChangesAsync();
            Promotion.Id = PromotionDAO.Id;
            await SaveReference(Promotion);
            return true;
        }

        public async Task<bool> Update(Promotion Promotion)
        {
            PromotionDAO PromotionDAO = DataContext.Promotion.Where(x => x.Id == Promotion.Id).FirstOrDefault();
            if (PromotionDAO == null)
                return false;
            PromotionDAO.Id = Promotion.Id;
            PromotionDAO.Code = Promotion.Code;
            PromotionDAO.Name = Promotion.Name;
            PromotionDAO.StartDate = Promotion.StartDate;
            PromotionDAO.EndDate = Promotion.EndDate;
            PromotionDAO.OrganizationId = Promotion.OrganizationId;
            PromotionDAO.PromotionTypeId = Promotion.PromotionTypeId;
            PromotionDAO.Note = Promotion.Note;
            PromotionDAO.Priority = Promotion.Priority;
            PromotionDAO.StatusId = Promotion.StatusId;
            PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(Promotion);
            return true;
        }

        public async Task<bool> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            var PromotionDirectSalesOrderIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            await DataContext.PromotionDirectSalesOrderItemMapping
                .Where(x => PromotionDirectSalesOrderIds.Contains(x.PromotionDirectSalesOrderId)).DeleteFromQueryAsync();
            await DataContext.PromotionDirectSalesOrder
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrderDAOs = new List<PromotionDirectSalesOrderDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders != null)
            {
                foreach (PromotionDirectSalesOrder PromotionDirectSalesOrder in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders)
                {
                    PromotionDirectSalesOrderDAO PromotionDirectSalesOrderDAO = new PromotionDirectSalesOrderDAO();
                    PromotionDirectSalesOrderDAO.Id = PromotionDirectSalesOrder.Id;
                    PromotionDirectSalesOrderDAO.PromotionPolicyId = PromotionDirectSalesOrder.PromotionPolicyId;
                    PromotionDirectSalesOrderDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionDirectSalesOrderDAO.Note = PromotionDirectSalesOrder.Note;
                    PromotionDirectSalesOrderDAO.FromValue = PromotionDirectSalesOrder.FromValue;
                    PromotionDirectSalesOrderDAO.ToValue = PromotionDirectSalesOrder.ToValue;
                    PromotionDirectSalesOrderDAO.PromotionDiscountTypeId = PromotionDirectSalesOrder.PromotionDiscountTypeId;
                    PromotionDirectSalesOrderDAO.DiscountPercentage = PromotionDirectSalesOrder.DiscountPercentage;
                    PromotionDirectSalesOrderDAO.DiscountValue = PromotionDirectSalesOrder.DiscountValue;
                    PromotionDirectSalesOrderDAO.Price = PromotionDirectSalesOrder.Price;
                    PromotionDirectSalesOrderDAO.RowId = Guid.NewGuid();
                    PromotionDirectSalesOrderDAOs.Add(PromotionDirectSalesOrderDAO);
                    PromotionDirectSalesOrder.RowId = PromotionDirectSalesOrderDAO.RowId;
                }
                await DataContext.PromotionDirectSalesOrder.BulkMergeAsync(PromotionDirectSalesOrderDAOs);

                List<PromotionDirectSalesOrderItemMappingDAO> PromotionDirectSalesOrderItemMappingDAOs = new List<PromotionDirectSalesOrderItemMappingDAO>();
                foreach (PromotionDirectSalesOrder PromotionDirectSalesOrder in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionDirectSalesOrders)
                {
                    PromotionDirectSalesOrder.Id = PromotionDirectSalesOrderDAOs.Where(x => x.RowId == PromotionDirectSalesOrder.RowId).Select(x => x.Id).FirstOrDefault();
                    if (PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings != null)
                    {
                        foreach (var PromotionDirectSalesOrderItemMapping in PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings)
                        {
                            PromotionDirectSalesOrderItemMappingDAO PromotionDirectSalesOrderItemMappingDAO = new PromotionDirectSalesOrderItemMappingDAO();
                            PromotionDirectSalesOrderItemMappingDAO.ItemId = PromotionDirectSalesOrderItemMapping.ItemId;
                            PromotionDirectSalesOrderItemMappingDAO.PromotionDirectSalesOrderId = PromotionDirectSalesOrder.Id;
                            PromotionDirectSalesOrderItemMappingDAO.Quantity = PromotionDirectSalesOrderItemMapping.Quantity;
                            PromotionDirectSalesOrderItemMappingDAOs.Add(PromotionDirectSalesOrderItemMappingDAO);
                        }
                    }
                }
                await DataContext.PromotionDirectSalesOrderItemMapping.BulkMergeAsync(PromotionDirectSalesOrderItemMappingDAOs);
            }

            await DataContext.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> UpdateStore(Promotion Promotion)
        //{
        //    PromotionDAO PromotionDAO = DataContext.Promotion.Where(x => x.Id == Promotion.Id).FirstOrDefault();
        //    if (PromotionDAO == null)
        //        return false;
        //    PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;

        //    await DataContext.PromotionStore
        //        .Where(x => x.PromotionId == Promotion.Id)
        //        .DeleteFromQueryAsync();
        //    List<PromotionStoreDAO> PromotionStoreDAOs = new List<PromotionStoreDAO>();
        //    if (Promotion.PromotionStores != null)
        //    {
        //        foreach (PromotionStore PromotionStore in Promotion.PromotionStores)
        //        {
        //            PromotionStoreDAO PromotionStoreDAO = new PromotionStoreDAO();
        //            PromotionStoreDAO.Id = PromotionStore.Id;
        //            PromotionStoreDAO.PromotionPolicyId = PromotionStore.PromotionPolicyId;
        //            PromotionStoreDAO.PromotionId = Promotion.Id;
        //            PromotionStoreDAO.Note = PromotionStore.Note;
        //            PromotionStoreDAO.FromValue = PromotionStore.FromValue;
        //            PromotionStoreDAO.ToValue = PromotionStore.ToValue;
        //            PromotionStoreDAO.PromotionDiscountTypeId = PromotionStore.PromotionDiscountTypeId;
        //            PromotionStoreDAO.DiscountPercentage = PromotionStore.DiscountPercentage;
        //            PromotionStoreDAO.DiscountValue = PromotionStore.DiscountValue;
        //            PromotionStoreDAO.RowId = Guid.NewGuid();
        //            PromotionStoreDAOs.Add(PromotionStoreDAO);
        //            PromotionStore.RowId = PromotionStoreDAO.RowId;
        //        }
        //        await DataContext.PromotionStore.BulkMergeAsync(PromotionStoreDAOs);
        //    }
        //    await DataContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> UpdateStoreGrouping(Promotion Promotion)
        //{
        //    PromotionDAO PromotionDAO = DataContext.Promotion.Where(x => x.Id == Promotion.Id).FirstOrDefault();
        //    if (PromotionDAO == null)
        //        return false;
        //    PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;

        //    await DataContext.PromotionStoreGrouping
        //        .Where(x => x.PromotionId == Promotion.Id)
        //        .DeleteFromQueryAsync();
        //    List<PromotionStoreGroupingDAO> PromotionStoreGroupingDAOs = new List<PromotionStoreGroupingDAO>();
        //    if (Promotion.PromotionStoreGroupings != null)
        //    {
        //        foreach (PromotionStoreGrouping PromotionStoreGrouping in Promotion.PromotionStoreGroupings)
        //        {
        //            PromotionStoreGroupingDAO PromotionStoreGroupingDAO = new PromotionStoreGroupingDAO();
        //            PromotionStoreGroupingDAO.Id = PromotionStoreGrouping.Id;
        //            PromotionStoreGroupingDAO.PromotionPolicyId = PromotionStoreGrouping.PromotionPolicyId;
        //            PromotionStoreGroupingDAO.PromotionId = Promotion.Id;
        //            PromotionStoreGroupingDAO.Note = PromotionStoreGrouping.Note;
        //            PromotionStoreGroupingDAO.FromValue = PromotionStoreGrouping.FromValue;
        //            PromotionStoreGroupingDAO.ToValue = PromotionStoreGrouping.ToValue;
        //            PromotionStoreGroupingDAO.PromotionDiscountTypeId = PromotionStoreGrouping.PromotionDiscountTypeId;
        //            PromotionStoreGroupingDAO.DiscountPercentage = PromotionStoreGrouping.DiscountPercentage;
        //            PromotionStoreGroupingDAO.DiscountValue = PromotionStoreGrouping.DiscountValue;
        //            PromotionStoreGroupingDAO.RowId = Guid.NewGuid();
        //            PromotionStoreGroupingDAOs.Add(PromotionStoreGroupingDAO);
        //            PromotionStoreGrouping.RowId = PromotionStoreGroupingDAO.RowId;
        //        }
        //        await DataContext.PromotionStoreGrouping.BulkMergeAsync(PromotionStoreGroupingDAOs);
        //    }
        //    await DataContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> UpdateStoreType(Promotion Promotion)
        //{
        //    PromotionDAO PromotionDAO = DataContext.Promotion.Where(x => x.Id == Promotion.Id).FirstOrDefault();
        //    if (PromotionDAO == null)
        //        return false;
        //    PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;

        //    await DataContext.PromotionStoreType
        //        .Where(x => x.PromotionId == Promotion.Id)
        //        .DeleteFromQueryAsync();
        //    List<PromotionStoreTypeDAO> PromotionStoreTypeDAOs = new List<PromotionStoreTypeDAO>();
        //    if (Promotion.PromotionStoreTypes != null)
        //    {
        //        foreach (PromotionStoreType PromotionStoreType in Promotion.PromotionStoreTypes)
        //        {
        //            PromotionStoreTypeDAO PromotionStoreTypeDAO = new PromotionStoreTypeDAO();
        //            PromotionStoreTypeDAO.Id = PromotionStoreType.Id;
        //            PromotionStoreTypeDAO.PromotionPolicyId = PromotionStoreType.PromotionPolicyId;
        //            PromotionStoreTypeDAO.PromotionId = Promotion.Id;
        //            PromotionStoreTypeDAO.Note = PromotionStoreType.Note;
        //            PromotionStoreTypeDAO.FromValue = PromotionStoreType.FromValue;
        //            PromotionStoreTypeDAO.ToValue = PromotionStoreType.ToValue;
        //            PromotionStoreTypeDAO.PromotionDiscountTypeId = PromotionStoreType.PromotionDiscountTypeId;
        //            PromotionStoreTypeDAO.DiscountPercentage = PromotionStoreType.DiscountPercentage;
        //            PromotionStoreTypeDAO.DiscountValue = PromotionStoreType.DiscountValue;
        //            PromotionStoreTypeDAO.RowId = Guid.NewGuid();
        //            PromotionStoreTypeDAOs.Add(PromotionStoreTypeDAO);
        //            PromotionStoreType.RowId = PromotionStoreTypeDAO.RowId;
        //        }
        //        await DataContext.PromotionStoreType.BulkMergeAsync(PromotionStoreTypeDAOs);
        //    }
        //    await DataContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            var PromotionProductIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            await DataContext.PromotionProductItemMapping
                .Where(x => PromotionProductIds.Contains(x.PromotionProductId)).DeleteFromQueryAsync();
            await DataContext.PromotionProduct
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionProductDAO> PromotionProductDAOs = new List<PromotionProductDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts != null)
            {
                foreach (PromotionProduct PromotionProduct in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts)
                {
                    PromotionProductDAO PromotionProductDAO = new PromotionProductDAO();
                    PromotionProductDAO.Id = PromotionProduct.Id;
                    PromotionProductDAO.PromotionPolicyId = PromotionProduct.PromotionPolicyId;
                    PromotionProductDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionProductDAO.ProductId = PromotionProduct.ProductId;
                    PromotionProductDAO.Note = PromotionProduct.Note;
                    PromotionProductDAO.FromValue = PromotionProduct.FromValue;
                    PromotionProductDAO.ToValue = PromotionProduct.ToValue;
                    PromotionProductDAO.PromotionDiscountTypeId = PromotionProduct.PromotionDiscountTypeId;
                    PromotionProductDAO.DiscountPercentage = PromotionProduct.DiscountPercentage;
                    PromotionProductDAO.DiscountValue = PromotionProduct.DiscountValue;
                    PromotionProductDAO.Price = PromotionProduct.Price;
                    PromotionProductDAO.RowId = Guid.NewGuid();
                    PromotionProductDAOs.Add(PromotionProductDAO);
                    PromotionProduct.RowId = PromotionProductDAO.RowId;
                }
                await DataContext.PromotionProduct.BulkMergeAsync(PromotionProductDAOs);

                List<PromotionProductItemMappingDAO> PromotionProductItemMappingDAOs = new List<PromotionProductItemMappingDAO>();
                foreach (PromotionProduct PromotionProduct in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProducts)
                {
                    PromotionProduct.Id = PromotionProductDAOs.Where(x => x.RowId == PromotionProduct.RowId).Select(x => x.Id).FirstOrDefault();
                    if (PromotionProduct.PromotionProductItemMappings != null)
                    {
                        foreach (var PromotionProductItemMapping in PromotionProduct.PromotionProductItemMappings)
                        {
                            PromotionProductItemMappingDAO PromotionProductItemMappingDAO = new PromotionProductItemMappingDAO();
                            PromotionProductItemMappingDAO.ItemId = PromotionProductItemMapping.ItemId;
                            PromotionProductItemMappingDAO.PromotionProductId = PromotionProduct.Id;
                            PromotionProductItemMappingDAO.Quantity = PromotionProductItemMapping.Quantity;
                            PromotionProductItemMappingDAOs.Add(PromotionProductItemMappingDAO);
                        }
                    }
                }
                await DataContext.PromotionProductItemMapping.BulkMergeAsync(PromotionProductItemMappingDAOs);
            }

            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            var PromotionProductGroupingIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            await DataContext.PromotionProductGroupingItemMapping
                .Where(x => PromotionProductGroupingIds.Contains(x.PromotionProductGroupingId)).DeleteFromQueryAsync();
            await DataContext.PromotionProductGrouping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionProductGroupingDAO> PromotionProductGroupingDAOs = new List<PromotionProductGroupingDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings != null)
            {
                foreach (PromotionProductGrouping PromotionProductGrouping in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings)
                {
                    PromotionProductGroupingDAO PromotionProductGroupingDAO = new PromotionProductGroupingDAO();
                    PromotionProductGroupingDAO.Id = PromotionProductGrouping.Id;
                    PromotionProductGroupingDAO.PromotionPolicyId = PromotionProductGrouping.PromotionPolicyId;
                    PromotionProductGroupingDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionProductGroupingDAO.ProductGroupingId = PromotionProductGrouping.ProductGroupingId;
                    PromotionProductGroupingDAO.Note = PromotionProductGrouping.Note;
                    PromotionProductGroupingDAO.FromValue = PromotionProductGrouping.FromValue;
                    PromotionProductGroupingDAO.ToValue = PromotionProductGrouping.ToValue;
                    PromotionProductGroupingDAO.PromotionDiscountTypeId = PromotionProductGrouping.PromotionDiscountTypeId;
                    PromotionProductGroupingDAO.DiscountPercentage = PromotionProductGrouping.DiscountPercentage;
                    PromotionProductGroupingDAO.DiscountValue = PromotionProductGrouping.DiscountValue;
                    PromotionProductGroupingDAO.Price = PromotionProductGrouping.Price;
                    PromotionProductGroupingDAO.RowId = Guid.NewGuid();
                    PromotionProductGroupingDAOs.Add(PromotionProductGroupingDAO);
                    PromotionProductGrouping.RowId = PromotionProductGroupingDAO.RowId;
                }
                await DataContext.PromotionProductGrouping.BulkMergeAsync(PromotionProductGroupingDAOs);

                List<PromotionProductGroupingItemMappingDAO> PromotionProductGroupingItemMappingDAOs = new List<PromotionProductGroupingItemMappingDAO>();
                foreach (PromotionProductGrouping PromotionProductGrouping in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductGroupings)
                {
                    PromotionProductGrouping.Id = PromotionProductGroupingDAOs.Where(x => x.RowId == PromotionProductGrouping.RowId).Select(x => x.Id).FirstOrDefault();
                    if (PromotionProductGrouping.PromotionProductGroupingItemMappings != null)
                    {
                        foreach (var PromotionProductGroupingItemMapping in PromotionProductGrouping.PromotionProductGroupingItemMappings)
                        {
                            PromotionProductGroupingItemMappingDAO PromotionProductGroupingItemMappingDAO = new PromotionProductGroupingItemMappingDAO();
                            PromotionProductGroupingItemMappingDAO.ItemId = PromotionProductGroupingItemMapping.ItemId;
                            PromotionProductGroupingItemMappingDAO.PromotionProductGroupingId = PromotionProductGrouping.Id;
                            PromotionProductGroupingItemMappingDAO.Quantity = PromotionProductGroupingItemMapping.Quantity;
                            PromotionProductGroupingItemMappingDAOs.Add(PromotionProductGroupingItemMappingDAO);
                        }
                    }
                }
                await DataContext.PromotionProductGroupingItemMapping.BulkMergeAsync(PromotionProductGroupingItemMappingDAOs);
            }

            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            var PromotionProductTypeIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            await DataContext.PromotionProductTypeItemMapping
                .Where(x => PromotionProductTypeIds.Contains(x.PromotionProductTypeId)).DeleteFromQueryAsync();
            await DataContext.PromotionProductType
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionProductTypeDAO> PromotionProductTypeDAOs = new List<PromotionProductTypeDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes != null)
            {
                foreach (PromotionProductType PromotionProductType in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes)
                {
                    PromotionProductTypeDAO PromotionProductTypeDAO = new PromotionProductTypeDAO();
                    PromotionProductTypeDAO.Id = PromotionProductType.Id;
                    PromotionProductTypeDAO.PromotionPolicyId = PromotionProductType.PromotionPolicyId;
                    PromotionProductTypeDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionProductTypeDAO.ProductTypeId = PromotionProductType.ProductTypeId;
                    PromotionProductTypeDAO.Note = PromotionProductType.Note;
                    PromotionProductTypeDAO.FromValue = PromotionProductType.FromValue;
                    PromotionProductTypeDAO.ToValue = PromotionProductType.ToValue;
                    PromotionProductTypeDAO.PromotionDiscountTypeId = PromotionProductType.PromotionDiscountTypeId;
                    PromotionProductTypeDAO.DiscountPercentage = PromotionProductType.DiscountPercentage;
                    PromotionProductTypeDAO.DiscountValue = PromotionProductType.DiscountValue;
                    PromotionProductTypeDAO.Price = PromotionProductType.Price;
                    PromotionProductTypeDAO.RowId = Guid.NewGuid();
                    PromotionProductTypeDAOs.Add(PromotionProductTypeDAO);
                    PromotionProductType.RowId = PromotionProductTypeDAO.RowId;
                }
                await DataContext.PromotionProductType.BulkMergeAsync(PromotionProductTypeDAOs);

                List<PromotionProductTypeItemMappingDAO> PromotionProductTypeItemMappingDAOs = new List<PromotionProductTypeItemMappingDAO>();
                foreach (PromotionProductType PromotionProductType in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionProductTypes)
                {
                    PromotionProductType.Id = PromotionProductTypeDAOs.Where(x => x.RowId == PromotionProductType.RowId).Select(x => x.Id).FirstOrDefault();
                    if (PromotionProductType.PromotionProductTypeItemMappings != null)
                    {
                        foreach (var PromotionProductTypeItemMapping in PromotionProductType.PromotionProductTypeItemMappings)
                        {
                            PromotionProductTypeItemMappingDAO PromotionProductTypeItemMappingDAO = new PromotionProductTypeItemMappingDAO();
                            PromotionProductTypeItemMappingDAO.ItemId = PromotionProductTypeItemMapping.ItemId;
                            PromotionProductTypeItemMappingDAO.PromotionProductTypeId = PromotionProductType.Id;
                            PromotionProductTypeItemMappingDAO.Quantity = PromotionProductTypeItemMapping.Quantity;
                            PromotionProductTypeItemMappingDAOs.Add(PromotionProductTypeItemMappingDAO);
                        }
                    }
                }
                await DataContext.PromotionProductTypeItemMapping.BulkMergeAsync(PromotionProductTypeItemMappingDAOs);
            }

            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            var PromotionComboIds = PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            await DataContext.PromotionComboInItemMapping
                .Where(x => PromotionComboIds.Contains(x.PromotionComboId)).DeleteFromQueryAsync();
            await DataContext.PromotionComboOutItemMapping
                .Where(x => PromotionComboIds.Contains(x.PromotionComboId)).DeleteFromQueryAsync();
            await DataContext.PromotionCombo
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionComboDAO> PromotionComboDAOs = new List<PromotionComboDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos != null)
            {
                foreach (PromotionCombo PromotionCombo in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos)
                {
                    PromotionComboDAO PromotionComboDAO = new PromotionComboDAO();
                    PromotionComboDAO.Id = PromotionCombo.Id;
                    PromotionComboDAO.PromotionPolicyId = PromotionCombo.PromotionPolicyId;
                    PromotionComboDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionComboDAO.Name = PromotionCombo.Name;
                    PromotionComboDAO.Note = PromotionCombo.Note;
                    PromotionComboDAO.PromotionDiscountTypeId = PromotionCombo.PromotionDiscountTypeId;
                    PromotionComboDAO.DiscountPercentage = PromotionCombo.DiscountPercentage;
                    PromotionComboDAO.DiscountValue = PromotionCombo.DiscountValue;
                    PromotionComboDAO.Price = PromotionCombo.Price;
                    PromotionComboDAO.RowId = Guid.NewGuid();
                    PromotionComboDAOs.Add(PromotionComboDAO);
                    PromotionCombo.RowId = PromotionComboDAO.RowId;
                }
                await DataContext.PromotionCombo.BulkMergeAsync(PromotionComboDAOs);

                List<PromotionComboInItemMappingDAO> PromotionComboInItemMappingDAOs = new List<PromotionComboInItemMappingDAO>();
                List<PromotionComboOutItemMappingDAO> PromotionComboOutItemMappingDAOs = new List<PromotionComboOutItemMappingDAO>();
                foreach (PromotionCombo PromotionCombo in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionCombos)
                {
                    PromotionCombo.Id = PromotionComboDAOs.Where(x => x.RowId == PromotionCombo.RowId).Select(x => x.Id).FirstOrDefault();
                    if (PromotionCombo.PromotionComboInItemMappings != null)
                    {
                        foreach (var PromotionComboItemMapping in PromotionCombo.PromotionComboInItemMappings)
                        {
                            PromotionComboInItemMappingDAO PromotionComboInItemMappingDAO = new PromotionComboInItemMappingDAO();
                            PromotionComboInItemMappingDAO.ItemId = PromotionComboItemMapping.ItemId;
                            PromotionComboInItemMappingDAO.PromotionComboId = PromotionCombo.Id;
                            PromotionComboInItemMappingDAO.From = PromotionComboItemMapping.From;
                            PromotionComboInItemMappingDAO.To = PromotionComboItemMapping.To;
                            PromotionComboInItemMappingDAOs.Add(PromotionComboInItemMappingDAO);
                        }
                    }

                    if (PromotionCombo.PromotionComboOutItemMappings != null)
                    {
                        foreach (var PromotionComboItemMapping in PromotionCombo.PromotionComboOutItemMappings)
                        {
                            PromotionComboOutItemMappingDAO PromotionComboOutItemMappingDAO = new PromotionComboOutItemMappingDAO();
                            PromotionComboOutItemMappingDAO.ItemId = PromotionComboItemMapping.ItemId;
                            PromotionComboOutItemMappingDAO.PromotionComboId = PromotionCombo.Id;
                            PromotionComboOutItemMappingDAO.Quantity = PromotionComboItemMapping.Quantity;
                            PromotionComboOutItemMappingDAOs.Add(PromotionComboOutItemMappingDAO);
                        }
                    }
                }
                await DataContext.PromotionComboInItemMapping.BulkMergeAsync(PromotionComboInItemMappingDAOs);
                await DataContext.PromotionComboOutItemMapping.BulkMergeAsync(PromotionComboOutItemMappingDAOs);
            }

            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId &&
                x.PromotionPolicyId == PromotionPromotionPolicyMapping.PromotionPolicyId)
                .FirstOrDefault();
            if (PromotionPromotionPolicyMappingDAO == null)
                return false;
            PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;

            await DataContext.PromotionSamePrice
                .Where(x => x.PromotionId == PromotionPromotionPolicyMapping.PromotionId)
                .DeleteFromQueryAsync();
            List<PromotionSamePriceDAO> PromotionSamePriceDAOs = new List<PromotionSamePriceDAO>();
            if (PromotionPromotionPolicyMapping.PromotionPolicy.PromotionSamePrices != null)
            {
                foreach (PromotionSamePrice PromotionSamePrice in PromotionPromotionPolicyMapping.PromotionPolicy.PromotionSamePrices)
                {
                    PromotionSamePriceDAO PromotionSamePriceDAO = new PromotionSamePriceDAO();
                    PromotionSamePriceDAO.Id = PromotionSamePrice.Id;
                    PromotionSamePriceDAO.PromotionPolicyId = PromotionSamePrice.PromotionPolicyId;
                    PromotionSamePriceDAO.PromotionId = PromotionPromotionPolicyMapping.PromotionId;
                    PromotionSamePriceDAO.Note = PromotionSamePrice.Note;
                    PromotionSamePriceDAO.Price = PromotionSamePrice.Price;
                    PromotionSamePriceDAO.RowId = Guid.NewGuid();
                    PromotionSamePriceDAOs.Add(PromotionSamePriceDAO);
                    PromotionSamePrice.RowId = PromotionSamePriceDAO.RowId;
                }
                await DataContext.PromotionSamePrice.BulkMergeAsync(PromotionSamePriceDAOs);
            }
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Promotion Promotion)
        {
            await DataContext.Promotion.Where(x => x.Id == Promotion.Id).UpdateFromQueryAsync(x => new PromotionDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<Promotion> Promotions)
        {
            List<PromotionDAO> PromotionDAOs = new List<PromotionDAO>();
            foreach (Promotion Promotion in Promotions)
            {
                PromotionDAO PromotionDAO = new PromotionDAO();
                PromotionDAO.Id = Promotion.Id;
                PromotionDAO.Code = Promotion.Code;
                PromotionDAO.Name = Promotion.Name;
                PromotionDAO.StartDate = Promotion.StartDate;
                PromotionDAO.EndDate = Promotion.EndDate;
                PromotionDAO.OrganizationId = Promotion.OrganizationId;
                PromotionDAO.PromotionTypeId = Promotion.PromotionTypeId;
                PromotionDAO.Note = Promotion.Note;
                PromotionDAO.Priority = Promotion.Priority;
                PromotionDAO.StatusId = Promotion.StatusId;
                PromotionDAO.CreatedAt = StaticParams.DateTimeNow;
                PromotionDAO.UpdatedAt = StaticParams.DateTimeNow;
                PromotionDAOs.Add(PromotionDAO);
            }
            await DataContext.BulkMergeAsync(PromotionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Promotion> Promotions)
        {
            List<long> Ids = Promotions.Select(x => x.Id).ToList();
            await DataContext.Promotion
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new PromotionDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(Promotion Promotion)
        {
            await DataContext.PromotionPromotionPolicyMapping
                .Where(x => x.PromotionId == Promotion.Id)
                .DeleteFromQueryAsync();
            List<PromotionPromotionPolicyMappingDAO> PromotionPromotionPolicyMappingDAOs = new List<PromotionPromotionPolicyMappingDAO>();
            if (Promotion.PromotionPromotionPolicyMappings != null)
            {
                foreach (PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping in Promotion.PromotionPromotionPolicyMappings)
                {
                    PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = new PromotionPromotionPolicyMappingDAO();
                    PromotionPromotionPolicyMappingDAO.PromotionId = Promotion.Id;
                    PromotionPromotionPolicyMappingDAO.PromotionPolicyId = PromotionPromotionPolicyMapping.PromotionPolicyId;
                    PromotionPromotionPolicyMappingDAO.Note = PromotionPromotionPolicyMapping.Note;
                    PromotionPromotionPolicyMappingDAO.StatusId = PromotionPromotionPolicyMapping.StatusId;
                    PromotionPromotionPolicyMappingDAOs.Add(PromotionPromotionPolicyMappingDAO);
                }
                await DataContext.PromotionPromotionPolicyMapping.BulkMergeAsync(PromotionPromotionPolicyMappingDAOs);
            }

            await DataContext.PromotionStoreGroupingMapping
                .Where(x => x.PromotionId == Promotion.Id)
                .DeleteFromQueryAsync();
            List<PromotionStoreGroupingMappingDAO> PromotionStoreGroupingMappingDAOs = new List<PromotionStoreGroupingMappingDAO>();
            if (Promotion.PromotionStoreGroupingMappings != null)
            {
                foreach (PromotionStoreGroupingMapping PromotionStoreGroupingMapping in Promotion.PromotionStoreGroupingMappings)
                {
                    PromotionStoreGroupingMappingDAO PromotionStoreGroupingMappingDAO = new PromotionStoreGroupingMappingDAO();
                    PromotionStoreGroupingMappingDAO.PromotionId = Promotion.Id;
                    PromotionStoreGroupingMappingDAO.StoreGroupingId = PromotionStoreGroupingMapping.StoreGroupingId;
                    PromotionStoreGroupingMappingDAOs.Add(PromotionStoreGroupingMappingDAO);
                }
                await DataContext.PromotionStoreGroupingMapping.BulkMergeAsync(PromotionStoreGroupingMappingDAOs);
            }

            await DataContext.PromotionStoreMapping
                .Where(x => x.PromotionId == Promotion.Id)
                .DeleteFromQueryAsync();
            List<PromotionStoreMappingDAO> PromotionStoreMappingDAOs = new List<PromotionStoreMappingDAO>();
            if (Promotion.PromotionStoreMappings != null)
            {
                foreach (PromotionStoreMapping PromotionStoreMapping in Promotion.PromotionStoreMappings)
                {
                    PromotionStoreMappingDAO PromotionStoreMappingDAO = new PromotionStoreMappingDAO();
                    PromotionStoreMappingDAO.PromotionId = Promotion.Id;
                    PromotionStoreMappingDAO.StoreId = PromotionStoreMapping.StoreId;
                    PromotionStoreMappingDAOs.Add(PromotionStoreMappingDAO);
                }
                await DataContext.PromotionStoreMapping.BulkMergeAsync(PromotionStoreMappingDAOs);
            }
            await DataContext.PromotionStoreTypeMapping
                .Where(x => x.PromotionId == Promotion.Id)
                .DeleteFromQueryAsync();
            List<PromotionStoreTypeMappingDAO> PromotionStoreTypeMappingDAOs = new List<PromotionStoreTypeMappingDAO>();
            if (Promotion.PromotionStoreTypeMappings != null)
            {
                foreach (PromotionStoreTypeMapping PromotionStoreTypeMapping in Promotion.PromotionStoreTypeMappings)
                {
                    PromotionStoreTypeMappingDAO PromotionStoreTypeMappingDAO = new PromotionStoreTypeMappingDAO();
                    PromotionStoreTypeMappingDAO.PromotionId = Promotion.Id;
                    PromotionStoreTypeMappingDAO.StoreTypeId = PromotionStoreTypeMapping.StoreTypeId;
                    PromotionStoreTypeMappingDAOs.Add(PromotionStoreTypeMappingDAO);
                }
                await DataContext.PromotionStoreTypeMapping.BulkMergeAsync(PromotionStoreTypeMappingDAOs);
            }
        }
    }
}
