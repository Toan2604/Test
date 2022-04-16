using DMS.Rpc.reports.report_store_checking.report_store_unchecked;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store_checking.report_store_unchecked
{
    public partial class ReportStoreUncheckedControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportStoreUnchecked_ReportStoreUncheckedFilterDTO = new ReportStoreUnchecked_ReportStoreUncheckedFilterDTO
            {
                Take = 20,

            };
        }

        public async Task Get_ReportStoreUnchecked()
        {
            var Result = await ReportStoreUncheckedController.List(ReportStoreUnchecked_ReportStoreUncheckedFilterDTO);
            if (Result.Value != null) ReportStoreUnchecked_ReportStoreUncheckedDTOs = Result.Value;
            if (Result.Result != null) ReportStoreUnchecked_ReportStoreUncheckedDTOs =
                     (List<ReportStoreUnchecked_ReportStoreUncheckedDTO>)((BadRequestObjectResult)Result.Result).Value;
        }
    }
}
