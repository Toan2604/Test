using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiCriteriaGeneral
{
    public interface IKpiCriteriaGeneralService : IServiceScoped
    {
        Task<int> Count(KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter);
        Task<List<KpiCriteriaGeneral>> List(KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter);
    }

    public class KpiCriteriaGeneralService : BaseService, IKpiCriteriaGeneralService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiCriteriaGeneralValidator KpiCriteriaGeneralValidator;

        public KpiCriteriaGeneralService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiCriteriaGeneralValidator KpiCriteriaGeneralValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiCriteriaGeneralValidator = KpiCriteriaGeneralValidator;
        }
        public async Task<int> Count(KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter)
        {
            try
            {
                int result = await UOW.KpiCriteriaGeneralRepository.Count(KpiCriteriaGeneralFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaGeneralService));
            }
            return 0;
        }

        public async Task<List<KpiCriteriaGeneral>> List(KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter)
        {
            try
            {
                List<KpiCriteriaGeneral> KpiCriteriaGenerals = await UOW.KpiCriteriaGeneralRepository.List(KpiCriteriaGeneralFilter);
                return KpiCriteriaGenerals;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(KpiCriteriaGeneralService));
            }
            return null;
        }
    }
}
