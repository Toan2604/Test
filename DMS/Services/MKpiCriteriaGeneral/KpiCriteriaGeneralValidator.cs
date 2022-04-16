using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiCriteriaGeneral
{
    public interface IKpiCriteriaGeneralValidator : IServiceScoped
    {
        Task<bool> Create(KpiCriteriaGeneral KpiCriteriaGeneral);
        Task<bool> Update(KpiCriteriaGeneral KpiCriteriaGeneral);
        Task<bool> Delete(KpiCriteriaGeneral KpiCriteriaGeneral);
        Task<bool> BulkDelete(List<KpiCriteriaGeneral> KpiCriteriaGenerals);
        Task<bool> Import(List<KpiCriteriaGeneral> KpiCriteriaGenerals);
    }

    public class KpiCriteriaGeneralValidator : IKpiCriteriaGeneralValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiCriteriaGeneralValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiCriteriaGeneral KpiCriteriaGeneral)
        {
            KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter = new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiCriteriaGeneral.Id },
                Selects = KpiCriteriaGeneralSelect.Id
            };

            int count = await UOW.KpiCriteriaGeneralRepository.Count(KpiCriteriaGeneralFilter);
            if (count == 0)
                KpiCriteriaGeneral.AddError(nameof(KpiCriteriaGeneralValidator), nameof(KpiCriteriaGeneral.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiCriteriaGeneral KpiCriteriaGeneral)
        {
            return KpiCriteriaGeneral.IsValidated;
        }

        public async Task<bool> Update(KpiCriteriaGeneral KpiCriteriaGeneral)
        {
            if (await ValidateId(KpiCriteriaGeneral))
            {
            }
            return KpiCriteriaGeneral.IsValidated;
        }

        public async Task<bool> Delete(KpiCriteriaGeneral KpiCriteriaGeneral)
        {
            if (await ValidateId(KpiCriteriaGeneral))
            {
            }
            return KpiCriteriaGeneral.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiCriteriaGeneral> KpiCriteriaGenerals)
        {
            return true;
        }

        public async Task<bool> Import(List<KpiCriteriaGeneral> KpiCriteriaGenerals)
        {
            return true;
        }
    }
}
