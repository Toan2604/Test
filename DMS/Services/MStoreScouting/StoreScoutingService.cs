using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.store_scouting;
using DMS.Services.MNotification;
using DMS.Services.MOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreScouting
{
    public interface IStoreScoutingService : IServiceScoped
    {
        Task<int> Count(StoreScoutingFilter StoreScoutingFilter);
        Task<List<StoreScouting>> List(StoreScoutingFilter StoreScoutingFilter);
        Task<StoreScouting> Get(long Id);
        Task<StoreScouting> Create(StoreScouting StoreScouting);
        Task<StoreScouting> Update(StoreScouting StoreScouting);
        Task<StoreScouting> Delete(StoreScouting StoreScouting);
        Task<StoreScouting> Reject(StoreScouting StoreScouting);
        Task<List<StoreScouting>> Import(List<StoreScouting> StoreScoutings);
        Task<StoreScoutingFilter> ToFilter(StoreScoutingFilter StoreScoutingFilter);
    }

    public class StoreScoutingService : BaseService, IStoreScoutingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private INotificationService NotificationService;
        private IOrganizationService OrganizationService;
        private IStoreScoutingValidator StoreScoutingValidator;
        private IStoreScoutingTemplate StoreScoutingTemplate;
        private IRabbitManager RabbitManager;

        public StoreScoutingService(
            IUOW UOW,
            ILogging Logging,
            INotificationService NotificationService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext,
            IStoreScoutingValidator StoreScoutingValidator,
            IStoreScoutingTemplate StoreScoutingTemplate,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.NotificationService = NotificationService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
            this.StoreScoutingValidator = StoreScoutingValidator;
            this.StoreScoutingTemplate = StoreScoutingTemplate;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreScoutingFilter StoreScoutingFilter)
        {
            try
            {
                int result = await UOW.StoreScoutingRepository.Count(StoreScoutingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return 0;
        }

        public async Task<List<StoreScouting>> List(StoreScoutingFilter StoreScoutingFilter)
        {
            try
            {
                List<StoreScouting> StoreScoutings = await UOW.StoreScoutingRepository.List(StoreScoutingFilter);
                return StoreScoutings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }
        public async Task<StoreScouting> Get(long Id)
        {
            StoreScouting StoreScouting = await UOW.StoreScoutingRepository.Get(Id);
            if (StoreScouting == null)
                return null;
            return StoreScouting;
        }

        public async Task<StoreScouting> Create(StoreScouting StoreScouting)
        {
            if (!await StoreScoutingValidator.Create(StoreScouting))
                return StoreScouting;

            try
            {
                var User = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                StoreScouting.CreatorId = User.Id;
                StoreScouting.OrganizationId = User.OrganizationId;
                StoreScouting.StoreScoutingStatusId = Enums.StoreScoutingStatusEnum.NOTOPEN.Id;

                await UOW.StoreScoutingRepository.Create(StoreScouting);

                StoreScouting = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                var RecipientIds = await ListReceipientId(User, StoreScoutingRoute.Reject);
                var Recipients = await UOW.AppUserRepository.List(RecipientIds);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification CreatorUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, User.RowId, StoreScouting, User, NotificationType.TOCREATOR);
                 GlobalUserNotifications.Add(CreatorUserNotification);
                foreach (var Recipient in Recipients)
                {
                    if (Recipient.RowId == User.RowId) continue;
                    GlobalUserNotification GlobalUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, Recipient.RowId, StoreScouting, User, NotificationType.CREATE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }

                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Logging.CreateAuditLog(StoreScouting, new { }, nameof(StoreScoutingService));
                StoreScouting = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                Sync(StoreScouting);
                return StoreScouting;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }


        public async Task<StoreScouting> Update(StoreScouting StoreScouting)
        {
            if (!await StoreScoutingValidator.Update(StoreScouting))
                return StoreScouting;
            try
            {
                var User = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var oldData = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);

                await UOW.StoreScoutingRepository.Update(StoreScouting);

                var newData = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(StoreScoutingService));
                Sync(newData);

                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>(); 
                var RecipientIds = await ListReceipientId(User, StoreScoutingRoute.Reject);
                var Recipients = await UOW.AppUserRepository.List(RecipientIds);
                GlobalUserNotification CreatorUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, User.RowId, oldData, User, NotificationType.TOUPDATER);
                GlobalUserNotifications.Add(CreatorUserNotification);
                foreach (var Recipient in Recipients)
                {
                    if (Recipient.RowId == User.RowId) continue;
                    GlobalUserNotification GlobalUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, Recipient.RowId, oldData, User, NotificationType.UPDATE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }

        public async Task<StoreScouting> Delete(StoreScouting StoreScouting)
        {
            if (!await StoreScoutingValidator.Delete(StoreScouting))
                return StoreScouting;
            try
            {
                var User = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var oldData = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                await UOW.StoreScoutingRepository.Delete(StoreScouting);

                StoreScouting = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                Sync(StoreScouting);

                var RecipientIds = await ListReceipientId(User, StoreScoutingRoute.Reject);
                var Recipients = await UOW.AppUserRepository.List(RecipientIds);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification CreatorUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, User.RowId, StoreScouting, User, NotificationType.TODELETER);
                GlobalUserNotifications.Add(CreatorUserNotification);
                foreach (var Recipient in Recipients)
                {
                    if (Recipient.RowId == User.RowId) continue;
                    GlobalUserNotification GlobalUserNotification = StoreScoutingTemplate.CreateAppUserNotification(User.RowId, Recipient.RowId, StoreScouting, User, NotificationType.DELETE);
                    GlobalUserNotifications.Add(GlobalUserNotification);
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code); 

                Logging.CreateAuditLog(StoreScouting, oldData, nameof(StoreScoutingService));
                return StoreScouting;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }

        public async Task<StoreScouting> Reject(StoreScouting StoreScouting)
        {
            if (!await StoreScoutingValidator.Reject(StoreScouting))
                return StoreScouting;

            try
            {
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var oldData = await UOW.StoreScoutingRepository.Get(StoreScouting.Id);
                var Creator = await UOW.AppUserRepository.Get(oldData.CreatorId);
                oldData.StoreScoutingStatusId = Enums.StoreScoutingStatusEnum.REJECTED.Id;

                await UOW.StoreScoutingRepository.Update(oldData);
                Sync(oldData);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                GlobalUserNotification GlobalUserNotification = StoreScoutingTemplate.CreateAppUserNotification(CurrentUser.RowId, Creator.RowId, StoreScouting, CurrentUser, NotificationType.REJECT);
                GlobalUserNotification CreatorUserNotification = StoreScoutingTemplate.CreateAppUserNotification(CurrentUser.RowId, CurrentUser.RowId, StoreScouting, CurrentUser, NotificationType.TOREJECTER);
                GlobalUserNotifications.Add(GlobalUserNotification);
                GlobalUserNotifications.Add(CreatorUserNotification);
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                Mail mail = new Mail
                {
                    Subject = "Từ chối tạo đại lý",
                    Body = $"{oldData.Name} đã bị từ chối bởi {CurrentUser.DisplayName}. Chi tiết xem tại {StoreScouting.Link}",
                    Recipients = new List<string> { Creator.Email },
                    RowId = Guid.NewGuid()
                };
                RabbitManager.PublishSingle(mail, RoutingKeyEnum.MailSend.Code);
                Logging.CreateAuditLog(StoreScouting, oldData, nameof(StoreScoutingService));
                return StoreScouting;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }

        public async Task<List<StoreScouting>> Import(List<StoreScouting> StoreScoutings)
        {
            if (!await StoreScoutingValidator.Import(StoreScoutings))
                return StoreScoutings;

            try
            {

                StoreScoutingFilter StoreScoutingFilter = new StoreScoutingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreScoutingSelect.Id | StoreScoutingSelect.Code | StoreScoutingSelect.Name
                };
                List<StoreScouting> dbStoreScoutings = await UOW.StoreScoutingRepository.List(StoreScoutingFilter);
                foreach (var StoreScouting in StoreScoutings)
                {
                    var oldData = dbStoreScoutings.Where(x => x.Id == StoreScouting.Id)
                                .FirstOrDefault();
                    if (oldData != null)
                    {
                        StoreScouting.RowId = oldData.RowId;
                    }
                    else
                    {
                        StoreScouting.RowId = Guid.NewGuid();
                    }
                }

                await UOW.StoreScoutingRepository.BulkMerge(StoreScoutings);

                Logging.CreateAuditLog(StoreScoutings, new { }, nameof(StoreScoutingService));
                return null;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreScoutingService));
            }
            return null;
        }

        private async Task<List<long>> ListReceipientId(AppUser CurrentUser, string Path)
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
                .Where(x => x.Path.StartsWith(CurrentUser.Organization.Path) || CurrentUser.Organization.Path.StartsWith(x.Path))
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
            return AppUserIds;
        }

        public async Task<StoreScoutingFilter> ToFilter(StoreScoutingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreScoutingFilter>();
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
                StoreScoutingFilter subFilter = new StoreScoutingFilter();
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

                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreScoutingStatusId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.StoreScoutingStatusId, FilterPermissionDefinition.IdFilter);

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

        private void Sync(StoreScouting StoreScouting)
        {
            RabbitManager.PublishSingle(StoreScouting, RoutingKeyEnum.StoreScoutingSync.Code);
        }
    }
}
