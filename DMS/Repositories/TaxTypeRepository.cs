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
    public interface ITaxTypeRepository
    {
        Task<int> Count(TaxTypeFilter TaxTypeFilter);
        Task<int> CountAll(TaxTypeFilter TaxTypeFilter);
        Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter);
        Task<TaxType> Get(long Id);
        Task<bool> BulkMerge(List<TaxType> TaxTypes);
    }
    public class TaxTypeRepository : ITaxTypeRepository
    {
        private DataContext DataContext;
        public TaxTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<TaxTypeDAO> DynamicFilter(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Percentage, filter.Percentage);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<TaxTypeDAO> OrFilter(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<TaxTypeDAO> initQuery = query.Where(q => false);
            foreach (TaxTypeFilter TaxTypeFilter in filter.OrFilter)
            {
                IQueryable<TaxTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, TaxTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, TaxTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, TaxTypeFilter.Name);
                queryable = queryable.Where(q => q.Percentage, TaxTypeFilter.Percentage);
                queryable = queryable.Where(q => q.StatusId, TaxTypeFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<TaxTypeDAO> DynamicOrder(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TaxTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case TaxTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case TaxTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case TaxTypeOrder.Percentage:
                            query = query.OrderBy(q => q.Percentage);
                            break;
                        case TaxTypeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TaxTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case TaxTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case TaxTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case TaxTypeOrder.Percentage:
                            query = query.OrderByDescending(q => q.Percentage);
                            break;
                        case TaxTypeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<TaxType>> DynamicSelect(IQueryable<TaxTypeDAO> query, TaxTypeFilter filter)
        {
            List<TaxType> TaxTypes = await query.Select(q => new TaxType()
            {
                Id = filter.Selects.Contains(TaxTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(TaxTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(TaxTypeSelect.Name) ? q.Name : default(string),
                Percentage = filter.Selects.Contains(TaxTypeSelect.Percentage) ? q.Percentage : default(decimal),
                StatusId = filter.Selects.Contains(TaxTypeSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(TaxTypeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Used = q.Used,
            }).ToListWithNoLockAsync();
            return TaxTypes;
        }

        public async Task<int> Count(TaxTypeFilter filter)
        {
            IQueryable<TaxTypeDAO> TaxTypes = DataContext.TaxType;
            TaxTypes = DynamicFilter(TaxTypes, filter);
            TaxTypes = OrFilter(TaxTypes, filter);
            return await TaxTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(TaxTypeFilter filter)
        {
            IQueryable<TaxTypeDAO> TaxTypes = DataContext.TaxType;
            TaxTypes = DynamicFilter(TaxTypes, filter);
            return await TaxTypes.CountWithNoLockAsync();
        }

        public async Task<List<TaxType>> List(TaxTypeFilter filter)
        {
            if (filter == null) return new List<TaxType>();
            IQueryable<TaxTypeDAO> TaxTypeDAOs = DataContext.TaxType;
            TaxTypeDAOs = DynamicFilter(TaxTypeDAOs, filter);
            TaxTypeDAOs = OrFilter(TaxTypeDAOs, filter);
            TaxTypeDAOs = DynamicOrder(TaxTypeDAOs, filter);
            List<TaxType> TaxTypes = await DynamicSelect(TaxTypeDAOs, filter);
            return TaxTypes;
        }

        public async Task<TaxType> Get(long Id)
        {
            TaxType TaxType = await DataContext.TaxType.Where(x => x.Id == Id).Select(x => new TaxType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Percentage = x.Percentage,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (TaxType == null)
                return null;

            return TaxType;
        }
        public async Task<bool> BulkMerge(List<TaxType> TaxTypes)
        {
            List<TaxTypeDAO> TaxTypeDAOs = TaxTypes.Select(x => new TaxTypeDAO
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                StatusId = x.StatusId,
                Percentage = x.Percentage,
                Used = x.Used,
                RowId = x.RowId,
            }).ToList();
            await DataContext.BulkMergeAsync(TaxTypeDAOs);
            return true;
        }
    }
}
