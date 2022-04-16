using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Rpc.web.direct_sales_order;
using DMS.ABE.Services.MWorkflow;
using DMS.FAST.Enums;

namespace DMS.ABE.Services.MDirectSalesOrder
{
    public interface IDirectSalesOrderService : IServiceScoped
    {
        Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> Count(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<DirectSalesOrder> Get(long Id);
        Task<DirectSalesOrder> Create(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Update(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Delete(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Approve(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Reject(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> Send(DirectSalesOrder DirectSalesOrder);
        Task<List<DirectSalesOrder>> WebList(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> WebCount(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<DirectSalesOrder> WebCreate(DirectSalesOrder DirectSalesOrder);
        Task<DirectSalesOrder> WebUpdate(DirectSalesOrder DirectSalesOrder);
    }
    public class DirectSalesOrderService : BaseService, IDirectSalesOrderService
    {
        private IUOW UOW;
        private ILogging Logging;
        private IRabbitManager RabbitManager;
        private IDirectSalesOrderValidator DirectSalesOrderValidator;
        private ICurrentContext CurrentContext;
        private IWorkflowService WorkflowService;
        private IDirectSalesOrderTemplate DirectSalesOrderTemplate;

        public DirectSalesOrderService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            IWorkflowService WorkflowService,
            IDirectSalesOrderValidator DirectSalesOrderValidator,
            IDirectSalesOrderTemplate DirectSalesOrderTemplate
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.WorkflowService = WorkflowService;
            this.DirectSalesOrderValidator = DirectSalesOrderValidator;
            this.DirectSalesOrderTemplate = DirectSalesOrderTemplate;
        }

        public async Task<DirectSalesOrder> Create(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Create(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                Store Store = await GetStore();
                if (Store == null)
                    return null;

                DirectSalesOrder.StoreUserCreatorId = CurrentContext.StoreUserId;
                DirectSalesOrder.BuyerStoreId = Store.Id; // cửa hàng mua là cửa hàng của storeUser hiện tại
                DirectSalesOrder.BuyerStoreTypeId = Store.StoreTypeId;
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString();
                DirectSalesOrder.DirectSalesOrderSourceTypeId = DirectSalesOrderSourceTypeEnum.FROM_STORE.Id; // set source Type
                DirectSalesOrder.OrderDate = StaticParams.DateTimeNow; 
                DirectSalesOrder.RequestStateId = RequestStateEnum.APPROVED.Id; // don hang tao tren mobile ko co wf => requestStateId = Approved
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.DRAFT.Id; 
                DirectSalesOrder.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id; 
                DirectSalesOrder.GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_DRAFT.Id;
                AppUser SaleEmployee = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                if (SaleEmployee != null)
                {
                    DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                await CalculateOrder(DirectSalesOrder, Store.Id); // tính toán nội dung đơn hàng
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Create(DirectSalesOrder); // tạo mới đơn
                await UOW.Commit();
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString(); // gán lại Code đơn hàng
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderService)); // ghi log
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();
                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Update(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Update(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                Store Store = await GetStore();
                if (Store == null)
                    return null;
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                if (oldData.SaleEmployeeId != DirectSalesOrder.SaleEmployeeId)
                {
                    var SaleEmployee = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                    DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.DRAFT.Id;
                await CalculateOrder(DirectSalesOrder, Store.Id); // tính toán nội dung đơn hàng
                DirectSalesOrder.BuyerStoreTypeId = Store.StoreTypeId;
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder); // tạo mới đơn
                await UOW.Commit();
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderService)); // ghi log
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();
                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Delete(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Delete(DirectSalesOrder))
                return DirectSalesOrder;

            try
            {
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Delete(DirectSalesOrder);
                await UOW.Commit();
                Sync(new List<DirectSalesOrder> { DirectSalesOrder });
                Logging.CreateAuditLog(new { }, DirectSalesOrder, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Approve(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Approve(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                await UOW.Commit();

                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                var AppUser = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                var StoreUserId = CurrentContext.StoreUserId;
                StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
                Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                var hasWorkflow = await HasWorkflow(DirectSalesOrder);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();

                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.APPROVED.Id;
                if (hasWorkflow && SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == GlobalUserTypeEnum.STOREUSER.Id) //start bới store
                {
                    Dictionary<string, string> Parameters = await MapParameters(DirectSalesOrder);
                    GenericEnum RequestState = await WorkflowService.Send(oldData.RowId, WorkflowTypeEnum.DIRECT_SALES_ORDER.Id, DirectSalesOrder.OrganizationId, Parameters, AppUser);
                    DirectSalesOrder.RequestStateId = RequestState.Id;

                    //bắn thông báo
                    if (StoreUser != null)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(StoreUser.RowId, StoreUser.RowId, DirectSalesOrder, Store, NotificationType.TOSENDER);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    var RequestWorkflowMappings = await UOW.RequestWorkflowStepMappingRepository.List(DirectSalesOrder.RowId);
                    List<Guid> RecipientRowIds = new List<Guid>();
                    RecipientRowIds.AddRange(RequestWorkflowMappings.Where(x => x.AppUser != null && x.AppUser.RowId != default(Guid)).Select(x => x.AppUser.RowId));
                    RecipientRowIds.Add(DirectSalesOrder.SaleEmployee.RowId);
                    RecipientRowIds = RecipientRowIds.Distinct().ToList();
                    foreach (var RecipientRowId in RecipientRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(StoreUser.RowId, RecipientRowId, DirectSalesOrder, Store, NotificationType.SEND);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                else //start bới appuser
                {
                    if (StoreUser != null)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(StoreUser.RowId, StoreUser.RowId, DirectSalesOrder, Store, NotificationType.TOAPPROVER);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    var RequestWorkflowMappings = await UOW.RequestWorkflowStepMappingRepository.List(DirectSalesOrder.RowId);
                    List<Guid> RecipientRowIds = new List<Guid>();
                    RecipientRowIds.AddRange(RequestWorkflowMappings.Where(x => x.AppUser != null && x.AppUser.RowId != default(Guid)).Select(x => x.AppUser.RowId));
                    RecipientRowIds.Add(DirectSalesOrder.SaleEmployee.RowId);
                    RecipientRowIds = RecipientRowIds.Distinct().ToList();
                    foreach (var RecipientRowId in RecipientRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(StoreUser.RowId, RecipientRowId, DirectSalesOrder, Store, NotificationType.APPROVE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }

                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);


                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, oldData, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();

                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Reject(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Reject(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.REJECTED.Id; // update StoreApprovalStateId
                //DirectSalesOrder.RequestStateId = RequestStateEnum.REJECTED.Id;
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                await UOW.Commit();
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                var StoreUserId = CurrentContext.StoreUserId;
                StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
                Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                if (StoreUser != null)
                {
                    GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateStoreUserNotification(StoreUser.RowId, StoreUser.RowId, DirectSalesOrder, Store, NotificationType.TOREJECTER);
                    RabbitManager.PublishList(new List<GlobalUserNotification> { GlobalUserNotification }, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                }
                var RequestWorkflowMappings = await UOW.RequestWorkflowStepMappingRepository.List(DirectSalesOrder.RowId);
                List<Guid> RecipientRowIds = new List<Guid>();
                RecipientRowIds.AddRange(RequestWorkflowMappings.Where(x => x.AppUser != null).Select(x => x.AppUser.RowId));
                RecipientRowIds.Add(DirectSalesOrder.SaleEmployee.RowId);
                RecipientRowIds = RecipientRowIds.Distinct().ToList();
                var GlobalUserNotifications = new List<GlobalUserNotification>();
                foreach (var RecipientRowId in RecipientRowIds)
                {
                    GlobalUserNotification GlobalUserNotification = DirectSalesOrderTemplate.CreateAppUserNotification(StoreUser.RowId, RecipientRowId, DirectSalesOrder, Store, NotificationType.REJECT);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, oldData, nameof(DirectSalesOrderService));
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();

                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Send(DirectSalesOrder DirectSalesOrder)
        {
            try
            {
                if (DirectSalesOrder.Id == 0)
                    DirectSalesOrder = await Create(DirectSalesOrder);
                else
                    DirectSalesOrder = await Update(DirectSalesOrder);
                if (DirectSalesOrder.IsValidated == false)
                    return DirectSalesOrder;

                DirectSalesOrder = await Approve(DirectSalesOrder);
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();

                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> Get(long Id)
        {
            DirectSalesOrder DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(Id);
            if (DirectSalesOrder == null)
                return null;
            return DirectSalesOrder;
        }
        public async Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId }; // filter đơn hàng có của hàng mua là cửa hàng của storeUser
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        
        public async Task<int> Count(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId }; // filter đơn hàng có của hàng mua là cửa hàng của storeUser
                DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };
                int result = await UOW.DirectSalesOrderRepository.Count(DirectSalesOrderFilter);
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
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId }; // filter đơn hàng có của hàng mua là cửa hàng của storeUser
                
                DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                {
                    In = new List<long> {
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                };

                if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 2)
                {// nếu cấu hình đại lý phê duyệt trước thì lấy các đơn hàng có GeneralApproval = (trừ NEW)
                    DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                    {
                        In = new List<long> {
                    GeneralApprovalStateEnum.REJECTED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                    };
                }

                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
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
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId }; // filter đơn hàng có của hàng mua là cửa hàng của storeUser

                DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                {
                    In = new List<long> {
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                };

                if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 2)
                {// nếu cấu hình đại lý phê duyệt trước thì lấy các đơn hàng có GeneralApproval = (trừ NEW)
                    DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                    {
                        In = new List<long> {
                    GeneralApprovalStateEnum.REJECTED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                    };
                }
                int result = await UOW.DirectSalesOrderRepository.Count(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        private async Task<Store> GetStore()
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            if (StoreUser == null)
            {
                return null;
            } // check storeUser co ton tai khong
            Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
            if (Store == null)
            {
                return null;
            } // check store tuong ung vs storeUser co ton tai khong
            return Store;
        }
        private async Task<List<Item>> ApplyPrice(List<Item> Items, long StoreId)
        {
            var Store = await UOW.StoreRepository.Get(StoreId);
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
                .Where(x => x.Path.StartsWith(Store.Organization.Path) || Store.Organization.Path.StartsWith(x.Path))
                .Select(x => x.Id)
                .ToList();

            var ItemIds = Items.Select(x => x.Id).Distinct().ToList();
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

            var PriceListItemMappingAllStore = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();
            PriceListItemMappings.AddRange(PriceListItemMappingAllStore);

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
            var PriceListItemMappingStoreGrouping = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

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
            var PriceListItemMappingStoreType = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

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
            var PriceListItemMappingStoreDetail = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreGrouping);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreType);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreDetail);

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
                //item.SalePrice = result[item.Id] * (1 + item.Product.TaxType.Percentage / 100);
                item.SalePrice = result[item.Id];
            }
            // gia hien thi tren man hinh khong tinh thue
            return Items;
        } // áp giá cho list item
        private async Task<DirectSalesOrder> CalculateOrder(DirectSalesOrder DirectSalesOrder, long StoreId)
        {
            var ProductIds = new List<long>();
            var ItemIds = new List<long>();
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                ItemIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).ToList());

                ItemFilter SubItemFilter = new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = ItemIds },
                    Selects = ItemSelect.Id | ItemSelect.SalePrice | ItemSelect.ProductId
                };
                List<Item> ListItems;
                ListItems = await UOW.ItemRepository.List(SubItemFilter);


                ProductIds.AddRange(ListItems.Select(x => x.ProductId).ToList());
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
            List<Item> Items;
            Items = await UOW.ItemRepository.List(ItemFilter);

            Items = await ApplyPrice(Items, StoreId); // áp giá cho Item trong đơn hàng

            ProductFilter ProductFilter = new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id | ProductSelect.TaxType
            };

            List<Product> Products;
            Products = await UOW.ProductRepository.List(ProductFilter);
            var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
            });

            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                    var Product = Products.Where(x => Item.ProductId == x.Id).FirstOrDefault();
                    DirectSalesOrderContent.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;
                    DirectSalesOrderContent.UnitOfMeasureId = Product.UnitOfMeasureId; // đơn hàng tạo từ Mobile mặc định UOM của product
                    UnitOfMeasure UOM = new UnitOfMeasure
                    {
                        Id = Product.UnitOfMeasure.Id,
                        Code = Product.UnitOfMeasure.Code,
                        Name = Product.UnitOfMeasure.Name,
                        Description = Product.UnitOfMeasure.Description,
                        StatusId = Product.UnitOfMeasure.StatusId,
                        Factor = 1
                    };
                    DirectSalesOrderContent.RequestedQuantity = DirectSalesOrderContent.Quantity * UOM.Factor.Value;
                    DirectSalesOrderContent.PrimaryPrice = Item.SalePrice.GetValueOrDefault(0); // mặc định = giá của item vì không cho sửa giá trên mobile
                    DirectSalesOrderContent.SalePrice = DirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value; // giá của một đơn vị item
                    DirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id; // mặc định ko cho sửa giá

                    decimal SubAmount = DirectSalesOrderContent.Quantity * DirectSalesOrderContent.SalePrice; // giá của một dòng theo đơn vị tính
                    DirectSalesOrderContent.Amount = SubAmount; // tạm tính
                    if (DirectSalesOrderContent.DiscountAmount > 0 && 
                        (DirectSalesOrderContent.DiscountPercentage == null || DirectSalesOrderContent.DiscountPercentage == 0))
                    {
                        DirectSalesOrderContent.Amount = SubAmount;
                        if (DirectSalesOrderContent.DiscountAmount.HasValue && DirectSalesOrderContent.DiscountAmount.Value > 0)
                        {
                            DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                        }

                    } // trường hợp lấy chiết khấu từ FAST về chỉ có discount amount
                    else if (DirectSalesOrderContent.DiscountPercentage.HasValue)
                    {
                        DirectSalesOrderContent.DiscountAmount = SubAmount * DirectSalesOrderContent.DiscountPercentage.Value / 100;
                        DirectSalesOrderContent.DiscountAmount = Math.Round(DirectSalesOrderContent.DiscountAmount ?? 0, 0);
                        DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                    }
                } // gán primaryUOMId, RequestedQuantity
                DirectSalesOrder.SubTotal = DirectSalesOrder.DirectSalesOrderContents.Sum(x => x.Amount); //tổng trước chiết khấu
                if (DirectSalesOrder.GeneralDiscountPercentage.HasValue && DirectSalesOrder.GeneralDiscountPercentage > 0)
                {
                    DirectSalesOrder.GeneralDiscountAmount = DirectSalesOrder.SubTotal * (DirectSalesOrder.GeneralDiscountPercentage / 100);
                    DirectSalesOrder.GeneralDiscountAmount = Math.Round(DirectSalesOrder.GeneralDiscountAmount.Value, 0);
                }  //tính tổng chiết khấu theo % chiết khấu chung

                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    var Product = Products.Where(x => DirectSalesOrderContent.Item.ProductId == x.Id).FirstOrDefault();
                    //phân bổ chiết khấu chung = tổng chiết khấu chung * (tổng từng line/tổng trc chiết khấu)
                    DirectSalesOrderContent.GeneralDiscountPercentage = DirectSalesOrderContent.Amount / DirectSalesOrder.SubTotal * 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount * DirectSalesOrderContent.GeneralDiscountPercentage / 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = Math.Round(DirectSalesOrderContent.GeneralDiscountAmount ?? 0, 0);
                    //thuê từng line = (tổng từng line - chiết khấu phân bổ) * % thuế
                    DirectSalesOrderContent.TaxAmount = (DirectSalesOrderContent.Amount - (DirectSalesOrderContent.GeneralDiscountAmount.HasValue ? DirectSalesOrderContent.GeneralDiscountAmount.Value : 0)) * Product.TaxType.Percentage / 100;
                    DirectSalesOrderContent.TaxAmount = Math.Round(DirectSalesOrderContent.TaxAmount ?? 0, 0);
                } // chiết khấu phân bổ theo dòng
                DirectSalesOrder.TotalTaxAmount = DirectSalesOrder.DirectSalesOrderContents.Where(x => x.TaxAmount.HasValue).Sum(x => x.TaxAmount.Value); // tính tổng thuê theo dòng
                DirectSalesOrder.TotalTaxAmount = Math.Round(DirectSalesOrder.TotalTaxAmount, 0);
                DirectSalesOrder.TotalAfterTax = DirectSalesOrder.SubTotal - (DirectSalesOrder.GeneralDiscountAmount.HasValue ? DirectSalesOrder.GeneralDiscountAmount.Value : 0) + DirectSalesOrder.TotalTaxAmount;  //tổng phải thanh toán
                DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax; // không có promotion nên tổng sau thuế là tổng cuối
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
            return DirectSalesOrder;
        } // tinh subtotal, totalTaxAmount,  totalAfterTax, Total, phần trăm chiết khẩu chung, tổng chiết khấu
        private void Sync(List<DirectSalesOrder> DirectSalesOrders)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            AppUsers.AddRange(DirectSalesOrders.Where(x => x.SaleEmployeeId != null).Select(x => new AppUser { Id = x.SaleEmployeeId }));
            AppUsers.AddRange(DirectSalesOrders.Where(x => x.CreatorId != null).Select(x => new AppUser { Id = x.CreatorId.Value }));
            AppUsers = AppUsers.Distinct().ToList();

            List<Organization> Organizations = DirectSalesOrders.Select(x => new Organization { Id = x.OrganizationId }).Distinct().ToList();

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
            RabbitManager.PublishList(Items, RoutingKeyEnum.ItemUsed.Code);
            RabbitManager.PublishList(UnitOfMeasures, RoutingKeyEnum.UnitOfMeasureUsed.Code);
        }
        #region Web
        public async Task<List<DirectSalesOrder>> WebList(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId };
                DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                {
                    In = new List<long> {
                    GeneralApprovalStateEnum.STORE_APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                };
                
                if(SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 2)
                {// nếu cấu hình đại lý phê duyệt trước thì lấy các đơn hàng có GeneralApproval = (trừ NEW)
                    DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                    {
                        In = new List<long> {
                    GeneralApprovalStateEnum.PENDING.Id,
                    GeneralApprovalStateEnum.REJECTED.Id,
                    GeneralApprovalStateEnum.APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                    };
                }
                List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
                return DirectSalesOrders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<int> WebCount(DirectSalesOrderFilter DirectSalesOrderFilter)
        {
            try
            {
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                long StoreUserId = CurrentContext.StoreUserId;
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Store
                });
                StoreUser StoreUser = StoreUsers.FirstOrDefault();
                DirectSalesOrderFilter.BuyerStoreId = new IdFilter { Equal = StoreUser.StoreId };
                DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                {
                    In = new List<long> {
                    GeneralApprovalStateEnum.STORE_APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                };

                if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == 2)
                {// nếu cấu hình đại lý phê duyệt trước thì lấy các đơn hàng có GeneralApproval = (trừ NEW)
                    DirectSalesOrderFilter.GeneralApprovalStateId = new IdFilter
                    {
                        In = new List<long> {
                    GeneralApprovalStateEnum.PENDING.Id,
                    GeneralApprovalStateEnum.REJECTED.Id,
                    GeneralApprovalStateEnum.APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_APPROVED.Id,
                    GeneralApprovalStateEnum.STORE_DRAFT.Id,
                    GeneralApprovalStateEnum.STORE_PENDING.Id,
                    GeneralApprovalStateEnum.STORE_REJECTED.Id, }
                    };
                }
                int result = await UOW.DirectSalesOrderRepository.Count(DirectSalesOrderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DirectSalesOrderService));
            }
            return 0;
        }
        public async Task<DirectSalesOrder> WebCreate(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Create(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                Store Store = await GetStore();
                if (Store == null)
                    return null;
                DirectSalesOrder.StoreUserCreatorId = CurrentContext.StoreUserId;
                DirectSalesOrder.BuyerStoreId = Store.Id; // cửa hàng mua là cửa hàng của storeUser hiện tại
                DirectSalesOrder.BuyerStoreTypeId = Store.StoreTypeId; // cửa hàng mua là cửa hàng của storeUser hiện tại
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString();
                DirectSalesOrder.DirectSalesOrderSourceTypeId = DirectSalesOrderSourceTypeEnum.FROM_STORE.Id; // set source Type
                DirectSalesOrder.OrderDate = StaticParams.DateTimeNow;
                DirectSalesOrder.RequestStateId = RequestStateEnum.APPROVED.Id; // don hang tao tren mobile ko co wf => requestStateId = Approved
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.DRAFT.Id;
                DirectSalesOrder.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id;
                DirectSalesOrder.GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_DRAFT.Id;
                AppUser SaleEmployee = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                if (SaleEmployee != null)
                {
                    DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                await WebCalculateOrder(DirectSalesOrder, Store.Id); // tính toán nội dung đơn hàng
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Create(DirectSalesOrder); // tạo mới đơn
                await UOW.Commit();
                DirectSalesOrder.Code = DirectSalesOrder.Id.ToString(); // gán lại Code đơn hàng
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder);
                SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
                await UOW.DirectSalesOrderRepository.UpdateState(DirectSalesOrder, SystemConfiguration);
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderService)); // ghi log
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();
                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        public async Task<DirectSalesOrder> WebUpdate(DirectSalesOrder DirectSalesOrder)
        {
            if (!await DirectSalesOrderValidator.Update(DirectSalesOrder))
                return DirectSalesOrder;
            try
            {
                Store Store = await GetStore();
                if (Store == null)
                    return null;
                var oldData = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                if (oldData.SaleEmployeeId != DirectSalesOrder.SaleEmployeeId)
                {
                    var SaleEmployee = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
                    DirectSalesOrder.OrganizationId = SaleEmployee.OrganizationId;
                }
                DirectSalesOrder.StoreApprovalStateId = StoreApprovalStateEnum.DRAFT.Id;
                await WebCalculateOrder(DirectSalesOrder, Store.Id); // tính toán nội dung đơn hàng
                var DirectSalesOrders = new List<DirectSalesOrder> { DirectSalesOrder };
                await CheckStateOrder(DirectSalesOrders);
                await UOW.Begin();
                await UOW.DirectSalesOrderRepository.Update(DirectSalesOrder); // tạo mới đơn
                await UOW.Commit();
                DirectSalesOrder = await UOW.DirectSalesOrderRepository.Get(DirectSalesOrder.Id);
                Sync(new List<DirectSalesOrder> { DirectSalesOrder }); // sync đơn hàng
                Logging.CreateAuditLog(DirectSalesOrder, new { }, nameof(DirectSalesOrderService)); // ghi log
                return DirectSalesOrder;
            }
            catch (Exception Exception)
            {
                await UOW.Rollback();
                Logging.CreateSystemLog(Exception, nameof(DirectSalesOrderService));
            }
            return null;
        }
        private async Task<DirectSalesOrder> WebCalculateOrder(DirectSalesOrder DirectSalesOrder, long StoreId) //web sử dụng cả UOM grouping
        {
            var ProductIds = new List<long>();
            var ItemIds = new List<long>();
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                ItemIds.AddRange(DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).ToList());

                ItemFilter SubItemFilter = new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = ItemIds },
                    Selects = ItemSelect.Id | ItemSelect.SalePrice | ItemSelect.ProductId
                };
                List<Item> ListItems;
                ListItems = await UOW.ItemRepository.List(SubItemFilter);


                ProductIds.AddRange(ListItems.Select(x => x.ProductId).ToList());
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
            List<Item> Items;
            Items = await UOW.ItemRepository.List(ItemFilter);

            Items = await ApplyPrice(Items, StoreId); // áp giá cho Item trong đơn hàng

            ProductFilter ProductFilter = new ProductFilter
            {
                Id = new IdFilter { In = ProductIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id | ProductSelect.TaxType
            };

            List<Product> Products;
            Products = await UOW.ProductRepository.List(ProductFilter);
            var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
            });

            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                    var Product = Products.Where(x => Item.ProductId == x.Id).FirstOrDefault();
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
                    DirectSalesOrderContent.RequestedQuantity = DirectSalesOrderContent.Quantity * UOM.Factor.Value;
                    DirectSalesOrderContent.PrimaryPrice = Item.SalePrice.GetValueOrDefault(0); // mặc định = giá của item vì không cho sửa giá trên mobile
                    DirectSalesOrderContent.SalePrice = DirectSalesOrderContent.PrimaryPrice * UOM.Factor.Value; // giá của một đơn vị item
                    DirectSalesOrderContent.EditedPriceStatusId = EditedPriceStatusEnum.INACTIVE.Id; // mặc định ko cho sửa giá

