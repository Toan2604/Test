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
    public interface IShowingOrderContentRepository
    {
        Task<int> Count(ShowingOrderContentFilter ShowingOrderContentFilter);
        Task<int> CountAll(ShowingOrderContentFilter ShowingOrderContentFilter);
        Task<List<ShowingOrderContent>> List(ShowingOrderContentFilter ShowingOrderContentFilter);
        Task<List<ShowingOrderContent>> List(List<long> Ids);
        Task<ShowingOrderContent> Get(long Id);
        Task<bool> Create(ShowingOrderContent ShowingOrderContent);
        Task<bool> Update(ShowingOrderContent ShowingOrderContent);
        Task<bool> Delete(ShowingOrderContent ShowingOrderContent);
        Task<bool> BulkMerge(List<ShowingOrderContent> ShowingOrderContents);
        Task<bool> BulkDelete(List<ShowingOrderContent> ShowingOrderContents);
    }
    public class ShowingOrderContentRepository : IShowingOrderContentRepository
    {
        private DataContext DataContext;
        public ShowingOrderContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ShowingOrderContentDAO> DynamicFilter(IQueryable<ShowingOrderContentDAO> query, ShowingOrderContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ShowingOrderId, filter.ShowingOrderId);
            query = query.Where(q => q.ShowingItemId, filter.ShowingItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.Amount, filter.Amount);
            return query;
        }

        private IQueryable<ShowingOrderContentDAO> OrFilter(IQueryable<ShowingOrderContentDAO> query, ShowingOrderContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ShowingOrderContentDAO> initQuery = query.Where(q => false);
            foreach (ShowingOrderContentFilter ShowingOrderContentFilter in filter.OrFilter)
            {
                IQueryable<ShowingOrderContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.ShowingOrderId, filter.ShowingOrderId);
                queryable = queryable.Where(q => q.ShowingItemId, filter.ShowingItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.SalePrice, filter.SalePrice);
                queryable = queryable.Where(q => q.Quantity, filter.Quantity);
                queryable = queryable.Where(q => q.Amount, filter.Amount);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ShowingOrderContentDAO> DynamicOrder(IQueryable<ShowingOrderContentDAO> query, ShowingOrderContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ShowingOrderContentOrder.ShowingOrder:
                            query = query.OrderBy(q => q.ShowingOrderId);
                            break;
                        case ShowingOrderContentOrder.ShowingItem:
                            query = query.OrderBy(q => q.ShowingItemId);
                            break;
                        case ShowingOrderContentOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case ShowingOrderContentOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case ShowingOrderContentOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case ShowingOrderContentOrder.Amount:
                            query = query.OrderBy(q => q.Amount);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ShowingOrderContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ShowingOrderContentOrder.ShowingOrder:
                            query = query.OrderByDescending(q => q.ShowingOrderId);
                            break;
                        case ShowingOrderContentOrder.ShowingItem:
                            query = query.OrderByDescending(q => q.ShowingItemId);
                            break;
                        case ShowingOrderContentOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case ShowingOrderContentOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case ShowingOrderContentOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case ShowingOrderContentOrder.Amount:
                            query = query.OrderByDescending(q => q.Amount);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ShowingOrderContent>> DynamicSelect(IQueryable<ShowingOrderContentDAO> query, ShowingOrderContentFilter filter)
        {
            List<ShowingOrderContent> ShowingOrderContents = await query.Select(q => new ShowingOrderContent()
            {
                Id = filter.Selects.Contains(ShowingOrderContentSelect.Id) ? q.Id : default(long),
                ShowingOrderId = filter.Selects.Contains(ShowingOrderContentSelect.ShowingOrder) ? q.ShowingOrderId : default(long),
                ShowingItemId = filter.Selects.Contains(ShowingOrderContentSelect.ShowingItem) ? q.ShowingItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(ShowingOrderContentSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                SalePrice = filter.Selects.Contains(ShowingOrderContentSelect.SalePrice) ? q.SalePrice : default(decimal),
                Quantity = filter.Selects.Contains(ShowingOrderContentSelect.Quantity) ? q.Quantity : default(long),
                Amount = filter.Selects.Contains(ShowingOrderContentSelect.Amount) ? q.Amount : default(decimal),
                ShowingItem = filter.Selects.Contains(ShowingOrderContentSelect.ShowingItem) && q.ShowingItem != null ? new ShowingItem
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
                ShowingOrder = filter.Selects.Contains(ShowingOrderContentSelect.ShowingOrder) && q.ShowingOrder != null ? new ShowingOrder
                {
                    Id = q.ShowingOrder.Id,
                    Code = q.ShowingOrder.Code,
                    AppUserId = q.ShowingOrder.AppUserId,
                    OrganizationId = q.ShowingOrder.OrganizationId,
                    StoreId = q.ShowingOrder.StoreId,
                    Date = q.ShowingOrder.Date,
                    StatusId = q.ShowingOrder.StatusId,
                    Total = q.ShowingOrder.Total,
                    RowId = q.ShowingOrder.RowId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(ShowingOrderContentSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
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
            return ShowingOrderContents;
        }

        public async Task<int> Count(ShowingOrderContentFilter filter)
        {
            IQueryable<ShowingOrderContentDAO> ShowingOrderContents = DataContext.ShowingOrderContent.AsNoTracking();
            ShowingOrderContents = DynamicFilter(ShowingOrderContents, filter);
            ShowingOrderContents = OrFilter(ShowingOrderContents, filter);
            return await ShowingOrderContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ShowingOrderContentFilter filter)
        {
            IQueryable<ShowingOrderContentDAO> ShowingOrderContents = DataContext.ShowingOrderContent.AsNoTracking();
            ShowingOrderContents = DynamicFilter(ShowingOrderContents, filter);
            return await ShowingOrderContents.CountWithNoLockAsync();
        }

        public async Task<List<ShowingOrderContent>> List(ShowingOrderContentFilter filter)
        {
            if (filter == null) return new List<ShowingOrderContent>();
            IQueryable<ShowingOrderContentDAO> ShowingOrderContentDAOs = DataContext.ShowingOrderContent.AsNoTracking();
            ShowingOrderContentDAOs = DynamicFilter(ShowingOrderContentDAOs, filter);
            ShowingOrderContentDAOs = OrFilter(ShowingOrderContentDAOs, filter);
            ShowingOrderContentDAOs = DynamicOrder(ShowingOrderContentDAOs, filter);
            List<ShowingOrderContent> ShowingOrderContents = await DynamicSelect(ShowingOrderContentDAOs, filter);
            return ShowingOrderContents;
        }

        public async Task<List<ShowingOrderContent>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<ShowingOrderContent> ShowingOrderContents = await DataContext.ShowingOrderContent.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new ShowingOrderContent()
            {
                Id = x.Id,
                ShowingOrderId = x.ShowingOrderId,
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
                ShowingOrder = x.ShowingOrder == null ? null : new ShowingOrder
                {
                    Id = x.ShowingOrder.Id,
                    Code = x.ShowingOrder.Code,
                    AppUserId = x.ShowingOrder.AppUserId,
                    OrganizationId = x.ShowingOrder.OrganizationId,
                    StoreId = x.ShowingOrder.StoreId,
                    Date = x.ShowingOrder.Date,
                    StatusId = x.ShowingOrder.StatusId,
                    Total = x.ShowingOrder.Total,
                    RowId = x.ShowingOrder.RowId,
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


            return ShowingOrderContents;
        }

        public async Task<ShowingOrderContent> Get(long Id)
        {
            ShowingOrderContent ShowingOrderContent = await DataContext.ShowingOrderContent.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new ShowingOrderContent()
            {
                Id = x.Id,
                ShowingOrderId = x.ShowingOrderId,
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
                ShowingOrder = x.ShowingOrder == null ? null : new ShowingOrder
                {
                    Id = x.ShowingOrder.Id,
                    Code = x.ShowingOrder.Code,
                    AppUserId = x.ShowingOrder.AppUserId,
                    OrganizationId = x.ShowingOrder.OrganizationId,
                    StoreId = x.ShowingOrder.StoreId,
                    Date = x.ShowingOrder.Date,
                    StatusId = x.ShowingOrder.StatusId,
                    Total = x.ShowingOrder.Total,
                    RowId = x.ShowingOrder.RowId,
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

            if (ShowingOrderContent == null)
                return null;

            return ShowingOrderContent;
        }
        public async Task<bool> Create(ShowingOrderContent ShowingOrderContent)
        {
            ShowingOrderContentDAO ShowingOrderContentDAO = new ShowingOrderContentDAO();
            ShowingOrderContentDAO.Id = ShowingOrderContent.Id;
            ShowingOrderContentDAO.ShowingOrderId = ShowingOrderContent.ShowingOrderId;
            ShowingOrderContentDAO.ShowingItemId = ShowingOrderContent.ShowingItemId;
            ShowingOrderContentDAO.UnitOfMeasureId = ShowingOrderContent.UnitOfMeasureId;
            ShowingOrderContentDAO.SalePrice = ShowingOrderContent.SalePrice;
            ShowingOrderContentDAO.Quantity = ShowingOrderContent.Quantity;
            ShowingOrderContentDAO.Amount = ShowingOrderContent.Amount;
            DataContext.ShowingOrderContent.Add(ShowingOrderContentDAO);
            await DataContext.SaveChangesAsync();
            ShowingOrderContent.Id = ShowingOrderContentDAO.Id;
            await SaveReference(ShowingOrderContent);
            return true;
        }

        public async Task<bool> Update(ShowingOrderContent ShowingOrderContent)
        {
            ShowingOrderContentDAO ShowingOrderContentDAO = DataContext.ShowingOrderContent.Where(x => x.Id == ShowingOrderContent.Id).FirstOrDefault();
            if (ShowingOrderContentDAO == null)
                return false;
            ShowingOrderContentDAO.Id = ShowingOrderContent.Id;
            ShowingOrderContentDAO.ShowingOrderId = ShowingOrderContent.ShowingOrderId;
            ShowingOrderContentDAO.ShowingItemId = ShowingOrderContent.ShowingItemId;
            ShowingOrderContentDAO.UnitOfMeasureId = ShowingOrderContent.UnitOfMeasureId;
            ShowingOrderContentDAO.SalePrice = ShowingOrderContent.SalePrice;
            ShowingOrderContentDAO.Quantity = ShowingOrderContent.Quantity;
            ShowingOrderContentDAO.Amount = ShowingOrderContent.Amount;
            await DataContext.SaveChangesAsync();
            await SaveReference(ShowingOrderContent);
            return true;
        }

        public async Task<bool> Delete(ShowingOrderContent ShowingOrderContent)
        {
            await DataContext.ShowingOrderContent.Where(x => x.Id == ShowingOrderContent.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<ShowingOrderContent> ShowingOrderContents)
        {
            List<ShowingOrderContentDAO> ShowingOrderContentDAOs = new List<ShowingOrderContentDAO>();
            foreach (ShowingOrderContent ShowingOrderContent in ShowingOrderContents)
            {
                ShowingOrderContentDAO ShowingOrderContentDAO = new ShowingOrderContentDAO();
                ShowingOrderContentDAO.Id = ShowingOrderContent.Id;
                ShowingOrderContentDAO.ShowingOrderId = ShowingOrderContent.ShowingOrderId;
                ShowingOrderContentDAO.ShowingItemId = ShowingOrderContent.ShowingItemId;
                ShowingOrderContentDAO.UnitOfMeasureId = ShowingOrderContent.UnitOfMeasureId;
                ShowingOrderContentDAO.SalePrice = ShowingOrderContent.SalePrice;
                ShowingOrderContentDAO.Quantity = ShowingOrderContent.Quantity;
                ShowingOrderContentDAO.Amount = ShowingOrderContent.Amount;
                ShowingOrderContentDAOs.Add(ShowingOrderContentDAO);
            }
            await DataContext.BulkMergeAsync(ShowingOrderContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ShowingOrderContent> ShowingOrderContents)
        {
            List<long> Ids = ShowingOrderContents.Select(x => x.Id).ToList();
            await DataContext.ShowingOrderContent
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(ShowingOrderContent ShowingOrderContent)
        {
        }

    }
}
