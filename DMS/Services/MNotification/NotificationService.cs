using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MNotification
{
    public interface INotificationService : IServiceScoped
    {
        Task<int> Count(NotificationFilter NotificationFilter);
        Task<List<Notification>> List(NotificationFilter NotificationFilter);
        Task<Notification> Get(long Id);
        Task<Notification> Create(Notification Notification);
        Task<Notification> Update(Notification Notification);
        Task<Notification> Delete(Notification Notification);
        Task<Notification> Send(Notification Notification);
        Task<List<Notification>> BulkDelete(List<Notification> Notifications);
        Task<List<Notification>> Import(List<Notification> Notifications);
        NotificationFilter ToFilter(NotificationFilter NotificationFilter);
    }

    public class NotificationService : BaseService, INotificationService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRabbitManager RabbitManager;
        private INotificationValidator NotificationValidator;
        private INotificationTemplate NotificationTemplate;

        public NotificationService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            INotificationValidator NotificationValidator,
            INotificationTemplate NotificationTemplate
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.NotificationValidator = NotificationValidator;
            this.NotificationTemplate = NotificationTemplate;
        }
        public async Task<int> Count(NotificationFilter NotificationFilter)
        {
            try
            {
                int result = await UOW.NotificationRepository.Count(NotificationFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return 0;
        }

        public async Task<List<Notification>> List(NotificationFilter NotificationFilter)
        {
            try
            {
                List<Notification> Notifications = await UOW.NotificationRepository.List(NotificationFilter);
                return Notifications;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }
        public async Task<Notification> Get(long Id)
        {
            Notification Notification = await UOW.NotificationRepository.Get(Id);
            if (Notification == null)
                return null;
            return Notification;
        }

        public async Task<Notification> Send(Notification Notification)
        {
            if (!await NotificationValidator.Update(Notification))
                return Notification;

            try
            {
                var oldData = await UOW.NotificationRepository.Get(Notification.Id);

                Notification.NotificationStatusId = Enums.NotificationStatusEnum.SENT.Id;

                await UOW.NotificationRepository.Update(Notification);
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);

                List<AppUser> AppUsers = new List<AppUser>();
                if (Notification.OrganizationId.HasValue)
                {
                    AppUserFilter AppUserFilter = new AppUserFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        OrganizationId = new IdFilter { Equal = Notification.OrganizationId.Value },
                        Selects = AppUserSelect.Id | AppUserSelect.RowId,
                        StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
                    };

                    AppUsers = await UOW.AppUserRepository.List(AppUserFilter);
                }
                else
                {
                    AppUserFilter AppUserFilter = new AppUserFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = AppUserSelect.Id | AppUserSelect.RowId,
                        StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
                    };

                    AppUsers = await UOW.AppUserRepository.List(AppUserFilter);
                }

                if (AppUsers != null && AppUsers.Any())
                {
                    var AppUserRowIds = AppUsers.Select(x => x.RowId).ToList();
                    List<GlobalUserNotification> GlobalUserNotifications = AppUserRowIds
                        .Select(x => NotificationTemplate.CreateAppUserNotification(CurrentUser.RowId, x, Notification, CurrentUser, NotificationType.CREATE)).ToList();
                    RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                }
                var newData = await UOW.NotificationRepository.Get(Notification.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(NotificationService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public async Task<Notification> Create(Notification Notification)
        {
            if (!await NotificationValidator.Create(Notification))
                return Notification;

            try
            {
                Notification.NotificationStatusId = Enums.NotificationStatusEnum.UNSEND.Id;
                await UOW.NotificationRepository.Create(Notification);
                Logging.CreateAuditLog(Notification, new { }, nameof(NotificationService));
                return await UOW.NotificationRepository.Get(Notification.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public async Task<Notification> Update(Notification Notification)
        {
            if (!await NotificationValidator.Update(Notification))
                return Notification;
            try
            {
                var oldData = await UOW.NotificationRepository.Get(Notification.Id);
                await UOW.NotificationRepository.Update(Notification);
                var newData = await UOW.NotificationRepository.Get(Notification.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(NotificationService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public async Task<Notification> Delete(Notification Notification)
        {
            if (!await NotificationValidator.Delete(Notification))
                return Notification;

            try
            {

                await UOW.NotificationRepository.Delete(Notification);

                Logging.CreateAuditLog(new { }, Notification, nameof(NotificationService));
                return Notification;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public async Task<List<Notification>> BulkDelete(List<Notification> Notifications)
        {
            if (!await NotificationValidator.BulkDelete(Notifications))
                return Notifications;

            try
            {

                await UOW.NotificationRepository.BulkDelete(Notifications);

                Logging.CreateAuditLog(new { }, Notifications, nameof(NotificationService));
                return Notifications;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public async Task<List<Notification>> Import(List<Notification> Notifications)
        {
            if (!await NotificationValidator.Import(Notifications))
                return Notifications;
            try
            {

                await UOW.NotificationRepository.BulkMerge(Notifications);


                Logging.CreateAuditLog(Notifications, new { }, nameof(NotificationService));
                return Notifications;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NotificationService));
            }
            return null;
        }

        public NotificationFilter ToFilter(NotificationFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<NotificationFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                NotificationFilter subFilter = new NotificationFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }

    }
}
