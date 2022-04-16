using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IErpApprovalStateRepository
    {
        Task<int> Count(ErpApprovalStateFilter ErpApprovalStateFilter);
        Task<int> CountAll(ErpApprovalStateFilter ErpApprovalStateFilter);
        Task<List<ErpApprovalState>> List(ErpApprovalStateFilter ErpApprovalStateFilter);
        Task<List<ErpApprovalState>> List(List<long> Ids);
        Task<ErpApprovalState> Get(long Id);
    }
    public class ErpApprovalStateRepository : IErpApprovalStateRepository
    {
        private DataContext DataContext;
        public ErpApprovalStateRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ErpApprovalStateDAO> DynamicFilter(IQueryable<ErpApprovalStateDAO> query, ErpApprovalStateFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<ErpApprovalStateDAO> OrFilter(IQueryable<ErpApprovalStateDAO> query, ErpApprovalStateFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ErpApprovalStateDAO> initQuery = query.Where(q => false);
            foreach (ErpApprovalStateFilter ErpApprovalStateFilter in filter.OrFilter)
            {
                IQueryable<ErpApprovalStateDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ErpApprovalStateFilter.Id);
                queryable = queryable.Where(q => q.Code, ErpApprovalStateFilter.Code);
                queryable = queryable.Where(q => q.Name, ErpApprovalStateFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ErpApprovalStateDAO> DynamicOrder(IQueryable<ErpApprovalStateDAO> query, ErpApprovalStateFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ErpApprovalStateOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ErpApprovalStateOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ErpApprovalStateOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ErpApprovalStateOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ErpApprovalStateOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ErpApprovalStateOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ErpApprovalState>> DynamicSelect(IQueryable<ErpApprovalStateDAO> query, ErpApprovalStateFilter filter)
        {
            List<ErpApprovalState> ErpApprovalStates = await query.Select(q => new ErpApprovalState()
            {
                Id = filter.Selects.Contains(ErpApprovalStateSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ErpApprovalStateSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ErpApprovalStateSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return ErpApprovalStates;
        }

        public async Task<int> Count(ErpApprovalStateFilter filter)
        {
            IQueryable<ErpApprovalStateDAO> ErpApprovalStateDAOs = DataContext.ErpApprovalState.AsNoTracking();
            ErpApprovalStateDAOs = DynamicFilter(ErpApprovalStateDAOs, filter);
            ErpApprovalStateDAOs = OrFilter(ErpApprovalStateDAOs, filter);
            return await ErpApprovalStateDAOs.CountAsync();
        }
        public async Task<int> CountAll(ErpApprovalStateFilter filter)
        {
            IQueryable<ErpApprovalStateDAO> ErpApprovalStateDAOs = DataContext.ErpApprovalState.AsNoTracking();
            ErpApprovalStateDAOs = DynamicFilter(ErpApprovalStateDAOs, filter);
            return await ErpApprovalStateDAOs.CountAsync();
        }

        public async Task<List<ErpApprovalState>> List(ErpApprovalStateFilter filter)
        {
            if (filter == null) return new List<ErpApprovalState>();
            IQueryable<ErpApprovalStateDAO> ErpApprovalStateDAOs = DataContext.ErpApprovalState.AsNoTracking();
            ErpApprovalStateDAOs = DynamicFilter(ErpApprovalStateDAOs, filter);
            ErpApprovalStateDAOs = OrFilter(ErpApprovalStateDAOs, filter);
            ErpApprovalStateDAOs = DynamicOrder(ErpApprovalStateDAOs, filter);
            List<ErpApprovalState> ErpApprovalStates = await DynamicSelect(ErpApprovalStateDAOs, filter);
            return ErpApprovalStates;
        }

        public async Task<ErpApprovalState> Get(long Id)
        {
            ErpApprovalState ErpApprovalState = await DataContext.ErpApprovalState.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new ErpApprovalState()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefaultAsync();

            if (ErpApprovalState == null)
                return null;

            return ErpApprovalState;
        }

        public async Task<List<ErpApprovalState>> List(List<long> Ids)
        {
            List<ErpApprovalState> ErpApprovalStates = await DataContext.ErpApprovalState.AsNoTracking()
               .Where(x => Ids.Contains(x.Id))
               .Select(x => new ErpApprovalState()
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
               }).ToListAsync();
            return ErpApprovalStates;
        }
    }
}
