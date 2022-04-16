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
    public interface IUsedVariationRepository
    {
        Task<int> Count(UsedVariationFilter UsedVariationFilter);
        Task<int> CountAll(UsedVariationFilter UsedVariationFilter);
        Task<List<UsedVariation>> List(UsedVariationFilter UsedVariationFilter);
        Task<UsedVariation> Get(long Id);
        Task<bool> BulkMerge(List<UsedVariation> UsedVariations);
    }
    public class UsedVariationRepository : IUsedVariationRepository
    {
        private DataContext DataContext;
        public UsedVariationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<UsedVariationDAO> DynamicFilter(IQueryable<UsedVariationDAO> query, UsedVariationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<UsedVariationDAO> OrFilter(IQueryable<UsedVariationDAO> query, UsedVariationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<UsedVariationDAO> initQuery = query.Where(q => false);
            foreach (UsedVariationFilter UsedVariationFilter in filter.OrFilter)
            {
                IQueryable<UsedVariationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UsedVariationFilter.Id);
                queryable = queryable.Where(q => q.Code, UsedVariationFilter.Code);
                queryable = queryable.Where(q => q.Name, UsedVariationFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<UsedVariationDAO> DynamicOrder(IQueryable<UsedVariationDAO> query, UsedVariationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case UsedVariationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case UsedVariationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case UsedVariationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case UsedVariationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case UsedVariationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case UsedVariationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<UsedVariation>> DynamicSelect(IQueryable<UsedVariationDAO> query, UsedVariationFilter filter)
        {
            List<UsedVariation> UsedVariations = await query.Select(q => new UsedVariation()
            {
                Id = filter.Selects.Contains(UsedVariationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(UsedVariationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(UsedVariationSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return UsedVariations;
        }

        public async Task<int> Count(UsedVariationFilter filter)
        {
            IQueryable<UsedVariationDAO> UsedVariations = DataContext.UsedVariation.AsNoTracking();
            UsedVariations = DynamicFilter(UsedVariations, filter);
            UsedVariations = OrFilter(UsedVariations, filter);
            return await UsedVariations.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(UsedVariationFilter filter)
        {
            IQueryable<UsedVariationDAO> UsedVariations = DataContext.UsedVariation.AsNoTracking();
            UsedVariations = DynamicFilter(UsedVariations, filter);
            return await UsedVariations.CountWithNoLockAsync();
        }

        public async Task<List<UsedVariation>> List(UsedVariationFilter filter)
        {
            if (filter == null) return new List<UsedVariation>();
            IQueryable<UsedVariationDAO> UsedVariationDAOs = DataContext.UsedVariation.AsNoTracking();
            UsedVariationDAOs = DynamicFilter(UsedVariationDAOs, filter);
            UsedVariationDAOs = OrFilter(UsedVariationDAOs, filter);
            UsedVariationDAOs = DynamicOrder(UsedVariationDAOs, filter);
            List<UsedVariation> UsedVariations = await DynamicSelect(UsedVariationDAOs, filter);
            return UsedVariations;
        }

        public async Task<UsedVariation> Get(long Id)
        {
            UsedVariation UsedVariation = await DataContext.UsedVariation.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new UsedVariation()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (UsedVariation == null)
                return null;

            return UsedVariation;
        }
        public async Task<bool> BulkMerge(List<UsedVariation> UsedVariations)
        {
            List<UsedVariationDAO> UsedVariationDAOs = UsedVariations.Select(x => new UsedVariationDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            await DataContext.BulkMergeAsync(UsedVariationDAOs);
            return true;
        }
    }
}