                    decimal SubAmount = DirectSalesOrderContent.Quantity * DirectSalesOrderContent.SalePrice; // giá của một dòng theo đơn vị tính
                    DirectSalesOrderContent.Amount = SubAmount; // tạm tính
                    if (DirectSalesOrderContent.DiscountPercentage.HasValue)
                    {
                        DirectSalesOrderContent.DiscountAmount = SubAmount * DirectSalesOrderContent.DiscountPercentage.Value / 100;
                        DirectSalesOrderContent.DiscountAmount = Math.Round(DirectSalesOrderContent.DiscountAmount ?? 0, 0);
                        DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                    } // áp giảm giá nếu có
                    else
                    {
                        DirectSalesOrderContent.Amount = SubAmount;
                        if (DirectSalesOrderContent.DiscountAmount.HasValue && DirectSalesOrderContent.DiscountAmount.Value > 0)
                        {
                            DirectSalesOrderContent.Amount = SubAmount - DirectSalesOrderContent.DiscountAmount.Value;
                        }
                    }
                } // gán primaryUOMId, RequestedQuantity
                DirectSalesOrder.SubTotal = DirectSalesOrder.DirectSalesOrderContents.Sum(x => x.Amount); //tổng trước chiết khấu
                if (DirectSalesOrder.GeneralDiscountPercentage.HasValue && DirectSalesOrder.GeneralDiscountPercentage > 0)
                {
                    DirectSalesOrder.GeneralDiscountAmount = DirectSalesOrder.SubTotal * (DirectSalesOrder.GeneralDiscountPercentage / 100);
                    DirectSalesOrder.GeneralDiscountAmount = Math.Round(DirectSalesOrder.GeneralDiscountAmount.Value, 0);
                }  //tính tổng chiết khấu theo % chiết khấu chung

                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    var Product = Products.Where(x => DirectSalesOrderContent.Item.ProductId == x.Id).FirstOrDefault();
                    //phân bổ chiết khấu chung = tổng chiết khấu chung * (tổng từng line/tổng trc chiết khấu)
                    DirectSalesOrderContent.GeneralDiscountPercentage = DirectSalesOrderContent.Amount / DirectSalesOrder.SubTotal * 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount * DirectSalesOrderContent.GeneralDiscountPercentage / 100;
                    DirectSalesOrderContent.GeneralDiscountAmount = Math.Round(DirectSalesOrderContent.GeneralDiscountAmount ?? 0, 0);
                    //thuê từng line = (tổng từng line - chiết khấu phân bổ) * % thuế
                    DirectSalesOrderContent.TaxAmount = (DirectSalesOrderContent.Amount - (DirectSalesOrderContent.GeneralDiscountAmount.HasValue ? DirectSalesOrderContent.GeneralDiscountAmount.Value : 0)) * Product.TaxType.Percentage / 100;
                    DirectSalesOrderContent.TaxAmount = Math.Round(DirectSalesOrderContent.TaxAmount ?? 0, 0);
                } // chiết khấu phân bổ theo dòng
                DirectSalesOrder.TotalTaxAmount = DirectSalesOrder.DirectSalesOrderContents.Where(x => x.TaxAmount.HasValue).Sum(x => x.TaxAmount.Value); // tính tổng thuê theo dòng
                DirectSalesOrder.TotalTaxAmount = Math.Round(DirectSalesOrder.TotalTaxAmount, 0);
                DirectSalesOrder.TotalAfterTax = DirectSalesOrder.SubTotal - (DirectSalesOrder.GeneralDiscountAmount.HasValue ? DirectSalesOrder.GeneralDiscountAmount.Value : 0) + DirectSalesOrder.TotalTaxAmount;  //tổng phải thanh toán
                DirectSalesOrder.Total = DirectSalesOrder.TotalAfterTax; // không có promotion nên tổng sau thuế là tổng cuối
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
            return DirectSalesOrder;
        }
        #endregion

        private async Task<Dictionary<string, string>> MapParameters(DirectSalesOrder DirectSalesOrder)
        {
            var AppUser = await UOW.AppUserRepository.Get(DirectSalesOrder.SaleEmployeeId);
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
            Parameters.Add("Username", AppUser.Username);
            Parameters.Add("Type", nameof(DirectSalesOrder));
            return Parameters;
        }
        private async Task<bool> HasWorkflow(DirectSalesOrder DirectSalesOrder)
        {
            Organization Organization = await UOW.OrganizationRepository.Get(DirectSalesOrder.OrganizationId);
            List<WorkflowDefinition> WorkflowDefinitions = await UOW.WorkflowDefinitionRepository.List(new WorkflowDefinitionFilter
            {
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                StartDate = new DateFilter { LessEqual = StaticParams.DateTimeNow },
                WorkflowTypeId = new IdFilter { Equal = WorkflowTypeEnum.DIRECT_SALES_ORDER.Id },
                Selects = WorkflowDefinitionSelect.Id | WorkflowDefinitionSelect.Organization,
                Skip = 0,
                Take = int.MaxValue,
            });
            WorkflowDefinition WorkflowDefinition = null;
            WorkflowDefinitions = WorkflowDefinitions
                .Where(x => Organization.Path.StartsWith(x.Organization.Path))
                .OrderByDescending(x => x.Organization.Path).ToList();
            foreach (var wd in WorkflowDefinitions)
            {
                if (Organization.Path.StartsWith(wd.Organization.Path))
                {
                    WorkflowDefinition = wd;
                    break;
                }
            }
            return WorkflowDefinition != null;
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
                BalanceAmount = Balances.Sum(x => x.DebitAmount) - Balances.Sum(x => x.CreditAmount);
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
    }
}
