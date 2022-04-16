using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MTaxType
{
    public interface ITaxTypeService : IServiceScoped
    {
        Task<int> Count(TaxTypeFilter TaxTypeFilter);
        Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter);
        Task<TaxType> Get(long Id);
    }

    public class TaxTypeService : BaseService, ITaxTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private ITaxTypeValidator TaxTypeValidator;

        public TaxTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            ITaxTypeValidator TaxTypeValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.TaxTypeValidator = TaxTypeValidator;
        }
        public async Task<int> Count(TaxTypeFilter TaxTypeFilter)
        {
            try
            {
                int result = await UOW.TaxTypeRepository.Count(TaxTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaxTypeService));
            }
            return 0;
        }

        public async Task<List<TaxType>> List(TaxTypeFilter TaxTypeFilter)
        {
            try
            {
                List<TaxType> TaxTypes = await UOW.TaxTypeRepository.List(TaxTypeFilter);
                return TaxTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(TaxTypeService));
            }
            return null;
        }
        public async Task<TaxType> Get(long Id)
        {
            TaxType TaxType = await UOW.TaxTypeRepository.Get(Id);
            if (TaxType == null)
                return null;
            return TaxType;
        }
    }
}
