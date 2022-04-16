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
    public interface IKpiProductGroupingTypeRepository
    {
        Task<int> Count(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
        Task<int> CountAll(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
        Task<List<KpiProductGroupingType>> List(KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter);
        Task<List<KpiProductGroupingType>> List(List<long> Ids);
        Task<KpiProductGroupingType> Get(long Id);
        Task<bool> Create(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> Update(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> Delete(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> BulkMerge(List<KpiProductGroupingType> KpiProductGroupingTypes);
        Task<bool> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes);
    }
    public class KpiProductGroupingTypeRepository : IKpiProductGroupingTypeRepository
    {
        private DataContext DataContext;
        public KpiProductGroupingTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiProductGroupingTypeDAO> DynamicFilter(IQueryable<KpiProductGroupingTypeDAO> query, KpiProductGroupingTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<KpiProductGroupingTypeDAO> OrFilter(IQueryable<KpiProductGroupingTypeDAO> query, KpiProductGroupingTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiProductGroupingTypeDAO> initQuery = query.Where(q => false);
            foreach (KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter in filter.OrFilter)
            {
                IQueryable<KpiProductGroupingTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiProductGroupingTypeDAO> DynamicOrder(IQueryable<KpiProductGroupingTypeDAO> query, KpiProductGroupingTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiProductGroupingTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case KpiProductGroupingTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiProductGroupingTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case KpiProductGroupingTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiProductGroupingType>> DynamicSelect(IQueryable<KpiProductGroupingTypeDAO> query, KpiProductGroupingTypeFilter filter)
        {
            List<KpiProductGroupingType> KpiProductGroupingTypes = await query.Select(q => new KpiProductGroupingType()
            {
                Id = filter.Selects.Contains(KpiProductGroupingTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(KpiProductGroupingTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(KpiProductGroupingTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return KpiProductGroupingTypes;
        }

        public async Task<int> Count(KpiProductGroupingTypeFilter filter)
        {
            IQueryable<KpiProductGroupingTypeDAO> KpiProductGroupingTypes = DataContext.KpiProductGroupingType.AsNoTracking();
            KpiProductGroupingTypes = DynamicFilter(KpiProductGroupingTypes, filter);
            KpiProductGroupingTypes = OrFilter(KpiProductGroupingTypes, filter);
            return await KpiProductGroupingTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiProductGroupingTypeFilter filter)
        {
            IQueryable<KpiProductGroupingTypeDAO> KpiProductGroupingTypes = DataContext.KpiProductGroupingType.AsNoTracking();
            KpiProductGroupingTypes = DynamicFilter(KpiProductGroupingTypes, filter);
            return await KpiProductGroupingTypes.CountWithNoLockAsync();
        }

        public async Task<List<KpiProductGroupingType>> List(KpiProductGroupingTypeFilter filter)
        {
            if (filter == null) return new List<KpiProductGroupingType>();
            IQueryable<KpiProductGroupingTypeDAO> KpiProductGroupingTypeDAOs = DataContext.KpiProductGroupingType.AsNoTracking();
            KpiProductGroupingTypeDAOs = DynamicFilter(KpiProductGroupingTypeDAOs, filter);
            KpiProductGroupingTypeDAOs = OrFilter(KpiProductGroupingTypeDAOs, filter);
            KpiProductGroupingTypeDAOs = DynamicOrder(KpiProductGroupingTypeDAOs, filter);
            List<KpiProductGroupingType> KpiProductGroupingTypes = await DynamicSelect(KpiProductGroupingTypeDAOs, filter);
            return KpiProductGroupingTypes;
        }

        public async Task<List<KpiProductGroupingType>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<KpiProductGroupingType> KpiProductGroupingTypes = await DataContext.KpiProductGroupingType.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new KpiProductGroupingType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            return KpiProductGroupingTypes;
        }

        public async Task<KpiProductGroupingType> Get(long Id)
        {
            KpiProductGroupingType KpiProductGroupingType = await DataContext.KpiProductGroupingType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new KpiProductGroupingType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiProductGroupingType == null)
                return null;

            return KpiProductGroupingType;
        }
        public async Task<bool> Create(KpiProductGroupingType KpiProductGroupingType)
        {
            KpiProductGroupingTypeDAO KpiProductGroupingTypeDAO = new KpiProductGroupingTypeDAO();
            KpiProductGroupingTypeDAO.Id = KpiProductGroupingType.Id;
            KpiProductGroupingTypeDAO.Code = KpiProductGroupingType.Code;
            KpiProductGroupingTypeDAO.Name = KpiProductGroupingType.Name;
            DataContext.KpiProductGroupingType.Add(KpiProductGroupingTypeDAO);
            await DataContext.SaveChangesAsync();
            KpiProductGroupingType.Id = KpiProductGroupingTypeDAO.Id;
            await SaveReference(KpiProductGroupingType);
            return true;
        }

        public async Task<bool> Update(KpiProductGroupingType KpiProductGroupingType)
        {
            KpiProductGroupingTypeDAO KpiProductGroupingTypeDAO = DataContext.KpiProductGroupingType.Where(x => x.Id == KpiProductGroupingType.Id).FirstOrDefault();
            if (KpiProductGroupingTypeDAO == null)
                return false;
            KpiProductGroupingTypeDAO.Id = KpiProductGroupingType.Id;
            KpiProductGroupingTypeDAO.Code = KpiProductGroupingType.Code;
            KpiProductGroupingTypeDAO.Name = KpiProductGroupingType.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiProductGroupingType);
            return true;
        }

        public async Task<bool> Delete(KpiProductGroupingType KpiProductGroupingType)
        {
            await DataContext.KpiProductGroupingType.Where(x => x.Id == KpiProductGroupingType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            List<KpiProductGroupingTypeDAO> KpiProductGroupingTypeDAOs = new List<KpiProductGroupingTypeDAO>();
            foreach (KpiProductGroupingType KpiProductGroupingType in KpiProductGroupingTypes)
            {
                KpiProductGroupingTypeDAO KpiProductGroupingTypeDAO = new KpiProductGroupingTypeDAO();
                KpiProductGroupingTypeDAO.Id = KpiProductGroupingType.Id;
                KpiProductGroupingTypeDAO.Code = KpiProductGroupingType.Code;
                KpiProductGroupingTypeDAO.Name = KpiProductGroupingType.Name;
                KpiProductGroupingTypeDAOs.Add(KpiProductGroupingTypeDAO);
            }
            await DataContext.BulkMergeAsync(KpiProductGroupingTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            List<long> Ids = KpiProductGroupingTypes.Select(x => x.Id).ToList();
            await DataContext.KpiProductGroupingType
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiProductGroupingType KpiProductGroupingType)
        {
        }

    }
}
