using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MShowingOrder
{
    public interface IShowingOrderService : IServiceScoped
    {
        Task<int> Count(ShowingOrderFilter ShowingOrderFilter);
        Task<List<ShowingOrder>> List(ShowingOrderFilter ShowingOrderFilter);
        Task<List<ShowingItem>> ListShowingItem(ShowingItemFilter ShowingItemFilter);
        Task<ShowingOrder> Get(long Id);
        Task<ShowingOrder> Create(ShowingOrder ShowingOrder);
        Task<ShowingOrder> Update(ShowingOrder ShowingOrder);
        Task<ShowingOrder> Delete(ShowingOrder ShowingOrder);
        Task<List<ShowingOrder>> BulkDelete(List<ShowingOrder> ShowingOrders);
        Task<List<ShowingOrder>> Import(List<ShowingOrder> ShowingOrders);
        Task<ShowingOrderFilter> ToFilter(ShowingOrderFilter ShowingOrderFilter);
    }

    public class ShowingOrderService : BaseService, IShowingOrderService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IShowingOrderValidator ShowingOrderValidator;
        private IRabbitManager RabbitManager;

        public ShowingOrderService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IShowingOrderValidator ShowingOrderValidator,
            ILogging Logging,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ShowingOrderValidator = ShowingOrderValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(ShowingOrderFilter ShowingOrderFilter)
        {
            try
            {
                int result = await UOW.ShowingOrderRepository.Count(ShowingOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return 0;
        }

        public async Task<List<ShowingOrder>> List(ShowingOrderFilter ShowingOrderFilter)
        {
            try
            {
                List<ShowingOrder> ShowingOrders = await UOW.ShowingOrderRepository.List(ShowingOrderFilter);
                return ShowingOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<List<ShowingItem>> ListShowingItem(ShowingItemFilter ShowingItemFilter)
        {
            try
            {
                List<ShowingItem> ShowingItems = await UOW.ShowingItemRepository.List(ShowingItemFilter);
                return ShowingItems;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<ShowingOrder> Get(long Id)
        {
            ShowingOrder ShowingOrder = await UOW.ShowingOrderRepository.Get(Id);
            if (ShowingOrder == null)
                return null;
            return ShowingOrder;
        }
        public async Task<ShowingOrder> Create(ShowingOrder ShowingOrder)
        {
            if (!await ShowingOrderValidator.Create(ShowingOrder))
                return ShowingOrder;

            try
            {
                await Calculator(ShowingOrder);
                List<ShowingOrder> ShowingOrders = new List<ShowingOrder>();
                if (ShowingOrder.Stores != null && ShowingOrder.Stores.Any())
                {
                    foreach (var Store in ShowingOrder.Stores)
                    {
                        var newObj = Utils.Clone(ShowingOrder);
                        newObj.StoreId = Store.Id;
                        newObj.AppUserId = CurrentContext.UserId;
                        newObj.RowId = Guid.NewGuid();
                        ShowingOrders.Add(newObj);
                    }
                }
                await UOW.ShowingOrderRepository.BulkMerge(ShowingOrders);
                List<long> ShowingOrderIds = ShowingOrders.Select(x => x.Id).ToList();
                ShowingOrders = await UOW.ShowingOrderRepository.List(ShowingOrderIds);
                NotifyUsed(ShowingOrder);
                Sync(ShowingOrders);
                Logging.CreateAuditLog(ShowingOrder, new { }, nameof(ShowingOrderService));
                return ShowingOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<ShowingOrder> Update(ShowingOrder ShowingOrder)
        {
            if (!await ShowingOrderValidator.Update(ShowingOrder))
                return ShowingOrder;
            try
            {
                var oldData = await UOW.ShowingOrderRepository.Get(ShowingOrder.Id);
                await Calculator(ShowingOrder);
                await UOW.ShowingOrderRepository.Update(ShowingOrder);
                ShowingOrder = await UOW.ShowingOrderRepository.Get(ShowingOrder.Id);

                NotifyUsed(ShowingOrder);
                Sync(new List<ShowingOrder> { ShowingOrder });
                ShowingOrder = await UOW.ShowingOrderRepository.Get(ShowingOrder.Id);
                Logging.CreateAuditLog(ShowingOrder, oldData, nameof(ShowingOrderService));
                return ShowingOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<ShowingOrder> Delete(ShowingOrder ShowingOrder)
        {
            if (!await ShowingOrderValidator.Delete(ShowingOrder))
                return ShowingOrder;

            try
            {
                await UOW.ShowingOrderRepository.Delete(ShowingOrder);
                Sync(new List<ShowingOrder> {
                    ShowingOrder
                });
                Logging.CreateAuditLog(new { }, ShowingOrder, nameof(ShowingOrderService));
                return ShowingOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<List<ShowingOrder>> BulkDelete(List<ShowingOrder> ShowingOrders)
        {
            if (!await ShowingOrderValidator.BulkDelete(ShowingOrders))
                return ShowingOrders;

            try
            {
                await UOW.ShowingOrderRepository.BulkDelete(ShowingOrders);
                Logging.CreateAuditLog(new { }, ShowingOrders, nameof(ShowingOrderService));
                return ShowingOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;

        }

        public async Task<List<ShowingOrder>> Import(List<ShowingOrder> ShowingOrders)
        {
            if (!await ShowingOrderValidator.Import(ShowingOrders))
                return ShowingOrders;
            try
            {
                await UOW.ShowingOrderRepository.BulkMerge(ShowingOrders);

                Logging.CreateAuditLog(ShowingOrders, new { }, nameof(ShowingOrderService));
                return ShowingOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ShowingOrderService));
            }
            return null;
        }

        public async Task<ShowingOrderFilter> ToFilter(ShowingOrderFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ShowingOrderFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ShowingOrderFilter subFilter = new ShowingOrderFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Date))
                        subFilter.Date = FilterBuilder.Merge(subFilter.Date, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ShowingWarehouseId))
                        subFilter.ShowingWarehouseId = FilterBuilder.Merge(subFilter.ShowingWarehouseId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Total))
                        subFilter.Total = FilterBuilder.Merge(subFilter.Total, FilterPermissionDefinition.DecimalFilter);
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

        private async Task Calculator(ShowingOrder ShowingOrder)
        {
            var ShowingItemIds = ShowingOrder.ShowingOrderContents.Select(x => x.ShowingItemId).Distinct().ToList();
            var ShowingItems = await UOW.ShowingItemRepository.List(new ShowingItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ShowingItemSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                Id = new IdFilter { In = ShowingItemIds }
            });
            ShowingOrder.Total = 0; // tính lại total nên gán total = 0
            foreach (var ShowingOrderContent in ShowingOrder.ShowingOrderContents)
            {
                ShowingItem ShowingItem = ShowingItems.Where(x => x.Id == ShowingOrderContent.ShowingItemId).FirstOrDefault();
                if (ShowingItem != null)
                {
                    ShowingOrderContent.Amount = ShowingItem.SalePrice * ShowingOrderContent.Quantity;
                    ShowingOrder.Total += ShowingOrderContent.Amount;
                };
            }
        }

        private void NotifyUsed(ShowingOrder ShowingOrder)
        {
            {
                List<long> ShowingItemIds = ShowingOrder.ShowingOrderContents.Select(i => i.ShowingItemId).ToList();
                List<ShowingItem> ShowingItemMessages = ShowingItemIds.Select(i => new ShowingItem { Id = i }).ToList();
                RabbitManager.PublishList(ShowingItemMessages, RoutingKeyEnum.ShowingItemUsed.Code);
            }

            {
                List<long> UOMIds = ShowingOrder.ShowingOrderContents.Select(i => i.UnitOfMeasureId).ToList();
                List<UnitOfMeasure> UnitOfMeasureMessages = UOMIds.Select(x => new UnitOfMeasure { Id = x }).ToList();
                RabbitManager.PublishList(UnitOfMeasureMessages, RoutingKeyEnum.UnitOfMeasureUsed.Code);
            }

            {
                if (ShowingOrder.Stores != null)
                {
                    List<long> StoreIds = ShowingOrder.Stores.Select(x => x.Id).Distinct().ToList();
                    List<Store> storeMessages = StoreIds.Select(x => new Store { Id = x }).ToList();
                    RabbitManager.PublishList(storeMessages, RoutingKeyEnum.StoreUsed.Code);
                }
            }

            {
                AppUser AppUserMessage = new AppUser { Id = ShowingOrder.AppUserId };
                RabbitManager.PublishSingle(AppUserMessage, RoutingKeyEnum.AppUserUsed.Code);
            }

            {
                Organization OrganizationMessage = new Organization { Id = ShowingOrder.OrganizationId };
                RabbitManager.PublishSingle(OrganizationMessage, RoutingKeyEnum.OrganizationUsed.Code);
            }
        }

        private void Sync(List<ShowingOrder> ShowingOrders)
        {
            List<AppUser> AppUsers = ShowingOrders.Select(x => new AppUser { Id = x.AppUserId }).Distinct().ToList();
            List<Organization> Organizations = ShowingOrders.Select(x => new Organization { Id = x.OrganizationId }).Distinct().ToList();
            List<Store> Stores = ShowingOrders.Select(x => new Store { Id = x.StoreId }).Distinct().ToList();
            List<ShowingItem> ShowingItems = ShowingOrders.Where(x => x.ShowingOrderContents != null)
                .SelectMany(x => x.ShowingOrderContents).Select(x => new ShowingItem { Id = x.ShowingItemId }).Distinct().ToList();
            List<UnitOfMeasure> UnitOfMeasures = ShowingOrders.Where(x => x.ShowingOrderContents != null)
                .SelectMany(x => x.ShowingOrderContents).Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }).Distinct().ToList();

            RabbitManager.PublishList(ShowingOrders, RoutingKeyEnum.ShowingOrderSync.Code);
            RabbitManager.PublishList(AppUsers, RoutingKeyEnum.AppUserUsed.Code);
            RabbitManager.PublishList(Organizations, RoutingKeyEnum.OrganizationUsed.Code);
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
            RabbitManager.PublishList(ShowingItems, RoutingKeyEnum.ShowingItemUsed.Code);
            RabbitManager.PublishList(UnitOfMeasures, RoutingKeyEnum.UnitOfMeasureUsed.Code);
        }
    }
}
