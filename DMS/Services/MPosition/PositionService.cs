using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPosition
{
    public interface IPositionService : IServiceScoped
    {
        Task<int> Count(PositionFilter PositionFilter);
        Task<List<Position>> List(PositionFilter PositionFilter);
        Task<Position> Get(long Id);
        PositionFilter ToFilter(PositionFilter PositionFilter);
    }

    public class PositionService : BaseService, IPositionService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IPositionValidator PositionValidator;

        public PositionService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IPositionValidator PositionValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.PositionValidator = PositionValidator;
        }
        public async Task<int> Count(PositionFilter PositionFilter)
        {
            try
            {
                int result = await UOW.PositionRepository.Count(PositionFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PositionService));
            }
            return 0;
        }

        public async Task<List<Position>> List(PositionFilter PositionFilter)
        {
            try
            {
                List<Position> Positions = await UOW.PositionRepository.List(PositionFilter);
                return Positions;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PositionService));
            }
            return null;
        }
        public async Task<Position> Get(long Id)
        {
            Position Position = await UOW.PositionRepository.Get(Id);
            if (Position == null)
                return null;
            return Position;
        }
        public PositionFilter ToFilter(PositionFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<PositionFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                PositionFilter subFilter = new PositionFilter();
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
