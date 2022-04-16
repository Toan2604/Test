using DMS.Rpc.reports.report_store.report_statistic_store_scouting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store.report_statistic_store_scouting
{
    public partial class ReportStatisticStoreScoutingControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportStatisticStoreScoutingFilterDTO = new ReportStatisticStoreScouting_ReportStatisticStoreScoutingFilterDTO
            {
                Take = 20,
                Date = new DateFilter { GreaterEqual = new DateTime(2021,11,19).AddHours(-7), LessEqual = new DateTime(2021,11,20).AddHours(-7).AddSeconds(-1)},

            };
        }

        public async Task Get_ReportStatisticStoreScouting()
        {
            var Result = await ReportStatisticStoreScoutingController.List(ReportStatisticStoreScoutingFilterDTO);
            if (Result.Value != null) ReportStatisticStoreScoutingDTOs = Result.Value;
            if (Result.Result != null) ReportStatisticStoreScoutingDTOs =
                     (List<ReportStatisticStoreScouting_ReportStatisticStoreScoutingDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
