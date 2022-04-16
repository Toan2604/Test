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
    public interface IKpiCriteriaItemRepository
    {
        Task<int> Count(KpiCriteriaItemFilter KpiCriteriaItemFilter);
        Task<int> CountAll(KpiCriteriaItemFilter KpiCriteriaItemFilter);
        Task<List<KpiCriteriaItem>> List(KpiCriteriaItemFilter KpiCriteriaItemFilter);
        Task<KpiCriteriaItem> Get(long Id);
        Task<bool> Create(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> Update(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> Delete(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> BulkMerge(List<KpiCriteriaItem> KpiCriteriaItems);
        Task<bool> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems);
    }
    public class KpiCriteriaItemRepository : IKpiCriteriaItemRepository
    {
        private DataContext DataContext;
        public KpiCriteriaItemRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiCriteriaItemDAO> DynamicFilter(IQueryable<KpiCriteriaItemDAO> query, KpiCriteriaItemFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.StatusId == StatusEnum.ACTIVE.Id);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<KpiCriteriaItemDAO> OrFilter(IQueryable<KpiCriteriaItemDAO> query, KpiCriteriaItemFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiCriteriaItemDAO> initQuery = query.Where(q => false);
            foreach (KpiCriteriaItemFilter KpiCriteriaItemFilter in filter.OrFilter)
            {
                IQueryable<KpiCriteriaItemDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, KpiCriteriaItemFilter.Id);
                queryable = queryable.Where(q => q.Code, KpiCriteriaItemFilter.Code);
                queryable = queryable.Where(q => q.Name, KpiCriteriaItemFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiCriteriaItemDAO> DynamicOrder(IQueryable<KpiCriteriaItemDAO> query, KpiCriteriaItemFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiCriteriaItemOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiCriteriaItemOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case KpiCriteriaItemOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiCriteriaItemOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiCriteriaItemOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case KpiCriteriaItemOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiCriteriaItem>> DynamicSelect(IQueryable<KpiCriteriaItemDAO> query, KpiCriteriaItemFilter filter)
        {
            List<KpiCriteriaItem> KpiCriteriaItems = await query.Select(q => new KpiCriteriaItem()
            {
                Id = filter.Selects.Contains(KpiCriteriaItemSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(KpiCriteriaItemSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(KpiCriteriaItemSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return KpiCriteriaItems;
        }

        public async Task<int> Count(KpiCriteriaItemFilter filter)
        {
            IQueryable<KpiCriteriaItemDAO> KpiCriteriaItems = DataContext.KpiCriteriaItem.AsNoTracking();
            KpiCriteriaItems = DynamicFilter(KpiCriteriaItems, filter);
            KpiCriteriaItems = OrFilter(KpiCriteriaItems, filter);
            return await KpiCriteriaItems.CountWithNoLockAsync();
        }

        public async Task<int> CountAll(KpiCriteriaItemFilter filter)
        {
            IQueryable<KpiCriteriaItemDAO> KpiCriteriaItems = DataContext.KpiCriteriaItem.AsNoTracking();
            KpiCriteriaItems = DynamicFilter(KpiCriteriaItems, filter);
            return await KpiCriteriaItems.CountWithNoLockAsync();
        }
        public async Task<List<KpiCriteriaItem>> List(KpiCriteriaItemFilter filter)
        {
            if (filter == null) return new List<KpiCriteriaItem>();
            IQueryable<KpiCriteriaItemDAO> KpiCriteriaItemDAOs = DataContext.KpiCriteriaItem.AsNoTracking();
            KpiCriteriaItemDAOs = DynamicFilter(KpiCriteriaItemDAOs, filter);
            KpiCriteriaItemDAOs = OrFilter(KpiCriteriaItemDAOs, filter);
            KpiCriteriaItemDAOs = DynamicOrder(KpiCriteriaItemDAOs, filter);
            List<KpiCriteriaItem> KpiCriteriaItems = await DynamicSelect(KpiCriteriaItemDAOs, filter);
            return KpiCriteriaItems;
        }

        public async Task<KpiCriteriaItem> Get(long Id)
        {
            KpiCriteriaItem KpiCriteriaItem = await DataContext.KpiCriteriaItem.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new KpiCriteriaItem()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiCriteriaItem == null)
                return null;

            return KpiCriteriaItem;
        }
        public async Task<bool> Create(KpiCriteriaItem KpiCriteriaItem)
        {
            KpiCriteriaItemDAO KpiCriteriaItemDAO = new KpiCriteriaItemDAO();
            KpiCriteriaItemDAO.Id = KpiCriteriaItem.Id;
            KpiCriteriaItemDAO.Code = KpiCriteriaItem.Code;
            KpiCriteriaItemDAO.Name = KpiCriteriaItem.Name;
            DataContext.KpiCriteriaItem.Add(KpiCriteriaItemDAO);
            await DataContext.SaveChangesAsync();
            KpiCriteriaItem.Id = KpiCriteriaItemDAO.Id;
            await SaveReference(KpiCriteriaItem);
            return true;
        }

        public async Task<bool> Update(KpiCriteriaItem KpiCriteriaItem)
        {
            KpiCriteriaItemDAO KpiCriteriaItemDAO = DataContext.KpiCriteriaItem.Where(x => x.Id == KpiCriteriaItem.Id).FirstOrDefault();
            if (KpiCriteriaItemDAO == null)
                return false;
            KpiCriteriaItemDAO.Id = KpiCriteriaItem.Id;
            KpiCriteriaItemDAO.Code = KpiCriteriaItem.Code;
            KpiCriteriaItemDAO.Name = KpiCriteriaItem.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiCriteriaItem);
            return true;
        }

        public async Task<bool> Delete(KpiCriteriaItem KpiCriteriaItem)
        {
            await DataContext.KpiCriteriaItem.Where(x => x.Id == KpiCriteriaItem.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            List<KpiCriteriaItemDAO> KpiCriteriaItemDAOs = new List<KpiCriteriaItemDAO>();
            foreach (KpiCriteriaItem KpiCriteriaItem in KpiCriteriaItems)
            {
                KpiCriteriaItemDAO KpiCriteriaItemDAO = new KpiCriteriaItemDAO();
                KpiCriteriaItemDAO.Id = KpiCriteriaItem.Id;
                KpiCriteriaItemDAO.Code = KpiCriteriaItem.Code;
                KpiCriteriaItemDAO.Name = KpiCriteriaItem.Name;
                KpiCriteriaItemDAOs.Add(KpiCriteriaItemDAO);
            }
            await DataContext.BulkMergeAsync(KpiCriteriaItemDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            List<long> Ids = KpiCriteriaItems.Select(x => x.Id).ToList();
            await DataContext.KpiCriteriaItem
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiCriteriaItem KpiCriteriaItem)
        {
        }

    }
}
