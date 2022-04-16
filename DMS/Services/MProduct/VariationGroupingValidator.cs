using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IVariationGroupingValidator : IServiceScoped
    {
        Task<bool> Create(VariationGrouping VariationGrouping);
        Task<bool> Update(VariationGrouping VariationGrouping);
        Task<bool> Delete(VariationGrouping VariationGrouping);
        Task<bool> BulkDelete(List<VariationGrouping> VariationGroupings);
        Task<bool> Import(List<VariationGrouping> VariationGroupings);
    }

    public class VariationGroupingValidator : IVariationGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public VariationGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(VariationGrouping VariationGrouping)
        {
            VariationGroupingFilter VariationGroupingFilter = new VariationGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = VariationGrouping.Id },
                Selects = VariationGroupingSelect.Id
            };

            int count = await UOW.VariationGroupingRepository.Count(VariationGroupingFilter);
            if (count == 0)
                VariationGrouping.AddError(nameof(VariationGroupingValidator), nameof(VariationGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(VariationGrouping VariationGrouping)
        {
            return VariationGrouping.IsValidated;
        }

        public async Task<bool> Update(VariationGrouping VariationGrouping)
        {
            if (await ValidateId(VariationGrouping))
            {
            }
            return VariationGrouping.IsValidated;
        }

        public async Task<bool> Delete(VariationGrouping VariationGrouping)
        {
            if (await ValidateId(VariationGrouping))
            {
            }
            return VariationGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<VariationGrouping> VariationGroupings)
        {
            return true;
        }

        public async Task<bool> Import(List<VariationGrouping> VariationGroupings)
        {
            return true;
        }
    }
}
