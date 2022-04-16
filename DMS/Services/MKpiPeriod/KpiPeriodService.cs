using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiPeriod
{
    public interface IKpiPeriodService : IServiceScoped
    {
        Task<int> Count(KpiPeriodFilter KpiPeriodFilter);
        Task<List<KpiPeriod>> List(KpiPeriodFilter KpiPeriodFilter);
        Task<KpiPeriod> Get(long Id);
    }

    public class KpiPeriodService : BaseService, IKpiPeriodService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiPeriodValidator KpiPeriodValidator;

        public KpiPeriodService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiPeriodValidator KpiPeriodValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiPeriodValidator = KpiPeriodValidator;
        }
        public async Task<int> Count(KpiPeriodFilter KpiPeriodFilter)
        {
            try
            {
                int result = await UOW.KpiPeriodRepository.Count(KpiPeriodFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiPeriodService));
            }
            return 0;
        }

        public async Task<List<KpiPeriod>> List(KpiPeriodFilter KpiPeriodFilter)
        {
            try
            {
                List<KpiPeriod> KpiPeriods = await UOW.KpiPeriodRepository.List(KpiPeriodFilter);
                return KpiPeriods;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiPeriodService));
            }
            return null;
        }
        public async Task<KpiPeriod> Get(long Id)
        {
            KpiPeriod KpiPeriod = await UOW.KpiPeriodRepository.Get(Id);
            if (KpiPeriod == null)
                return null;
            return KpiPeriod;
        }
    }
}
