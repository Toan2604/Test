using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingCriteria
{
    public interface IKpiProductGroupingCriteriaValidator : IServiceScoped
    {
        Task<bool> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria);
        Task<bool> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
        Task<bool> Import(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias);
    }

    public class KpiProductGroupingCriteriaValidator : IKpiProductGroupingCriteriaValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiProductGroupingCriteriaValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            KpiProductGroupingCriteriaFilter KpiProductGroupingCriteriaFilter = new KpiProductGroupingCriteriaFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiProductGroupingCriteria.Id },
                Selects = KpiProductGroupingCriteriaSelect.Id
            };

            int count = await UOW.KpiProductGroupingCriteriaRepository.Count(KpiProductGroupingCriteriaFilter);
            if (count == 0)
                KpiProductGroupingCriteria.AddError(nameof(KpiProductGroupingCriteriaValidator), nameof(KpiProductGroupingCriteria.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            return KpiProductGroupingCriteria.IsValidated;
        }

        public async Task<bool> Update(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            if (await ValidateId(KpiProductGroupingCriteria))
            {
            }
            return KpiProductGroupingCriteria.IsValidated;
        }

        public async Task<bool> Delete(KpiProductGroupingCriteria KpiProductGroupingCriteria)
        {
            if (await ValidateId(KpiProductGroupingCriteria))
            {
            }
            return KpiProductGroupingCriteria.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            foreach (KpiProductGroupingCriteria KpiProductGroupingCriteria in KpiProductGroupingCriterias)
            {
                await Delete(KpiProductGroupingCriteria);
            }
            return KpiProductGroupingCriterias.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiProductGroupingCriteria> KpiProductGroupingCriterias)
        {
            return true;
        }
    }
}
