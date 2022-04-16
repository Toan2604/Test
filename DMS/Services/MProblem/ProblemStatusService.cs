using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProblem
{
    public interface IProblemStatusService : IServiceScoped
    {
        Task<int> Count(ProblemStatusFilter ProblemStatusFilter);
        Task<List<ProblemStatus>> List(ProblemStatusFilter ProblemStatusFilter);
    }

    public class ProblemStatusService : BaseService, IProblemStatusService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public ProblemStatusService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(ProblemStatusFilter ProblemStatusFilter)
        {
            try
            {
                int result = await UOW.ProblemStatusRepository.Count(ProblemStatusFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemStatusService));
            }
            return 0;
        }

        public async Task<List<ProblemStatus>> List(ProblemStatusFilter ProblemStatusFilter)
        {
            try
            {
                List<ProblemStatus> ProblemStatuss = await UOW.ProblemStatusRepository.List(ProblemStatusFilter);
                return ProblemStatuss;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProblemStatusService));
            }
            return null;
        }
    }
}
