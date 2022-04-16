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
    public interface IKpiYearRepository
    {
        Task<int> Count(KpiYearFilter KpiYearFilter);
        Task<int> CountAll(KpiYearFilter KpiYearFilter);
        Task<List<KpiYear>> List(KpiYearFilter KpiYearFilter);
        Task<KpiYear> Get(long Id);
    }
    public class KpiYearRepository : IKpiYearRepository
    {
        private DataContext DataContext;
        public KpiYearRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiYearDAO> DynamicFilter(IQueryable<KpiYearDAO> query, KpiYearFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<KpiYearDAO> OrFilter(IQueryable<KpiYearDAO> query, KpiYearFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiYearDAO> initQuery = query.Where(q => false);
            foreach (KpiYearFilter KpiYearFilter in filter.OrFilter)
            {
                IQueryable<KpiYearDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiYearDAO> DynamicOrder(IQueryable<KpiYearDAO> query, KpiYearFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiYearOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiYearOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case KpiYearOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiYearOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiYearOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case KpiYearOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiYear>> DynamicSelect(IQueryable<KpiYearDAO> query, KpiYearFilter filter)
        {
            List<KpiYear> KpiYears = await query.Select(q => new KpiYear()
            {
                Id = filter.Selects.Contains(KpiYearSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(KpiYearSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(KpiYearSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return KpiYears;
        }

        public async Task<int> Count(KpiYearFilter filter)
        {
            IQueryable<KpiYearDAO> KpiYears = DataContext.KpiYear.AsNoTracking();
            KpiYears = DynamicFilter(KpiYears, filter);
            KpiYears = OrFilter(KpiYears, filter);
            return await KpiYears.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiYearFilter filter)
        {
            IQueryable<KpiYearDAO> KpiYears = DataContext.KpiYear.AsNoTracking();
            KpiYears = DynamicFilter(KpiYears, filter);
            return await KpiYears.CountWithNoLockAsync();
        }

        public async Task<List<KpiYear>> List(KpiYearFilter filter)
        {
            if (filter == null) return new List<KpiYear>();
            IQueryable<KpiYearDAO> KpiYearDAOs = DataContext.KpiYear.AsNoTracking();
            KpiYearDAOs = DynamicFilter(KpiYearDAOs, filter);
            KpiYearDAOs = OrFilter(KpiYearDAOs, filter);
            KpiYearDAOs = DynamicOrder(KpiYearDAOs, filter);
            List<KpiYear> KpiYears = await DynamicSelect(KpiYearDAOs, filter);
            return KpiYears;
        }

        public async Task<KpiYear> Get(long Id)
        {
            KpiYear KpiYear = await DataContext.KpiYear.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new KpiYear()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiYear == null)
                return null;

            return KpiYear;
        }
    }
}
