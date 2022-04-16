using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingContent
{
    public interface IKpiProductGroupingContentValidator : IServiceScoped
    {
        Task<bool> Create(KpiProductGroupingContent KpiProductGroupingContent);
        Task<bool> Update(KpiProductGroupingContent KpiProductGroupingContent);
        Task<bool> Delete(KpiProductGroupingContent KpiProductGroupingContent);
        Task<bool> BulkDelete(List<KpiProductGroupingContent> KpiProductGroupingContents);
        Task<bool> Import(List<KpiProductGroupingContent> KpiProductGroupingContents);
    }

    public class KpiProductGroupingContentValidator : IKpiProductGroupingContentValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiProductGroupingContentValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiProductGroupingContent KpiProductGroupingContent)
        {
            KpiProductGroupingContentFilter KpiProductGroupingContentFilter = new KpiProductGroupingContentFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiProductGroupingContent.Id },
                Selects = KpiProductGroupingContentSelect.Id
            };

            int count = await UOW.KpiProductGroupingContentRepository.Count(KpiProductGroupingContentFilter);
            if (count == 0)
                KpiProductGroupingContent.AddError(nameof(KpiProductGroupingContentValidator), nameof(KpiProductGroupingContent.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiProductGroupingContent KpiProductGroupingContent)
        {
            return KpiProductGroupingContent.IsValidated;
        }

        public async Task<bool> Update(KpiProductGroupingContent KpiProductGroupingContent)
        {
            if (await ValidateId(KpiProductGroupingContent))
            {
            }
            return KpiProductGroupingContent.IsValidated;
        }

        public async Task<bool> Delete(KpiProductGroupingContent KpiProductGroupingContent)
        {
            if (await ValidateId(KpiProductGroupingContent))
            {
            }
            return KpiProductGroupingContent.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiProductGroupingContent> KpiProductGroupingContents)
        {
            foreach (KpiProductGroupingContent KpiProductGroupingContent in KpiProductGroupingContents)
            {
                await Delete(KpiProductGroupingContent);
            }
            return KpiProductGroupingContents.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiProductGroupingContent> KpiProductGroupingContents)
        {
            return true;
        }
    }
}
