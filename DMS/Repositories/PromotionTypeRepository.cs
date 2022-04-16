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
    public interface IPromotionTypeRepository
    {
        Task<int> Count(PromotionTypeFilter PromotionTypeFilter);
        Task<int> CountAll(PromotionTypeFilter PromotionTypeFilter);
        Task<List<PromotionType>> List(PromotionTypeFilter PromotionTypeFilter);
        Task<PromotionType> Get(long Id);
    }
    public class PromotionTypeRepository : IPromotionTypeRepository
    {
        private DataContext DataContext;
        public PromotionTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionTypeDAO> DynamicFilter(IQueryable<PromotionTypeDAO> query, PromotionTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<PromotionTypeDAO> OrFilter(IQueryable<PromotionTypeDAO> query, PromotionTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionTypeDAO> initQuery = query.Where(q => false);
            foreach (PromotionTypeFilter PromotionTypeFilter in filter.OrFilter)
            {
                IQueryable<PromotionTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionTypeDAO> DynamicOrder(IQueryable<PromotionTypeDAO> query, PromotionTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionType>> DynamicSelect(IQueryable<PromotionTypeDAO> query, PromotionTypeFilter filter)
        {
            List<PromotionType> PromotionTypes = await query.Select(q => new PromotionType()
            {
                Id = filter.Selects.Contains(PromotionTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return PromotionTypes;
        }

        public async Task<int> Count(PromotionTypeFilter filter)
        {
            IQueryable<PromotionTypeDAO> PromotionTypes = DataContext.PromotionType.AsNoTracking();
            PromotionTypes = DynamicFilter(PromotionTypes, filter);
            PromotionTypes = OrFilter(PromotionTypes, filter);
            return await PromotionTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionTypeFilter filter)
        {
            IQueryable<PromotionTypeDAO> PromotionTypes = DataContext.PromotionType.AsNoTracking();
            PromotionTypes = DynamicFilter(PromotionTypes, filter);
            return await PromotionTypes.CountWithNoLockAsync();
        }

        public async Task<List<PromotionType>> List(PromotionTypeFilter filter)
        {
            if (filter == null) return new List<PromotionType>();
            IQueryable<PromotionTypeDAO> PromotionTypeDAOs = DataContext.PromotionType.AsNoTracking();
            PromotionTypeDAOs = DynamicFilter(PromotionTypeDAOs, filter);
            PromotionTypeDAOs = OrFilter(PromotionTypeDAOs, filter);
            PromotionTypeDAOs = DynamicOrder(PromotionTypeDAOs, filter);
            List<PromotionType> PromotionTypes = await DynamicSelect(PromotionTypeDAOs, filter);
            return PromotionTypes;
        }

        public async Task<PromotionType> Get(long Id)
        {
            PromotionType PromotionType = await DataContext.PromotionType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionType == null)
                return null;

            return PromotionType;
        }
    }
}
