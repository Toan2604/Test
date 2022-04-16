using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionDiscountType
{
    public interface IPromotionDiscountTypeService : IServiceScoped
    {
        Task<int> Count(PromotionDiscountTypeFilter PromotionDiscountTypeFilter);
        Task<List<PromotionDiscountType>> List(PromotionDiscountTypeFilter PromotionDiscountTypeFilter);
    }

    public class PromotionDiscountTypeService : BaseService, IPromotionDiscountTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionDiscountTypeValidator PromotionDiscountTypeValidator;

        public PromotionDiscountTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionDiscountTypeValidator PromotionDiscountTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionDiscountTypeValidator = PromotionDiscountTypeValidator;
        }
        public async Task<int> Count(PromotionDiscountTypeFilter PromotionDiscountTypeFilter)
        {
            try
            {
                int result = await UOW.PromotionDiscountTypeRepository.Count(PromotionDiscountTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionDiscountTypeService));
            }
            return 0;
        }

        public async Task<List<PromotionDiscountType>> List(PromotionDiscountTypeFilter PromotionDiscountTypeFilter)
        {
            try
            {
                List<PromotionDiscountType> PromotionDiscountTypes = await UOW.PromotionDiscountTypeRepository.List(PromotionDiscountTypeFilter);
                return PromotionDiscountTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionDiscountTypeService));
            }
            return null;
        }
    }
}
