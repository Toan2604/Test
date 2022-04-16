using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingType
{
    public interface IKpiProductGroupingTypeValidator : IServiceScoped
    {
        Task<bool> Create(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> Update(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> Delete(KpiProductGroupingType KpiProductGroupingType);
        Task<bool> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes);
        Task<bool> Import(List<KpiProductGroupingType> KpiProductGroupingTypes);
    }

    public class KpiProductGroupingTypeValidator : IKpiProductGroupingTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiProductGroupingTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiProductGroupingType KpiProductGroupingType)
        {
            KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter = new KpiProductGroupingTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiProductGroupingType.Id },
                Selects = KpiProductGroupingTypeSelect.Id
            };

            int count = await UOW.KpiProductGroupingTypeRepository.Count(KpiProductGroupingTypeFilter);
            if (count == 0)
                KpiProductGroupingType.AddError(nameof(KpiProductGroupingTypeValidator), nameof(KpiProductGroupingType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiProductGroupingType KpiProductGroupingType)
        {
            return KpiProductGroupingType.IsValidated;
        }

        public async Task<bool> Update(KpiProductGroupingType KpiProductGroupingType)
        {
            if (await ValidateId(KpiProductGroupingType))
            {
            }
            return KpiProductGroupingType.IsValidated;
        }

        public async Task<bool> Delete(KpiProductGroupingType KpiProductGroupingType)
        {
            if (await ValidateId(KpiProductGroupingType))
            {
            }
            return KpiProductGroupingType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            foreach (KpiProductGroupingType KpiProductGroupingType in KpiProductGroupingTypes)
            {
                await Delete(KpiProductGroupingType);
            }
            return KpiProductGroupingTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiProductGroupingType> KpiProductGroupingTypes)
        {
            return true;
        }
    }
}
