using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPriceListItemHistory
{
    public interface IPriceListItemHistoryValidator : IServiceScoped
    {
        Task<bool> Create(PriceListItemHistory PriceListItemHistory);
        Task<bool> Update(PriceListItemHistory PriceListItemHistory);
        Task<bool> Delete(PriceListItemHistory PriceListItemHistory);
        Task<bool> BulkDelete(List<PriceListItemHistory> PriceListItemHistories);
        Task<bool> Import(List<PriceListItemHistory> PriceListItemHistories);
    }

    public class PriceListItemHistoryValidator : IPriceListItemHistoryValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PriceListItemHistoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PriceListItemHistory PriceListItemHistory)
        {
            PriceListItemHistoryFilter PriceListItemHistoryFilter = new PriceListItemHistoryFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PriceListItemHistory.Id },
                Selects = PriceListItemHistorySelect.Id
            };

            int count = await UOW.PriceListItemHistoryRepository.Count(PriceListItemHistoryFilter);
            if (count == 0)
                PriceListItemHistory.AddError(nameof(PriceListItemHistoryValidator), nameof(PriceListItemHistory.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PriceListItemHistory PriceListItemHistory)
        {
            return PriceListItemHistory.IsValidated;
        }

        public async Task<bool> Update(PriceListItemHistory PriceListItemHistory)
        {
            if (await ValidateId(PriceListItemHistory))
            {
            }
            return PriceListItemHistory.IsValidated;
        }

        public async Task<bool> Delete(PriceListItemHistory PriceListItemHistory)
        {
            if (await ValidateId(PriceListItemHistory))
            {
            }
            return PriceListItemHistory.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PriceListItemHistory> PriceListItemHistories)
        {
            foreach (PriceListItemHistory PriceListItemHistory in PriceListItemHistories)
            {
                await Delete(PriceListItemHistory);
            }
            return PriceListItemHistories.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<PriceListItemHistory> PriceListItemHistories)
        {
            return true;
        }
    }
}
