using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.problem_type
{
    public partial class ProblemTypeController : RpcController
    {
        [Route(ProblemTypeRoute.FilterListStatus), HttpPost]
        public async Task<List<ProblemType_StatusDTO>> FilterListStatus()
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
            List<ProblemType_StatusDTO> ProblemType_StatusDTOs = Statuses
                .Select(x => new ProblemType_StatusDTO(x)).ToList();
            return ProblemType_StatusDTOs;
        }

        [Route(ProblemTypeRoute.SingleListStatus), HttpPost]
        public async Task<List<ProblemType_StatusDTO>> SingleListStatus()
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
            List<ProblemType_StatusDTO> ProblemType_StatusDTOs = Statuses
                .Select(x => new ProblemType_StatusDTO(x)).ToList();
            return ProblemType_StatusDTOs;
        }
    }
}

