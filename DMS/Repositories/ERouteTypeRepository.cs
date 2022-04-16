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
    public interface IERouteTypeRepository
    {
        Task<int> Count(ERouteTypeFilter ERouteTypeFilter);
        Task<int> CountAll(ERouteTypeFilter ERouteTypeFilter);
        Task<List<ERouteType>> List(ERouteTypeFilter ERouteTypeFilter);
        Task<ERouteType> Get(long Id);
        Task<bool> Create(ERouteType ERouteType);
        Task<bool> Update(ERouteType ERouteType);
        Task<bool> Delete(ERouteType ERouteType);
        Task<bool> BulkMerge(List<ERouteType> ERouteTypes);
        Task<bool> BulkDelete(List<ERouteType> ERouteTypes);
    }
    public class ERouteTypeRepository : IERouteTypeRepository
    {
        private DataContext DataContext;
        public ERouteTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ERouteTypeDAO> DynamicFilter(IQueryable<ERouteTypeDAO> query, ERouteTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<ERouteTypeDAO> OrFilter(IQueryable<ERouteTypeDAO> query, ERouteTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ERouteTypeDAO> initQuery = query.Where(q => false);
            foreach (ERouteTypeFilter ERouteTypeFilter in filter.OrFilter)
            {
                IQueryable<ERouteTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ERouteTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, ERouteTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, ERouteTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ERouteTypeDAO> DynamicOrder(IQueryable<ERouteTypeDAO> query, ERouteTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ERouteTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ERouteTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ERouteTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ERouteTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ERouteTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ERouteTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ERouteType>> DynamicSelect(IQueryable<ERouteTypeDAO> query, ERouteTypeFilter filter)
        {
            List<ERouteType> ERouteTypes = await query.Select(q => new ERouteType()
            {
                Id = filter.Selects.Contains(ERouteTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ERouteTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ERouteTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return ERouteTypes;
        }

        public async Task<int> Count(ERouteTypeFilter filter)
        {
            IQueryable<ERouteTypeDAO> ERouteTypes = DataContext.ERouteType.AsNoTracking();
            ERouteTypes = DynamicFilter(ERouteTypes, filter);
            ERouteTypes = OrFilter(ERouteTypes, filter);
            return await ERouteTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ERouteTypeFilter filter)
        {
            IQueryable<ERouteTypeDAO> ERouteTypes = DataContext.ERouteType.AsNoTracking();
            ERouteTypes = DynamicFilter(ERouteTypes, filter);
            return await ERouteTypes.CountWithNoLockAsync();
        }

        public async Task<List<ERouteType>> List(ERouteTypeFilter filter)
        {
            if (filter == null) return new List<ERouteType>();
            IQueryable<ERouteTypeDAO> ERouteTypeDAOs = DataContext.ERouteType.AsNoTracking();
            ERouteTypeDAOs = DynamicFilter(ERouteTypeDAOs, filter);
            ERouteTypeDAOs = OrFilter(ERouteTypeDAOs, filter);
            ERouteTypeDAOs = DynamicOrder(ERouteTypeDAOs, filter);
            List<ERouteType> ERouteTypes = await DynamicSelect(ERouteTypeDAOs, filter);
            return ERouteTypes;
        }

        public async Task<ERouteType> Get(long Id)
        {
            ERouteType ERouteType = await DataContext.ERouteType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ERouteType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (ERouteType == null)
                return null;

            return ERouteType;
        }
        public async Task<bool> Create(ERouteType ERouteType)
        {
            ERouteTypeDAO ERouteTypeDAO = new ERouteTypeDAO();
            ERouteTypeDAO.Id = ERouteType.Id;
            ERouteTypeDAO.Code = ERouteType.Code;
            ERouteTypeDAO.Name = ERouteType.Name;
            DataContext.ERouteType.Add(ERouteTypeDAO);
            await DataContext.SaveChangesAsync();
            ERouteType.Id = ERouteTypeDAO.Id;
            await SaveReference(ERouteType);
            return true;
        }

        public async Task<bool> Update(ERouteType ERouteType)
        {
            ERouteTypeDAO ERouteTypeDAO = DataContext.ERouteType.Where(x => x.Id == ERouteType.Id).FirstOrDefault();
            if (ERouteTypeDAO == null)
                return false;
            ERouteTypeDAO.Id = ERouteType.Id;
            ERouteTypeDAO.Code = ERouteType.Code;
            ERouteTypeDAO.Name = ERouteType.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(ERouteType);
            return true;
        }

        public async Task<bool> Delete(ERouteType ERouteType)
        {
            await DataContext.ERouteType.Where(x => x.Id == ERouteType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<ERouteType> ERouteTypes)
        {
            List<ERouteTypeDAO> ERouteTypeDAOs = new List<ERouteTypeDAO>();
            foreach (ERouteType ERouteType in ERouteTypes)
            {
                ERouteTypeDAO ERouteTypeDAO = new ERouteTypeDAO();
                ERouteTypeDAO.Id = ERouteType.Id;
                ERouteTypeDAO.Code = ERouteType.Code;
                ERouteTypeDAO.Name = ERouteType.Name;
                ERouteTypeDAOs.Add(ERouteTypeDAO);
            }
            await DataContext.BulkMergeAsync(ERouteTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ERouteType> ERouteTypes)
        {
            List<long> Ids = ERouteTypes.Select(x => x.Id).ToList();
            await DataContext.ERouteType
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(ERouteType ERouteType)
        {
        }

    }
}
