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
    public interface IWorkflowParameterRepository
    {
        Task<int> Count(WorkflowParameterFilter WorkflowParameterFilter);
        Task<int> CountAll(WorkflowParameterFilter WorkflowParameterFilter);
        Task<List<WorkflowParameter>> List(WorkflowParameterFilter WorkflowParameterFilter);
        Task<WorkflowParameter> Get(long Id);
    }
    public class WorkflowParameterRepository : IWorkflowParameterRepository
    {
        private DataContext DataContext;
        public WorkflowParameterRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<WorkflowParameterDAO> DynamicFilter(IQueryable<WorkflowParameterDAO> query, WorkflowParameterFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.WorkflowTypeId, filter.WorkflowTypeId);
            query = query.Where(q => q.WorkflowParameterTypeId, filter.WorkflowParameterTypeId);
            return query;
        }

        private IQueryable<WorkflowParameterDAO> OrFilter(IQueryable<WorkflowParameterDAO> query, WorkflowParameterFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkflowParameterDAO> initQuery = query.Where(q => false);
            foreach (WorkflowParameterFilter WorkflowParameterFilter in filter.OrFilter)
            {
                IQueryable<WorkflowParameterDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkflowParameterFilter.Id);
                queryable = queryable.Where(q => q.Code, WorkflowParameterFilter.Code);
                queryable = queryable.Where(q => q.Name, WorkflowParameterFilter.Name);
                queryable = queryable.Where(q => q.WorkflowTypeId, WorkflowParameterFilter.WorkflowTypeId);
                queryable = queryable.Where(q => q.WorkflowParameterTypeId, WorkflowParameterFilter.WorkflowParameterTypeId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WorkflowParameterDAO> DynamicOrder(IQueryable<WorkflowParameterDAO> query, WorkflowParameterFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowParameterOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkflowParameterOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WorkflowParameterOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WorkflowParameterOrder.WorkflowType:
                            query = query.OrderBy(q => q.WorkflowTypeId);
                            break;
                        case WorkflowParameterOrder.WorkflowParameterType:
                            query = query.OrderBy(q => q.WorkflowParameterTypeId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowParameterOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkflowParameterOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WorkflowParameterOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WorkflowParameterOrder.WorkflowType:
                            query = query.OrderByDescending(q => q.WorkflowTypeId);
                            break;
                        case WorkflowParameterOrder.WorkflowParameterType:
                            query = query.OrderByDescending(q => q.WorkflowParameterTypeId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<WorkflowParameter>> DynamicSelect(IQueryable<WorkflowParameterDAO> query, WorkflowParameterFilter filter)
        {
            List<WorkflowParameter> WorkflowParameters = await query.Select(q => new WorkflowParameter()
            {
                Id = filter.Selects.Contains(WorkflowParameterSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WorkflowParameterSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WorkflowParameterSelect.Name) ? q.Name : default(string),
                WorkflowParameterTypeId = filter.Selects.Contains(WorkflowParameterSelect.WorkflowParameterType) ? q.WorkflowParameterTypeId : default(long),
                WorkflowParameterType = filter.Selects.Contains(WorkflowParameterSelect.WorkflowParameterType) && q.WorkflowParameterType != null ? new WorkflowParameterType
                {
                    Id = q.WorkflowParameterType.Id,
                    Code = q.WorkflowParameterType.Code,
                    Name = q.WorkflowParameterType.Name,
                } : null,
                WorkflowTypeId = filter.Selects.Contains(WorkflowParameterSelect.WorkflowType) ? q.WorkflowTypeId : default(long),
                WorkflowType = filter.Selects.Contains(WorkflowParameterSelect.WorkflowType) && q.WorkflowType != null ? new WorkflowType
                {
                    Id = q.WorkflowType.Id,
                    Code = q.WorkflowType.Code,
                    Name = q.WorkflowType.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return WorkflowParameters;
        }

        public async Task<int> Count(WorkflowParameterFilter filter)
        {
            IQueryable<WorkflowParameterDAO> WorkflowParameters = DataContext.WorkflowParameter.AsNoTracking();
            WorkflowParameters = DynamicFilter(WorkflowParameters, filter);
            WorkflowParameters = OrFilter(WorkflowParameters, filter);
            return await WorkflowParameters.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(WorkflowParameterFilter filter)
        {
            IQueryable<WorkflowParameterDAO> WorkflowParameters = DataContext.WorkflowParameter.AsNoTracking();
            WorkflowParameters = DynamicFilter(WorkflowParameters, filter);
            return await WorkflowParameters.CountWithNoLockAsync();
        }

        public async Task<List<WorkflowParameter>> List(WorkflowParameterFilter filter)
        {
            if (filter == null) return new List<WorkflowParameter>();
            IQueryable<WorkflowParameterDAO> WorkflowParameterDAOs = DataContext.WorkflowParameter.AsNoTracking();
            WorkflowParameterDAOs = DynamicFilter(WorkflowParameterDAOs, filter);
            WorkflowParameterDAOs = OrFilter(WorkflowParameterDAOs, filter);
            WorkflowParameterDAOs = DynamicOrder(WorkflowParameterDAOs, filter);
            List<WorkflowParameter> WorkflowParameters = await DynamicSelect(WorkflowParameterDAOs, filter);
            return WorkflowParameters;
        }

        public async Task<WorkflowParameter> Get(long Id)
        {
            WorkflowParameter WorkflowParameter = await DataContext.WorkflowParameter.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new WorkflowParameter()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                WorkflowTypeId = x.WorkflowTypeId,
                WorkflowParameterTypeId = x.WorkflowParameterTypeId,
                WorkflowParameterType = x.WorkflowParameterType == null ? null : new WorkflowParameterType
                {
                    Id = x.WorkflowParameterType.Id,
                    Code = x.WorkflowParameterType.Code,
                    Name = x.WorkflowParameterType.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (WorkflowParameter == null)
                return null;

            return WorkflowParameter;
        }
    }
}
