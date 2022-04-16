using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MEstimatedRevenue
{
    public interface IEstimatedRevenueService : IServiceScoped
    {
        Task<int> Count(EstimatedRevenueFilter EstimatedRevenueFilter);
        Task<List<EstimatedRevenue>> List(EstimatedRevenueFilter EstimatedRevenueFilter);
    }

    public class EstimatedRevenueService : BaseService, IEstimatedRevenueService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IEstimatedRevenueValidator EstimatedRevenueValidator;

        public EstimatedRevenueService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IEstimatedRevenueValidator EstimatedRevenueValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.EstimatedRevenueValidator = EstimatedRevenueValidator;
        }
        public async Task<int> Count(EstimatedRevenueFilter EstimatedRevenueFilter)
        {
            try
            {
                int result = await UOW.EstimatedRevenueRepository.Count(EstimatedRevenueFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(EstimatedRevenueService));
            }
            return 0;
        }

        public async Task<List<EstimatedRevenue>> List(EstimatedRevenueFilter EstimatedRevenueFilter)
        {
            try
            {
                List<EstimatedRevenue> EstimatedRevenues = await UOW.EstimatedRevenueRepository.List(EstimatedRevenueFilter);
                return EstimatedRevenues;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EstimatedRevenueService));
            }
            return null;
        }
    }
}
