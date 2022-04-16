using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSurvey
{
    public interface ISurveyOptionTypeService : IServiceScoped
    {
        Task<int> Count(SurveyOptionTypeFilter SurveyOptionTypeFilter);
        Task<List<SurveyOptionType>> List(SurveyOptionTypeFilter SurveyOptionTypeFilter);
    }

    public class SurveyOptionTypeService : BaseService, ISurveyOptionTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public SurveyOptionTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(SurveyOptionTypeFilter SurveyOptionTypeFilter)
        {
            try
            {
                int result = await UOW.SurveyOptionTypeRepository.Count(SurveyOptionTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyOptionTypeService));
            }
            return 0;
        }

        public async Task<List<SurveyOptionType>> List(SurveyOptionTypeFilter SurveyOptionTypeFilter)
        {
            try
            {
                List<SurveyOptionType> SurveyOptionTypes = await UOW.SurveyOptionTypeRepository.List(SurveyOptionTypeFilter);
                return SurveyOptionTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(SurveyOptionTypeService));
            }
            return null;
        }
    }
}
