using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.Repositories;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Handlers.Configuration;

namespace DMS.Services.MLuckyDrawType
{
    public interface ILuckyDrawTypeService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawTypeFilter LuckyDrawTypeFilter);
        Task<List<LuckyDrawType>> List(LuckyDrawTypeFilter LuckyDrawTypeFilter);
    }

    public class LuckyDrawTypeService : BaseService, ILuckyDrawTypeService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        

        public LuckyDrawTypeService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
           
        }
        public async Task<int> Count(LuckyDrawTypeFilter LuckyDrawTypeFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawTypeRepository.Count(LuckyDrawTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawTypeService));
            }
            return 0;
        }

        public async Task<List<LuckyDrawType>> List(LuckyDrawTypeFilter LuckyDrawTypeFilter)
        {
            try
            {
                List<LuckyDrawType> LuckyDrawTypes = await UOW.LuckyDrawTypeRepository.List(LuckyDrawTypeFilter);
                return LuckyDrawTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawTypeService));
            }
            return null;
        }

    }
}
