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
    public interface IShowingOrderContentWithDrawRepository
    {
        Task<int> Count(ShowingOrderContentWithDrawFilter ShowingOrderContentWithDrawFilter);
        Task<int> CountAll(ShowingOrderContentWithDrawFilter ShowingOrderContentWithDrawFilter);
        Task<List<ShowingOrderContentWithDraw>> List(ShowingOrderContentWithDrawFilter ShowingOrderContentWithDrawFilter);
        Task<List<ShowingOrderContentWithDraw>> List(List<long> Ids);
        Task<ShowingOrderContentWithDraw> Get(long Id);
        Task<bool> Create(ShowingOrderContentWithDraw ShowingOrderContentWithDraw);
        Task<bool> Update(ShowingOrderContentWithDraw ShowingOrderContentWithDraw);
        Task<bool> Delete(ShowingOrderContentWithDraw ShowingOrderContentWithDraw);
        Task<bool> BulkMerge(List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws);
        Task<bool> BulkDelete(List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws);
    }
    public class ShowingOrderContentWithDrawRepository : IShowingOrderContentWithDrawRepository
    {
        private DataContext DataContext;
        public ShowingOrderContentWithDrawRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ShowingOrderContentWithDrawDAO> DynamicFilter(IQueryable<ShowingOrderContentWithDrawDAO> query, ShowingOrderContentWithDrawFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ShowingOrderWithDrawId, filter.ShowingOrderWithDrawId);
            query = query.Where(q => q.ShowingItemId, filter.ShowingItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.Amount, filter.Amount);
            return query;
        }

        private IQueryable<ShowingOrderContentWithDrawDAO> OrFilter(IQueryable<ShowingOrderContentWithDrawDAO> query, ShowingOrderContentWithDrawFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ShowingOrderContentWithDrawDAO> initQuery = query.Where(q => false);
            foreach (ShowingOrderContentWithDrawFilter ShowingOrderContentWithDrawFilter in filter.OrFilter)
            {
                IQueryable<ShowingOrderContentWithDrawDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.ShowingOrderWithDrawId, filter.ShowingOrderWithDrawId);
                queryable = queryable.Where(q => q.ShowingItemId, filter.ShowingItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.SalePrice, filter.SalePrice);
                queryable = queryable.Where(q => q.Quantity, filter.Quantity);
                queryable = queryable.Where(q => q.Amount, filter.Amount);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ShowingOrderContentWithDrawDAO> DynamicOrder(IQueryable<ShowingOrderContentWithDrawDAO> query, ShowingOrderContentWithDrawFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderContentWithDrawOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ShowingOrderContentWithDrawOrder.ShowingOrderWithDraw:
                            query = query.OrderBy(q => q.ShowingOrderWithDrawId);
                            break;
                        case ShowingOrderContentWithDrawOrder.ShowingItem:
                            query = query.OrderBy(q => q.ShowingItemId);
                            break;
                        case ShowingOrderContentWithDrawOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case ShowingOrderContentWithDrawOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ShowingOrderContentWithDrawOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case ShowingOrderContentWithDrawOrder.Amount:
                            query = query.OrderBy(q => q.Amount);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderContentWithDrawOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ShowingOrderContentWithDrawOrder.ShowingOrderWithDraw:
                            query = query.OrderByDescending(q => q.ShowingOrderWithDrawId);
                            break;
                        case ShowingOrderContentWithDrawOrder.ShowingItem:
                            query = query.OrderByDescending(q => q.ShowingItemId);
                            break;
                        case ShowingOrderContentWithDrawOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case ShowingOrderContentWithDrawOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ShowingOrderContentWithDrawOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case ShowingOrderContentWithDrawOrder.Amount:
                            query = query.OrderByDescending(q => q.Amount);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ShowingOrderContentWithDraw>> DynamicSelect(IQueryable<ShowingOrderContentWithDrawDAO> query, ShowingOrderContentWithDrawFilter filter)
        {
            List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws = await query.Select(q => new ShowingOrderContentWithDraw()
            {
                Id = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.Id) ? q.Id : default(long),
                ShowingOrderWithDrawId = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.ShowingOrderWithDraw) ? q.ShowingOrderWithDrawId : default(long),
                ShowingItemId = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.ShowingItem) ? q.ShowingItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                SalePrice = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.SalePrice) ? q.SalePrice : default(decimal),
                Quantity = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.Quantity) ? q.Quantity : default(long),
                Amount = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.Amount) ? q.Amount : default(decimal),
                ShowingItem = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.ShowingItem) && q.ShowingItem != null ? new ShowingItem
                {
                    Id = q.ShowingItem.Id,
                    Code = q.ShowingItem.Code,
                    Name = q.ShowingItem.Name,
                    ShowingCategoryId = q.ShowingItem.ShowingCategoryId,
                    UnitOfMeasureId = q.ShowingItem.UnitOfMeasureId,
                    SalePrice = q.ShowingItem.SalePrice,
                    Description = q.ShowingItem.Description,
                    StatusId = q.ShowingItem.StatusId,
                    Used = q.ShowingItem.Used,
                    RowId = q.ShowingItem.RowId,
                } : null,
                ShowingOrderWithDraw = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.ShowingOrderWithDraw) && q.ShowingOrderWithDraw != null ? new ShowingOrderWithDraw
                {
                    Id = q.ShowingOrderWithDraw.Id,
                    Code = q.ShowingOrderWithDraw.Code,
                    AppUserId = q.ShowingOrderWithDraw.AppUserId,
                    OrganizationId = q.ShowingOrderWithDraw.OrganizationId,
                    StoreId = q.ShowingOrderWithDraw.StoreId,
                    Date = q.ShowingOrderWithDraw.Date,
                    StatusId = q.ShowingOrderWithDraw.StatusId,
                    Total = q.ShowingOrderWithDraw.Total,
                    RowId = q.ShowingOrderWithDraw.RowId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(ShowingOrderContentWithDrawSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    Used = q.UnitOfMeasure.Used,
                    RowId = q.UnitOfMeasure.RowId,
                } : null,
            }).ToListWithNoLockAsync();
            return ShowingOrderContentWithDraws;
        }

        public async Task<int> Count(ShowingOrderContentWithDrawFilter filter)
        {
            IQueryable<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDraws = DataContext.ShowingOrderContentWithDraw.AsNoTracking();
            ShowingOrderContentWithDraws = DynamicFilter(ShowingOrderContentWithDraws, filter);
            ShowingOrderContentWithDraws = OrFilter(ShowingOrderContentWithDraws, filter);
            return await ShowingOrderContentWithDraws.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ShowingOrderContentWithDrawFilter filter)
        {
            IQueryable<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDraws = DataContext.ShowingOrderContentWithDraw.AsNoTracking();
            ShowingOrderContentWithDraws = DynamicFilter(ShowingOrderContentWithDraws, filter);
            return await ShowingOrderContentWithDraws.CountWithNoLockAsync();
        }

        public async Task<List<ShowingOrderContentWithDraw>> List(ShowingOrderContentWithDrawFilter filter)
        {
            if (filter == null) return new List<ShowingOrderContentWithDraw>();
            IQueryable<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDrawDAOs = DataContext.ShowingOrderContentWithDraw.AsNoTracking();
            ShowingOrderContentWithDrawDAOs = DynamicFilter(ShowingOrderContentWithDrawDAOs, filter);
            ShowingOrderContentWithDrawDAOs = OrFilter(ShowingOrderContentWithDrawDAOs, filter);
            ShowingOrderContentWithDrawDAOs = DynamicOrder(ShowingOrderContentWithDrawDAOs, filter);
            List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws = await DynamicSelect(ShowingOrderContentWithDrawDAOs, filter);
            return ShowingOrderContentWithDraws;
        }

        public async Task<List<ShowingOrderContentWithDraw>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws = await DataContext.ShowingOrderContentWithDraw.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new ShowingOrderContentWithDraw()
            {
                Id = x.Id,
                ShowingOrderWithDrawId = x.ShowingOrderWithDrawId,
                ShowingItemId = x.ShowingItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                SalePrice = x.SalePrice,
                Quantity = x.Quantity,
                Amount = x.Amount,
                ShowingItem = x.ShowingItem == null ? null : new ShowingItem
                {
                    Id = x.ShowingItem.Id,
                    Code = x.ShowingItem.Code,
                    Name = x.ShowingItem.Name,
                    ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                    UnitOfMeasureId = x.ShowingItem.UnitOfMeasureId,
                    SalePrice = x.ShowingItem.SalePrice,
                    Description = x.ShowingItem.Description,
                    StatusId = x.ShowingItem.StatusId,
                    Used = x.ShowingItem.Used,
                    RowId = x.ShowingItem.RowId,
                },
                ShowingOrderWithDraw = x.ShowingOrderWithDraw == null ? null : new ShowingOrderWithDraw
                {
                    Id = x.ShowingOrderWithDraw.Id,
                    Code = x.ShowingOrderWithDraw.Code,
                    AppUserId = x.ShowingOrderWithDraw.AppUserId,
                    OrganizationId = x.ShowingOrderWithDraw.OrganizationId,
                    StoreId = x.ShowingOrderWithDraw.StoreId,
                    Date = x.ShowingOrderWithDraw.Date,
                    StatusId = x.ShowingOrderWithDraw.StatusId,
                    Total = x.ShowingOrderWithDraw.Total,
                    RowId = x.ShowingOrderWithDraw.RowId,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    Used = x.UnitOfMeasure.Used,
                    RowId = x.UnitOfMeasure.RowId,
                },
            }).ToListWithNoLockAsync();


            return ShowingOrderContentWithDraws;
        }

        public async Task<ShowingOrderContentWithDraw> Get(long Id)
        {
            ShowingOrderContentWithDraw ShowingOrderContentWithDraw = await DataContext.ShowingOrderContentWithDraw.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new ShowingOrderContentWithDraw()
            {
                Id = x.Id,
                ShowingOrderWithDrawId = x.ShowingOrderWithDrawId,
                ShowingItemId = x.ShowingItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                SalePrice = x.SalePrice,
                Quantity = x.Quantity,
                Amount = x.Amount,
                ShowingItem = x.ShowingItem == null ? null : new ShowingItem
                {
                    Id = x.ShowingItem.Id,
                    Code = x.ShowingItem.Code,
                    Name = x.ShowingItem.Name,
                    ShowingCategoryId = x.ShowingItem.ShowingCategoryId,
                    UnitOfMeasureId = x.ShowingItem.UnitOfMeasureId,
                    SalePrice = x.ShowingItem.SalePrice,
                    Description = x.ShowingItem.Description,
                    StatusId = x.ShowingItem.StatusId,
                    Used = x.ShowingItem.Used,
                    RowId = x.ShowingItem.RowId,
                },
                ShowingOrderWithDraw = x.ShowingOrderWithDraw == null ? null : new ShowingOrderWithDraw
                {
                    Id = x.ShowingOrderWithDraw.Id,
                    Code = x.ShowingOrderWithDraw.Code,
                    AppUserId = x.ShowingOrderWithDraw.AppUserId,
                    OrganizationId = x.ShowingOrderWithDraw.OrganizationId,
                    StoreId = x.ShowingOrderWithDraw.StoreId,
                    Date = x.ShowingOrderWithDraw.Date,
                    StatusId = x.ShowingOrderWithDraw.StatusId,
                    Total = x.ShowingOrderWithDraw.Total,
                    RowId = x.ShowingOrderWithDraw.RowId,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    Used = x.UnitOfMeasure.Used,
                    RowId = x.UnitOfMeasure.RowId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ShowingOrderContentWithDraw == null)
                return null;

            return ShowingOrderContentWithDraw;
        }
        public async Task<bool> Create(ShowingOrderContentWithDraw ShowingOrderContentWithDraw)
        {
            ShowingOrderContentWithDrawDAO ShowingOrderContentWithDrawDAO = new ShowingOrderContentWithDrawDAO();
            ShowingOrderContentWithDrawDAO.Id = ShowingOrderContentWithDraw.Id;
            ShowingOrderContentWithDrawDAO.ShowingOrderWithDrawId = ShowingOrderContentWithDraw.ShowingOrderWithDrawId;
            ShowingOrderContentWithDrawDAO.ShowingItemId = ShowingOrderContentWithDraw.ShowingItemId;
            ShowingOrderContentWithDrawDAO.UnitOfMeasureId = ShowingOrderContentWithDraw.UnitOfMeasureId;
            ShowingOrderContentWithDrawDAO.SalePrice = ShowingOrderContentWithDraw.SalePrice;
            ShowingOrderContentWithDrawDAO.Quantity = ShowingOrderContentWithDraw.Quantity;
            ShowingOrderContentWithDrawDAO.Amount = ShowingOrderContentWithDraw.Amount;
            DataContext.ShowingOrderContentWithDraw.Add(ShowingOrderContentWithDrawDAO);
            await DataContext.SaveChangesAsync();
            ShowingOrderContentWithDraw.Id = ShowingOrderContentWithDrawDAO.Id;
            await SaveReference(ShowingOrderContentWithDraw);
            return true;
        }

        public async Task<bool> Update(ShowingOrderContentWithDraw ShowingOrderContentWithDraw)
        {
            ShowingOrderContentWithDrawDAO ShowingOrderContentWithDrawDAO = DataContext.ShowingOrderContentWithDraw.Where(x => x.Id == ShowingOrderContentWithDraw.Id).FirstOrDefault();
            if (ShowingOrderContentWithDrawDAO == null)
                return false;
            ShowingOrderContentWithDrawDAO.Id = ShowingOrderContentWithDraw.Id;
            ShowingOrderContentWithDrawDAO.ShowingOrderWithDrawId = ShowingOrderContentWithDraw.ShowingOrderWithDrawId;
            ShowingOrderContentWithDrawDAO.ShowingItemId = ShowingOrderContentWithDraw.ShowingItemId;
            ShowingOrderContentWithDrawDAO.UnitOfMeasureId = ShowingOrderContentWithDraw.UnitOfMeasureId;
            ShowingOrderContentWithDrawDAO.SalePrice = ShowingOrderContentWithDraw.SalePrice;
            ShowingOrderContentWithDrawDAO.Quantity = ShowingOrderContentWithDraw.Quantity;
            ShowingOrderContentWithDrawDAO.Amount = ShowingOrderContentWithDraw.Amount;
            await DataContext.SaveChangesAsync();
            await SaveReference(ShowingOrderContentWithDraw);
            return true;
        }

        public async Task<bool> Delete(ShowingOrderContentWithDraw ShowingOrderContentWithDraw)
        {
            await DataContext.ShowingOrderContentWithDraw.Where(x => x.Id == ShowingOrderContentWithDraw.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws)
        {
            List<ShowingOrderContentWithDrawDAO> ShowingOrderContentWithDrawDAOs = new List<ShowingOrderContentWithDrawDAO>();
            foreach (ShowingOrderContentWithDraw ShowingOrderContentWithDraw in ShowingOrderContentWithDraws)
            {
                ShowingOrderContentWithDrawDAO ShowingOrderContentWithDrawDAO = new ShowingOrderContentWithDrawDAO();
                ShowingOrderContentWithDrawDAO.Id = ShowingOrderContentWithDraw.Id;
                ShowingOrderContentWithDrawDAO.ShowingOrderWithDrawId = ShowingOrderContentWithDraw.ShowingOrderWithDrawId;
                ShowingOrderContentWithDrawDAO.ShowingItemId = ShowingOrderContentWithDraw.ShowingItemId;
                ShowingOrderContentWithDrawDAO.UnitOfMeasureId = ShowingOrderContentWithDraw.UnitOfMeasureId;
                ShowingOrderContentWithDrawDAO.SalePrice = ShowingOrderContentWithDraw.SalePrice;
                ShowingOrderContentWithDrawDAO.Quantity = ShowingOrderContentWithDraw.Quantity;
                ShowingOrderContentWithDrawDAO.Amount = ShowingOrderContentWithDraw.Amount;
                ShowingOrderContentWithDrawDAOs.Add(ShowingOrderContentWithDrawDAO);
            }
            await DataContext.BulkMergeAsync(ShowingOrderContentWithDrawDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws)
        {
            List<long> Ids = ShowingOrderContentWithDraws.Select(x => x.Id).ToList();
            await DataContext.ShowingOrderContentWithDraw
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(ShowingOrderContentWithDraw ShowingOrderContentWithDraw)
        {
        }

    }
}
