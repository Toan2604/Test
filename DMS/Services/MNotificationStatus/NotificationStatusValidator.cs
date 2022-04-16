using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MNotificationStatus
{
    public interface INotificationStatusValidator : IServiceScoped
    {
        Task<bool> Create(NotificationStatus NotificationStatus);
        Task<bool> Update(NotificationStatus NotificationStatus);
        Task<bool> Delete(NotificationStatus NotificationStatus);
        Task<bool> BulkDelete(List<NotificationStatus> NotificationStatuses);
        Task<bool> Import(List<NotificationStatus> NotificationStatuses);
    }

    public class NotificationStatusValidator : INotificationStatusValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public NotificationStatusValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(NotificationStatus NotificationStatus)
        {
            NotificationStatusFilter NotificationStatusFilter = new NotificationStatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = NotificationStatus.Id },
                Selects = NotificationStatusSelect.Id
            };

            int count = await UOW.NotificationStatusRepository.Count(NotificationStatusFilter);
            if (count == 0)
                NotificationStatus.AddError(nameof(NotificationStatusValidator), nameof(NotificationStatus.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(NotificationStatus NotificationStatus)
        {
            return NotificationStatus.IsValidated;
        }

        public async Task<bool> Update(NotificationStatus NotificationStatus)
        {
            if (await ValidateId(NotificationStatus))
            {
            }
            return NotificationStatus.IsValidated;
        }

        public async Task<bool> Delete(NotificationStatus NotificationStatus)
        {
            if (await ValidateId(NotificationStatus))
            {
            }
            return NotificationStatus.IsValidated;
        }

        public async Task<bool> BulkDelete(List<NotificationStatus> NotificationStatuses)
        {
            return true;
        }

        public async Task<bool> Import(List<NotificationStatus> NotificationStatuses)
        {
            return true;
        }
    }
}
