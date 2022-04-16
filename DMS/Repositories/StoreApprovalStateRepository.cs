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
    public interface IStoreApprovalStateRepository
    {
        Task<int> Count(StoreApprovalStateFilter StoreApprovalStateFilter);
        Task<int> CountAll(StoreApprovalStateFilter StoreApprovalStateFilter);
        Task<List<StoreApprovalState>> List(StoreApprovalStateFilter StoreApprovalStateFilter);
        Task<List<StoreApprovalState>> List(List<long> Ids);
        Task<StoreApprovalState> Get(long Id);
    }
    public class StoreApprovalStateRepository : IStoreApprovalStateRepository
    {
        private DataContext DataContext;
        public StoreApprovalStateRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreApprovalStateDAO> DynamicFilter(IQueryable<StoreApprovalStateDAO> query, StoreApprovalStateFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<StoreApprovalStateDAO> OrFilter(IQueryable<StoreApprovalStateDAO> query, StoreApprovalStateFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreApprovalStateDAO> initQuery = query.Where(q => false);
            foreach (StoreApprovalStateFilter StoreApprovalStateFilter in filter.OrFilter)
            {
                IQueryable<StoreApprovalStateDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreApprovalStateFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreApprovalStateFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreApprovalStateFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreApprovalStateDAO> DynamicOrder(IQueryable<StoreApprovalStateDAO> query, StoreApprovalStateFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreApprovalStateOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreApprovalStateOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreApprovalStateOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreApprovalStateOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreApprovalStateOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreApprovalStateOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreApprovalState>> DynamicSelect(IQueryable<StoreApprovalStateDAO> query, StoreApprovalStateFilter filter)
        {
            List<StoreApprovalState> StoreApprovalStates = await query.Select(q => new StoreApprovalState()
            {
                Id = filter.Selects.Contains(StoreApprovalStateSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreApprovalStateSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreApprovalStateSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return StoreApprovalStates;
        }

        public async Task<int> Count(StoreApprovalStateFilter filter)
        {
            IQueryable<StoreApprovalStateDAO> StoreApprovalStateDAOs = DataContext.StoreApprovalState.AsNoTracking();
            StoreApprovalStateDAOs = DynamicFilter(StoreApprovalStateDAOs, filter);
            StoreApprovalStateDAOs = OrFilter(StoreApprovalStateDAOs, filter);
            return await StoreApprovalStateDAOs.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreApprovalStateFilter filter)
        {
            IQueryable<StoreApprovalStateDAO> StoreApprovalStateDAOs = DataContext.StoreApprovalState.AsNoTracking();
            StoreApprovalStateDAOs = DynamicFilter(StoreApprovalStateDAOs, filter);
            return await StoreApprovalStateDAOs.CountWithNoLockAsync();
        }

        public async Task<List<StoreApprovalState>> List(StoreApprovalStateFilter filter)
        {
            if (filter == null) return new List<StoreApprovalState>();
            IQueryable<StoreApprovalStateDAO> StoreApprovalStateDAOs = DataContext.StoreApprovalState.AsNoTracking();
            StoreApprovalStateDAOs = DynamicFilter(StoreApprovalStateDAOs, filter);
            StoreApprovalStateDAOs = OrFilter(StoreApprovalStateDAOs, filter);
            StoreApprovalStateDAOs = DynamicOrder(StoreApprovalStateDAOs, filter);
            List<StoreApprovalState> StoreApprovalStates = await DynamicSelect(StoreApprovalStateDAOs, filter);
            return StoreApprovalStates;
        }

        public async Task<StoreApprovalState> Get(long Id)
        {
            StoreApprovalState StoreApprovalState = await DataContext.StoreApprovalState.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new StoreApprovalState()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefaultWithNoLockAsync();

            if (StoreApprovalState == null)
                return null;

            return StoreApprovalState;
        }

        public async Task<List<StoreApprovalState>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<StoreApprovalState> StoreApprovalStates = await DataContext.StoreApprovalState.AsNoTracking()
               .Where(x => x.Id, IdFilter)
               .Select(x => new StoreApprovalState()
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
               }).ToListWithNoLockAsync();
            return StoreApprovalStates;
        }
    }
}
