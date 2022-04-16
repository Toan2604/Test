using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGroupingContent
{
    public interface IKpiProductGroupingContentService : IServiceScoped
    {
        Task<int> Count(KpiProductGroupingContentFilter KpiProductGroupingContentFilter);
        Task<List<KpiProductGroupingContent>> List(KpiProductGroupingContentFilter KpiProductGroupingContentFilter);
        Task<KpiProductGroupingContent> Get(long Id);
        Task<KpiProductGroupingContent> Create(KpiProductGroupingContent KpiProductGroupingContent);
        Task<KpiProductGroupingContent> Update(KpiProductGroupingContent KpiProductGroupingContent);
        Task<KpiProductGroupingContent> Delete(KpiProductGroupingContent KpiProductGroupingContent);
        Task<List<KpiProductGroupingContent>> BulkDelete(List<KpiProductGroupingContent> KpiProductGroupingContents);
        Task<List<KpiProductGroupingContent>> Import(List<KpiProductGroupingContent> KpiProductGroupingContents);
        Task<KpiProductGroupingContentFilter> ToFilter(KpiProductGroupingContentFilter KpiProductGroupingContentFilter);
    }

    public class KpiProductGroupingContentService : BaseService, IKpiProductGroupingContentService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiProductGroupingContentValidator KpiProductGroupingContentValidator;

        public KpiProductGroupingContentService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IKpiProductGroupingContentValidator KpiProductGroupingContentValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiProductGroupingContentValidator = KpiProductGroupingContentValidator;
        }
        public async Task<int> Count(KpiProductGroupingContentFilter KpiProductGroupingContentFilter)
        {
            try
            {
                int result = await UOW.KpiProductGroupingContentRepository.Count(KpiProductGroupingContentFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return 0;
        }

        public async Task<List<KpiProductGroupingContent>> List(KpiProductGroupingContentFilter KpiProductGroupingContentFilter)
        {
            try
            {
                List<KpiProductGroupingContent> KpiProductGroupingContents = await UOW.KpiProductGroupingContentRepository.List(KpiProductGroupingContentFilter);
                return KpiProductGroupingContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;
        }

        public async Task<KpiProductGroupingContent> Get(long Id)
        {
            KpiProductGroupingContent KpiProductGroupingContent = await UOW.KpiProductGroupingContentRepository.Get(Id);
            if (KpiProductGroupingContent == null)
                return null;
            return KpiProductGroupingContent;
        }
        public async Task<KpiProductGroupingContent> Create(KpiProductGroupingContent KpiProductGroupingContent)
        {
            if (!await KpiProductGroupingContentValidator.Create(KpiProductGroupingContent))
                return KpiProductGroupingContent;

            try
            {
                await UOW.KpiProductGroupingContentRepository.Create(KpiProductGroupingContent);
                KpiProductGroupingContent = await UOW.KpiProductGroupingContentRepository.Get(KpiProductGroupingContent.Id);
                Logging.CreateAuditLog(KpiProductGroupingContent, new { }, nameof(KpiProductGroupingContentService));
                return KpiProductGroupingContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;
        }

        public async Task<KpiProductGroupingContent> Update(KpiProductGroupingContent KpiProductGroupingContent)
        {
            if (!await KpiProductGroupingContentValidator.Update(KpiProductGroupingContent))
                return KpiProductGroupingContent;
            try
            {
                var oldData = await UOW.KpiProductGroupingContentRepository.Get(KpiProductGroupingContent.Id);

                await UOW.KpiProductGroupingContentRepository.Update(KpiProductGroupingContent);

                KpiProductGroupingContent = await UOW.KpiProductGroupingContentRepository.Get(KpiProductGroupingContent.Id);
                Logging.CreateAuditLog(KpiProductGroupingContent, oldData, nameof(KpiProductGroupingContentService));
                return KpiProductGroupingContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;
        }

        public async Task<KpiProductGroupingContent> Delete(KpiProductGroupingContent KpiProductGroupingContent)
        {
            if (!await KpiProductGroupingContentValidator.Delete(KpiProductGroupingContent))
                return KpiProductGroupingContent;

            try
            {
                await UOW.KpiProductGroupingContentRepository.Delete(KpiProductGroupingContent);
                Logging.CreateAuditLog(new { }, KpiProductGroupingContent, nameof(KpiProductGroupingContentService));
                return KpiProductGroupingContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;
        }

        public async Task<List<KpiProductGroupingContent>> BulkDelete(List<KpiProductGroupingContent> KpiProductGroupingContents)
        {
            if (!await KpiProductGroupingContentValidator.BulkDelete(KpiProductGroupingContents))
                return KpiProductGroupingContents;

            try
            {
                await UOW.KpiProductGroupingContentRepository.BulkDelete(KpiProductGroupingContents);
                Logging.CreateAuditLog(new { }, KpiProductGroupingContents, nameof(KpiProductGroupingContentService));
                return KpiProductGroupingContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;

        }

        public async Task<List<KpiProductGroupingContent>> Import(List<KpiProductGroupingContent> KpiProductGroupingContents)
        {
            if (!await KpiProductGroupingContentValidator.Import(KpiProductGroupingContents))
                return KpiProductGroupingContents;
            try
            {
                await UOW.KpiProductGroupingContentRepository.BulkMerge(KpiProductGroupingContents);

                Logging.CreateAuditLog(KpiProductGroupingContents, new { }, nameof(KpiProductGroupingContentService));
                return KpiProductGroupingContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiProductGroupingContentService));
            }
            return null;
        }

        public async Task<KpiProductGroupingContentFilter> ToFilter(KpiProductGroupingContentFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<KpiProductGroupingContentFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                KpiProductGroupingContentFilter subFilter = new KpiProductGroupingContentFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.KpiProductGroupingId))
                        subFilter.KpiProductGroupingId = FilterBuilder.Merge(subFilter.KpiProductGroupingId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProductGroupingId))
                        subFilter.ProductGroupingId = FilterBuilder.Merge(subFilter.ProductGroupingId, FilterPermissionDefinition.IdFilter);
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
