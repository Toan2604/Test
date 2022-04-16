using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MNotification
{
    public interface INotificationValidator : IServiceScoped
    {
        Task<bool> Create(Notification Notification);
        Task<bool> Update(Notification Notification);
        Task<bool> Delete(Notification Notification);
        Task<bool> BulkDelete(List<Notification> Notifications);
        Task<bool> Import(List<Notification> Notifications);
    }

    public class NotificationValidator : INotificationValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            TitleEmpty,
            TitleOverLength,
            OrganizationNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public NotificationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Notification Notification)
        {
            NotificationFilter NotificationFilter = new NotificationFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Notification.Id },
                Selects = NotificationSelect.Id
            };

            int count = await UOW.NotificationRepository.Count(NotificationFilter);
            if (count == 0)
                Notification.AddError(nameof(NotificationValidator), nameof(Notification.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateTitle(Notification Notification)
        {
            if (string.IsNullOrWhiteSpace(Notification.Title))
            {
                Notification.AddError(nameof(NotificationValidator), nameof(Notification.Title), ErrorCode.TitleEmpty);
            }
            else
            {
                if (Notification.Title.Length > 255)
                {
                    Notification.AddError(nameof(NotificationValidator), nameof(Notification.Title), ErrorCode.TitleOverLength);
                }
            }
            return Notification.IsValidated;
        }

        public async Task<bool> ValidateOrganization(Notification Notification)
        {
            if (Notification.OrganizationId != null && Notification.OrganizationId != 0)
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = Notification.OrganizationId }
                };
                int count = await UOW.OrganizationRepository.Count(OrganizationFilter);

                if (count == 0)
                    Notification.AddError(nameof(NotificationValidator), nameof(Notification.Organization), ErrorCode.OrganizationNotExisted);
            }
            return Notification.IsValidated;
        }

        public async Task<bool> Create(Notification Notification)
        {
            await ValidateTitle(Notification);
            return Notification.IsValidated;
        }

        public async Task<bool> Update(Notification Notification)
        {
            if (await ValidateId(Notification))
            {
            }
            return Notification.IsValidated;
        }

        public async Task<bool> Delete(Notification Notification)
        {
            if (await ValidateId(Notification))
            {
            }
            return Notification.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Notification> Notifications)
        {
            return true;
        }

        public async Task<bool> Import(List<Notification> Notifications)
        {
            return true;
        }
    }
}
