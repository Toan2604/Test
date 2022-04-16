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
    public interface IShowingCategoryRepository
    {
        Task<int> Count(ShowingCategoryFilter ShowingCategoryFilter);
        Task<int> CountAll(ShowingCategoryFilter ShowingCategoryFilter);
        Task<List<ShowingCategory>> List(ShowingCategoryFilter ShowingCategoryFilter);
        Task<List<ShowingCategory>> List(List<long> Ids);
        Task<ShowingCategory> Get(long Id);
        Task<bool> Create(ShowingCategory ShowingCategory);
        Task<bool> Update(ShowingCategory ShowingCategory);
        Task<bool> Delete(ShowingCategory ShowingCategory);
        Task<bool> BulkMerge(List<ShowingCategory> ShowingCategories);
        Task<bool> BulkDelete(List<ShowingCategory> ShowingCategories);
        Task<bool> Used(List<long> Ids);
    }
    public class ShowingCategoryRepository : IShowingCategoryRepository
    {
        private DataContext DataContext;
        public ShowingCategoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ShowingCategoryDAO> DynamicFilter(IQueryable<ShowingCategoryDAO> query, ShowingCategoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ParentId.Value, filter.ParentId);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.ImageId.Value, filter.ImageId);
            query = query.Where(q => q.RowId, filter.RowId);
            return query;
        }

        private IQueryable<ShowingCategoryDAO> OrFilter(IQueryable<ShowingCategoryDAO> query, ShowingCategoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ShowingCategoryDAO> initQuery = query.Where(q => false);
            foreach (ShowingCategoryFilter ShowingCategoryFilter in filter.OrFilter)
            {
                IQueryable<ShowingCategoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                queryable = queryable.Where(q => q.Code, filter.Code);
                queryable = queryable.Where(q => q.Name, filter.Name);
                queryable = queryable.Where(q => q.ParentId.Value, filter.ParentId);
                queryable = queryable.Where(q => q.Path, filter.Path);
                queryable = queryable.Where(q => q.Level, filter.Level);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.ImageId.Value, filter.ImageId);
                queryable = queryable.Where(q => q.RowId, filter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ShowingCategoryDAO> DynamicOrder(IQueryable<ShowingCategoryDAO> query, ShowingCategoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ShowingCategoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ShowingCategoryOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ShowingCategoryOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ShowingCategoryOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case ShowingCategoryOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case ShowingCategoryOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case ShowingCategoryOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ShowingCategoryOrder.Image:
                            query = query.OrderBy(q => q.ImageId);
                            break;
                        case ShowingCategoryOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                        case ShowingCategoryOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ShowingCategoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ShowingCategoryOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ShowingCategoryOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ShowingCategoryOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case ShowingCategoryOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case ShowingCategoryOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case ShowingCategoryOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ShowingCategoryOrder.Image:
                            query = query.OrderByDescending(q => q.ImageId);
                            break;
                        case ShowingCategoryOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                        case ShowingCategoryOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ShowingCategory>> DynamicSelect(IQueryable<ShowingCategoryDAO> query, ShowingCategoryFilter filter)
        {
            List<ShowingCategory> ShowingCategories = await query.Select(q => new ShowingCategory()
            {
                Id = filter.Selects.Contains(ShowingCategorySelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ShowingCategorySelect.Code) ? q.Code : default(string),
                Description = filter.Selects.Contains(ShowingCategorySelect.Description) ? q.Description : default(string),
                Name = filter.Selects.Contains(ShowingCategorySelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(ShowingCategorySelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(ShowingCategorySelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(ShowingCategorySelect.Level) ? q.Level : default(long),
                StatusId = filter.Selects.Contains(ShowingCategorySelect.Status) ? q.StatusId : default(long),
                ImageId = filter.Selects.Contains(ShowingCategorySelect.Image) ? q.ImageId : default(long?),
                RowId = filter.Selects.Contains(ShowingCategorySelect.Row) ? q.RowId : default(Guid),
                Used = filter.Selects.Contains(ShowingCategorySelect.Used) ? q.Used : default(bool),
                Image = filter.Selects.Contains(ShowingCategorySelect.Image) && q.Image != null ? new Image
                {
                    Id = q.Image.Id,
                    Name = q.Image.Name,
                    Url = q.Image.Url,
                    ThumbnailUrl = q.Image.ThumbnailUrl,
                    RowId = q.Image.RowId,
                } : null,
                Parent = filter.Selects.Contains(ShowingCategorySelect.Parent) && q.Parent != null ? new ShowingCategory
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Level = q.Parent.Level,
                    StatusId = q.Parent.StatusId,
                    ImageId = q.Parent.ImageId,
                    RowId = q.Parent.RowId,
                    Used = q.Parent.Used,
                } : null,
                Status = filter.Selects.Contains(ShowingCategorySelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return ShowingCategories;
        }

        public async Task<int> Count(ShowingCategoryFilter filter)
        {
            IQueryable<ShowingCategoryDAO> ShowingCategories = DataContext.ShowingCategory.AsNoTracking();
            ShowingCategories = DynamicFilter(ShowingCategories, filter);
            ShowingCategories = OrFilter(ShowingCategories, filter);
            return await ShowingCategories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ShowingCategoryFilter filter)
        {
            IQueryable<ShowingCategoryDAO> ShowingCategories = DataContext.ShowingCategory.AsNoTracking();
            ShowingCategories = DynamicFilter(ShowingCategories, filter);
            return await ShowingCategories.CountWithNoLockAsync();
        }

        public async Task<List<ShowingCategory>> List(ShowingCategoryFilter filter)
        {
            if (filter == null) return new List<ShowingCategory>();
            IQueryable<ShowingCategoryDAO> ShowingCategoryDAOs = DataContext.ShowingCategory.AsNoTracking();
            ShowingCategoryDAOs = DynamicFilter(ShowingCategoryDAOs, filter);
            ShowingCategoryDAOs = OrFilter(ShowingCategoryDAOs, filter);
            ShowingCategoryDAOs = DynamicOrder(ShowingCategoryDAOs, filter);
            List<ShowingCategory> ShowingCategories = await DynamicSelect(ShowingCategoryDAOs, filter);
            return ShowingCategories;
        }

        public async Task<List<ShowingCategory>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<ShowingCategory> ShowingCategories = await DataContext.ShowingCategory.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new ShowingCategory()
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
                ImageId = x.ImageId,
                RowId = x.RowId,
                Used = x.Used,
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                    RowId = x.Image.RowId,
                },
                Parent = x.Parent == null ? null : new ShowingCategory
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    ImageId = x.Parent.ImageId,
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


            return ShowingCategories;
        }

        public async Task<ShowingCategory> Get(long Id)
        {
            ShowingCategory ShowingCategory = await DataContext.ShowingCategory.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ShowingCategory()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                StatusId = x.StatusId,
                ImageId = x.ImageId,
                RowId = x.RowId,
                Used = x.Used,
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                    RowId = x.Image.RowId,
                },
                Parent = x.Parent == null ? null : new ShowingCategory
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    ImageId = x.Parent.ImageId,
                    RowId = x.Parent.RowId,
                    Used = x.Parent.Used,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ShowingCategory == null)
                return null;

            return ShowingCategory;
        }
        public async Task<bool> Create(ShowingCategory ShowingCategory)
        {
            ShowingCategoryDAO ShowingCategoryDAO = new ShowingCategoryDAO();
            ShowingCategoryDAO.Id = ShowingCategory.Id;
            ShowingCategoryDAO.Code = ShowingCategory.Code;
            ShowingCategoryDAO.Description = ShowingCategory.Description;
            ShowingCategoryDAO.Name = ShowingCategory.Name;
            ShowingCategoryDAO.ParentId = ShowingCategory.ParentId;
            ShowingCategoryDAO.Path = ShowingCategory.Path;
            ShowingCategoryDAO.Level = ShowingCategory.Level;
            ShowingCategoryDAO.StatusId = ShowingCategory.StatusId;
            ShowingCategoryDAO.ImageId = ShowingCategory.ImageId;
            ShowingCategoryDAO.RowId = ShowingCategory.RowId;
            ShowingCategoryDAO.Used = ShowingCategory.Used;
            ShowingCategoryDAO.Path = "";
            ShowingCategoryDAO.Level = 1;
            ShowingCategoryDAO.CreatedAt = StaticParams.DateTimeNow;
            ShowingCategoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ShowingCategory.Add(ShowingCategoryDAO);
            await DataContext.SaveChangesAsync();
            ShowingCategory.Id = ShowingCategoryDAO.Id;
            await SaveReference(ShowingCategory);
            await BuildPath();
            return true;
        }

        public async Task<bool> Update(ShowingCategory ShowingCategory)
        {
            ShowingCategoryDAO ShowingCategoryDAO = DataContext.ShowingCategory.Where(x => x.Id == ShowingCategory.Id).FirstOrDefault();
            if (ShowingCategoryDAO == null)
                return false;
            ShowingCategoryDAO.Id = ShowingCategory.Id;
            ShowingCategoryDAO.Code = ShowingCategory.Code;
            ShowingCategoryDAO.Description = ShowingCategory.Description;
            ShowingCategoryDAO.Name = ShowingCategory.Name;
            ShowingCategoryDAO.ParentId = ShowingCategory.ParentId;
            ShowingCategoryDAO.Path = ShowingCategory.Path;
            ShowingCategoryDAO.Level = ShowingCategory.Level;
            ShowingCategoryDAO.StatusId = ShowingCategory.StatusId;
            ShowingCategoryDAO.ImageId = ShowingCategory.ImageId;
            ShowingCategoryDAO.RowId = ShowingCategory.RowId;
            ShowingCategoryDAO.Used = ShowingCategory.Used;
            ShowingCategoryDAO.Path = "";
            ShowingCategoryDAO.Level = 1;
            ShowingCategoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ShowingCategory);
            await BuildPath();
            return true;
        }

        public async Task<bool> Delete(ShowingCategory ShowingCategory)
        {
            ShowingCategoryDAO ShowingCategoryDAO = await DataContext.ShowingCategory.Where(x => x.Id == ShowingCategory.Id).FirstOrDefaultWithNoLockAsync();
            await DataContext.ShowingCategory.Where(x => x.Path.StartsWith(ShowingCategoryDAO.Id + ".")).UpdateFromQueryAsync(x => new ShowingCategoryDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            await DataContext.ShowingCategory.Where(x => x.Id == ShowingCategory.Id).UpdateFromQueryAsync(x => new ShowingCategoryDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            await BuildPath();
            return true;
        }

        public async Task<bool> BulkMerge(List<ShowingCategory> ShowingCategories)
        {
            List<ShowingCategoryDAO> ShowingCategoryDAOs = new List<ShowingCategoryDAO>();
            foreach (ShowingCategory ShowingCategory in ShowingCategories)
            {
                ShowingCategoryDAO ShowingCategoryDAO = new ShowingCategoryDAO();
                ShowingCategoryDAO.Id = ShowingCategory.Id;
                ShowingCategoryDAO.Code = ShowingCategory.Code;
                ShowingCategoryDAO.Name = ShowingCategory.Name;
                ShowingCategoryDAO.ParentId = ShowingCategory.ParentId;
                ShowingCategoryDAO.Path = ShowingCategory.Path;
                ShowingCategoryDAO.Level = ShowingCategory.Level;
                ShowingCategoryDAO.StatusId = ShowingCategory.StatusId;
                ShowingCategoryDAO.ImageId = ShowingCategory.ImageId;
                ShowingCategoryDAO.RowId = ShowingCategory.RowId;
                ShowingCategoryDAO.Used = ShowingCategory.Used;
                ShowingCategoryDAO.CreatedAt = StaticParams.DateTimeNow;
                ShowingCategoryDAO.UpdatedAt = StaticParams.DateTimeNow;
                ShowingCategoryDAOs.Add(ShowingCategoryDAO);
            }
            await DataContext.BulkMergeAsync(ShowingCategoryDAOs);
            await BuildPath();
            return true;
        }

        public async Task<bool> BulkDelete(List<ShowingCategory> ShowingCategories)
        {
            List<long> Ids = ShowingCategories.Select(x => x.Id).ToList();
            await DataContext.ShowingCategory
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ShowingCategoryDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            await BuildPath();
            return true;
        }

        private async Task SaveReference(ShowingCategory ShowingCategory)
        {
        }

        private async Task BuildPath()
        {
            List<ShowingCategoryDAO> ShowingCategoryDAOs = await DataContext.ShowingCategory
                .Where(x => x.DeletedAt == null)
                .AsNoTracking().ToListWithNoLockAsync();
            Queue<ShowingCategoryDAO> queue = new Queue<ShowingCategoryDAO>();
            ShowingCategoryDAOs.ForEach(x =>
            {
                if (!x.ParentId.HasValue)
                {
                    x.Path = x.Id + ".";
                    x.Level = 1;
                    queue.Enqueue(x);
                }
            });
            while (queue.Count > 0)
            {
                ShowingCategoryDAO Parent = queue.Dequeue();
                foreach (ShowingCategoryDAO ShowingCategoryDAO in ShowingCategoryDAOs)
                {
                    if (ShowingCategoryDAO.ParentId == Parent.Id)
                    {
                        ShowingCategoryDAO.Path = Parent.Path + ShowingCategoryDAO.Id + ".";
                        ShowingCategoryDAO.Level = Parent.Level + 1;
                        queue.Enqueue(ShowingCategoryDAO);
                    }
                }
            }
            await DataContext.BulkMergeAsync(ShowingCategoryDAOs);
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.ShowingCategory
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ShowingCategoryDAO { Used = true });
            return true;
        }
    }
}
