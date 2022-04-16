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
    public interface IVariationGroupingService : IServiceScoped
    {
        Task<int> Count(VariationGroupingFilter VariationGroupingFilter);
        Task<List<VariationGrouping>> List(VariationGroupingFilter VariationGroupingFilter);
        Task<VariationGrouping> Get(long Id);
    }

    public class VariationGroupingService : BaseService, IVariationGroupingService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IVariationGroupingValidator VariationGroupingValidator;

        public VariationGroupingService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IVariationGroupingValidator VariationGroupingValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.VariationGroupingValidator = VariationGroupingValidator;
        }
        public async Task<int> Count(VariationGroupingFilter VariationGroupingFilter)
        {
            try
            {
                int result = await UOW.VariationGroupingRepository.Count(VariationGroupingFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(VariationGroupingService));
                throw new MessageException(ex.InnerException);
            }
        }

        public async Task<List<VariationGrouping>> List(VariationGroupingFilter VariationGroupingFilter)
        {
            try
            {
                List<VariationGrouping> VariationGroupings = await UOW.VariationGroupingRepository.List(VariationGroupingFilter);
                return VariationGroupings;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(VariationGroupingService));
                throw new MessageException(ex.InnerException);
            }
        }
        public async Task<VariationGrouping> Get(long Id)
        {
            VariationGrouping VariationGrouping = await UOW.VariationGroupingRepository.Get(Id);
            if (VariationGrouping == null)
                return null;
            return VariationGrouping;
        }
    }
}
