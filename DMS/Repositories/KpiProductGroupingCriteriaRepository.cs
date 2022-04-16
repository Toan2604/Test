using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IKpiProductGroupingCriteriaRepository
    {
        Task<int> Count(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
        Task<int> CountAll(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
        Task<List<KpiProductGroupingCriteria>> List(KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter);
        Task<List<KpiProductGroupingCriteria>> List(List<long> Ids);
        Task<KpiProductGroupingCriteria> Get(long Id);
        Task<bool> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> BulkMerge(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
        Task<bool> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
    }
    public class KpiProductGroupingCriteriaRepository : IKpiProductGroupingCriteriaRepository
    {
        private DataContext DataContext;
        public KpiProductGroupingCriteriaRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiProductGroupingCriteriaDAO> DynamicFilter(IQueryable<KpiProductGroupingCriteriaDAO> query, KpiProductGroupingCriteriaFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.StatusId == StatusEnum.ACTIVE.Id);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<KpiProductGroupingCriteriaDAO> OrFilter(IQueryable<KpiProductGroupingCriteriaDAO> query, KpiProductGroupingCriteriaFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiProductGroupingCriteriaDAO> initQuery = query.Where(q => false);
            foreach (KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter in filter.OrFilter)
            {
                IQueryable<KpiProductGroupingCriteriaDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiProductGroupingCriteriaDAO> DynamicOrder(IQueryable<KpiProductGroupingCriteriaDAO> query, KpiProductGroupingCriteriaFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingCriteriaOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiProductGroupingCriteriaOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case KpiProductGroupingCriteriaOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingCriteriaOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiProductGroupingCriteriaOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case KpiProductGroupingCriteriaOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiProductGroupingCriteria>> DynamicSelect(IQueryable<KpiProductGroupingCriteriaDAO> query, KpiProductGroupingCriteriaFilter filter)
        {
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await query.Select(q => new KpiProductGroupingCriteria()
            {
                Id = filter.Selects.Contains(KpiProductGroupingCriteriaSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(KpiProductGroupingCriteriaSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(KpiProductGroupingCriteriaSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return KpiProductGroupingCriterias;
        }

        public async Task<int> Count(KpiProductGroupingCriteriaFilter filter)
        {
            IQueryable<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriterias = DataContext.KpiProductGroupingCriteria.AsNoTracking();
            KpiProductGroupingCriterias = DynamicFilter(KpiProductGroupingCriterias, filter);
            KpiProductGroupingCriterias = OrFilter(KpiProductGroupingCriterias, filter);
            return await KpiProductGroupingCriterias.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiProductGroupingCriteriaFilter filter)
        {
            IQueryable<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriterias = DataContext.KpiProductGroupingCriteria.AsNoTracking();
            KpiProductGroupingCriterias = DynamicFilter(KpiProductGroupingCriterias, filter);
            return await KpiProductGroupingCriterias.CountWithNoLockAsync();
        }

        public async Task<List<KpiProductGroupingCriteria>> List(KpiProductGroupingCriteriaFilter filter)
        {
            if (filter == null) return new List<KpiProductGroupingCriteria>();
            IQueryable<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriteriaDAOs = DataContext.KpiProductGroupingCriteria.AsNoTracking();
            KpiProductGroupingCriteriaDAOs = DynamicFilter(KpiProductGroupingCriteriaDAOs, filter);
            KpiProductGroupingCriteriaDAOs = OrFilter(KpiProductGroupingCriteriaDAOs, filter);
            KpiProductGroupingCriteriaDAOs = DynamicOrder(KpiProductGroupingCriteriaDAOs, filter);
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await DynamicSelect(KpiProductGroupingCriteriaDAOs, filter);
            return KpiProductGroupingCriterias;
        }

        public async Task<List<KpiProductGroupingCriteria>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = await DataContext.KpiProductGroupingCriteria.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new KpiProductGroupingCriteria()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();


            return KpiProductGroupingCriterias;
        }

        public async Task<KpiProductGroupingCriteria> Get(long Id)
        {
            KpiProductGroupingCriteria KpiProductGroupingCriteria = await DataContext.KpiProductGroupingCriteria.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new KpiProductGroupingCriteria()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiProductGroupingCriteria == null)
                return null;

            return KpiProductGroupingCriteria;
        }
        public async Task<bool> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            KpiProductGroupingCriteriaDAO KpiProductGroupingCriteriaDAO = new KpiProductGroupingCriteriaDAO();
            KpiProductGroupingCriteriaDAO.Id = KpiProductGroupingCriteria.Id;
            KpiProductGroupingCriteriaDAO.Code = KpiProductGroupingCriteria.Code;
            KpiProductGroupingCriteriaDAO.Name = KpiProductGroupingCriteria.Name;
            DataContext.KpiProductGroupingCriteria.Add(KpiProductGroupingCriteriaDAO);
            await DataContext.SaveChangesAsync();
            KpiProductGroupingCriteria.Id = KpiProductGroupingCriteriaDAO.Id;
            await SaveReference(KpiProductGroupingCriteria);
            return true;
        }

        public async Task<bool> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            KpiProductGroupingCriteriaDAO KpiProductGroupingCriteriaDAO = DataContext.KpiProductGroupingCriteria.Where(x => x.Id == KpiProductGroupingCriteria.Id).FirstOrDefault();
            if (KpiProductGroupingCriteriaDAO == null)
                return false;
            KpiProductGroupingCriteriaDAO.Id = KpiProductGroupingCriteria.Id;
            KpiProductGroupingCriteriaDAO.Code = KpiProductGroupingCriteria.Code;
            KpiProductGroupingCriteriaDAO.Name = KpiProductGroupingCriteria.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiProductGroupingCriteria);
            return true;
        }

        public async Task<bool> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            await DataContext.KpiProductGroupingCriteria.Where(x => x.Id == KpiProductGroupingCriteria.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            List<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriteriaDAOs = new List<KpiProductGroupingCriteriaDAO>();
            foreach (KpiProductGroupingCriteria KpiProductGroupingCriteria in KpiProductGroupingCriterias)
            {
                KpiProductGroupingCriteriaDAO KpiProductGroupingCriteriaDAO = new KpiProductGroupingCriteriaDAO();
                KpiProductGroupingCriteriaDAO.Id = KpiProductGroupingCriteria.Id;
                KpiProductGroupingCriteriaDAO.Code = KpiProductGroupingCriteria.Code;
                KpiProductGroupingCriteriaDAO.Name = KpiProductGroupingCriteria.Name;
                KpiProductGroupingCriteriaDAOs.Add(KpiProductGroupingCriteriaDAO);
            }
            await DataContext.BulkMergeAsync(KpiProductGroupingCriteriaDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            List<long> Ids = KpiProductGroupingCriterias.Select(x => x.Id).ToList();
            await DataContext.KpiProductGroupingCriteria
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
        }

    }
}
