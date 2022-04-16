using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiCriteriaItem
{
    public interface IKpiCriteriaItemValidator : IServiceScoped
    {
        Task<bool> Create(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> Update(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> Delete(KpiCriteriaItem KpiCriteriaItem);
        Task<bool> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems);
        Task<bool> Import(List<KpiCriteriaItem> KpiCriteriaItems);
    }

    public class KpiCriteriaItemValidator : IKpiCriteriaItemValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiCriteriaItemValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiCriteriaItem KpiCriteriaItem)
        {
            KpiCriteriaItemFilter KpiCriteriaItemFilter = new KpiCriteriaItemFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiCriteriaItem.Id },
                Selects = KpiCriteriaItemSelect.Id
            };

            int count = await UOW.KpiCriteriaItemRepository.Count(KpiCriteriaItemFilter);
            if (count == 0)
                KpiCriteriaItem.AddError(nameof(KpiCriteriaItemValidator), nameof(KpiCriteriaItem.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiCriteriaItem KpiCriteriaItem)
        {
            return KpiCriteriaItem.IsValidated;
        }

        public async Task<bool> Update(KpiCriteriaItem KpiCriteriaItem)
        {
            if (await ValidateId(KpiCriteriaItem))
            {
            }
            return KpiCriteriaItem.IsValidated;
        }

        public async Task<bool> Delete(KpiCriteriaItem KpiCriteriaItem)
        {
            if (await ValidateId(KpiCriteriaItem))
            {
            }
            return KpiCriteriaItem.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            return true;
        }

        public async Task<bool> Import(List<KpiCriteriaItem> KpiCriteriaItems)
        {
            return true;
        }
    }
}
