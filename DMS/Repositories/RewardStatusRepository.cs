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
    public interface IRewardStatusRepository
    {
        Task<int> Count(RewardStatusFilter RewardStatusFilter);
        Task<int> CountAll(RewardStatusFilter RewardStatusFilter);
        Task<List<RewardStatus>> List(RewardStatusFilter RewardStatusFilter);
        Task<RewardStatus> Get(long Id);
    }
    public class RewardStatusRepository : IRewardStatusRepository
    {
        private DataContext DataContext;
        public RewardStatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<RewardStatusDAO> DynamicFilter(IQueryable<RewardStatusDAO> query, RewardStatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<RewardStatusDAO> OrFilter(IQueryable<RewardStatusDAO> query, RewardStatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<RewardStatusDAO> initQuery = query.Where(q => false);
            foreach (RewardStatusFilter RewardStatusFilter in filter.OrFilter)
            {
                IQueryable<RewardStatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, RewardStatusFilter.Id);
                queryable = queryable.Where(q => q.Code, RewardStatusFilter.Code);
                queryable = queryable.Where(q => q.Name, RewardStatusFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<RewardStatusDAO> DynamicOrder(IQueryable<RewardStatusDAO> query, RewardStatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case RewardStatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case RewardStatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case RewardStatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case RewardStatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case RewardStatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case RewardStatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<RewardStatus>> DynamicSelect(IQueryable<RewardStatusDAO> query, RewardStatusFilter filter)
        {
            List<RewardStatus> RewardStatuses = await query.Select(q => new RewardStatus()
            {
                Id = filter.Selects.Contains(RewardStatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(RewardStatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(RewardStatusSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return RewardStatuses;
        }

        public async Task<int> Count(RewardStatusFilter filter)
        {
            IQueryable<RewardStatusDAO> RewardStatuses = DataContext.RewardStatus.AsNoTracking();
            RewardStatuses = DynamicFilter(RewardStatuses, filter);
            RewardStatuses = OrFilter(RewardStatuses, filter);
            return await RewardStatuses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(RewardStatusFilter filter)
        {
            IQueryable<RewardStatusDAO> RewardStatuses = DataContext.RewardStatus.AsNoTracking();
            RewardStatuses = DynamicFilter(RewardStatuses, filter);
            return await RewardStatuses.CountWithNoLockAsync();
        }

        public async Task<List<RewardStatus>> List(RewardStatusFilter filter)
        {
            if (filter == null) return new List<RewardStatus>();
            IQueryable<RewardStatusDAO> RewardStatusDAOs = DataContext.RewardStatus.AsNoTracking();
            RewardStatusDAOs = DynamicFilter(RewardStatusDAOs, filter);
            RewardStatusDAOs = OrFilter(RewardStatusDAOs, filter);
            RewardStatusDAOs = DynamicOrder(RewardStatusDAOs, filter);
            List<RewardStatus> RewardStatuses = await DynamicSelect(RewardStatusDAOs, filter);
            return RewardStatuses;
        }

        public async Task<RewardStatus> Get(long Id)
        {
            RewardStatus RewardStatus = await DataContext.RewardStatus.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new RewardStatus()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (RewardStatus == null)
                return null;

            return RewardStatus;
        }
    }
}
