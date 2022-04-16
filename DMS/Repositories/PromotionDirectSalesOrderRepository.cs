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
    public interface IPromotionDirectSalesOrderRepository
    {
        Task<int> Count(PromotionDirectSalesOrderFilter PromotionDirectSalesOrderFilter);
        Task<int> CountAll(PromotionDirectSalesOrderFilter PromotionDirectSalesOrderFilter);
        Task<List<PromotionDirectSalesOrder>> List(PromotionDirectSalesOrderFilter PromotionDirectSalesOrderFilter);
        Task<PromotionDirectSalesOrder> Get(long Id);
        Task<bool> Create(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> Update(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> Delete(PromotionDirectSalesOrder PromotionDirectSalesOrder);
        Task<bool> BulkMerge(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders);
        Task<bool> BulkDelete(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders);
    }
    public class PromotionDirectSalesOrderRepository : IPromotionDirectSalesOrderRepository
    {
        private DataContext DataContext;
        public PromotionDirectSalesOrderRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionDirectSalesOrderDAO> DynamicFilter(IQueryable<PromotionDirectSalesOrderDAO> query, PromotionDirectSalesOrderFilter filter)
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

        private IQueryable<PromotionDirectSalesOrderDAO> OrFilter(IQueryable<PromotionDirectSalesOrderDAO> query, PromotionDirectSalesOrderFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionDirectSalesOrderDAO> initQuery = query.Where(q => false);
            foreach (PromotionDirectSalesOrderFilter PromotionDirectSalesOrderFilter in filter.OrFilter)
            {
                IQueryable<PromotionDirectSalesOrderDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionDirectSalesOrderFilter.Id);
                queryable = queryable.Where(q => q.PromotionPolicyId, PromotionDirectSalesOrderFilter.PromotionPolicyId);
                queryable = queryable.Where(q => q.PromotionId, PromotionDirectSalesOrderFilter.PromotionId);
                queryable = queryable.Where(q => q.Note, PromotionDirectSalesOrderFilter.Note);
                queryable = queryable.Where(q => q.FromValue, PromotionDirectSalesOrderFilter.FromValue);
                queryable = queryable.Where(q => q.ToValue, PromotionDirectSalesOrderFilter.ToValue);
                queryable = queryable.Where(q => q.PromotionDiscountTypeId, PromotionDirectSalesOrderFilter.PromotionDiscountTypeId);
                queryable = queryable.Where(q => q.DiscountPercentage, PromotionDirectSalesOrderFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountValue, PromotionDirectSalesOrderFilter.DiscountValue);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionDirectSalesOrderDAO> DynamicOrder(IQueryable<PromotionDirectSalesOrderDAO> query, PromotionDirectSalesOrderFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionDirectSalesOrderOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionDirectSalesOrderOrder.PromotionPolicy:
                            query = query.OrderBy(q => q.PromotionPolicyId);
                            break;
                        case PromotionDirectSalesOrderOrder.Promotion:
                            query = query.OrderBy(q => q.PromotionId);
                            break;
                        case PromotionDirectSalesOrderOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case PromotionDirectSalesOrderOrder.FromValue:
                            query = query.OrderBy(q => q.FromValue);
                            break;
                        case PromotionDirectSalesOrderOrder.ToValue:
                            query = query.OrderBy(q => q.ToValue);
                            break;
                        case PromotionDirectSalesOrderOrder.PromotionDiscountType:
                            query = query.OrderBy(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionDirectSalesOrderOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case PromotionDirectSalesOrderOrder.DiscountValue:
                            query = query.OrderBy(q => q.DiscountValue);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionDirectSalesOrderOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionDirectSalesOrderOrder.PromotionPolicy:
                            query = query.OrderByDescending(q => q.PromotionPolicyId);
                            break;
                        case PromotionDirectSalesOrderOrder.Promotion:
                            query = query.OrderByDescending(q => q.PromotionId);
                            break;
                        case PromotionDirectSalesOrderOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case PromotionDirectSalesOrderOrder.FromValue:
                            query = query.OrderByDescending(q => q.FromValue);
                            break;
                        case PromotionDirectSalesOrderOrder.ToValue:
                            query = query.OrderByDescending(q => q.ToValue);
                            break;
                        case PromotionDirectSalesOrderOrder.PromotionDiscountType:
                            query = query.OrderByDescending(q => q.PromotionDiscountTypeId);
                            break;
                        case PromotionDirectSalesOrderOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case PromotionDirectSalesOrderOrder.DiscountValue:
                            query = query.OrderByDescending(q => q.DiscountValue);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionDirectSalesOrder>> DynamicSelect(IQueryable<PromotionDirectSalesOrderDAO> query, PromotionDirectSalesOrderFilter filter)
        {
            List<PromotionDirectSalesOrder> PromotionDirectSalesOrders = await query.Select(q => new PromotionDirectSalesOrder()
            {
                Id = filter.Selects.Contains(PromotionDirectSalesOrderSelect.Id) ? q.Id : default(long),
                PromotionPolicyId = filter.Selects.Contains(PromotionDirectSalesOrderSelect.PromotionPolicy) ? q.PromotionPolicyId : default(long),
                PromotionId = filter.Selects.Contains(PromotionDirectSalesOrderSelect.Promotion) ? q.PromotionId : default(long),
                Note = filter.Selects.Contains(PromotionDirectSalesOrderSelect.Note) ? q.Note : default(string),
                FromValue = filter.Selects.Contains(PromotionDirectSalesOrderSelect.FromValue) ? q.FromValue : default(decimal),
                ToValue = filter.Selects.Contains(PromotionDirectSalesOrderSelect.ToValue) ? q.ToValue : default(decimal),
                PromotionDiscountTypeId = filter.Selects.Contains(PromotionDirectSalesOrderSelect.PromotionDiscountType) ? q.PromotionDiscountTypeId : default(long),
                DiscountPercentage = filter.Selects.Contains(PromotionDirectSalesOrderSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountValue = filter.Selects.Contains(PromotionDirectSalesOrderSelect.DiscountValue) ? q.DiscountValue : default(decimal?),
                Promotion = filter.Selects.Contains(PromotionDirectSalesOrderSelect.Promotion) && q.Promotion != null ? new Promotion
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
                PromotionDiscountType = filter.Selects.Contains(PromotionDirectSalesOrderSelect.PromotionDiscountType) && q.PromotionDiscountType != null ? new PromotionDiscountType
                {
                    Id = q.PromotionDiscountType.Id,
                    Code = q.PromotionDiscountType.Code,
                    Name = q.PromotionDiscountType.Name,
                } : null,
                PromotionPolicy = filter.Selects.Contains(PromotionDirectSalesOrderSelect.PromotionPolicy) && q.PromotionPolicy != null ? new PromotionPolicy
                {
                    Id = q.PromotionPolicy.Id,
                    Code = q.PromotionPolicy.Code,
                    Name = q.PromotionPolicy.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionDirectSalesOrders;
        }

        public async Task<int> Count(PromotionDirectSalesOrderFilter filter)
        {
            IQueryable<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrders = DataContext.PromotionDirectSalesOrder.AsNoTracking();
            PromotionDirectSalesOrders = DynamicFilter(PromotionDirectSalesOrders, filter);
            PromotionDirectSalesOrders = OrFilter(PromotionDirectSalesOrders, filter);
            return await PromotionDirectSalesOrders.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionDirectSalesOrderFilter filter)
        {
            IQueryable<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrders = DataContext.PromotionDirectSalesOrder.AsNoTracking();
            PromotionDirectSalesOrders = DynamicFilter(PromotionDirectSalesOrders, filter);
            return await PromotionDirectSalesOrders.CountWithNoLockAsync();
        }

        public async Task<List<PromotionDirectSalesOrder>> List(PromotionDirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<PromotionDirectSalesOrder>();
            IQueryable<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrderDAOs = DataContext.PromotionDirectSalesOrder.AsNoTracking();
            PromotionDirectSalesOrderDAOs = DynamicFilter(PromotionDirectSalesOrderDAOs, filter);
            PromotionDirectSalesOrderDAOs = OrFilter(PromotionDirectSalesOrderDAOs, filter);
            PromotionDirectSalesOrderDAOs = DynamicOrder(PromotionDirectSalesOrderDAOs, filter);
            List<PromotionDirectSalesOrder> PromotionDirectSalesOrders = await DynamicSelect(PromotionDirectSalesOrderDAOs, filter);
            return PromotionDirectSalesOrders;
        }

        public async Task<PromotionDirectSalesOrder> Get(long Id)
        {
            PromotionDirectSalesOrder PromotionDirectSalesOrder = await DataContext.PromotionDirectSalesOrder.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionDirectSalesOrder()
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

            if (PromotionDirectSalesOrder == null)
                return null;
            PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings = await DataContext.PromotionDirectSalesOrderItemMapping.AsNoTracking()
                .Where(x => x.PromotionDirectSalesOrderId == PromotionDirectSalesOrder.Id)
                .Where(x => x.Item.DeletedAt == null)
                .Select(x => new PromotionDirectSalesOrderItemMapping
                {
                    PromotionDirectSalesOrderId = x.PromotionDirectSalesOrderId,
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

            return PromotionDirectSalesOrder;
        }
        public async Task<bool> Create(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            PromotionDirectSalesOrderDAO PromotionDirectSalesOrderDAO = new PromotionDirectSalesOrderDAO();
            PromotionDirectSalesOrderDAO.Id = PromotionDirectSalesOrder.Id;
            PromotionDirectSalesOrderDAO.PromotionPolicyId = PromotionDirectSalesOrder.PromotionPolicyId;
            PromotionDirectSalesOrderDAO.PromotionId = PromotionDirectSalesOrder.PromotionId;
            PromotionDirectSalesOrderDAO.Note = PromotionDirectSalesOrder.Note;
            PromotionDirectSalesOrderDAO.FromValue = PromotionDirectSalesOrder.FromValue;
            PromotionDirectSalesOrderDAO.ToValue = PromotionDirectSalesOrder.ToValue;
            PromotionDirectSalesOrderDAO.PromotionDiscountTypeId = PromotionDirectSalesOrder.PromotionDiscountTypeId;
            PromotionDirectSalesOrderDAO.DiscountPercentage = PromotionDirectSalesOrder.DiscountPercentage;
            PromotionDirectSalesOrderDAO.DiscountValue = PromotionDirectSalesOrder.DiscountValue;
            DataContext.PromotionDirectSalesOrder.Add(PromotionDirectSalesOrderDAO);
            await DataContext.SaveChangesAsync();
            PromotionDirectSalesOrder.Id = PromotionDirectSalesOrderDAO.Id;
            await SaveReference(PromotionDirectSalesOrder);
            return true;
        }

        public async Task<bool> Update(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            PromotionDirectSalesOrderDAO PromotionDirectSalesOrderDAO = DataContext.PromotionDirectSalesOrder.Where(x => x.Id == PromotionDirectSalesOrder.Id).FirstOrDefault();
            if (PromotionDirectSalesOrderDAO == null)
                return false;
            PromotionDirectSalesOrderDAO.Id = PromotionDirectSalesOrder.Id;
            PromotionDirectSalesOrderDAO.PromotionPolicyId = PromotionDirectSalesOrder.PromotionPolicyId;
            PromotionDirectSalesOrderDAO.PromotionId = PromotionDirectSalesOrder.PromotionId;
            PromotionDirectSalesOrderDAO.Note = PromotionDirectSalesOrder.Note;
            PromotionDirectSalesOrderDAO.FromValue = PromotionDirectSalesOrder.FromValue;
            PromotionDirectSalesOrderDAO.ToValue = PromotionDirectSalesOrder.ToValue;
            PromotionDirectSalesOrderDAO.PromotionDiscountTypeId = PromotionDirectSalesOrder.PromotionDiscountTypeId;
            PromotionDirectSalesOrderDAO.DiscountPercentage = PromotionDirectSalesOrder.DiscountPercentage;
            PromotionDirectSalesOrderDAO.DiscountValue = PromotionDirectSalesOrder.DiscountValue;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionDirectSalesOrder);
            return true;
        }

        public async Task<bool> Delete(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            await DataContext.PromotionDirectSalesOrder.Where(x => x.Id == PromotionDirectSalesOrder.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders)
        {
            List<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrderDAOs = new List<PromotionDirectSalesOrderDAO>();
            foreach (PromotionDirectSalesOrder PromotionDirectSalesOrder in PromotionDirectSalesOrders)
            {
                PromotionDirectSalesOrderDAO PromotionDirectSalesOrderDAO = new PromotionDirectSalesOrderDAO();
                PromotionDirectSalesOrderDAO.Id = PromotionDirectSalesOrder.Id;
                PromotionDirectSalesOrderDAO.PromotionPolicyId = PromotionDirectSalesOrder.PromotionPolicyId;
                PromotionDirectSalesOrderDAO.PromotionId = PromotionDirectSalesOrder.PromotionId;
                PromotionDirectSalesOrderDAO.Note = PromotionDirectSalesOrder.Note;
                PromotionDirectSalesOrderDAO.FromValue = PromotionDirectSalesOrder.FromValue;
                PromotionDirectSalesOrderDAO.ToValue = PromotionDirectSalesOrder.ToValue;
                PromotionDirectSalesOrderDAO.PromotionDiscountTypeId = PromotionDirectSalesOrder.PromotionDiscountTypeId;
                PromotionDirectSalesOrderDAO.DiscountPercentage = PromotionDirectSalesOrder.DiscountPercentage;
                PromotionDirectSalesOrderDAO.DiscountValue = PromotionDirectSalesOrder.DiscountValue;
                PromotionDirectSalesOrderDAOs.Add(PromotionDirectSalesOrderDAO);
            }
            await DataContext.BulkMergeAsync(PromotionDirectSalesOrderDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionDirectSalesOrder> PromotionDirectSalesOrders)
        {
            List<long> Ids = PromotionDirectSalesOrders.Select(x => x.Id).ToList();
            await DataContext.PromotionDirectSalesOrder
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionDirectSalesOrder PromotionDirectSalesOrder)
        {
            await DataContext.PromotionDirectSalesOrderItemMapping
                .Where(x => x.PromotionDirectSalesOrderId == PromotionDirectSalesOrder.Id)
                .DeleteFromQueryAsync();
            List<PromotionDirectSalesOrderItemMappingDAO> PromotionDirectSalesOrderItemMappingDAOs = new List<PromotionDirectSalesOrderItemMappingDAO>();
            if (PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings != null)
            {
                foreach (PromotionDirectSalesOrderItemMapping PromotionDirectSalesOrderItemMapping in PromotionDirectSalesOrder.PromotionDirectSalesOrderItemMappings)
                {
                    PromotionDirectSalesOrderItemMappingDAO PromotionDirectSalesOrderItemMappingDAO = new PromotionDirectSalesOrderItemMappingDAO();
                    PromotionDirectSalesOrderItemMappingDAO.PromotionDirectSalesOrderId = PromotionDirectSalesOrder.Id;
                    PromotionDirectSalesOrderItemMappingDAO.ItemId = PromotionDirectSalesOrderItemMapping.ItemId;
                    PromotionDirectSalesOrderItemMappingDAO.Quantity = PromotionDirectSalesOrderItemMapping.Quantity;
                    PromotionDirectSalesOrderItemMappingDAOs.Add(PromotionDirectSalesOrderItemMappingDAO);
                }
                await DataContext.PromotionDirectSalesOrderItemMapping.BulkMergeAsync(PromotionDirectSalesOrderItemMappingDAOs);
            }
        }

    }
}
