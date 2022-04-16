using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductType
{
    public interface IPromotionProductTypeService : IServiceScoped
    {
        Task<int> Count(PromotionProductTypeFilter PromotionProductTypeFilter);
        Task<List<PromotionProductType>> List(PromotionProductTypeFilter PromotionProductTypeFilter);
        Task<PromotionProductType> Get(long Id);
        Task<PromotionProductType> Create(PromotionProductType PromotionProductType);
        Task<PromotionProductType> Update(PromotionProductType PromotionProductType);
        Task<PromotionProductType> Delete(PromotionProductType PromotionProductType);
        Task<List<PromotionProductType>> BulkDelete(List<PromotionProductType> PromotionProductTypes);
        Task<List<PromotionProductType>> Import(List<PromotionProductType> PromotionProductTypes);
        Task<PromotionProductTypeFilter> ToFilter(PromotionProductTypeFilter PromotionProductTypeFilter);
    }

    public class PromotionProductTypeService : BaseService, IPromotionProductTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionProductTypeValidator PromotionProductTypeValidator;

        public PromotionProductTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionProductTypeValidator PromotionProductTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionProductTypeValidator = PromotionProductTypeValidator;
        }
        public async Task<int> Count(PromotionProductTypeFilter PromotionProductTypeFilter)
        {
            try
            {
                int result = await UOW.PromotionProductTypeRepository.Count(PromotionProductTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return 0;
        }

        public async Task<List<PromotionProductType>> List(PromotionProductTypeFilter PromotionProductTypeFilter)
        {
            try
            {
                List<PromotionProductType> PromotionProductTypes = await UOW.PromotionProductTypeRepository.List(PromotionProductTypeFilter);
                return PromotionProductTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }
        public async Task<PromotionProductType> Get(long Id)
        {
            PromotionProductType PromotionProductType = await UOW.PromotionProductTypeRepository.Get(Id);
            if (PromotionProductType == null)
                return null;
            return PromotionProductType;
        }

        public async Task<PromotionProductType> Create(PromotionProductType PromotionProductType)
        {
            if (!await PromotionProductTypeValidator.Create(PromotionProductType))
                return PromotionProductType;

            try
            {

                await UOW.PromotionProductTypeRepository.Create(PromotionProductType);

                PromotionProductType = await UOW.PromotionProductTypeRepository.Get(PromotionProductType.Id);
                Logging.CreateAuditLog(PromotionProductType, new { }, nameof(PromotionProductTypeService));
                return PromotionProductType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }

        public async Task<PromotionProductType> Update(PromotionProductType PromotionProductType)
        {
            if (!await PromotionProductTypeValidator.Update(PromotionProductType))
                return PromotionProductType;
            try
            {
                var oldData = await UOW.PromotionProductTypeRepository.Get(PromotionProductType.Id);


                await UOW.PromotionProductTypeRepository.Update(PromotionProductType);


                PromotionProductType = await UOW.PromotionProductTypeRepository.Get(PromotionProductType.Id);
                Logging.CreateAuditLog(PromotionProductType, oldData, nameof(PromotionProductTypeService));
                return PromotionProductType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }

        public async Task<PromotionProductType> Delete(PromotionProductType PromotionProductType)
        {
            if (!await PromotionProductTypeValidator.Delete(PromotionProductType))
                return PromotionProductType;

            try
            {

                await UOW.PromotionProductTypeRepository.Delete(PromotionProductType);

                Logging.CreateAuditLog(new { }, PromotionProductType, nameof(PromotionProductTypeService));
                return PromotionProductType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }

        public async Task<List<PromotionProductType>> BulkDelete(List<PromotionProductType> PromotionProductTypes)
        {
            if (!await PromotionProductTypeValidator.BulkDelete(PromotionProductTypes))
                return PromotionProductTypes;

            try
            {

                await UOW.PromotionProductTypeRepository.BulkDelete(PromotionProductTypes);

                Logging.CreateAuditLog(new { }, PromotionProductTypes, nameof(PromotionProductTypeService));
                return PromotionProductTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }

        public async Task<List<PromotionProductType>> Import(List<PromotionProductType> PromotionProductTypes)
        {
            if (!await PromotionProductTypeValidator.Import(PromotionProductTypes))
                return PromotionProductTypes;
            try
            {

                await UOW.PromotionProductTypeRepository.BulkMerge(PromotionProductTypes);


                Logging.CreateAuditLog(PromotionProductTypes, new { }, nameof(PromotionProductTypeService));
                return PromotionProductTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductTypeService));
            }
            return null;
        }

        public async Task<PromotionProductTypeFilter> ToFilter(PromotionProductTypeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionProductTypeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionProductTypeFilter subFilter = new PromotionProductTypeFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionPolicyId))
                        subFilter.PromotionPolicyId = FilterBuilder.Merge(subFilter.PromotionPolicyId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionId))
                        subFilter.PromotionId = FilterBuilder.Merge(subFilter.PromotionId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.ProductTypeId))
                        subFilter.ProductTypeId = FilterBuilder.Merge(subFilter.ProductTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Note))






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
