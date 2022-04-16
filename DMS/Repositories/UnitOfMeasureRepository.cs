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
    public interface IUnitOfMeasureRepository
    {
        Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<int> CountAll(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter);
        Task<List<UnitOfMeasure>> List(List<long> Ids);
        Task<UnitOfMeasure> Get(long Id);
        Task<bool> BulkMerge(List<UnitOfMeasure> UnitOfMeasures);
    }
    public class UnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        private DataContext DataContext;
        public UnitOfMeasureRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<UnitOfMeasureDAO> DynamicFilter(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Description, filter.Description);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<UnitOfMeasureDAO> OrFilter(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<UnitOfMeasureDAO> initQuery = query.Where(q => false);
            foreach (UnitOfMeasureFilter UnitOfMeasureFilter in filter.OrFilter)
            {
                IQueryable<UnitOfMeasureDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, UnitOfMeasureFilter.Id);
                queryable = queryable.Where(q => q.Code, UnitOfMeasureFilter.Code);
                queryable = queryable.Where(q => q.Name, UnitOfMeasureFilter.Name);
                queryable = queryable.Where(q => q.Description, UnitOfMeasureFilter.Description);
                queryable = queryable.Where(q => q.StatusId, UnitOfMeasureFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<UnitOfMeasureDAO> DynamicOrder(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case UnitOfMeasureOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case UnitOfMeasureOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case UnitOfMeasureOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case UnitOfMeasureOrder.Description:
                            query = query.OrderBy(q => q.Description);
                            break;
                        case UnitOfMeasureOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case UnitOfMeasureOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case UnitOfMeasureOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case UnitOfMeasureOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case UnitOfMeasureOrder.Description:
                            query = query.OrderByDescending(q => q.Description);
                            break;
                        case UnitOfMeasureOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<UnitOfMeasure>> DynamicSelect(IQueryable<UnitOfMeasureDAO> query, UnitOfMeasureFilter filter)
        {
            List<UnitOfMeasure> UnitOfMeasures = await query.Select(q => new UnitOfMeasure()
            {
                Id = filter.Selects.Contains(UnitOfMeasureSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(UnitOfMeasureSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(UnitOfMeasureSelect.Name) ? q.Name : default(string),
                Description = filter.Selects.Contains(UnitOfMeasureSelect.Description) ? q.Description : default(string),
                StatusId = filter.Selects.Contains(UnitOfMeasureSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(UnitOfMeasureSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Used = q.Used,
                IsDecimal = q.IsDecimal,
            }).ToListWithNoLockAsync();
            return UnitOfMeasures;
        }

        public async Task<int> Count(UnitOfMeasureFilter filter)
        {
            IQueryable<UnitOfMeasureDAO> UnitOfMeasures = DataContext.UnitOfMeasure;
            UnitOfMeasures = DynamicFilter(UnitOfMeasures, filter);
            UnitOfMeasures = OrFilter(UnitOfMeasures, filter);
            return await UnitOfMeasures.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(UnitOfMeasureFilter filter)
        {
            IQueryable<UnitOfMeasureDAO> UnitOfMeasures = DataContext.UnitOfMeasure;
            UnitOfMeasures = DynamicFilter(UnitOfMeasures, filter);
            return await UnitOfMeasures.CountWithNoLockAsync();
        }

        public async Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter filter)
        {
            if (filter == null) return new List<UnitOfMeasure>();
            IQueryable<UnitOfMeasureDAO> UnitOfMeasureDAOs = DataContext.UnitOfMeasure.AsNoTracking();
            UnitOfMeasureDAOs = DynamicFilter(UnitOfMeasureDAOs, filter);
            UnitOfMeasureDAOs = OrFilter(UnitOfMeasureDAOs, filter);
            UnitOfMeasureDAOs = DynamicOrder(UnitOfMeasureDAOs, filter);
            List<UnitOfMeasure> UnitOfMeasures = await DynamicSelect(UnitOfMeasureDAOs, filter);
            return UnitOfMeasures;
        }

        public async Task<List<UnitOfMeasure>> List(List<long> Ids)
        {
            IQueryable<UnitOfMeasureDAO> UnitOfMeasureDAOs = DataContext.UnitOfMeasure.AsNoTracking();
            List<UnitOfMeasure> UnitOfMeasures = UnitOfMeasureDAOs.Where(q => q.Id, new IdFilter { In = Ids }).Select(x => new UnitOfMeasure()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StatusId = x.StatusId,
                Used = x.Used,
                IsDecimal = x.IsDecimal,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                RowId = x.RowId,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToList();
            return UnitOfMeasures;
        }

        public async Task<UnitOfMeasure> Get(long Id)
        {
            UnitOfMeasure UnitOfMeasure = await DataContext.UnitOfMeasure.Where(x => x.Id == Id).AsNoTracking().Select(x => new UnitOfMeasure()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                StatusId = x.StatusId,
                Used = x.Used,
                IsDecimal = x.IsDecimal,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (UnitOfMeasure == null)
                return null;

            return UnitOfMeasure;
        }
        public async Task<bool> BulkMerge(List<UnitOfMeasure> UnitOfMeasures)
        {
            List<UnitOfMeasureDAO> UnitOfMeasureDAOs = UnitOfMeasures.Select(x => new UnitOfMeasureDAO
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                Used = x.Used,
                IsDecimal = x.IsDecimal,
                RowId = x.RowId,
                StatusId = x.StatusId
            }).ToList();
            await DataContext.BulkMergeAsync(UnitOfMeasureDAOs);
            return true;
        }
    }
}
