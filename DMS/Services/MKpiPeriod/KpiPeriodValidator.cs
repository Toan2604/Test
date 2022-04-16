using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiPeriod
{
    public interface IKpiPeriodValidator : IServiceScoped
    {
        Task<bool> Create(KpiPeriod KpiPeriod);
        Task<bool> Update(KpiPeriod KpiPeriod);
        Task<bool> Delete(KpiPeriod KpiPeriod);
        Task<bool> BulkDelete(List<KpiPeriod> KpiPeriods);
        Task<bool> Import(List<KpiPeriod> KpiPeriods);
    }

    public class KpiPeriodValidator : IKpiPeriodValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiPeriodValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiPeriod KpiPeriod)
        {
            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiPeriod.Id },
                Selects = KpiPeriodSelect.Id
            };

            int count = await UOW.KpiPeriodRepository.Count(KpiPeriodFilter);
            if (count == 0)
                KpiPeriod.AddError(nameof(KpiPeriodValidator), nameof(KpiPeriod.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiPeriod KpiPeriod)
        {
            return KpiPeriod.IsValidated;
        }

        public async Task<bool> Update(KpiPeriod KpiPeriod)
        {
            if (await ValidateId(KpiPeriod))
            {
            }
            return KpiPeriod.IsValidated;
        }

        public async Task<bool> Delete(KpiPeriod KpiPeriod)
        {
            if (await ValidateId(KpiPeriod))
            {
            }
            return KpiPeriod.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiPeriod> KpiPeriods)
        {
            return true;
        }

        public async Task<bool> Import(List<KpiPeriod> KpiPeriods)
        {
            return true;
        }
    }
}
