using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MField
{
    public interface IFieldService : IServiceScoped
    {
        Task<int> Count(FieldFilter FieldFilter);
        Task<List<Field>> List(FieldFilter FieldFilter);
    }

    public class FieldService : BaseService, IFieldService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        public FieldService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(FieldFilter FieldFilter)
        {
            try
            {
                int result = await UOW.FieldRepository.Count(FieldFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(FieldService));
            }
            return 0;
        }

        public async Task<List<Field>> List(FieldFilter FieldFilter)
        {
            try
            {
                List<Field> Fields = await UOW.FieldRepository.List(FieldFilter);
                return Fields;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(FieldService));
            }
            return null;
        }
    }
}
