using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IVariationRepository
    {
        Task<int> Count(VariationFilter VariationFilter);
        Task<List<Variation>> List(VariationFilter VariationFilter);
        Task<Variation> Get(long Id);
    }
    public class VariationRepository : IVariationRepository
    {
        private DataContext DataContext;
        public VariationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<VariationDAO> DynamicFilter(IQueryable<VariationDAO> query, VariationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.VariationGroupingId, filter.VariationGroupingId);
           // query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<VariationDAO> OrFilter(IQueryable<VariationDAO> query, VariationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<VariationDAO> initQuery = query.Where(q => false);
            foreach (VariationFilter VariationFilter in filter.OrFilter)
            {
                IQueryable<VariationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, VariationFilter.Id);
                queryable = queryable.Where(q => q.Code, VariationFilter.Code);
                queryable = queryable.Where(q => q.Name, VariationFilter.Name);
                queryable = queryable.Where(q => q.VariationGroupingId, VariationFilter.VariationGroupingId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<VariationDAO> DynamicOrder(IQueryable<VariationDAO> query, VariationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case VariationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case VariationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case VariationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case VariationOrder.VariationGrouping:
                            query = query.OrderBy(q => q.VariationGroupingId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case VariationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case VariationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case VariationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case VariationOrder.VariationGrouping:
                            query = query.OrderByDescending(q => q.VariationGroupingId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Variation>> DynamicSelect(IQueryable<VariationDAO> query, VariationFilter filter)
        {
            List<Variation> Variations = await query.Select(q => new Variation()
            {
                Id = filter.Selects.Contains(VariationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(VariationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(VariationSelect.Name) ? q.Name : default(string),
                VariationGroupingId = filter.Selects.Contains(VariationSelect.VariationGrouping) ? q.VariationGroupingId : default(long),
                VariationGrouping = filter.Selects.Contains(VariationSelect.VariationGrouping) && q.VariationGrouping != null ? new VariationGrouping
                {
                    Id = q.VariationGrouping.Id,
                    Name = q.VariationGrouping.Name,
                    ProductId = q.VariationGrouping.ProductId,
                } : null,
            }).ToListAsync();
            return Variations;
        }

        public async Task<int> Count(VariationFilter filter)
        {
            IQueryable<VariationDAO> Variations = DataContext.Variation;
            Variations = DynamicFilter(Variations, filter);
            return await Variations.CountAsync();
        }

        public async Task<List<Variation>> List(VariationFilter filter)
        {
            if (filter == null) return new List<Variation>();
            IQueryable<VariationDAO> VariationDAOs = DataContext.Variation;
            VariationDAOs = DynamicFilter(VariationDAOs, filter);
            VariationDAOs = DynamicOrder(VariationDAOs, filter);
            List<Variation> Variations = await DynamicSelect(VariationDAOs, filter);
            return Variations;
        }

        public async Task<Variation> Get(long Id)
        {
            Variation Variation = await DataContext.Variation.Where(x => x.Id == Id).Select(x => new Variation()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                VariationGroupingId = x.VariationGroupingId,
                VariationGrouping = x.VariationGrouping == null ? null : new VariationGrouping
                {
                    Id = x.VariationGrouping.Id,
                    Name = x.VariationGrouping.Name,
                    ProductId = x.VariationGrouping.ProductId,
                },
            }).FirstOrDefaultAsync();

            if (Variation == null)
                return null;

            return Variation;
        }
    }
}
