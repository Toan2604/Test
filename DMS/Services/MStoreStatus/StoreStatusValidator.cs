using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreStatus
{
    public interface IStoreStatusValidator : IServiceScoped
    {
        Task<bool> Create(StoreStatus StoreStatus);
        Task<bool> Update(StoreStatus StoreStatus);
        Task<bool> Delete(StoreStatus StoreStatus);
        Task<bool> BulkDelete(List<StoreStatus> StoreStatuses);
        Task<bool> Import(List<StoreStatus> StoreStatuses);
    }

    public class StoreStatusValidator : IStoreStatusValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreStatusValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreStatus StoreStatus)
        {
            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreStatus.Id },
                Selects = StoreStatusSelect.Id
            };

            int count = await UOW.StoreStatusRepository.Count(StoreStatusFilter);
            if (count == 0)
                StoreStatus.AddError(nameof(StoreStatusValidator), nameof(StoreStatus.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(StoreStatus StoreStatus)
        {
            return StoreStatus.IsValidated;
        }

        public async Task<bool> Update(StoreStatus StoreStatus)
        {
            if (await ValidateId(StoreStatus))
            {
            }
            return StoreStatus.IsValidated;
        }

        public async Task<bool> Delete(StoreStatus StoreStatus)
        {
            if (await ValidateId(StoreStatus))
            {
            }
            return StoreStatus.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreStatus> StoreStatuses)
        {
            foreach (StoreStatus StoreStatus in StoreStatuses)
            {
                await Delete(StoreStatus);
            }
            return StoreStatuses.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<StoreStatus> StoreStatuses)
        {
            return true;
        }
    }
}
