using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IStoreGroupingRepository
    {
        Task<int> Count(StoreGroupingFilter StoreGroupingFilter);
        Task<int> CountAll(StoreGroupingFilter StoreGroupingFilter);
        Task<List<StoreGrouping>> List(StoreGroupingFilter StoreGroupingFilter);
        Task<List<StoreGrouping>> List(List<long> Ids);
        Task<StoreGrouping> Get(long Id);
        Task<bool> Create(StoreGrouping StoreGrouping);
        Task<bool> Update(StoreGrouping StoreGrouping);
        Task<bool> Delete(StoreGrouping StoreGrouping);
        Task<bool> BulkMerge(List<StoreGrouping> StoreGroupings);
        Task<bool> BulkDelete(List<StoreGrouping> StoreGroupings);
        Task<bool> Used(List<long> Ids);
    }
    public class StoreGroupingRepository : IStoreGroupingRepository
    {
        private DataContext DataContext;
        public StoreGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreGroupingDAO> DynamicFilter(IQueryable<StoreGroupingDAO> query, StoreGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ParentId, filter.ParentId);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<StoreGroupingDAO> OrFilter(IQueryable<StoreGroupingDAO> query, StoreGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreGroupingDAO> initQuery = query.Where(q => false);
            foreach (StoreGroupingFilter StoreGroupingFilter in filter.OrFilter)
            {
                IQueryable<StoreGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreGroupingFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreGroupingFilter.Code);
                queryable = queryable.Where(q => q.Name, StoreGroupingFilter.Name);
                queryable = queryable.Where(q => q.ParentId, StoreGroupingFilter.ParentId);
                queryable = queryable.Where(q => q.Path, StoreGroupingFilter.Path);
                queryable = queryable.Where(q => q.Level, StoreGroupingFilter.Level);
                queryable = queryable.Where(q => q.StatusId, StoreGroupingFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreGroupingDAO> DynamicOrder(IQueryable<StoreGroupingDAO> query, StoreGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreGroupingOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreGroupingOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case StoreGroupingOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case StoreGroupingOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case StoreGroupingOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case StoreGroupingOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreGroupingOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreGroupingOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case StoreGroupingOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case StoreGroupingOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case StoreGroupingOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case StoreGroupingOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreGrouping>> DynamicSelect(IQueryable<StoreGroupingDAO> query, StoreGroupingFilter filter)
        {
            List<StoreGrouping> StoreGroupings = await query.Select(q => new StoreGrouping()
            {
                Id = filter.Selects.Contains(StoreGroupingSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreGroupingSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(StoreGroupingSelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(StoreGroupingSelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(StoreGroupingSelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(StoreGroupingSelect.Level) ? q.Level : default(long),
                StatusId = filter.Selects.Contains(StoreGroupingSelect.Status) ? q.StatusId : default(long),
                Parent = filter.Selects.Contains(StoreGroupingSelect.Parent) && q.Parent != null ? new StoreGrouping
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Level = q.Parent.Level,
                    StatusId = q.Parent.StatusId,
                } : null,
                Status = filter.Selects.Contains(StoreGroupingSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name
                } : null,
            }).ToListWithNoLockAsync();
            return StoreGroupings;
        }

        public async Task<int> Count(StoreGroupingFilter filter)
        {
            IQueryable<StoreGroupingDAO> StoreGroupings = DataContext.StoreGrouping;
            StoreGroupings = DynamicFilter(StoreGroupings, filter);
            StoreGroupings = OrFilter(StoreGroupings, filter);
            return await StoreGroupings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(StoreGroupingFilter filter)
        {
            IQueryable<StoreGroupingDAO> StoreGroupings = DataContext.StoreGrouping;
            StoreGroupings = DynamicFilter(StoreGroupings, filter);
            return await StoreGroupings.CountWithNoLockAsync();
        }

        public async Task<List<StoreGrouping>> List(StoreGroupingFilter filter)
        {
            if (filter == null) return new List<StoreGrouping>();
            IQueryable<StoreGroupingDAO> StoreGroupingDAOs = DataContext.StoreGrouping.AsNoTracking();
            StoreGroupingDAOs = DynamicFilter(StoreGroupingDAOs, filter);
            StoreGroupingDAOs = OrFilter(StoreGroupingDAOs, filter);
            StoreGroupingDAOs = DynamicOrder(StoreGroupingDAOs, filter);
            List<StoreGrouping> StoreGroupings = await DynamicSelect(StoreGroupingDAOs, filter);
            return StoreGroupings;
        }

        public async Task<List<StoreGrouping>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
              .BulkInsertValuesIntoTempTableAsync<long>(Ids);
            var query = from s in DataContext.StoreGrouping
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;
            List<StoreGrouping> StoreGroupings = await DataContext.StoreGrouping.AsNoTracking()
            .Select(x => new StoreGrouping()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                Parent = x.Parent == null ? null : new StoreGrouping
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    RowId = x.Parent.RowId,
                    Used = x.Parent.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();

            return StoreGroupings;
        }

        public async Task<StoreGrouping> Get(long Id)
        {
            StoreGrouping StoreGrouping = await DataContext.StoreGrouping.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new StoreGrouping()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Path = x.Path,
                    Level = x.Level,
                    StatusId = x.StatusId,
                    CreatedAt = x.CreatedAt,
                    DeletedAt = x.DeletedAt,
                    UpdatedAt = x.UpdatedAt,
                    Used = x.Used,
                    RowId = x.RowId,
                    Parent = x.Parent == null ? null : new StoreGrouping
                    {
                        Id = x.Parent.Id,
                        Code = x.Parent.Code,
                        Name = x.Parent.Name,
                        ParentId = x.Parent.ParentId,
                        Path = x.Parent.Path,
                        Level = x.Parent.Level,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name
                    }
                }).FirstOrDefaultWithNoLockAsync();

            if (StoreGrouping == null)
                return null;

            return StoreGrouping;
        }
        public async Task<bool> Create(StoreGrouping StoreGrouping)
        {
            StoreGroupingDAO StoreGroupingDAO = new StoreGroupingDAO();
            StoreGroupingDAO.Id = StoreGrouping.Id;
            StoreGroupingDAO.Code = StoreGrouping.Code;
            StoreGroupingDAO.Name = StoreGrouping.Name;
            StoreGroupingDAO.ParentId = StoreGrouping.ParentId;
            StoreGroupingDAO.Path = "";
            StoreGroupingDAO.Level = 1;
            StoreGroupingDAO.StatusId = StoreGrouping.StatusId;
            StoreGroupingDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreGroupingDAO.RowId = Guid.NewGuid();
            DataContext.StoreGrouping.Add(StoreGroupingDAO);
            await DataContext.SaveChangesAsync();
            StoreGrouping.Id = StoreGroupingDAO.Id;
            await BuildPath();
            return true;
        }

        public async Task<bool> Update(StoreGrouping StoreGrouping)
        {
            StoreGroupingDAO StoreGroupingDAO = DataContext.StoreGrouping.Where(x => x.Id == StoreGrouping.Id).FirstOrDefault();
            if (StoreGroupingDAO == null)
                return false;
            StoreGroupingDAO.Id = StoreGrouping.Id;
            StoreGroupingDAO.Code = StoreGrouping.Code;
            StoreGroupingDAO.Name = StoreGrouping.Name;
            StoreGroupingDAO.ParentId = StoreGrouping.ParentId;
            StoreGroupingDAO.Path = "";
            StoreGroupingDAO.Level = 1;
            StoreGroupingDAO.StatusId = StoreGrouping.StatusId;
            StoreGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await BuildPath();
            return true;
        }

        public async Task<bool> Delete(StoreGrouping StoreGrouping)
        {
            StoreGroupingDAO StoreGroupingDAO = await DataContext.StoreGrouping.Where(x => x.Id == StoreGrouping.Id).FirstOrDefaultWithNoLockAsync();
            await DataContext.StoreGrouping.Where(x => x.Path.StartsWith(StoreGroupingDAO.Id + ".")).UpdateFromQueryAsync(x => new StoreGroupingDAO { DeletedAt = StaticParams.DateTimeNow });
            await DataContext.StoreGrouping.Where(x => x.Id == StoreGrouping.Id).UpdateFromQueryAsync(x => new StoreGroupingDAO { DeletedAt = StaticParams.DateTimeNow });
            await BuildPath();
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreGrouping> StoreGroupings)
        {
            List<StoreGroupingDAO> StoreGroupingDAOs = new List<StoreGroupingDAO>();
            foreach (StoreGrouping StoreGrouping in StoreGroupings)
            {
                StoreGroupingDAO StoreGroupingDAO = new StoreGroupingDAO();
                StoreGroupingDAO.Id = StoreGrouping.Id;
                StoreGroupingDAO.Code = StoreGrouping.Code;
                StoreGroupingDAO.Name = StoreGrouping.Name;
                StoreGroupingDAO.ParentId = StoreGrouping.ParentId;
                StoreGroupingDAO.Path = "";
                StoreGroupingDAO.Level = 1;
                StoreGroupingDAO.StatusId = StoreGrouping.StatusId;
                StoreGroupingDAO.CreatedAt = StaticParams.DateTimeNow;
                StoreGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreGroupingDAO.RowId = StoreGrouping.RowId;
                StoreGroupingDAOs.Add(StoreGroupingDAO);
            }
            await DataContext.BulkMergeAsync(StoreGroupingDAOs);
            await BuildPath();
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreGrouping> StoreGroupings)
        {
            List<long> Ids = StoreGroupings.Select(x => x.Id).ToList();
            await DataContext.StoreGrouping
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreGroupingDAO { DeletedAt = StaticParams.DateTimeNow });
            await BuildPath();
            return true;
        }

        private async Task BuildPath()
        {
            List<StoreGroupingDAO> StoreGroupingDAOs = await DataContext.StoreGrouping
                .Where(x => x.DeletedAt == null)
                .ToListWithNoLockAsync();
            Queue<StoreGroupingDAO> queue = new Queue<StoreGroupingDAO>();
            StoreGroupingDAOs.ForEach(x =>
            {
                if (!x.ParentId.HasValue)
                {
                    x.Path = x.Id + ".";
                    queue.Enqueue(x);
                }
            });
            while (queue.Count > 0)
            {
                StoreGroupingDAO Parent = queue.Dequeue();
                foreach (StoreGroupingDAO StoreGroupingDAO in StoreGroupingDAOs)
                {
                    if (StoreGroupingDAO.ParentId == Parent.Id)
                    {
                        StoreGroupingDAO.Path = Parent.Path + StoreGroupingDAO.Id + ".";
                        queue.Enqueue(StoreGroupingDAO);
                    }
                }
            }
            await DataContext.BulkMergeAsync(StoreGroupingDAOs);
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.StoreGrouping
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreGroupingDAO { Used = true });
            return true;
        }
    }
}
