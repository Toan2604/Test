using DMS.Rpc.reports.report_store.report_store_general;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store.report_store_general
{
    public partial class ReportStoreGeneralControllerFeature : BaseTests
    {
        public async Task When_UserInput_Filter()
        {
            ReportStoreGeneralFilterDTO = new ReportStoreGeneral_ReportStoreGeneralFilterDTO
            {
                Take = 20,
                CheckIn = new DateFilter { GreaterEqual = new DateTime(2021, 11, 19).AddHours(-7), LessEqual = new DateTime(2021, 11, 20).AddHours(-7).AddSeconds(-1) },

            };
        }

        public async Task Get_ReportStoreGeneral()
        {
            var Result = await ReportStoreGeneralController.List(ReportStoreGeneralFilterDTO);
            if (Result.Value != null) ReportStoreGeneralDTOs = Result.Value;
            if (Result.Result != null) ReportStoreGeneralDTOs =
                     (List<ReportStoreGeneral_ReportStoreGeneralDTO>)((BadRequestObjectResult)Result.Result).Value;

        }
    }
}
