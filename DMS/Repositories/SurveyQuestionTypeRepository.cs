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
    public interface ISurveyQuestionTypeRepository
    {
        Task<int> Count(SurveyQuestionTypeFilter SurveyQuestionTypeFilter);
        Task<int> CountAll(SurveyQuestionTypeFilter SurveyQuestionTypeFilter);
        Task<List<SurveyQuestionType>> List(SurveyQuestionTypeFilter SurveyQuestionTypeFilter);
        Task<SurveyQuestionType> Get(long Id);
    }
    public class SurveyQuestionTypeRepository : ISurveyQuestionTypeRepository
    {
        private DataContext DataContext;
        public SurveyQuestionTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<SurveyQuestionTypeDAO> DynamicFilter(IQueryable<SurveyQuestionTypeDAO> query, SurveyQuestionTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<SurveyQuestionTypeDAO> OrFilter(IQueryable<SurveyQuestionTypeDAO> query, SurveyQuestionTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<SurveyQuestionTypeDAO> initQuery = query.Where(q => false);
            foreach (SurveyQuestionTypeFilter SurveyQuestionTypeFilter in filter.OrFilter)
            {
                IQueryable<SurveyQuestionTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, SurveyQuestionTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, SurveyQuestionTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, SurveyQuestionTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<SurveyQuestionTypeDAO> DynamicOrder(IQueryable<SurveyQuestionTypeDAO> query, SurveyQuestionTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SurveyQuestionTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case SurveyQuestionTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case SurveyQuestionTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SurveyQuestionTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case SurveyQuestionTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case SurveyQuestionTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<SurveyQuestionType>> DynamicSelect(IQueryable<SurveyQuestionTypeDAO> query, SurveyQuestionTypeFilter filter)
        {
            List<SurveyQuestionType> SurveyQuestionTypes = await query.Select(q => new SurveyQuestionType()
            {
                Id = filter.Selects.Contains(SurveyQuestionTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(SurveyQuestionTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(SurveyQuestionTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return SurveyQuestionTypes;
        }

        public async Task<int> Count(SurveyQuestionTypeFilter filter)
        {
            IQueryable<SurveyQuestionTypeDAO> SurveyQuestionTypes = DataContext.SurveyQuestionType.AsNoTracking();
            SurveyQuestionTypes = DynamicFilter(SurveyQuestionTypes, filter);
            SurveyQuestionTypes = OrFilter(SurveyQuestionTypes, filter);
            return await SurveyQuestionTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(SurveyQuestionTypeFilter filter)
        {
            IQueryable<SurveyQuestionTypeDAO> SurveyQuestionTypes = DataContext.SurveyQuestionType.AsNoTracking();
            SurveyQuestionTypes = DynamicFilter(SurveyQuestionTypes, filter);
            return await SurveyQuestionTypes.CountWithNoLockAsync();
        }
        public async Task<List<SurveyQuestionType>> List(SurveyQuestionTypeFilter filter)
        {
            if (filter == null) return new List<SurveyQuestionType>();
            IQueryable<SurveyQuestionTypeDAO> SurveyQuestionTypeDAOs = DataContext.SurveyQuestionType.AsNoTracking();
            SurveyQuestionTypeDAOs = DynamicFilter(SurveyQuestionTypeDAOs, filter);
            SurveyQuestionTypeDAOs = OrFilter(SurveyQuestionTypeDAOs, filter);
            SurveyQuestionTypeDAOs = DynamicOrder(SurveyQuestionTypeDAOs, filter);
            List<SurveyQuestionType> SurveyQuestionTypes = await DynamicSelect(SurveyQuestionTypeDAOs, filter);
            return SurveyQuestionTypes;
        }

        public async Task<SurveyQuestionType> Get(long Id)
        {
            SurveyQuestionType SurveyQuestionType = await DataContext.SurveyQuestionType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new SurveyQuestionType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (SurveyQuestionType == null)
                return null;

            return SurveyQuestionType;
        }
    }
}
