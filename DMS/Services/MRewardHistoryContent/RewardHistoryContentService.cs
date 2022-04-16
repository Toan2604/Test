using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRewardHistoryContent
{
    public interface IRewardHistoryContentService : IServiceScoped
    {
        Task<int> Count(RewardHistoryContentFilter RewardHistoryContentFilter);
        Task<List<RewardHistoryContent>> List(RewardHistoryContentFilter RewardHistoryContentFilter);
        Task<RewardHistoryContent> Get(long Id);
        Task<RewardHistoryContent> Create(RewardHistoryContent RewardHistoryContent);
        Task<RewardHistoryContent> Update(RewardHistoryContent RewardHistoryContent);
        Task<RewardHistoryContent> Delete(RewardHistoryContent RewardHistoryContent);
        Task<List<RewardHistoryContent>> BulkDelete(List<RewardHistoryContent> RewardHistoryContents);
        Task<List<RewardHistoryContent>> Import(List<RewardHistoryContent> RewardHistoryContents);
        Task<RewardHistoryContentFilter> ToFilter(RewardHistoryContentFilter RewardHistoryContentFilter);
    }

    public class RewardHistoryContentService : BaseService, IRewardHistoryContentService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRewardHistoryContentValidator RewardHistoryContentValidator;

        public RewardHistoryContentService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRewardHistoryContentValidator RewardHistoryContentValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RewardHistoryContentValidator = RewardHistoryContentValidator;
        }
        public async Task<int> Count(RewardHistoryContentFilter RewardHistoryContentFilter)
        {
            try
            {
                int result = await UOW.RewardHistoryContentRepository.Count(RewardHistoryContentFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return 0;
        }

        public async Task<List<RewardHistoryContent>> List(RewardHistoryContentFilter RewardHistoryContentFilter)
        {
            try
            {
                List<RewardHistoryContent> RewardHistoryContents = await UOW.RewardHistoryContentRepository.List(RewardHistoryContentFilter);
                return RewardHistoryContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }
        public async Task<RewardHistoryContent> Get(long Id)
        {
            RewardHistoryContent RewardHistoryContent = await UOW.RewardHistoryContentRepository.Get(Id);
            if (RewardHistoryContent == null)
                return null;
            return RewardHistoryContent;
        }

        public async Task<RewardHistoryContent> Create(RewardHistoryContent RewardHistoryContent)
        {
            if (!await RewardHistoryContentValidator.Create(RewardHistoryContent))
                return RewardHistoryContent;

            try
            {

                await UOW.RewardHistoryContentRepository.Create(RewardHistoryContent);

                RewardHistoryContent = await UOW.RewardHistoryContentRepository.Get(RewardHistoryContent.Id);
                Logging.CreateAuditLog(RewardHistoryContent, new { }, nameof(RewardHistoryContentService));
                return RewardHistoryContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }

        public async Task<RewardHistoryContent> Update(RewardHistoryContent RewardHistoryContent)
        {
            if (!await RewardHistoryContentValidator.Update(RewardHistoryContent))
                return RewardHistoryContent;
            try
            {
                var oldData = await UOW.RewardHistoryContentRepository.Get(RewardHistoryContent.Id);


                await UOW.RewardHistoryContentRepository.Update(RewardHistoryContent);


                RewardHistoryContent = await UOW.RewardHistoryContentRepository.Get(RewardHistoryContent.Id);
                Logging.CreateAuditLog(RewardHistoryContent, oldData, nameof(RewardHistoryContentService));
                return RewardHistoryContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }

        public async Task<RewardHistoryContent> Delete(RewardHistoryContent RewardHistoryContent)
        {
            if (!await RewardHistoryContentValidator.Delete(RewardHistoryContent))
                return RewardHistoryContent;

            try
            {

                await UOW.RewardHistoryContentRepository.Delete(RewardHistoryContent);

                Logging.CreateAuditLog(new { }, RewardHistoryContent, nameof(RewardHistoryContentService));
                return RewardHistoryContent;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }

        public async Task<List<RewardHistoryContent>> BulkDelete(List<RewardHistoryContent> RewardHistoryContents)
        {
            if (!await RewardHistoryContentValidator.BulkDelete(RewardHistoryContents))
                return RewardHistoryContents;

            try
            {

                await UOW.RewardHistoryContentRepository.BulkDelete(RewardHistoryContents);

                Logging.CreateAuditLog(new { }, RewardHistoryContents, nameof(RewardHistoryContentService));
                return RewardHistoryContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }

        public async Task<List<RewardHistoryContent>> Import(List<RewardHistoryContent> RewardHistoryContents)
        {
            if (!await RewardHistoryContentValidator.Import(RewardHistoryContents))
                return RewardHistoryContents;
            try
            {

                await UOW.RewardHistoryContentRepository.BulkMerge(RewardHistoryContents);


                Logging.CreateAuditLog(RewardHistoryContents, new { }, nameof(RewardHistoryContentService));
                return RewardHistoryContents;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RewardHistoryContentService));
            }
            return null;
        }

        public async Task<RewardHistoryContentFilter> ToFilter(RewardHistoryContentFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<RewardHistoryContentFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                RewardHistoryContentFilter subFilter = new RewardHistoryContentFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RewardHistoryId))
                        subFilter.RewardHistoryId = FilterBuilder.Merge(subFilter.RewardHistoryId, FilterPermissionDefinition.IdFilter);
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
