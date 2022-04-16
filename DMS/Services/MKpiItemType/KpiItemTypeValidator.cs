using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiItemType
{
    public interface IKpiItemTypeValidator : IServiceScoped
    {
        Task<bool> Create(KpiItemType KpiItemType);
        Task<bool> Update(KpiItemType KpiItemType);
        Task<bool> Delete(KpiItemType KpiItemType);
        Task<bool> BulkDelete(List<KpiItemType> KpiItemTypes);
        Task<bool> Import(List<KpiItemType> KpiItemTypes);
    }

    public class KpiItemTypeValidator : IKpiItemTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiItemTypeValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiItemType KpiItemType)
        {
            KpiItemTypeFilter KpiItemTypeFilter = new KpiItemTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiItemType.Id },
                Selects = KpiItemTypeSelect.Id
            };

            int count = await UOW.KpiItemTypeRepository.Count(KpiItemTypeFilter);
            if (count == 0)
                KpiItemType.AddError(nameof(KpiItemTypeValidator), nameof(KpiItemType.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiItemType KpiItemType)
        {
            return KpiItemType.IsValidated;
        }

        public async Task<bool> Update(KpiItemType KpiItemType)
        {
            if (await ValidateId(KpiItemType))
            {
            }
            return KpiItemType.IsValidated;
        }

        public async Task<bool> Delete(KpiItemType KpiItemType)
        {
            if (await ValidateId(KpiItemType))
            {
            }
            return KpiItemType.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiItemType> KpiItemTypes)
        {
            foreach (KpiItemType KpiItemType in KpiItemTypes)
            {
                await Delete(KpiItemType);
            }
            return KpiItemTypes.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiItemType> KpiItemTypes)
        {
            return true;
        }
    }
}
