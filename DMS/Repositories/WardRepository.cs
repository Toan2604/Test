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
    public interface IWardRepository
    {
        Task<int> Count(WardFilter WardFilter);
        Task<int> CountAll(WardFilter WardFilter);
        Task<List<Ward>> List(WardFilter WardFilter);
        Task<Ward> Get(long Id);
        Task<bool> BulkMerge(List<Ward> Wards);
    }
    public class WardRepository : IWardRepository
    {
        private DataContext DataContext;
        public WardRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<WardDAO> DynamicFilter(IQueryable<WardDAO> query, WardFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<WardDAO> OrFilter(IQueryable<WardDAO> query, WardFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WardDAO> initQuery = query.Where(q => false);
            foreach (WardFilter WardFilter in filter.OrFilter)
            {
                IQueryable<WardDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WardFilter.Id);
                queryable = queryable.Where(q => q.Code, WardFilter.Code);
                queryable = queryable.Where(q => q.Name, WardFilter.Name);
                queryable = queryable.Where(q => q.Priority, WardFilter.Priority);
                queryable = queryable.Where(q => q.DistrictId, WardFilter.DistrictId);
                queryable = queryable.Where(q => q.StatusId, WardFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WardDAO> DynamicOrder(IQueryable<WardDAO> query, WardFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WardOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WardOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WardOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WardOrder.Priority:
                            query = query.OrderBy(q => q.Priority == null).ThenBy(x => x.Priority);
                            break;
                        case WardOrder.District:
                            query = query.OrderBy(q => q.DistrictId);
                            break;
                        case WardOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WardOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WardOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WardOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WardOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority == null).ThenByDescending(x => x.Priority);
                            break;
                        case WardOrder.District:
                            query = query.OrderByDescending(q => q.DistrictId);
                            break;
                        case WardOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Ward>> DynamicSelect(IQueryable<WardDAO> query, WardFilter filter)
        {
            List<Ward> Wards = await query.Select(q => new Ward()
            {
                Id = filter.Selects.Contains(WardSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(WardSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WardSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(WardSelect.Priority) ? q.Priority : default(long?),
                DistrictId = filter.Selects.Contains(WardSelect.District) ? q.DistrictId : default(long),
                StatusId = filter.Selects.Contains(WardSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(WardSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(DistrictSelect.RowId) ? q.RowId : default(Guid)
            }).ToListWithNoLockAsync();
            return Wards;
        }

        public async Task<int> Count(WardFilter filter)
        {
            IQueryable<WardDAO> Wards = DataContext.Ward;
            Wards = DynamicFilter(Wards, filter);
            Wards = OrFilter(Wards, filter);
            return await Wards.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(WardFilter filter)
        {
            IQueryable<WardDAO> Wards = DataContext.Ward;
            Wards = DynamicFilter(Wards, filter);
            return await Wards.CountWithNoLockAsync();
        }
        public async Task<List<Ward>> List(WardFilter filter)
        {
            if (filter == null) return new List<Ward>();
            IQueryable<WardDAO> WardDAOs = DataContext.Ward;
            WardDAOs = DynamicFilter(WardDAOs, filter);
            WardDAOs = OrFilter(WardDAOs, filter);
            WardDAOs = DynamicOrder(WardDAOs, filter);
            List<Ward> Wards = await DynamicSelect(WardDAOs, filter);
            return Wards;
        }

        public async Task<Ward> Get(long Id)
        {
            Ward Ward = await DataContext.Ward.Where(x => x.Id == Id).AsNoTracking().Select(x => new Ward()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                DistrictId = x.DistrictId,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Ward == null)
                return null;

            return Ward;
        }
        public async Task<bool> BulkMerge(List<Ward> Wards)
        {
            List<WardDAO> WardDAOs = Wards.Select(x => new WardDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                DistrictId = x.DistrictId,
                Priority = x.Priority,
                RowId = x.RowId,
                StatusId = x.StatusId,
            }).ToList();
            await DataContext.BulkMergeAsync(WardDAOs);
            return true;
        }
    }
}
