using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MImage;
using DMS.Services.MNotification;
using DMS.Services.MStoreScouting;
using DMS.Services.MWorkflow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStore
{
    public interface IStoreService : IServiceScoped
    {
        Task<int> Count(StoreFilter StoreFilter);
        Task<List<Store>> List(StoreFilter StoreFilter);
        Task<List<long>> ListToIds(StoreFilter StoreFilter);
        Task<Store> Get(long Id);
        Task<Store> Create(Store Store);
        Task<Store> Update(Store Store);
        Task<Store> Delete(Store Store);
        Task<List<Store>> FindDuplicatedStore(Store Store);
        Task<List<Store>> BulkDelete(List<Store> Stores);
        Task<List<Store>> Import(List<Store> Stores);
        Task<List<Store>> Export(StoreFilter StoreFilter);
        Task<List<Store>> BulkMerge(List<Store> Stores);
        StoreFilter ToFilter(StoreFilter StoreFilter);
        Task<Image> SaveImage(Image Image);
    }

    public class StoreService : BaseService, IStoreService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreTemplate StoreTemplate;
        private IStoreScoutingTemplate StoreScoutingTemplate;
        private INotificationService NotificationService;
        private IStoreValidator StoreValidator;
        private IWorkflowService WorkflowService;
        private IImageService ImageService;
        private IRabbitManager RabbitManager;
        public StoreService(
            IUOW UOW,
            ILogging Logging,
            INotificationService NotificationService,
            ICurrentContext CurrentContext,
            IImageService ImageService,
            IWorkflowService WorkflowService,
            IStoreValidator StoreValidator,
            IStoreTemplate StoreTemplate,
            IStoreScoutingTemplate StoreScoutingTemplate,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.NotificationService = NotificationService;
            this.WorkflowService = WorkflowService;
            this.StoreValidator = StoreValidator;
            this.StoreTemplate = StoreTemplate;
            this.StoreScoutingTemplate = StoreScoutingTemplate;
            this.ImageService = ImageService;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreFilter StoreFilter)
        {
            try
            {
                int result = await UOW.StoreRepository.Count(StoreFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return 0;
        }

        public async Task<List<Store>> List(StoreFilter StoreFilter)
        {
            try
            {
                List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }
        public async Task<List<long>> ListToIds(StoreFilter StoreFilter)
        {
            try
            {
                List<long> StoreIds = await UOW.StoreRepository.ListToIds(StoreFilter);
                return StoreIds;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<Store> Get(long Id)
        {
            Store Store = await UOW.StoreRepository.Get(Id);
            if (Store == null)
                return null;
            Store.StoreHistories = await UOW.RequestHistoryRepository.ListRequestHistory<Store>(new RequestHistoryFilter { RequestId = Store.Id });
            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter
            {
                Skip = 0,
                Take = 1,
                Selects = StoreCheckingSelect.ALL,
                StoreId = new IdFilter { Equal = Id },
                SaleEmployeeId = new IdFilter { Equal = CurrentContext.UserId },
                CheckOutAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow.Date, Less = StaticParams.DateTimeNow.Date.AddDays(1) }
            };
            int count = await UOW.StoreCheckingRepository.Count(StoreCheckingFilter);
            Store.HasChecking = count > 0;

            List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(
                new StoreUserFilter
                {
                    Skip = 0,
                    Take = 1,
                    StoreId = new IdFilter { Equal = Id },
                    Selects = StoreUserSelect.Id | StoreUserSelect.Username | StoreUserSelect.Status,
                });

            if (StoreUsers.FirstOrDefault() != null)
            {
                Store.StoreUser = StoreUsers.FirstOrDefault();
                Store.StoreUserId = Store.StoreUser.Id;
            }

            return Store;
        }

        public async Task<RequestHistory<Store>> GetHistoryDetail(Guid Id)
        {
            RequestHistory<Store> StoreHistory = await UOW.RequestHistoryRepository.GetRequestHistory<Store>(Id);
            return StoreHistory;
        }

        public async Task<Store> Create(Store Store)
        {
            if (!await StoreValidator.Create(Store))
                return Store;

            try
            {
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                Store.UnsignName = Store.Name.ChangeToEnglishChar();
                Store.UnsignAddress = Store.Address.ChangeToEnglishChar();

                if (Store.BrandInStores != null)
                {
                    Store.BrandInStores.ForEach(x => x.CreatorId = CurrentContext.UserId);
                }
                await UOW.StoreRepository.Create(Store);
                StoreStatusHistory StoreStatusHistory = new StoreStatusHistory
                {
                    StoreId = Store.Id,
                    AppUserId = CurrentContext.UserId,
                    CreatedAt = DateTime.Now,
                };
                if (Store.StoreScoutingId.HasValue)
                    StoreStatusHistory.PreviousStoreStatusId = StoreStatusHistoryTypeEnum.STORE_SCOUTING.Id;
                else
                    StoreStatusHistory.PreviousStoreStatusId = StoreStatusHistoryTypeEnum.NEW.Id;

                if (Store.StoreStatusId == StoreStatusEnum.OFFICIAL.Id)
                    StoreStatusHistory.StoreStatusId = StoreStatusHistoryTypeEnum.OFFICIAL.Id;
                if (Store.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                    StoreStatusHistory.StoreStatusId = StoreStatusHistoryTypeEnum.DRAFT.Id;

                SyncStoreStatusHistory(new List<StoreStatusHistory> { StoreStatusHistory });
                StoreHistory StoreHistory = new StoreHistory
                {
                    StoreId = Store.Id,
                    Name = Store.Name,
                    Code = Store.Code,
                    AppUserId = CurrentContext.UserId,
                    CreatedAt = StaticParams.DateTimeNow,
                    StatusId = Store.StatusId,
                    StoreStatusId = Store.StoreStatusId,
                    EstimatedRevenueId = Store.EstimatedRevenueId,
                    Latitude = Store.Latitude,
                    Longitude = Store.Longitude,
                };
                SyncStoreHistory(new List<StoreHistory>{StoreHistory});

                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                if (Store.StoreScoutingId.HasValue)
                {
                    StoreScouting StoreScouting = await UOW.StoreScoutingRepository.Get(Store.StoreScoutingId.Value);

                    if (StoreScouting != null)
                    {
                        StoreScouting.StoreScoutingStatusId = Enums.StoreScoutingStatusEnum.OPENED.Id;
                    }
                    await UOW.StoreScoutingRepository.Update(StoreScouting);
                    GlobalUserNotification CurrentUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, StoreScouting, Store, CurrentUser, NotificationType.TOCREATOR);
                    GlobalUserNotifications.Add(CurrentUserNotification);
                    if (CurrentUser.Id != StoreScouting.CreatorId)
                    {
                        var SaleEmployee = await UOW.AppUserRepository.GetSimple(StoreScouting.CreatorId);
                        GlobalUserNotification SaleEmployeeUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, StoreScouting, Store, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(SaleEmployeeUserNotification);
                    }


                    Store.StoreImageMappings = StoreScouting.StoreScoutingImageMappings?.Select(x => new StoreImageMapping
                    {
                        StoreId = Store.Id,
                        ImageId = x.ImageId,
                        Image = new Image
                        {
                            Id = x.Image.Id,
                            Name = x.Image.Name,
                            Url = x.Image.Url,
                        }
                    }).ToList();
                    await UOW.StoreRepository.Update(Store);
                }
                else
                {
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, Store, CurrentUser, NotificationType.TOCREATOR);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                    var SaleEmployeeIds = Store.AppUserStoreMappings.Select(x => x.AppUserId).ToList();
                    var SaleEmployees = await UOW.AppUserRepository.List(SaleEmployeeIds);
                    foreach (var Employee in SaleEmployees)
                    {
                        if (Employee.Id == CurrentUser.Id) continue;
                        GlobalUserNotification EmployeeNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, Employee.RowId, Store, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(EmployeeNotification);
                    }
                }

                Store = await UOW.StoreRepository.Get(Store.Id);
                Sync(new List<Store> { Store });


                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                var RequestHistory = await UOW.RequestHistoryRepository.CreateRequestHistory(Store.Id, CurrentContext.UserId, Store, null);
                RabbitManager.PublishSingle(RequestHistory, RoutingKeyEnum.StoreHistorySync.Code);
                Logging.CreateAuditLog(Store, new { }, nameof(StoreService));
                return await UOW.StoreRepository.Get(Store.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<List<Store>> FindDuplicatedStore(Store Store)
        {
            StoreMessage StoreMessage = new StoreMessage();
            List<Store> DuplicatedStores = new List<Store>();
            if (Store.OrganizationId > 0)
            {
                List<Store> StoresInOrg = await UOW.StoreRepository.List(new StoreFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name | StoreSelect.StoreType | StoreSelect.Address
                        | StoreSelect.OwnerPhone | StoreSelect.OwnerName | StoreSelect.StoreStatusId | StoreSelect.Status
                        | StoreSelect.StoreStatus | StoreSelect.Organization,
                    OrganizationId = new IdFilter { Equal = Store.OrganizationId }
                });
                foreach (Store store in StoresInOrg)
                {
                    if (store.Id == Store.Id)
                        continue;
                    bool hasDuplicated = false;
                    if (!string.IsNullOrWhiteSpace(Store.Name) && store.Name != null)
                    {
                        if (Store.Name.ToLower().Equals(store.Name.ToLower()))
                        {
                            store.AddWarning(nameof(StoreValidator), nameof(store.Name), StoreMessage.Warning.Duplicated);
                            hasDuplicated = true;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store.Address) && store.Address != null)
                    {
                        if (Store.Address.ToLower().Equals(store.Address.ToLower()))
                        {
                            store.AddWarning(nameof(StoreValidator), nameof(store.Address), StoreMessage.Warning.Duplicated);
                            hasDuplicated = true;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Store.OwnerPhone) && store.OwnerPhone != null)
                    {
                        if (Store.OwnerPhone.ToLower().Equals(store.OwnerPhone.ToLower()))
                        {
                            store.AddWarning(nameof(StoreValidator), nameof(store.OwnerPhone), StoreMessage.Warning.Duplicated);
                            hasDuplicated = true;
                        }
                    }

                    if (hasDuplicated) DuplicatedStores.Add(store);
                }
            }    
            return DuplicatedStores;
        }
        public async Task<Store> Update(Store Store)
        {
            if (!await StoreValidator.Update(Store))
                return Store;
            try
            {
                var oldData = await UOW.StoreRepository.Get(Store.Id);

                Store.UnsignName = Store.Name.ChangeToEnglishChar();
                Store.UnsignAddress = Store.Address.ChangeToEnglishChar();

                if (Store.BrandInStores != null)
                {
                    foreach (var BrandInStore in Store.BrandInStores)
                    {
                        BrandInStore.StoreId = Store.Id;
                        if (BrandInStore.Id == 0)
                            BrandInStore.CreatorId = CurrentContext.UserId;
                    }
                }
                if (Store.DebtLimited != null && Store.DebtLimited != (oldData.DebtLimited ?? -1))
                {
                    List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(new DirectSalesOrderFilter
                    {
                        Take = int.MaxValue,
                        Skip = 0,
                        Selects = DirectSalesOrderSelect.Id | DirectSalesOrderSelect.BuyerStore | DirectSalesOrderSelect.TotalAfterTax | DirectSalesOrderSelect.InventoryCheckState | DirectSalesOrderSelect.StoreBalanceCheckState,
                        GeneralApprovalStateId = new IdFilter
                        {
                            NotIn = new List<long>
                        {
                            GeneralApprovalStateEnum.APPROVED.Id,
                            GeneralApprovalStateEnum.REJECTED.Id,
                            GeneralApprovalStateEnum.STORE_APPROVED.Id,
                            GeneralApprovalStateEnum.STORE_REJECTED.Id
                        }
                        },
                        BuyerStoreId = new IdFilter { Equal = Store.Id }
                    });
                    List<StoreBalance> StoreBalances = await UOW.StoreBalanceRepository.List(new StoreBalanceFilter
                    {
                        Take = int.MaxValue,
                        Skip = 0,
                        Selects = StoreBalanceSelect.CreditAmount | StoreBalanceSelect.DebitAmount,
                        StoreId = new IdFilter { Equal = Store.Id }
                    });
                    var BalanceAmount = (StoreBalances?.Sum(x => x.DebitAmount) ?? 0) - (StoreBalances?.Sum(x => x.CreditAmount) ?? 0);
                    if (DirectSalesOrders != null && DirectSalesOrders.Any())
                    {
                        List<DirectSalesOrder> BulkUpdateList = new List<DirectSalesOrder>();
                        foreach (var DirectSalesOrder in DirectSalesOrders)
                        {
                            var oldStateId = DirectSalesOrder.StoreBalanceCheckStateId;
                            if ((BalanceAmount + DirectSalesOrder.TotalAfterTax) <= Store.DebtLimited) DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.PASS.Id;
                            else DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.NOT_PASS.Id;
                            if (DirectSalesOrder.StoreBalanceCheckStateId != oldStateId) BulkUpdateList.Add(DirectSalesOrder);
                        }
                        if (BulkUpdateList.Any()) await UOW.DirectSalesOrderRepository.BulkUpdateCheckState(BulkUpdateList);
                    }
                }
                if (Store.StoreStatusId != oldData.StoreStatusId)
                {
                    StoreStatusHistory StoreStatusHistory = new StoreStatusHistory
                    {
                        StoreId = Store.Id,
                        AppUserId = CurrentContext.UserId,
                        CreatedAt = DateTime.Now,
                    };
                    if (oldData.StoreStatusId == StoreStatusEnum.OFFICIAL.Id)
                        StoreStatusHistory.PreviousStoreStatusId = StoreStatusHistoryTypeEnum.OFFICIAL.Id;
                    if (oldData.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                        StoreStatusHistory.PreviousStoreStatusId = StoreStatusHistoryTypeEnum.DRAFT.Id;

                    if (Store.StoreStatusId == StoreStatusEnum.OFFICIAL.Id)
                        StoreStatusHistory.StoreStatusId = StoreStatusHistoryTypeEnum.OFFICIAL.Id;
                    if (Store.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                        StoreStatusHistory.StoreStatusId = StoreStatusHistoryTypeEnum.DRAFT.Id;

                    SyncStoreStatusHistory(new List<StoreStatusHistory> { StoreStatusHistory });
                }

                if (Store.StoreTypeId != oldData.StoreTypeId)
                {
                    StoreTypeHistory StoreTypeHistory = new StoreTypeHistory
                    {
                        StoreId = Store.Id,
                        AppUserId = CurrentContext.UserId,
                        CreatedAt = DateTime.Now,
                    };
                    StoreTypeHistory.PreviousStoreTypeId = oldData.StoreTypeId;
                    StoreTypeHistory.StoreTypeId = Store.StoreTypeId;

                    await UOW.StoreTypeHistoryRepository.Create(StoreTypeHistory);
                }


                await UOW.StoreRepository.Update(Store);
                if (Store.StatusId == StatusEnum.INACTIVE.Id)
                {
                    await UOW.AppUserStoreMappingRepository.Delete(null, oldData.Id);
                }
                if (Store.StoreStatusId == StoreStatusEnum.OFFICIAL.Id && oldData.StoreStatusId != StoreStatusEnum.OFFICIAL.Id)
                {
                    if (oldData.AppUserId.HasValue)
                        await UOW.AppUserStoreMappingRepository.Delete(oldData.AppUserId.Value, oldData.Id);
                    if (Store.AppUserId.HasValue)
                        await UOW.AppUserStoreMappingRepository.Update(Store.AppUserId.Value, Store.Id);
                }

                Store = await UOW.StoreRepository.Get(Store.Id);
                Sync(new List<Store> { Store });

                StoreHistory StoreHistory = new StoreHistory
                {
                    StoreId = Store.Id,
                    Name = Store.Name,
                    Code = Store.Code,
                    AppUserId = CurrentContext.UserId,
                    CreatedAt = StaticParams.DateTimeNow,
                    StatusId = Store.StatusId,
                    StoreStatusId = Store.StoreStatusId,
                    EstimatedRevenueId = Store.EstimatedRevenueId,
                    Latitude = Store.Latitude,
                    Longitude = Store.Longitude,
                };
                SyncStoreHistory(new List<StoreHistory> { StoreHistory });

                #region Thông báo

                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var SaleEmployeeIds = Store.AppUserStoreMappings.Select(x => x.AppUserId).ToList();
                if (CurrentUser.Id != Store.CreatorId && !SaleEmployeeIds.Contains(Store.CreatorId))
                {
                    SaleEmployeeIds.Add(Store.CreatorId);
                }
                var SaleEmployees = await UOW.AppUserRepository.List(SaleEmployeeIds);
                
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                if (Store.StoreStatusId == StoreStatusEnum.OFFICIAL.Id && oldData.StoreStatusId != Store.StoreStatusId)  //khi phê duyệt đại lý
                {
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, Store, CurrentUser, NotificationType.TOAPPROVER);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                    foreach (var SaleEmployee in SaleEmployees)
                    {
                        if (SaleEmployee.Id != CurrentUser.Id) continue;
                        GlobalUserNotification SaleEmployeeNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, Store, CurrentUser, NotificationType.APPROVE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                else if (Store.StoreTypeId != oldData.StoreTypeId) // khi chuyển loại đại lý
                {
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, Store, CurrentUser, NotificationType.TOCHANGER);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                    foreach (var SaleEmployee in SaleEmployees)
                    {
                        if (SaleEmployee.Id != CurrentUser.Id) continue;
                        GlobalUserNotification SaleEmployeeNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, Store, CurrentUser, NotificationType.CHANGE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                else //khi update các thông tin khác
                {
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, Store, CurrentUser, NotificationType.TOUPDATER);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                    foreach (var SaleEmployee in SaleEmployees)
                    {
                        if (SaleEmployee.Id != CurrentUser.Id) continue;
                        GlobalUserNotification SaleEmployeeNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, SaleEmployee.RowId, Store, CurrentUser, NotificationType.UPDATE);

                    }
                }
                #endregion
                if (Store.StatusId != oldData.StatusId)
                {
                    if (Store.StatusId == StatusEnum.INACTIVE.Id)
                    {
                        var StoreUser = (await UOW.StoreUserRepository.List(new StoreUserFilter
                        {
                            Take = 1,
                            Skip = 0,
                            StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                            Selects = StoreUserSelect.ALL
                        })).FirstOrDefault();
                        if (StoreUser != null)
                        {
                            StoreUser.StatusId = StatusEnum.INACTIVE.Id;
                            await UOW.StoreUserRepository.Update(StoreUser);
                        }
                    }
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                var RequestHistory = await UOW.RequestHistoryRepository.CreateRequestHistory(Store.Id, CurrentContext.UserId, Store, null);
                RabbitManager.PublishSingle(RequestHistory, RoutingKeyEnum.StoreHistorySync.Code);
                Logging.CreateAuditLog(Store, oldData, nameof(StoreService));
                return Store;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<Store> Delete(Store Store)
        {
            if (!await StoreValidator.Delete(Store))
                return Store;

            try
            {
                var oldData = await UOW.StoreRepository.Get(Store.Id);

                if (oldData.StoreStatusId == StoreStatusEnum.OFFICIAL.Id)
                {
                    if (oldData.AppUserId.HasValue)
                        await UOW.AppUserStoreMappingRepository.Update(oldData.AppUserId.Value, oldData.Id);
                }

                await UOW.StoreRepository.Delete(Store);


                Store = await UOW.StoreRepository.Get(Store.Id);
                Sync(new List<Store> { Store });

                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                var RecipientIds = await ListAppUserInOrgs(Store);
                var Recipients = await UOW.AppUserRepository.List(RecipientIds);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                {
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, Store, CurrentUser, NotificationType.TODELETER);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                foreach (var Recipient in Recipients)
                {
                    if (Recipient.RowId == CurrentUser.RowId) continue;
                    GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, Recipient.RowId, Store, CurrentUser, NotificationType.DELETE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }

                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Logging.CreateAuditLog(new { }, Store, nameof(StoreService));
                return Store;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<List<Store>> BulkDelete(List<Store> Stores)
        {
            if (!await StoreValidator.BulkDelete(Stores))
                return Stores;

            try
            {
                await UOW.StoreRepository.BulkDelete(Stores);
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                foreach (var Store in Stores)
                {
                    var RecipientIds = await ListAppUserInOrgs(Store);
                    var Recipients = await UOW.AppUserRepository.List(RecipientIds);
                    foreach (var Recipient in Recipients)
                    {
                        GlobalUserNotification GlobalUserNotification = StoreTemplate.CreateAppUserNotification(CurrentUser.RowId, Recipient.RowId, Store, CurrentUser, NotificationType.DELETE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }

                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                var Ids = Stores.Select(x => x.Id).ToList();
                Stores = await UOW.StoreRepository.List(Ids);
                Sync(Stores);

                Logging.CreateAuditLog(new { }, Stores, nameof(StoreService));
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }
        public async Task<List<Store>> BulkMerge(List<Store> Stores)
        {
            if (!await StoreValidator.BulkMerge(Stores))
                return Stores;
            try
            {
                await UOW.StoreRepository.BulkMerge(Stores);

                List<long> Ids = Stores.Select(x => x.Id).ToList();
                Stores = await UOW.StoreRepository.List(Ids);
                Sync(Stores);
                Logging.CreateAuditLog(Stores, new { }, nameof(StoreService));
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<List<Store>> Import(List<Store> Stores)
        {
            if (!await StoreValidator.Import(Stores))
                return Stores;

            try
            {
                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.Name | StoreSelect.ParentStore,
                };
                List<Store> dbStores = await UOW.StoreRepository.List(StoreFilter);
                var createCounter = Stores.Where(x => x.Id == 0).Count();
                foreach (var Store in Stores)
                {
                    Store.UnsignName = Store.Name.ChangeToEnglishChar();
                    Store.UnsignAddress = Store.Address.ChangeToEnglishChar();

                    var oldData = dbStores.Where(x => x.Id == Store.Id)
                                .FirstOrDefault();
                    if (oldData != null)
                    {
                        Store.RowId = oldData.RowId;
                        Store.Used = oldData.Used;
                    }
                    else
                    {
                        Store.Used = false;
                        Store.RowId = Guid.NewGuid();
                    }

                }
                Stores = Stores.Distinct().ToList();
                await UOW.StoreRepository.BulkMerge(Stores);

                dbStores = await UOW.StoreRepository.List(StoreFilter);
                foreach (var store in Stores)
                {
                    if (store.ParentStore != null)
                    {
                        long ParentStoreId = dbStores.Where(p => p.Code == store.ParentStore.Code)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();
                        if (ParentStoreId != 0)
                            store.ParentStoreId = ParentStoreId;
                    }
                }
                await UOW.StoreRepository.BulkMerge(Stores);

                var Ids = Stores.Select(x => x.Id).ToList();
                Stores = await UOW.StoreRepository.List(Ids);
                Sync(Stores);

                List<StoreHistory> StoreHitories = Stores.Select(x => new StoreHistory(x)).ToList();
                foreach (var StoreHistory in StoreHitories)
                {
                    StoreHistory.AppUserId = CurrentContext.UserId;
                    StoreHistory.CreatedAt = StaticParams.DateTimeNow;
                }
                SyncStoreHistory(StoreHitories);

                var dict = Stores.ToDictionary(x => x.Id, y => y);
                var RequestHistories = await UOW.RequestHistoryRepository.BulkCreateRequestHistory(dict, CurrentContext.UserId);
                RabbitManager.PublishList(RequestHistories, RoutingKeyEnum.StoreHistorySync.Code);
                Logging.CreateAuditLog(Stores, new { }, nameof(StoreService));
                return null;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public async Task<List<Store>> Export(StoreFilter StoreFilter)
        {
            try
            {
                StoreFilter.Selects = StoreSelect.Id;
                List<Store> Stores = await UOW.StoreRepository.List(StoreFilter);
                List<long> Ids = Stores.Select(x => x.Id).ToList();
                Stores = new List<Store>();
                for (int i = 0; i < Ids.Count; i += 1000)
                {
                    List<long> SubIds = Ids.Skip(i).Take(1000).ToList();
                    List<Store> SubStores = await UOW.StoreRepository.List(SubIds);
                    Stores.AddRange(SubStores);
                }
                return Stores;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreService));
            }
            return null;
        }

        public StoreFilter ToFilter(StoreFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreFilter subFilter = new StoreFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreGroupingId))
                        subFilter.StoreGroupingId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreTypeId))
                        subFilter.StoreTypeId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterPermissionDefinition.IdFilter;
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

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/store/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            string thumbnailPath = $"/store/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path, thumbnailPath, 128, 128);
            return Image;
        }

        private async Task<List<long>> ListAppUserInOrgs(Store Store)
        {
            var Org = await UOW.OrganizationRepository.Get(Store.OrganizationId);
            List<long> Ids = new List<long>();
            if (Org != null)
            {
                var OrganizationIds = (await UOW.OrganizationRepository.List(new OrganizationFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = OrganizationSelect.Id,
                    Path = new StringFilter { StartWith = Org.Path }
                })).Select(x => x.Id).ToList();
                Ids = (await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.Id,
                    OrganizationId = new IdFilter { In = OrganizationIds }
                })).Select(x => x.Id).Distinct().ToList();
            }

            return Ids;
        }

        private void Sync(List<Store> Stores)
        {
            List<AppUser> AppUsers = new List<AppUser>();
            AppUsers.AddRange(Stores.Select(x => new AppUser { Id = x.CreatorId }));
            AppUsers.AddRange(Stores.Where(x => x.AppUserStoreMappings != null).SelectMany(x => x.AppUserStoreMappings).Select(x => new AppUser { Id = x.AppUserId }));
            AppUsers = AppUsers.Distinct().ToList();
            List<BrandInStore> BrandInStores = Stores.SelectMany(x => x.BrandInStores).Distinct().ToList();
            List<Brand> Brands = Stores.Where(x => x.BrandInStores != null).SelectMany(x => x.BrandInStores).Select(x => new Brand { Id = x.BrandId }).Distinct().ToList();
            List<StoreType> StoreTypes = Stores.Select(x => new StoreType { Id = x.StoreTypeId }).Distinct().ToList();
            List<StoreGrouping> StoreGroupings = Stores.Where(x => x.StoreStoreGroupingMappings != null).SelectMany(x => x.StoreStoreGroupingMappings).Select(x => new StoreGrouping { Id = x.StoreGroupingId }).Distinct().ToList();
            List<Province> Provinces = Stores.Where(x => x.ProvinceId.HasValue).Select(x => new Province { Id = x.ProvinceId.Value }).Distinct().ToList();
            List<District> Districts = Stores.Where(x => x.DistrictId.HasValue).Select(x => new District { Id = x.DistrictId.Value }).Distinct().ToList();
            List<Ward> Wards = Stores.Where(x => x.WardId.HasValue).Select(x => new Ward { Id = x.WardId.Value }).Distinct().ToList();
            List<Organization> Organizations = Stores.Select(x => new Organization { Id = x.OrganizationId }).Distinct().ToList();

            RabbitManager.PublishList(BrandInStores, RoutingKeyEnum.BrandInStoreSync.Code);
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreSync.Code);
            RabbitManager.PublishList(AppUsers, RoutingKeyEnum.AppUserUsed.Code);
            RabbitManager.PublishList(Brands, RoutingKeyEnum.BrandUsed.Code);
            RabbitManager.PublishList(StoreTypes, RoutingKeyEnum.StoreTypeUsed.Code);
            RabbitManager.PublishList(StoreGroupings, RoutingKeyEnum.StoreGroupingUsed.Code);
            RabbitManager.PublishList(Provinces, RoutingKeyEnum.ProvinceUsed.Code);
            RabbitManager.PublishList(Districts, RoutingKeyEnum.DistrictUsed.Code);
            RabbitManager.PublishList(Wards, RoutingKeyEnum.WardUsed.Code);
            RabbitManager.PublishList(Organizations, RoutingKeyEnum.OrganizationUsed.Code);
        }

        private void SyncStoreStatusHistory(List<StoreStatusHistory> StoreStatusHistories)
        {
            RabbitManager.PublishList(StoreStatusHistories, RoutingKeyEnum.StoreStatusHistorySync.Code);
        }
        private void SyncStoreHistory(List<StoreHistory> StoreHistories)
        {
            RabbitManager.PublishList(StoreHistories, RoutingKeyEnum.StoreHistorySync.Code);
        }
    }
}
