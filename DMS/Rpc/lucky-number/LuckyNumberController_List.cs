using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public partial class LuckyNumberController : RpcController
    {
        [Route(LuckyNumberRoute.FilterListOrganization), HttpPost]
        public async Task<List<LuckyNumber_OrganizationDTO>> FilterListOrganization([FromBody] LuckyNumber_OrganizationFilterDTO LuckyNumber_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = LuckyNumber_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = LuckyNumber_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = LuckyNumber_OrganizationFilterDTO.Name;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<LuckyNumber_OrganizationDTO> KpiGeneral_OrganizationDTOs = Organizations
                .Select(x => new LuckyNumber_OrganizationDTO(x)).ToList();
            return KpiGeneral_OrganizationDTOs;
        }

        [Route(LuckyNumberRoute.FilterListRewardStatus), HttpPost]
        public async Task<List<LuckyNumber_RewardStatusDTO>> FilterListRewardStatus([FromBody] LuckyNumber_RewardStatusFilterDTO LuckyNumber_RewardStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RewardStatusFilter RewardStatusFilter = new RewardStatusFilter();
            RewardStatusFilter.Skip = 0;
            RewardStatusFilter.Take = int.MaxValue;
            RewardStatusFilter.Take = 20;
            RewardStatusFilter.OrderBy = RewardStatusOrder.Id;
            RewardStatusFilter.OrderType = OrderType.ASC;
            RewardStatusFilter.Selects = RewardStatusSelect.ALL;

            List<RewardStatus> RewardStatuses = await RewardStatusService.List(RewardStatusFilter);
            List<LuckyNumber_RewardStatusDTO> LuckyNumber_RewardStatusDTOs = RewardStatuses
                .Select(x => new LuckyNumber_RewardStatusDTO(x)).ToList();
            return LuckyNumber_RewardStatusDTOs;
        }

        [Route(LuckyNumberRoute.SingleListRewardStatus), HttpPost]
        public async Task<List<LuckyNumber_RewardStatusDTO>> SingleListRewardStatus([FromBody] LuckyNumber_RewardStatusFilterDTO LuckyNumber_RewardStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RewardStatusFilter RewardStatusFilter = new RewardStatusFilter();
            RewardStatusFilter.Skip = 0;
            RewardStatusFilter.Take = int.MaxValue;
            RewardStatusFilter.Take = 20;
            RewardStatusFilter.OrderBy = RewardStatusOrder.Id;
            RewardStatusFilter.OrderType = OrderType.ASC;
            RewardStatusFilter.Selects = RewardStatusSelect.ALL;

            List<RewardStatus> RewardStatuses = await RewardStatusService.List(RewardStatusFilter);
            List<LuckyNumber_RewardStatusDTO> LuckyNumber_RewardStatusDTOs = RewardStatuses
                .Select(x => new LuckyNumber_RewardStatusDTO(x)).ToList();
            return LuckyNumber_RewardStatusDTOs;
        }

    }
}

