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
using DMS.Handlers.Configuration;

namespace DMS.Services.MLuckyDrawStructure
{
    public interface ILuckyDrawStructureService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawStructureFilter LuckyDrawStructureFilter);
        Task<List<LuckyDrawStructure>> List(LuckyDrawStructureFilter LuckyDrawStructureFilter);
        Task<LuckyDrawStructure> Get(long Id);
        Task<LuckyDrawStructure> Create(LuckyDrawStructure LuckyDrawStructure);
        Task<LuckyDrawStructure> Update(LuckyDrawStructure LuckyDrawStructure);
        Task<LuckyDrawStructure> Delete(LuckyDrawStructure LuckyDrawStructure);
        Task<List<LuckyDrawStructure>> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures);
        Task<List<LuckyDrawStructure>> Import(List<LuckyDrawStructure> LuckyDrawStructures);
        Task<LuckyDrawStructureFilter> ToFilter(LuckyDrawStructureFilter LuckyDrawStructureFilter);
    }

    public class LuckyDrawStructureService : BaseService, ILuckyDrawStructureService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        
        private ILuckyDrawStructureValidator LuckyDrawStructureValidator;

        public LuckyDrawStructureService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            ILuckyDrawStructureValidator LuckyDrawStructureValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
           
            this.LuckyDrawStructureValidator = LuckyDrawStructureValidator;
        }
        public async Task<int> Count(LuckyDrawStructureFilter LuckyDrawStructureFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawStructureRepository.Count(LuckyDrawStructureFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return 0;
        }

        public async Task<List<LuckyDrawStructure>> List(LuckyDrawStructureFilter LuckyDrawStructureFilter)
        {
            try
            {
                List<LuckyDrawStructure> LuckyDrawStructures = await UOW.LuckyDrawStructureRepository.List(LuckyDrawStructureFilter);
                return LuckyDrawStructures;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;
        }

        public async Task<LuckyDrawStructure> Get(long Id)
        {
            LuckyDrawStructure LuckyDrawStructure = await UOW.LuckyDrawStructureRepository.Get(Id);
            await LuckyDrawStructureValidator.Get(LuckyDrawStructure);
            if (LuckyDrawStructure == null)
                return null;
            return LuckyDrawStructure;
        }
        
        public async Task<LuckyDrawStructure> Create(LuckyDrawStructure LuckyDrawStructure)
        {
            if (!await LuckyDrawStructureValidator.Create(LuckyDrawStructure))
                return LuckyDrawStructure;

            try
            {
                await UOW.LuckyDrawStructureRepository.Create(LuckyDrawStructure);
                LuckyDrawStructure = await UOW.LuckyDrawStructureRepository.Get(LuckyDrawStructure.Id);
                Logging.CreateAuditLog(LuckyDrawStructure, new { }, nameof(LuckyDrawStructureService));
                return LuckyDrawStructure;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;
        }

        public async Task<LuckyDrawStructure> Update(LuckyDrawStructure LuckyDrawStructure)
        {
            if (!await LuckyDrawStructureValidator.Update(LuckyDrawStructure))
                return LuckyDrawStructure;
            try
            {
                var oldData = await UOW.LuckyDrawStructureRepository.Get(LuckyDrawStructure.Id);

                await UOW.LuckyDrawStructureRepository.Update(LuckyDrawStructure);

                LuckyDrawStructure = await UOW.LuckyDrawStructureRepository.Get(LuckyDrawStructure.Id);
                Logging.CreateAuditLog(LuckyDrawStructure, oldData, nameof(LuckyDrawStructureService));
                return LuckyDrawStructure;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;
        }

        public async Task<LuckyDrawStructure> Delete(LuckyDrawStructure LuckyDrawStructure)
        {
            if (!await LuckyDrawStructureValidator.Delete(LuckyDrawStructure))
                return LuckyDrawStructure;

            try
            {
                await UOW.LuckyDrawStructureRepository.Delete(LuckyDrawStructure);
                Logging.CreateAuditLog(new { }, LuckyDrawStructure, nameof(LuckyDrawStructureService));
                return LuckyDrawStructure;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;
        }

        public async Task<List<LuckyDrawStructure>> BulkDelete(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            if (!await LuckyDrawStructureValidator.BulkDelete(LuckyDrawStructures))
                return LuckyDrawStructures;

            try
            {
                await UOW.LuckyDrawStructureRepository.BulkDelete(LuckyDrawStructures);
                Logging.CreateAuditLog(new { }, LuckyDrawStructures, nameof(LuckyDrawStructureService));
                return LuckyDrawStructures;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;

        }
        
        public async Task<List<LuckyDrawStructure>> Import(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            if (!await LuckyDrawStructureValidator.Import(LuckyDrawStructures))
                return LuckyDrawStructures;
            try
            {
                await UOW.LuckyDrawStructureRepository.BulkMerge(LuckyDrawStructures);

                Logging.CreateAuditLog(LuckyDrawStructures, new { }, nameof(LuckyDrawStructureService));
                return LuckyDrawStructures;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawStructureService));
            }
            return null;
        }     
        
        public async Task<LuckyDrawStructureFilter> ToFilter(LuckyDrawStructureFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawStructureFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawStructureFilter subFilter = new LuckyDrawStructureFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawId))
                        subFilter.LuckyDrawId = FilterBuilder.Merge(subFilter.LuckyDrawId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Value))
                        subFilter.Value = FilterBuilder.Merge(subFilter.Value, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Quantity))
                        subFilter.Quantity = FilterBuilder.Merge(subFilter.Quantity, FilterPermissionDefinition.LongFilter);
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
    }
}
