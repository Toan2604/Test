using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface ICategoryRepository
    {
        Task<int> Count(CategoryFilter CategoryFilter);
        Task<List<Category>> List(CategoryFilter CategoryFilter);
        Task<Category> Get(long Id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext DataContext;
        public CategoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<CategoryDAO> DynamicFilter(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ParentId, filter.ParentId);
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.ImageId, filter.ImageId);
            query = query.Where(q => q.RowId, filter.RowId);
            query = query.Where(q => q.OrderNumber, filter.OrderNumber);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<CategoryDAO> OrFilter(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<CategoryDAO> initQuery = query.Where(q => false);
            foreach (CategoryFilter CategoryFilter in filter.OrFilter)
            {
                IQueryable<CategoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, CategoryFilter.Id);
                queryable = queryable.Where(q => q.Code, CategoryFilter.Code);
                queryable = queryable.Where(q => q.Name, CategoryFilter.Name);
                queryable = queryable.Where(q => q.ParentId, CategoryFilter.ParentId);
                queryable = queryable.Where(q => q.Path, CategoryFilter.Path);
                queryable = queryable.Where(q => q.Level, CategoryFilter.Level);
                queryable = queryable.Where(q => q.StatusId, CategoryFilter.StatusId);
                queryable = queryable.Where(q => q.ImageId, CategoryFilter.ImageId);
                queryable = queryable.Where(q => q.RowId, CategoryFilter.RowId);
                queryable = queryable.Where(q => q.OrderNumber, CategoryFilter.OrderNumber);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<CategoryDAO> DynamicOrder(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case CategoryOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case CategoryOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case CategoryOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case CategoryOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case CategoryOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case CategoryOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case CategoryOrder.Image:
                            query = query.OrderBy(q => q.ImageId);
                            break;
                        case CategoryOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                        case CategoryOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                        case CategoryOrder.OrderNumber:
                            query = query.OrderBy(q => q.OrderNumber);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case CategoryOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case CategoryOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case CategoryOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case CategoryOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case CategoryOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case CategoryOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case CategoryOrder.Image:
                            query = query.OrderByDescending(q => q.ImageId);
                            break;
                        case CategoryOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                        case CategoryOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                        case CategoryOrder.OrderNumber:
                            query = query.OrderByDescending(q => q.OrderNumber);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Category>> DynamicSelect(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            List<Category> Categories = await query.Select(q => new Category()
            {
                Id = filter.Selects.Contains(CategorySelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(CategorySelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(CategorySelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(CategorySelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(CategorySelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(CategorySelect.Level) ? q.Level : default(long),
                StatusId = filter.Selects.Contains(CategorySelect.Status) ? q.StatusId : default(long),
                ImageId = filter.Selects.Contains(CategorySelect.Image) ? q.ImageId : default(long?),
                RowId = filter.Selects.Contains(CategorySelect.Row) ? q.RowId : default(Guid),
                Used = filter.Selects.Contains(CategorySelect.Used) ? q.Used : default(bool),
                OrderNumber = filter.Selects.Contains(CategorySelect.OrderNumber) ? q.OrderNumber : default(long),
                Image = filter.Selects.Contains(CategorySelect.Image) && q.Image != null ? new Image
                {
                    Id = q.Image.Id,
                    Name = q.Image.Name,
                    Url = q.Image.Url,
                    ThumbnailUrl = q.Image.ThumbnailUrl,
                } : null,
                Parent = filter.Selects.Contains(CategorySelect.Parent) && q.Parent != null ? new Category
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
                Status = filter.Selects.Contains(CategorySelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListAsync();

            foreach (var Category in Categories)
            {
                var count = Categories.Where(x => x.Path.StartsWith(Category.Path) && x.Id != Category.Id).Count();
                if (count > 0)
                    Category.HasChildren = true;
            }
            return Categories;
        }

        public async Task<int> Count(CategoryFilter filter)
        {
            IQueryable<CategoryDAO> Categories = DataContext.Category.AsNoTracking();
            Categories = DynamicFilter(Categories, filter);
            return await Categories.CountAsync();
        }

        public async Task<List<Category>> List(CategoryFilter filter)
        {
            if (filter == null) return new List<Category>();
            IQueryable<CategoryDAO> CategoryDAOs = DataContext.Category.AsNoTracking();
            CategoryDAOs = DynamicFilter(CategoryDAOs, filter);
            CategoryDAOs = DynamicOrder(CategoryDAOs, filter);
            List<Category> Categories = await DynamicSelect(CategoryDAOs, filter);
            return Categories;
        }

        public async Task<Category> Get(long Id)
        {
            Category Category = await DataContext.Category.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Category()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
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
                OrderNumber = x.OrderNumber,
                Image = x.Image == null ? null : new Image
                {
                    Id = x.Image.Id,
                    Name = x.Image.Name,
                    Url = x.Image.Url,
                    ThumbnailUrl = x.Image.ThumbnailUrl,
                },
                Parent = x.Parent == null ? null : new Category
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
            }).FirstOrDefaultAsync();

            if (Category == null)
                return null;

            return Category;
        }
    }
}
