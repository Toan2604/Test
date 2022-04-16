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
    public interface IStoreScoutingStatusRepository
    {
        Task<int> Count(StoreScoutingStatusFilter StoreScoutingStatusFilter);
        Task<int> CountAll(StoreScoutingStatusFilter StoreScoutingStatusFilter);
        Task<List<StoreScoutingStatus>> List(StoreScoutingStatusFilter StoreScoutingStatusFilter);
        Task<StoreScoutingStatus> Get(long Id);
        Task<bool> Create(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> Update(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> Delete(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> BulkMerge(List<StoreScoutingStatus> StoreScoutingStatuses);
        Task<bool> BulkDelete(List<StoreScoutingStatus> StoreScoutingStatuses);
    }
    public class StoreScoutingStatusRepository : IStoreScoutingStatusRepository
    {
        private DataContext DataContext;
        public StoreScoutingStatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreScoutingStatusDAO> DynamicFilter(IQueryable<StoreScoutingStatusDAO> query, StoreScoutingStatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<StoreScoutingStatusDAO> OrFilter(IQueryable<StoreScoutingStatusDAO> query, StoreScoutingStatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreScoutingStatusDAO> initQuery = query.Where(q => false);
            foreach (StoreScoutingStatusFilter StoreScoutingStatusFilter in filter.OrFilter)
            {
                IQueryable<StoreScoutingStatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreScoutingStatusDAO> DynamicOrder(IQueryable<StoreScoutingStatusDAO> query, StoreScoutingStatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreScoutingStatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreScoutingStatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreScoutingStatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreScoutingStatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreScoutingStatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreScoutingStatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreScoutingStatus>> DynamicSelect(IQueryable<StoreScoutingStatusDAO> query, StoreScoutingStatusFilter filter)
        {
            List<StoreScoutingStatus> StoreScoutingStatuses = await query.Select(q => new StoreScoutingStatus()
            {
                Id = filter.Selects.Contains(StoreScoutingStatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreScoutingStatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreScoutingStatusSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return StoreScoutingStatuses;
        }

        public async Task<int> Count(StoreScoutingStatusFilter filter)
        {
            IQueryable<StoreScoutingStatusDAO> StoreScoutingStatuses = DataContext.StoreScoutingStatus.AsNoTracking();
            StoreScoutingStatuses = DynamicFilter(StoreScoutingStatuses, filter);
            StoreScoutingStatuses = OrFilter(StoreScoutingStatuses, filter);
            return await StoreScoutingStatuses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreScoutingStatusFilter filter)
        {
            IQueryable<StoreScoutingStatusDAO> StoreScoutingStatuses = DataContext.StoreScoutingStatus.AsNoTracking();
            StoreScoutingStatuses = DynamicFilter(StoreScoutingStatuses, filter);
            return await StoreScoutingStatuses.CountWithNoLockAsync();
        }
        public async Task<List<StoreScoutingStatus>> List(StoreScoutingStatusFilter filter)
        {
            if (filter == null) return new List<StoreScoutingStatus>();
            IQueryable<StoreScoutingStatusDAO> StoreScoutingStatusDAOs = DataContext.StoreScoutingStatus.AsNoTracking();
            StoreScoutingStatusDAOs = DynamicFilter(StoreScoutingStatusDAOs, filter);
            StoreScoutingStatusDAOs = OrFilter(StoreScoutingStatusDAOs, filter);
            StoreScoutingStatusDAOs = DynamicOrder(StoreScoutingStatusDAOs, filter);
            List<StoreScoutingStatus> StoreScoutingStatuses = await DynamicSelect(StoreScoutingStatusDAOs, filter);
            return StoreScoutingStatuses;
        }

        public async Task<StoreScoutingStatus> Get(long Id)
        {
            StoreScoutingStatus StoreScoutingStatus = await DataContext.StoreScoutingStatus.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new StoreScoutingStatus()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (StoreScoutingStatus == null)
                return null;

            return StoreScoutingStatus;
        }
        public async Task<bool> Create(StoreScoutingStatus StoreScoutingStatus)
        {
            StoreScoutingStatusDAO StoreScoutingStatusDAO = new StoreScoutingStatusDAO();
            StoreScoutingStatusDAO.Id = StoreScoutingStatus.Id;
            StoreScoutingStatusDAO.Code = StoreScoutingStatus.Code;
            StoreScoutingStatusDAO.Name = StoreScoutingStatus.Name;
            DataContext.StoreScoutingStatus.Add(StoreScoutingStatusDAO);
            await DataContext.SaveChangesAsync();
            StoreScoutingStatus.Id = StoreScoutingStatusDAO.Id;
            await SaveReference(StoreScoutingStatus);
            return true;
        }

        public async Task<bool> Update(StoreScoutingStatus StoreScoutingStatus)
        {
            StoreScoutingStatusDAO StoreScoutingStatusDAO = DataContext.StoreScoutingStatus.Where(x => x.Id == StoreScoutingStatus.Id).FirstOrDefault();
            if (StoreScoutingStatusDAO == null)
                return false;
            StoreScoutingStatusDAO.Id = StoreScoutingStatus.Id;
            StoreScoutingStatusDAO.Code = StoreScoutingStatus.Code;
            StoreScoutingStatusDAO.Name = StoreScoutingStatus.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(StoreScoutingStatus);
            return true;
        }

        public async Task<bool> Delete(StoreScoutingStatus StoreScoutingStatus)
        {
            await DataContext.StoreScoutingStatus.Where(x => x.Id == StoreScoutingStatus.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreScoutingStatus> StoreScoutingStatuses)
        {
            List<StoreScoutingStatusDAO> StoreScoutingStatusDAOs = new List<StoreScoutingStatusDAO>();
            foreach (StoreScoutingStatus StoreScoutingStatus in StoreScoutingStatuses)
            {
                StoreScoutingStatusDAO StoreScoutingStatusDAO = new StoreScoutingStatusDAO();
                StoreScoutingStatusDAO.Id = StoreScoutingStatus.Id;
                StoreScoutingStatusDAO.Code = StoreScoutingStatus.Code;
                StoreScoutingStatusDAO.Name = StoreScoutingStatus.Name;
                StoreScoutingStatusDAOs.Add(StoreScoutingStatusDAO);
            }
            await DataContext.BulkMergeAsync(StoreScoutingStatusDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreScoutingStatus> StoreScoutingStatuses)
        {
            List<long> Ids = StoreScoutingStatuses.Select(x => x.Id).ToList();
            await DataContext.StoreScoutingStatus
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(StoreScoutingStatus StoreScoutingStatus)
        {
        }

    }
}
