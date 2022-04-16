using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IStoreTypeRepository
    {
        Task<int> Count(StoreTypeFilter StoreTypeFilter);
        Task<List<StoreType>> List(StoreTypeFilter StoreTypeFilter);
        Task<List<StoreType>> List(List<long> Ids);
        Task<StoreType> Get(long Id);
        Task<bool> Create(StoreType StoreType);
        Task<bool> Update(StoreType StoreType);
        Task<bool> Delete(StoreType StoreType);
        Task<bool> BulkMerge(List<StoreType> StoreTypes);
        Task<bool> BulkDelete(List<StoreType> StoreTypes);
    }
    public class StoreTypeRepository : IStoreTypeRepository
    {
        private DataContext DataContext;
        public StoreTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreTypeDAO> DynamicFilter(IQueryable<StoreTypeDAO> query, StoreTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.ColorId, filter.ColorId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<StoreTypeDAO> OrFilter(IQueryable<StoreTypeDAO> query, StoreTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreTypeDAO> initQuery = query.Where(q => false);
            foreach (StoreTypeFilter StoreTypeFilter in filter.OrFilter)
            {
                IQueryable<StoreTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreTypeFilter.Name);
                queryable = queryable.Where(q => q.StatusId, StoreTypeFilter.StatusId);
                queryable = queryable.Where(q => q.ColorId, StoreTypeFilter.ColorId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreTypeDAO> DynamicOrder(IQueryable<StoreTypeDAO> query, StoreTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case StoreTypeOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case StoreTypeOrder.Color:
                            query = query.OrderBy(q => q.ColorId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case StoreTypeOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case StoreTypeOrder.Color:
                            query = query.OrderByDescending(q => q.ColorId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreType>> DynamicSelect(IQueryable<StoreTypeDAO> query, StoreTypeFilter filter)
        {
            List<StoreType> StoreTypes = await query.Select(q => new StoreType()
            {
                Id = filter.Selects.Contains(StoreTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreTypeSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(StoreTypeSelect.Status) ? q.StatusId : default(long),
                ColorId = filter.Selects.Contains(StoreTypeSelect.Color) ? q.ColorId : default(long?),
                Color = filter.Selects.Contains(StoreTypeSelect.Color) && q.Color != null ? new Color
                {
                    Id = q.Color.Id,
                    Code = q.Color.Code,
                    Name = q.Color.Name,
                } : null,
                Status = filter.Selects.Contains(StoreTypeSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Used = q.Used,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
                RowId = q.RowId,
            }).ToListAsync();
            return StoreTypes;
        }

        public async Task<int> Count(StoreTypeFilter filter)
        {
            IQueryable<StoreTypeDAO> StoreTypes = DataContext.StoreType;
            StoreTypes = DynamicFilter(StoreTypes, filter);
            return await StoreTypes.CountAsync();
        }

        public async Task<List<StoreType>> List(StoreTypeFilter filter)
        {
            if (filter == null) return new List<StoreType>();
            IQueryable<StoreTypeDAO> StoreTypeDAOs = DataContext.StoreType.AsNoTracking();
            StoreTypeDAOs = DynamicFilter(StoreTypeDAOs, filter);
            StoreTypeDAOs = DynamicOrder(StoreTypeDAOs, filter);
            List<StoreType> StoreTypes = await DynamicSelect(StoreTypeDAOs, filter);
            return StoreTypes;
        }

        public async Task<List<StoreType>> List(List<long> Ids)
        {
            List<StoreType> StoreTypes = await DataContext.StoreType.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new StoreType()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ColorId = x.ColorId,
                StatusId = x.StatusId,
                Used = x.Used,
                RowId = x.RowId,
                Color = x.Color == null ? null : new Color
                {
                    Id = x.Color.Id,
                    Code = x.Color.Code,
                    Name = x.Color.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListAsync();

            return StoreTypes;
        }

        public async Task<StoreType> Get(long Id)
        {
            StoreType StoreType = await DataContext.StoreType.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new StoreType()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ColorId = x.ColorId,
                    StatusId = x.StatusId,
                    Used = x.Used,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    RowId = x.RowId,
                    Color = x.Color == null ? null : new Color
                    {
                        Id = x.Color.Id,
                        Code = x.Color.Code,
                        Name = x.Color.Name,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultAsync();

            if (StoreType == null)
                return null;

            return StoreType;
        }
        public async Task<bool> Create(StoreType StoreType)
        {
            StoreTypeDAO StoreTypeDAO = new StoreTypeDAO();
            StoreTypeDAO.Id = StoreType.Id;
            StoreTypeDAO.Code = StoreType.Code;
            StoreTypeDAO.Name = StoreType.Name;
            StoreTypeDAO.StatusId = StoreType.StatusId;
            StoreTypeDAO.ColorId = StoreType.ColorId;
            StoreTypeDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreTypeDAO.Used = false;
            StoreTypeDAO.RowId = Guid.NewGuid();
            DataContext.StoreType.Add(StoreTypeDAO);
            await DataContext.SaveChangesAsync();
            StoreType.Id = StoreTypeDAO.Id;
            return true;
        }

        public async Task<bool> Update(StoreType StoreType)
        {
            StoreTypeDAO StoreTypeDAO = DataContext.StoreType.Where(x => x.Id == StoreType.Id).FirstOrDefault();
            if (StoreTypeDAO == null)
                return false;
            StoreTypeDAO.Id = StoreType.Id;
            StoreTypeDAO.Code = StoreType.Code;
            StoreTypeDAO.Name = StoreType.Name;
            StoreTypeDAO.StatusId = StoreType.StatusId;
            StoreTypeDAO.ColorId = StoreType.ColorId;
            StoreTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(StoreType StoreType)
        {
            await DataContext.StoreType.Where(x => x.Id == StoreType.Id).UpdateFromQueryAsync(x => new StoreTypeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreType> StoreTypes)
        {
            List<StoreTypeDAO> StoreTypeDAOs = new List<StoreTypeDAO>();
            foreach (StoreType StoreType in StoreTypes)
            {
                StoreTypeDAO StoreTypeDAO = new StoreTypeDAO();
                StoreTypeDAO.Id = StoreType.Id;
                StoreTypeDAO.Code = StoreType.Code;
                StoreTypeDAO.Name = StoreType.Name;
                StoreTypeDAO.ColorId = StoreType.ColorId;
                StoreTypeDAO.StatusId = StoreType.StatusId;
                StoreTypeDAO.CreatedAt = StaticParams.DateTimeNow;
                StoreTypeDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreTypeDAOs.Add(StoreTypeDAO);
            }
            await DataContext.BulkMergeAsync(StoreTypeDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreType> StoreTypes)
        {
            List<long> Ids = StoreTypes.Select(x => x.Id).ToList();
            await DataContext.StoreType
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreTypeDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }
    }
}
