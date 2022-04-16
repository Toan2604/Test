using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiItemContent
{
    public interface IKpiItemContentValidator : IServiceScoped
    {
        Task<bool> Create(KpiItemContent KpiItemContent);
        Task<bool> Update(KpiItemContent KpiItemContent);
        Task<bool> Delete(KpiItemContent KpiItemContent);
        Task<bool> BulkDelete(List<KpiItemContent> KpiItemContents);
        Task<bool> Import(List<KpiItemContent> KpiItemContents);
    }

    public class KpiItemContentValidator : IKpiItemContentValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiItemContentValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiItemContent KpiItemContent)
        {
            KpiItemContentFilter KpiItemContentFilter = new KpiItemContentFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiItemContent.Id },
                Selects = KpiItemContentSelect.Id
            };

            int count = await UOW.KpiItemContentRepository.Count(KpiItemContentFilter);
            if (count == 0)
                KpiItemContent.AddError(nameof(KpiItemContentValidator), nameof(KpiItemContent.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiItemContent KpiItemContent)
        {
            return KpiItemContent.IsValidated;
        }

        public async Task<bool> Update(KpiItemContent KpiItemContent)
        {
            if (await ValidateId(KpiItemContent))
            {
            }
            return KpiItemContent.IsValidated;
        }

        public async Task<bool> Delete(KpiItemContent KpiItemContent)
        {
            if (await ValidateId(KpiItemContent))
            {
            }
            return KpiItemContent.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiItemContent> KpiItemContents)
        {
            foreach (KpiItemContent KpiItemContent in KpiItemContents)
            {
                await Delete(KpiItemContent);
            }
            return KpiItemContents.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiItemContent> KpiItemContents)
        {
            return true;
        }
    }
}
