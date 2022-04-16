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
    public interface IDistrictRepository
    {
        Task<int> Count(DistrictFilter DistrictFilter);
        Task<int> CountAll(DistrictFilter DistrictFilter);
        Task<List<District>> List(DistrictFilter DistrictFilter);
        Task<District> Get(long Id);
        Task<bool> BulkMerge(List<District> Districts);
    }
    public class DistrictRepository : IDistrictRepository
    {
        private DataContext DataContext;
        public DistrictRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<DistrictDAO> DynamicFilter(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<DistrictDAO> OrFilter(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DistrictDAO> initQuery = query.Where(q => false);
            foreach (DistrictFilter DistrictFilter in filter.OrFilter)
            {
                IQueryable<DistrictDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DistrictFilter.Id);
                queryable = queryable.Where(q => q.Code, DistrictFilter.Code);
                queryable = queryable.Where(q => q.Name, DistrictFilter.Name);
                queryable = queryable.Where(q => q.Priority, DistrictFilter.Priority);
                queryable = queryable.Where(q => q.ProvinceId, DistrictFilter.ProvinceId);
                queryable = queryable.Where(q => q.StatusId, DistrictFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<DistrictDAO> DynamicOrder(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DistrictOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DistrictOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case DistrictOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case DistrictOrder.Priority:
                            query = query.OrderBy(q => q.Priority == null).ThenBy(x => x.Priority);
                            break;
                        case DistrictOrder.Province:
                            query = query.OrderBy(q => q.ProvinceId);
                            break;
                        case DistrictOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DistrictOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DistrictOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case DistrictOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case DistrictOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority == null).ThenByDescending(x => x.Priority);
                            break;
                        case DistrictOrder.Province:
                            query = query.OrderByDescending(q => q.ProvinceId);
                            break;
                        case DistrictOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<District>> DynamicSelect(IQueryable<DistrictDAO> query, DistrictFilter filter)
        {
            List<District> Districts = await query.Select(q => new District()
            {
                Id = filter.Selects.Contains(DistrictSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(DistrictSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(DistrictSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(DistrictSelect.Priority) ? q.Priority : default(long?),
                ProvinceId = filter.Selects.Contains(DistrictSelect.Province) ? q.ProvinceId : default(long),
                StatusId = filter.Selects.Contains(DistrictSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(DistrictSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(DistrictSelect.RowId) ? q.RowId : default(Guid)
            }).ToListWithNoLockAsync();
            return Districts;
        }

        public async Task<int> Count(DistrictFilter filter)
        {
            IQueryable<DistrictDAO> Districts = DataContext.District;
            Districts = DynamicFilter(Districts, filter);
            Districts = OrFilter(Districts, filter);
            return await Districts.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(DistrictFilter filter)
        {
            IQueryable<DistrictDAO> Districts = DataContext.District;
            Districts = DynamicFilter(Districts, filter);
            return await Districts.CountWithNoLockAsync();
        }

        public async Task<List<District>> List(DistrictFilter filter)
        {
            if (filter == null) return new List<District>();
            IQueryable<DistrictDAO> DistrictDAOs = DataContext.District;
            DistrictDAOs = DynamicFilter(DistrictDAOs, filter);
            DistrictDAOs = OrFilter(DistrictDAOs, filter);
            DistrictDAOs = DynamicOrder(DistrictDAOs, filter);
            List<District> Districts = await DynamicSelect(DistrictDAOs, filter);
            return Districts;
        }

        public async Task<District> Get(long Id)
        {
            District District = await DataContext.District.Where(x => x.Id == Id).AsNoTracking().Select(x => new District()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Priority = x.Priority,
                ProvinceId = x.ProvinceId,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (District == null)
                return null;

            return District;
        }

        public async Task<bool> BulkMerge(List<District> Districts)
        {
            List<DistrictDAO> DistrictDAOs = Districts.Select(x => new DistrictDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ProvinceId = x.ProvinceId,
                Priority = x.Priority,
                RowId = x.RowId,
                StatusId = x.StatusId,
            }).ToList();
            await DataContext.BulkMergeAsync(DistrictDAOs);
            return true;
        }
    }
}
