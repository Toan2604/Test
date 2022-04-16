using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPromotionProductAppliedType
{
    public interface IPromotionProductAppliedTypeService : IServiceScoped
    {
        Task<int> Count(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter);
        Task<List<PromotionProductAppliedType>> List(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter);
    }

    public class PromotionProductAppliedTypeService : BaseService, IPromotionProductAppliedTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPromotionProductAppliedTypeValidator PromotionProductAppliedTypeValidator;

        public PromotionProductAppliedTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPromotionProductAppliedTypeValidator PromotionProductAppliedTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PromotionProductAppliedTypeValidator = PromotionProductAppliedTypeValidator;
        }
        public async Task<int> Count(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter)
        {
            try
            {
                int result = await UOW.PromotionProductAppliedTypeRepository.Count(PromotionProductAppliedTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductAppliedTypeService));
            }
            return 0;
        }

        public async Task<List<PromotionProductAppliedType>> List(PromotionProductAppliedTypeFilter PromotionProductAppliedTypeFilter)
        {
            try
            {
                List<PromotionProductAppliedType> PromotionProductAppliedTypes = await UOW.PromotionProductAppliedTypeRepository.List(PromotionProductAppliedTypeFilter);
                return PromotionProductAppliedTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PromotionProductAppliedTypeService));
            }
            return null;
        }
    }
}
