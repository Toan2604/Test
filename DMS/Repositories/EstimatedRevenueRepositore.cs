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
    public interface IEstimatedRevenueRepository
    {
        Task<int> Count(EstimatedRevenueFilter EstimatedRevenueFilter);
        Task<int> CountAll(EstimatedRevenueFilter EstimatedRevenueFilter);
        Task<List<EstimatedRevenue>> List(EstimatedRevenueFilter EstimatedRevenueFilter);
        Task<List<EstimatedRevenue>> List(List<long> Ids);
        Task<EstimatedRevenue> Get(long Id);
    }
    public class EstimatedRevenueRepository : IEstimatedRevenueRepository
    {
        private DataContext DataContext;
        public EstimatedRevenueRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<EstimatedRevenueDAO> DynamicFilter(IQueryable<EstimatedRevenueDAO> query, EstimatedRevenueFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<EstimatedRevenueDAO> OrFilter(IQueryable<EstimatedRevenueDAO> query, EstimatedRevenueFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<EstimatedRevenueDAO> initQuery = query.Where(q => false);
            foreach (EstimatedRevenueFilter EstimatedRevenueFilter in filter.OrFilter)
            {
                IQueryable<EstimatedRevenueDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, EstimatedRevenueFilter.Id);
                queryable = queryable.Where(q => q.Code, EstimatedRevenueFilter.Code);
                queryable = queryable.Where(q => q.Name, EstimatedRevenueFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<EstimatedRevenueDAO> DynamicOrder(IQueryable<EstimatedRevenueDAO> query, EstimatedRevenueFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case EstimatedRevenueOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case EstimatedRevenueOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case EstimatedRevenueOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case EstimatedRevenueOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case EstimatedRevenueOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case EstimatedRevenueOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<EstimatedRevenue>> DynamicSelect(IQueryable<EstimatedRevenueDAO> query, EstimatedRevenueFilter filter)
        {
            List<EstimatedRevenue> EstimatedRevenues = await query.Select(q => new EstimatedRevenue()
            {
                Id = filter.Selects.Contains(EstimatedRevenueSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(EstimatedRevenueSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(EstimatedRevenueSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return EstimatedRevenues;
        }

        public async Task<int> Count(EstimatedRevenueFilter filter)
        {
            IQueryable<EstimatedRevenueDAO> EstimatedRevenueDAOs = DataContext.EstimatedRevenue.AsNoTracking();
            EstimatedRevenueDAOs = DynamicFilter(EstimatedRevenueDAOs, filter);
            EstimatedRevenueDAOs = OrFilter(EstimatedRevenueDAOs, filter);
            return await EstimatedRevenueDAOs.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(EstimatedRevenueFilter filter)
        {
            IQueryable<EstimatedRevenueDAO> EstimatedRevenueDAOs = DataContext.EstimatedRevenue.AsNoTracking();
            EstimatedRevenueDAOs = DynamicFilter(EstimatedRevenueDAOs, filter);
            return await EstimatedRevenueDAOs.CountWithNoLockAsync();
        }

        public async Task<List<EstimatedRevenue>> List(EstimatedRevenueFilter filter)
        {
            if (filter == null) return new List<EstimatedRevenue>();
            IQueryable<EstimatedRevenueDAO> EstimatedRevenueDAOs = DataContext.EstimatedRevenue.AsNoTracking();
            EstimatedRevenueDAOs = DynamicFilter(EstimatedRevenueDAOs, filter);
            EstimatedRevenueDAOs = OrFilter(EstimatedRevenueDAOs, filter);
            EstimatedRevenueDAOs = DynamicOrder(EstimatedRevenueDAOs, filter);
            List<EstimatedRevenue> EstimatedRevenues = await DynamicSelect(EstimatedRevenueDAOs, filter);
            return EstimatedRevenues;
        }

        public async Task<EstimatedRevenue> Get(long Id)
        {
            EstimatedRevenue EstimatedRevenue = await DataContext.EstimatedRevenue.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new EstimatedRevenue()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefaultWithNoLockAsync();

            if (EstimatedRevenue == null)
                return null;

            return EstimatedRevenue;
        }

        public async Task<List<EstimatedRevenue>> List(List<long> Ids)
        {
            List<EstimatedRevenue> EstimatedRevenues = await DataContext.EstimatedRevenue.AsNoTracking()
               .Where(x => Ids.Contains(x.Id))
               .Select(x => new EstimatedRevenue()
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
               }).ToListWithNoLockAsync();
            return EstimatedRevenues;
        }
    }
}
