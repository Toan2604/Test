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
    public interface IEntityComponentRepository
    {
        Task<int> Count(EntityComponentFilter EntityComponentFilter);
        Task<int> CountAll(EntityComponentFilter EntityComponentFilter);
        Task<List<EntityComponent>> List(EntityComponentFilter EntityComponentFilter);
        Task<EntityComponent> Get(long Id);
        Task<bool> BulkMerge(List<EntityComponent> EntityComponents);
    }
    public class EntityComponentRepository : IEntityComponentRepository
    {
        private DataContext DataContext;
        public EntityComponentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<EntityComponentDAO> DynamicFilter(IQueryable<EntityComponentDAO> query, EntityComponentFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<EntityComponentDAO> OrFilter(IQueryable<EntityComponentDAO> query, EntityComponentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<EntityComponentDAO> initQuery = query.Where(q => false);
            foreach (EntityComponentFilter EntityComponentFilter in filter.OrFilter)
            {
                IQueryable<EntityComponentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, EntityComponentFilter.Id);
                queryable = queryable.Where(q => q.Code, EntityComponentFilter.Code);
                queryable = queryable.Where(q => q.Name, EntityComponentFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<EntityComponentDAO> DynamicOrder(IQueryable<EntityComponentDAO> query, EntityComponentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case EntityComponentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case EntityComponentOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case EntityComponentOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case EntityComponentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case EntityComponentOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case EntityComponentOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<EntityComponent>> DynamicSelect(IQueryable<EntityComponentDAO> query, EntityComponentFilter filter)
        {
            List<EntityComponent> EntityComponents = await query.Select(q => new EntityComponent()
            {
                Id = filter.Selects.Contains(EntityComponentSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(EntityComponentSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(EntityComponentSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return EntityComponents;
        }

        public async Task<int> Count(EntityComponentFilter filter)
        {
            IQueryable<EntityComponentDAO> EntityComponents = DataContext.EntityComponent.AsNoTracking();
            EntityComponents = DynamicFilter(EntityComponents, filter);
            EntityComponents = OrFilter(EntityComponents, filter);
            return await EntityComponents.CountWithNoLockAsync();
        }

        public async Task<int> CountAll(EntityComponentFilter filter)
        {
            IQueryable<EntityComponentDAO> EntityComponents = DataContext.EntityComponent.AsNoTracking();
            EntityComponents = DynamicFilter(EntityComponents, filter);
            return await EntityComponents.CountWithNoLockAsync();
        }

        public async Task<List<EntityComponent>> List(EntityComponentFilter filter)
        {
            if (filter == null) return new List<EntityComponent>();
            IQueryable<EntityComponentDAO> EntityComponentDAOs = DataContext.EntityComponent.AsNoTracking();
            EntityComponentDAOs = DynamicFilter(EntityComponentDAOs, filter);
            EntityComponentDAOs = OrFilter(EntityComponentDAOs, filter);
            EntityComponentDAOs = DynamicOrder(EntityComponentDAOs, filter);
            List<EntityComponent> EntityComponents = await DynamicSelect(EntityComponentDAOs, filter);
            return EntityComponents;
        }

        public async Task<EntityComponent> Get(long Id)
        {
            EntityComponent EntityComponent = await DataContext.EntityComponent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new EntityComponent()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (EntityComponent == null)
                return null;

            return EntityComponent;
        }

        public async Task<bool> BulkMerge(List<EntityComponent> EntityComponents)
        {
            List<EntityComponentDAO> EntityComponentDAOs = EntityComponents.Select(x => new EntityComponentDAO
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            await DataContext.BulkMergeAsync(EntityComponentDAOs);
            return true;
        }
    }
}
