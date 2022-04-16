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
    public interface IPromotionDiscountTypeRepository
    {
        Task<int> Count(PromotionDiscountTypeFilter PromotionDiscountTypeFilter);
        Task<int> CountAll(PromotionDiscountTypeFilter PromotionDiscountTypeFilter);
        Task<List<PromotionDiscountType>> List(PromotionDiscountTypeFilter PromotionDiscountTypeFilter);
        Task<PromotionDiscountType> Get(long Id);
    }
    public class PromotionDiscountTypeRepository : IPromotionDiscountTypeRepository
    {
        private DataContext DataContext;
        public PromotionDiscountTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PromotionDiscountTypeDAO> DynamicFilter(IQueryable<PromotionDiscountTypeDAO> query, PromotionDiscountTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<PromotionDiscountTypeDAO> OrFilter(IQueryable<PromotionDiscountTypeDAO> query, PromotionDiscountTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PromotionDiscountTypeDAO> initQuery = query.Where(q => false);
            foreach (PromotionDiscountTypeFilter PromotionDiscountTypeFilter in filter.OrFilter)
            {
                IQueryable<PromotionDiscountTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PromotionDiscountTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, PromotionDiscountTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, PromotionDiscountTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PromotionDiscountTypeDAO> DynamicOrder(IQueryable<PromotionDiscountTypeDAO> query, PromotionDiscountTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PromotionDiscountTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PromotionDiscountTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PromotionDiscountTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PromotionDiscountTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PromotionDiscountTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PromotionDiscountTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PromotionDiscountType>> DynamicSelect(IQueryable<PromotionDiscountTypeDAO> query, PromotionDiscountTypeFilter filter)
        {
            List<PromotionDiscountType> PromotionDiscountTypes = await query.Select(q => new PromotionDiscountType()
            {
                Id = filter.Selects.Contains(PromotionDiscountTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PromotionDiscountTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PromotionDiscountTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return PromotionDiscountTypes;
        }

        public async Task<int> Count(PromotionDiscountTypeFilter filter)
        {
            IQueryable<PromotionDiscountTypeDAO> PromotionDiscountTypes = DataContext.PromotionDiscountType.AsNoTracking();
            PromotionDiscountTypes = DynamicFilter(PromotionDiscountTypes, filter);
            PromotionDiscountTypes = OrFilter(PromotionDiscountTypes, filter);
            return await PromotionDiscountTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PromotionDiscountTypeFilter filter)
        {
            IQueryable<PromotionDiscountTypeDAO> PromotionDiscountTypes = DataContext.PromotionDiscountType.AsNoTracking();
            PromotionDiscountTypes = DynamicFilter(PromotionDiscountTypes, filter);
            return await PromotionDiscountTypes.CountWithNoLockAsync();
        }
        public async Task<List<PromotionDiscountType>> List(PromotionDiscountTypeFilter filter)
        {
            if (filter == null) return new List<PromotionDiscountType>();
            IQueryable<PromotionDiscountTypeDAO> PromotionDiscountTypeDAOs = DataContext.PromotionDiscountType.AsNoTracking();
            PromotionDiscountTypeDAOs = DynamicFilter(PromotionDiscountTypeDAOs, filter);
            PromotionDiscountTypeDAOs = OrFilter(PromotionDiscountTypeDAOs, filter);
            PromotionDiscountTypeDAOs = DynamicOrder(PromotionDiscountTypeDAOs, filter);
            List<PromotionDiscountType> PromotionDiscountTypes = await DynamicSelect(PromotionDiscountTypeDAOs, filter);
            return PromotionDiscountTypes;
        }

        public async Task<PromotionDiscountType> Get(long Id)
        {
            PromotionDiscountType PromotionDiscountType = await DataContext.PromotionDiscountType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PromotionDiscountType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (PromotionDiscountType == null)
                return null;

            return PromotionDiscountType;
        }
    }
}
