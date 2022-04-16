using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotion
{
    public interface IPromotionService : IServiceScoped
    {
        Task<int> Count(PromotionFilter PromotionFilter);
        Task<List<Promotion>> List(PromotionFilter PromotionFilter);
        Task<Promotion> Get(long Id);
        Task<Promotion> Create(Promotion Promotion);
        Task<Promotion> CreateDraft();
        Task<Promotion> Update(Promotion Promotion);
        Task<PromotionPromotionPolicyMapping> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<PromotionPromotionPolicyMapping> UpdateStore(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<PromotionPromotionPolicyMapping> UpdateStoreGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        //Task<PromotionPromotionPolicyMapping> UpdateStoreType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<PromotionPromotionPolicyMapping> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<PromotionPromotionPolicyMapping> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<PromotionPromotionPolicyMapping> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<PromotionPromotionPolicyMapping> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<PromotionPromotionPolicyMapping> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping);
        Task<Promotion> Delete(Promotion Promotion);
        Task<List<Promotion>> BulkDelete(List<Promotion> Promotions);
        Task<List<Promotion>> Import(List<Promotion> Promotions);
        Task<PromotionFilter> ToFilter(PromotionFilter PromotionFilter);
    }

    public class PromotionService : BaseService, IPromotionService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionValidator PromotionValidator;

        public PromotionService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionValidator PromotionValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionValidator = PromotionValidator;
        }
        public async Task<int> Count(PromotionFilter PromotionFilter)
        {
            try
            {
                int result = await UOW.PromotionRepository.Count(PromotionFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return 0;
        }

        public async Task<List<Promotion>> List(PromotionFilter PromotionFilter)
        {
            try
            {
                List<Promotion> Promotions = await UOW.PromotionRepository.List(PromotionFilter);
                return Promotions;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }
        public async Task<Promotion> Get(long Id)
        {
            Promotion Promotion = await UOW.PromotionRepository.Get(Id);
            if (Promotion == null)
                return null;
            return Promotion;
        }

        public async Task<Promotion> CreateDraft()
        {
            Promotion Promotion = new Promotion();
            Promotion.StartDate = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone);
            Promotion.StatusId = StatusEnum.INACTIVE.Id;
            Promotion.PromotionPromotionPolicyMappings = new List<PromotionPromotionPolicyMapping>();
            foreach (var PromotionPolicy in PromotionPolicyEnum.PromotionPolicyEnumList)
            {
                PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping = new PromotionPromotionPolicyMapping();
                PromotionPromotionPolicyMapping.PromotionPolicyId = PromotionPolicy.Id;
                PromotionPromotionPolicyMapping.StatusId = StatusEnum.INACTIVE.Id;
                PromotionPromotionPolicyMapping.PromotionPolicy = new PromotionPolicy
                {
                    Id = PromotionPolicy.Id,
                    Name = PromotionPolicy.Name,
                    Code = PromotionPolicy.Code,
                };
                Promotion.PromotionPromotionPolicyMappings.Add(PromotionPromotionPolicyMapping);
            }
            return Promotion;
        }

        public async Task<Promotion> Create(Promotion Promotion)
        {
            if (!await PromotionValidator.Create(Promotion))
                return Promotion;

            try
            {

                await UOW.PromotionRepository.Create(Promotion);

                Promotion = await UOW.PromotionRepository.Get(Promotion.Id);
                Logging.CreateAuditLog(Promotion, new { }, nameof(PromotionService));
                return Promotion;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<Promotion> Update(Promotion Promotion)
        {
            if (!await PromotionValidator.Update(Promotion))
                return Promotion;
            try
            {
                var oldData = await UOW.PromotionRepository.Get(Promotion.Id);


                await UOW.PromotionRepository.Update(Promotion);


                Promotion = await UOW.PromotionRepository.Get(Promotion.Id);
                Logging.CreateAuditLog(Promotion, oldData, nameof(PromotionService));
                return Promotion;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> UpdateDirectSalesOrder(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateDirectSalesOrder(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateDirectSalesOrder(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        //public async Task<PromotionPromotionPolicyMapping> UpdateStore(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        //{
        //    if (!await PromotionValidator.Update(Promotion))
        //        return Promotion;
        //    try
        //    {
        //        var oldData = await UOW.PromotionRepository.Get(Promotion.Id);

        //        
        //        await UOW.PromotionRepository.Update(Promotion);
        //        

        //        Promotion = await UOW.PromotionRepository.Get(Promotion.Id);
        //        Logging.CreateAuditLog(Promotion, oldData, nameof(PromotionService));
        //        return Promotion;
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //        if (ex.InnerException == null)
        //        {
        //           Logging.CreateSystemLog(ex, nameof(PromotionService));
        //            throw new MessageException(ex);
        //        }
        //        else
        //        {
        //           Logging.CreateSystemLog(ex.InnerException, nameof(PromotionService));
        //            throw new MessageException(ex.InnerException);
        //        }
        //    }
        //}

        //public async Task<PromotionPromotionPolicyMapping> UpdateStoreGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        //{
        //    if (!await PromotionValidator.Update(Promotion))
        //        return Promotion;
        //    try
        //    {
        //        var oldData = await UOW.PromotionRepository.Get(Promotion.Id);

        //        
        //        await UOW.PromotionRepository.Update(Promotion);
        //        

        //        Promotion = await UOW.PromotionRepository.Get(Promotion.Id);
        //        Logging.CreateAuditLog(Promotion, oldData, nameof(PromotionService));
        //        return Promotion;
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //        if (ex.InnerException == null)
        //        {
        //           Logging.CreateSystemLog(ex, nameof(PromotionService));
        //            throw new MessageException(ex);
        //        }
        //        else
        //        {
        //           Logging.CreateSystemLog(ex.InnerException, nameof(PromotionService));
        //            throw new MessageException(ex.InnerException);
        //        }
        //    }
        //}

        //public async Task<PromotionPromotionPolicyMapping> UpdateStoreType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        //{
        //    if (!await PromotionValidator.Update(Promotion))
        //        return Promotion;
        //    try
        //    {
        //        var oldData = await UOW.PromotionRepository.Get(Promotion.Id);

        //        
        //        await UOW.PromotionRepository.Update(Promotion);
        //        

        //        Promotion = await UOW.PromotionRepository.Get(Promotion.Id);
        //        Logging.CreateAuditLog(Promotion, oldData, nameof(PromotionService));
        //        return Promotion;
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //        if (ex.InnerException == null)
        //        {
        //           Logging.CreateSystemLog(ex, nameof(PromotionService));
        //            throw new MessageException(ex);
        //        }
        //        else
        //        {
        //           Logging.CreateSystemLog(ex.InnerException, nameof(PromotionService));
        //            throw new MessageException(ex.InnerException);
        //        }
        //    }
        //}

        public async Task<PromotionPromotionPolicyMapping> UpdateProduct(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateProduct(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateProduct(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> UpdateProductGrouping(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateProductGrouping(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateProductGrouping(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> UpdateProductType(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateProductType(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateProductType(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> UpdateCombo(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateCombo(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateCombo(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionPromotionPolicyMapping> UpdateSamePrice(PromotionPromotionPolicyMapping PromotionPromotionPolicyMapping)
        {
            if (!await PromotionValidator.UpdateSamePrice(PromotionPromotionPolicyMapping))
                return PromotionPromotionPolicyMapping;
            try
            {
                var oldData = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);


                await UOW.PromotionRepository.UpdateSamePrice(PromotionPromotionPolicyMapping);


                PromotionPromotionPolicyMapping = await UOW.PromotionPolicyRepository.GetMapping(PromotionPromotionPolicyMapping.PromotionPolicyId, PromotionPromotionPolicyMapping.PromotionId);
                Logging.CreateAuditLog(PromotionPromotionPolicyMapping, oldData, nameof(PromotionService));
                return PromotionPromotionPolicyMapping;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<Promotion> Delete(Promotion Promotion)
        {
            if (!await PromotionValidator.Delete(Promotion))
                return Promotion;

            try
            {

                await UOW.PromotionRepository.Delete(Promotion);

                Logging.CreateAuditLog(new { }, Promotion, nameof(PromotionService));
                return Promotion;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<List<Promotion>> BulkDelete(List<Promotion> Promotions)
        {
            if (!await PromotionValidator.BulkDelete(Promotions))
                return Promotions;

            try
            {

                await UOW.PromotionRepository.BulkDelete(Promotions);

                Logging.CreateAuditLog(new { }, Promotions, nameof(PromotionService));
                return Promotions;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<List<Promotion>> Import(List<Promotion> Promotions)
        {
            if (!await PromotionValidator.Import(Promotions))
                return Promotions;
            try
            {

                await UOW.PromotionRepository.BulkMerge(Promotions);


                Logging.CreateAuditLog(Promotions, new { }, nameof(PromotionService));
                return Promotions;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionService));
            }
            return null;
        }

        public async Task<PromotionFilter> ToFilter(PromotionFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionFilter subFilter = new PromotionFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Code))






                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))






                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.StartDate))





                        subFilter.StartDate = FilterBuilder.Merge(subFilter.StartDate, FilterPermissionDefinition.DateFilter);


                    if (FilterPermissionDefinition.Name == nameof(subFilter.EndDate))





                        subFilter.EndDate = FilterBuilder.Merge(subFilter.EndDate, FilterPermissionDefinition.DateFilter);


                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionTypeId))
                        subFilter.PromotionTypeId = FilterBuilder.Merge(subFilter.PromotionTypeId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Note))






                        subFilter.Note = FilterBuilder.Merge(subFilter.Note, FilterPermissionDefinition.StringFilter);

                    if (FilterPermissionDefinition.Name == nameof(subFilter.Priority))

                        subFilter.Priority = FilterBuilder.Merge(subFilter.Priority, FilterPermissionDefinition.LongFilter);






                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
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
