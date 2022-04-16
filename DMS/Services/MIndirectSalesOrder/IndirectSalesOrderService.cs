
using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.indirect_sales_order;
using DMS.Services.MNotification;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MWorkflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MIndirectSalesOrder
{
    public interface IIndirectSalesOrderService : IServiceScoped
    {
        Task<int> Count(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<IndirectSalesOrder>> List(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<int> CountNew(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<IndirectSalesOrder>> ListNew(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<int> CountPending(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<IndirectSalesOrder>> ListPending(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<int> CountCompleted(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<IndirectSalesOrder>> ListCompleted(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<int> CountInScoped(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<IndirectSalesOrder>> ListInScoped(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<IndirectSalesOrder> Get(long Id);
        Task<IndirectSalesOrder> GetDetail(long Id);
        Task<long> CountItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<List<Item>> ListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<long> MobileCountItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<List<Item>> MobileListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId);
        Task<Item> GetItem(long Id, long SalesEmployeeId);
        Task<IndirectSalesOrder> Create(IndirectSalesOrder IndirectSalesOrder);
        Task<IndirectSalesOrder> Update(IndirectSalesOrder IndirectSalesOrder);
        Task<IndirectSalesOrder> Delete(IndirectSalesOrder IndirectSalesOrder);
        Task<IndirectSalesOrder> Send(IndirectSalesOrder IndirectSalesOrder);
        Task<IndirectSalesOrder> Approve(IndirectSalesOrder IndirectSalesOrder);
        Task<IndirectSalesOrder> Reject(IndirectSalesOrder IndirectSalesOrder);
        Task<List<IndirectSalesOrder>> BulkApprove(List<IndirectSalesOrder> IndirectSalesOrders);
        Task<List<IndirectSalesOrder>> BulkReject(List<IndirectSalesOrder> IndirectSalesOrders);
        Task<IndirectSalesOrder> PreviewIndirectOrder(IndirectSalesOrder IndirectSalesOrder);
        Task<List<IndirectSalesOrder>> BulkDelete(List<IndirectSalesOrder> IndirectSalesOrders);
        Task<List<IndirectSalesOrder>> Import(List<IndirectSalesOrder> IndirectSalesOrders);
        Task<IndirectSalesOrderFilter> ToFilter(IndirectSalesOrderFilter IndirectSalesOrderFilter);
        Task<List<Store>> ListStore(StoreFilter StoreFilter);
        Task<List<Store>> ListInScoped(StoreFilter StoreFilter, long AppUserId);
        Task<int> CountInScoped(StoreFilter StoreFilter, long AppUserId);
        Task<long> MobileCountProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId);
        Task<List<Product>> MobileListProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId);
    }

    public class IndirectSalesOrderService : BaseService, IIndirectSalesOrderService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IIndirectSalesOrderTemplate IndirectSalesOrderTemplate;
        private INotificationService NotificationService;
        private IIndirectSalesOrderValidator IndirectSalesOrderValidator;
        private IRabbitManager RabbitManager;
        private IOrganizationService OrganizationService;
        private IWorkflowService WorkflowService;
        private IItemService ItemService;

        public IndirectSalesOrderService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IIndirectSalesOrderTemplate IndirectSalesOrderTemplate,
            INotificationService NotificationService,
            IRabbitManager RabbitManager,
            IIndirectSalesOrderValidator IndirectSalesOrderValidator,
            IOrganizationService OrganizationService,
            IItemService ItemService,
            IWorkflowService WorkflowService
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.IndirectSalesOrderTemplate = IndirectSalesOrderTemplate;
            this.NotificationService = NotificationService;
            this.RabbitManager = RabbitManager;
            this.IndirectSalesOrderValidator = IndirectSalesOrderValidator;
            this.OrganizationService = OrganizationService;
            this.ItemService = ItemService;
            this.WorkflowService = WorkflowService;
        }

        public async Task<int> Count(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.IndirectSalesOrderRepository.Count(IndirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<IndirectSalesOrder>> List(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                if (IndirectSalesOrderFilter.StoreStatusId != null && IndirectSalesOrderFilter.StoreStatusId.Equal == 1) IndirectSalesOrderFilter.StoreStatusId = null;
                List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderFilter);
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<int> CountNew(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.IndirectSalesOrderRepository.CountNew(IndirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<IndirectSalesOrder>> ListNew(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.ListNew(IndirectSalesOrderFilter);
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<int> CountPending(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.IndirectSalesOrderRepository.CountPending(IndirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<IndirectSalesOrder>> ListPending(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.ListPending(IndirectSalesOrderFilter);
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<int> CountCompleted(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.IndirectSalesOrderRepository.CountCompleted(IndirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<IndirectSalesOrder>> ListCompleted(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.ListCompleted(IndirectSalesOrderFilter);
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> CountInScoped(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                int result = await UOW.IndirectSalesOrderRepository.CountInScoped(IndirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return 0;
        }
        public async Task<List<IndirectSalesOrder>> ListInScoped(IndirectSalesOrderFilter IndirectSalesOrderFilter)
        {
            try
            {
                IndirectSalesOrderFilter.ApproverId = new IdFilter { Equal = CurrentContext.UserId };
                List<IndirectSalesOrder> IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.ListInScoped(IndirectSalesOrderFilter);
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<long> CountItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            long count = await UOW.ItemRepository.Count(ItemFilter);
            return count;
        }

        public async Task<List<Item>> ListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);
                var Ids = Items.Select(x => x.Id).ToList();
                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);
                if (AppUser != null && Items != null)
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
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }
        public async Task<long> MobileCountItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            long count;
            var SystemConfig = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfig.USE_ELASTICSEARCH)
                count = await UOW.EsItemRepository.Count(ItemFilter);
            else
                count = await UOW.ItemRepository.Count(ItemFilter);

            return count;
        }

        public async Task<List<Item>> MobileListItem(ItemFilter ItemFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                List<Item> Items;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Items = await UOW.EsItemRepository.List(ItemFilter);
                else
                    Items = await UOW.ItemRepository.List(ItemFilter);

                var Ids = Items.Select(x => x.Id).ToList();
                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);
                if (AppUser != null && Items != null)
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
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<Item> GetItem(long Id, long SalesEmployeeId)
        {
            try
            {
                Item Item = await UOW.ItemRepository.Get(Id);
                List<Item> Items = new List<Item> { Item };
                await ApplyPrice(Items, SalesEmployeeId, null);
                return Items.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<IndirectSalesOrder> Get(long Id)
        {
            IndirectSalesOrder IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(Id);
            if (IndirectSalesOrder == null)
                return null;

            IndirectSalesOrder.RequestState = await WorkflowService.GetRequestState(IndirectSalesOrder.RowId);
            if (IndirectSalesOrder.RequestState == null)
            {
                IndirectSalesOrder.RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping
                {
                    AppUserId = IndirectSalesOrder.SaleEmployeeId,
                    CreatedAt = IndirectSalesOrder.CreatedAt,
                    UpdatedAt = IndirectSalesOrder.UpdatedAt,
                    RequestId = IndirectSalesOrder.RowId,
                    AppUser = IndirectSalesOrder.SaleEmployee == null ? null : new AppUser
                    {
                        Id = IndirectSalesOrder.SaleEmployee.Id,
                        Username = IndirectSalesOrder.SaleEmployee.Username,
                        DisplayName = IndirectSalesOrder.SaleEmployee.DisplayName,
                    },
                };
                IndirectSalesOrder.RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.WorkflowStateId = IndirectSalesOrder.RequestStateId;
                IndirectSalesOrder.RequestState = WorkflowService.GetRequestState(IndirectSalesOrder.RequestStateId);
                RequestWorkflowStepMapping.WorkflowState = WorkflowService.GetWorkflowState(RequestWorkflowStepMapping.WorkflowStateId);
            }
            else
            {
                IndirectSalesOrder.RequestStateId = IndirectSalesOrder.RequestState.Id;
                IndirectSalesOrder.RequestWorkflowStepMappings = await WorkflowService.ListRequestWorkflowStepMapping(IndirectSalesOrder.RowId);
            }
            return IndirectSalesOrder;
        }

        public async Task<IndirectSalesOrder> GetDetail(long Id)
        {
            IndirectSalesOrder IndirectSalesOrder = await Get(Id);
            return IndirectSalesOrder;
        }

        public async Task<IndirectSalesOrder> Create(IndirectSalesOrder IndirectSalesOrder)
        {
            if (!await IndirectSalesOrderValidator.Create(IndirectSalesOrder))
                return IndirectSalesOrder;

            try
            {
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.SaleEmployeeId);
                var BuyerStore = await UOW.StoreRepository.Get(IndirectSalesOrder.BuyerStoreId);
                //await Calculator(IndirectSalesOrder);

                IndirectSalesOrder.BuyerStoreTypeId = BuyerStore.StoreTypeId;
                IndirectSalesOrder.RequestStateId = RequestStateEnum.NEW.Id;
                IndirectSalesOrder.Code = IndirectSalesOrder.Id.ToString();
                IndirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                IndirectSalesOrder.CreatorId = CurrentContext.UserId;
                await UOW.IndirectSalesOrderRepository.Create(IndirectSalesOrder);
                IndirectSalesOrder.Code = IndirectSalesOrder.Id.ToString();
                await UOW.IndirectSalesOrderRepository.Update(IndirectSalesOrder);
                await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);

                IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);

                Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
                Logging.CreateAuditLog(IndirectSalesOrder, new { }, nameof(IndirectSalesOrderService));
                return IndirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<IndirectSalesOrder> Update(IndirectSalesOrder IndirectSalesOrder)
        {
            if (!await IndirectSalesOrderValidator.Update(IndirectSalesOrder))
                return IndirectSalesOrder;
            try
            {
                var oldData = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
                if (oldData.SaleEmployeeId != IndirectSalesOrder.SaleEmployeeId)
                {
                    var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.SaleEmployeeId);
                    IndirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                var BuyerStore = await UOW.StoreRepository.Get(IndirectSalesOrder.BuyerStoreId);
                IndirectSalesOrder.BuyerStoreTypeId = BuyerStore.StoreTypeId;
                IndirectSalesOrder.RequestStateId = oldData.RequestStateId;
                //await Calculator(IndirectSalesOrder);

                await UOW.IndirectSalesOrderRepository.Update(IndirectSalesOrder);

                IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
                Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
                Logging.CreateAuditLog(IndirectSalesOrder, oldData, nameof(IndirectSalesOrderService));
                return IndirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<IndirectSalesOrder> Delete(IndirectSalesOrder IndirectSalesOrder)
        {
            if (!await IndirectSalesOrderValidator.Delete(IndirectSalesOrder))
                return IndirectSalesOrder;

            try
            {
                await UOW.IndirectSalesOrderRepository.Delete(IndirectSalesOrder);

                Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
                Logging.CreateAuditLog(new { }, IndirectSalesOrder, nameof(IndirectSalesOrderService));
                return IndirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<IndirectSalesOrder> PreviewIndirectOrder(IndirectSalesOrder IndirectSalesOrder)
        {
            if (!await IndirectSalesOrderValidator.Create(IndirectSalesOrder))
                return IndirectSalesOrder;

            try
            {
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.SaleEmployeeId);
                await Calculator(IndirectSalesOrder);

                IndirectSalesOrder.RequestStateId = RequestStateEnum.NEW.Id;
                IndirectSalesOrder.Code = IndirectSalesOrder.Id.ToString();
                IndirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                IndirectSalesOrder.CreatorId = CurrentContext.UserId;


                Logging.CreateAuditLog(IndirectSalesOrder, new { }, nameof(IndirectSalesOrderService));
                return IndirectSalesOrder;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<List<IndirectSalesOrder>> BulkDelete(List<IndirectSalesOrder> IndirectSalesOrders)
        {
            if (!await IndirectSalesOrderValidator.BulkDelete(IndirectSalesOrders))
                return IndirectSalesOrders;

            try
            {

                await UOW.IndirectSalesOrderRepository.BulkDelete(IndirectSalesOrders);

                Logging.CreateAuditLog(new { }, IndirectSalesOrders, nameof(IndirectSalesOrderService));
                return IndirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<List<IndirectSalesOrder>> Import(List<IndirectSalesOrder> IndirectSalesOrders)
        {
            if (!await IndirectSalesOrderValidator.Import(IndirectSalesOrders))
                return IndirectSalesOrders;
            try
            {
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var BuyerStores = await UOW.StoreRepository.List(new StoreFilter
                {
                    Id = new IdFilter { In = IndirectSalesOrders.Select(x => x.BuyerStoreId).Distinct().ToList() },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreSelect.Id | StoreSelect.StoreType
                });
                Dictionary<long,long> DicBuyerStores = BuyerStores.ToDictionary(x => x.Id, y => y.StoreTypeId);

                foreach (var IndirectSalesOrder in IndirectSalesOrders)
                {
                    IndirectSalesOrder.RequestStateId = RequestStateEnum.NEW.Id;
                    IndirectSalesOrder.Code = IndirectSalesOrder.Id.ToString();
                    IndirectSalesOrder.OrganizationId = CurrentUser.OrganizationId;
                    IndirectSalesOrder.CreatorId = CurrentContext.UserId;
                    IndirectSalesOrder.BuyerStoreTypeId = DicBuyerStores[IndirectSalesOrder.BuyerStoreId];
                }
                await UOW.IndirectSalesOrderRepository.Import(IndirectSalesOrders);

                List<long> IndirectSalesOrderIds = IndirectSalesOrders.Select(x => x.Id).ToList();
                IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderIds);

                foreach (var IndirectSalesOrder in IndirectSalesOrders)
                {
                    IndirectSalesOrder.Code = IndirectSalesOrder.Id.ToString();
                    Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
                    GenericEnum RequestState = await WorkflowService.Send(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, IndirectSalesOrder.OrganizationId, Parameters);
                    IndirectSalesOrder.RequestStateId = RequestState.Id;
                }

                await UOW.IndirectSalesOrderRepository.BulkMerge(IndirectSalesOrders);
                IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderIds);

                Sync(IndirectSalesOrders);
                List<long> ProductIds = IndirectSalesOrders.SelectMany(x => x.IndirectSalesOrderContents).Select(x => x.Item.ProductId).Distinct().ToList();
                SyncProductCal(ProductIds);

                Logging.CreateAuditLog(IndirectSalesOrders, new { }, nameof(IndirectSalesOrderService));
                return null;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }

        public async Task<IndirectSalesOrderFilter> ToFilter(IndirectSalesOrderFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<IndirectSalesOrderFilter>();
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
                IndirectSalesOrderFilter subFilter = new IndirectSalesOrderFilter();
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

                    if (FilterPermissionDefinition.Name == nameof(subFilter.SellerStoreId))
                        subFilter.SellerStoreId = FilterBuilder.Merge(subFilter.SellerStoreId, FilterPermissionDefinition.IdFilter);

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

        private async Task<IndirectSalesOrder> Calculator(IndirectSalesOrder IndirectSalesOrder)
        {
            var ProductIds = new List<long>();
            var ItemIds = new List<long>();
            if (IndirectSalesOrder.IndirectSalesOrderContents != null)
            {
                ProductIds.AddRange(IndirectSalesOrder.IndirectSalesOrderContents.Select(x => x.Item.ProductId).ToList());
                ItemIds.AddRange(IndirectSalesOrder.IndirectSalesOrderContents.Select(x => x.ItemId).ToList());
            }
            if (IndirectSalesOrder.IndirectSalesOrderPromotions != null)
            {
                ProductIds.AddRange(IndirectSalesOrder.IndirectSalesOrderPromotions.Select(x => x.Item.ProductId).ToList());
                ItemIds.AddRange(IndirectSalesOrder.IndirectSalesOrderPromotions.Select(x => x.ItemId).ToList());
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
            var Items = await ListItem(ItemFilter, IndirectSalesOrder.SaleEmployeeId, IndirectSalesOrder.BuyerStoreId);

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
            if (IndirectSalesOrder.IndirectSalesOrderContents != null)
            {
                foreach (var IndirectSalesOrderContent in IndirectSalesOrder.IndirectSalesOrderContents)
                {
                    var Item = Items.Where(x => x.Id == IndirectSalesOrderContent.ItemId).FirstOrDefault();
                    var Product = Products.Where(x => IndirectSalesOrderContent.Item.ProductId == x.Id).FirstOrDefault();
                    IndirectSalesOrderContent.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;

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
                    var UOM = UnitOfMeasures.Where(x => IndirectSalesOrderContent.UnitOfMeasureId == x.Id).FirstOrDefault();
                    IndirectSalesOrderContent.UnitOfMeasure = UOM;
                    //IndirectSalesOrderContent.TaxPercentage = Product.TaxType.Percentage;
                    IndirectSalesOrderContent.RequestedQuantity = IndirectSalesOrderContent.Quantity * UOM.Factor.Value;

                    //Trường hợp không sửa giá, giá bán = giá bán cơ sở của sản phẩm * hệ số quy đổi của đơn vị tính
                    if (IndirectSalesOrder.EditedPriceStatusId == EditedPriceStatusEnum.INACTIVE.Id)
                    {
                        IndirectSalesOrderContent.PrimaryPrice = Item.SalePrice.GetValueOrDefault(0);
                        IndirectSalesOrderContent.SalePrice = IndirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value;
                        IndirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id;
                    }

                    if (IndirectSalesOrder.EditedPriceStatusId == EditedPriceStatusEnum.ACTIVE.Id)
                    {
                        IndirectSalesOrderContent.SalePrice = IndirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value;
                        if (Item.SalePrice == IndirectSalesOrderContent.PrimaryPrice)
                            IndirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id;
                        else
                            IndirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.ACTIVE.Id;
                    }

                    IndirectSalesOrderContent.Amount = IndirectSalesOrderContent.Quantity * IndirectSalesOrderContent.SalePrice;
                }

                //tổng trước chiết khấu
                IndirectSalesOrder.SubTotal = IndirectSalesOrder.IndirectSalesOrderContents.Sum(x => x.Amount);

                //tính tổng chiết khấu theo % chiết khấu chung
                if (IndirectSalesOrder.GeneralDiscountPercentage.HasValue && IndirectSalesOrder.GeneralDiscountPercentage > 0)
                {
                    IndirectSalesOrder.GeneralDiscountAmount = Math.Round(IndirectSalesOrder.SubTotal * IndirectSalesOrder.GeneralDiscountPercentage.Value / 100, 0);
                }
                foreach (var IndirectSalesOrderContent in IndirectSalesOrder.IndirectSalesOrderContents)
                {
                    //phân bổ chiết khấu chung = tổng chiết khấu chung * (tổng từng line/tổng trc chiết khấu)
                    IndirectSalesOrderContent.GeneralDiscountPercentage = IndirectSalesOrderContent.Amount / IndirectSalesOrder.SubTotal * 100;
                    IndirectSalesOrderContent.GeneralDiscountAmount = IndirectSalesOrder.GeneralDiscountAmount * IndirectSalesOrderContent.GeneralDiscountPercentage / 100;
                    IndirectSalesOrderContent.GeneralDiscountAmount = Math.Round(IndirectSalesOrderContent.GeneralDiscountAmount ?? 0, 0);
                }
                //tổng phải thanh toán
                IndirectSalesOrder.Total = IndirectSalesOrder.SubTotal - IndirectSalesOrder.GeneralDiscountAmount.GetValueOrDefault(0);
            }
            else
            {
                IndirectSalesOrder.SubTotal = 0;
                IndirectSalesOrder.GeneralDiscountPercentage = null;
                IndirectSalesOrder.GeneralDiscountAmount = null;
                IndirectSalesOrder.Total = 0;
            }

            //sản phẩm khuyến mãi
            if (IndirectSalesOrder.IndirectSalesOrderPromotions != null)
            {
                foreach (var IndirectSalesOrderPromotion in IndirectSalesOrder.IndirectSalesOrderPromotions)
                {
                    var Product = Products.Where(x => IndirectSalesOrderPromotion.Item.ProductId == x.Id).FirstOrDefault();
                    IndirectSalesOrderPromotion.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;

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
                    var UOM = UnitOfMeasures.Where(x => IndirectSalesOrderPromotion.UnitOfMeasureId == x.Id).FirstOrDefault();
                    IndirectSalesOrderPromotion.RequestedQuantity = IndirectSalesOrderPromotion.Quantity * UOM.Factor.Value;
                }
            }

            return IndirectSalesOrder;
        }

        private async Task<List<Item>> ApplyPrice(List<Item> Items, long? SalesEmployeeId, long? StoreId)
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

            var ItemIds = Items.Select(x => x.Id).Distinct().ToList();
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();

            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();

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

                PriceListItemMappingFilter PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.ALLSTORE.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.INDIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                };
                var PriceListItemMappingAllStore = await UOW.PriceListItemMappingRepository.List(PriceListItemMappingFilter);
                PriceListItemMappings.AddRange(PriceListItemMappingAllStore);

                PriceListItemMappingFilter = new PriceListItemMappingFilter
                {
                    ItemId = new IdFilter { In = ItemIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = PriceListItemMappingSelect.ALL,
                    PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STOREGROUPING.Id },
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.INDIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
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
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.INDIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
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
                    SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.INDIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
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

            //nhân giá với thuế
            foreach (var item in Items)
            {
                item.SalePrice = result[item.Id] * (1 + item.Product.TaxType.Percentage / 100);
            }
            return Items;
        }

        public async Task<IndirectSalesOrder> Send(IndirectSalesOrder IndirectSalesOrder)
        {
            if (IndirectSalesOrder.Id == 0)
                IndirectSalesOrder = await Create(IndirectSalesOrder);
            else
                IndirectSalesOrder = await Update(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated == false)
                return IndirectSalesOrder;
            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
            GenericEnum RequestState = await WorkflowService.Send(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, IndirectSalesOrder.OrganizationId, Parameters);
            IndirectSalesOrder.RequestStateId = RequestState.Id;
            await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);
            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            GlobalUserNotification CurrentUserNotification = 
                IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, IndirectSalesOrder, CurrentUser, NotificationType.TOSENDER);
            GlobalUserNotifications.Add(CurrentUserNotification);
            if (CurrentContext.UserId != IndirectSalesOrder.SaleEmployeeId)
            {
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.SaleEmployeeId);
                GlobalUserNotification SaleEmployeeNotification = 
                    IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, IndirectSalesOrder, CurrentUser, NotificationType.TOSALEEMPLOYEE);
                GlobalUserNotifications.Add(SaleEmployeeNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
            Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
            List<long> ProductIds = IndirectSalesOrder.IndirectSalesOrderContents.Select(x => x.Item.ProductId).Distinct().ToList();
            SyncProductCal(ProductIds);

            return IndirectSalesOrder;
        }

        public async Task<IndirectSalesOrder> Approve(IndirectSalesOrder IndirectSalesOrder)
        {
            IndirectSalesOrder = await Update(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated == false)
                return IndirectSalesOrder;
            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
            await WorkflowService.Approve(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(IndirectSalesOrder.RowId);
            IndirectSalesOrder.RequestStateId = RequestState.Id;
            await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);

            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            GlobalUserNotification CurrentUserNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, IndirectSalesOrder, CurrentUser, NotificationType.TOAPPROVER);
            GlobalUserNotifications.Add(CurrentUserNotification);
            var Creator = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.CreatorId);
            GlobalUserNotification CreatorNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, Creator.RowId, IndirectSalesOrder, CurrentUser, NotificationType.APPROVE);
            GlobalUserNotifications.Add(CurrentUserNotification);
            if (IndirectSalesOrder.CreatorId != IndirectSalesOrder.SaleEmployeeId)
            {
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.CreatorId);
                GlobalUserNotification SaleEmployeeNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, IndirectSalesOrder, CurrentUser, NotificationType.APPROVE);
                GlobalUserNotifications.Add(CurrentUserNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
            Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
            List<long> ProductIds = IndirectSalesOrder.IndirectSalesOrderContents.Select(x => x.Item.ProductId).Distinct().ToList();
            SyncProductCal(ProductIds);

            return IndirectSalesOrder;
        }

        public async Task<IndirectSalesOrder> Reject(IndirectSalesOrder IndirectSalesOrder)
        {
            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
            GenericEnum Action = await WorkflowService.Reject(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, Parameters);
            RequestState RequestState = await WorkflowService.GetRequestState(IndirectSalesOrder.RowId);
            IndirectSalesOrder.RequestStateId = RequestState.Id;
            await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);

            IndirectSalesOrder = await UOW.IndirectSalesOrderRepository.Get(IndirectSalesOrder.Id);
            var GlobalUserNotifications = new List<GlobalUserNotification>();
            var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            GlobalUserNotification CurrentUserNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, IndirectSalesOrder, CurrentUser, NotificationType.TOREJECTER);
            GlobalUserNotifications.Add(CurrentUserNotification);
            var Creator = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.CreatorId);
            GlobalUserNotification CreatorNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, Creator.RowId, IndirectSalesOrder, CurrentUser, NotificationType.REJECT);
            GlobalUserNotifications.Add(CurrentUserNotification);
            if (IndirectSalesOrder.CreatorId != IndirectSalesOrder.SaleEmployeeId)
            {
                var SaleEmployee = await UOW.AppUserRepository.GetSimple(IndirectSalesOrder.CreatorId);
                GlobalUserNotification SaleEmployeeNotification = IndirectSalesOrderTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, IndirectSalesOrder, CurrentUser, NotificationType.REJECT);
                GlobalUserNotifications.Add(SaleEmployeeNotification);
            }
            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
            Sync(new List<IndirectSalesOrder> { IndirectSalesOrder });
            return IndirectSalesOrder;
        }

        public async Task<List<IndirectSalesOrder>> BulkApprove(List<IndirectSalesOrder> IndirectSalesOrders)
        {
            foreach (var IndirectSalesOrder in IndirectSalesOrders)
            {
                Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
                await WorkflowService.Approve(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, Parameters);
                RequestState RequestState = await WorkflowService.GetRequestState(IndirectSalesOrder.RowId);
                IndirectSalesOrder.RequestStateId = RequestState.Id;
                await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);
            }
            var IndirectSalesOrderIds = IndirectSalesOrders.Select(x => x.Id).ToList();
            IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderIds);
            Sync(IndirectSalesOrders);
            return IndirectSalesOrders;
        }

        public async Task<List<IndirectSalesOrder>> BulkReject(List<IndirectSalesOrder> IndirectSalesOrders)
        {
            foreach (var IndirectSalesOrder in IndirectSalesOrders)
            {
                Dictionary<string, string> Parameters = await MapParameters(IndirectSalesOrder);
                GenericEnum Action = await WorkflowService.Reject(IndirectSalesOrder.RowId, WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id, Parameters);
                RequestState RequestState = await WorkflowService.GetRequestState(IndirectSalesOrder.RowId);
                IndirectSalesOrder.RequestStateId = RequestState.Id;
                await UOW.IndirectSalesOrderRepository.UpdateState(IndirectSalesOrder);
            }
            var IndirectSalesOrderIds = IndirectSalesOrders.Select(x => x.Id).ToList();
            IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(IndirectSalesOrderIds);
            Sync(IndirectSalesOrders);
            return IndirectSalesOrders;
        }

        private async Task<Dictionary<string, string>> MapParameters(IndirectSalesOrder IndirectSalesOrder)
        {
            var AppUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            Parameters.Add(nameof(IndirectSalesOrder.Id), IndirectSalesOrder.Id.ToString());
            Parameters.Add(nameof(IndirectSalesOrder.Code), IndirectSalesOrder.Code);
            Parameters.Add(nameof(IndirectSalesOrder.SaleEmployeeId), IndirectSalesOrder.SaleEmployeeId.ToString());
            Parameters.Add(nameof(IndirectSalesOrder.BuyerStoreId), IndirectSalesOrder.BuyerStoreId.ToString());
            Parameters.Add(nameof(AppUser.DisplayName), AppUser.DisplayName);
            Parameters.Add(nameof(IndirectSalesOrder.RequestStateId), IndirectSalesOrder.RequestStateId.ToString());

            Parameters.Add(nameof(IndirectSalesOrder.Total), IndirectSalesOrder.Total.ToString());
            Parameters.Add(nameof(IndirectSalesOrder.TotalDiscountAmount), IndirectSalesOrder.TotalDiscountAmount.ToString());
            Parameters.Add(nameof(IndirectSalesOrder.TotalRequestedQuantity), IndirectSalesOrder.TotalRequestedQuantity.ToString());
            Parameters.Add(nameof(IndirectSalesOrder.OrganizationId), IndirectSalesOrder.OrganizationId.ToString());

            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(IndirectSalesOrder.RowId);
            if (RequestWorkflowDefinitionMapping == null)
                Parameters.Add(nameof(RequestState), RequestStateEnum.NEW.Id.ToString());
            else
                Parameters.Add(nameof(RequestState), RequestWorkflowDefinitionMapping.RequestStateId.ToString());
            Parameters.Add("Username", CurrentContext.UserName);
            Parameters.Add("Type", nameof(IndirectSalesOrder));
            return Parameters;
        }

        private async Task<List<long>> ListReceipientId(AppUser AppUser, string Path)
        {
            var Ids = await UOW.PermissionRepository.ListAppUser(Path);
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
                Selects = AppUserSelect.Id | AppUserSelect.Organization,
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });
            var AppUserIds = AppUsers.Where(x => OrganizationIds.Contains(x.OrganizationId)).Select(x => x.Id).ToList();
            AppUserIds = AppUserIds.Intersect(Ids).ToList();
            return AppUserIds;
        }

        private void Sync(List<IndirectSalesOrder> IndirectSalesOrders)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            AppUsers.AddRange(IndirectSalesOrders.Select(x => new AppUser { Id = x.SaleEmployeeId }));
            AppUsers.AddRange(IndirectSalesOrders.Select(x => new AppUser { Id = x.CreatorId }));
            AppUsers = AppUsers.Distinct().ToList();

            List<Organization> Organizations = IndirectSalesOrders.Select(x => new Organization { Id = x.OrganizationId }).Distinct().ToList();

            List<Store> Stores = new List<Store>();
            Stores.AddRange(IndirectSalesOrders.Select(x => new Store { Id = x.BuyerStoreId }));
            Stores.AddRange(IndirectSalesOrders.Select(x => new Store { Id = x.SellerStoreId }));
            Stores = Stores.Distinct().ToList();

            List<Item> Items = new List<Item>();
            Items.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderContents != null)
                .SelectMany(x => x.IndirectSalesOrderContents).Select(x => new Item { Id = x.ItemId }));
            Items.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderPromotions != null)
                .SelectMany(x => x.IndirectSalesOrderPromotions).Select(x => new Item { Id = x.ItemId }));
            Items = Items.Distinct().ToList();
            List<UnitOfMeasure> UnitOfMeasures = new List<UnitOfMeasure>();
            UnitOfMeasures.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderContents != null)
                .SelectMany(x => x.IndirectSalesOrderContents).Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasures.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderContents != null)
                .SelectMany(x => x.IndirectSalesOrderContents).Select(x => new UnitOfMeasure { Id = x.PrimaryUnitOfMeasureId }));
            UnitOfMeasures.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderPromotions != null)
                .SelectMany(x => x.IndirectSalesOrderPromotions).Select(x => new UnitOfMeasure { Id = x.UnitOfMeasureId }));
            UnitOfMeasures.AddRange(IndirectSalesOrders.Where(x => x.IndirectSalesOrderPromotions != null)
                .SelectMany(x => x.IndirectSalesOrderPromotions).Select(x => new UnitOfMeasure { Id = x.PrimaryUnitOfMeasureId }));
            UnitOfMeasures = UnitOfMeasures.Distinct().ToList();

            RabbitManager.PublishList(IndirectSalesOrders, RoutingKeyEnum.IndirectSalesOrderSync.Code);
            RabbitManager.PublishList(AppUsers, RoutingKeyEnum.AppUserUsed.Code);
            RabbitManager.PublishList(Organizations, RoutingKeyEnum.OrganizationUsed.Code);
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
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
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ALLOW_DRAFT_STORE_TO_CREATE_ORDER == false)
            {
                StoreFilter.StoreStatusId = new IdFilter { NotEqual = StoreStatusEnum.DRAFT.Id };
            }
            StoreFilter.AppUserId = new IdFilter { Equal = AppUserId };
            List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
            return Stores;
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

                if (ProductFilter.ItemSalePrice.GreaterEqual != null)
                    Products = Products.Where(x => x.Items[0].SalePrice >= ProductFilter.ItemSalePrice.GreaterEqual.Value).ToList();
                if (ProductFilter.ItemSalePrice.LessEqual != null)
                    Products = Products.Where(x => x.Items[0].SalePrice <= ProductFilter.ItemSalePrice.LessEqual.Value).ToList();

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

        public async Task<List<Product>> MobileListProduct(ProductFilter ProductFilter, long? SalesEmployeeId, long? StoreId)
        {
            try
            {
                List<Product> Products;
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

                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);
                var Ids = Items.Select(x => x.Id).ToList();
                AppUser AppUser = await UOW.AppUserRepository.GetSimple(SalesEmployeeId.Value);
                if (AppUser != null && Items != null)
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
                    //if (ItemFilter.SalePrice != null && ItemFilter.SalePrice.HasValue)
                    //{
                    //    if (ItemFilter.SalePrice.GreaterEqual.HasValue)
                    //        Items = Items.Where(x => x.SalePrice >= ItemFilter.SalePrice.GreaterEqual.Value).ToList();
                    //    if (ItemFilter.SalePrice.LessEqual.HasValue)
                    //        Items = Items.Where(x => x.SalePrice <= ItemFilter.SalePrice.LessEqual.Value).ToList();
                    //}
                }
                var VariationGroupings = Products.Where(x => x.UsedVariationId == UsedVariationEnum.USED.Id).SelectMany(x => x.VariationGroupings).ToList();
                var VariationGroupingIds = VariationGroupings?.Select(x => x.Id).ToList();
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
                Logging.CreateSystemLog(ex, nameof(IndirectSalesOrderService));
            }
            return null;
        }
        private async Task<List<AppUser>> ListApproverRecipient(IndirectSalesOrder IndirectSalesOrder)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await UOW.RequestWorkflowStepMappingRepository.List(IndirectSalesOrder.RowId);
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
