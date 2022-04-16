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
    public interface IItemHistoryRepository
    {
        Task<int> Count(ItemHistoryFilter ItemHistoryFilter);
        Task<int> CountAll(ItemHistoryFilter ItemHistoryFilter);
        Task<List<ItemHistory>> List(ItemHistoryFilter ItemHistoryFilter);
        Task<ItemHistory> Get(long Id);
    }
    public class ItemHistoryRepository : IItemHistoryRepository
    {
        private DataContext DataContext;
        public ItemHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ItemHistoryDAO> DynamicFilter(IQueryable<ItemHistoryDAO> query, ItemHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.Time, filter.Time);
            query = query.Where(q => q.ModifierId, filter.ModifierId);
            query = query.Where(q => q.OldPrice, filter.OldPrice);
            query = query.Where(q => q.NewPrice, filter.NewPrice);
            return query;
        }

        private IQueryable<ItemHistoryDAO> OrFilter(IQueryable<ItemHistoryDAO> query, ItemHistoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ItemHistoryDAO> initQuery = query.Where(q => false);
            foreach (ItemHistoryFilter ItemHistoryFilter in filter.OrFilter)
            {
                IQueryable<ItemHistoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.ItemId, filter.ItemId);
                queryable = queryable.Where(q => q.Time, filter.Time);
                queryable = queryable.Where(q => q.ModifierId, filter.ModifierId);
                queryable = queryable.Where(q => q.OldPrice, filter.OldPrice);
                queryable = queryable.Where(q => q.NewPrice, filter.NewPrice);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ItemHistoryDAO> DynamicOrder(IQueryable<ItemHistoryDAO> query, ItemHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ItemHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ItemHistoryOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case ItemHistoryOrder.Time:
                            query = query.OrderBy(q => q.Time);
                            break;
                        case ItemHistoryOrder.Modifier:
                            query = query.OrderBy(q => q.ModifierId);
                            break;
                        case ItemHistoryOrder.OldPrice:
                            query = query.OrderBy(q => q.OldPrice);
                            break;
                        case ItemHistoryOrder.NewPrice:
                            query = query.OrderBy(q => q.NewPrice);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ItemHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ItemHistoryOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case ItemHistoryOrder.Time:
                            query = query.OrderByDescending(q => q.Time);
                            break;
                        case ItemHistoryOrder.Modifier:
                            query = query.OrderByDescending(q => q.ModifierId);
                            break;
                        case ItemHistoryOrder.OldPrice:
                            query = query.OrderByDescending(q => q.OldPrice);
                            break;
                        case ItemHistoryOrder.NewPrice:
                            query = query.OrderByDescending(q => q.NewPrice);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ItemHistory>> DynamicSelect(IQueryable<ItemHistoryDAO> query, ItemHistoryFilter filter)
        {
            List<ItemHistory> ItemHistories = await query.Select(q => new ItemHistory()
            {
                Id = filter.Selects.Contains(ItemHistorySelect.Id) ? q.Id : default(long),
                ItemId = filter.Selects.Contains(ItemHistorySelect.Item) ? q.ItemId : default(long),
                Time = filter.Selects.Contains(ItemHistorySelect.Time) ? q.Time : default(DateTime),
                ModifierId = filter.Selects.Contains(ItemHistorySelect.Modifier) ? q.ModifierId : default(long),
                OldPrice = filter.Selects.Contains(ItemHistorySelect.OldPrice) ? q.OldPrice : default(long),
                NewPrice = filter.Selects.Contains(ItemHistorySelect.NewPrice) ? q.NewPrice : default(long),
                Item = filter.Selects.Contains(ItemHistorySelect.Item) && q.Item != null ? new Item
                {
                    Id = q.Item.Id,
                    ProductId = q.Item.ProductId,
                    Code = q.Item.Code,
                    Name = q.Item.Name,
                    ScanCode = q.Item.ScanCode,
                    SalePrice = q.Item.SalePrice,
                    RetailPrice = q.Item.RetailPrice,
                    StatusId = q.Item.StatusId,
                    Used = q.Item.Used,
                } : null,
                Modifier = filter.Selects.Contains(ItemHistorySelect.Modifier) && q.Modifier != null ? new AppUser
                {
                    Id = q.Modifier.Id,
                    Username = q.Modifier.Username,
                    DisplayName = q.Modifier.DisplayName,
                    Address = q.Modifier.Address,
                    Email = q.Modifier.Email,
                    Phone = q.Modifier.Phone,
                    PositionId = q.Modifier.PositionId,
                    Department = q.Modifier.Department,
                    OrganizationId = q.Modifier.OrganizationId,
                    StatusId = q.Modifier.StatusId,
                    Avatar = q.Modifier.Avatar,
                    ProvinceId = q.Modifier.ProvinceId,
                    SexId = q.Modifier.SexId,
                    Birthday = q.Modifier.Birthday,
                } : null,
            }).ToListWithNoLockAsync();
            return ItemHistories;
        }

        public async Task<int> Count(ItemHistoryFilter filter)
        {
            IQueryable<ItemHistoryDAO> ItemHistories = DataContext.ItemHistory.AsNoTracking();
            ItemHistories = DynamicFilter(ItemHistories, filter);
            ItemHistories = OrFilter(ItemHistories, filter);
            return await ItemHistories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ItemHistoryFilter filter)
        {
            IQueryable<ItemHistoryDAO> ItemHistories = DataContext.ItemHistory.AsNoTracking();
            ItemHistories = DynamicFilter(ItemHistories, filter);
            return await ItemHistories.CountWithNoLockAsync();
        }

        public async Task<List<ItemHistory>> List(ItemHistoryFilter filter)
        {
            if (filter == null) return new List<ItemHistory>();
            IQueryable<ItemHistoryDAO> ItemHistoryDAOs = DataContext.ItemHistory.AsNoTracking();
            ItemHistoryDAOs = DynamicFilter(ItemHistoryDAOs, filter);
            ItemHistoryDAOs = OrFilter(ItemHistoryDAOs, filter);
            ItemHistoryDAOs = DynamicOrder(ItemHistoryDAOs, filter);
            List<ItemHistory> ItemHistories = await DynamicSelect(ItemHistoryDAOs, filter);
            return ItemHistories;
        }

        public async Task<ItemHistory> Get(long Id)
        {
            ItemHistory ItemHistory = await DataContext.ItemHistory.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ItemHistory()
            {
                Id = x.Id,
                ItemId = x.ItemId,
                Time = x.Time,
                ModifierId = x.ModifierId,
                OldPrice = x.OldPrice,
                NewPrice = x.NewPrice,
                Item = x.Item == null ? null : new Item
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
                Modifier = x.Modifier == null ? null : new AppUser
                {
                    Id = x.Modifier.Id,
                    Username = x.Modifier.Username,
                    DisplayName = x.Modifier.DisplayName,
                    Address = x.Modifier.Address,
                    Email = x.Modifier.Email,
                    Phone = x.Modifier.Phone,
                    PositionId = x.Modifier.PositionId,
                    Department = x.Modifier.Department,
                    OrganizationId = x.Modifier.OrganizationId,
                    StatusId = x.Modifier.StatusId,
                    Avatar = x.Modifier.Avatar,
                    ProvinceId = x.Modifier.ProvinceId,
                    SexId = x.Modifier.SexId,
                    Birthday = x.Modifier.Birthday,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ItemHistory == null)
                return null;

            return ItemHistory;
        }
    }
}
