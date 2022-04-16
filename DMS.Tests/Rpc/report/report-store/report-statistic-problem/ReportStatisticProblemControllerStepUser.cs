using DMS.Rpc.reports.report_store.report_statistic_problem;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store.report_statistic_problem
{
    public partial class ReportStatisticProblemControllerFeature : BaseTests
    {
        public async Task When_UserInput_Filter()
        {
            ReportStatisticProblemFilterDTO = new ReportStatisticProblem_ReportStatisticProblemFilterDTO
            {
                Take = 20,
                Date = new DateFilter { GreaterEqual = new DateTime(2021, 11, 19).AddHours(-7), LessEqual = new DateTime(2021, 11, 20).AddHours(-7).AddSeconds(-1) },

            };
        }

        public async Task Get_ReportStatisticProblem()
        {
            var Result = await ReportStatisticProblemController.List(ReportStatisticProblemFilterDTO);
            if (Result.Value != null) ReportStatisticProblemDTOs = Result.Value;
            if (Result.Result != null) ReportStatisticProblemDTOs =
                     (List<ReportStatisticProblem_ReportStatisticProblemDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
