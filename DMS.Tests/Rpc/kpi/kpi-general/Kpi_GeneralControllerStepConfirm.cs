using DMS.Rpc.kpi_general;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DMS.Tests.Rpc.kpi.kpi_general
{
    public partial class Kpi_GeneralControllerFeature
    {
        private async Task Then_ImportSuccess(int count)
        {
            int kpigeneral_count = await DataContext.KpiGeneral.CountAsync();
            int kpigeneralcontent_count = await DataContext.KpiGeneralContent.CountAsync();
            int kpigeneralcontentkpiperiodmapping_count = await DataContext.KpiGeneralContentKpiPeriodMapping.CountAsync();
            Assert.AreEqual(count, kpigeneral_count);
            Assert.AreEqual(count * 12, kpigeneralcontent_count); // có 12 chỉ tiêu
            Assert.AreEqual(count * 12 * 17, kpigeneralcontentkpiperiodmapping_count);// mỗi chỉ tiêu 17 kỳ
        }
        private async Task Then_Success()
        {
            await Then_Organization(KpiGeneral_KpiGeneralDTO);
            await Then_Id(KpiGeneral_KpiGeneralDTO);
            await Then_Employees(KpiGeneral_KpiGeneralDTO);
            await Then_Status(KpiGeneral_KpiGeneralDTO);
            await Then_KpiGeneralContent(KpiGeneral_KpiGeneralDTO);
            await Then_KpiYear(KpiGeneral_KpiGeneralDTO);
            await Then_Value(KpiGeneral_KpiGeneralDTO);
        }

        private async Task Then_Organization(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.Organization)));
            Assert.AreEqual(1, KpiGeneral_KpiGeneralDTO.OrganizationId);
        }
        private async Task Then_Id(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.Id)));
            Assert.AreEqual(0, KpiGeneral_KpiGeneralDTO.Id);
        }
        private async Task Then_Employees(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.EmployeeIds)));
            //Assert.AreEqual(0, KpiGeneral_KpiGeneralDTO.EmployeeId);
        }
        private async Task Then_Status(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.Status)));
            Assert.AreEqual(1, KpiGeneral_KpiGeneralDTO.StatusId);
        }
        private async Task Then_KpiGeneralContent(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.KpiGeneralContents)));
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.KpiGeneralContents.Any(x => x.KpiGeneralContentKpiPeriodMappings.Count != 17));
            // Phải có đủ 17 kỳ KPI
        }
        private async Task Then_KpiYear(KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.KpiYear)));
            Assert.AreEqual(2021, KpiGeneral_KpiGeneralDTO.KpiYearId);
        }

        private async Task Then_Value (KpiGeneral_KpiGeneralDTO KpiGeneral_KpiGeneralDTO)
        {
            Assert.AreEqual(false, KpiGeneral_KpiGeneralDTO.Errors.ContainsKey(nameof(KpiGeneral_KpiGeneralDTO.Id)));

            bool flag = false;
            foreach (var content in KpiGeneral_KpiGeneralDTO.KpiGeneralContents)
            {
                foreach (var item in content.KpiGeneralContentKpiPeriodMappings)
                {
                    if (item.Value != null) flag = true;
                }
            }// Cần ít nhất một giá trị
            Assert.AreEqual(true, flag);
        }
    }
}
