using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IWorkflowParameterTypeRepository
    {
        Task<int> Count(WorkflowParameterTypeFilter WorkflowParameterTypeFilter);
        Task<int> CountAll(WorkflowParameterTypeFilter WorkflowParameterTypeFilter);
        Task<List<WorkflowParameterType>> List(WorkflowParameterTypeFilter WorkflowParameterTypeFilter);
        Task<WorkflowParameterType> Get(long Id);
    }
    public class WorkflowParameterTypeRepository : IWorkflowParameterTypeRepository
    {
        private DataContext DataContext;
        public WorkflowParameterTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<WorkflowParameterTypeDAO> DynamicFilter(IQueryable<WorkflowParameterTypeDAO> query, WorkflowParameterTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<WorkflowParameterTypeDAO> OrFilter(IQueryable<WorkflowParameterTypeDAO> query, WorkflowParameterTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkflowParameterTypeDAO> initQuery = query.Where(q => false);
            foreach (WorkflowParameterTypeFilter WorkflowParameterTypeFilter in filter.OrFilter)
            {
                IQueryable<WorkflowParameterTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkflowParameterTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, WorkflowParameterTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, WorkflowParameterTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WorkflowParameterTypeDAO> DynamicOrder(IQueryable<WorkflowParameterTypeDAO> query, WorkflowParameterTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowParameterTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkflowParameterTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WorkflowParameterTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowParameterTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkflowParameterTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WorkflowParameterTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<WorkflowParameterType>> DynamicSelect(IQueryable<WorkflowParameterTypeDAO> query, WorkflowParameterTypeFilter filter)
        {
            List<WorkflowParameterType> WorkflowParameterTypes = await query.Select(q => new WorkflowParameterType()
            {
                Id = filter.Selects.Contains(WorkflowParameterTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WorkflowParameterTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WorkflowParameterTypeSelect.Name) ? q.Name : default(string),
            }).AsNoTracking().ToListAsync();
            return WorkflowParameterTypes;
        }

        public async Task<int> Count(WorkflowParameterTypeFilter filter)
        {
            IQueryable<WorkflowParameterTypeDAO> WorkflowParameterTypes = DataContext.WorkflowParameterType;
            WorkflowParameterTypes = DynamicFilter(WorkflowParameterTypes, filter);
            WorkflowParameterTypes = OrFilter(WorkflowParameterTypes, filter);
            return await WorkflowParameterTypes.CountAsync();
        }
        public async Task<int> CountAll(WorkflowParameterTypeFilter filter)
        {
            IQueryable<WorkflowParameterTypeDAO> WorkflowParameterTypes = DataContext.WorkflowParameterType;
            WorkflowParameterTypes = DynamicFilter(WorkflowParameterTypes, filter);
            return await WorkflowParameterTypes.CountAsync();
        }

        public async Task<List<WorkflowParameterType>> List(WorkflowParameterTypeFilter filter)
        {
            if (filter == null) return new List<WorkflowParameterType>();
            IQueryable<WorkflowParameterTypeDAO> WorkflowParameterTypeDAOs = DataContext.WorkflowParameterType;
            WorkflowParameterTypeDAOs = DynamicFilter(WorkflowParameterTypeDAOs, filter);
            WorkflowParameterTypeDAOs = OrFilter(WorkflowParameterTypeDAOs, filter);
            WorkflowParameterTypeDAOs = DynamicOrder(WorkflowParameterTypeDAOs, filter);
            List<WorkflowParameterType> WorkflowParameterTypes = await DynamicSelect(WorkflowParameterTypeDAOs, filter);
            return WorkflowParameterTypes;
        }

        public async Task<WorkflowParameterType> Get(long Id)
        {
            WorkflowParameterType WorkflowParameterType = await DataContext.WorkflowParameterType.Where(x => x.Id == Id).Select(x => new WorkflowParameterType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).AsNoTracking().FirstOrDefaultAsync();

            if (WorkflowParameterType == null)
                return null;

            return WorkflowParameterType;
        }
    }
}
