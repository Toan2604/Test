using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MExportTemplate
{
    public interface IExportTemplateService : IServiceScoped
    {
        Task<int> Count(ExportTemplateFilter ExportTemplateFilter);
        Task<List<ExportTemplate>> List(ExportTemplateFilter ExportTemplateFilter);
        Task<ExportTemplate> Get(long Id);
        Task<ExportTemplate> Update(ExportTemplate ExportTemplate);
        Task<ExportTemplateFilter> ToFilter(ExportTemplateFilter ExportTemplateFilter);
    }

    public class ExportTemplateService : BaseService, IExportTemplateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IExportTemplateValidator ExportTemplateValidator;

        public ExportTemplateService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IExportTemplateValidator ExportTemplateValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ExportTemplateValidator = ExportTemplateValidator;
        }
        public async Task<int> Count(ExportTemplateFilter ExportTemplateFilter)
        {
            try
            {
                int result = await UOW.ExportTemplateRepository.Count(ExportTemplateFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ExportTemplateService));
            }
            return 0;
        }

        public async Task<List<ExportTemplate>> List(ExportTemplateFilter ExportTemplateFilter)
        {
            try
            {
                List<ExportTemplate> ExportTemplates = await UOW.ExportTemplateRepository.List(ExportTemplateFilter);
                return ExportTemplates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ExportTemplateService));
            }
            return null;
        }

        public async Task<ExportTemplate> Get(long Id)
        {
            ExportTemplate ExportTemplate = await UOW.ExportTemplateRepository.Get(Id);
            if (ExportTemplate == null)
                return null;
            return ExportTemplate;
        }

        public async Task<ExportTemplate> Update(ExportTemplate ExportTemplate)
        {
            if (!await ExportTemplateValidator.Update(ExportTemplate))
                return ExportTemplate;
            try
            {
                var oldData = await UOW.ExportTemplateRepository.Get(ExportTemplate.Id);

                await UOW.ExportTemplateRepository.Update(ExportTemplate);

                ExportTemplate = await UOW.ExportTemplateRepository.Get(ExportTemplate.Id);
                Logging.CreateAuditLog(ExportTemplate, oldData, nameof(ExportTemplateService));
                return ExportTemplate;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ExportTemplateService));
            }
            return null;
        }

        public async Task<ExportTemplateFilter> ToFilter(ExportTemplateFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ExportTemplateFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ExportTemplateFilter subFilter = new ExportTemplateFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Extension))
                        subFilter.Extension = FilterBuilder.Merge(subFilter.Extension, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
    }
}
