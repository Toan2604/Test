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
    public interface IUnitOfMeasureGroupingRepository
    {
        Task<int> Count(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<int> CountAll(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter);
        Task<UnitOfMeasureGrouping> Get(long Id);
        Task<bool> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings);
    }
    public class UnitOfMeasureGroupingRepository : IUnitOfMeasureGroupingRepository
    {
        private DataContext DataContext;
        public UnitOfMeasureGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<UnitOfMeasureGroupingDAO> DynamicFilter(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<UnitOfMeasureGroupingDAO> OrFilter(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<UnitOfMeasureGroupingDAO> initQuery = query.Where(q => false);
            foreach (UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter in filter.OrFilter)
            {
                IQueryable<UnitOfMeasureGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UnitOfMeasureGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, UnitOfMeasureGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, UnitOfMeasureGroupingFilter.Name);
                queryable = queryable.Where(q => q.Description, UnitOfMeasureGroupingFilter.Description);
                queryable = queryable.Where(q => q.UnitOfMeasureId, UnitOfMeasureGroupingFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.StatusId, UnitOfMeasureGroupingFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<UnitOfMeasureGroupingDAO> DynamicOrder(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case UnitOfMeasureGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case UnitOfMeasureGroupingOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case UnitOfMeasureGroupingOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case UnitOfMeasureGroupingOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case UnitOfMeasureGroupingOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case UnitOfMeasureGroupingOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case UnitOfMeasureGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case UnitOfMeasureGroupingOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case UnitOfMeasureGroupingOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case UnitOfMeasureGroupingOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case UnitOfMeasureGroupingOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case UnitOfMeasureGroupingOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<UnitOfMeasureGrouping>> DynamicSelect(IQueryable<UnitOfMeasureGroupingDAO> query, UnitOfMeasureGroupingFilter filter)
        {
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await query.Select(q => new UnitOfMeasureGrouping()
            {
                Id = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Description) ? q.Description : default(string),
                UnitOfMeasureId = filter.Selects.Contains(UnitOfMeasureGroupingSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                StatusId = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(UnitOfMeasureGroupingSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(UnitOfMeasureGroupingSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
                Used = q.Used,
            }).ToListWithNoLockAsync();
            var Ids = UnitOfMeasureGroupings.Select(x => x.Id).ToList();
            if (filter.Selects.Contains(UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents))
            {
                var UnitOfMeasureGroupingContents = await DataContext.UnitOfMeasureGroupingContent.Where(x => x.UnitOfMeasureGroupingId, new IdFilter { In = Ids }).Select(x => new UnitOfMeasureGroupingContent
                {
                    Id = x.Id,
                    Factor = x.Factor,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Name = x.UnitOfMeasure.Name,
                        Code = x.UnitOfMeasure.Code,
                    }
                }).ToListWithNoLockAsync();
                foreach (UnitOfMeasureGrouping UnitOfMeasureGrouping in UnitOfMeasureGroupings)
                {
                    UnitOfMeasureGrouping.UnitOfMeasureGroupingContents = UnitOfMeasureGroupingContents.Where(x => x.UnitOfMeasureGroupingId == UnitOfMeasureGrouping.Id).ToList();
                }
            }
            return UnitOfMeasureGroupings;
        }

        public async Task<int> Count(UnitOfMeasureGroupingFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings = DataContext.UnitOfMeasureGrouping;
            UnitOfMeasureGroupings = DynamicFilter(UnitOfMeasureGroupings, filter);
            UnitOfMeasureGroupings = OrFilter(UnitOfMeasureGroupings, filter);
            return await UnitOfMeasureGroupings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(UnitOfMeasureGroupingFilter filter)
        {
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings = DataContext.UnitOfMeasureGrouping;
            UnitOfMeasureGroupings = DynamicFilter(UnitOfMeasureGroupings, filter);
            return await UnitOfMeasureGroupings.CountWithNoLockAsync();
        }

        public async Task<List<UnitOfMeasureGrouping>> List(UnitOfMeasureGroupingFilter filter)
        {
            if (filter == null) return new List<UnitOfMeasureGrouping>();
            IQueryable<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = DataContext.UnitOfMeasureGrouping.AsNoTracking();
            UnitOfMeasureGroupingDAOs = DynamicFilter(UnitOfMeasureGroupingDAOs, filter);
            UnitOfMeasureGroupingDAOs = OrFilter(UnitOfMeasureGroupingDAOs, filter);
            UnitOfMeasureGroupingDAOs = DynamicOrder(UnitOfMeasureGroupingDAOs, filter);
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await DynamicSelect(UnitOfMeasureGroupingDAOs, filter);
            return UnitOfMeasureGroupings;
        }

        public async Task<UnitOfMeasureGrouping> Get(long Id)
        {
            UnitOfMeasureGrouping UnitOfMeasureGrouping = await DataContext.UnitOfMeasureGrouping.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new UnitOfMeasureGrouping()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    StatusId = x.StatusId,
                    Used = x.Used,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).FirstOrDefaultWithNoLockAsync();

            if (UnitOfMeasureGrouping == null)
                return null;
            UnitOfMeasureGrouping.UnitOfMeasureGroupingContents = await DataContext.UnitOfMeasureGroupingContent
                .Where(x => x.UnitOfMeasureGroupingId == UnitOfMeasureGrouping.Id)
                .Select(x => new UnitOfMeasureGroupingContent
                {
                    Id = x.Id,
                    UnitOfMeasureGroupingId = x.UnitOfMeasureGroupingId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Factor = x.Factor,
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    }
                }).ToListWithNoLockAsync();

            return UnitOfMeasureGrouping;
        }
        public async Task<bool> BulkMerge(List<UnitOfMeasureGrouping> UnitOfMeasureGroupings)
        {
            List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs = UnitOfMeasureGroupings.Select(x => new UnitOfMeasureGroupingDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Used = x.Used,
                RowId = x.RowId,
                StatusId = x.StatusId,
                Description = x.Description,
                UnitOfMeasureId = x.UnitOfMeasureId,
            }).ToList();

            var Ids = UnitOfMeasureGroupings.Select(x => x.Id).ToList();
            await DataContext.UnitOfMeasureGroupingContent
                .Where(x => Ids.Contains(x.UnitOfMeasureGroupingId))
                .DeleteFromQueryAsync();
            List<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs = UnitOfMeasureGroupings
                .SelectMany(x => x.UnitOfMeasureGroupingContents.Select(y => new UnitOfMeasureGroupingContentDAO
                {
                    Id = y.Id,
                    Factor = y.Factor,
                    RowId = y.RowId,
                    UnitOfMeasureId = y.UnitOfMeasureId,
                    UnitOfMeasureGroupingId = y.UnitOfMeasureGroupingId,
                })).ToList();
            await DataContext.BulkMergeAsync(UnitOfMeasureGroupingDAOs);
            await DataContext.BulkMergeAsync(UnitOfMeasureGroupingContentDAOs);
            return true;
        }
    }
}
