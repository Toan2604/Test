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
    public interface IPromotionProductTypeRepository
    {
        Task<int> Count(PromotionProductTypeFilter PromotionProductTypeFilter);
        Task<int> CountAll(PromotionProductTypeFilter PromotionProductTypeFilter);
        Task<List<PromotionProductType>> List(PromotionProductTypeFilter PromotionProductTypeFilter);
        Task<PromotionProductType> Get(long Id);
        Task<bool> Create(PromotionProductType PromotionProductType);
        Task<bool> Update(PromotionProductType PromotionProductType);
        Task<bool> Delete(PromotionProductType PromotionProductType);
        Task<bool> BulkMerge(List<PromotionProductType> PromotionProductTypes);
        Task<bool> BulkDelete(List<PromotionProductType> PromotionProductTypes);
    }
    public class PromotionProductTypeRepository : IPromotionProductTypeRepository
    {
        private DataContext DataContext;
        public PromotionProductTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionProductTypeDAO> DynamicFilter(IQueryable<PromotionProductTypeDAO> query, PromotionProductTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PromotionPolicyId, filter.PromotionPolicyId);
            query = query.Where(q => q.PromotionId, filter.PromotionId);
            query = query.Where(q => q.ProductTypeId, filter.ProductTypeId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.FromValue, filter.FromValue);
            query = query.Where(q => q.ToValue, filter.ToValue);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountValue, filter.DiscountValue);
            return query;
        }

        private IQueryable<PromotionProductTypeDAO> OrFilter(IQueryable<PromotionProductTypeDAO> query, PromotionProductTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionProductTypeDAO> initQuery = query.Where(q => false);
            foreach (PromotionProductTypeFilter PromotionProductTypeFilter in filter.OrFilter)
            {
                IQueryable<PromotionProductTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionProductTypeFilter.Id);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionProductTypeFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionProductTypeFilter.PromotionId);
                queryable = queryable.Where(q => q.ProductTypeId, PromotionProductTypeFilter.ProductTypeId);
                queryable = queryable.Where(q => q.Note, PromotionProductTypeFilter.Note);
                queryable = queryable.Where(q => q.FromValue, PromotionProductTypeFilter.FromValue);
                queryable = queryable.Where(q => q.ToValue, PromotionProductTypeFilter.ToValue);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionProductTypeFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionProductTypeFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionProductTypeFilter.DiscountValue);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionProductTypeDAO> DynamicOrder(IQueryable<PromotionProductTypeDAO> query, PromotionProductTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionProductTypeOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductTypeOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionProductTypeOrder.ProductType:
                            query = query.OrderBy(q => q.ProductTypeId);
                            break;
                        case PromotionProductTypeOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionProductTypeOrder.FromValue:
                            query = query.OrderBy(q => q.FromValue);
                            break;
                        case PromotionProductTypeOrder.ToValue:
                            query = query.OrderBy(q => q.ToValue);
                            break;
                        case PromotionProductTypeOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductTypeOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionProductTypeOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionProductTypeOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductTypeOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionProductTypeOrder.ProductType:
                            query = query.OrderByDescending(q => q.ProductTypeId);
                            break;
                        case PromotionProductTypeOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionProductTypeOrder.FromValue:
                            query = query.OrderByDescending(q => q.FromValue);
                            break;
                        case PromotionProductTypeOrder.ToValue:
                            query = query.OrderByDescending(q => q.ToValue);
                            break;
                        case PromotionProductTypeOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductTypeOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionProductTypeOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionProductType>> DynamicSelect(IQueryable<PromotionProductTypeDAO> query, PromotionProductTypeFilter filter)
        {
            List<PromotionProductType> PromotionProductTypes = await query.Select(q => new PromotionProductType()
            {
                Id = filter.Selects.Contains(PromotionProductTypeSelect.Id) ? q.Id : default(long),
                PromotionPolicyId = filter.Selects.Contains(PromotionProductTypeSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionProductTypeSelect.Promotion) ? q.PromotionId : default(long),
                ProductTypeId = filter.Selects.Contains(PromotionProductTypeSelect.ProductType) ? q.ProductTypeId : default(long),
                Note = filter.Selects.Contains(PromotionProductTypeSelect.Note) ? q.Note : default(string),
                FromValue = filter.Selects.Contains(PromotionProductTypeSelect.FromValue) ? q.FromValue : default(decimal),
                ToValue = filter.Selects.Contains(PromotionProductTypeSelect.ToValue) ? q.ToValue : default(decimal),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionProductTypeSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionProductTypeSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionProductTypeSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                ProductType = filter.Selects.Contains(PromotionProductTypeSelect.ProductType) && q.ProductType != null ? new ProductType
                {
                    Id = q.ProductType.Id,
                    Code = q.ProductType.Code,
                    Name = q.ProductType.Name,
                    Description = q.ProductType.Description,
                    StatusId = q.ProductType.StatusId,
                    Used = q.ProductType.Used,
                } : null,
                Promotion = filter.Selects.Contains(PromotionProductTypeSelect.Promotion) && q.Promotion != null ? new Promotion
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
                PromotionDiscountType = filter.Selects.Contains(PromotionProductTypeSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionProductTypeSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionProductTypes;
        }

        public async Task<int> Count(PromotionProductTypeFilter filter)
        {
            IQueryable<PromotionProductTypeDAO> PromotionProductTypes = DataContext.PromotionProductType.AsNoTracking();
            PromotionProductTypes = DynamicFilter(PromotionProductTypes, filter);
            PromotionProductTypes = OrFilter(PromotionProductTypes, filter);
            return await PromotionProductTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionProductTypeFilter filter)
        {
            IQueryable<PromotionProductTypeDAO> PromotionProductTypes = DataContext.PromotionProductType.AsNoTracking();
            PromotionProductTypes = DynamicFilter(PromotionProductTypes, filter);
            return await PromotionProductTypes.CountWithNoLockAsync();
        }
        public async Task<List<PromotionProductType>> List(PromotionProductTypeFilter filter)
        {
            if (filter == null) return new List<PromotionProductType>();
            IQueryable<PromotionProductTypeDAO> PromotionProductTypeDAOs = DataContext.PromotionProductType.AsNoTracking();
            PromotionProductTypeDAOs = DynamicFilter(PromotionProductTypeDAOs, filter);
            PromotionProductTypeDAOs = OrFilter(PromotionProductTypeDAOs, filter);
            PromotionProductTypeDAOs = DynamicOrder(PromotionProductTypeDAOs, filter);
            List<PromotionProductType> PromotionProductTypes = await DynamicSelect(PromotionProductTypeDAOs, filter);
            return PromotionProductTypes;
        }

        public async Task<PromotionProductType> Get(long Id)
        {
            PromotionProductType PromotionProductType = await DataContext.PromotionProductType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionProductType()
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
                ProductType = x.ProductType == null ? null : new ProductType
                {
                    Id = x.ProductType.Id,
                    Code = x.ProductType.Code,
                    Name = x.ProductType.Name,
                    Description = x.ProductType.Description,
                    StatusId = x.ProductType.StatusId,
                    Used = x.ProductType.Used,
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

            if (PromotionProductType == null)
                return null;
            PromotionProductType.PromotionProductTypeItemMappings = await DataContext.PromotionProductTypeItemMapping.AsNoTracking()
                .Where(x => x.PromotionProductTypeId == PromotionProductType.Id)
                .Where(x => x.Item.DeletedAt == null)
                .Select(x => new PromotionProductTypeItemMapping
                {
                    PromotionProductTypeId = x.PromotionProductTypeId,
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

            return PromotionProductType;
        }
        public async Task<bool> Create(PromotionProductType PromotionProductType)
        {
            PromotionProductTypeDAO PromotionProductTypeDAO = new PromotionProductTypeDAO();
            PromotionProductTypeDAO.Id = PromotionProductType.Id;
            PromotionProductTypeDAO.PromotionPolicyId = PromotionProductType.PromotionPolicyId;
            PromotionProductTypeDAO.PromotionId = PromotionProductType.PromotionId;
            PromotionProductTypeDAO.ProductTypeId = PromotionProductType.ProductTypeId;
            PromotionProductTypeDAO.Note = PromotionProductType.Note;
            PromotionProductTypeDAO.FromValue = PromotionProductType.FromValue;
            PromotionProductTypeDAO.ToValue = PromotionProductType.ToValue;
            PromotionProductTypeDAO.PromotionDiscountTypeId = PromotionProductType.PromotionDiscountTypeId;
            PromotionProductTypeDAO.DiscountPercentage = PromotionProductType.DiscountPercentage;
            PromotionProductTypeDAO.DiscountValue = PromotionProductType.DiscountValue;
            DataContext.PromotionProductType.Add(PromotionProductTypeDAO);
            await DataContext.SaveChangesAsync();
            PromotionProductType.Id = PromotionProductTypeDAO.Id;
            await SaveReference(PromotionProductType);
            return true;
        }

        public async Task<bool> Update(PromotionProductType PromotionProductType)
        {
            PromotionProductTypeDAO PromotionProductTypeDAO = DataContext.PromotionProductType.Where(x => x.Id == PromotionProductType.Id).FirstOrDefault();
            if (PromotionProductTypeDAO == null)
                return false;
            PromotionProductTypeDAO.Id = PromotionProductType.Id;
            PromotionProductTypeDAO.PromotionPolicyId = PromotionProductType.PromotionPolicyId;
            PromotionProductTypeDAO.PromotionId = PromotionProductType.PromotionId;
            PromotionProductTypeDAO.ProductTypeId = PromotionProductType.ProductTypeId;
            PromotionProductTypeDAO.Note = PromotionProductType.Note;
            PromotionProductTypeDAO.FromValue = PromotionProductType.FromValue;
            PromotionProductTypeDAO.ToValue = PromotionProductType.ToValue;
            PromotionProductTypeDAO.PromotionDiscountTypeId = PromotionProductType.PromotionDiscountTypeId;
            PromotionProductTypeDAO.DiscountPercentage = PromotionProductType.DiscountPercentage;
            PromotionProductTypeDAO.DiscountValue = PromotionProductType.DiscountValue;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionProductType);
            return true;
        }

        public async Task<bool> Delete(PromotionProductType PromotionProductType)
        {
            await DataContext.PromotionProductType.Where(x => x.Id == PromotionProductType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionProductType> PromotionProductTypes)
        {
            List<PromotionProductTypeDAO> PromotionProductTypeDAOs = new List<PromotionProductTypeDAO>();
            foreach (PromotionProductType PromotionProductType in PromotionProductTypes)
            {
                PromotionProductTypeDAO PromotionProductTypeDAO = new PromotionProductTypeDAO();
                PromotionProductTypeDAO.Id = PromotionProductType.Id;
                PromotionProductTypeDAO.PromotionPolicyId = PromotionProductType.PromotionPolicyId;
                PromotionProductTypeDAO.PromotionId = PromotionProductType.PromotionId;
                PromotionProductTypeDAO.ProductTypeId = PromotionProductType.ProductTypeId;
                PromotionProductTypeDAO.Note = PromotionProductType.Note;
                PromotionProductTypeDAO.FromValue = PromotionProductType.FromValue;
                PromotionProductTypeDAO.ToValue = PromotionProductType.ToValue;
                PromotionProductTypeDAO.PromotionDiscountTypeId = PromotionProductType.PromotionDiscountTypeId;
                PromotionProductTypeDAO.DiscountPercentage = PromotionProductType.DiscountPercentage;
                PromotionProductTypeDAO.DiscountValue = PromotionProductType.DiscountValue;
                PromotionProductTypeDAOs.Add(PromotionProductTypeDAO);
            }
            await DataContext.BulkMergeAsync(PromotionProductTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionProductType> PromotionProductTypes)
        {
            List<long> Ids = PromotionProductTypes.Select(x => x.Id).ToList();
            await DataContext.PromotionProductType
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionProductType PromotionProductType)
        {
            await DataContext.PromotionProductTypeItemMapping
                .Where(x => x.PromotionProductTypeId == PromotionProductType.Id)
                .DeleteFromQueryAsync();
            List<PromotionProductTypeItemMappingDAO> PromotionProductTypeItemMappingDAOs = new List<PromotionProductTypeItemMappingDAO>();
            if (PromotionProductType.PromotionProductTypeItemMappings != null)
            {
                foreach (PromotionProductTypeItemMapping PromotionProductTypeItemMapping in PromotionProductType.PromotionProductTypeItemMappings)
                {
                    PromotionProductTypeItemMappingDAO PromotionProductTypeItemMappingDAO = new PromotionProductTypeItemMappingDAO();
                    PromotionProductTypeItemMappingDAO.PromotionProductTypeId = PromotionProductType.Id;
                    PromotionProductTypeItemMappingDAO.ItemId = PromotionProductTypeItemMapping.ItemId;
                    PromotionProductTypeItemMappingDAO.Quantity = PromotionProductTypeItemMapping.Quantity;
                    PromotionProductTypeItemMappingDAOs.Add(PromotionProductTypeItemMappingDAO);
                }
                await DataContext.PromotionProductTypeItemMapping.BulkMergeAsync(PromotionProductTypeItemMappingDAOs);
            }
        }

    }
}
