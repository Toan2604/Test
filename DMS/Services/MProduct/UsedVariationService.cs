using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IUsedVariationService : IServiceScoped
    {
        Task<int> Count(UsedVariationFilter UsedVariationFilter);
        Task<List<UsedVariation>> List(UsedVariationFilter UsedVariationFilter);
    }

    public class UsedVariationService : BaseService, IUsedVariationService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public UsedVariationService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(UsedVariationFilter UsedVariationFilter)
        {
            try
            {
                int result = await UOW.UsedVariationRepository.Count(UsedVariationFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(UsedVariationService));
                throw new MessageException(ex.InnerException);
            }
        }

        public async Task<List<UsedVariation>> List(UsedVariationFilter UsedVariationFilter)
        {
            try
            {
                List<UsedVariation> UsedVariations = await UOW.UsedVariationRepository.List(UsedVariationFilter);
                return UsedVariations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(UsedVariationService));
                throw new MessageException(ex.InnerException);
            }
        }
    }
}
