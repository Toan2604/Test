using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.direct_sales_order;
using DMS.Services.MNotification;
using DMS.Services.MOrganization;
using DMS.Services.MStoreUser;
using DMS.Services.MSystemConfiguration;
using DMS.Services.MWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderService : IServiceScoped
    {
        Task<int> Count(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountNew(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListNew(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountCompleted(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListCompleted(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountInScoped(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListInScoped(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<DirectSalesOrder> Get(long Id);
        Task<DirectSalesOrder> GetDetail(long Id);
        Task<List<Item>> ListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<List<Item>> MobileListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<DirectSalesOrder> ApplyPromotionCode(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Create(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Update(DirectSalesOrder DirectSalesOrder);
        Task<List<DirectSalesOrder>> BulkMerge(List<DirectSalesOrder> DirectSalesOrders);
        Task<DirectSalesOrder> Delete(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Send(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Approve(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Reject(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrderFilter> ToFilter(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<Store>> ListStore(StoreFilter StoreFilter);
        Task<List<Store>> ListInScoped(StoreFilter StoreFilter, long AppUserId);
        Task<int> CountInScoped(StoreFilter StoreFilter, long AppUserId);
        Task<List<Product>> MobileListProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId);
        Task<long> MobileCountProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId);
    }

    public class DirectSalesOrderService : BaseService, IDirectSalesOrderService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IDirectSalesOrderTemplate DirectSalesOrderTemplate;
        private INotificationService NotificationService;
        private IDirectSalesOrderValidator DirectSalesOrderValidator;
        private IRabbitManager RabbitManager;
        private IOrganizationService OrganizationService;
        private IStoreUserService StoreUserService;
        private IWorkflowService WorkflowService;
        private ISystemConfigurationService SystemConfigurationService;

        public DirectSalesOrderService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IDirectSalesOrderTemplate DirectSalesOrderTemplate,
            INotificationService NotificationService,
            IRabbitManager RabbitManager,
            IDirectSalesOrderValidator DirectSalesOrderValidator,
            IOrganizationService OrganizationService,
            IStoreUserService StoreUserService,
            IWorkflowService WorkflowService,
            ISystemConfigurationService SystemConfigurationService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.DirectSalesOrderTemplate = DirectSalesOrderTemplate;
            this.NotificationService = NotificationService;
            this.RabbitManager = RabbitManager;
            this.DirectSalesOrderValidator = DirectSalesOrderValidator;
            this.OrganizationService = OrganizationService;
            this.StoreUserService = StoreUserService;
            this.WorkflowService = WorkflowService;
            this.SystemConfigurationService = SystemConfigurationService;
        }

        public async Task<int> Count(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.DirectSalesOrderRepository.Count(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> CountNew(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.DirectSalesOrderRepository.CountNew(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<DirectSalesOrder>> ListNew(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.ListNew(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> CountPending(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.DirectSalesOrderRepository.CountPending(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<DirectSalesOrder>> ListPending(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.ListPending(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> CountCompleted(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.DirectSalesOrderRepository.CountCompleted(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<DirectSalesOrder>> ListCompleted(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.ListCompleted(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> CountInScoped(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.DirectSalesOrderRepository.CountInScoped(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<DirectSalesOrder>> ListInScoped(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                DirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.ListInScoped(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<List<Item>> ListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);
                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);
                var Ids = Items.Select(x => x.Id).ToList();

                if (AppUser != null)
                {
                    List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = WarehouseSelect.Id,
                        StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                        OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
                    });
                    var WarehouseIds = Warehouses.Select(x => x.Id).ToList();

                    InventoryFilter InventoryFilter = new InventoryFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        ItemId = new IdFilter { In = Ids },
                        WarehouseId = new IdFilter { In = WarehouseIds },
                        Selects = InventorySelect.SaleStock | InventorySelect.Item
                    };

                    var inventories = await UOW.InventoryRepository.List(InventoryFilter);
                    var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                    foreach (var item in Items)
                    {
                        item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                        item.HasInventory = item.SaleStock > 0;
                    }

                    await ApplyPrice(Items, SalesEmployeeId, StoreId, false);
                }

                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<List<Item>> MobileListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);

                List<Item> Items;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Items = await UOW.EsItemRepository.List(ItemFilter);
                else
                    Items = await UOW.ItemRepository.List(ItemFilter);

                var Ids = Items.Select(x => x.Id).ToList();

                if (AppUser != null)
                {
                    List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = WarehouseSelect.Id,
                        StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                        OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
                    });
                    var WarehouseIds = Warehouses.Select(x => x.Id).ToList();

                    InventoryFilter InventoryFilter = new InventoryFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        ItemId = new IdFilter { In = Ids },
                        WarehouseId = new IdFilter { In = WarehouseIds },
                        Selects = InventorySelect.SaleStock | InventorySelect.Item
                    };

                    var inventories = await UOW.InventoryRepository.List(InventoryFilter);
                    var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                    foreach (var item in Items)
                    {
                        item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                        item.HasInventory = item.SaleStock > 0;
                    }

                    await ApplyPrice(Items, SalesEmployeeId, StoreId);
                    if (ItemFilter.SalePrice != null && ItemFilter.SalePrice.HasValue)
                    {
                        if (ItemFilter.SalePrice.GreaterEqual.HasValue)
                            Items = Items.Where(x => x.SalePrice >= ItemFilter.SalePrice.GreaterEqual.Value).ToList();
                        if (ItemFilter.SalePrice.LessEqual.HasValue)
                            Items = Items.Where(x => x.SalePrice <= ItemFilter.SalePrice.LessEqual.Value).ToList();
                    }
                }

                return Items;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Get(long Id)
        {
            DirectSalesOrder DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(Id);
            if (DirectSalesOrder == null)
                return null;
            DirectSalesOrder.RequestState = await WorkflowService.GetRequestState(DirectSalesOrder.RowId);
            if (DirectSalesOrder.RequestState == null)
            {
                DirectSalesOrder.RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping
                {
                    AppUserId = DirectSalesOrder.SaleEmployeeId,
                    CreatedAt = DirectSalesOrder.CreatedAt,
                    UpdatedAt = DirectSalesOrder.UpdatedAt,
                    RequestId = DirectSalesOrder.RowId,
                    AppUser = DirectSalesOrder.SaleEmployee == null ? null : new AppUser
                    {
                        Id = DirectSalesOrder.SaleEmployee.Id,
                        Username = DirectSalesOrder.SaleEmployee.Username,
                        DisplayName = DirectSalesOrder.SaleEmployee.DisplayName,
                    },
                };
                DirectSalesOrder.RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.WorkflowStateId = DirectSalesOrder.RequestStateId;
                DirectSalesOrder.RequestState = WorkflowService.GetRequestState(DirectSalesOrder.RequestStateId);
                RequestWorkflowStepMapping.WorkflowState = WorkflowService.GetWorkflowState(RequestWorkflowStepMapping.WorkflowStateId);
            }
            else
            {
                DirectSalesOrder.RequestStateId = DirectSalesOrder.RequestState.Id;
                DirectSalesOrder.RequestWorkflowStepMappings = await WorkflowService.ListRequestWorkflowStepMapping(DirectSalesOrder.RowId);
            }
            return DirectSalesOrder;
        }
        public async Task<DirectSalesOrder> GetDetail(long Id)
        {
            DirectSalesOrder DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(Id);
            if (DirectSalesOrder == null)
                return null;
            DirectSalesOrder.RequestState = await WorkflowService.GetRequestState(DirectSalesOrder.RowId);
            if (DirectSalesOrder.RequestState == null)
            {
                DirectSalesOrder.RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping
                {
                    AppUserId = DirectSalesOrder.SaleEmployeeId,
                    CreatedAt = DirectSalesOrder.CreatedAt,
                    UpdatedAt = DirectSalesOrder.UpdatedAt,
                    RequestId = DirectSalesOrder.RowId,
                    AppUser = DirectSalesOrder.SaleEmployee == null ? null : new AppUser
                    {
                        Id = DirectSalesOrder.SaleEmployee.Id,
                        Username = DirectSalesOrder.SaleEmployee.Username,
                        DisplayName = DirectSalesOrder.SaleEmployee.DisplayName,
                    },
                };
                DirectSalesOrder.RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.WorkflowStateId = DirectSalesOrder.RequestStateId;
                DirectSalesOrder.RequestState = WorkflowService.GetRequestState(DirectSalesOrder.RequestStateId);
                RequestWorkflowStepMapping.WorkflowState = WorkflowService.GetWorkflowState(RequestWorkflowStepMapping.WorkflowStateId);
            }
            else
            {
                DirectSalesOrder.RequestStateId = DirectSalesOrder.RequestState.Id;
                DirectSalesOrder.RequestWorkflowStepMappings = await WorkflowService.ListRequestWorkflowStepMapping(DirectSalesOrder.RowId);
            }
            return DirectSalesOrder;
        }
        public async Task<DirectSalesOrder> ApplyPromotionCode(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.ApplyPromotionCode(DirectSalesOrder))
                return DirectSalesOrder;

            try
            {
                await Calculator(DirectSalesOrder);
                return DirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Create(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Create(DirectSalesOrder))
                return DirectSalesOrder;

            try
            {
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(DirectSalesOrder.SaleEmployeeId);
                var BuyerStore = await UOW.StoreRepository.Get(DirectSalesOrder.BuyerStoreId);
                await Calculator(DirectSalesOrder);

                DirectSalesOrder.RequestStateId = RequestStateEnum.NEW.Id;
                DirectSalesOrder.GeneralApprovalStateId = GeneralApprovalStateEnum.NEW.Id;
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString();
                DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                DirectSalesOrder.BuyerStoreTypeId = BuyerStore.StoreTypeId;
                DirectSalesOrder.CreatorId = CurrentContext.UserId;
                DirectSalesOrder.DirectSalesOrderSourceTypeId = DirectSalesOrderSourceTypeEnum.FROM_EMPLOYEE.Id; // create DirectSalesOrder from DMS
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                DirectSalesOrder = DirectSalesOrders.FirstOrDefault();
                await UOW.DirectSalesOrderRepository.Create(DirectSalesOrder);
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString();
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);

                var PromotionCodeId = DirectSalesOrder.PromotionCodeId;
                if (DirectSalesOrder.PromotionCodeId.HasValue)
                {
                    PromotionCodeHistory PromotionCodeHistory = new PromotionCodeHistory()
                    {
                        PromotionCodeId = DirectSalesOrder.PromotionCodeId.Value,
                        AppliedAt = StaticParams.DateTimeNow,
                        RowId = DirectSalesOrder.RowId
                    };
                    await UOW.PromotionCodeHistoryRepository.Create(PromotionCodeHistory);
                }

                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                DirectSalesOrder.PromotionCodeId = PromotionCodeId;
                Sync(new List<DirectSalesOrder> { DirectSalesOrder });
                Logging.CreateAuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Update(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Update(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(DirectSalesOrder.SaleEmployeeId);
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var BuyerStore = await UOW.StoreRepository.Get(DirectSalesOrder.BuyerStoreId);
                DirectSalesOrder.BuyerStoreTypeId = BuyerStore.StoreTypeId;
                if (oldData.SaleEmployeeId != DirectSalesOrder.SaleEmployeeId)
                {
                    DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                await Calculator(DirectSalesOrder);
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);

                var RecipientRowIds = new List<Guid>();
                RecipientRowIds.Add(SaleEmployee.RowId);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                if (DirectSalesOrder.CreatorId.HasValue && DirectSalesOrder.CreatorId != DirectSalesOrder.SaleEmployeeId)
                {
                    var Creator = await UOW.AppUserRepository.GetSimple(DirectSalesOrder.CreatorId.Value);
                    RecipientRowIds.Add(Creator.RowId);
                }
                var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Selects = StoreUserSelect.RowId,
                    Take = int.MaxValue,
                    Skip = 0,
                    StoreId = new IdFilter { Equal = DirectSalesOrder.BuyerStoreId },
                });
                var StoreUserRowId = StoreUsers.Select(x => x.RowId).FirstOrDefault();


                if (DirectSalesOrder.ErpApprovalStateId != oldData.ErpApprovalStateId)
                {
                    foreach (var RecipientRowId in RecipientRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, RecipientRowId, DirectSalesOrder, CurrentUser, NotificationType.UPDATE_ERP, oldData);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    if (StoreUserRowId != null)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUserRowId, DirectSalesOrder, CurrentUser, NotificationType.UPDATE_ERP, oldData);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                if (DirectSalesOrder.DirectSalesOrderContents.Count() != oldData.DirectSalesOrderContents.Count() || DirectSalesOrder.Total != oldData.Total)
                {
                    foreach (var RecipientRowId in RecipientRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, RecipientRowId, DirectSalesOrder, CurrentUser, NotificationType.UPDATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    if (StoreUserRowId != null)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUserRowId, DirectSalesOrder, CurrentUser, NotificationType.UPDATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                   
                if (GlobalUserNotifications.Any()) RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Sync(new List<DirectSalesOrder> { DirectSalesOrder });
                Logging.CreateAuditLog(DirectSalesOrder, oldData, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<List<DirectSalesOrder>> BulkMerge(List<DirectSalesOrder> DirectSalesOrders) //đồng bộ lại dữ liệu đơn hàng nhận từ FAST về
        {
            try
            {
                List<long> Ids = DirectSalesOrders.Select(x => x.Id).ToList();
                var OldDirectSalesOrders = await UOW.DirectSalesOrderRepository.List(Ids);
                List<long> StoreIds = DirectSalesOrders.Select(x => x.BuyerStoreId).ToList();
                List<long> AppUserIds = DirectSalesOrders.Select(x => x.SaleEmployeeId).ToList();
                AppUserIds.AddRange(DirectSalesOrders.Where(x => x.CreatorId.HasValue).Select(x => x.CreatorId.Value).ToList());
                AppUserIds = AppUserIds.Distinct().ToList();
                var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Selects = StoreUserSelect.RowId | StoreUserSelect.Store,
                    Take = int.MaxValue,
                    Skip = 0,
                    StoreId = new IdFilter { In = StoreIds }
                });
                var AppUsers = await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Selects = AppUserSelect.RowId | AppUserSelect.Id,
                    Take = int.MaxValue,
                    Skip = 0,
                    Id = new IdFilter { In = AppUserIds }
                });
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                var Sender = await UOW.AppUserRepository.GetSimple(1); //admin

                await UOW.DirectSalesOrderRepository.BulkUpdate(DirectSalesOrders); //sửa lại toàn bộ content đơn hàng khi nhận dữ liệu đồng bộ về
                DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(Ids);
                foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
                {
                    List<Guid> RecipientRowIds = new List<Guid>();
                    var StoreUser = StoreUsers.FirstOrDefault(x => x.StoreId == DirectSalesOrder.BuyerStoreId);
                    if (StoreUser != null) RecipientRowIds.Add(StoreUser.RowId);
                    var appusers = AppUsers.Where(x => x.Id == DirectSalesOrder.SaleEmployeeId || (DirectSalesOrder.CreatorId.HasValue && x.Id == DirectSalesOrder.CreatorId)).ToList();
                    RecipientRowIds.AddRange(appusers.Select(x => x.RowId).ToList());
                    var oldData = OldDirectSalesOrders.FirstOrDefault(x => x.Id == DirectSalesOrder.Id);
                    if (oldData.ErpApprovalStateId != DirectSalesOrder.ErpApprovalStateId)
                    {
                        foreach (var RecipientRowId in RecipientRowIds)
                        {
                            GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(Sender.RowId, RecipientRowId, DirectSalesOrder, Sender, NotificationType.UPDATE_ERP, oldData);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                    else
                    {
                        foreach (var RecipientRowId in RecipientRowIds)
                        {
                            GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(Sender.RowId, RecipientRowId, DirectSalesOrder, Sender, NotificationType.UPDATE);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                }
                DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(Ids);
                RabbitManager.PublishList(DirectSalesOrders, RoutingKeyEnum.DirectSalesOrderSync.Code); //đồng bộ lại trạng thái ERP
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Delete(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Delete(DirectSalesOrder))
                return DirectSalesOrder;

            try
            {
                await UOW.DirectSalesOrderRepository.Delete(DirectSalesOrder);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder });
                Logging.CreateAuditLog(new { }, DirectSalesOrder, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrderFilter> ToFilter(DirectSalesOrderFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<DirectSalesOrderFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;

            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                OrderBy = OrganizationOrder.Id,
                OrderType = OrderType.ASC
            });

            foreach (var currentFilter in CurrentContext.Filters)
            {
                DirectSalesOrderFilter subFilter = new DirectSalesOrderFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                    {
                        var organizationIds = FilterOrganization(Organizations, FilterPermissionDefinition.IdFilter);
                        IdFilter IdFilter = new IdFilter { In = organizationIds };
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, IdFilter);
                    }

                    if (FilterPermissionDefinition.Name == nameof(subFilter.BuyerStoreId))
                        subFilter.BuyerStoreId = FilterBuilder.Merge(subFilter.BuyerStoreId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.Total))
                        subFilter.Total = FilterBuilder.Merge(subFilter.Total, FilterPermissionDefinition.DecimalFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrderDate))
                        subFilter.OrderDate = FilterBuilder.Merge(subFilter.OrderDate, FilterPermissionDefinition.DateFilter);

                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                            if (subFilter.AppUserId == null) subFilter.AppUserId = new IdFilter { };
                            subFilter.AppUserId.Equal = CurrentContext.UserId;
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                            if (subFilter.AppUserId == null) subFilter.AppUserId = new IdFilter { };
                            subFilter.AppUserId.NotEqual = CurrentContext.UserId;
                        }
                    }
                }
            }
            return filter;
        }
        private async Task<DirectSalesOrder> Calculator(DirectSalesOrder DirectSalesOrder)
        {
            var ProductIds = new List<long>();
            var ItemIds = new List<long>();
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                ProductIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.Item.ProductId).ToList());
                ItemIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).ToList());
            }
            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                ProductIds.AddRange(DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.Item.ProductId).ToList());
                ItemIds.AddRange(DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.ItemId).ToList());
            }
            ProductIds = ProductIds.Distinct().ToList();
            ItemIds = ItemIds.Distinct().ToList();

            ItemFilter ItemFilter = new ItemFilter
            {
                Skip = 0,
                Take = ItemIds.Count,
                Id = new IdFilter { In = ItemIds },
                Selects = ItemSelect.ALL,
            };
            var Items = await ListItem(ItemFilter, DirectSalesOrder.SaleEmployeeId, DirectSalesOrder.BuyerStoreId);

            var Products = await UOW.ProductRepository.List(new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id | ProductSelect.TaxType
            });

            var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
            });

            //sản phẩm bán
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                    var Product = Products.Where(x => DirectSalesOrderContent.Item.ProductId == x.Id).FirstOrDefault();
                    DirectSalesOrderContent.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;

                    List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
                    if (Product.UnitOfMeasureGroupingId.HasValue)
                    {
                        var UOMG = UOMGs.Where(x => x.Id == Product.UnitOfMeasureGroupingId).FirstOrDefault();
                        UnitOfMeasures = UOMG.UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasure
                        {
                            Id = x.UnitOfMeasure.Id,
                            Code = x.UnitOfMeasure.Code,
                            Name = x.UnitOfMeasure.Name,
                            Description = x.UnitOfMeasure.Description,
                            StatusId = x.UnitOfMeasure.StatusId,
                            Factor = x.Factor
                        }).ToList();
                    }

                    UnitOfMeasures.Add(new UnitOfMeasure
                    {
                        Id = Product.UnitOfMeasure.Id,
                        Code = Product.UnitOfMeasure.Code,
                        Name = Product.UnitOfMeasure.Name,
                        Description = Product.UnitOfMeasure.Description,
                        StatusId = Product.UnitOfMeasure.StatusId,
                        Factor = 1
                    });
                    var UOM = UnitOfMeasures.Where(x => DirectSalesOrderContent.UnitOfMeasureId == x.Id).FirstOrDefault();
                    //DirectSalesOrderContent.TaxPercentage = Product.TaxType.Percentage;
                    DirectSalesOrderContent.RequestedQuantity = DirectSalesOrderContent.Quantity * UOM.Factor.Value;

                    //Trường hợp không sửa giá, giá bán = giá bán cơ sở của sản phẩm * hệ số quy đổi của đơn vị tính
                    if (DirectSalesOrder.EditedPriceStatusId == EditedPriceStatusEnum.INACTIVE.Id)
                    {
                        DirectSalesOrderContent.PrimaryPrice = Item.SalePrice.GetValueOrDefault(0);
                        DirectSalesOrderContent.SalePrice = DirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value;
                        DirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id;
                    }

                    if (DirectSalesOrder.EditedPriceStatusId == EditedPriceStatusEnum.ACTIVE.Id)
                    {
                        DirectSalesOrderContent.SalePrice = DirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value;
                        if (Item.SalePrice == DirectSalesOrderContent.PrimaryPrice)
                            DirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id;
                        else
                            DirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.ACTIVE.Id;
                    }

                    //giá tiền từng line trước chiết khấu
                    var SubAmount = DirectSalesOrderContent.Quantity * DirectSalesOrderContent.SalePrice;
                    if (DirectSalesOrderContent.DiscountPercentage.HasValue)
                    {
                        DirectSalesOrderContent.DiscountAmount = SubAmount * DirectSalesOrderContent.DiscountPercentage.Value / 100;
                        DirectSalesOrderContent.DiscountAmount = Math.Round(DirectSalesOrderContent.DiscountAmount ?? 0, 0);
                        DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                    }
                    else
                    {
                        DirectSalesOrderContent.Amount = SubAmount;
                        if (DirectSalesOrderContent.DiscountAmount.HasValue && DirectSalesOrderContent.DiscountAmount.Value > 0)
                        {
                            DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                        }
                    }
                }

                //tổng trước chiết khấu
                DirectSalesOrder.SubTotal = DirectSalesOrder.DirectSalesOrderContents.Sum(x => x.Amount);

                //tính tổng chiết khấu theo % chiết khấu chung
                if (DirectSalesOrder.GeneralDiscountPercentage.HasValue && DirectSalesOrder.GeneralDiscountPercentage > 0)
                {
                    DirectSalesOrder.GeneralDiscountAmount = DirectSalesOrder.SubTotal * (DirectSalesOrder.GeneralDiscountPercentage / 100);
                    DirectSalesOrder.GeneralDiscountAmount = Math.Round(DirectSalesOrder.GeneralDiscountAmount.Value, 0);
                }
                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    //phân bổ chiết khấu chung = tổng chiết khấu chung * (tổng từng line/tổng trc chiết khấu)
                    DirectSalesOrderContent.GeneralDiscountPercentage = DirectSalesOrderContent.Amount / DirectSalesOrder.SubTotal * 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount * DirectSalesOrderContent.GeneralDiscountPercentage / 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = Math.Round(DirectSalesOrderContent.GeneralDiscountAmount ?? 0, 0);
                    //thuê từng line = (tổng từng line - chiết khấu phân bổ) * % thuế
                    DirectSalesOrderContent.TaxAmount = (DirectSalesOrderContent.Amount - (DirectSalesOrderContent.GeneralDiscountAmount.HasValue ? DirectSalesOrderContent.GeneralDiscountAmount.Value : 0)) * DirectSalesOrderContent.TaxPercentage / 100;
                    DirectSalesOrderContent.TaxAmount = Math.Round(DirectSalesOrderContent.TaxAmount ?? 0, 0);
                }

                DirectSalesOrder.TotalTaxAmount = DirectSalesOrder.DirectSalesOrderContents.Where(x => x.TaxAmount.HasValue).Sum(x => x.TaxAmount.Value);
                DirectSalesOrder.TotalTaxAmount = Math.Round(DirectSalesOrder.TotalTaxAmount, 0);
                //tổng phải thanh toán
                DirectSalesOrder.TotalAfterTax = DirectSalesOrder.SubTotal - (DirectSalesOrder.GeneralDiscountAmount.HasValue ? DirectSalesOrder.GeneralDiscountAmount.Value : 0) + DirectSalesOrder.TotalTaxAmount;
                if (!string.IsNullOrWhiteSpace(DirectSalesOrder.PromotionCode))
                {
                    await CalculatePromotionCode(DirectSalesOrder);
                }
                else
                {
                    DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax;
                }
            }
            else
            {
                DirectSalesOrder.SubTotal = 0;
                DirectSalesOrder.GeneralDiscountPercentage = null;
                DirectSalesOrder.GeneralDiscountAmount = null;
                DirectSalesOrder.TotalTaxAmount = 0;
                DirectSalesOrder.TotalAfterTax = 0;
                DirectSalesOrder.Total = 0;
            }

            //sản phẩm khuyến mãi
            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                foreach (var DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    var Product = Products.Where(x => DirectSalesOrderPromotion.Item.ProductId == x.Id).FirstOrDefault();
                    DirectSalesOrderPromotion.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;

                    List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
                    if (Product.UnitOfMeasureGroupingId.HasValue)
                    {
                        var UOMG = UOMGs.Where(x => x.Id == Product.UnitOfMeasureGroupingId).FirstOrDefault();
                        UnitOfMeasures = UOMG.UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasure
                        {
                            Id = x.UnitOfMeasure.Id,
                            Code = x.UnitOfMeasure.Code,
                            Name = x.UnitOfMeasure.Name,
                            Description = x.UnitOfMeasure.Description,
                            StatusId = x.UnitOfMeasure.StatusId,
                            Factor = x.Factor
                        }).ToList();
                    }

                    UnitOfMeasures.Add(new UnitOfMeasure
                    {
                        Id = Product.UnitOfMeasure.Id,
                        Code = Product.UnitOfMeasure.Code,
                        Name = Product.UnitOfMeasure.Name,
                        Description = Product.UnitOfMeasure.Description,
                        StatusId = Product.UnitOfMeasure.StatusId,
                        Factor = 1
                    });
                    var UOM = UnitOfMeasures.Where(x => DirectSalesOrderPromotion.UnitOfMeasureId == x.Id).FirstOrDefault();
                    DirectSalesOrderPromotion.RequestedQuantity = DirectSalesOrderPromotion.Quantity * UOM.Factor.Value;
                }
            }

            return DirectSalesOrder;
        }
        private async Task CalculatePromotionCode(DirectSalesOrder DirectSalesOrder)
        {
            PromotionCodeFilter PromotionCodeFilter = new PromotionCodeFilter()
            {
                Code = new StringFilter { Equal = DirectSalesOrder.PromotionCode },
                Skip = 0,
                Take = 1,
                Selects = PromotionCodeSelect.Id
            };
            var PromotionCodes = await UOW.PromotionCodeRepository.List(PromotionCodeFilter);
            var PromotionCodeId = PromotionCodes.Select(x => x.Id).FirstOrDefault();
            var PromotionCode = await UOW.PromotionCodeRepository.Get(PromotionCodeId);

            var ProductIds = PromotionCode.PromotionCodeProductMappings.Select(x => x.ProductId).ToList();
            var ProductIdsInOrder = DirectSalesOrder.DirectSalesOrderContents?.Select(x => x.Item.ProductId).Distinct().ToList();

            DirectSalesOrder.PromotionCodeId = PromotionCode.Id;
            if (PromotionCode.PromotionDiscountTypeId == PromotionDiscountTypeEnum.AMOUNT.Id)
            {
                if (PromotionCode.PromotionProductAppliedTypeId == PromotionProductAppliedTypeEnum.ALL.Id)
                {
                    DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax - PromotionCode.Value;
                    DirectSalesOrder.PromotionValue = PromotionCode.Value;
                }
                else
                {
                    var Intersect = ProductIdsInOrder.Intersect(ProductIds).Count();
                    if (Intersect > 0)
                    {
                        DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax - PromotionCode.Value;
                        DirectSalesOrder.PromotionValue = PromotionCode.Value;
                    }
                }
            }
            else if (PromotionCode.PromotionDiscountTypeId == PromotionDiscountTypeEnum.PERCENTAGE.Id)
            {
                decimal PromotionValue = 0;
                if (PromotionCode.PromotionProductAppliedTypeId == PromotionProductAppliedTypeEnum.ALL.Id)
                {
                    foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                    {
                        PromotionValue += (DirectSalesOrderContent.Amount - DirectSalesOrderContent.GeneralDiscountAmount.GetValueOrDefault(0) + DirectSalesOrderContent.TaxAmount.GetValueOrDefault(0)) * PromotionCode.Value / 100;
                    }
                }
                else
                {
                    var Intersect = ProductIdsInOrder.Intersect(ProductIds).ToList();
                    foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                    {
                        if (Intersect.Contains(DirectSalesOrderContent.Item.ProductId))
                            PromotionValue += (DirectSalesOrderContent.Amount - DirectSalesOrderContent.GeneralDiscountAmount.GetValueOrDefault(0) + DirectSalesOrderContent.TaxAmount.GetValueOrDefault(0)) * PromotionCode.Value / 100;
                    }
                }

                if (PromotionCode.MaxValue.HasValue && PromotionCode.MaxValue.Value < PromotionValue)
                {
                    PromotionValue = PromotionCode.MaxValue.Value;
                    DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax - PromotionValue;
                }
                else
                {
                    DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax - PromotionValue;
                }
                DirectSalesOrder.PromotionValue = PromotionValue;
                if (DirectSalesOrder.Total <= 0)
                    DirectSalesOrder.Total = 0;
            }
        }
        private async Task<List<Item>> ApplyPrice(List<Item> Items, long? SalesEmployeeId, long? StoreId, bool VAT = true)
        {
            var SalesEmployee = await UOW.AppUserRepository.Get(SalesEmployeeId.Value);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
            var OrganizationIds = Organizations
                .Where(x => x.Path.StartsWith(SalesEmployee.Organization.Path) || SalesEmployee.Organization.Path.StartsWith(x.Path))
                .Select(x => x.Id)
                .ToList();


            var ItemIds = Items.Select(x => x.Id).ToList();
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();
            PriceListItemMappingFilter PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.ALLSTORE.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingAllStore = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);
            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();
            PriceListItemMappings.AddRange(PriceListItemMappingAllStore);

            if (StoreId.HasValue)
            {
                var Store = await UOW.StoreRepository.Get(StoreId.Value);
                var parentIds = Store.Organization.Path.Split('.');
                OrganizationIds = new List<long>();
                foreach (var parentId in parentIds)
                {
                    if (!string.IsNullOrEmpty(parentId))
                    {
                        OrganizationIds.Add(parentId.ParseLong());
                    }
                };
                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STOREGROUPING.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreGroupingId = new IdFilter { In = Store.StoreStoreGroupingMappings?.Select(x => x.StoreGroupingId).ToList() },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreGrouping = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);

                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STORETYPE.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreTypeId = new IdFilter { Equal = Store.StoreTypeId },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreType = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);

                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.DETAILS.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    StoreId = new IdFilter { Equal = StoreId },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingStoreDetail = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreGrouping);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreType);
                PriceListItemMappings.AddRange(PriceListItemMappingStoreDetail);
            }

            //Áp giá theo cấu hình
            //Ưu tiên lấy giá thấp hơn
            if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 0)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MaxValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MaxValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MaxValue)
                            .Min();
                        if (targetPrice < result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MaxValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }
            //Ưu tiên lấy giá cao hơn
            else if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 1)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MinValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MinValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MinValue)
                            .Max();
                        if (targetPrice > result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MinValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }

            foreach (var item in Items)
            {
                item.SalePrice = VAT ? result[item.Id] * (1 + item.Product.TaxType.Percentage / 100) : result[item.Id];
            }

            return Items;
        }
        public async Task<DirectSalesOrder> Send(DirectSalesOrder DirectSalesOrder)
        {
            if (DirectSalesOrder.Id == 0)
                DirectSalesOrder = await Create(DirectSalesOrder);
            else
                DirectSalesOrder = await Update(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated == false)
                return DirectSalesOrder;

            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == GlobalUserTypeEnum.APPUSER.Id) //appuser bắt đầu WF
            {
                Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
                GenericEnum RequestState = await WorkflowService.Send(DirectSalesOrder.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, DirectSalesOrder.OrganizationId, Parameters);
                DirectSalesOrder.RequestStateId = RequestState.Id;
                if (RequestState.Id == RequestStateEnum.APPROVED.Id)
                {
                    if (SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER == true)
                    {
                        if (DirectSalesOrder.BuyerStore.IsStoreApprovalDirectSalesOrder == true)
                            DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.PENDING.Id;
                        else
                            DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.APPROVED.Id;
                    }
                    else if (SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER == false)
                    {
                        DirectSalesOrder.StoreApprovalStateId = null;
                    }
                } // If SystemConfiguration allow buyer store approve direct sales order and 
                  // Store is Store approval direct sales order then StoreApproval is Pending, else it is null
                  //else
                  //{
                  //    DirectSalesOrder.StoreApprovalStateId = null;
                  //}
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);

            }
            else //store user bắt đầu WF
            {
                //Nếu cấu hình cho phép cửa hàng phê duyệt
                //Check trạng thái đại lý có được phê duyệt hay không
                //Nếu không thì đưa vào WF như bình thường với trường hợp nhân viên bắt đầu WF
                if (SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER == true)
                {
                    if (DirectSalesOrder.BuyerStore.IsStoreApprovalDirectSalesOrder == true)
                    {
                        DirectSalesOrder.RequestStateId = RequestStateEnum.NEW.Id;
                        DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.PENDING.Id;
                    }
                    else
                    {
                        Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
                        GenericEnum RequestState = await WorkflowService.Send(DirectSalesOrder.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, DirectSalesOrder.OrganizationId, Parameters);
                        DirectSalesOrder.RequestStateId = RequestState.Id;
                        DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.APPROVED.Id;
                    }
                }
                //Nếu cấu hình không cho phép cửa hàng phê duyệt
                //Đưa vào WF như trường hợp với nhân viên
                else
                {
                    Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
                    GenericEnum RequestState = await WorkflowService.Send(DirectSalesOrder.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, DirectSalesOrder.OrganizationId, Parameters);
                    DirectSalesOrder.RequestStateId = RequestState.Id;
                    DirectSalesOrder.StoreApprovalStateId = null;
                }
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
            }

            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            GlobalUserNotification UserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.TOSENDER);
            GlobalUserNotifications.Add(UserNotification);
            var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Take = 1,
                Skip = 0,
                Selects = StoreUserSelect.ALL,
                StoreId = new IdFilter { Equal = DirectSalesOrder.BuyerStoreId }
            });
            var StoreUser = StoreUsers.FirstOrDefault();
            if (StoreUser != null)
            {
                GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.SEND);
                GlobalUserNotifications.Add(GlobalUserNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

            Sync(new List<DirectSalesOrder> { DirectSalesOrder });
            List<long> ProductIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.Item.ProductId).Distinct().ToList();
            SyncProductCal(ProductIds);

            return DirectSalesOrder;
        }
        public async Task<DirectSalesOrder> Approve(DirectSalesOrder DirectSalesOrder)
        {
            DirectSalesOrder = await Update(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated == false)
                return DirectSalesOrder;
            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
            await WorkflowService.Approve(DirectSalesOrder.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(DirectSalesOrder.RowId);
            DirectSalesOrder.RequestStateId = RequestState.Id;
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (RequestState.Id == RequestStateEnum.APPROVED.Id)
            {
                if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == GlobalUserTypeEnum.APPUSER.Id &&
                    SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER == true)
                {
                    if (DirectSalesOrder.BuyerStore.IsStoreApprovalDirectSalesOrder == true)
                        DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.PENDING.Id;
                    else
                        DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.APPROVED.Id;
                }
                else if (SystemConfiguration.ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER == false)
                {
                    DirectSalesOrder.StoreApprovalStateId = null;
                }
            } // If SystemConfiguration allow buyer store approve direct sales order and 
              // Store is Store approval direct sales order then StoreApproval is Pending, else it is null
            else
            {
                DirectSalesOrder.StoreApprovalStateId = null;
            }
            await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            var UserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.TOAPPROVER);
            GlobalUserNotifications.Add(UserNotification);
            var AppUserIds = new List<long> { DirectSalesOrder.SaleEmployeeId };
            if (DirectSalesOrder.CreatorId != null && DirectSalesOrder.CreatorId != DirectSalesOrder.SaleEmployeeId) AppUserIds.Add(DirectSalesOrder.CreatorId.Value);
            AppUserIds.Remove(CurrentUser.Id);
            var AppUsers = await UOW.AppUserRepository.List(AppUserIds);
            foreach (var AppUser in AppUsers)
            {
                var AppUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, AppUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.APPROVE);
                GlobalUserNotifications.Add(AppUserNotification);
            }
            var StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Take = 1,
                Skip = 0,
                Selects = StoreUserSelect.RowId,
                StoreId = new IdFilter { Equal = DirectSalesOrder.BuyerStoreId }
            });
            var StoreUser = StoreUsers.FirstOrDefault();
            if (StoreUser != null)
            {
                GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.APPROVE);
                GlobalUserNotifications.Add(GlobalUserNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
            Sync(new List<DirectSalesOrder> { DirectSalesOrder });
            List<long> ProductIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.Item.ProductId).Distinct().ToList();
            SyncProductCal(ProductIds);

            return DirectSalesOrder;
        }
        public async Task<DirectSalesOrder> Reject(DirectSalesOrder DirectSalesOrder)
        {
            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
            GenericEnum Action = await WorkflowService.Reject(DirectSalesOrder.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(DirectSalesOrder.RowId);
            DirectSalesOrder.RequestStateId = RequestState.Id;
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
            DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            var UserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.TOREJECTER);
            GlobalUserNotifications.Add(UserNotification);
            var AppUserIds = new List<long> { DirectSalesOrder.SaleEmployeeId };
            if (DirectSalesOrder.CreatorId != null && DirectSalesOrder.CreatorId != DirectSalesOrder.SaleEmployeeId) AppUserIds.Add(DirectSalesOrder.CreatorId.Value);
            AppUserIds.Remove(CurrentUser.Id);
            var AppUsers = await UOW.AppUserRepository.List(AppUserIds);
            foreach (var AppUser in AppUsers)
            {
                var AppUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, AppUser.RowId, DirectSalesOrder, CurrentUser, NotificationType.APPROVE);
                GlobalUserNotifications.Add(AppUserNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
            Sync(new List<DirectSalesOrder> { DirectSalesOrder });

            return DirectSalesOrder;
        }
        private async Task<Dictionary<string, string>> MapParameters(DirectSalesOrder DirectSalesOrder)
        {
            var AppUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            Parameters.Add(nameof(DirectSalesOrder.Id), DirectSalesOrder.Id.ToString());
            Parameters.Add(nameof(DirectSalesOrder.Code), DirectSalesOrder.Code);
            Parameters.Add(nameof(DirectSalesOrder.SaleEmployeeId), DirectSalesOrder.SaleEmployeeId.ToString());
            Parameters.Add(nameof(DirectSalesOrder.BuyerStoreId), DirectSalesOrder.BuyerStoreId.ToString());
            Parameters.Add(nameof(AppUser.DisplayName), AppUser.DisplayName);
            Parameters.Add(nameof(DirectSalesOrder.RequestStateId), DirectSalesOrder.RequestStateId.ToString());

            Parameters.Add(nameof(DirectSalesOrder.Total), DirectSalesOrder.Total.ToString());
            Parameters.Add(nameof(DirectSalesOrder.TotalDiscountAmount), DirectSalesOrder.TotalDiscountAmount.ToString());
            Parameters.Add(nameof(DirectSalesOrder.TotalRequestedQuantity), DirectSalesOrder.TotalRequestedQuantity.ToString());
            Parameters.Add(nameof(DirectSalesOrder.OrganizationId), DirectSalesOrder.OrganizationId.ToString());

            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(DirectSalesOrder.RowId);
            if (RequestWorkflowDefinitionMapping == null)
                Parameters.Add(nameof(RequestState), RequestStateEnum.NEW.Id.ToString());
            else
                Parameters.Add(nameof(RequestState), RequestWorkflowDefinitionMapping.RequestStateId.ToString());
            Parameters.Add("Username", CurrentContext.UserName);
            Parameters.Add("Type", nameof(DirectSalesOrder));
            return Parameters;
        }
        private async Task<List<Guid>> ListReceipientRowId(AppUser AppUser, string Path)
        {
            var Ids = await UOW.PermissionRepository.ListAppUser(Path);
            List<AppUser> appUsers = await UOW.AppUserRepository.List(Ids);
            List<Guid> RowIds = appUsers.Select(x => x.RowId).ToList();
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
            var OrganizationIds = Organizations
                .Where(x => x.Path.StartsWith(AppUser.Organization.Path) || AppUser.Organization.Path.StartsWith(x.Path))
                .Select(x => x.Id)
                .ToList();

            var AppUsers = await UOW.AppUserRepository.List(new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Organization | AppUserSelect.RowId,
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            var AppUserRowIds = AppUsers.Where(x => OrganizationIds.Contains(x.OrganizationId)).Select(x => x.RowId).ToList();
            AppUserRowIds = AppUserRowIds.Intersect(RowIds).Distinct().ToList();
            return AppUserRowIds;
        }
        private void Sync(List<DirectSalesOrder> DirectSalesOrders)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            AppUsers.AddRange(DirectSalesOrders.Select(x => new AppUser { Id = x.SaleEmployeeId }));
            AppUsers.AddRange(DirectSalesOrders.Where(x => x.CreatorId != null).Select(x => new AppUser { Id = x.CreatorId.Value }));
            AppUsers = AppUsers.Distinct().ToList();

            List<Organization> Organizations = DirectSalesOrders.Select(x => new Organization { Id = x.OrganizationId }).Distinct().ToList();

            string PromotionCode = DirectSalesOrders.Where(x => !string.IsNullOrWhiteSpace(x.PromotionCode)).Select(x => x.PromotionCode).Distinct().FirstOrDefault();
            var PromotionCodeFilter = new PromotionCodeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = PromotionCodeSelect.Id,
                Code = new StringFilter { Equal = PromotionCode }
            };
            var PromotionCodes = (UOW.PromotionCodeRepository.List(PromotionCodeFilter)).Result;

            List<Store> Stores = DirectSalesOrders.Select(x => new Store { Id = x.BuyerStoreId }).Distinct().ToList();

            List<Item> Items = new List<Item>();
            Items.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderContents != null)
                .SelectMany(x => x.DirectSalesOrderContents).Select(x => new Item { Id = x.ItemId }));
            Items.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderPromotions != null)
                .SelectMany(x => x.DirectSalesOrderPromotions).Select(x => new Item { Id = x.ItemId }));
            Items = Items.Distinct().ToList();
            List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
            UnitOfMeasures.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderContents != null)
                .SelectMany(x => x.DirectSalesOrderContents).Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasures.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderContents != null)
                .SelectMany(x => x.DirectSalesOrderContents).Select(x => new UnitOfMeasure { Id = x.PrimaryUnitOfMeasureId }));
            UnitOfMeasures.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderPromotions != null)
                .SelectMany(x => x.DirectSalesOrderPromotions).Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasures.AddRange(DirectSalesOrders.Where(x => x.DirectSalesOrderPromotions != null)
                .SelectMany(x => x.DirectSalesOrderPromotions).Select(x => new UnitOfMeasure { Id = x.PrimaryUnitOfMeasureId }));
            UnitOfMeasures = UnitOfMeasures.Distinct().ToList();

            RabbitManager.PublishList(DirectSalesOrders, RoutingKeyEnum.DirectSalesOrderSync.Code);
            RabbitManager.PublishList(AppUsers, RoutingKeyEnum.AppUserUsed.Code);
            RabbitManager.PublishList(Organizations, RoutingKeyEnum.OrganizationUsed.Code);
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
            RabbitManager.PublishList(PromotionCodes, RoutingKeyEnum.PromotionCodeUsed.Code);
            RabbitManager.PublishList(Items, RoutingKeyEnum.ItemUsed.Code);
            RabbitManager.PublishList(UnitOfMeasures, RoutingKeyEnum.UnitOfMeasureUsed.Code);
        }
        private void SyncProductCal(List<long> ProducIds)
        {
            var Products = ProducIds.Select(x => new Product { Id = x }).ToList();
            RabbitManager.PublishList(Products, RoutingKeyEnum.ProductCal.Code);
        }
        public async Task<List<Store>> ListStore(StoreFilter StoreFilter)
        {
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER == false)
            {
                StoreFilter.StoreStatusId = new IdFilter { NotEqual = StoreStatusEnum.DRAFT.Id };
            }
            List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
            return Stores;
        }
        public async Task<List<Store>> ListInScoped(StoreFilter StoreFilter, long AppUserId)
        {
            try
            {
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER == false)
                {
                    StoreFilter.StoreStatusId = new IdFilter { NotEqual = StoreStatusEnum.DRAFT.Id };
                }
                StoreFilter.AppUserId = new IdFilter { Equal = AppUserId };
                List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
                List<long> StoreIds = Stores.Select(x => x.Id).ToList();
                List<StoreBalance> StoreBalances = await UOW.StoreBalanceRepository.List(new StoreBalanceFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = StoreBalanceSelect.Store | StoreBalanceSelect.Organization | StoreBalanceSelect.CreditAmount | StoreBalanceSelect.DebitAmount,
                    StoreId = new IdFilter { In = StoreIds }
                });
                List<StoreBalance> BalanceByStore = new List<StoreBalance>();
                foreach (var Store in Stores)
                {
                    BalanceByStore = StoreBalances.Where(x => x.StoreId == Store.Id).ToList();
                    Store.BalanceAmount = BalanceByStore == null ? null : (decimal?)(BalanceByStore.Sum(x => x.DebitAmount) - BalanceByStore.Sum(x => x.CreditAmount));
                }
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }

            return null;
        }
        public async Task<int> CountInScoped(StoreFilter StoreFilter, long AppUserId)
        {
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER == false)
            {
                StoreFilter.StoreStatusId = new IdFilter { NotEqual = StoreStatusEnum.DRAFT.Id };
            }
            StoreFilter.AppUserId = new IdFilter { Equal = AppUserId };
            int result = await UOW.StoreRepository.Count(StoreFilter);
            return result;
        }
        public async Task<List<Product>> MobileListProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                List<Product> Products;

                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();

                if (SystemConfig.USE_ELASTICSEARCH)
                    Products = await UOW.EsProductRepository.List(ProductFilter);
                else
                    Products = await UOW.ProductRepository.List(ProductFilter);

                var ProductIds = Products.Select(x => x.Id).ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    ProductId = new IdFilter { In = ProductIds },
                    Selects = ItemSelect.ALL,
                    OrderBy = ItemOrder.Id,
                    OrderType = OrderType.ASC,
                    Skip = 0,
                    Take = int.MaxValue,
                };

                List<Item> Items = await UOW.ItemRepository.List(ItemFilter); ;
                var Ids = Items.Select(x => x.Id).ToList();

                if (AppUser != null)
                {
                    List<Warehouse> Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = WarehouseSelect.Id,
                        StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                        OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
                    });
                    var WarehouseIds = Warehouses.Select(x => x.Id).ToList();

                    InventoryFilter InventoryFilter = new InventoryFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        ItemId = new IdFilter { In = Ids },
                        WarehouseId = new IdFilter { In = WarehouseIds },
                        Selects = InventorySelect.SaleStock | InventorySelect.Item
                    };

                    var inventories = await UOW.InventoryRepository.List(InventoryFilter);
                    var list = inventories.GroupBy(x => x.ItemId).Select(x => new { ItemId = x.Key, SaleStock = x.Sum(s => s.SaleStock) }).ToList();

                    foreach (var item in Items)
                    {
                        item.SaleStock = list.Where(i => i.ItemId == item.Id).Select(i => i.SaleStock).FirstOrDefault();
                        item.HasInventory = item.SaleStock > 0;
                    }

                    await ApplyPrice(Items, SalesEmployeeId, StoreId);
                }
                var VariationGroupings = Products.SelectMany(x => x.VariationGroupings);
                var VariationGroupingIds = new List<long>();
                if (VariationGroupings != null)
                    VariationGroupingIds = VariationGroupings.Select(x => x.Id).ToList();
                VariationFilter VariationFilter = new VariationFilter
                {
                    VariationGroupingId = new IdFilter { In = VariationGroupingIds },
                    Selects = VariationSelect.ALL,
                    OrderBy = VariationOrder.Id,
                    OrderType = OrderType.ASC,
                    Skip = 0,
                    Take = int.MaxValue,
                };
                List<Variation> VariationsInDb = await UOW.VariationRepository.List(VariationFilter);
                foreach (var Product in Products)
                {
                    List<Item> items = new List<Item>();
                    if (Product.VariationGroupings != null && Product.VariationGroupings.Count > 0)
                    {
                        var Variations = VariationsInDb.Where(x => x.VariationGroupingId == Product.VariationGroupings[0].Id).ToList();
                        foreach (Variation Variation in Variations)
                        {
                            Item Item = Items.Where(x => x.ProductId == Product.Id && x.Code.Split("-").Contains(Variation.Code)).FirstOrDefault();
                            if (Item != null)
                                items.Add(Item);
                        }
                    }
                    List<Item> NonVariationsItems = Items.Where(x => x.ProductId == Product.Id).ToList();
                    Product.Items = items.Count > 0 ? items : NonVariationsItems;
                    Product.VariationCounter = Items.Where(i => i.ProductId == Product.Id).Count();
                    Product.Items = Items.Where(x => x.ProductId == Product.Id).ToList();
                }
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<long> MobileCountProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId)
        {
            long count;

            List<Product> Products;
            if (ProductFilter.ItemSalePrice != null)
            {
                ProductFilter SubProductFilter = ProductFilter.Clone();
                SubProductFilter.Skip = 0;
                SubProductFilter.Take = int.MaxValue;
                SubProductFilter.OrderBy = new ProductOrder();
                SubProductFilter.ItemSalePrice = null;
                SubProductFilter.Selects = ProductSelect.Id | ProductSelect.UsedVariationId | ProductSelect.VariationGrouping;

                Products = await MobileListProduct(SubProductFilter, SalesEmployeeId, StoreId);

                if (ProductFilter.SalePrice.GreaterEqual != null)
                    Products = Products.Where(x => x.Items[0].SalePrice >= ProductFilter.SalePrice.GreaterEqual.Value).ToList();
                if (ProductFilter.SalePrice.LessEqual != null)
                    Products = Products.Where(x => x.Items[0].SalePrice <= ProductFilter.SalePrice.LessEqual.Value).ToList();

                count = Products.Count();
            }
            else
            {
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    count = await UOW.EsProductRepository.Count(ProductFilter);
                else
                    count = await UOW.ProductRepository.Count(ProductFilter);
            }

            return count;
        }
        private async Task CheckStateOrder(List<DirectSalesOrder> DirectSalesOrders)
        {
            var AppUserIds = DirectSalesOrders.Select(x => x.SaleEmployeeId).Distinct().ToList();
            var AppUsers = await UOW.AppUserRepository.List(AppUserIds);
            var StoreIds = DirectSalesOrders.Select(x => x.BuyerStoreId).Distinct().ToList();
            var Stores = await UOW.StoreRepository.List(StoreIds);
            List<long> ItemIds = new List<long>();
            var DirectSalesOrderContents = DirectSalesOrders.Where(x => x.DirectSalesOrderContents != null).SelectMany(x => x.DirectSalesOrderContents).ToList();
            var DirectSalesOrderPromotions = DirectSalesOrders.Where(x => x.DirectSalesOrderPromotions != null).SelectMany(x => x.DirectSalesOrderPromotions).ToList();
            ItemIds.AddRange(DirectSalesOrderContents?.Select(x => x.ItemId));
            ItemIds.AddRange(DirectSalesOrderPromotions?.Select(x => x.ItemId));
            ItemIds = ItemIds.Distinct().ToList();
            var StoreBalances = await UOW.StoreBalanceRepository.List(new StoreBalanceFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreBalanceSelect.CreditAmount | StoreBalanceSelect.DebitAmount | StoreBalanceSelect.Store,
                StoreId = new IdFilter { In = StoreIds }
            });
            var Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = WarehouseSelect.Id,
                OrganizationId = new IdFilter { In = AppUsers.Select(x => x.OrganizationId).ToList() }
            });
            var WarehouseIds = Warehouses.Select(x => x.Id).Distinct().ToList();
            Warehouses = await UOW.WarehouseRepository.List(WarehouseIds);
            var WarehouseOrganizationMappings = Warehouses.SelectMany(x => x.WarehouseOrganizationMappings).ToList();
            var Inventories = await UOW.InventoryRepository.List(new InventoryFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = InventorySelect.ALL,
                WarehouseId = new IdFilter { In = WarehouseIds },
                ItemId = new IdFilter { In = ItemIds }
            });
            var Balances = new List<StoreBalance>();
            var AppUser = new AppUser();
            decimal BalanceAmount, DebtLimit;
            foreach (var DirectSalesOrder in DirectSalesOrders)
            {
                var ItemInScopedIds = new List<long>();
                DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.PASS.Id;
                DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.PASS.Id;
                DirectSalesOrder.DirectSalesOrderContents?.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.PASS.Id);
                DirectSalesOrder.DirectSalesOrderPromotions?.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.PASS.Id);
                //công nợ
                Balances = StoreBalances.Where(x => x.StoreId == DirectSalesOrder.BuyerStoreId).ToList();
                BalanceAmount = Balances.Sum(x => x.DebitAmount ?? 0) - Balances.Sum(x => x.CreditAmount ?? 0);
                DebtLimit = Stores.Where(x => x.Id == DirectSalesOrder.BuyerStoreId).FirstOrDefault().DebtLimited ?? 0;
                if (BalanceAmount + DirectSalesOrder.TotalAfterTax > DebtLimit) DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.NOT_PASS.Id;
                //tồn kho
                AppUser = AppUsers.Where(x => x.Id == DirectSalesOrder.SaleEmployeeId).FirstOrDefault();
                if (DirectSalesOrder.DirectSalesOrderContents != null && DirectSalesOrder.DirectSalesOrderContents.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId));
                if (DirectSalesOrder.DirectSalesOrderPromotions != null && DirectSalesOrder.DirectSalesOrderPromotions.Any())
                    ItemInScopedIds.AddRange(DirectSalesOrder.DirectSalesOrderPromotions.Select(x => x.ItemId));
                ItemInScopedIds = ItemInScopedIds.Distinct().ToList();

                var WarehouseInScopedIds = WarehouseOrganizationMappings.Where(x => x.Organization.Path.StartsWith(AppUser.Organization.Path)).Select(x => x.WarehouseId).ToList();
                var InventoryInScoped = Inventories.Where(x => WarehouseInScopedIds.Contains(x.WarehouseId)).ToList();
                foreach (var ItemId in ItemInScopedIds)
                {
                    var contents = DirectSalesOrder.DirectSalesOrderContents?.Where(x => x.ItemId == ItemId).ToList();
                    var promotions = DirectSalesOrder.DirectSalesOrderPromotions?.Where(x => x.ItemId == ItemId).ToList();
                    var SaleQuantity = (contents?.Sum(x => (x.Quantity * x.Factor)) ?? 0) + (promotions?.Sum(x => (x.Quantity * x.Factor)) ?? 0);
                    var StockQuantity = InventoryInScoped?.Where(x => x.ItemId == ItemId).Sum(x => x.SaleStock) ?? 0;
                    if (SaleQuantity > StockQuantity)
                    {
                        DirectSalesOrder.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id;
                        if (contents != null) contents.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id);
                        if (promotions != null) promotions.ForEach(x => x.InventoryCheckStateId = CheckStateEnum.NOT_PASS.Id);
                    }
                }
            }
        }
        private async Task<List<AppUser>> ListApproverRecipient(DirectSalesOrder DirectSalesOrder)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await UOW.RequestWorkflowStepMappingRepository.List(DirectSalesOrder.RowId);
            RequestWorkflowStepMappings = RequestWorkflowStepMappings.Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id).ToList();
            if (RequestWorkflowStepMappings.Any())
            {
                List<long> WorkflowStepIds = RequestWorkflowStepMappings.Select(x => x.WorkflowStepId).ToList();
                List<WorkflowStep> WorkflowSteps = await UOW.WorkflowStepRepository.List(new WorkflowStepFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = WorkflowStepSelect.Role,
                    Id = new IdFilter { In = WorkflowStepIds }
                });
                var RoleIds = WorkflowSteps.Select(x => x.RoleId).ToList();
                AppUsers = await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = AppUserSelect.Id | AppUserSelect.RowId,
                    RoleId = new IdFilter { In = RoleIds }
                });
            }
            return AppUsers;
        }
    }
}
