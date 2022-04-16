using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreScoutingStatus
{
    public interface IStoreScoutingStatusValidator : IServiceScoped
    {
        Task<bool> Create(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> Update(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> Delete(StoreScoutingStatus StoreScoutingStatus);
        Task<bool> BulkDelete(List<StoreScoutingStatus> StoreScoutingStatuses);
        Task<bool> Import(List<StoreScoutingStatus> StoreScoutingStatuses);
    }

    public class StoreScoutingStatusValidator : IStoreScoutingStatusValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreScoutingStatusValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreScoutingStatus StoreScoutingStatus)
        {
            StoreScoutingStatusFilter StoreScoutingStatusFilter = new StoreScoutingStatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreScoutingStatus.Id },
                Selects = StoreScoutingStatusSelect.Id
            };

            int count = await UOW.StoreScoutingStatusRepository.Count(StoreScoutingStatusFilter);
            if (count == 0)
                StoreScoutingStatus.AddError(nameof(StoreScoutingStatusValidator), nameof(StoreScoutingStatus.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(StoreScoutingStatus StoreScoutingStatus)
        {
            return StoreScoutingStatus.IsValidated;
        }

        public async Task<bool> Update(StoreScoutingStatus StoreScoutingStatus)
        {
            if (await ValidateId(StoreScoutingStatus))
            {
            }
            return StoreScoutingStatus.IsValidated;
        }

        public async Task<bool> Delete(StoreScoutingStatus StoreScoutingStatus)
        {
            if (await ValidateId(StoreScoutingStatus))
            {
            }
            return StoreScoutingStatus.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreScoutingStatus> StoreScoutingStatuses)
        {
            return true;
        }

        public async Task<bool> Import(List<StoreScoutingStatus> StoreScoutingStatuses)
        {
            return true;
        }
    }
}
