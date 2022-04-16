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
    public interface IPriceListTypeRepository
    {
        Task<int> Count(PriceListTypeFilter PriceListTypeFilter);
        Task<int> CountAll(PriceListTypeFilter PriceListTypeFilter);
        Task<List<PriceListType>> List(PriceListTypeFilter PriceListTypeFilter);
        Task<PriceListType> Get(long Id);
        Task<bool> Create(PriceListType PriceListType);
        Task<bool> Update(PriceListType PriceListType);
        Task<bool> Delete(PriceListType PriceListType);
        Task<bool> BulkMerge(List<PriceListType> PriceListTypes);
        Task<bool> BulkDelete(List<PriceListType> PriceListTypes);
    }
    public class PriceListTypeRepository : IPriceListTypeRepository
    {
        private DataContext DataContext;
        public PriceListTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PriceListTypeDAO> DynamicFilter(IQueryable<PriceListTypeDAO> query, PriceListTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<PriceListTypeDAO> OrFilter(IQueryable<PriceListTypeDAO> query, PriceListTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PriceListTypeDAO> initQuery = query.Where(q => false);
            foreach (PriceListTypeFilter PriceListTypeFilter in filter.OrFilter)
            {
                IQueryable<PriceListTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PriceListTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, PriceListTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, PriceListTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PriceListTypeDAO> DynamicOrder(IQueryable<PriceListTypeDAO> query, PriceListTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PriceListTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PriceListTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PriceListTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PriceListTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PriceListTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PriceListTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<PriceListType>> DynamicSelect(IQueryable<PriceListTypeDAO> query, PriceListTypeFilter filter)
        {
            List<PriceListType> PriceListTypes = await query.Select(q => new PriceListType()
            {
                Id = filter.Selects.Contains(PriceListTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PriceListTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PriceListTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return PriceListTypes;
        }

        public async Task<int> Count(PriceListTypeFilter filter)
        {
            IQueryable<PriceListTypeDAO> PriceListTypes = DataContext.PriceListType.AsNoTracking();
            PriceListTypes = DynamicFilter(PriceListTypes, filter);
            PriceListTypes = OrFilter(PriceListTypes, filter);
            return await PriceListTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(PriceListTypeFilter filter)
        {
            IQueryable<PriceListTypeDAO> PriceListTypes = DataContext.PriceListType.AsNoTracking();
            PriceListTypes = DynamicFilter(PriceListTypes, filter);
            return await PriceListTypes.CountWithNoLockAsync();
        }

        public async Task<List<PriceListType>> List(PriceListTypeFilter filter)
        {
            if (filter == null) return new List<PriceListType>();
            IQueryable<PriceListTypeDAO> PriceListTypeDAOs = DataContext.PriceListType.AsNoTracking();
            PriceListTypeDAOs = DynamicFilter(PriceListTypeDAOs, filter);
            PriceListTypeDAOs = OrFilter(PriceListTypeDAOs, filter);
            PriceListTypeDAOs = DynamicOrder(PriceListTypeDAOs, filter);
            List<PriceListType> PriceListTypes = await DynamicSelect(PriceListTypeDAOs, filter);
            return PriceListTypes;
        }

        public async Task<PriceListType> Get(long Id)
        {
            PriceListType PriceListType = await DataContext.PriceListType.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new PriceListType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (PriceListType == null)
                return null;

            return PriceListType;
        }
        public async Task<bool> Create(PriceListType PriceListType)
        {
            PriceListTypeDAO PriceListTypeDAO = new PriceListTypeDAO();
            PriceListTypeDAO.Id = PriceListType.Id;
            PriceListTypeDAO.Code = PriceListType.Code;
            PriceListTypeDAO.Name = PriceListType.Name;
            DataContext.PriceListType.Add(PriceListTypeDAO);
            await DataContext.SaveChangesAsync();
            PriceListType.Id = PriceListTypeDAO.Id;
            await SaveReference(PriceListType);
            return true;
        }

        public async Task<bool> Update(PriceListType PriceListType)
        {
            PriceListTypeDAO PriceListTypeDAO = DataContext.PriceListType.Where(x => x.Id == PriceListType.Id).FirstOrDefault();
            if (PriceListTypeDAO == null)
                return false;
            PriceListTypeDAO.Id = PriceListType.Id;
            PriceListTypeDAO.Code = PriceListType.Code;
            PriceListTypeDAO.Name = PriceListType.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(PriceListType);
            return true;
        }

        public async Task<bool> Delete(PriceListType PriceListType)
        {
            await DataContext.PriceListType.Where(x => x.Id == PriceListType.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<PriceListType> PriceListTypes)
        {
            List<PriceListTypeDAO> PriceListTypeDAOs = new List<PriceListTypeDAO>();
            foreach (PriceListType PriceListType in PriceListTypes)
            {
                PriceListTypeDAO PriceListTypeDAO = new PriceListTypeDAO();
                PriceListTypeDAO.Id = PriceListType.Id;
                PriceListTypeDAO.Code = PriceListType.Code;
                PriceListTypeDAO.Name = PriceListType.Name;
                PriceListTypeDAOs.Add(PriceListTypeDAO);
            }
            await DataContext.BulkMergeAsync(PriceListTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<PriceListType> PriceListTypes)
        {
            List<long> Ids = PriceListTypes.Select(x => x.Id).ToList();
            await DataContext.PriceListType
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(PriceListType PriceListType)
        {
        }

    }
}
