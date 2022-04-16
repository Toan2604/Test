using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiYear
{
    public interface IKpiYearValidator : IServiceScoped
    {
        Task<bool> Create(KpiYear KpiYear);
        Task<bool> Update(KpiYear KpiYear);
        Task<bool> Delete(KpiYear KpiYear);
        Task<bool> BulkDelete(List<KpiYear> KpiYears);
        Task<bool> Import(List<KpiYear> KpiYears);
    }

    public class KpiYearValidator : IKpiYearValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiYearValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiYear KpiYear)
        {
            KpiYearFilter KpiYearFilter = new KpiYearFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiYear.Id },
                Selects = KpiYearSelect.Id
            };

            int count = await UOW.KpiYearRepository.Count(KpiYearFilter);
            if (count == 0)
                KpiYear.AddError(nameof(KpiYearValidator), nameof(KpiYear.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiYear KpiYear)
        {
            return KpiYear.IsValidated;
        }

        public async Task<bool> Update(KpiYear KpiYear)
        {
            if (await ValidateId(KpiYear))
            {
            }
            return KpiYear.IsValidated;
        }

        public async Task<bool> Delete(KpiYear KpiYear)
        {
            if (await ValidateId(KpiYear))
            {
            }
            return KpiYear.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiYear> KpiYears)
        {
            return true;
        }

        public async Task<bool> Import(List<KpiYear> KpiYears)
        {
            return true;
        }
    }
}
