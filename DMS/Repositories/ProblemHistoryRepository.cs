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
    public interface IProblemHistoryRepository
    {
        Task<int> Count(ProblemHistoryFilter ProblemHistoryFilter);
        Task<int> CountAll(ProblemHistoryFilter ProblemHistoryFilter);
        Task<List<ProblemHistory>> List(ProblemHistoryFilter ProblemHistoryFilter);
        Task<ProblemHistory> Get(long Id);
        Task<bool> Create(ProblemHistory ProblemHistory);
        Task<bool> Update(ProblemHistory ProblemHistory);
        Task<bool> Delete(ProblemHistory ProblemHistory);
        Task<bool> BulkMerge(List<ProblemHistory> ProblemHistories);
        Task<bool> BulkDelete(List<ProblemHistory> ProblemHistories);
    }
    public class ProblemHistoryRepository : IProblemHistoryRepository
    {
        private DataContext DataContext;
        public ProblemHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProblemHistoryDAO> DynamicFilter(IQueryable<ProblemHistoryDAO> query, ProblemHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ProblemId, filter.ProblemId);
            query = query.Where(q => q.Time, filter.Time);
            query = query.Where(q => q.ModifierId, filter.ModifierId);
            query = query.Where(q => q.ProblemStatusId, filter.ProblemStatusId);
            return query;
        }

        private IQueryable<ProblemHistoryDAO> OrFilter(IQueryable<ProblemHistoryDAO> query, ProblemHistoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProblemHistoryDAO> initQuery = query.Where(q => false);
            foreach (ProblemHistoryFilter ProblemHistoryFilter in filter.OrFilter)
            {
                IQueryable<ProblemHistoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.ProblemId, filter.ProblemId);
                queryable = queryable.Where(q => q.Time, filter.Time);
                queryable = queryable.Where(q => q.ModifierId, filter.ModifierId);
                queryable = queryable.Where(q => q.ProblemStatusId, filter.ProblemStatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProblemHistoryDAO> DynamicOrder(IQueryable<ProblemHistoryDAO> query, ProblemHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProblemHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProblemHistoryOrder.Problem:
                            query = query.OrderBy(q => q.ProblemId);
                            break;
                        case ProblemHistoryOrder.Time:
                            query = query.OrderBy(q => q.Time);
                            break;
                        case ProblemHistoryOrder.Modifier:
                            query = query.OrderBy(q => q.ModifierId);
                            break;
                        case ProblemHistoryOrder.ProblemStatus:
                            query = query.OrderBy(q => q.ProblemStatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProblemHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProblemHistoryOrder.Problem:
                            query = query.OrderByDescending(q => q.ProblemId);
                            break;
                        case ProblemHistoryOrder.Time:
                            query = query.OrderByDescending(q => q.Time);
                            break;
                        case ProblemHistoryOrder.Modifier:
                            query = query.OrderByDescending(q => q.ModifierId);
                            break;
                        case ProblemHistoryOrder.ProblemStatus:
                            query = query.OrderByDescending(q => q.ProblemStatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ProblemHistory>> DynamicSelect(IQueryable<ProblemHistoryDAO> query, ProblemHistoryFilter filter)
        {
            List<ProblemHistory> ProblemHistories = await query.Select(q => new ProblemHistory()
            {
                Id = filter.Selects.Contains(ProblemHistorySelect.Id) ? q.Id : default(long),
                ProblemId = filter.Selects.Contains(ProblemHistorySelect.Problem) ? q.ProblemId : default(long),
                Time = filter.Selects.Contains(ProblemHistorySelect.Time) ? q.Time : default(DateTime),
                ModifierId = filter.Selects.Contains(ProblemHistorySelect.Modifier) ? q.ModifierId : default(long),
                ProblemStatusId = filter.Selects.Contains(ProblemHistorySelect.ProblemStatus) ? q.ProblemStatusId : default(long),
                Modifier = filter.Selects.Contains(ProblemHistorySelect.Modifier) && q.Modifier != null ? new AppUser
                {
                    Id = q.Modifier.Id,
                    Username = q.Modifier.Username,
                    DisplayName = q.Modifier.DisplayName,
                    Address = q.Modifier.Address,
                    Email = q.Modifier.Email,
                    Phone = q.Modifier.Phone,
                    PositionId = q.Modifier.PositionId,
                    Department = q.Modifier.Department,
                    OrganizationId = q.Modifier.OrganizationId,
                    StatusId = q.Modifier.StatusId,
                    Avatar = q.Modifier.Avatar,
                    ProvinceId = q.Modifier.ProvinceId,
                    SexId = q.Modifier.SexId,
                    Birthday = q.Modifier.Birthday,
                } : null,
                Problem = filter.Selects.Contains(ProblemHistorySelect.Problem) && q.Problem != null ? new Problem
                {
                    Id = q.Problem.Id,
                    Code = q.Problem.Code,
                    StoreCheckingId = q.Problem.StoreCheckingId,
                    StoreId = q.Problem.StoreId,
                    CreatorId = q.Problem.CreatorId,
                    ProblemTypeId = q.Problem.ProblemTypeId,
                    NoteAt = q.Problem.NoteAt,
                    CompletedAt = q.Problem.CompletedAt,
                    Content = q.Problem.Content,
                    ProblemStatusId = q.Problem.ProblemStatusId,
                } : null,
                ProblemStatus = filter.Selects.Contains(ProblemHistorySelect.ProblemStatus) && q.ProblemStatus != null ? new ProblemStatus
                {
                    Id = q.ProblemStatus.Id,
                    Code = q.ProblemStatus.Code,
                    Name = q.ProblemStatus.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return ProblemHistories;
        }

        public async Task<int> Count(ProblemHistoryFilter filter)
        {
            IQueryable<ProblemHistoryDAO> ProblemHistories = DataContext.ProblemHistory.AsNoTracking();
            ProblemHistories = DynamicFilter(ProblemHistories, filter);
            ProblemHistories = OrFilter(ProblemHistories, filter);
            return await ProblemHistories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ProblemHistoryFilter filter)
        {
            IQueryable<ProblemHistoryDAO> ProblemHistories = DataContext.ProblemHistory.AsNoTracking();
            ProblemHistories = DynamicFilter(ProblemHistories, filter);
            return await ProblemHistories.CountWithNoLockAsync();
        }
        public async Task<List<ProblemHistory>> List(ProblemHistoryFilter filter)
        {
            if (filter == null) return new List<ProblemHistory>();
            IQueryable<ProblemHistoryDAO> ProblemHistoryDAOs = DataContext.ProblemHistory.AsNoTracking();
            ProblemHistoryDAOs = DynamicFilter(ProblemHistoryDAOs, filter);
            ProblemHistoryDAOs = OrFilter(ProblemHistoryDAOs, filter);
            ProblemHistoryDAOs = DynamicOrder(ProblemHistoryDAOs, filter);
            List<ProblemHistory> ProblemHistories = await DynamicSelect(ProblemHistoryDAOs, filter);
            return ProblemHistories;
        }

        public async Task<ProblemHistory> Get(long Id)
        {
            ProblemHistory ProblemHistory = await DataContext.ProblemHistory.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ProblemHistory()
            {
                Id = x.Id,
                ProblemId = x.ProblemId,
                Time = x.Time,
                ModifierId = x.ModifierId,
                ProblemStatusId = x.ProblemStatusId,
                Modifier = x.Modifier == null ? null : new AppUser
                {
                    Id = x.Modifier.Id,
                    Username = x.Modifier.Username,
                    DisplayName = x.Modifier.DisplayName,
                    Address = x.Modifier.Address,
                    Email = x.Modifier.Email,
                    Phone = x.Modifier.Phone,
                    PositionId = x.Modifier.PositionId,
                    Department = x.Modifier.Department,
                    OrganizationId = x.Modifier.OrganizationId,
                    StatusId = x.Modifier.StatusId,
                    Avatar = x.Modifier.Avatar,
                    ProvinceId = x.Modifier.ProvinceId,
                    SexId = x.Modifier.SexId,
                    Birthday = x.Modifier.Birthday,
                },
                Problem = x.Problem == null ? null : new Problem
                {
                    Id = x.Problem.Id,
                    Code = x.Problem.Code,
                    StoreCheckingId = x.Problem.StoreCheckingId,
                    StoreId = x.Problem.StoreId,
                    CreatorId = x.Problem.CreatorId,
                    ProblemTypeId = x.Problem.ProblemTypeId,
                    NoteAt = x.Problem.NoteAt,
                    CompletedAt = x.Problem.CompletedAt,
                    Content = x.Problem.Content,
                    ProblemStatusId = x.Problem.ProblemStatusId,
                },
                ProblemStatus = x.ProblemStatus == null ? null : new ProblemStatus
                {
                    Id = x.ProblemStatus.Id,
                    Code = x.ProblemStatus.Code,
                    Name = x.ProblemStatus.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ProblemHistory == null)
                return null;

            return ProblemHistory;
        }
        public async Task<bool> Create(ProblemHistory ProblemHistory)
        {
            ProblemHistoryDAO ProblemHistoryDAO = new ProblemHistoryDAO();
            ProblemHistoryDAO.Id = ProblemHistory.Id;
            ProblemHistoryDAO.ProblemId = ProblemHistory.ProblemId;
            ProblemHistoryDAO.Time = ProblemHistory.Time;
            ProblemHistoryDAO.ModifierId = ProblemHistory.ModifierId;
            ProblemHistoryDAO.ProblemStatusId = ProblemHistory.ProblemStatusId;
            DataContext.ProblemHistory.Add(ProblemHistoryDAO);
            await DataContext.SaveChangesAsync();
            ProblemHistory.Id = ProblemHistoryDAO.Id;
            await SaveReference(ProblemHistory);
            return true;
        }

        public async Task<bool> Update(ProblemHistory ProblemHistory)
        {
            ProblemHistoryDAO ProblemHistoryDAO = DataContext.ProblemHistory.Where(x => x.Id == ProblemHistory.Id).FirstOrDefault();
            if (ProblemHistoryDAO == null)
                return false;
            ProblemHistoryDAO.Id = ProblemHistory.Id;
            ProblemHistoryDAO.ProblemId = ProblemHistory.ProblemId;
            ProblemHistoryDAO.Time = ProblemHistory.Time;
            ProblemHistoryDAO.ModifierId = ProblemHistory.ModifierId;
            ProblemHistoryDAO.ProblemStatusId = ProblemHistory.ProblemStatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(ProblemHistory);
            return true;
        }

        public async Task<bool> Delete(ProblemHistory ProblemHistory)
        {
            await DataContext.ProblemHistory.Where(x => x.Id == ProblemHistory.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<ProblemHistory> ProblemHistories)
        {
            List<ProblemHistoryDAO> ProblemHistoryDAOs = new List<ProblemHistoryDAO>();
            foreach (ProblemHistory ProblemHistory in ProblemHistories)
            {
                ProblemHistoryDAO ProblemHistoryDAO = new ProblemHistoryDAO();
                ProblemHistoryDAO.Id = ProblemHistory.Id;
                ProblemHistoryDAO.ProblemId = ProblemHistory.ProblemId;
                ProblemHistoryDAO.Time = ProblemHistory.Time;
                ProblemHistoryDAO.ModifierId = ProblemHistory.ModifierId;
                ProblemHistoryDAO.ProblemStatusId = ProblemHistory.ProblemStatusId;
                ProblemHistoryDAOs.Add(ProblemHistoryDAO);
            }
            await DataContext.BulkMergeAsync(ProblemHistoryDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ProblemHistory> ProblemHistories)
        {
            List<long> Ids = ProblemHistories.Select(x => x.Id).ToList();
            await DataContext.ProblemHistory
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(ProblemHistory ProblemHistory)
        {
        }

    }
}
