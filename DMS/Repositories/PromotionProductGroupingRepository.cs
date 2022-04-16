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
    public interface IPromotionProductGroupingRepository
    {
        Task<int> Count(PromotionProductGroupingFilter PromotionProductGroupingFilter);
        Task<int> CountAll(PromotionProductGroupingFilter PromotionProductGroupingFilter);
        Task<List<PromotionProductGrouping>> List(PromotionProductGroupingFilter PromotionProductGroupingFilter);
        Task<PromotionProductGrouping> Get(long Id);
        Task<bool> Create(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> Update(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> Delete(PromotionProductGrouping PromotionProductGrouping);
        Task<bool> BulkMerge(List<PromotionProductGrouping> PromotionProductGroupings);
        Task<bool> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings);
    }
    public class PromotionProductGroupingRepository : IPromotionProductGroupingRepository
    {
        private DataContext DataContext;
        public PromotionProductGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionProductGroupingDAO> DynamicFilter(IQueryable<PromotionProductGroupingDAO> query, PromotionProductGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PromotionPolicyId, filter.PromotionPolicyId);
            query = query.Where(q => q.PromotionId, filter.PromotionId);
            query = query.Where(q => q.ProductGroupingId, filter.ProductGroupingId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.FromValue, filter.FromValue);
            query = query.Where(q => q.ToValue, filter.ToValue);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountValue, filter.DiscountValue);
            return query;
        }

        private IQueryable<PromotionProductGroupingDAO> OrFilter(IQueryable<PromotionProductGroupingDAO> query, PromotionProductGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionProductGroupingDAO> initQuery = query.Where(q => false);
            foreach (PromotionProductGroupingFilter PromotionProductGroupingFilter in filter.OrFilter)
            {
                IQueryable<PromotionProductGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionProductGroupingFilter.Id);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionProductGroupingFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionProductGroupingFilter.PromotionId);
                queryable = queryable.Where(q => q.ProductGroupingId, PromotionProductGroupingFilter.ProductGroupingId);
                queryable = queryable.Where(q => q.Note, PromotionProductGroupingFilter.Note);
                queryable = queryable.Where(q => q.FromValue, PromotionProductGroupingFilter.FromValue);
                queryable = queryable.Where(q => q.ToValue, PromotionProductGroupingFilter.ToValue);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionProductGroupingFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionProductGroupingFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionProductGroupingFilter.DiscountValue);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionProductGroupingDAO> DynamicOrder(IQueryable<PromotionProductGroupingDAO> query, PromotionProductGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionProductGroupingOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductGroupingOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionProductGroupingOrder.ProductGrouping:
                            query = query.OrderBy(q => q.ProductGroupingId);
                            break;
                        case PromotionProductGroupingOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionProductGroupingOrder.FromValue:
                            query = query.OrderBy(q => q.FromValue);
                            break;
                        case PromotionProductGroupingOrder.ToValue:
                            query = query.OrderBy(q => q.ToValue);
                            break;
                        case PromotionProductGroupingOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductGroupingOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionProductGroupingOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionProductGroupingOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionProductGroupingOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionProductGroupingOrder.ProductGrouping:
                            query = query.OrderByDescending(q => q.ProductGroupingId);
                            break;
                        case PromotionProductGroupingOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionProductGroupingOrder.FromValue:
                            query = query.OrderByDescending(q => q.FromValue);
                            break;
                        case PromotionProductGroupingOrder.ToValue:
                            query = query.OrderByDescending(q => q.ToValue);
                            break;
                        case PromotionProductGroupingOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionProductGroupingOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionProductGroupingOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionProductGrouping>> DynamicSelect(IQueryable<PromotionProductGroupingDAO> query, PromotionProductGroupingFilter filter)
        {
            List<PromotionProductGrouping> PromotionProductGroupings = await query.Select(q => new PromotionProductGrouping()
            {
                Id = filter.Selects.Contains(PromotionProductGroupingSelect.Id) ? q.Id : default(long),
                PromotionPolicyId = filter.Selects.Contains(PromotionProductGroupingSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionProductGroupingSelect.Promotion) ? q.PromotionId : default(long),
                ProductGroupingId = filter.Selects.Contains(PromotionProductGroupingSelect.ProductGrouping) ? q.ProductGroupingId : default(long),
                Note = filter.Selects.Contains(PromotionProductGroupingSelect.Note) ? q.Note : default(string),
                FromValue = filter.Selects.Contains(PromotionProductGroupingSelect.FromValue) ? q.FromValue : default(decimal),
                ToValue = filter.Selects.Contains(PromotionProductGroupingSelect.ToValue) ? q.ToValue : default(decimal),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionProductGroupingSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionProductGroupingSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionProductGroupingSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                ProductGrouping = filter.Selects.Contains(PromotionProductGroupingSelect.ProductGrouping) && q.ProductGrouping != null ? new ProductGrouping
                {
                    Id = q.ProductGrouping.Id,
                    Code = q.ProductGrouping.Code,
                    Name = q.ProductGrouping.Name,
                    Description = q.ProductGrouping.Description,
                    ParentId = q.ProductGrouping.ParentId,
                    Path = q.ProductGrouping.Path,
                    Level = q.ProductGrouping.Level,
                } : null,
                Promotion = filter.Selects.Contains(PromotionProductGroupingSelect.Promotion) && q.Promotion != null ? new Promotion
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
                PromotionDiscountType = filter.Selects.Contains(PromotionProductGroupingSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionProductGroupingSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionProductGroupings;
        }

        public async Task<int> Count(PromotionProductGroupingFilter filter)
        {
            IQueryable<PromotionProductGroupingDAO> PromotionProductGroupings = DataContext.PromotionProductGrouping.AsNoTracking();
            PromotionProductGroupings = DynamicFilter(PromotionProductGroupings, filter);
            PromotionProductGroupings = OrFilter(PromotionProductGroupings, filter);
            return await PromotionProductGroupings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionProductGroupingFilter filter)
        {
            IQueryable<PromotionProductGroupingDAO> PromotionProductGroupings = DataContext.PromotionProductGrouping.AsNoTracking();
            PromotionProductGroupings = DynamicFilter(PromotionProductGroupings, filter);
            return await PromotionProductGroupings.CountWithNoLockAsync();
        }

        public async Task<List<PromotionProductGrouping>> List(PromotionProductGroupingFilter filter)
        {
            if (filter == null) return new List<PromotionProductGrouping>();
            IQueryable<PromotionProductGroupingDAO> PromotionProductGroupingDAOs = DataContext.PromotionProductGrouping.AsNoTracking();
            PromotionProductGroupingDAOs = DynamicFilter(PromotionProductGroupingDAOs, filter);
            PromotionProductGroupingDAOs = OrFilter(PromotionProductGroupingDAOs, filter);
            PromotionProductGroupingDAOs = DynamicOrder(PromotionProductGroupingDAOs, filter);
            List<PromotionProductGrouping> PromotionProductGroupings = await DynamicSelect(PromotionProductGroupingDAOs, filter);
            return PromotionProductGroupings;
        }

        public async Task<PromotionProductGrouping> Get(long Id)
        {
            PromotionProductGrouping PromotionProductGrouping = await DataContext.PromotionProductGrouping.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionProductGrouping()
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
                ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                {
                    Id = x.ProductGrouping.Id,
                    Code = x.ProductGrouping.Code,
                    Name = x.ProductGrouping.Name,
                    Description = x.ProductGrouping.Description,
                    ParentId = x.ProductGrouping.ParentId,
                    Path = x.ProductGrouping.Path,
                    Level = x.ProductGrouping.Level,
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

            if (PromotionProductGrouping == null)
                return null;
            PromotionProductGrouping.PromotionProductGroupingItemMappings = await DataContext.PromotionProductGroupingItemMapping.AsNoTracking()
                .Where(x => x.PromotionProductGroupingId == PromotionProductGrouping.Id)
                .Where(x => x.Item.DeletedAt == null)
                .Select(x => new PromotionProductGroupingItemMapping
                {
                    PromotionProductGroupingId = x.PromotionProductGroupingId,
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

            return PromotionProductGrouping;
        }
        public async Task<bool> Create(PromotionProductGrouping PromotionProductGrouping)
        {
            PromotionProductGroupingDAO PromotionProductGroupingDAO = new PromotionProductGroupingDAO();
            PromotionProductGroupingDAO.Id = PromotionProductGrouping.Id;
            PromotionProductGroupingDAO.PromotionPolicyId = PromotionProductGrouping.PromotionPolicyId;
            PromotionProductGroupingDAO.PromotionId = PromotionProductGrouping.PromotionId;
            PromotionProductGroupingDAO.ProductGroupingId = PromotionProductGrouping.ProductGroupingId;
            PromotionProductGroupingDAO.Note = PromotionProductGrouping.Note;
            PromotionProductGroupingDAO.FromValue = PromotionProductGrouping.FromValue;
            PromotionProductGroupingDAO.ToValue = PromotionProductGrouping.ToValue;
            PromotionProductGroupingDAO.PromotionDiscountTypeId = PromotionProductGrouping.PromotionDiscountTypeId;
            PromotionProductGroupingDAO.DiscountPercentage = PromotionProductGrouping.DiscountPercentage;
            PromotionProductGroupingDAO.DiscountValue = PromotionProductGrouping.DiscountValue;
            DataContext.PromotionProductGrouping.Add(PromotionProductGroupingDAO);
            await DataContext.SaveChangesAsync();
            PromotionProductGrouping.Id = PromotionProductGroupingDAO.Id;
            await SaveReference(PromotionProductGrouping);
            return true;
        }

        public async Task<bool> Update(PromotionProductGrouping PromotionProductGrouping)
        {
            PromotionProductGroupingDAO PromotionProductGroupingDAO = DataContext.PromotionProductGrouping.Where(x => x.Id == PromotionProductGrouping.Id).FirstOrDefault();
            if (PromotionProductGroupingDAO == null)
                return false;
            PromotionProductGroupingDAO.Id = PromotionProductGrouping.Id;
            PromotionProductGroupingDAO.PromotionPolicyId = PromotionProductGrouping.PromotionPolicyId;
            PromotionProductGroupingDAO.PromotionId = PromotionProductGrouping.PromotionId;
            PromotionProductGroupingDAO.ProductGroupingId = PromotionProductGrouping.ProductGroupingId;
            PromotionProductGroupingDAO.Note = PromotionProductGrouping.Note;
            PromotionProductGroupingDAO.FromValue = PromotionProductGrouping.FromValue;
            PromotionProductGroupingDAO.ToValue = PromotionProductGrouping.ToValue;
            PromotionProductGroupingDAO.PromotionDiscountTypeId = PromotionProductGrouping.PromotionDiscountTypeId;
            PromotionProductGroupingDAO.DiscountPercentage = PromotionProductGrouping.DiscountPercentage;
            PromotionProductGroupingDAO.DiscountValue = PromotionProductGrouping.DiscountValue;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionProductGrouping);
            return true;
        }

        public async Task<bool> Delete(PromotionProductGrouping PromotionProductGrouping)
        {
            await DataContext.PromotionProductGrouping.Where(x => x.Id == PromotionProductGrouping.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            List<PromotionProductGroupingDAO> PromotionProductGroupingDAOs = new List<PromotionProductGroupingDAO>();
            foreach (PromotionProductGrouping PromotionProductGrouping in PromotionProductGroupings)
            {
                PromotionProductGroupingDAO PromotionProductGroupingDAO = new PromotionProductGroupingDAO();
                PromotionProductGroupingDAO.Id = PromotionProductGrouping.Id;
                PromotionProductGroupingDAO.PromotionPolicyId = PromotionProductGrouping.PromotionPolicyId;
                PromotionProductGroupingDAO.PromotionId = PromotionProductGrouping.PromotionId;
                PromotionProductGroupingDAO.ProductGroupingId = PromotionProductGrouping.ProductGroupingId;
                PromotionProductGroupingDAO.Note = PromotionProductGrouping.Note;
                PromotionProductGroupingDAO.FromValue = PromotionProductGrouping.FromValue;
                PromotionProductGroupingDAO.ToValue = PromotionProductGrouping.ToValue;
                PromotionProductGroupingDAO.PromotionDiscountTypeId = PromotionProductGrouping.PromotionDiscountTypeId;
                PromotionProductGroupingDAO.DiscountPercentage = PromotionProductGrouping.DiscountPercentage;
                PromotionProductGroupingDAO.DiscountValue = PromotionProductGrouping.DiscountValue;
                PromotionProductGroupingDAOs.Add(PromotionProductGroupingDAO);
            }
            await DataContext.BulkMergeAsync(PromotionProductGroupingDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            List<long> Ids = PromotionProductGroupings.Select(x => x.Id).ToList();
            await DataContext.PromotionProductGrouping
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionProductGrouping PromotionProductGrouping)
        {
            await DataContext.PromotionProductGroupingItemMapping
                .Where(x => x.PromotionProductGroupingId == PromotionProductGrouping.Id)
                .DeleteFromQueryAsync();
            List<PromotionProductGroupingItemMappingDAO> PromotionProductGroupingItemMappingDAOs = new List<PromotionProductGroupingItemMappingDAO>();
            if (PromotionProductGrouping.PromotionProductGroupingItemMappings != null)
            {
                foreach (PromotionProductGroupingItemMapping PromotionProductGroupingItemMapping in PromotionProductGrouping.PromotionProductGroupingItemMappings)
                {
                    PromotionProductGroupingItemMappingDAO PromotionProductGroupingItemMappingDAO = new PromotionProductGroupingItemMappingDAO();
                    PromotionProductGroupingItemMappingDAO.PromotionProductGroupingId = PromotionProductGrouping.Id;
                    PromotionProductGroupingItemMappingDAO.ItemId = PromotionProductGroupingItemMapping.ItemId;
                    PromotionProductGroupingItemMappingDAO.Quantity = PromotionProductGroupingItemMapping.Quantity;
                    PromotionProductGroupingItemMappingDAOs.Add(PromotionProductGroupingItemMappingDAO);
                }
                await DataContext.PromotionProductGroupingItemMapping.BulkMergeAsync(PromotionProductGroupingItemMappingDAOs);
            }
        }

    }
}
