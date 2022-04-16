using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MNation
{
    public interface INationService : IServiceScoped
    {
        Task<int> Count(NationFilter NationFilter);
        Task<List<Nation>> List(NationFilter NationFilter);
        Task<Nation> Get(long Id);
    }

    public class NationService : BaseService, INationService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private INationValidator NationValidator;

        public NationService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            INationValidator NationValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.NationValidator = NationValidator;
        }
        public async Task<int> Count(NationFilter NationFilter)
        {
            try
            {
                int result = await UOW.NationRepository.Count(NationFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NationService));
            }
            return 0;
        }

        public async Task<List<Nation>> List(NationFilter NationFilter)
        {
            try
            {
                List<Nation> Nations = await UOW.NationRepository.List(NationFilter);
                return Nations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(NationService));
            }
            return null;
        }
        public async Task<Nation> Get(long Id)
        {
            Nation Nation = await UOW.NationRepository.Get(Id);
            if (Nation == null)
                return null;
            return Nation;
        }

    }
}
