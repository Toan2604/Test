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
    public interface IGeneralApprovalStateRepository
    {
        Task<int> Count(GeneralApprovalStateFilter GeneralApprovalStateFilter);
        Task<int> CountAll(GeneralApprovalStateFilter GeneralApprovalStateFilter);
        Task<List<GeneralApprovalState>> List(GeneralApprovalStateFilter GeneralApprovalStateFilter);
        Task<List<GeneralApprovalState>> List(List<long> Ids);
        Task<GeneralApprovalState> Get(long Id);
    }
    public class GeneralApprovalStateRepository : IGeneralApprovalStateRepository
    {
        private DataContext DataContext;
        public GeneralApprovalStateRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<GeneralApprovalStateDAO> DynamicFilter(IQueryable<GeneralApprovalStateDAO> query, GeneralApprovalStateFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<GeneralApprovalStateDAO> OrFilter(IQueryable<GeneralApprovalStateDAO> query, GeneralApprovalStateFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<GeneralApprovalStateDAO> initQuery = query.Where(q => false);
            foreach (GeneralApprovalStateFilter GeneralApprovalStateFilter in filter.OrFilter)
            {
                IQueryable<GeneralApprovalStateDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, GeneralApprovalStateFilter.Id);
                queryable = queryable.Where(q => q.Code, GeneralApprovalStateFilter.Code);
                queryable = queryable.Where(q => q.Name, GeneralApprovalStateFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<GeneralApprovalStateDAO> DynamicOrder(IQueryable<GeneralApprovalStateDAO> query, GeneralApprovalStateFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case GeneralApprovalStateOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case GeneralApprovalStateOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case GeneralApprovalStateOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case GeneralApprovalStateOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case GeneralApprovalStateOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case GeneralApprovalStateOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<GeneralApprovalState>> DynamicSelect(IQueryable<GeneralApprovalStateDAO> query, GeneralApprovalStateFilter filter)
        {
            List<GeneralApprovalState> GeneralApprovalStates = await query.Select(q => new GeneralApprovalState()
            {
                Id = filter.Selects.Contains(GeneralApprovalStateSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(GeneralApprovalStateSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(GeneralApprovalStateSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return GeneralApprovalStates;
        }

        public async Task<int> Count(GeneralApprovalStateFilter filter)
        {
            IQueryable<GeneralApprovalStateDAO> GeneralApprovalStateDAOs = DataContext.GeneralApprovalState.AsNoTracking();
            GeneralApprovalStateDAOs = DynamicFilter(GeneralApprovalStateDAOs, filter);
            GeneralApprovalStateDAOs = OrFilter(GeneralApprovalStateDAOs, filter);
            return await GeneralApprovalStateDAOs.CountAsync();
        }
        public async Task<int> CountAll(GeneralApprovalStateFilter filter)
        {
            IQueryable<GeneralApprovalStateDAO> GeneralApprovalStateDAOs = DataContext.GeneralApprovalState.AsNoTracking();
            GeneralApprovalStateDAOs = DynamicFilter(GeneralApprovalStateDAOs, filter);
            return await GeneralApprovalStateDAOs.CountAsync();
        }

        public async Task<List<GeneralApprovalState>> List(GeneralApprovalStateFilter filter)
        {
            if (filter == null) return new List<GeneralApprovalState>();
            IQueryable<GeneralApprovalStateDAO> GeneralApprovalStateDAOs = DataContext.GeneralApprovalState.AsNoTracking();
            GeneralApprovalStateDAOs = DynamicFilter(GeneralApprovalStateDAOs, filter);
            GeneralApprovalStateDAOs = OrFilter(GeneralApprovalStateDAOs, filter);
            GeneralApprovalStateDAOs = DynamicOrder(GeneralApprovalStateDAOs, filter);
            List<GeneralApprovalState> GeneralApprovalStates = await DynamicSelect(GeneralApprovalStateDAOs, filter);
            return GeneralApprovalStates;
        }

        public async Task<GeneralApprovalState> Get(long Id)
        {
            GeneralApprovalState GeneralApprovalState = await DataContext.GeneralApprovalState.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new GeneralApprovalState()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefaultAsync();

            if (GeneralApprovalState == null)
                return null;

            return GeneralApprovalState;
        }

        public async Task<List<GeneralApprovalState>> List(List<long> Ids)
        {
            List<GeneralApprovalState> GeneralApprovalStates = await DataContext.GeneralApprovalState.AsNoTracking()
               .Where(x => Ids.Contains(x.Id))
               .Select(x => new GeneralApprovalState()
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
               }).ToListAsync();
            return GeneralApprovalStates;
        }
    }
}
