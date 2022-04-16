using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPriceListType
{
    public interface IPriceListTypeValidator : IServiceScoped
    {
        Task<bool> Create(PriceListType PriceListType);
        Task<bool> Update(PriceListType PriceListType);
        Task<bool> Delete(PriceListType PriceListType);
        Task<bool> BulkDelete(List<PriceListType> PriceListTypes);
        Task<bool> Import(List<PriceListType> PriceListTypes);
    }

    public class PriceListTypeValidator : IPriceListTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PriceListTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(PriceListType PriceListType)
        {
            PriceListTypeFilter PriceListTypeFilter = new PriceListTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = PriceListType.Id },
                Selects = PriceListTypeSelect.Id
            };

            int count = await UOW.PriceListTypeRepository.Count(PriceListTypeFilter);
            if (count == 0)
                PriceListType.AddError(nameof(PriceListTypeValidator), nameof(PriceListType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(PriceListType PriceListType)
        {
            return PriceListType.IsValidated;
        }

        public async Task<bool> Update(PriceListType PriceListType)
        {
            if (await ValidateId(PriceListType))
            {
            }
            return PriceListType.IsValidated;
        }

        public async Task<bool> Delete(PriceListType PriceListType)
        {
            if (await ValidateId(PriceListType))
            {
            }
            return PriceListType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<PriceListType> PriceListTypes)
        {
            return true;
        }

        public async Task<bool> Import(List<PriceListType> PriceListTypes)
        {
            return true;
        }
    }
}
