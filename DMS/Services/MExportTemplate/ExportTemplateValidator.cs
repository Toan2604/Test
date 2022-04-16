using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MExportTemplate
{
    public interface IExportTemplateValidator : IServiceScoped
    {
        Task<bool> Create(ExportTemplate ExportTemplate);
        Task<bool> Update(ExportTemplate ExportTemplate);
        Task<bool> Delete(ExportTemplate ExportTemplate);
        Task<bool> BulkDelete(List<ExportTemplate> ExportTemplates);
        Task<bool> Import(List<ExportTemplate> ExportTemplates);
    }

    public class ExportTemplateValidator : IExportTemplateValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ExportTemplateValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(ExportTemplate ExportTemplate)
        {
            ExportTemplateFilter ExportTemplateFilter = new ExportTemplateFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = ExportTemplate.Id },
                Selects = ExportTemplateSelect.Id
            };

            int count = await UOW.ExportTemplateRepository.Count(ExportTemplateFilter);
            if (count == 0)
                ExportTemplate.AddError(nameof(ExportTemplateValidator), nameof(ExportTemplate.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(ExportTemplate ExportTemplate)
        {
            return ExportTemplate.IsValidated;
        }

        public async Task<bool> Update(ExportTemplate ExportTemplate)
        {
            if (await ValidateId(ExportTemplate))
            {
            }
            return ExportTemplate.IsValidated;
        }

        public async Task<bool> Delete(ExportTemplate ExportTemplate)
        {
            if (await ValidateId(ExportTemplate))
            {
            }
            return ExportTemplate.IsValidated;
        }

        public async Task<bool> BulkDelete(List<ExportTemplate> ExportTemplates)
        {
            foreach (ExportTemplate ExportTemplate in ExportTemplates)
            {
                await Delete(ExportTemplate);
            }
            return ExportTemplates.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<ExportTemplate> ExportTemplates)
        {
            return true;
        }
    }
}
