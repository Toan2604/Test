using DMS.Rpc.reports.report_store.report_store_state_change;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Tests.Rpc.report.report_store.report_store_state_change
{
    public partial class ReportStoreStateChangeControllerFeature
    {
        public async Task When_UserInput_Filter()
        {
            ReportStoreStateChangeFilterDTO = new ReportStoreStateChange_ReportStoreStateChangeFilterDTO
            {
                Take = 20,
                CreatedAt = new DateFilter { GreaterEqual = new DateTime(2021, 11, 19).AddHours(-7), LessEqual = new DateTime(2021, 11, 20).AddHours(-7).AddSeconds(-1) },

            };
        }

        public async Task Get_ReportStoreStateChange()
        {
            ReportStoreStateChangeDTOs = await ReportStoreStateChangeController.List(ReportStoreStateChangeFilterDTO);

        }
    }
}
