using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProductType
{
    public interface IProductTypeService : IServiceScoped
    {
        Task<int> Count(ProductTypeFilter ProductTypeFilter);
        Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter);
        Task<ProductType> Get(long Id);
    }

    public class ProductTypeService : BaseService, IProductTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IProductTypeValidator ProductTypeValidator;

        public ProductTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IProductTypeValidator ProductTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ProductTypeValidator = ProductTypeValidator;
        }
        public async Task<int> Count(ProductTypeFilter ProductTypeFilter)
        {
            try
            {
                int result = await UOW.ProductTypeRepository.Count(ProductTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductTypeService));
            }
            return 0;
        }

        public async Task<List<ProductType>> List(ProductTypeFilter ProductTypeFilter)
        {
            try
            {
                List<ProductType> ProductTypes = await UOW.ProductTypeRepository.List(ProductTypeFilter);
                return ProductTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductTypeService));
            }
            return null;
        }
        public async Task<ProductType> Get(long Id)
        {
            ProductType ProductType = await UOW.ProductTypeRepository.Get(Id);
            if (ProductType == null)
                return null;
            return ProductType;
        }
    }
}
