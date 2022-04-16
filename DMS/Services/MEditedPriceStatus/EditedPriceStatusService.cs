using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MEditedPriceStatus
{
    public interface IEditedPriceStatusService : IServiceScoped
    {
        Task<int> Count(EditedPriceStatusFilter EditedPriceStatusFilter);
        Task<List<EditedPriceStatus>> List(EditedPriceStatusFilter EditedPriceStatusFilter);
        Task<EditedPriceStatus> Get(long Id);
        EditedPriceStatusFilter ToFilter(EditedPriceStatusFilter EditedPriceStatusFilter);
    }

    public class EditedPriceStatusService : BaseService, IEditedPriceStatusService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IEditedPriceStatusValidator EditedPriceStatusValidator;

        public EditedPriceStatusService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IEditedPriceStatusValidator EditedPriceStatusValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.EditedPriceStatusValidator = EditedPriceStatusValidator;
        }
        public async Task<int> Count(EditedPriceStatusFilter EditedPriceStatusFilter)
        {
            try
            {
                int result = await UOW.EditedPriceStatusRepository.Count(EditedPriceStatusFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EditedPriceStatusService));
            }
            return 0;
        }

        public async Task<List<EditedPriceStatus>> List(EditedPriceStatusFilter EditedPriceStatusFilter)
        {
            try
            {
                List<EditedPriceStatus> EditedPriceStatuss = await UOW.EditedPriceStatusRepository.List(EditedPriceStatusFilter);
                return EditedPriceStatuss;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(EditedPriceStatusService));
            }
            return null;
        }
        public async Task<EditedPriceStatus> Get(long Id)
        {
            EditedPriceStatus EditedPriceStatus = await UOW.EditedPriceStatusRepository.Get(Id);
            if (EditedPriceStatus == null)
                return null;
            return EditedPriceStatus;
        }

        public EditedPriceStatusFilter ToFilter(EditedPriceStatusFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<EditedPriceStatusFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                EditedPriceStatusFilter subFilter = new EditedPriceStatusFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
