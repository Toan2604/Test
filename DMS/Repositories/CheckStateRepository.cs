using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using DMS.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.Repositories
{
    public interface ICheckStateRepository
    {
        Task<int> CountAll(CheckStateFilter CheckStateFilter);
        Task<int> Count(CheckStateFilter CheckStateFilter);
        Task<List<CheckState>> List(CheckStateFilter CheckStateFilter);
        Task<List<CheckState>> List(List<long> Ids);
        Task<CheckState> Get(long Id);
        Task<bool> Create(CheckState CheckState);
        Task<bool> Update(CheckState CheckState);
        Task<bool> Delete(CheckState CheckState);
        Task<bool> BulkMerge(List<CheckState> CheckStates);
        Task<bool> BulkDelete(List<CheckState> CheckStates);
    }
    public class CheckStateRepository : ICheckStateRepository
    {
        private DataContext DataContext;
        public CheckStateRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<CheckStateDAO>> DynamicFilter(IQueryable<CheckStateDAO> query, CheckStateFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private async Task<IQueryable<CheckStateDAO>> OrFilter(IQueryable<CheckStateDAO> query, CheckStateFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<CheckStateDAO> initQuery = query.Where(q => false);
            foreach (CheckStateFilter CheckStateFilter in filter.OrFilter)
            {
                IQueryable<CheckStateDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, CheckStateFilter.Id);
                queryable = queryable.Where(q => q.Code, CheckStateFilter.Code);
                queryable = queryable.Where(q => q.Name, CheckStateFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<CheckStateDAO> DynamicOrder(IQueryable<CheckStateDAO> query, CheckStateFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CheckStateOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case CheckStateOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case CheckStateOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CheckStateOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case CheckStateOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case CheckStateOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<CheckState>> DynamicSelect(IQueryable<CheckStateDAO> query, CheckStateFilter filter)
        {
            List<CheckState> CheckStates = await query.Select(q => new CheckState()
            {
                Id = filter.Selects.Contains(CheckStateSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(CheckStateSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(CheckStateSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return CheckStates;
        }

        public async Task<int> CountAll(CheckStateFilter filter)
        {
            IQueryable<CheckStateDAO> CheckStateDAOs = DataContext.CheckState.AsNoTracking();
            CheckStateDAOs = await DynamicFilter(CheckStateDAOs, filter);
            return await CheckStateDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(CheckStateFilter filter)
        {
            IQueryable<CheckStateDAO> CheckStateDAOs = DataContext.CheckState.AsNoTracking();
            CheckStateDAOs = await DynamicFilter(CheckStateDAOs, filter);
            CheckStateDAOs = await OrFilter(CheckStateDAOs, filter);
            return await CheckStateDAOs.CountWithNoLockAsync();
        }

        public async Task<List<CheckState>> List(CheckStateFilter filter)
        {
            if (filter == null) return new List<CheckState>();
            IQueryable<CheckStateDAO> CheckStateDAOs = DataContext.CheckState.AsNoTracking();
            CheckStateDAOs = await DynamicFilter(CheckStateDAOs, filter);
            CheckStateDAOs = await OrFilter(CheckStateDAOs, filter);
            CheckStateDAOs = DynamicOrder(CheckStateDAOs, filter);
            List<CheckState> CheckStates = await DynamicSelect(CheckStateDAOs, filter);
            return CheckStates;
        }

        public async Task<List<CheckState>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<CheckStateDAO> query = DataContext.CheckState.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<CheckState> CheckStates = await query.AsNoTracking()
            .Select(x => new CheckState()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();
            

            return CheckStates;
        }

        public async Task<CheckState> Get(long Id)
        {
            CheckState CheckState = await DataContext.CheckState.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new CheckState()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (CheckState == null)
                return null;

            return CheckState;
        }
        
        public async Task<bool> Create(CheckState CheckState)
        {
            CheckStateDAO CheckStateDAO = new CheckStateDAO();
            CheckStateDAO.Id = CheckState.Id;
            CheckStateDAO.Code = CheckState.Code;
            CheckStateDAO.Name = CheckState.Name;
            DataContext.CheckState.Add(CheckStateDAO);
            await DataContext.SaveChangesAsync();
            CheckState.Id = CheckStateDAO.Id;
            await SaveReference(CheckState);
            return true;
        }

        public async Task<bool> Update(CheckState CheckState)
        {
            CheckStateDAO CheckStateDAO = DataContext.CheckState
                .Where(x => x.Id == CheckState.Id)
                .FirstOrDefault();
            if (CheckStateDAO == null)
                return false;
            CheckStateDAO.Id = CheckState.Id;
            CheckStateDAO.Code = CheckState.Code;
            CheckStateDAO.Name = CheckState.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(CheckState);
            return true;
        }

        public async Task<bool> Delete(CheckState CheckState)
        {
            await DataContext.CheckState
                .Where(x => x.Id == CheckState.Id)
                .DeleteFromQueryAsync();
            return true;
        }
        
        public async Task<bool> BulkMerge(List<CheckState> CheckStates)
        {
            IdFilter IdFilter = new IdFilter { In = CheckStates.Select(x => x.Id).ToList() };
            List<CheckStateDAO> CheckStateDAOs = new List<CheckStateDAO>();
            List<CheckStateDAO> DbCheckStateDAOs = await DataContext.CheckState
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (CheckState CheckState in CheckStates)
            {
                CheckStateDAO CheckStateDAO = DbCheckStateDAOs
                        .Where(x => x.Id == CheckState.Id)
                        .FirstOrDefault();
                if (CheckStateDAO == null)
                {
                    CheckStateDAO = new CheckStateDAO();
                }
                CheckStateDAO.Code = CheckState.Code;
                CheckStateDAO.Name = CheckState.Name;
                CheckStateDAOs.Add(CheckStateDAO);
            }
            await DataContext.BulkMergeAsync(CheckStateDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<CheckState> CheckStates)
        {
            List<long> Ids = CheckStates.Select(x => x.Id).ToList();
            await DataContext.CheckState
                .WhereBulkContains(Ids, x => x.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(CheckState CheckState)
        {
        }
        
    }
}
