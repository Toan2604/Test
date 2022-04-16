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
    public interface ILuckyNumberGroupingRepository
    {
        Task<int> Count(LuckyNumberGroupingFilter LuckyNumberGroupingFilter);
        Task<int> CountAll(LuckyNumberGroupingFilter LuckyNumberGroupingFilter);
        Task<List<LuckyNumberGrouping>> List(LuckyNumberGroupingFilter LuckyNumberGroupingFilter);
        Task<List<LuckyNumberGrouping>> List(List<long> Ids);
        Task<LuckyNumberGrouping> Get(long Id);
        Task<bool> Create(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> Update(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> Delete(LuckyNumberGrouping LuckyNumberGrouping);
        Task<bool> BulkMerge(List<LuckyNumberGrouping> LuckyNumberGroupings);
        Task<bool> BulkDelete(List<LuckyNumberGrouping> LuckyNumberGroupings);
    }
    public class LuckyNumberGroupingRepository : ILuckyNumberGroupingRepository
    {
        private DataContext DataContext;
        public LuckyNumberGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<LuckyNumberGroupingDAO> DynamicFilter(IQueryable<LuckyNumberGroupingDAO> query, LuckyNumberGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.EndDate == null).Union(query.Where(q => q.EndDate, filter.EndDate));
            return query;
        }

        private IQueryable<LuckyNumberGroupingDAO> OrFilter(IQueryable<LuckyNumberGroupingDAO> query, LuckyNumberGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyNumberGroupingDAO> initQuery = query.Where(q => false);
            foreach (LuckyNumberGroupingFilter LuckyNumberGroupingFilter in filter.OrFilter)
            {
                IQueryable<LuckyNumberGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyNumberGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, LuckyNumberGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, LuckyNumberGroupingFilter.Name);
                queryable = queryable.Where(q => q.OrganizationId, LuckyNumberGroupingFilter.OrganizationId);
                queryable = queryable.Where(q => q.StatusId, LuckyNumberGroupingFilter.StatusId);
                queryable = queryable.Where(q => q.StartDate, LuckyNumberGroupingFilter.StartDate);
                queryable = queryable.Where(q => q.EndDate, LuckyNumberGroupingFilter.EndDate);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<LuckyNumberGroupingDAO> DynamicOrder(IQueryable<LuckyNumberGroupingDAO> query, LuckyNumberGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyNumberGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyNumberGroupingOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case LuckyNumberGroupingOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case LuckyNumberGroupingOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case LuckyNumberGroupingOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case LuckyNumberGroupingOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case LuckyNumberGroupingOrder.EndDate:
                            query = query.OrderBy(q => q.EndDate);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyNumberGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyNumberGroupingOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case LuckyNumberGroupingOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case LuckyNumberGroupingOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case LuckyNumberGroupingOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case LuckyNumberGroupingOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case LuckyNumberGroupingOrder.EndDate:
                            query = query.OrderByDescending(q => q.EndDate);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyNumberGrouping>> DynamicSelect(IQueryable<LuckyNumberGroupingDAO> query, LuckyNumberGroupingFilter filter)
        {
            List<LuckyNumberGrouping> LuckyNumberGroupings = await query.Select(q => new LuckyNumberGrouping()
            {
                Id = filter.Selects.Contains(LuckyNumberGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(LuckyNumberGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(LuckyNumberGroupingSelect.Name) ? q.Name : default(string),
                OrganizationId = filter.Selects.Contains(LuckyNumberGroupingSelect.Organization) ? q.OrganizationId : default(long),
                StatusId = filter.Selects.Contains(LuckyNumberGroupingSelect.Status) ? q.StatusId : default(long),
                StartDate = filter.Selects.Contains(LuckyNumberGroupingSelect.StartDate) ? q.StartDate : default(DateTime),
                EndDate = filter.Selects.Contains(LuckyNumberGroupingSelect.EndDate) ? q.EndDate : default(DateTime?),
                Organization = filter.Selects.Contains(LuckyNumberGroupingSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                    RowId = q.Organization.RowId,
                } : null,
                Status = filter.Selects.Contains(LuckyNumberGroupingSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return LuckyNumberGroupings;
        }

        public async Task<int> Count(LuckyNumberGroupingFilter filter)
        {
            IQueryable<LuckyNumberGroupingDAO> LuckyNumberGroupings = DataContext.LuckyNumberGrouping.AsNoTracking();
            LuckyNumberGroupings = DynamicFilter(LuckyNumberGroupings, filter);
            LuckyNumberGroupings = OrFilter(LuckyNumberGroupings, filter);
            return await LuckyNumberGroupings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(LuckyNumberGroupingFilter filter)
        {
            IQueryable<LuckyNumberGroupingDAO> LuckyNumberGroupings = DataContext.LuckyNumberGrouping.AsNoTracking();
            LuckyNumberGroupings = DynamicFilter(LuckyNumberGroupings, filter);
            return await LuckyNumberGroupings.CountWithNoLockAsync();
        }

        public async Task<List<LuckyNumberGrouping>> List(LuckyNumberGroupingFilter filter)
        {
            if (filter == null) return new List<LuckyNumberGrouping>();
            IQueryable<LuckyNumberGroupingDAO> LuckyNumberGroupingDAOs = DataContext.LuckyNumberGrouping.AsNoTracking();
            LuckyNumberGroupingDAOs = DynamicFilter(LuckyNumberGroupingDAOs, filter);
            LuckyNumberGroupingDAOs = OrFilter(LuckyNumberGroupingDAOs, filter);
            LuckyNumberGroupingDAOs = DynamicOrder(LuckyNumberGroupingDAOs, filter);
            List<LuckyNumberGrouping> LuckyNumberGroupings = await DynamicSelect(LuckyNumberGroupingDAOs, filter);
            return LuckyNumberGroupings;
        }

        public async Task<List<LuckyNumberGrouping>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<LuckyNumberGrouping> LuckyNumberGroupings = await DataContext.LuckyNumberGrouping.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new LuckyNumberGrouping()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                OrganizationId = x.OrganizationId,
                StatusId = x.StatusId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    RowId = x.Organization.RowId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();


            return LuckyNumberGroupings;
        }

        public async Task<LuckyNumberGrouping> Get(long Id)
        {
            LuckyNumberGrouping LuckyNumberGrouping = await DataContext.LuckyNumberGrouping.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new LuckyNumberGrouping()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                OrganizationId = x.OrganizationId,
                StatusId = x.StatusId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                    RowId = x.Organization.RowId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (LuckyNumberGrouping == null)
                return null;

            LuckyNumberGrouping.LuckyNumbers = await DataContext.LuckyNumber
                .Where(x => x.LuckyNumberGroupingId == LuckyNumberGrouping.Id)
                .Select(x => new LuckyNumber
                {
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Value = x.Value,
                    RewardStatusId = x.RewardStatusId,
                    RowId = x.RowId,
                    Used = x.Used,
                    RewardStatus = x.RewardStatus == null ? null : new RewardStatus
                    {
                        Id = x.RewardStatus.Id,
                        Code = x.RewardStatus.Code,
                        Name = x.RewardStatus.Name,
                    },
                }).ToListWithNoLockAsync();
            return LuckyNumberGrouping;
        }
        public async Task<bool> Create(LuckyNumberGrouping LuckyNumberGrouping)
        {
            LuckyNumberGroupingDAO LuckyNumberGroupingDAO = new LuckyNumberGroupingDAO();
            LuckyNumberGroupingDAO.Id = LuckyNumberGrouping.Id;
            LuckyNumberGroupingDAO.Code = LuckyNumberGrouping.Code;
            LuckyNumberGroupingDAO.Name = LuckyNumberGrouping.Name;
            LuckyNumberGroupingDAO.OrganizationId = LuckyNumberGrouping.OrganizationId;
            LuckyNumberGroupingDAO.StatusId = LuckyNumberGrouping.StatusId;
            LuckyNumberGroupingDAO.StartDate = LuckyNumberGrouping.StartDate;
            LuckyNumberGroupingDAO.EndDate = LuckyNumberGrouping.EndDate;
            LuckyNumberGroupingDAO.CreatedAt = StaticParams.DateTimeNow;
            LuckyNumberGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.LuckyNumberGrouping.Add(LuckyNumberGroupingDAO);
            await DataContext.SaveChangesAsync();
            LuckyNumberGrouping.Id = LuckyNumberGroupingDAO.Id;
            await SaveReference(LuckyNumberGrouping);
            return true;
        }

        public async Task<bool> Update(LuckyNumberGrouping LuckyNumberGrouping)
        {
            LuckyNumberGroupingDAO LuckyNumberGroupingDAO = DataContext.LuckyNumberGrouping.Where(x => x.Id == LuckyNumberGrouping.Id).FirstOrDefault();
            if (LuckyNumberGroupingDAO == null)
                return false;
            LuckyNumberGroupingDAO.Id = LuckyNumberGrouping.Id;
            LuckyNumberGroupingDAO.Code = LuckyNumberGrouping.Code;
            LuckyNumberGroupingDAO.Name = LuckyNumberGrouping.Name;
            LuckyNumberGroupingDAO.OrganizationId = LuckyNumberGrouping.OrganizationId;
            LuckyNumberGroupingDAO.StatusId = LuckyNumberGrouping.StatusId;
            LuckyNumberGroupingDAO.StartDate = LuckyNumberGrouping.StartDate;
            LuckyNumberGroupingDAO.EndDate = LuckyNumberGrouping.EndDate;
            LuckyNumberGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(LuckyNumberGrouping);
            return true;
        }

        public async Task<bool> Delete(LuckyNumberGrouping LuckyNumberGrouping)
        {
            await DataContext.LuckyNumberGrouping.Where(x => x.Id == LuckyNumberGrouping.Id).UpdateFromQueryAsync(x => new LuckyNumberGroupingDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<LuckyNumberGrouping> LuckyNumberGroupings)
        {
            List<LuckyNumberGroupingDAO> LuckyNumberGroupingDAOs = new List<LuckyNumberGroupingDAO>();
            foreach (LuckyNumberGrouping LuckyNumberGrouping in LuckyNumberGroupings)
            {
                LuckyNumberGroupingDAO LuckyNumberGroupingDAO = new LuckyNumberGroupingDAO();
                LuckyNumberGroupingDAO.Id = LuckyNumberGrouping.Id;
                LuckyNumberGroupingDAO.Code = LuckyNumberGrouping.Code;
                LuckyNumberGroupingDAO.Name = LuckyNumberGrouping.Name;
                LuckyNumberGroupingDAO.OrganizationId = LuckyNumberGrouping.OrganizationId;
                LuckyNumberGroupingDAO.StatusId = LuckyNumberGrouping.StatusId;
                LuckyNumberGroupingDAO.StartDate = LuckyNumberGrouping.StartDate;
                LuckyNumberGroupingDAO.EndDate = LuckyNumberGrouping.EndDate;
                LuckyNumberGroupingDAO.CreatedAt = LuckyNumberGrouping.CreatedAt;
                LuckyNumberGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
                LuckyNumberGroupingDAOs.Add(LuckyNumberGroupingDAO);
            }
            await DataContext.BulkMergeAsync(LuckyNumberGroupingDAOs);

            foreach (var LuckyNumberGrouping in LuckyNumberGroupings)
            {
                var LuckyNumberGroupingId = LuckyNumberGroupingDAOs.Where(x => x.OrganizationId == LuckyNumberGrouping.OrganizationId).Select(x => x.Id).FirstOrDefault();
                LuckyNumberGrouping.LuckyNumbers?.ForEach(x => x.LuckyNumberGroupingId = LuckyNumberGroupingId);
            }

            List<LuckyNumberDAO> LuckyNumberDAOs = LuckyNumberGroupings.Where(x => x.LuckyNumbers != null).SelectMany(x => x.LuckyNumbers).Select(x => new LuckyNumberDAO
            {
                CreatedAt = StaticParams.DateTimeNow,
                UpdatedAt = StaticParams.DateTimeNow,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Value = x.Value,
                LuckyNumberGroupingId = x.LuckyNumberGroupingId,
                RewardStatusId = x.RewardStatusId,
                RowId = x.RowId,
                Used = x.Used,
            }).ToList();
            await DataContext.BulkMergeAsync(LuckyNumberDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyNumberGrouping> LuckyNumberGroupings)
        {
            List<long> Ids = LuckyNumberGroupings.Select(x => x.Id).ToList();
            await DataContext.LuckyNumberGrouping
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new LuckyNumberGroupingDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(LuckyNumberGrouping LuckyNumberGrouping)
        {
            var LuckyNumberIds = await DataContext.LuckyNumber.Where(x => x.LuckyNumberGroupingId == LuckyNumberGrouping.Id).Select(x => x.Id).ToListWithNoLockAsync();
            List<LuckyNumberDAO> LuckyNumberDAOs = await DataContext.LuckyNumber
                .Where(x => x.LuckyNumberGroupingId == LuckyNumberGrouping.Id).ToListWithNoLockAsync();
            foreach (LuckyNumberDAO LuckyNumberDAO in LuckyNumberDAOs)
            {
                if (LuckyNumberDAO.Used == false)
                {
                    LuckyNumberDAO.DeletedAt = StaticParams.DateTimeNow;
                }
            }
            if (LuckyNumberGrouping.LuckyNumbers != null)
            {
                foreach (LuckyNumber LuckyNumber in LuckyNumberGrouping.LuckyNumbers)
                {
                    LuckyNumberDAO LuckyNumberDAO = LuckyNumberDAOs
                        .Where(x => x.Id == LuckyNumber.Id && x.Id != 0).FirstOrDefault();
                    if (LuckyNumberDAO == null)
                    {
                        LuckyNumberDAO = new LuckyNumberDAO()
                        {
                            LuckyNumberGroupingId = LuckyNumberGrouping.Id,
                            Code = LuckyNumber.Code,
                            Name = LuckyNumber.Name,
                            Value = LuckyNumber.Value,
                            RewardStatusId = LuckyNumber.RewardStatusId,
                            RowId = Guid.NewGuid(),
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            DeletedAt = null,
                            Used = false,
                        };
                        LuckyNumberDAOs.Add(LuckyNumberDAO);
                    }
                    else
                    {
                        LuckyNumberDAO.Id = LuckyNumber.Id;
                        LuckyNumberDAO.LuckyNumberGroupingId = LuckyNumberGrouping.Id;
                        LuckyNumberDAO.Code = LuckyNumber.Code;
                        LuckyNumberDAO.Name = LuckyNumber.Name;
                        LuckyNumberDAO.Value = LuckyNumber.Value;
                        LuckyNumberDAO.RewardStatusId = LuckyNumber.RewardStatusId;
                        LuckyNumberDAO.UpdatedAt = StaticParams.DateTimeNow;
                        LuckyNumberDAO.DeletedAt = null;
                    }
                }
                await DataContext.LuckyNumber.BulkMergeAsync(LuckyNumberDAOs);
            }
        }

    }
}
