using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductGrouping
{
    public interface IPromotionProductGroupingService : IServiceScoped
    {
        Task<int> Count(PromotionProductGroupingFilter PromotionProductGroupingFilter);
        Task<List<PromotionProductGrouping>> List(PromotionProductGroupingFilter PromotionProductGroupingFilter);
        Task<PromotionProductGrouping> Get(long Id);
        Task<PromotionProductGrouping> Create(PromotionProductGrouping PromotionProductGrouping);
        Task<PromotionProductGrouping> Update(PromotionProductGrouping PromotionProductGrouping);
        Task<PromotionProductGrouping> Delete(PromotionProductGrouping PromotionProductGrouping);
        Task<List<PromotionProductGrouping>> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings);
        Task<List<PromotionProductGrouping>> Import(List<PromotionProductGrouping> PromotionProductGroupings);
        Task<PromotionProductGroupingFilter> ToFilter(PromotionProductGroupingFilter PromotionProductGroupingFilter);
    }

    public class PromotionProductGroupingService : BaseService, IPromotionProductGroupingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionProductGroupingValidator PromotionProductGroupingValidator;

        public PromotionProductGroupingService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionProductGroupingValidator PromotionProductGroupingValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionProductGroupingValidator = PromotionProductGroupingValidator;
        }
        public async Task<int> Count(PromotionProductGroupingFilter PromotionProductGroupingFilter)
        {
            try
            {
                int result = await UOW.PromotionProductGroupingRepository.Count(PromotionProductGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return 0;
        }

        public async Task<List<PromotionProductGrouping>> List(PromotionProductGroupingFilter PromotionProductGroupingFilter)
        {
            try
            {
                List<PromotionProductGrouping> PromotionProductGroupings = await UOW.PromotionProductGroupingRepository.List(PromotionProductGroupingFilter);
                return PromotionProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }
        public async Task<PromotionProductGrouping> Get(long Id)
        {
            PromotionProductGrouping PromotionProductGrouping = await UOW.PromotionProductGroupingRepository.Get(Id);
            if (PromotionProductGrouping == null)
                return null;
            return PromotionProductGrouping;
        }

        public async Task<PromotionProductGrouping> Create(PromotionProductGrouping PromotionProductGrouping)
        {
            if (!await PromotionProductGroupingValidator.Create(PromotionProductGrouping))
                return PromotionProductGrouping;

            try
            {

                await UOW.PromotionProductGroupingRepository.Create(PromotionProductGrouping);

                PromotionProductGrouping = await UOW.PromotionProductGroupingRepository.Get(PromotionProductGrouping.Id);
                Logging.CreateAuditLog(PromotionProductGrouping, new { }, nameof(PromotionProductGroupingService));
                return PromotionProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }

        public async Task<PromotionProductGrouping> Update(PromotionProductGrouping PromotionProductGrouping)
        {
            if (!await PromotionProductGroupingValidator.Update(PromotionProductGrouping))
                return PromotionProductGrouping;
            try
            {
                var oldData = await UOW.PromotionProductGroupingRepository.Get(PromotionProductGrouping.Id);


                await UOW.PromotionProductGroupingRepository.Update(PromotionProductGrouping);


                PromotionProductGrouping = await UOW.PromotionProductGroupingRepository.Get(PromotionProductGrouping.Id);
                Logging.CreateAuditLog(PromotionProductGrouping, oldData, nameof(PromotionProductGroupingService));
                return PromotionProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }

        public async Task<PromotionProductGrouping> Delete(PromotionProductGrouping PromotionProductGrouping)
        {
            if (!await PromotionProductGroupingValidator.Delete(PromotionProductGrouping))
                return PromotionProductGrouping;

            try
            {

                await UOW.PromotionProductGroupingRepository.Delete(PromotionProductGrouping);

                Logging.CreateAuditLog(new { }, PromotionProductGrouping, nameof(PromotionProductGroupingService));
                return PromotionProductGrouping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }

        public async Task<List<PromotionProductGrouping>> BulkDelete(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            if (!await PromotionProductGroupingValidator.BulkDelete(PromotionProductGroupings))
                return PromotionProductGroupings;

            try
            {

                await UOW.PromotionProductGroupingRepository.BulkDelete(PromotionProductGroupings);

                Logging.CreateAuditLog(new { }, PromotionProductGroupings, nameof(PromotionProductGroupingService));
                return PromotionProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }

        public async Task<List<PromotionProductGrouping>> Import(List<PromotionProductGrouping> PromotionProductGroupings)
        {
            if (!await PromotionProductGroupingValidator.Import(PromotionProductGroupings))
                return PromotionProductGroupings;
            try
            {

                await UOW.PromotionProductGroupingRepository.BulkMerge(PromotionProductGroupings);


                Logging.CreateAuditLog(PromotionProductGroupings, new { }, nameof(PromotionProductGroupingService));
                return PromotionProductGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductGroupingService));
            }
            return null;
        }

        public async Task<PromotionProductGroupingFilter> ToFilter(PromotionProductGroupingFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionProductGroupingFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionProductGroupingFilter subFilter = new PromotionProductGroupingFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionPolicyId))
                        subFilter.PromotionPolicyId = FilterBuilder.Merge(subFilter.PromotionPolicyId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionId))
                        subFilter.PromotionId = FilterBuilder.Merge(subFilter.PromotionId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.ProductGroupingId))
                        subFilter.ProductGroupingId = FilterBuilder.Merge(subFilter.ProductGroupingId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Note))






                        subFilter.Note = FilterBuilder.Merge(subFilter.Note, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.FromValue))


                        subFilter.FromValue = FilterBuilder.Merge(subFilter.FromValue, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.ToValue))


                        subFilter.ToValue = FilterBuilder.Merge(subFilter.ToValue, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionDiscountTypeId))
                        subFilter.PromotionDiscountTypeId = FilterBuilder.Merge(subFilter.PromotionDiscountTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.DiscountPercentage))


                        subFilter.DiscountPercentage = FilterBuilder.Merge(subFilter.DiscountPercentage, FilterPermissionDefinition.DecimalFilter);





                    if (FilterPermissionDefinition.Name == nameof(subFilter.DiscountValue))


                        subFilter.DiscountValue = FilterBuilder.Merge(subFilter.DiscountValue, FilterPermissionDefinition.DecimalFilter);





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
