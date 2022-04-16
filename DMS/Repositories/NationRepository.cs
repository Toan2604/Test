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
    public interface INationRepository
    {
        Task<int> Count(NationFilter NationFilter);
        Task<int> CountAll(NationFilter NationFilter);
        Task<List<Nation>> List(NationFilter NationFilter);
        Task<Nation> Get(long Id);
        Task<bool> BulkMerge(List<Nation> Nations);
    }
    public class NationRepository : INationRepository
    {
        private DataContext DataContext;
        public NationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<NationDAO> DynamicFilter(IQueryable<NationDAO> query, NationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<NationDAO> OrFilter(IQueryable<NationDAO> query, NationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<NationDAO> initQuery = query.Where(q => false);
            foreach (NationFilter NationFilter in filter.OrFilter)
            {
                IQueryable<NationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, NationFilter.Id);
                queryable = queryable.Where(q => q.Name, NationFilter.Name);
                queryable = queryable.Where(q => q.Priority, NationFilter.Priority);
                queryable = queryable.Where(q => q.StatusId, NationFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<NationDAO> DynamicOrder(IQueryable<NationDAO> query, NationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case NationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case NationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case NationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case NationOrder.Priority:
                            query = query.OrderBy(q => q.Priority == null).ThenBy(x => x.Priority);
                            break;
                        case NationOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case NationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case NationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case NationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case NationOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority == null).ThenByDescending(x => x.Priority);
                            break;
                        case NationOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Nation>> DynamicSelect(IQueryable<NationDAO> query, NationFilter filter)
        {
            List<Nation> Nations = await query.Select(q => new Nation()
            {
                Id = filter.Selects.Contains(NationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(NationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(NationSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(NationSelect.Priority) ? q.Priority : default(long?),
                StatusId = filter.Selects.Contains(NationSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(NationSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(NationSelect.RowId) ? q.RowId : default(Guid)
            }).ToListWithNoLockAsync();
            return Nations;
        }

        public async Task<int> Count(NationFilter filter)
        {
            IQueryable<NationDAO> Nations = DataContext.Nation;
            Nations = DynamicFilter(Nations, filter);
            Nations = OrFilter(Nations, filter);
            return await Nations.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(NationFilter filter)
        {
            IQueryable<NationDAO> Nations = DataContext.Nation;
            Nations = DynamicFilter(Nations, filter);
            return await Nations.CountWithNoLockAsync();
        }

        public async Task<List<Nation>> List(NationFilter filter)
        {
            if (filter == null) return new List<Nation>();
            IQueryable<NationDAO> NationDAOs = DataContext.Nation;
            NationDAOs = DynamicFilter(NationDAOs, filter);
            NationDAOs = OrFilter(NationDAOs, filter);
            NationDAOs = DynamicOrder(NationDAOs, filter);
            List<Nation> Nations = await DynamicSelect(NationDAOs, filter);
            return Nations;
        }

        public async Task<Nation> Get(long Id)
        {
            Nation Nation = await DataContext.Nation.Where(x => x.Id == Id).AsNoTracking().Select(x => new Nation()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Nation == null)
                return null;

            return Nation;
        }
        public async Task<bool> BulkMerge(List<Nation> Nations)
        {
            List<NationDAO> NationDAOs = Nations.Select(x => new NationDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Used = x.Used,
                Priority = x.Priority,
                RowId = x.RowId,
                StatusId = x.StatusId,
            }).ToList();
            await DataContext.BulkMergeAsync(NationDAOs);
            return true;
        }
    }
}
