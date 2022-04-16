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
    public interface IPromotionCodeHistoryRepository
    {
        Task<int> Count(PromotionCodeHistoryFilter PromotionCodeHistoryFilter);
        Task<int> CountAll(PromotionCodeHistoryFilter PromotionCodeHistoryFilter);
        Task<List<PromotionCodeHistory>> List(PromotionCodeHistoryFilter PromotionCodeHistoryFilter);
        Task<PromotionCodeHistory> Get(long Id);
        Task<bool> Create(PromotionCodeHistory PromotionCodeHistory);
        Task<bool> Update(PromotionCodeHistory PromotionCodeHistory);
        Task<bool> Delete(PromotionCodeHistory PromotionCodeHistory);
        Task<bool> BulkMerge(List<PromotionCodeHistory> PromotionCodeHistories);
        Task<bool> BulkDelete(List<PromotionCodeHistory> PromotionCodeHistories);
    }
    public class PromotionCodeHistoryRepository : IPromotionCodeHistoryRepository
    {
        private DataContext DataContext;
        public PromotionCodeHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionCodeHistoryDAO> DynamicFilter(IQueryable<PromotionCodeHistoryDAO> query, PromotionCodeHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.PromotionCodeId, filter.PromotionCodeId);
            query = query.Where(q => q.AppliedAt, filter.AppliedAt);
            query = query.Where(q => q.RowId, filter.RowId);
            return query;
        }

        private IQueryable<PromotionCodeHistoryDAO> OrFilter(IQueryable<PromotionCodeHistoryDAO> query, PromotionCodeHistoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionCodeHistoryDAO> initQuery = query.Where(q => false);
            foreach (PromotionCodeHistoryFilter PromotionCodeHistoryFilter in filter.OrFilter)
            {
                IQueryable<PromotionCodeHistoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionCodeHistoryFilter.Id);
                queryable = queryable.Where(q => q.PromotionCodeId, PromotionCodeHistoryFilter.PromotionCodeId);
                queryable = queryable.Where(q => q.AppliedAt, PromotionCodeHistoryFilter.AppliedAt);
                queryable = queryable.Where(q => q.RowId, PromotionCodeHistoryFilter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionCodeHistoryDAO> DynamicOrder(IQueryable<PromotionCodeHistoryDAO> query, PromotionCodeHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionCodeHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionCodeHistoryOrder.PromotionCode:
                            query = query.OrderBy(q => q.PromotionCodeId);
                            break;
                        case PromotionCodeHistoryOrder.AppliedAt:
                            query = query.OrderBy(q => q.AppliedAt);
                            break;
                        case PromotionCodeHistoryOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionCodeHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionCodeHistoryOrder.PromotionCode:
                            query = query.OrderByDescending(q => q.PromotionCodeId);
                            break;
                        case PromotionCodeHistoryOrder.AppliedAt:
                            query = query.OrderByDescending(q => q.AppliedAt);
                            break;
                        case PromotionCodeHistoryOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionCodeHistory>> DynamicSelect(IQueryable<PromotionCodeHistoryDAO> query, PromotionCodeHistoryFilter filter)
        {
            List<PromotionCodeHistory> PromotionCodeHistories = await query.Select(q => new PromotionCodeHistory()
            {
                Id = filter.Selects.Contains(PromotionCodeHistorySelect.Id) ? q.Id : default(long),
                PromotionCodeId = filter.Selects.Contains(PromotionCodeHistorySelect.PromotionCode) ? q.PromotionCodeId : default(long),
                AppliedAt = filter.Selects.Contains(PromotionCodeHistorySelect.AppliedAt) ? q.AppliedAt : default(DateTime),
                RowId = filter.Selects.Contains(PromotionCodeHistorySelect.Row) ? q.RowId : default(Guid),
                PromotionCode = filter.Selects.Contains(PromotionCodeHistorySelect.PromotionCode) && q.PromotionCode != null ? new PromotionCode
                {
                    Id = q.PromotionCode.Id,
                    Code = q.PromotionCode.Code,
                    Name = q.PromotionCode.Name,
                    Quantity = q.PromotionCode.Quantity,
                    PromotionDiscountTypeId = q.PromotionCode.PromotionDiscountTypeId,
                    Value = q.PromotionCode.Value,
                    MaxValue = q.PromotionCode.MaxValue,
                    PromotionTypeId = q.PromotionCode.PromotionTypeId,
                    PromotionProductAppliedTypeId = q.PromotionCode.PromotionProductAppliedTypeId,
                    OrganizationId = q.PromotionCode.OrganizationId,
                    StartDate = q.PromotionCode.StartDate,
                    EndDate = q.PromotionCode.EndDate,
                    StatusId = q.PromotionCode.StatusId,
                } : null,
            }).ToListWithNoLockAsync();
            return PromotionCodeHistories;
        }

        public async Task<int> Count(PromotionCodeHistoryFilter filter)
        {
            IQueryable<PromotionCodeHistoryDAO> PromotionCodeHistories = DataContext.PromotionCodeHistory.AsNoTracking();
            PromotionCodeHistories = DynamicFilter(PromotionCodeHistories, filter);
            PromotionCodeHistories = OrFilter(PromotionCodeHistories, filter);
            return await PromotionCodeHistories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionCodeHistoryFilter filter)
        {
            IQueryable<PromotionCodeHistoryDAO> PromotionCodeHistories = DataContext.PromotionCodeHistory.AsNoTracking();
            PromotionCodeHistories = DynamicFilter(PromotionCodeHistories, filter);
            return await PromotionCodeHistories.CountWithNoLockAsync();
        }

        public async Task<List<PromotionCodeHistory>> List(PromotionCodeHistoryFilter filter)
        {
            if (filter == null) return new List<PromotionCodeHistory>();
            IQueryable<PromotionCodeHistoryDAO> PromotionCodeHistoryDAOs = DataContext.PromotionCodeHistory.AsNoTracking();
            PromotionCodeHistoryDAOs = DynamicFilter(PromotionCodeHistoryDAOs, filter);
            PromotionCodeHistoryDAOs = OrFilter(PromotionCodeHistoryDAOs, filter);
            PromotionCodeHistoryDAOs = DynamicOrder(PromotionCodeHistoryDAOs, filter);
            List<PromotionCodeHistory> PromotionCodeHistories = await DynamicSelect(PromotionCodeHistoryDAOs, filter);
            return PromotionCodeHistories;
        }

        public async Task<PromotionCodeHistory> Get(long Id)
        {
            PromotionCodeHistory PromotionCodeHistory = await DataContext.PromotionCodeHistory.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionCodeHistory()
            {
                Id = x.Id,
                PromotionCodeId = x.PromotionCodeId,
                AppliedAt = x.AppliedAt,
                RowId = x.RowId,
                PromotionCode = x.PromotionCode == null ? null : new PromotionCode
                {
                    Id = x.PromotionCode.Id,
                    Code = x.PromotionCode.Code,
                    Name = x.PromotionCode.Name,
                    Quantity = x.PromotionCode.Quantity,
                    PromotionDiscountTypeId = x.PromotionCode.PromotionDiscountTypeId,
                    Value = x.PromotionCode.Value,
                    MaxValue = x.PromotionCode.MaxValue,
                    PromotionTypeId = x.PromotionCode.PromotionTypeId,
                    PromotionProductAppliedTypeId = x.PromotionCode.PromotionProductAppliedTypeId,
                    OrganizationId = x.PromotionCode.OrganizationId,
                    StartDate = x.PromotionCode.StartDate,
                    EndDate = x.PromotionCode.EndDate,
                    StatusId = x.PromotionCode.StatusId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionCodeHistory == null)
                return null;

            return PromotionCodeHistory;
        }
        public async Task<bool> Create(PromotionCodeHistory PromotionCodeHistory)
        {
            PromotionCodeHistoryDAO PromotionCodeHistoryDAO = new PromotionCodeHistoryDAO();
            PromotionCodeHistoryDAO.Id = PromotionCodeHistory.Id;
            PromotionCodeHistoryDAO.PromotionCodeId = PromotionCodeHistory.PromotionCodeId;
            PromotionCodeHistoryDAO.AppliedAt = PromotionCodeHistory.AppliedAt;
            PromotionCodeHistoryDAO.RowId = PromotionCodeHistory.RowId;
            DataContext.PromotionCodeHistory.Add(PromotionCodeHistoryDAO);
            await DataContext.SaveChangesAsync();
            PromotionCodeHistory.Id = PromotionCodeHistoryDAO.Id;
            await SaveReference(PromotionCodeHistory);
            return true;
        }

        public async Task<bool> Update(PromotionCodeHistory PromotionCodeHistory)
        {
            PromotionCodeHistoryDAO PromotionCodeHistoryDAO = DataContext.PromotionCodeHistory.Where(x => x.Id == PromotionCodeHistory.Id).FirstOrDefault();
            if (PromotionCodeHistoryDAO == null)
                return false;
            PromotionCodeHistoryDAO.Id = PromotionCodeHistory.Id;
            PromotionCodeHistoryDAO.PromotionCodeId = PromotionCodeHistory.PromotionCodeId;
            PromotionCodeHistoryDAO.AppliedAt = PromotionCodeHistory.AppliedAt;
            PromotionCodeHistoryDAO.RowId = PromotionCodeHistory.RowId;
            await DataContext.SaveChangesAsync();
            await SaveReference(PromotionCodeHistory);
            return true;
        }

        public async Task<bool> Delete(PromotionCodeHistory PromotionCodeHistory)
        {
            await DataContext.PromotionCodeHistory.Where(x => x.Id == PromotionCodeHistory.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PromotionCodeHistory> PromotionCodeHistories)
        {
            List<PromotionCodeHistoryDAO> PromotionCodeHistoryDAOs = new List<PromotionCodeHistoryDAO>();
            foreach (PromotionCodeHistory PromotionCodeHistory in PromotionCodeHistories)
            {
                PromotionCodeHistoryDAO PromotionCodeHistoryDAO = new PromotionCodeHistoryDAO();
                PromotionCodeHistoryDAO.Id = PromotionCodeHistory.Id;
                PromotionCodeHistoryDAO.PromotionCodeId = PromotionCodeHistory.PromotionCodeId;
                PromotionCodeHistoryDAO.AppliedAt = PromotionCodeHistory.AppliedAt;
                PromotionCodeHistoryDAO.RowId = PromotionCodeHistory.RowId;
                PromotionCodeHistoryDAOs.Add(PromotionCodeHistoryDAO);
            }
            await DataContext.BulkMergeAsync(PromotionCodeHistoryDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PromotionCodeHistory> PromotionCodeHistories)
        {
            List<long> Ids = PromotionCodeHistories.Select(x => x.Id).ToList();
            await DataContext.PromotionCodeHistory
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PromotionCodeHistory PromotionCodeHistory)
        {
        }

    }
}
