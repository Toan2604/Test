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
    public interface IProblemStatusRepository
    {
        Task<int> Count(ProblemStatusFilter ProblemStatusFilter);
        Task<int> CountAll(ProblemStatusFilter ProblemStatusFilter);
        Task<List<ProblemStatus>> List(ProblemStatusFilter ProblemStatusFilter);
        Task<ProblemStatus> Get(long Id);
        Task<bool> Create(ProblemStatus ProblemStatus);
        Task<bool> Update(ProblemStatus ProblemStatus);
        Task<bool> Delete(ProblemStatus ProblemStatus);
        Task<bool> BulkMerge(List<ProblemStatus> ProblemStatuses);
        Task<bool> BulkDelete(List<ProblemStatus> ProblemStatuses);
    }
    public class ProblemStatusRepository : IProblemStatusRepository
    {
        private DataContext DataContext;
        public ProblemStatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProblemStatusDAO> DynamicFilter(IQueryable<ProblemStatusDAO> query, ProblemStatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<ProblemStatusDAO> OrFilter(IQueryable<ProblemStatusDAO> query, ProblemStatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProblemStatusDAO> initQuery = query.Where(q => false);
            foreach (ProblemStatusFilter ProblemStatusFilter in filter.OrFilter)
            {
                IQueryable<ProblemStatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProblemStatusDAO> DynamicOrder(IQueryable<ProblemStatusDAO> query, ProblemStatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProblemStatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProblemStatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProblemStatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProblemStatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProblemStatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProblemStatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ProblemStatus>> DynamicSelect(IQueryable<ProblemStatusDAO> query, ProblemStatusFilter filter)
        {
            List<ProblemStatus> ProblemStatuses = await query.Select(q => new ProblemStatus()
            {
                Id = filter.Selects.Contains(ProblemStatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProblemStatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProblemStatusSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return ProblemStatuses;
        }

        public async Task<int> Count(ProblemStatusFilter filter)
        {
            IQueryable<ProblemStatusDAO> ProblemStatuses = DataContext.ProblemStatus.AsNoTracking();
            ProblemStatuses = DynamicFilter(ProblemStatuses, filter);
            ProblemStatuses = OrFilter(ProblemStatuses, filter);
            return await ProblemStatuses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ProblemStatusFilter filter)
        {
            IQueryable<ProblemStatusDAO> ProblemStatuses = DataContext.ProblemStatus.AsNoTracking();
            ProblemStatuses = DynamicFilter(ProblemStatuses, filter);
            return await ProblemStatuses.CountWithNoLockAsync();
        }

        public async Task<List<ProblemStatus>> List(ProblemStatusFilter filter)
        {
            if (filter == null) return new List<ProblemStatus>();
            IQueryable<ProblemStatusDAO> ProblemStatusDAOs = DataContext.ProblemStatus.AsNoTracking();
            ProblemStatusDAOs = DynamicFilter(ProblemStatusDAOs, filter);
            ProblemStatusDAOs = OrFilter(ProblemStatusDAOs, filter);
            ProblemStatusDAOs = DynamicOrder(ProblemStatusDAOs, filter);
            List<ProblemStatus> ProblemStatuses = await DynamicSelect(ProblemStatusDAOs, filter);
            return ProblemStatuses;
        }

        public async Task<ProblemStatus> Get(long Id)
        {
            ProblemStatus ProblemStatus = await DataContext.ProblemStatus.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ProblemStatus()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (ProblemStatus == null)
                return null;

            return ProblemStatus;
        }
        public async Task<bool> Create(ProblemStatus ProblemStatus)
        {
            ProblemStatusDAO ProblemStatusDAO = new ProblemStatusDAO();
            ProblemStatusDAO.Id = ProblemStatus.Id;
            ProblemStatusDAO.Code = ProblemStatus.Code;
            ProblemStatusDAO.Name = ProblemStatus.Name;
            DataContext.ProblemStatus.Add(ProblemStatusDAO);
            await DataContext.SaveChangesAsync();
            ProblemStatus.Id = ProblemStatusDAO.Id;
            await SaveReference(ProblemStatus);
            return true;
        }

        public async Task<bool> Update(ProblemStatus ProblemStatus)
        {
            ProblemStatusDAO ProblemStatusDAO = DataContext.ProblemStatus.Where(x => x.Id == ProblemStatus.Id).FirstOrDefault();
            if (ProblemStatusDAO == null)
                return false;
            ProblemStatusDAO.Id = ProblemStatus.Id;
            ProblemStatusDAO.Code = ProblemStatus.Code;
            ProblemStatusDAO.Name = ProblemStatus.Name;
            await DataContext.SaveChangesAsync();
            await SaveReference(ProblemStatus);
            return true;
        }

        public async Task<bool> Delete(ProblemStatus ProblemStatus)
        {
            await DataContext.ProblemStatus.Where(x => x.Id == ProblemStatus.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<ProblemStatus> ProblemStatuses)
        {
            List<ProblemStatusDAO> ProblemStatusDAOs = new List<ProblemStatusDAO>();
            foreach (ProblemStatus ProblemStatus in ProblemStatuses)
            {
                ProblemStatusDAO ProblemStatusDAO = new ProblemStatusDAO();
                ProblemStatusDAO.Id = ProblemStatus.Id;
                ProblemStatusDAO.Code = ProblemStatus.Code;
                ProblemStatusDAO.Name = ProblemStatus.Name;
                ProblemStatusDAOs.Add(ProblemStatusDAO);
            }
            await DataContext.BulkMergeAsync(ProblemStatusDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ProblemStatus> ProblemStatuses)
        {
            List<long> Ids = ProblemStatuses.Select(x => x.Id).ToList();
            await DataContext.ProblemStatus
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(ProblemStatus ProblemStatus)
        {
        }

    }
}
