using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProduct
{
    public interface IPromotionProductService : IServiceScoped
    {
        Task<int> Count(PromotionProductFilter PromotionProductFilter);
        Task<List<PromotionProduct>> List(PromotionProductFilter PromotionProductFilter);
        Task<PromotionProduct> Get(long Id);
        Task<PromotionProduct> Create(PromotionProduct PromotionProduct);
        Task<PromotionProduct> Update(PromotionProduct PromotionProduct);
        Task<PromotionProduct> Delete(PromotionProduct PromotionProduct);
        Task<List<PromotionProduct>> BulkDelete(List<PromotionProduct> PromotionProducts);
        Task<List<PromotionProduct>> Import(List<PromotionProduct> PromotionProducts);
        Task<PromotionProductFilter> ToFilter(PromotionProductFilter PromotionProductFilter);
    }

    public class PromotionProductService : BaseService, IPromotionProductService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionProductValidator PromotionProductValidator;

        public PromotionProductService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionProductValidator PromotionProductValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionProductValidator = PromotionProductValidator;
        }
        public async Task<int> Count(PromotionProductFilter PromotionProductFilter)
        {
            try
            {
                int result = await UOW.PromotionProductRepository.Count(PromotionProductFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return 0;
        }

        public async Task<List<PromotionProduct>> List(PromotionProductFilter PromotionProductFilter)
        {
            try
            {
                List<PromotionProduct> PromotionProducts = await UOW.PromotionProductRepository.List(PromotionProductFilter);
                return PromotionProducts;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }
        public async Task<PromotionProduct> Get(long Id)
        {
            PromotionProduct PromotionProduct = await UOW.PromotionProductRepository.Get(Id);
            if (PromotionProduct == null)
                return null;
            return PromotionProduct;
        }

        public async Task<PromotionProduct> Create(PromotionProduct PromotionProduct)
        {
            if (!await PromotionProductValidator.Create(PromotionProduct))
                return PromotionProduct;

            try
            {

                await UOW.PromotionProductRepository.Create(PromotionProduct);

                PromotionProduct = await UOW.PromotionProductRepository.Get(PromotionProduct.Id);
                Logging.CreateAuditLog(PromotionProduct, new { }, nameof(PromotionProductService));
                return PromotionProduct;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }

        public async Task<PromotionProduct> Update(PromotionProduct PromotionProduct)
        {
            if (!await PromotionProductValidator.Update(PromotionProduct))
                return PromotionProduct;
            try
            {
                var oldData = await UOW.PromotionProductRepository.Get(PromotionProduct.Id);


                await UOW.PromotionProductRepository.Update(PromotionProduct);


                PromotionProduct = await UOW.PromotionProductRepository.Get(PromotionProduct.Id);
                Logging.CreateAuditLog(PromotionProduct, oldData, nameof(PromotionProductService));
                return PromotionProduct;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }

        public async Task<PromotionProduct> Delete(PromotionProduct PromotionProduct)
        {
            if (!await PromotionProductValidator.Delete(PromotionProduct))
                return PromotionProduct;

            try
            {

                await UOW.PromotionProductRepository.Delete(PromotionProduct);

                Logging.CreateAuditLog(new { }, PromotionProduct, nameof(PromotionProductService));
                return PromotionProduct;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }

        public async Task<List<PromotionProduct>> BulkDelete(List<PromotionProduct> PromotionProducts)
        {
            if (!await PromotionProductValidator.BulkDelete(PromotionProducts))
                return PromotionProducts;

            try
            {

                await UOW.PromotionProductRepository.BulkDelete(PromotionProducts);

                Logging.CreateAuditLog(new { }, PromotionProducts, nameof(PromotionProductService));
                return PromotionProducts;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }

        public async Task<List<PromotionProduct>> Import(List<PromotionProduct> PromotionProducts)
        {
            if (!await PromotionProductValidator.Import(PromotionProducts))
                return PromotionProducts;
            try
            {

                await UOW.PromotionProductRepository.BulkMerge(PromotionProducts);


                Logging.CreateAuditLog(PromotionProducts, new { }, nameof(PromotionProductService));
                return PromotionProducts;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductService));
            }
            return null;
        }

        public async Task<PromotionProductFilter> ToFilter(PromotionProductFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PromotionProductFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PromotionProductFilter subFilter = new PromotionProductFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionPolicyId))
                        subFilter.PromotionPolicyId = FilterBuilder.Merge(subFilter.PromotionPolicyId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.PromotionId))
                        subFilter.PromotionId = FilterBuilder.Merge(subFilter.PromotionId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.ProductId))
                        subFilter.ProductId = FilterBuilder.Merge(subFilter.ProductId, FilterPermissionDefinition.IdFilter); if (FilterPermissionDefinition.Name == nameof(subFilter.Note))






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
