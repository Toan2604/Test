using DMS.Common;
using DMS.Entities;
using DMS.Services.MPosition;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.position
{
    public class PositionController : RpcController
    {
        private IStatusService StatusService;
        private IPositionService PositionService;
        private ICurrentContext CurrentContext;
        public PositionController(
            IStatusService StatusService,
            IPositionService PositionService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.PositionService = PositionService;
            this.CurrentContext = CurrentContext;
        }

        [Route(PositionRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Position_PositionFilterDTO Position_PositionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PositionFilter PositionFilter = ConvertFilterDTOToFilterEntity(Position_PositionFilterDTO);
            PositionFilter = PositionService.ToFilter(PositionFilter);
            int count = await PositionService.Count(PositionFilter);
            return count;
        }

        [Route(PositionRoute.List), HttpPost]
        public async Task<ActionResult<List<Position_PositionDTO>>> List([FromBody] Position_PositionFilterDTO Position_PositionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            PositionFilter PositionFilter = ConvertFilterDTOToFilterEntity(Position_PositionFilterDTO);
            PositionFilter = PositionService.ToFilter(PositionFilter);
            List<Position> Positions = await PositionService.List(PositionFilter);
            List<Position_PositionDTO> Position_PositionDTOs = Positions
                .Select(c => new Position_PositionDTO(c)).ToList();
            return Position_PositionDTOs;
        }

        [Route(PositionRoute.Get), HttpPost]
        public async Task<ActionResult<Position_PositionDTO>> Get([FromBody] Position_PositionDTO Position_PositionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Position_PositionDTO.Id))
                return Forbid();

            Position Position = await PositionService.Get(Position_PositionDTO.Id);
            return new Position_PositionDTO(Position);
        }
        private async Task<bool> HasPermission(long Id)
        {
            PositionFilter PositionFilter = new PositionFilter();
            PositionFilter = PositionService.ToFilter(PositionFilter);
            if (Id == 0)
            {

            }
            else
            {
                PositionFilter.Id = new IdFilter { Equal = Id };
                int count = await PositionService.Count(PositionFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Position ConvertDTOToEntity(Position_PositionDTO Position_PositionDTO)
        {
            Position_PositionDTO.TrimString();
            Position Position = new Position();
            Position.Id = Position_PositionDTO.Id;
            Position.Code = Position_PositionDTO.Code;
            Position.Name = Position_PositionDTO.Name;
            Position.StatusId = Position_PositionDTO.StatusId;
            Position.Status = Position_PositionDTO.Status == null ? null : new Status
            {
                Id = Position_PositionDTO.Status.Id,
                Code = Position_PositionDTO.Status.Code,
                Name = Position_PositionDTO.Status.Name,
            };
            Position.BaseLanguage = CurrentContext.Language;
            return Position;
        }

        private PositionFilter ConvertFilterDTOToFilterEntity(Position_PositionFilterDTO Position_PositionFilterDTO)
        {
            PositionFilter PositionFilter = new PositionFilter();
            PositionFilter.Selects = PositionSelect.ALL;
            PositionFilter.Skip = Position_PositionFilterDTO.Skip;
            PositionFilter.Take = Position_PositionFilterDTO.Take;
            PositionFilter.OrderBy = Position_PositionFilterDTO.OrderBy;
            PositionFilter.OrderType = Position_PositionFilterDTO.OrderType;

            PositionFilter.Id = Position_PositionFilterDTO.Id;
            PositionFilter.Code = Position_PositionFilterDTO.Code;
            PositionFilter.Name = Position_PositionFilterDTO.Name;
            PositionFilter.StatusId = Position_PositionFilterDTO.StatusId;
            PositionFilter.CreatedAt = Position_PositionFilterDTO.CreatedAt;
            PositionFilter.UpdatedAt = Position_PositionFilterDTO.UpdatedAt;
            return PositionFilter;
        }

        [Route(PositionRoute.FilterListStatus), HttpPost]
        public async Task<List<Position_StatusDTO>> FilterListStatus([FromBody] Position_StatusFilterDTO Position_StatusFilterDTO)
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
            List<Position_StatusDTO> Position_StatusDTOs = Statuses
                .Select(x => new Position_StatusDTO(x)).ToList();
            return Position_StatusDTOs;
        }

        [Route(PositionRoute.SingleListStatus), HttpPost]
        public async Task<List<Position_StatusDTO>> SingleListStatus([FromBody] Position_StatusFilterDTO Position_StatusFilterDTO)
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
            List<Position_StatusDTO> Position_StatusDTOs = Statuses
                .Select(x => new Position_StatusDTO(x)).ToList();
            return Position_StatusDTOs;
        }

    }
}

