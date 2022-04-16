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
    public interface IKpiItemTypeRepository
    {
        Task<int> Count(KpiItemTypeFilter KpiItemTypeFilter);
        Task<int> CountAll(KpiItemTypeFilter KpiItemTypeFilter);
        Task<List<KpiItemType>> List(KpiItemTypeFilter KpiItemTypeFilter);
        Task<List<KpiItemType>> List(List<long> Ids);
        Task<KpiItemType> Get(long Id);
        Task<bool> Create(KpiItemType KpiItemType);
        Task<bool> Update(KpiItemType KpiItemType);
        Task<bool> Delete(KpiItemType KpiItemType);
        Task<bool> BulkMerge(List<KpiItemType> KpiItemTypes);
        Task<bool> BulkDelete(List<KpiItemType> KpiItemTypes);
    }
    public class KpiItemTypeRepository : IKpiItemTypeRepository
    {
        private DataContext DataContext;
        public KpiItemTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiItemTypeDAO> DynamicFilter(IQueryable<KpiItemTypeDAO> query, KpiItemTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<KpiItemTypeDAO> OrFilter(IQueryable<KpiItemTypeDAO> query, KpiItemTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiItemTypeDAO> initQuery = query.Where(q => false);
            foreach (KpiItemTypeFilter KpiItemTypeFilter in filter.OrFilter)
            {
                IQueryable<KpiItemTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, KpiItemTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, KpiItemTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, KpiItemTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiItemTypeDAO> DynamicOrder(IQueryable<KpiItemTypeDAO> query, KpiItemTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiItemTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiItemTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case KpiItemTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiItemTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiItemTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case KpiItemTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiItemType>> DynamicSelect(IQueryable<KpiItemTypeDAO> query, KpiItemTypeFilter filter)
        {
            List<KpiItemType> KpiItemTypes = await query.Select(q => new KpiItemType()
            {
                Id = filter.Selects.Contains(KpiItemTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(KpiItemTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(KpiItemTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return KpiItemTypes;
        }

        public async Task<int> Count(KpiItemTypeFilter filter)
        {
            IQueryable<KpiItemTypeDAO> KpiItemTypes = DataContext.KpiItemType.AsNoTracking();
            KpiItemTypes = DynamicFilter(KpiItemTypes, filter);
            KpiItemTypes = OrFilter(KpiItemTypes, filter);
            return await KpiItemTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiItemTypeFilter filter)
        {
            IQueryable<KpiItemTypeDAO> KpiItemTypes = DataContext.KpiItemType.AsNoTracking();
            KpiItemTypes = DynamicFilter(KpiItemTypes, filter);
            return await KpiItemTypes.CountWithNoLockAsync();
        }

        public async Task<List<KpiItemType>> List(KpiItemTypeFilter filter)
        {
            if (filter == null) return new List<KpiItemType>();
            IQueryable<KpiItemTypeDAO> KpiItemTypeDAOs = DataContext.KpiItemType.AsNoTracking();
            KpiItemTypeDAOs = DynamicFilter(KpiItemTypeDAOs, filter);
            KpiItemTypeDAOs = OrFilter(KpiItemTypeDAOs, filter);
            KpiItemTypeDAOs = DynamicOrder(KpiItemTypeDAOs, filter);
            List<KpiItemType> KpiItemTypes = await DynamicSelect(KpiItemTypeDAOs, filter);
            return KpiItemTypes;
        }

        public async Task<List<KpiItemType>> List(List<long> Ids)
        {
            List<KpiItemType> KpiItemTypes = await DataContext.KpiItemType.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new KpiItemType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();


            return KpiItemTypes;
        }

        public async Task<KpiItemType> Get(long Id)
        {
            KpiItemType KpiItemType = await DataContext.KpiItemType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new KpiItemType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiItemType == null)
                return null;

            return KpiItemType;
        }
        public async Task<bool> Create(KpiItemType KpiItemType)
        {
            KpiItemTypeDAO KpiItemTypeDAO = new KpiItemTypeDAO();
            KpiItemTypeDAO.Id = KpiItemType.Id;
            KpiItemTypeDAO.Code = KpiItemType.Code;
            KpiItemTypeDAO.Name = KpiItemType.Name;
            DataContext.KpiItemType.Add(KpiItemTypeDAO);
            await DataContext.SaveChangesAsync();
            KpiItemType.Id = KpiItemTypeDAO.Id;
            await SaveReference(KpiItemType);
            return true;
        }

        public async Task<bool> Update(KpiItemType KpiItemType)
        {
            KpiItemTypeDAO KpiItemTypeDAO = DataContext.KpiItemType.Where(x => x.Id == KpiItemType.Id).FirstOrDefault();
            if (KpiItemTypeDAO == null)
                return false;
            KpiItemTypeDAO.Id = KpiItemType.Id;
            KpiItemTypeDAO.Code = KpiItemType.Code;
            KpiItemTypeDAO.Name = KpiItemType.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiItemType);
            return true;
        }

        public async Task<bool> Delete(KpiItemType KpiItemType)
        {
            await DataContext.KpiItemType.Where(x => x.Id == KpiItemType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiItemType> KpiItemTypes)
        {
            List<KpiItemTypeDAO> KpiItemTypeDAOs = new List<KpiItemTypeDAO>();
            foreach (KpiItemType KpiItemType in KpiItemTypes)
            {
                KpiItemTypeDAO KpiItemTypeDAO = new KpiItemTypeDAO();
                KpiItemTypeDAO.Id = KpiItemType.Id;
                KpiItemTypeDAO.Code = KpiItemType.Code;
                KpiItemTypeDAO.Name = KpiItemType.Name;
                KpiItemTypeDAOs.Add(KpiItemTypeDAO);
            }
            await DataContext.BulkMergeAsync(KpiItemTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiItemType> KpiItemTypes)
        {
            List<long> Ids = KpiItemTypes.Select(x => x.Id).ToList();
            await DataContext.KpiItemType
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiItemType KpiItemType)
        {
        }

    }
}
