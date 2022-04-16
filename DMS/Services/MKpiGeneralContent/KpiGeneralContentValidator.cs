using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiGeneralContent
{
    public interface IKpiGeneralContentValidator : IServiceScoped
    {
        Task<bool> Create(KpiGeneralContent KpiGeneralContent);
        Task<bool> Update(KpiGeneralContent KpiGeneralContent);
        Task<bool> Delete(KpiGeneralContent KpiGeneralContent);
        Task<bool> BulkDelete(List<KpiGeneralContent> KpiGeneralContents);
        Task<bool> Import(List<KpiGeneralContent> KpiGeneralContents);
    }

    public class KpiGeneralContentValidator : IKpiGeneralContentValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiGeneralContentValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiGeneralContent KpiGeneralContent)
        {
            KpiGeneralContentFilter KpiGeneralContentFilter = new KpiGeneralContentFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiGeneralContent.Id },
                Selects = KpiGeneralContentSelect.Id
            };

            int count = await UOW.KpiGeneralContentRepository.Count(KpiGeneralContentFilter);
            if (count == 0)
                KpiGeneralContent.AddError(nameof(KpiGeneralContentValidator), nameof(KpiGeneralContent.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(KpiGeneralContent KpiGeneralContent)
        {
            return KpiGeneralContent.IsValidated;
        }

        public async Task<bool> Update(KpiGeneralContent KpiGeneralContent)
        {
            if (await ValidateId(KpiGeneralContent))
            {
            }
            return KpiGeneralContent.IsValidated;
        }

        public async Task<bool> Delete(KpiGeneralContent KpiGeneralContent)
        {
            if (await ValidateId(KpiGeneralContent))
            {
            }
            return KpiGeneralContent.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiGeneralContent> KpiGeneralContents)
        {
            foreach (KpiGeneralContent KpiGeneralContent in KpiGeneralContents)
            {
                await Delete(KpiGeneralContent);
            }
            return KpiGeneralContents.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiGeneralContent> KpiGeneralContents)
        {
            return true;
        }
    }
}
