using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MShowingItem
{
    public interface IShowingItemService : IServiceScoped
    {
        Task<int> Count(ShowingItemFilter ShowingItemFilter);
        Task<List<ShowingItem>> List(ShowingItemFilter ShowingItemFilter);
        Task<ShowingItem> Get(long Id);
        Task<ShowingItem> Create(ShowingItem ShowingItem);
        Task<ShowingItem> Update(ShowingItem ShowingItem);
        Task<ShowingItem> Delete(ShowingItem ShowingItem);
        Task<List<ShowingItem>> BulkDelete(List<ShowingItem> ShowingItems);
        Task<List<ShowingItem>> Import(List<ShowingItem> ShowingItems);
        Task<ShowingItemFilter> ToFilter(ShowingItemFilter ShowingItemFilter);
        Task<Image> SaveImage(Image Image);
    }

    public class ShowingItemService : BaseService, IShowingItemService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IShowingItemValidator ShowingItemValidator;
        private IRabbitManager RabbitManager;
        private IImageService ImageService;

        public ShowingItemService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IShowingItemValidator ShowingItemValidator,
            ILogging Logging,
            IRabbitManager RabbitManager,
            IImageService ImageService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ShowingItemValidator = ShowingItemValidator;
            this.RabbitManager = RabbitManager;
            this.ImageService = ImageService;
        }
        public async Task<int> Count(ShowingItemFilter ShowingItemFilter)
        {
            try
            {
                int result = await UOW.ShowingItemRepository.Count(ShowingItemFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return 0;
        }

        public async Task<List<ShowingItem>> List(ShowingItemFilter ShowingItemFilter)
        {
            try
            {
                List<ShowingItem> ShowingItems = await UOW.ShowingItemRepository.List(ShowingItemFilter);
                //var Ids = ShowingItems.Select(x => x.Id).ToList();
                //AppUser AppUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                //if (AppUser != null)
                //{
                //    List<ShowingWarehouse> ShowingWarehouses = await UOW.ShowingWarehouseRepository.List(new ShowingWarehouseFilter
                //    {
                //        Skip = 0,
                //        Take = int.MaxValue,
                //        Selects = ShowingWarehouseSelect.Id,
                //        StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                //        OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
                //    });
                //    var ShowingWarehouseIds = ShowingWarehouses.Select(x => x.Id).ToList();

                //    ShowingInventoryFilter ShowingInventoryFilter = new ShowingInventoryFilter
                //    {
                //        Skip = 0,
                //        Take = int.MaxValue,
                //        ShowingItemId = new IdFilter { In = Ids },
                //        ShowingWarehouseId = new IdFilter { In = ShowingWarehouseIds },
                //        Selects = ShowingInventorySelect.SaleStock | ShowingInventorySelect.ShowingItem
                //    };

                //    var ShowingInventories = await UOW.ShowingInventoryRepository.List(ShowingInventoryFilter);
                //    var list = ShowingInventories.GroupBy(x => x.ShowingItemId).Select(x => new { ShowingItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                //    foreach (var ShowingItem in ShowingItems)
                //    {
                //        ShowingItem.SaleStock = list.Where(i => i.ShowingItemId == ShowingItem.Id).Select(i => i.SaleStock).FirstOrDefault();
                //        ShowingItem.HasInventory = ShowingItem.SaleStock > 0;
                //    }
                //}
                return ShowingItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;
        }

        public async Task<ShowingItem> Get(long Id)
        {
            ShowingItem ShowingItem = await UOW.ShowingItemRepository.Get(Id);
            if (ShowingItem == null)
                return null;
            return ShowingItem;
        }
        public async Task<ShowingItem> Create(ShowingItem ShowingItem)
        {
            if (!await ShowingItemValidator.Create(ShowingItem))
                return ShowingItem;

            try
            {
                await UOW.ShowingItemRepository.Create(ShowingItem);
                var ShowingItems = await UOW.ShowingItemRepository.List(new List<long> { ShowingItem.Id });
                Sync(ShowingItems);
                ShowingItem = ShowingItems.FirstOrDefault();
                Logging.CreateAuditLog(ShowingItem, new { }, nameof(ShowingItemService));
                return ShowingItem;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;
        }

        public async Task<ShowingItem> Update(ShowingItem ShowingItem)
        {
            if (!await ShowingItemValidator.Update(ShowingItem))
                return ShowingItem;
            try
            {
                var oldData = await UOW.ShowingItemRepository.Get(ShowingItem.Id);

                await UOW.ShowingItemRepository.Update(ShowingItem);

                var ShowingItems = await UOW.ShowingItemRepository.List(new List<long> { ShowingItem.Id });
                Sync(ShowingItems);
                ShowingItem = ShowingItems.FirstOrDefault();
                Logging.CreateAuditLog(ShowingItem, oldData, nameof(ShowingItemService));
                return ShowingItem;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;
        }

        public async Task<ShowingItem> Delete(ShowingItem ShowingItem)
        {
            if (!await ShowingItemValidator.Delete(ShowingItem))
                return ShowingItem;

            try
            {
                await UOW.ShowingItemRepository.Delete(ShowingItem);
                var ShowingItems = await UOW.ShowingItemRepository.List(new List<long> { ShowingItem.Id });
                Sync(ShowingItems);
                ShowingItem = ShowingItems.FirstOrDefault();
                Logging.CreateAuditLog(new { }, ShowingItem, nameof(ShowingItemService));
                return ShowingItem;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;
        }

        public async Task<List<ShowingItem>> BulkDelete(List<ShowingItem> ShowingItems)
        {
            if (!await ShowingItemValidator.BulkDelete(ShowingItems))
                return ShowingItems;

            try
            {
                await UOW.ShowingItemRepository.BulkDelete(ShowingItems);
                var Ids = ShowingItems.Select(x => x.Id).Distinct().ToList();
                ShowingItems = await UOW.ShowingItemRepository.List(Ids);
                Sync(ShowingItems);
                Logging.CreateAuditLog(new { }, ShowingItems, nameof(ShowingItemService));
                return ShowingItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;

        }

        public async Task<List<ShowingItem>> Import(List<ShowingItem> ShowingItems)
        {
            if (!await ShowingItemValidator.Import(ShowingItems))
                return ShowingItems;
            try
            {
                await UOW.ShowingItemRepository.BulkMerge(ShowingItems);
                var Ids = ShowingItems.Select(x => x.Id).Distinct().ToList();
                ShowingItems = await UOW.ShowingItemRepository.List(Ids);
                Sync(ShowingItems);
                Logging.CreateAuditLog(ShowingItems, new { }, nameof(ShowingItemService));
                return ShowingItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingItemService));
            }
            return null;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/showing-item/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}.{fileInfo.Extension}";
            string thumbnailPath = $"/showing-item/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}.{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path, thumbnailPath, 128, 128);
            return Image;
        }

        public async Task<ShowingItemFilter> ToFilter(ShowingItemFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ShowingItemFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ShowingItemFilter subFilter = new ShowingItemFilter();
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
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ShowingCategoryId))
                        subFilter.ShowingCategoryId = FilterBuilder.Merge(subFilter.ShowingCategoryId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.UnitOfMeasureId))
                        subFilter.UnitOfMeasureId = FilterBuilder.Merge(subFilter.UnitOfMeasureId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.SalePrice))
                        subFilter.SalePrice = FilterBuilder.Merge(subFilter.SalePrice, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Description))
                        subFilter.Description = FilterBuilder.Merge(subFilter.Description, FilterPermissionDefinition.StringFilter);
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

        private void Sync(List<ShowingItem> showingItems)
        {
            List<ShowingCategory> ShowingCategorys = showingItems.Select(x => new ShowingCategory { Id = x.ShowingCategoryId }).Distinct().ToList();
            List<UnitOfMeasure> UnitOfMeasures = showingItems.Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }).Distinct().ToList();

            RabbitManager.PublishList(showingItems, RoutingKeyEnum.ShowingItemSync.Code);
            RabbitManager.PublishList(ShowingCategorys, RoutingKeyEnum.ShowingCategoryUsed.Code);
            RabbitManager.PublishList(UnitOfMeasures, RoutingKeyEnum.UnitOfMeasureUsed.Code);
        }
    }
}
