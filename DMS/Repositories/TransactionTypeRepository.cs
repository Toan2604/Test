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
    public interface ITransactionTypeRepository
    {
        Task<int> Count(TransactionTypeFilter TransactionTypeFilter);
        Task<int> CountAll(TransactionTypeFilter TransactionTypeFilter);
        Task<List<TransactionType>> List(TransactionTypeFilter TransactionTypeFilter);
        Task<List<TransactionType>> List(List<long> Ids);
        Task<TransactionType> Get(long Id);
    }
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private DataContext DataContext;
        public TransactionTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<TransactionTypeDAO> DynamicFilter(IQueryable<TransactionTypeDAO> query, TransactionTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<TransactionTypeDAO> OrFilter(IQueryable<TransactionTypeDAO> query, TransactionTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<TransactionTypeDAO> initQuery = query.Where(q => false);
            foreach (TransactionTypeFilter TransactionTypeFilter in filter.OrFilter)
            {
                IQueryable<TransactionTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, TransactionTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, TransactionTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, TransactionTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<TransactionTypeDAO> DynamicOrder(IQueryable<TransactionTypeDAO> query, TransactionTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TransactionTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case TransactionTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case TransactionTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TransactionTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case TransactionTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case TransactionTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<TransactionType>> DynamicSelect(IQueryable<TransactionTypeDAO> query, TransactionTypeFilter filter)
        {
            List<TransactionType> TransactionTypes = await query.Select(q => new TransactionType()
            {
                Id = filter.Selects.Contains(TransactionTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(TransactionTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(TransactionTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return TransactionTypes;
        }

        public async Task<int> Count(TransactionTypeFilter filter)
        {
            IQueryable<TransactionTypeDAO> TransactionTypes = DataContext.TransactionType.AsNoTracking();
            TransactionTypes = DynamicFilter(TransactionTypes, filter);
            TransactionTypes = OrFilter(TransactionTypes, filter);
            return await TransactionTypes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(TransactionTypeFilter filter)
        {
            IQueryable<TransactionTypeDAO> TransactionTypes = DataContext.TransactionType.AsNoTracking();
            TransactionTypes = DynamicFilter(TransactionTypes, filter);
            return await TransactionTypes.CountWithNoLockAsync();
        }

        public async Task<List<TransactionType>> List(TransactionTypeFilter filter)
        {
            if (filter == null) return new List<TransactionType>();
            IQueryable<TransactionTypeDAO> TransactionTypeDAOs = DataContext.TransactionType.AsNoTracking();
            TransactionTypeDAOs = DynamicFilter(TransactionTypeDAOs, filter);
            TransactionTypeDAOs = OrFilter(TransactionTypeDAOs, filter);
            TransactionTypeDAOs = DynamicOrder(TransactionTypeDAOs, filter);
            List<TransactionType> TransactionTypes = await DynamicSelect(TransactionTypeDAOs, filter);
            return TransactionTypes;
        }

        public async Task<List<TransactionType>> List(List<long> Ids)
        {
            List<TransactionType> TransactionTypes = await DataContext.TransactionType.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new TransactionType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();


            return TransactionTypes;
        }

        public async Task<TransactionType> Get(long Id)
        {
            TransactionType TransactionType = await DataContext.TransactionType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new TransactionType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (TransactionType == null)
                return null;

            return TransactionType;
        }
    }
}
