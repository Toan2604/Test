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
    public interface IStoreStatusHistoryTypeRepository
    {
        Task<int> Count(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter);
        Task<int> CountAll(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter);
        Task<List<StoreStatusHistoryType>> List(StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter);
        Task<StoreStatusHistoryType> Get(long Id);
    }
    public class StoreStatusHistoryTypeRepository : IStoreStatusHistoryTypeRepository
    {
        private DataContext DataContext;
        public StoreStatusHistoryTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreStatusHistoryTypeDAO> DynamicFilter(IQueryable<StoreStatusHistoryTypeDAO> query, StoreStatusHistoryTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<StoreStatusHistoryTypeDAO> OrFilter(IQueryable<StoreStatusHistoryTypeDAO> query, StoreStatusHistoryTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreStatusHistoryTypeDAO> initQuery = query.Where(q => false);
            foreach (StoreStatusHistoryTypeFilter StoreStatusHistoryTypeFilter in filter.OrFilter)
            {
                IQueryable<StoreStatusHistoryTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreStatusHistoryTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreStatusHistoryTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreStatusHistoryTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreStatusHistoryTypeDAO> DynamicOrder(IQueryable<StoreStatusHistoryTypeDAO> query, StoreStatusHistoryTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusHistoryTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreStatusHistoryTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreStatusHistoryTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusHistoryTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreStatusHistoryTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreStatusHistoryTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreStatusHistoryType>> DynamicSelect(IQueryable<StoreStatusHistoryTypeDAO> query, StoreStatusHistoryTypeFilter filter)
        {
            List<StoreStatusHistoryType> StoreStatusHistoryTypees = await query.Select(q => new StoreStatusHistoryType()
            {
                Id = filter.Selects.Contains(StoreStatusHistoryTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreStatusHistoryTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreStatusHistoryTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return StoreStatusHistoryTypees;
        }

        public async Task<int> Count(StoreStatusHistoryTypeFilter filter)
        {
            IQueryable<StoreStatusHistoryTypeDAO> StoreStatusHistoryTypees = DataContext.StoreStatusHistoryType.AsNoTracking();
            StoreStatusHistoryTypees = DynamicFilter(StoreStatusHistoryTypees, filter);
            StoreStatusHistoryTypees = OrFilter(StoreStatusHistoryTypees, filter);
            return await StoreStatusHistoryTypees.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreStatusHistoryTypeFilter filter)
        {
            IQueryable<StoreStatusHistoryTypeDAO> StoreStatusHistoryTypees = DataContext.StoreStatusHistoryType.AsNoTracking();
            StoreStatusHistoryTypees = DynamicFilter(StoreStatusHistoryTypees, filter);
            return await StoreStatusHistoryTypees.CountWithNoLockAsync();
        }

        public async Task<List<StoreStatusHistoryType>> List(StoreStatusHistoryTypeFilter filter)
        {
            if (filter == null) return new List<StoreStatusHistoryType>();
            IQueryable<StoreStatusHistoryTypeDAO> StoreStatusHistoryTypeDAOs = DataContext.StoreStatusHistoryType.AsNoTracking();
            StoreStatusHistoryTypeDAOs = DynamicFilter(StoreStatusHistoryTypeDAOs, filter);
            StoreStatusHistoryTypeDAOs = OrFilter(StoreStatusHistoryTypeDAOs, filter);
            StoreStatusHistoryTypeDAOs = DynamicOrder(StoreStatusHistoryTypeDAOs, filter);
            List<StoreStatusHistoryType> StoreStatusHistoryTypees = await DynamicSelect(StoreStatusHistoryTypeDAOs, filter);
            return StoreStatusHistoryTypees;
        }

        public async Task<StoreStatusHistoryType> Get(long Id)
        {
            StoreStatusHistoryType StoreStatusHistoryType = await DataContext.StoreStatusHistoryType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new StoreStatusHistoryType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (StoreStatusHistoryType == null)
                return null;

            return StoreStatusHistoryType;
        }
    }
}
