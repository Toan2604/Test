using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MCategory
{
    public interface ICategoryService : IServiceScoped
    {
        Task<int> Count(CategoryFilter CategoryFilter);
        Task<List<Category>> List(CategoryFilter CategoryFilter);
        Task<Category> Get(long Id);
        Task<CategoryFilter> ToFilter(CategoryFilter CategoryFilter);
    }

    public class CategoryService : BaseService, ICategoryService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;

        public CategoryService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageService ImageService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageService = ImageService;
        }
        public async Task<int> Count(CategoryFilter CategoryFilter)
        {
            try
            {
                CategoryFilter.Skip = 0;
                CategoryFilter.Take = int.MaxValue;
                List<Category> Categories = await UOW.CategoryRepository.List(CategoryFilter); // lay ra cac categories thoa man
                List<Category> AllCategories = await UOW.CategoryRepository.List(new CategoryFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = CategorySelect.Id | CategorySelect.Code | CategorySelect.Name | CategorySelect.Image | CategorySelect.Parent | CategorySelect.Path | CategorySelect.Level
                }); // lay het cac categories
                foreach (Category Category in Categories)
                {
                    Category.HasChildren = AllCategories.Any(x => x.ParentId.HasValue && x.ParentId.Value == Category.Id);
                }
                if (CategoryFilter.HasChildren.HasValue && CategoryFilter.HasChildren.Value)
                {
                    Categories = Categories.Where(x => x.HasChildren).ToList();
                } // có category con
                else if (CategoryFilter.HasChildren.HasValue && !CategoryFilter.HasChildren.Value)
                {
                    Categories = Categories.Where(x => !x.HasChildren).ToList();
                } // không có category con
                int result = Categories.Count;
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CategoryService));
            }
            return 0;
        }

        public async Task<List<Category>> List(CategoryFilter CategoryFilter)
        {
            try
            {
                CategoryFilter.Skip = 0;
                CategoryFilter.Take = int.MaxValue;
                List<Category> Categories = await UOW.CategoryRepository.List(CategoryFilter); // lay ra cac categories thoa man
                List<Category> AllCategories = await UOW.CategoryRepository.List(new CategoryFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }, // mặc định lấy category đang active 
                    Selects = CategorySelect.Id | CategorySelect.Code | CategorySelect.Name | CategorySelect.Status | CategorySelect.Image | CategorySelect.Parent | CategorySelect.Path | CategorySelect.Level
                }); // lay het cac categories
                foreach (Category Category in Categories)
                {
                    Category.HasChildren = AllCategories.Any(x => x.ParentId.HasValue && x.ParentId.Value == Category.Id);
                }
                if (CategoryFilter.HasChildren.HasValue && CategoryFilter.HasChildren.Value)
                {
                    Categories = Categories.Where(x => x.HasChildren).ToList();
                } // có category con
                else if (CategoryFilter.HasChildren.HasValue && !CategoryFilter.HasChildren.Value)
                {
                    Categories = Categories.Where(x => !x.HasChildren).ToList();
                } // không có category con
                List<long> CategoryIds = Categories.Select(x => x.Id).ToList();
                
                ProductFilter ProductFilter = new ProductFilter
                {
                    CategoryId = new IdFilter { In = CategoryIds },
                    Selects = ProductSelect.Id | ProductSelect.Category,
                    Skip = 0,
                    Take = int.MaxValue,
                    OrderBy = ProductOrder.Id,
                };

                List<Product> Products;
                Products = await UOW.ProductRepository.List(ProductFilter);

                if (CategoryFilter.HasChildren.HasValue && CategoryFilter.HasChildren.Value || CategoryFilter.HasChildren == null)
                {
                    List<Category> Children = AllCategories.Where(x => x.ParentId.HasValue && CategoryIds.Contains(x.ParentId.Value)).ToList(); // lấy ra tất cả con
                    Categories = Categories.Union(Children).ToList();
                } // lấy ra đống con đi kèm nếu điều kiện có con = true hoặc bỏ trống
                foreach (Category Category in Categories)
                {
                    Category.ProductCounter = Products.Where(x => x.CategoryId == Category.Id).Count();
                }
                return Categories;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(CategoryService));
            }
            return null;
        }
        public async Task<Category> Get(long Id)
        {
            Category Category = await UOW.CategoryRepository.Get(Id);
            if (Category == null)
                return null;
            return Category;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/category/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path);
            return Image;
        }

        public async Task<CategoryFilter> ToFilter(CategoryFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<CategoryFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                CategoryFilter subFilter = new CategoryFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ParentId))
                        subFilter.ParentId = FilterBuilder.Merge(subFilter.ParentId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Path))
                        subFilter.Path = FilterBuilder.Merge(subFilter.Path, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Level))
                        subFilter.Level = FilterBuilder.Merge(subFilter.Level, FilterPermissionDefinition.LongFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
    }
}
