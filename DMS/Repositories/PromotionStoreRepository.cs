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
    public interface IPromotionStoreRepository
    {
        Task<int> Count(PromotionStoreFilter PromotionStoreFilter);
        Task<int> CountAll(PromotionStoreFilter PromotionStoreFilter);
        Task<List<PromotionStore>> List(PromotionStoreFilter PromotionStoreFilter);
        Task<PromotionStore> Get(long Id);
        Task<bool> Create(PromotionStore PromotionStore);
        Task<bool> Update(PromotionStore PromotionStore);
        Task<bool> Delete(PromotionStore PromotionStore);
        Task<bool> BulkMerge(List<PromotionStore> PromotionStores);
        Task<bool> BulkDelete(List<PromotionStore> PromotionStores);
    }
    public class PromotionStoreRepository : IPromotionStoreRepository
    {
        private DataContext DataContext;
        public PromotionStoreRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionStoreDAO> DynamicFilter(IQueryable<PromotionStoreDAO> query, PromotionStoreFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PromotionPolicyId, filter.PromotionPolicyId);
            query = query.Where(q => q.PromotionId, filter.PromotionId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.FromValue, filter.FromValue);
            query = query.Where(q => q.ToValue, filter.ToValue);
            query = query.Where(q => q.PromotionDiscountTypeId, filter.PromotionDiscountTypeId);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountValue, filter.DiscountValue);
            return query;
        }

        private IQueryable<PromotionStoreDAO> OrFilter(IQueryable<PromotionStoreDAO> query, PromotionStoreFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionStoreDAO> initQuery = query.Where(q => false);
            foreach (PromotionStoreFilter PromotionStoreFilter in filter.OrFilter)
            {
                IQueryable<PromotionStoreDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionStoreFilter.Id);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionStoreFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionStoreFilter.PromotionId);
                queryable = queryable.Where(q => q.Note, PromotionStoreFilter.Note);
                queryable = queryable.Where(q => q.FromValue, PromotionStoreFilter.FromValue);
                queryable = queryable.Where(q => q.ToValue, PromotionStoreFilter.ToValue);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionStoreFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionStoreFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionStoreFilter.DiscountValue);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionStoreDAO> DynamicOrder(IQueryable<PromotionStoreDAO> query, PromotionStoreFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionStoreOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionStoreOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionStoreOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionStoreOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionStoreOrder.FromValue:
                            query = query.OrderBy(q => q.FromValue);
                            break;
                        case PromotionStoreOrder.ToValue:
                            query = query.OrderBy(q => q.ToValue);
                            break;
                        case PromotionStoreOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionStoreOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionStoreOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionStoreOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionStoreOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionStoreOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionStoreOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionStoreOrder.FromValue:
                            query = query.OrderByDescending(q => q.FromValue);
                            break;
                        case PromotionStoreOrder.ToValue:
                            query = query.OrderByDescending(q => q.ToValue);
                            break;
                        case PromotionStoreOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionStoreOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionStoreOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionStore>> DynamicSelect(IQueryable<PromotionStoreDAO> query, PromotionStoreFilter filter)
        {
            List<PromotionStore> PromotionStores = await query.Select(q => new PromotionStore()
            {
                Id = filter.Selects.Contains(PromotionStoreSelect.Id) ? q.Id : default(long),
                PromotionPolicyId = filter.Selects.Contains(PromotionStoreSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionStoreSelect.Promotion) ? q.PromotionId : default(long),
                Note = filter.Selects.Contains(PromotionStoreSelect.Note) ? q.Note : default(string),
                FromValue = filter.Selects.Contains(PromotionStoreSelect.FromValue) ? q.FromValue : default(decimal),
                ToValue = filter.Selects.Contains(PromotionStoreSelect.ToValue) ? q.ToValue : default(decimal),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionStoreSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionStoreSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionStoreSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                Promotion = filter.Selects.Contains(PromotionStoreSelect.Promotion) && q.Promotion != null ? new Promotion
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
                PromotionDiscountType = filter.Selects.Contains(PromotionStoreSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionStoreSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionStores;
        }

        public async Task<int> Count(PromotionStoreFilter filter)
        {
            IQueryable<PromotionStoreDAO> PromotionStores = DataContext.PromotionStore.AsNoTracking();
            PromotionStores = DynamicFilter(PromotionStores, filter);
            PromotionStores = OrFilter(PromotionStores, filter);
            return await PromotionStores.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionStoreFilter filter)
        {
            IQueryable<PromotionStoreDAO> PromotionStores = DataContext.PromotionStore.AsNoTracking();
            PromotionStores = DynamicFilter(PromotionStores, filter);
            return await PromotionStores.CountWithNoLockAsync();
        }

        public async Task<List<PromotionStore>> List(PromotionStoreFilter filter)
        {
            if (filter == null) return new List<PromotionStore>();
            IQueryable<PromotionStoreDAO> PromotionStoreDAOs = DataContext.PromotionStore.AsNoTracking();
            PromotionStoreDAOs = DynamicFilter(PromotionStoreDAOs, filter);
            PromotionStoreDAOs = OrFilter(PromotionStoreDAOs, filter);
            PromotionStoreDAOs = DynamicOrder(PromotionStoreDAOs, filter);
            List<PromotionStore> PromotionStores = await DynamicSelect(PromotionStoreDAOs, filter);
            return PromotionStores;
        }

        public async Task<PromotionStore> Get(long Id)
        {
            PromotionStore PromotionStore = await DataContext.PromotionStore.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionStore()
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

            if (PromotionStore == null)
                return null;
            PromotionStore.PromotionStoreItemMappings = await DataContext.PromotionStoreItemMapping.AsNoTracking()
                .Where(x => x.PromotionStoreId == PromotionStore.Id)
                .Where(x => x.Item.DeletedAt == null)
                .Select(x => new PromotionStoreItemMapping
                {
                    PromotionStoreId = x.PromotionStoreId,
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

            return PromotionStore;
        }
        public async Task<bool> Create(PromotionStore PromotionStore)
        {
            PromotionStoreDAO PromotionStoreDAO = new PromotionStoreDAO();
            PromotionStoreDAO.Id = PromotionStore.Id;
            PromotionStoreDAO.PromotionPolicyId = PromotionStore.PromotionPolicyId;
            PromotionStoreDAO.PromotionId = PromotionStore.PromotionId;
            PromotionStoreDAO.Note = PromotionStore.Note;
            PromotionStoreDAO.FromValue = PromotionStore.FromValue;
            PromotionStoreDAO.ToValue = PromotionStore.ToValue;
            PromotionStoreDAO.PromotionDiscountTypeId = PromotionStore.PromotionDiscountTypeId;
            PromotionStoreDAO.DiscountPercentage = PromotionStore.DiscountPercentage;
            PromotionStoreDAO.DiscountValue = PromotionStore.DiscountValue;
            DataContext.PromotionStore.Add(PromotionStoreDAO);
            await DataContext.SaveChangesAsync();
            PromotionStore.Id = PromotionStoreDAO.Id;
            await SaveReference(PromotionStore);
            return true;
        }

        public async Task<bool> Update(PromotionStore PromotionStore)
        {
            PromotionStoreDAO PromotionStoreDAO = DataContext.PromotionStore.Where(x => x.Id == PromotionStore.Id).FirstOrDefault();
            if (PromotionStoreDAO == null)
                return false;
            PromotionStoreDAO.Id = PromotionStore.Id;
            PromotionStoreDAO.PromotionPolicyId = PromotionStore.PromotionPolicyId;
            PromotionStoreDAO.PromotionId = PromotionStore.PromotionId;
            PromotionStoreDAO.Note = PromotionStore.Note;
            PromotionStoreDAO.FromValue = PromotionStore.FromValue;
            PromotionStoreDAO.ToValue = PromotionStore.ToValue;
            PromotionStoreDAO.PromotionDiscountTypeId = PromotionStore.PromotionDiscountTypeId;
            PromotionStoreDAO.DiscountPercentage = PromotionStore.DiscountPercentage;
            PromotionStoreDAO.DiscountValue = PromotionStore.DiscountValue;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionStore);
            return true;
        }

        public async Task<bool> Delete(PromotionStore PromotionStore)
        {
            await DataContext.PromotionStore.Where(x => x.Id == PromotionStore.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionStore> PromotionStores)
        {
            List<PromotionStoreDAO> PromotionStoreDAOs = new List<PromotionStoreDAO>();
            foreach (PromotionStore PromotionStore in PromotionStores)
            {
                PromotionStoreDAO PromotionStoreDAO = new PromotionStoreDAO();
                PromotionStoreDAO.Id = PromotionStore.Id;
                PromotionStoreDAO.PromotionPolicyId = PromotionStore.PromotionPolicyId;
                PromotionStoreDAO.PromotionId = PromotionStore.PromotionId;
                PromotionStoreDAO.Note = PromotionStore.Note;
                PromotionStoreDAO.FromValue = PromotionStore.FromValue;
                PromotionStoreDAO.ToValue = PromotionStore.ToValue;
                PromotionStoreDAO.PromotionDiscountTypeId = PromotionStore.PromotionDiscountTypeId;
                PromotionStoreDAO.DiscountPercentage = PromotionStore.DiscountPercentage;
                PromotionStoreDAO.DiscountValue = PromotionStore.DiscountValue;
                PromotionStoreDAOs.Add(PromotionStoreDAO);
            }
            await DataContext.BulkMergeAsync(PromotionStoreDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionStore> PromotionStores)
        {
            List<long> Ids = PromotionStores.Select(x => x.Id).ToList();
            await DataContext.PromotionStore
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionStore PromotionStore)
        {
            await DataContext.PromotionStoreItemMapping
                .Where(x => x.PromotionStoreId == PromotionStore.Id)
                .DeleteFromQueryAsync();
            List<PromotionStoreItemMappingDAO> PromotionStoreItemMappingDAOs = new List<PromotionStoreItemMappingDAO>();
            if (PromotionStore.PromotionStoreItemMappings != null)
            {
                foreach (PromotionStoreItemMapping PromotionStoreItemMapping in PromotionStore.PromotionStoreItemMappings)
                {
                    PromotionStoreItemMappingDAO PromotionStoreItemMappingDAO = new PromotionStoreItemMappingDAO();
                    PromotionStoreItemMappingDAO.PromotionStoreId = PromotionStore.Id;
                    PromotionStoreItemMappingDAO.ItemId = PromotionStoreItemMapping.ItemId;
                    PromotionStoreItemMappingDAO.Quantity = PromotionStoreItemMapping.Quantity;
                    PromotionStoreItemMappingDAOs.Add(PromotionStoreItemMappingDAO);
                }
                await DataContext.PromotionStoreItemMapping.BulkMergeAsync(PromotionStoreItemMappingDAOs);
            }
        }

    }
}
