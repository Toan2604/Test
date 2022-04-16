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
    public interface IStoreStatusRepository
    {
        Task<int> Count(StoreStatusFilter StoreStatusFilter);
        Task<int> CountAll(StoreStatusFilter StoreStatusFilter);
        Task<List<StoreStatus>> List(StoreStatusFilter StoreStatusFilter);
        Task<List<StoreStatus>> List(List<long> Ids);
        Task<StoreStatus> Get(long Id);
    }
    public class StoreStatusRepository : IStoreStatusRepository
    {
        private DataContext DataContext;
        public StoreStatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreStatusDAO> DynamicFilter(IQueryable<StoreStatusDAO> query, StoreStatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<StoreStatusDAO> OrFilter(IQueryable<StoreStatusDAO> query, StoreStatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreStatusDAO> initQuery = query.Where(q => false);
            foreach (StoreStatusFilter StoreStatusFilter in filter.OrFilter)
            {
                IQueryable<StoreStatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreStatusFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreStatusFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreStatusFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreStatusDAO> DynamicOrder(IQueryable<StoreStatusDAO> query, StoreStatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreStatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreStatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreStatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreStatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreStatus>> DynamicSelect(IQueryable<StoreStatusDAO> query, StoreStatusFilter filter)
        {
            List<StoreStatus> StoreStatuses = await query.Select(q => new StoreStatus()
            {
                Id = filter.Selects.Contains(StoreStatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreStatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreStatusSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return StoreStatuses;
        }

        public async Task<int> Count(StoreStatusFilter filter)
        {
            IQueryable<StoreStatusDAO> StoreStatuses = DataContext.StoreStatus.AsNoTracking();
            StoreStatuses = DynamicFilter(StoreStatuses, filter);
            StoreStatuses = OrFilter(StoreStatuses, filter);
            return await StoreStatuses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreStatusFilter filter)
        {
            IQueryable<StoreStatusDAO> StoreStatuses = DataContext.StoreStatus.AsNoTracking();
            StoreStatuses = DynamicFilter(StoreStatuses, filter);
            return await StoreStatuses.CountWithNoLockAsync();
        }

        public async Task<List<StoreStatus>> List(StoreStatusFilter filter)
        {
            if (filter == null) return new List<StoreStatus>();
            IQueryable<StoreStatusDAO> StoreStatusDAOs = DataContext.StoreStatus.AsNoTracking();
            StoreStatusDAOs = DynamicFilter(StoreStatusDAOs, filter);
            StoreStatusDAOs = OrFilter(StoreStatusDAOs, filter);
            StoreStatusDAOs = DynamicOrder(StoreStatusDAOs, filter);
            List<StoreStatus> StoreStatuses = await DynamicSelect(StoreStatusDAOs, filter);
            return StoreStatuses;
        }

        public async Task<StoreStatus> Get(long Id)
        {
            StoreStatus StoreStatus = await DataContext.StoreStatus.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new StoreStatus()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefaultWithNoLockAsync();

            if (StoreStatus == null)
                return null;

            return StoreStatus;
        }

        public async Task<List<StoreStatus>> List(List<long> Ids)
        {
            List<StoreStatus> StoreStatuses = await DataContext.StoreStatus.AsNoTracking()
               .Where(x => Ids.Contains(x.Id))
               .Select(x => new StoreStatus()
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
               }).ToListWithNoLockAsync();
            return StoreStatuses;
        }
    }
}
