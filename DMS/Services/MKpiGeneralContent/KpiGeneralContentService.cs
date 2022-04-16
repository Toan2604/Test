using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiGeneralContent
{
    public interface IKpiGeneralContentService : IServiceScoped
    {
        Task<int> Count(KpiGeneralContentFilter KpiGeneralContentFilter);
        Task<List<KpiGeneralContent>> List(KpiGeneralContentFilter KpiGeneralContentFilter);
        Task<KpiGeneralContent> Get(long Id);
        Task<KpiGeneralContent> Create(KpiGeneralContent KpiGeneralContent);
        Task<KpiGeneralContent> Update(KpiGeneralContent KpiGeneralContent);
        Task<KpiGeneralContent> Delete(KpiGeneralContent KpiGeneralContent);
        Task<List<KpiGeneralContent>> BulkDelete(List<KpiGeneralContent> KpiGeneralContents);
        Task<List<KpiGeneralContent>> Import(List<KpiGeneralContent> KpiGeneralContents);
        Task<KpiGeneralContentFilter> ToFilter(KpiGeneralContentFilter KpiGeneralContentFilter);
    }

    public class KpiGeneralContentService : BaseService, IKpiGeneralContentService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiGeneralContentValidator KpiGeneralContentValidator;

        public KpiGeneralContentService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiGeneralContentValidator KpiGeneralContentValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiGeneralContentValidator = KpiGeneralContentValidator;
        }
        public async Task<int> Count(KpiGeneralContentFilter KpiGeneralContentFilter)
        {
            try
            {
                int result = await UOW.KpiGeneralContentRepository.Count(KpiGeneralContentFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return 0;
        }

        public async Task<List<KpiGeneralContent>> List(KpiGeneralContentFilter KpiGeneralContentFilter)
        {
            try
            {
                List<KpiGeneralContent> KpiGeneralContents = await UOW.KpiGeneralContentRepository.List(KpiGeneralContentFilter);
                return KpiGeneralContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }
        public async Task<KpiGeneralContent> Get(long Id)
        {
            KpiGeneralContent KpiGeneralContent = await UOW.KpiGeneralContentRepository.Get(Id);
            if (KpiGeneralContent == null)
                return null;
            return KpiGeneralContent;
        }

        public async Task<KpiGeneralContent> Create(KpiGeneralContent KpiGeneralContent)
        {
            if (!await KpiGeneralContentValidator.Create(KpiGeneralContent))
                return KpiGeneralContent;

            try
            {

                await UOW.KpiGeneralContentRepository.Create(KpiGeneralContent);

                KpiGeneralContent = await UOW.KpiGeneralContentRepository.Get(KpiGeneralContent.Id);
                Logging.CreateAuditLog(KpiGeneralContent, new { }, nameof(KpiGeneralContentService));
                return KpiGeneralContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }

        public async Task<KpiGeneralContent> Update(KpiGeneralContent KpiGeneralContent)
        {
            if (!await KpiGeneralContentValidator.Update(KpiGeneralContent))
                return KpiGeneralContent;
            try
            {
                var oldData = await UOW.KpiGeneralContentRepository.Get(KpiGeneralContent.Id);


                await UOW.KpiGeneralContentRepository.Update(KpiGeneralContent);


                KpiGeneralContent = await UOW.KpiGeneralContentRepository.Get(KpiGeneralContent.Id);
                Logging.CreateAuditLog(KpiGeneralContent, oldData, nameof(KpiGeneralContentService));
                return KpiGeneralContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }

        public async Task<KpiGeneralContent> Delete(KpiGeneralContent KpiGeneralContent)
        {
            if (!await KpiGeneralContentValidator.Delete(KpiGeneralContent))
                return KpiGeneralContent;

            try
            {

                await UOW.KpiGeneralContentRepository.Delete(KpiGeneralContent);

                Logging.CreateAuditLog(new { }, KpiGeneralContent, nameof(KpiGeneralContentService));
                return KpiGeneralContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }

        public async Task<List<KpiGeneralContent>> BulkDelete(List<KpiGeneralContent> KpiGeneralContents)
        {
            if (!await KpiGeneralContentValidator.BulkDelete(KpiGeneralContents))
                return KpiGeneralContents;

            try
            {

                await UOW.KpiGeneralContentRepository.BulkDelete(KpiGeneralContents);

                Logging.CreateAuditLog(new { }, KpiGeneralContents, nameof(KpiGeneralContentService));
                return KpiGeneralContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }

        public async Task<List<KpiGeneralContent>> Import(List<KpiGeneralContent> KpiGeneralContents)
        {
            if (!await KpiGeneralContentValidator.Import(KpiGeneralContents))
                return KpiGeneralContents;
            try
            {

                await UOW.KpiGeneralContentRepository.BulkMerge(KpiGeneralContents);


                Logging.CreateAuditLog(KpiGeneralContents, new { }, nameof(KpiGeneralContentService));
                return KpiGeneralContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiGeneralContentService));
            }
            return null;
        }

        public async Task<KpiGeneralContentFilter> ToFilter(KpiGeneralContentFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiGeneralContentFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiGeneralContentFilter subFilter = new KpiGeneralContentFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.KpiGeneralId))
                        subFilter.KpiGeneralId = FilterBuilder.Merge(subFilter.KpiGeneralId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.KpiCriteriaGeneralId))
                        subFilter.KpiCriteriaGeneralId = FilterBuilder.Merge(subFilter.KpiCriteriaGeneralId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
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
