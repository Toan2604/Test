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
    public interface ISalesOrderTypeRepository
    {
        Task<int> Count(SalesOrderTypeFilter SalesOrderTypeFilter);
        Task<int> CountAll(SalesOrderTypeFilter SalesOrderTypeFilter);
        Task<List<SalesOrderType>> List(SalesOrderTypeFilter SalesOrderTypeFilter);
        Task<SalesOrderType> Get(long Id);
        Task<bool> Create(SalesOrderType SalesOrderType);
        Task<bool> Update(SalesOrderType SalesOrderType);
        Task<bool> Delete(SalesOrderType SalesOrderType);
        Task<bool> BulkMerge(List<SalesOrderType> SalesOrderTypes);
        Task<bool> BulkDelete(List<SalesOrderType> SalesOrderTypes);
    }
    public class SalesOrderTypeRepository : ISalesOrderTypeRepository
    {
        private DataContext DataContext;
        public SalesOrderTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<SalesOrderTypeDAO> DynamicFilter(IQueryable<SalesOrderTypeDAO> query, SalesOrderTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<SalesOrderTypeDAO> OrFilter(IQueryable<SalesOrderTypeDAO> query, SalesOrderTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<SalesOrderTypeDAO> initQuery = query.Where(q => false);
            foreach (SalesOrderTypeFilter SalesOrderTypeFilter in filter.OrFilter)
            {
                IQueryable<SalesOrderTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, SalesOrderTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, SalesOrderTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, SalesOrderTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<SalesOrderTypeDAO> DynamicOrder(IQueryable<SalesOrderTypeDAO> query, SalesOrderTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SalesOrderTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case SalesOrderTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case SalesOrderTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SalesOrderTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case SalesOrderTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case SalesOrderTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<SalesOrderType>> DynamicSelect(IQueryable<SalesOrderTypeDAO> query, SalesOrderTypeFilter filter)
        {
            List<SalesOrderType> SalesOrderTypes = await query.Select(q => new SalesOrderType()
            {
                Id = filter.Selects.Contains(SalesOrderTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(SalesOrderTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(SalesOrderTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return SalesOrderTypes;
        }

        public async Task<int> Count(SalesOrderTypeFilter filter)
        {
            IQueryable<SalesOrderTypeDAO> SalesOrderTypes = DataContext.SalesOrderType.AsNoTracking();
            SalesOrderTypes = DynamicFilter(SalesOrderTypes, filter);
            SalesOrderTypes = OrFilter(SalesOrderTypes, filter);
            return await SalesOrderTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(SalesOrderTypeFilter filter)
        {
            IQueryable<SalesOrderTypeDAO> SalesOrderTypes = DataContext.SalesOrderType.AsNoTracking();
            SalesOrderTypes = DynamicFilter(SalesOrderTypes, filter);
            return await SalesOrderTypes.CountWithNoLockAsync();
        }

        public async Task<List<SalesOrderType>> List(SalesOrderTypeFilter filter)
        {
            if (filter == null) return new List<SalesOrderType>();
            IQueryable<SalesOrderTypeDAO> SalesOrderTypeDAOs = DataContext.SalesOrderType.AsNoTracking();
            SalesOrderTypeDAOs = DynamicFilter(SalesOrderTypeDAOs, filter);
            SalesOrderTypeDAOs = OrFilter(SalesOrderTypeDAOs, filter);
            SalesOrderTypeDAOs = DynamicOrder(SalesOrderTypeDAOs, filter);
            List<SalesOrderType> SalesOrderTypes = await DynamicSelect(SalesOrderTypeDAOs, filter);
            return SalesOrderTypes;
        }

        public async Task<SalesOrderType> Get(long Id)
        {
            SalesOrderType SalesOrderType = await DataContext.SalesOrderType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new SalesOrderType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (SalesOrderType == null)
                return null;

            return SalesOrderType;
        }
        public async Task<bool> Create(SalesOrderType SalesOrderType)
        {
            SalesOrderTypeDAO SalesOrderTypeDAO = new SalesOrderTypeDAO();
            SalesOrderTypeDAO.Id = SalesOrderType.Id;
            SalesOrderTypeDAO.Code = SalesOrderType.Code;
            SalesOrderTypeDAO.Name = SalesOrderType.Name;
            DataContext.SalesOrderType.Add(SalesOrderTypeDAO);
            await DataContext.SaveChangesAsync();
            SalesOrderType.Id = SalesOrderTypeDAO.Id;
            await SaveReference(SalesOrderType);
            return true;
        }

        public async Task<bool> Update(SalesOrderType SalesOrderType)
        {
            SalesOrderTypeDAO SalesOrderTypeDAO = DataContext.SalesOrderType.Where(x => x.Id == SalesOrderType.Id).FirstOrDefault();
            if (SalesOrderTypeDAO == null)
                return false;
            SalesOrderTypeDAO.Id = SalesOrderType.Id;
            SalesOrderTypeDAO.Code = SalesOrderType.Code;
            SalesOrderTypeDAO.Name = SalesOrderType.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(SalesOrderType);
            return true;
        }

        public async Task<bool> Delete(SalesOrderType SalesOrderType)
        {
            await DataContext.SalesOrderType.Where(x => x.Id == SalesOrderType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<SalesOrderType> SalesOrderTypes)
        {
            List<SalesOrderTypeDAO> SalesOrderTypeDAOs = new List<SalesOrderTypeDAO>();
            foreach (SalesOrderType SalesOrderType in SalesOrderTypes)
            {
                SalesOrderTypeDAO SalesOrderTypeDAO = new SalesOrderTypeDAO();
                SalesOrderTypeDAO.Id = SalesOrderType.Id;
                SalesOrderTypeDAO.Code = SalesOrderType.Code;
                SalesOrderTypeDAO.Name = SalesOrderType.Name;
                SalesOrderTypeDAOs.Add(SalesOrderTypeDAO);
            }
            await DataContext.BulkMergeAsync(SalesOrderTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<SalesOrderType> SalesOrderTypes)
        {
            List<long> Ids = SalesOrderTypes.Select(x => x.Id).ToList();
            await DataContext.SalesOrderType
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(SalesOrderType SalesOrderType)
        {
        }

    }
}
