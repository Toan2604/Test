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
    public interface IPriceListItemHistoryRepository
    {
        Task<int> Count(PriceListItemHistoryFilter PriceListItemHistoryFilter);
        Task<int> CountAll(PriceListItemHistoryFilter PriceListItemHistoryFilter);
        Task<List<PriceListItemHistory>> List(PriceListItemHistoryFilter PriceListItemHistoryFilter);
        Task<PriceListItemHistory> Get(long Id);
        Task<bool> Create(PriceListItemHistory PriceListItemHistory);
        Task<bool> Update(PriceListItemHistory PriceListItemHistory);
        Task<bool> Delete(PriceListItemHistory PriceListItemHistory);
        Task<bool> BulkMerge(List<PriceListItemHistory> PriceListItemHistories);
        Task<bool> BulkDelete(List<PriceListItemHistory> PriceListItemHistories);
    }
    public class PriceListItemHistoryRepository : IPriceListItemHistoryRepository
    {
        private DataContext DataContext;
        public PriceListItemHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PriceListItemHistoryDAO> DynamicFilter(IQueryable<PriceListItemHistoryDAO> query, PriceListItemHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            if (filter.UpdatedAt != null)
                query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PriceListId, filter.PriceListId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.OldPrice, filter.OldPrice);
            query = query.Where(q => q.NewPrice, filter.NewPrice);
            query = query.Where(q => q.ModifierId, filter.ModifierId);
            return query;
        }

        private IQueryable<PriceListItemHistoryDAO> OrFilter(IQueryable<PriceListItemHistoryDAO> query, PriceListItemHistoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PriceListItemHistoryDAO> initQuery = query.Where(q => false);
            foreach (PriceListItemHistoryFilter PriceListItemHistoryFilter in filter.OrFilter)
            {
                IQueryable<PriceListItemHistoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PriceListItemHistoryFilter.Id);
                queryable = queryable.Where(q => q.PriceListId, PriceListItemHistoryFilter.PriceListId);
                queryable = queryable.Where(q => q.ItemId, PriceListItemHistoryFilter.ItemId);
                queryable = queryable.Where(q => q.OldPrice, PriceListItemHistoryFilter.OldPrice);
                queryable = queryable.Where(q => q.NewPrice, PriceListItemHistoryFilter.NewPrice);
                queryable = queryable.Where(q => q.ModifierId, PriceListItemHistoryFilter.ModifierId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PriceListItemHistoryDAO> DynamicOrder(IQueryable<PriceListItemHistoryDAO> query, PriceListItemHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PriceListItemHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PriceListItemHistoryOrder.PriceList:
                            query = query.OrderBy(q => q.PriceListId);
                            break;
                        case PriceListItemHistoryOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case PriceListItemHistoryOrder.OldPrice:
                            query = query.OrderBy(q => q.OldPrice);
                            break;
                        case PriceListItemHistoryOrder.NewPrice:
                            query = query.OrderBy(q => q.NewPrice);
                            break;
                        case PriceListItemHistoryOrder.Modifier:
                            query = query.OrderBy(q => q.ModifierId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PriceListItemHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PriceListItemHistoryOrder.PriceList:
                            query = query.OrderByDescending(q => q.PriceListId);
                            break;
                        case PriceListItemHistoryOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case PriceListItemHistoryOrder.OldPrice:
                            query = query.OrderByDescending(q => q.OldPrice);
                            break;
                        case PriceListItemHistoryOrder.NewPrice:
                            query = query.OrderByDescending(q => q.NewPrice);
                            break;
                        case PriceListItemHistoryOrder.Modifier:
                            query = query.OrderByDescending(q => q.ModifierId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PriceListItemHistory>> DynamicSelect(IQueryable<PriceListItemHistoryDAO> query, PriceListItemHistoryFilter filter)
        {
            List<PriceListItemHistory> PriceListItemHistories = await query.Select(q => new PriceListItemHistory()
            {
                Id = filter.Selects.Contains(PriceListItemHistorySelect.Id) ? q.Id : default(long),
                PriceListId = filter.Selects.Contains(PriceListItemHistorySelect.PriceList) ? q.PriceListId : default(long),
                ItemId = filter.Selects.Contains(PriceListItemHistorySelect.Item) ? q.ItemId : default(long),
                OldPrice = filter.Selects.Contains(PriceListItemHistorySelect.OldPrice) ? q.OldPrice : default(decimal),
                NewPrice = filter.Selects.Contains(PriceListItemHistorySelect.NewPrice) ? q.NewPrice : default(decimal),
                ModifierId = filter.Selects.Contains(PriceListItemHistorySelect.Modifier) ? q.ModifierId : default(long),
                Item = filter.Selects.Contains(PriceListItemHistorySelect.Item) && q.Item != null ? new Item
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
                Modifier = filter.Selects.Contains(PriceListItemHistorySelect.Modifier) && q.Modifier != null ? new AppUser
                {
                    Id = q.Modifier.Id,
                    Username = q.Modifier.Username,
                    DisplayName = q.Modifier.DisplayName,
                    Avatar = q.Modifier.Avatar,
                } : null,
                PriceList = filter.Selects.Contains(PriceListItemHistorySelect.PriceList) && q.PriceList != null ? new PriceList
                {
                    Id = q.PriceList.Id,
                    Code = q.PriceList.Code,
                    Name = q.PriceList.Name,
                    StartDate = q.PriceList.StartDate,
                    EndDate = q.PriceList.EndDate,
                    StatusId = q.PriceList.StatusId,
                } : null,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return PriceListItemHistories;
        }

        public async Task<int> Count(PriceListItemHistoryFilter filter)
        {
            IQueryable<PriceListItemHistoryDAO> PriceListItemHistories = DataContext.PriceListItemHistory.AsNoTracking();
            PriceListItemHistories = DynamicFilter(PriceListItemHistories, filter);
            PriceListItemHistories = OrFilter(PriceListItemHistories, filter);
            return await PriceListItemHistories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PriceListItemHistoryFilter filter)
        {
            IQueryable<PriceListItemHistoryDAO> PriceListItemHistories = DataContext.PriceListItemHistory.AsNoTracking();
            PriceListItemHistories = DynamicFilter(PriceListItemHistories, filter);
            return await PriceListItemHistories.CountWithNoLockAsync();
        }

        public async Task<List<PriceListItemHistory>> List(PriceListItemHistoryFilter filter)
        {
            if (filter == null) return new List<PriceListItemHistory>();
            IQueryable<PriceListItemHistoryDAO> PriceListItemHistoryDAOs = DataContext.PriceListItemHistory.AsNoTracking();
            PriceListItemHistoryDAOs = DynamicFilter(PriceListItemHistoryDAOs, filter);
            PriceListItemHistoryDAOs = OrFilter(PriceListItemHistoryDAOs, filter);
            PriceListItemHistoryDAOs = DynamicOrder(PriceListItemHistoryDAOs, filter);
            List<PriceListItemHistory> PriceListItemHistories = await DynamicSelect(PriceListItemHistoryDAOs, filter);
            return PriceListItemHistories;
        }

        public async Task<PriceListItemHistory> Get(long Id)
        {
            PriceListItemHistory PriceListItemHistory = await DataContext.PriceListItemHistory.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PriceListItemHistory()
            {
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                PriceListId = x.PriceListId,
                ItemId = x.ItemId,
                OldPrice = x.OldPrice,
                NewPrice = x.NewPrice,
                ModifierId = x.ModifierId,
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
                    StatusId = x.Modifier.StatusId,
                },
                PriceList = x.PriceList == null ? null : new PriceList
                {
                    Id = x.PriceList.Id,
                    Code = x.PriceList.Code,
                    Name = x.PriceList.Name,
                    StartDate = x.PriceList.StartDate,
                    EndDate = x.PriceList.EndDate,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (PriceListItemHistory == null)
                return null;

            return PriceListItemHistory;
        }
        public async Task<bool> Create(PriceListItemHistory PriceListItemHistory)
        {
            PriceListItemHistoryDAO PriceListItemHistoryDAO = new PriceListItemHistoryDAO();
            PriceListItemHistoryDAO.Id = PriceListItemHistory.Id;
            PriceListItemHistoryDAO.PriceListId = PriceListItemHistory.PriceListId;
            PriceListItemHistoryDAO.ItemId = PriceListItemHistory.ItemId;
            PriceListItemHistoryDAO.OldPrice = PriceListItemHistory.OldPrice;
            PriceListItemHistoryDAO.NewPrice = PriceListItemHistory.NewPrice;
            PriceListItemHistoryDAO.ModifierId = PriceListItemHistory.ModifierId;
            PriceListItemHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.PriceListItemHistory.Add(PriceListItemHistoryDAO);
            await DataContext.SaveChangesAsync();
            PriceListItemHistory.Id = PriceListItemHistoryDAO.Id;
            await SaveReference(PriceListItemHistory);
            return true;
        }

        public async Task<bool> Update(PriceListItemHistory PriceListItemHistory)
        {
            PriceListItemHistoryDAO PriceListItemHistoryDAO = DataContext.PriceListItemHistory.Where(x => x.Id == PriceListItemHistory.Id).FirstOrDefault();
            if (PriceListItemHistoryDAO == null)
                return false;
            PriceListItemHistoryDAO.Id = PriceListItemHistory.Id;
            PriceListItemHistoryDAO.PriceListId = PriceListItemHistory.PriceListId;
            PriceListItemHistoryDAO.ItemId = PriceListItemHistory.ItemId;
            PriceListItemHistoryDAO.OldPrice = PriceListItemHistory.OldPrice;
            PriceListItemHistoryDAO.NewPrice = PriceListItemHistory.NewPrice;
            PriceListItemHistoryDAO.ModifierId = PriceListItemHistory.ModifierId;
            PriceListItemHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(PriceListItemHistory);
            return true;
        }

        public async Task<bool> Delete(PriceListItemHistory PriceListItemHistory)
        {
            await DataContext.PriceListItemHistory.Where(x => x.Id == PriceListItemHistory.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PriceListItemHistory> PriceListItemHistories)
        {
            List<PriceListItemHistoryDAO> PriceListItemHistoryDAOs = new List<PriceListItemHistoryDAO>();
            foreach (PriceListItemHistory PriceListItemHistory in PriceListItemHistories)
            {
                PriceListItemHistoryDAO PriceListItemHistoryDAO = new PriceListItemHistoryDAO();
                PriceListItemHistoryDAO.Id = PriceListItemHistory.Id;
                PriceListItemHistoryDAO.PriceListId = PriceListItemHistory.PriceListId;
                PriceListItemHistoryDAO.ItemId = PriceListItemHistory.ItemId;
                PriceListItemHistoryDAO.OldPrice = PriceListItemHistory.OldPrice;
                PriceListItemHistoryDAO.NewPrice = PriceListItemHistory.NewPrice;
                PriceListItemHistoryDAO.ModifierId = PriceListItemHistory.ModifierId;
                PriceListItemHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
                PriceListItemHistoryDAOs.Add(PriceListItemHistoryDAO);
            }
            await DataContext.BulkMergeAsync(PriceListItemHistoryDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PriceListItemHistory> PriceListItemHistories)
        {
            List<long> Ids = PriceListItemHistories.Select(x => x.Id).ToList();
            await DataContext.PriceListItemHistory
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PriceListItemHistory PriceListItemHistory)
        {
        }

    }
}
