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
    public interface IPromotionProductAppliedTypeRepository
    {
        Task<int> Count(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter);
        Task<int> CountAll(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter);
        Task<List<PromotionProductAppliedType>> List(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter);
        Task<PromotionProductAppliedType> Get(long Id);
    }
    public class PromotionProductAppliedTypeRepository : IPromotionProductAppliedTypeRepository
    {
        private DataContext DataContext;
        public PromotionProductAppliedTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionProductAppliedTypeDAO> DynamicFilter(IQueryable<PromotionProductAppliedTypeDAO> query, PromotionProductAppliedTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<PromotionProductAppliedTypeDAO> OrFilter(IQueryable<PromotionProductAppliedTypeDAO> query, PromotionProductAppliedTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionProductAppliedTypeDAO> initQuery = query.Where(q => false);
            foreach (PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter in filter.OrFilter)
            {
                IQueryable<PromotionProductAppliedTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionProductAppliedTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionProductAppliedTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionProductAppliedTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionProductAppliedTypeDAO> DynamicOrder(IQueryable<PromotionProductAppliedTypeDAO> query, PromotionProductAppliedTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductAppliedTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionProductAppliedTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionProductAppliedTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionProductAppliedTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionProductAppliedTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionProductAppliedTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionProductAppliedType>> DynamicSelect(IQueryable<PromotionProductAppliedTypeDAO> query, PromotionProductAppliedTypeFilter filter)
        {
            List<PromotionProductAppliedType> PromotionProductAppliedTypes = await query.Select(q => new PromotionProductAppliedType()
            {
                Id = filter.Selects.Contains(PromotionProductAppliedTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionProductAppliedTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionProductAppliedTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return PromotionProductAppliedTypes;
        }

        public async Task<int> Count(PromotionProductAppliedTypeFilter filter)
        {
            IQueryable<PromotionProductAppliedTypeDAO> PromotionProductAppliedTypes = DataContext.PromotionProductAppliedType.AsNoTracking();
            PromotionProductAppliedTypes = DynamicFilter(PromotionProductAppliedTypes, filter);
            PromotionProductAppliedTypes = OrFilter(PromotionProductAppliedTypes, filter);
            return await PromotionProductAppliedTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionProductAppliedTypeFilter filter)
        {
            IQueryable<PromotionProductAppliedTypeDAO> PromotionProductAppliedTypes = DataContext.PromotionProductAppliedType.AsNoTracking();
            PromotionProductAppliedTypes = DynamicFilter(PromotionProductAppliedTypes, filter);
            return await PromotionProductAppliedTypes.CountWithNoLockAsync();
        }

        public async Task<List<PromotionProductAppliedType>> List(PromotionProductAppliedTypeFilter filter)
        {
            if (filter == null) return new List<PromotionProductAppliedType>();
            IQueryable<PromotionProductAppliedTypeDAO> PromotionProductAppliedTypeDAOs = DataContext.PromotionProductAppliedType.AsNoTracking();
            PromotionProductAppliedTypeDAOs = DynamicFilter(PromotionProductAppliedTypeDAOs, filter);
            PromotionProductAppliedTypeDAOs = OrFilter(PromotionProductAppliedTypeDAOs, filter);
            PromotionProductAppliedTypeDAOs = DynamicOrder(PromotionProductAppliedTypeDAOs, filter);
            List<PromotionProductAppliedType> PromotionProductAppliedTypes = await DynamicSelect(PromotionProductAppliedTypeDAOs, filter);
            return PromotionProductAppliedTypes;
        }

        public async Task<PromotionProductAppliedType> Get(long Id)
        {
            PromotionProductAppliedType PromotionProductAppliedType = await DataContext.PromotionProductAppliedType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionProductAppliedType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionProductAppliedType == null)
                return null;

            return PromotionProductAppliedType;
        }
    }
}
