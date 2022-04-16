using DMS.Rpc.reports.report_store_checking.report_store_checked;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store_checking.report_store_checked
{
    public partial class ReportStoreCheckedControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportStoreChecked_ReportStoreCheckedFilterDTO = new ReportStoreChecked_ReportStoreCheckedFilterDTO
            {
                Take = 20,

            };
        }

        public async Task Get_ReportStoreChecked()
        {
            var Result = await ReportStoreCheckedController.List(ReportStoreChecked_ReportStoreCheckedFilterDTO);
            if (Result.Value != null) ReportStoreChecked_ReportStoreCheckedDTOs = Result.Value;
            if (Result.Result != null) ReportStoreChecked_ReportStoreCheckedDTOs =
                     (List<ReportStoreChecked_ReportStoreCheckedDTO>)((BadRequestObjectResult)Result.Result).Value;
        }
    }
}
