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
    public interface ISurveyOptionTypeRepository
    {
        Task<int> Count(SurveyOptionTypeFilter SurveyOptionTypeFilter);
        Task<int> CountAll(SurveyOptionTypeFilter SurveyOptionTypeFilter);
        Task<List<SurveyOptionType>> List(SurveyOptionTypeFilter SurveyOptionTypeFilter);
        Task<SurveyOptionType> Get(long Id);
    }
    public class SurveyOptionTypeRepository : ISurveyOptionTypeRepository
    {
        private DataContext DataContext;
        public SurveyOptionTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<SurveyOptionTypeDAO> DynamicFilter(IQueryable<SurveyOptionTypeDAO> query, SurveyOptionTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<SurveyOptionTypeDAO> OrFilter(IQueryable<SurveyOptionTypeDAO> query, SurveyOptionTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<SurveyOptionTypeDAO> initQuery = query.Where(q => false);
            foreach (SurveyOptionTypeFilter SurveyOptionTypeFilter in filter.OrFilter)
            {
                IQueryable<SurveyOptionTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, SurveyOptionTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, SurveyOptionTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, SurveyOptionTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<SurveyOptionTypeDAO> DynamicOrder(IQueryable<SurveyOptionTypeDAO> query, SurveyOptionTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SurveyOptionTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case SurveyOptionTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case SurveyOptionTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SurveyOptionTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case SurveyOptionTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case SurveyOptionTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<SurveyOptionType>> DynamicSelect(IQueryable<SurveyOptionTypeDAO> query, SurveyOptionTypeFilter filter)
        {
            List<SurveyOptionType> SurveyOptionTypes = await query.Select(q => new SurveyOptionType()
            {
                Id = filter.Selects.Contains(SurveyOptionTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(SurveyOptionTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(SurveyOptionTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return SurveyOptionTypes;
        }

        public async Task<int> Count(SurveyOptionTypeFilter filter)
        {
            IQueryable<SurveyOptionTypeDAO> SurveyOptionTypes = DataContext.SurveyOptionType.AsNoTracking();
            SurveyOptionTypes = DynamicFilter(SurveyOptionTypes, filter);
            SurveyOptionTypes = OrFilter(SurveyOptionTypes, filter);
            return await SurveyOptionTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(SurveyOptionTypeFilter filter)
        {
            IQueryable<SurveyOptionTypeDAO> SurveyOptionTypes = DataContext.SurveyOptionType.AsNoTracking();
            SurveyOptionTypes = DynamicFilter(SurveyOptionTypes, filter);
            return await SurveyOptionTypes.CountWithNoLockAsync();
        }
        public async Task<List<SurveyOptionType>> List(SurveyOptionTypeFilter filter)
        {
            if (filter == null) return new List<SurveyOptionType>();
            IQueryable<SurveyOptionTypeDAO> SurveyOptionTypeDAOs = DataContext.SurveyOptionType.AsNoTracking();
            SurveyOptionTypeDAOs = DynamicFilter(SurveyOptionTypeDAOs, filter);
            SurveyOptionTypeDAOs = OrFilter(SurveyOptionTypeDAOs, filter);
            SurveyOptionTypeDAOs = DynamicOrder(SurveyOptionTypeDAOs, filter);
            List<SurveyOptionType> SurveyOptionTypes = await DynamicSelect(SurveyOptionTypeDAOs, filter);
            return SurveyOptionTypes;
        }

        public async Task<SurveyOptionType> Get(long Id)
        {
            SurveyOptionType SurveyOptionType = await DataContext.SurveyOptionType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new SurveyOptionType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (SurveyOptionType == null)
                return null;

            return SurveyOptionType;
        }
    }
}
