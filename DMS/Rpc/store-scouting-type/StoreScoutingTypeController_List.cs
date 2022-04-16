using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting_type
{
    public partial class StoreScoutingTypeController : RpcController
    {
        [Route(StoreScoutingTypeRoute.FilterListStatus), HttpPost]
        public async Task<List<StoreScoutingType_StatusDTO>> FilterListStatus()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<StoreScoutingType_StatusDTO> StoreScoutingType_StatusDTOs = Statuses
                .Select(x => new StoreScoutingType_StatusDTO(x)).ToList();
            return StoreScoutingType_StatusDTOs;
        }

        [Route(StoreScoutingTypeRoute.SingleListStatus), HttpPost]
        public async Task<List<StoreScoutingType_StatusDTO>> SingleListStatus()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<StoreScoutingType_StatusDTO> StoreScoutingType_StatusDTOs = Statuses
                .Select(x => new StoreScoutingType_StatusDTO(x)).ToList();
            return StoreScoutingType_StatusDTOs;
        }
    }
}

