using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface ILuckyNumberRepository
    {
        Task<int> Count(LuckyNumberFilter LuckyNumberFilter);
        Task<int> CountAll(LuckyNumberFilter LuckyNumberFilter);
        Task<List<LuckyNumber>> List(LuckyNumberFilter LuckyNumberFilter);
        Task<LuckyNumber> Get(long Id);
        Task<bool> Create(LuckyNumber LuckyNumber);
        Task<bool> Update(LuckyNumber LuckyNumber);
        Task<bool> Delete(LuckyNumber LuckyNumber);
        Task<bool> BulkMerge(List<LuckyNumber> LuckyNumbers);
        Task<bool> BulkDelete(List<LuckyNumber> LuckyNumbers);
    }
    public class LuckyNumberRepository : ILuckyNumberRepository
    {
        private DataContext DataContext;
        public LuckyNumberRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<LuckyNumberDAO> DynamicFilter(IQueryable<LuckyNumberDAO> query, LuckyNumberFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Value, filter.Value);
            query = query.Where(q => q.LuckyNumberGroupingId, filter.LuckyNumberGroupingId);
            query = query.Where(q => q.LuckyNumberGrouping.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.RewardStatusId, filter.RewardStatusId);
            query = query.Where(q => q.RowId, filter.RowId);
            query = query.Where(q => q.UsedAt, filter.UsedAt);
            return query;
        }

        private IQueryable<LuckyNumberDAO> OrFilter(IQueryable<LuckyNumberDAO> query, LuckyNumberFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyNumberDAO> initQuery = query.Where(q => false);
            foreach (LuckyNumberFilter LuckyNumberFilter in filter.OrFilter)
            {
                IQueryable<LuckyNumberDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyNumberFilter.Id);
                queryable = queryable.Where(q => q.Code, LuckyNumberFilter.Code);
                queryable = queryable.Where(q => q.Name, LuckyNumberFilter.Name);
                queryable = queryable.Where(q => q.LuckyNumberGroupingId, LuckyNumberFilter.LuckyNumberGroupingId);
                queryable = queryable.Where(q => q.RewardStatusId, LuckyNumberFilter.RewardStatusId);
                queryable = queryable.Where(q => q.RowId, LuckyNumberFilter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<LuckyNumberDAO> DynamicOrder(IQueryable<LuckyNumberDAO> query, LuckyNumberFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyNumberOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyNumberOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case LuckyNumberOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case LuckyNumberOrder.Value:
                            query = query.OrderBy(q => q.Value);
                            break;
                        case LuckyNumberOrder.LuckyNumberGrouping:
                            query = query.OrderBy(q => q.LuckyNumberGroupingId);
                            break;
                        case LuckyNumberOrder.RewardStatus:
                            query = query.OrderBy(q => q.RewardStatusId);
                            break;
                        case LuckyNumberOrder.UsedAt:
                            query = query.OrderBy(q => q.UsedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyNumberOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyNumberOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case LuckyNumberOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case LuckyNumberOrder.Value:
                            query = query.OrderByDescending(q => q.Value);
                            break;
                        case LuckyNumberOrder.LuckyNumberGrouping:
                            query = query.OrderByDescending(q => q.LuckyNumberGroupingId);
                            break;
                        case LuckyNumberOrder.RewardStatus:
                            query = query.OrderByDescending(q => q.RewardStatusId);
                            break;
                        case LuckyNumberOrder.UsedAt:
                            query = query.OrderByDescending(q => q.UsedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyNumber>> DynamicSelect(IQueryable<LuckyNumberDAO> query, LuckyNumberFilter filter)
        {
            List<LuckyNumber> LuckyNumbers = await query.Select(q => new LuckyNumber()
            {
                Id = filter.Selects.Contains(LuckyNumberSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(LuckyNumberSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(LuckyNumberSelect.Name) ? q.Name : default(string),
                Value = filter.Selects.Contains(LuckyNumberSelect.Value) ? q.Value : default(string),
                LuckyNumberGroupingId = filter.Selects.Contains(LuckyNumberSelect.LuckyNumberGrouping) ? q.LuckyNumberGroupingId : default(long),
                RewardStatusId = filter.Selects.Contains(LuckyNumberSelect.RewardStatus) ? q.RewardStatusId : default(long),
                RowId = filter.Selects.Contains(LuckyNumberSelect.Row) ? q.RowId : default(Guid),
                LuckyNumberGrouping = filter.Selects.Contains(LuckyNumberSelect.LuckyNumberGrouping) && q.LuckyNumberGrouping != null ? new LuckyNumberGrouping
                {
                    CreatedAt = q.LuckyNumberGrouping.CreatedAt,
                    UpdatedAt = q.LuckyNumberGrouping.UpdatedAt,
                    Id = q.LuckyNumberGrouping.Id,
                    Code = q.LuckyNumberGrouping.Code,
                    Name = q.LuckyNumberGrouping.Name,
                    OrganizationId = q.LuckyNumberGrouping.OrganizationId,
                    StatusId = q.LuckyNumberGrouping.StatusId,
                    StartDate = q.LuckyNumberGrouping.StartDate,
                    EndDate = q.LuckyNumberGrouping.EndDate,
                    Organization = new Organization
                    {
                        Id = q.LuckyNumberGrouping.Organization.Id,
                        Code = q.LuckyNumberGrouping.Organization.Code,
                        Name = q.LuckyNumberGrouping.Organization.Name,
                    }
                } : null,
                RewardStatus = filter.Selects.Contains(LuckyNumberSelect.RewardStatus) && q.RewardStatus != null ? new RewardStatus
                {
                    Id = q.RewardStatus.Id,
                    Code = q.RewardStatus.Code,
                    Name = q.RewardStatus.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Used = q.Used,
                UsedAt = q.UsedAt,
            }).ToListWithNoLockAsync();
            return LuckyNumbers;
        }

        public async Task<int> Count(LuckyNumberFilter filter)
        {
            IQueryable<LuckyNumberDAO> LuckyNumbers = DataContext.LuckyNumber.AsNoTracking();
            LuckyNumbers = DynamicFilter(LuckyNumbers, filter);
            LuckyNumbers = OrFilter(LuckyNumbers, filter);
            return await LuckyNumbers.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(LuckyNumberFilter filter)
        {
            IQueryable<LuckyNumberDAO> LuckyNumbers = DataContext.LuckyNumber.AsNoTracking();
            LuckyNumbers = DynamicFilter(LuckyNumbers, filter);
            return await LuckyNumbers.CountWithNoLockAsync();
        }
        public async Task<List<LuckyNumber>> List(LuckyNumberFilter filter)
        {
            if (filter == null) return new List<LuckyNumber>();
            IQueryable<LuckyNumberDAO> LuckyNumberDAOs = DataContext.LuckyNumber.AsNoTracking();
            LuckyNumberDAOs = DynamicFilter(LuckyNumberDAOs, filter);
            LuckyNumberDAOs = OrFilter(LuckyNumberDAOs, filter);
            LuckyNumberDAOs = DynamicOrder(LuckyNumberDAOs, filter);
            List<LuckyNumber> LuckyNumbers = await DynamicSelect(LuckyNumberDAOs, filter);
            return LuckyNumbers;
        }

        public async Task<LuckyNumber> Get(long Id)
        {
            LuckyNumber LuckyNumber = await DataContext.LuckyNumber.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new LuckyNumber()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Value = x.Value,
                LuckyNumberGroupingId = x.LuckyNumberGroupingId,
                RewardStatusId = x.RewardStatusId,
                RowId = x.RowId,
                Used = x.Used,
                UsedAt = x.UsedAt,
                LuckyNumberGrouping = x.LuckyNumberGrouping == null ? null : new LuckyNumberGrouping
                {
                    CreatedAt = x.LuckyNumberGrouping.CreatedAt,
                    UpdatedAt = x.LuckyNumberGrouping.UpdatedAt,
                    Id = x.LuckyNumberGrouping.Id,
                    Code = x.LuckyNumberGrouping.Code,
                    Name = x.LuckyNumberGrouping.Name,
                    OrganizationId = x.LuckyNumberGrouping.OrganizationId,
                    StatusId = x.LuckyNumberGrouping.StatusId,
                    StartDate = x.LuckyNumberGrouping.StartDate,
                    EndDate = x.LuckyNumberGrouping.EndDate,
                },
                RewardStatus = x.RewardStatus == null ? null : new RewardStatus
                {
                    Id = x.RewardStatus.Id,
                    Code = x.RewardStatus.Code,
                    Name = x.RewardStatus.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (LuckyNumber == null)
                return null;

            return LuckyNumber;
        }
        public async Task<bool> Create(LuckyNumber LuckyNumber)
        {
            LuckyNumberDAO LuckyNumberDAO = new LuckyNumberDAO();
            LuckyNumberDAO.Id = LuckyNumber.Id;
            LuckyNumberDAO.Code = LuckyNumber.Code;
            LuckyNumberDAO.Name = LuckyNumber.Name;
            LuckyNumberDAO.Value = LuckyNumber.Value;
            LuckyNumberDAO.LuckyNumberGroupingId = LuckyNumber.LuckyNumberGroupingId;
            LuckyNumberDAO.RewardStatusId = LuckyNumber.RewardStatusId;
            LuckyNumberDAO.RowId = LuckyNumber.RowId;
            LuckyNumberDAO.CreatedAt = StaticParams.DateTimeNow;
            LuckyNumberDAO.UpdatedAt = StaticParams.DateTimeNow;
            LuckyNumberDAO.Used = false;
            LuckyNumberDAO.UsedAt = LuckyNumber.UsedAt;
            DataContext.LuckyNumber.Add(LuckyNumberDAO);
            await DataContext.SaveChangesAsync();
            LuckyNumber.Id = LuckyNumberDAO.Id;
            await SaveReference(LuckyNumber);
            return true;
        }

        public async Task<bool> Update(LuckyNumber LuckyNumber)
        {
            LuckyNumberDAO LuckyNumberDAO = DataContext.LuckyNumber.Where(x => x.Id == LuckyNumber.Id).FirstOrDefault();
            if (LuckyNumberDAO == null)
                return false;
            LuckyNumberDAO.Id = LuckyNumber.Id;
            LuckyNumberDAO.Code = LuckyNumber.Code;
            LuckyNumberDAO.Name = LuckyNumber.Name;
            LuckyNumberDAO.Value = LuckyNumber.Value;
            LuckyNumberDAO.LuckyNumberGroupingId = LuckyNumber.LuckyNumberGroupingId;
            LuckyNumberDAO.RewardStatusId = LuckyNumber.RewardStatusId;
            LuckyNumberDAO.RowId = LuckyNumber.RowId;
            LuckyNumberDAO.Used = LuckyNumber.Used;
            LuckyNumberDAO.UsedAt = LuckyNumber.UsedAt;
            LuckyNumberDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(LuckyNumber);
            return true;
        }

        public async Task<bool> Delete(LuckyNumber LuckyNumber)
        {
            await DataContext.LuckyNumber.Where(x => x.Id == LuckyNumber.Id).UpdateFromQueryAsync(x => new LuckyNumberDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<LuckyNumber> LuckyNumbers)
        {
            List<LuckyNumberDAO> LuckyNumberDAOs = new List<LuckyNumberDAO>();
            foreach (LuckyNumber LuckyNumber in LuckyNumbers)
            {
                LuckyNumberDAO LuckyNumberDAO = new LuckyNumberDAO();
                LuckyNumberDAO.Id = LuckyNumber.Id;
                LuckyNumberDAO.Code = LuckyNumber.Code;
                LuckyNumberDAO.Name = LuckyNumber.Name;
                LuckyNumberDAO.Value = LuckyNumber.Value;
                LuckyNumberDAO.LuckyNumberGroupingId = LuckyNumber.LuckyNumberGroupingId;
                LuckyNumberDAO.RewardStatusId = LuckyNumber.RewardStatusId;
                LuckyNumberDAO.RowId = LuckyNumber.RowId;
                LuckyNumberDAO.Used = LuckyNumber.Used;
                LuckyNumberDAO.CreatedAt = StaticParams.DateTimeNow;
                LuckyNumberDAO.UpdatedAt = StaticParams.DateTimeNow;
                LuckyNumberDAOs.Add(LuckyNumberDAO);
            }
            await DataContext.BulkMergeAsync(LuckyNumberDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyNumber> LuckyNumbers)
        {
            List<long> Ids = LuckyNumbers.Select(x => x.Id).ToList();
            await DataContext.LuckyNumber
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new LuckyNumberDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(LuckyNumber LuckyNumber)
        {
        }

    }
}
