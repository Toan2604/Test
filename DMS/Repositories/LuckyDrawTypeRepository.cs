using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using DMS.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.Repositories
{
    public interface ILuckyDrawTypeRepository
    {
        Task<int> CountAll(LuckyDrawTypeFilter LuckyDrawTypeFilter);
        Task<int> Count(LuckyDrawTypeFilter LuckyDrawTypeFilter);
        Task<List<LuckyDrawType>> List(LuckyDrawTypeFilter LuckyDrawTypeFilter);
        Task<List<LuckyDrawType>> List(List<long> Ids);
    }
    public class LuckyDrawTypeRepository : ILuckyDrawTypeRepository
    {
        private DataContext DataContext;
        public LuckyDrawTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawTypeDAO>> DynamicFilter(IQueryable<LuckyDrawTypeDAO> query, LuckyDrawTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private async Task<IQueryable<LuckyDrawTypeDAO>> OrFilter(IQueryable<LuckyDrawTypeDAO> query, LuckyDrawTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawTypeDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawTypeFilter LuckyDrawTypeFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, LuckyDrawTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, LuckyDrawTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<LuckyDrawTypeDAO> DynamicOrder(IQueryable<LuckyDrawTypeDAO> query, LuckyDrawTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case LuckyDrawTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case LuckyDrawTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDrawType>> DynamicSelect(IQueryable<LuckyDrawTypeDAO> query, LuckyDrawTypeFilter filter)
        {
            List<LuckyDrawType> LuckyDrawTypes = await query.Select(q => new LuckyDrawType()
            {
                Id = filter.Selects.Contains(LuckyDrawTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(LuckyDrawTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(LuckyDrawTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return LuckyDrawTypes;
        }

        public async Task<int> CountAll(LuckyDrawTypeFilter filter)
        {
            IQueryable<LuckyDrawTypeDAO> LuckyDrawTypeDAOs = DataContext.LuckyDrawType.AsNoTracking();
            LuckyDrawTypeDAOs = await DynamicFilter(LuckyDrawTypeDAOs, filter);
            return await LuckyDrawTypeDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(LuckyDrawTypeFilter filter)
        {
            IQueryable<LuckyDrawTypeDAO> LuckyDrawTypeDAOs = DataContext.LuckyDrawType.AsNoTracking();
            LuckyDrawTypeDAOs = await DynamicFilter(LuckyDrawTypeDAOs, filter);
            LuckyDrawTypeDAOs = await OrFilter(LuckyDrawTypeDAOs, filter);
            return await LuckyDrawTypeDAOs.CountWithNoLockAsync();
        }

        public async Task<List<LuckyDrawType>> List(LuckyDrawTypeFilter filter)
        {
            if (filter == null) return new List<LuckyDrawType>();
            IQueryable<LuckyDrawTypeDAO> LuckyDrawTypeDAOs = DataContext.LuckyDrawType.AsNoTracking();
            LuckyDrawTypeDAOs = await DynamicFilter(LuckyDrawTypeDAOs, filter);
            LuckyDrawTypeDAOs = await OrFilter(LuckyDrawTypeDAOs, filter);
            LuckyDrawTypeDAOs = DynamicOrder(LuckyDrawTypeDAOs, filter);
            List<LuckyDrawType> LuckyDrawTypes = await DynamicSelect(LuckyDrawTypeDAOs, filter);
            return LuckyDrawTypes;
        }

        public async Task<List<LuckyDrawType>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawTypeDAO> query = DataContext.LuckyDrawType.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDrawType> LuckyDrawTypes = await query.AsNoTracking()
            .Select(x => new LuckyDrawType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();
            

            return LuckyDrawTypes;
        }

    }
}
