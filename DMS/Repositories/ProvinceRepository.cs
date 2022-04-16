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
    public interface IProvinceRepository
    {
        Task<int> Count(ProvinceFilter ProvinceFilter);
        Task<int> CountAll(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(ProvinceFilter ProvinceFilter);
        Task<Province> Get(long Id);
        Task<bool> BulkMerge(List<Province> Provinces);
    }
    public class ProvinceRepository : IProvinceRepository
    {
        private DataContext DataContext;
        public ProvinceRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ProvinceDAO> DynamicFilter(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Priority, filter.Priority);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<ProvinceDAO> OrFilter(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ProvinceDAO> initQuery = query.Where(q => false);
            foreach (ProvinceFilter ProvinceFilter in filter.OrFilter)
            {
                IQueryable<ProvinceDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ProvinceFilter.Id);
                queryable = queryable.Where(q => q.Name, ProvinceFilter.Name);
                queryable = queryable.Where(q => q.Priority, ProvinceFilter.Priority);
                queryable = queryable.Where(q => q.StatusId, ProvinceFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ProvinceDAO> DynamicOrder(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            query = query.OrderBy(q => q.Priority == null).ThenBy(x => x.Priority);
                            break;
                        case ProvinceOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            query = query.OrderByDescending(q => q.Priority == null).ThenByDescending(x => x.Priority);
                            break;
                        case ProvinceOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Province>> DynamicSelect(IQueryable<ProvinceDAO> query, ProvinceFilter filter)
        {
            List<Province> Provinces = await query.Select(q => new Province()
            {
                Id = filter.Selects.Contains(ProvinceSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ProvinceSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ProvinceSelect.Name) ? q.Name : default(string),
                Priority = filter.Selects.Contains(ProvinceSelect.Priority) ? q.Priority : default(long?),
                StatusId = filter.Selects.Contains(ProvinceSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(ProvinceSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(ProvinceSelect.RowId) ? q.RowId : default(Guid)
            }).ToListWithNoLockAsync();
            return Provinces;
        }

        public async Task<int> Count(ProvinceFilter filter)
        {
            IQueryable<ProvinceDAO> Provinces = DataContext.Province;
            Provinces = DynamicFilter(Provinces, filter);
            Provinces = OrFilter(Provinces, filter);
            return await Provinces.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ProvinceFilter filter)
        {
            IQueryable<ProvinceDAO> Provinces = DataContext.Province;
            Provinces = DynamicFilter(Provinces, filter);
            return await Provinces.CountWithNoLockAsync();
        }

        public async Task<List<Province>> List(ProvinceFilter filter)
        {
            if (filter == null) return new List<Province>();
            IQueryable<ProvinceDAO> ProvinceDAOs = DataContext.Province;
            ProvinceDAOs = DynamicFilter(ProvinceDAOs, filter);
            ProvinceDAOs = OrFilter(ProvinceDAOs, filter);
            ProvinceDAOs = DynamicOrder(ProvinceDAOs, filter);
            List<Province> Provinces = await DynamicSelect(ProvinceDAOs, filter);
            return Provinces;
        }

        public async Task<Province> Get(long Id)
        {
            Province Province = await DataContext.Province.Where(x => x.Id == Id).AsNoTracking().Select(x => new Province()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Priority = x.Priority,
                StatusId = x.StatusId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Province == null)
                return null;

            return Province;
        }
        public async Task<bool> BulkMerge(List<Province> Provinces)
        {
            List<ProvinceDAO> ProvinceDAOs = Provinces.Select(x => new ProvinceDAO
            {
                Code = x.Code,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Name = x.Name,
                Priority = x.Priority,
                RowId = x.RowId,
                StatusId = x.StatusId,
            }).ToList();
            await DataContext.BulkMergeAsync(ProvinceDAOs);
            return true;
        }
    }
}
