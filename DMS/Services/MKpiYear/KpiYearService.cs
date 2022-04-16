using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiYear
{
    public interface IKpiYearService : IServiceScoped
    {
        Task<int> Count(KpiYearFilter KpiYearFilter);
        Task<List<KpiYear>> List(KpiYearFilter KpiYearFilter);
    }

    public class KpiYearService : BaseService, IKpiYearService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IKpiYearValidator KpiYearValidator;

        public KpiYearService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IKpiYearValidator KpiYearValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.KpiYearValidator = KpiYearValidator;
        }
        public async Task<int> Count(KpiYearFilter KpiYearFilter)
        {
            try
            {
                int result = await UOW.KpiYearRepository.Count(KpiYearFilter);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Logging.CreateSystemLog(ex, nameof(KpiYearService));
                    throw new MessageException(ex);
                }
                else
                {
                    Logging.CreateSystemLog(ex.InnerException, nameof(KpiYearService));
                }
                return 0;
            }
        }

        public async Task<List<KpiYear>> List(KpiYearFilter KpiYearFilter)
        {
            try
            {
                List<KpiYear> KpiYears = await UOW.KpiYearRepository.List(KpiYearFilter);
                return KpiYears;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Logging.CreateSystemLog(ex, nameof(KpiYearService));
                    throw new MessageException(ex);
                }
                else
                {
                    Logging.CreateSystemLog(ex.InnerException, nameof(KpiYearService));
                }
                return null;
            }
        }
    }
}
