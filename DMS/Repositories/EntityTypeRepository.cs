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
    public interface IEntityTypeRepository
    {
        Task<int> Count(EntityTypeFilter EntityTypeFilter);
        Task<int> CountAll(EntityTypeFilter EntityTypeFilter);
        Task<List<EntityType>> List(EntityTypeFilter EntityTypeFilter);
        Task<EntityType> Get(long Id);
        Task<bool> BulkMerge(List<EntityType> EntityTypes);
    }
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private DataContext DataContext;
        public EntityTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<EntityTypeDAO> DynamicFilter(IQueryable<EntityTypeDAO> query, EntityTypeFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<EntityTypeDAO> OrFilter(IQueryable<EntityTypeDAO> query, EntityTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<EntityTypeDAO> initQuery = query.Where(q => false);
            foreach (EntityTypeFilter EntityTypeFilter in filter.OrFilter)
            {
                IQueryable<EntityTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, EntityTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, EntityTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, EntityTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<EntityTypeDAO> DynamicOrder(IQueryable<EntityTypeDAO> query, EntityTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case EntityTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case EntityTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case EntityTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case EntityTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case EntityTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case EntityTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<EntityType>> DynamicSelect(IQueryable<EntityTypeDAO> query, EntityTypeFilter filter)
        {
            List<EntityType> EntityTypes = await query.Select(q => new EntityType()
            {
                Id = filter.Selects.Contains(EntityTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(EntityTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(EntityTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return EntityTypes;
        }

        public async Task<int> Count(EntityTypeFilter filter)
        {
            IQueryable<EntityTypeDAO> EntityTypes = DataContext.EntityType.AsNoTracking();
            EntityTypes = DynamicFilter(EntityTypes, filter);
            EntityTypes = OrFilter(EntityTypes, filter);
            return await EntityTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(EntityTypeFilter filter)
        {
            IQueryable<EntityTypeDAO> EntityTypes = DataContext.EntityType.AsNoTracking();
            EntityTypes = DynamicFilter(EntityTypes, filter);
            return await EntityTypes.CountWithNoLockAsync();
        }

        public async Task<List<EntityType>> List(EntityTypeFilter filter)
        {
            if (filter == null) return new List<EntityType>();
            IQueryable<EntityTypeDAO> EntityTypeDAOs = DataContext.EntityType.AsNoTracking();
            EntityTypeDAOs = DynamicFilter(EntityTypeDAOs, filter);
            EntityTypeDAOs = OrFilter(EntityTypeDAOs, filter);
            EntityTypeDAOs = DynamicOrder(EntityTypeDAOs, filter);
            List<EntityType> EntityTypes = await DynamicSelect(EntityTypeDAOs, filter);
            return EntityTypes;
        }

        public async Task<EntityType> Get(long Id)
        {
            EntityType EntityType = await DataContext.EntityType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new EntityType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (EntityType == null)
                return null;

            return EntityType;
        }

        public async Task<bool> BulkMerge(List<EntityType> EntityTypes)
        {
            List<EntityTypeDAO> EntityTypeDAOs = EntityTypes.Select(x => new EntityTypeDAO
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            await DataContext.BulkMergeAsync(EntityTypeDAOs);
            return true;
        }
    }
}
