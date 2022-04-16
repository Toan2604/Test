using TrueSight.Common;
using DMS.Handlers.Configuration;
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

namespace DMS.Services.MLuckyDrawNumber
{
    public interface ILuckyDrawNumberService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawNumberFilter LuckyDrawNumberFilter);
        Task<List<LuckyDrawNumber>> List(LuckyDrawNumberFilter LuckyDrawNumberFilter);
        Task<LuckyDrawNumber> Get(long Id);
        Task<List<LuckyDrawNumber>> Export(List<long> Ids);
    }

    public class LuckyDrawNumberService : BaseService, ILuckyDrawNumberService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        

        public LuckyDrawNumberService(
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

        public async Task<int> Count(LuckyDrawNumberFilter LuckyDrawNumberFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawNumberRepository.Count(LuckyDrawNumberFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawNumberService));
            }
            return 0;
        }

        public async Task<List<LuckyDrawNumber>> List(LuckyDrawNumberFilter LuckyDrawNumberFilter)
        {
            try
            {
                List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(LuckyDrawNumberFilter);
                return LuckyDrawNumbers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawNumberService));
            }
            return null;
        }

        public async Task<LuckyDrawNumber> Get(long Id)
        {
            LuckyDrawNumber LuckyDrawNumber = await UOW.LuckyDrawNumberRepository.Get(Id);
            if (LuckyDrawNumber == null)
                return null;
            return LuckyDrawNumber;
        }
        
        public async Task<LuckyDrawNumberFilter> ToFilter(LuckyDrawNumberFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawNumberFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawNumberFilter subFilter = new LuckyDrawNumberFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawStructureId))
                        subFilter.LuckyDrawStructureId = FilterBuilder.Merge(subFilter.LuckyDrawStructureId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
        public async Task<List<LuckyDrawNumber>> Export(List<long> Ids)
        {
            try
            {
                List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(Ids);
                return LuckyDrawNumbers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawNumberService));
            }
            return null;
        }

    }
}
