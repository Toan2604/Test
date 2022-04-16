using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionCode
{
    public interface IPromotionCodeService : IServiceScoped
    {
        Task<int> Count(PromotionCodeFilter PromotionCodeFilter);
        Task<List<PromotionCode>> List(PromotionCodeFilter PromotionCodeFilter);
        Task<PromotionCode> Get(long Id);
        Task<PromotionCode> Create(PromotionCode PromotionCode);
        Task<PromotionCode> Update(PromotionCode PromotionCode);
        Task<PromotionCode> Delete(PromotionCode PromotionCode);
        Task<List<PromotionCode>> BulkDelete(List<PromotionCode> PromotionCodes);
        Task<List<PromotionCode>> Import(List<PromotionCode> PromotionCodes);
        Task<PromotionCodeFilter> ToFilter(PromotionCodeFilter PromotionCodeFilter);
    }

    public class PromotionCodeService : BaseService, IPromotionCodeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionCodeValidator PromotionCodeValidator;

        public PromotionCodeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionCodeValidator PromotionCodeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionCodeValidator = PromotionCodeValidator;
        }
        public async Task<int> Count(PromotionCodeFilter PromotionCodeFilter)
        {
            try
            {
                int result = await UOW.PromotionCodeRepository.Count(PromotionCodeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return 0;
        }

        public async Task<List<PromotionCode>> List(PromotionCodeFilter PromotionCodeFilter)
        {
            try
            {
                List<PromotionCode> PromotionCodes = await UOW.PromotionCodeRepository.List(PromotionCodeFilter);
                return PromotionCodes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }
        public async Task<PromotionCode> Get(long Id)
        {
            PromotionCode PromotionCode = await UOW.PromotionCodeRepository.Get(Id);
            if (PromotionCode == null)
                return null;
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter()
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DirectSalesOrderSelect.Id | DirectSalesOrderSelect.Code | DirectSalesOrderSelect.PromotionValue | DirectSalesOrderSelect.TotalAfterTax | DirectSalesOrderSelect.Total | DirectSalesOrderSelect.BuyerStore
            };
            var DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(DirectSalesOrderFilter);
            foreach (var PromotionCodeHistory in PromotionCode.PromotionCodeHistories)
            {
                PromotionCodeHistory.DirectSalesOrder = DirectSalesOrders.Where(x => x.RowId == PromotionCodeHistory.RowId).FirstOrDefault();
            }
            return PromotionCode;
        }

        public async Task<PromotionCode> Create(PromotionCode PromotionCode)
        {
            if (!await PromotionCodeValidator.Create(PromotionCode))
                return PromotionCode;

            try
            {

                PromotionCode.StartDate = PromotionCode.StartDate.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
                if (PromotionCode.EndDate.HasValue)
                {
                    PromotionCode.EndDate = PromotionCode.EndDate.Value.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone).AddDays(1).AddSeconds(-1);
                }
                await UOW.PromotionCodeRepository.Create(PromotionCode);

                PromotionCode = await UOW.PromotionCodeRepository.Get(PromotionCode.Id);
                Logging.CreateAuditLog(PromotionCode, new { }, nameof(PromotionCodeService));
                return PromotionCode;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }

        public async Task<PromotionCode> Update(PromotionCode PromotionCode)
        {
            if (!await PromotionCodeValidator.Update(PromotionCode))
                return PromotionCode;
            try
            {
                var oldData = await UOW.PromotionCodeRepository.Get(PromotionCode.Id);
                if (oldData.Used)
                {
                    PromotionCode.Code = oldData.Code;
                    PromotionCode.EndDate = oldData.EndDate;
                    PromotionCode.MaxValue = oldData.MaxValue;
                    PromotionCode.OrganizationId = oldData.OrganizationId;
                    PromotionCode.PromotionDiscountTypeId = oldData.PromotionDiscountTypeId;
                    PromotionCode.PromotionProductAppliedTypeId = oldData.PromotionProductAppliedTypeId;
                    PromotionCode.PromotionTypeId = oldData.PromotionTypeId;
                    PromotionCode.Quantity = oldData.Quantity;
                    PromotionCode.StartDate = oldData.StartDate;
                    PromotionCode.Value = oldData.Value;
                    PromotionCode.PromotionCodeOrganizationMappings = oldData.PromotionCodeOrganizationMappings;
                    PromotionCode.PromotionCodeProductMappings = oldData.PromotionCodeProductMappings;
                    PromotionCode.PromotionCodeStoreMappings = oldData.PromotionCodeStoreMappings;
                }


                await UOW.PromotionCodeRepository.Update(PromotionCode);


                PromotionCode = await UOW.PromotionCodeRepository.Get(PromotionCode.Id);
                Logging.CreateAuditLog(PromotionCode, oldData, nameof(PromotionCodeService));
                return PromotionCode;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }

        public async Task<PromotionCode> Delete(PromotionCode PromotionCode)
        {
            if (!await PromotionCodeValidator.Delete(PromotionCode))
                return PromotionCode;

            try
            {

                await UOW.PromotionCodeRepository.Delete(PromotionCode);

                Logging.CreateAuditLog(new { }, PromotionCode, nameof(PromotionCodeService));
                return PromotionCode;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }

        public async Task<List<PromotionCode>> BulkDelete(List<PromotionCode> PromotionCodes)
        {
            if (!await PromotionCodeValidator.BulkDelete(PromotionCodes))
                return PromotionCodes;

            try
            {

                await UOW.PromotionCodeRepository.BulkDelete(PromotionCodes);

                Logging.CreateAuditLog(new { }, PromotionCodes, nameof(PromotionCodeService));
                return PromotionCodes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }

        public async Task<List<PromotionCode>> Import(List<PromotionCode> PromotionCodes)
        {
            if (!await PromotionCodeValidator.Import(PromotionCodes))
                return PromotionCodes;
            try
            {

                await UOW.PromotionCodeRepository.BulkMerge(PromotionCodes);


                Logging.CreateAuditLog(PromotionCodes, new { }, nameof(PromotionCodeService));
                return PromotionCodes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionCodeService));
            }
            return null;
        }

        public async Task<PromotionCodeFilter> ToFilter(PromotionCodeFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionCodeFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionCodeFilter subFilter = new PromotionCodeFilter();
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
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Quantity))
                        subFilter.Quantity = FilterBuilder.Merge(subFilter.Quantity, FilterPermissionDefinition.LongFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionDiscountTypeId))
                        subFilter.PromotionDiscountTypeId = FilterBuilder.Merge(subFilter.PromotionDiscountTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Value))
                        subFilter.Value = FilterBuilder.Merge(subFilter.Value, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.MaxValue))
                        subFilter.MaxValue = FilterBuilder.Merge(subFilter.MaxValue, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionTypeId))
                        subFilter.PromotionTypeId = FilterBuilder.Merge(subFilter.PromotionTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionProductAppliedTypeId))
                        subFilter.PromotionProductAppliedTypeId = FilterBuilder.Merge(subFilter.PromotionProductAppliedTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StartDate))
                        subFilter.StartDate = FilterBuilder.Merge(subFilter.StartDate, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.EndDate))
                        subFilter.EndDate = FilterBuilder.Merge(subFilter.EndDate, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
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
