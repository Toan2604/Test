using DMS.Helpers;
using DMS.Models;
using DMS.Rpc.kpi_item;
using DMS.Rpc.kpi_product_grouping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.kpi.kpi_product_grouping
{
    public partial class Kpi_ProductGroupingControllerFeature
    {
        private async Task Then_ImportSuccess(int count)
        {
            int KpiProductGrouping_count = await DataContext.KpiProductGrouping.CountAsync();
            int KpiProductGroupingcontent_count = await DataContext.KpiProductGroupingContent.CountAsync();
            int KpiProductGroupingcontentkpiperiodmapping_count = await DataContext.KpiProductGroupingContentCriteriaMapping.CountAsync();
            Assert.AreEqual(count, KpiProductGrouping_count);
            Assert.AreEqual(count * 1, KpiProductGroupingcontent_count); // có 1 item
            Assert.AreEqual(count * 1 * 4, KpiProductGroupingcontentkpiperiodmapping_count);// mỗi item có 4 chỉ tiêu
        }
        private async Task Then_Success()
        {
            await Then_Id(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Organization(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Employees(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Status(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Period(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_KpiYear(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_KpiTime(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_KpiProductGroupingType(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Content(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Item(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_Value(KpiProductGrouping_KpiProductGroupingDTO);
            await Then_OldKpi(KpiProductGrouping_KpiProductGroupingDTO);
        }

        private async Task Then_Content(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey("KpiProductGroupingContent.ProductGrouping"));
            //Assert.AreEqual(0, KpiProductGrouping_KpiProductGroupingDTO.Id);
        }
        private async Task Then_Id(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Id)));
            Assert.AreEqual(0, KpiProductGrouping_KpiProductGroupingDTO.Id);
        }
        private async Task Then_Employees(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Employees)));
            Assert.AreEqual(42946, KpiProductGrouping_KpiProductGroupingDTO.Employees[0].Id);
        }
        private async Task Then_Period(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.KpiPeriod)));
            Assert.AreEqual(108, KpiProductGrouping_KpiProductGroupingDTO.KpiPeriodId);
        }
        private async Task Then_KpiYear(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.KpiYear)));
            Assert.AreEqual(2021, KpiProductGrouping_KpiProductGroupingDTO.KpiYearId);
        }
        private async Task Then_KpiProductGroupingType(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingType)));
            Assert.AreEqual(2, KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingTypeId);
        }
        private async Task Then_KpiTime(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.KpiYear)));

            bool validate_year = KpiProductGrouping_KpiProductGroupingDTO.KpiYearId >= StaticParams.DateTimeNow.Year;
            bool validate_month = (KpiProductGrouping_KpiProductGroupingDTO.KpiPeriodId - 200) * 3 >= StaticParams.DateTimeNow.Month || // Qúy * 3
                                  (KpiProductGrouping_KpiProductGroupingDTO.KpiPeriodId - 100) >= StaticParams.DateTimeNow.Month; // Tháng
            Assert.AreEqual(true, validate_year && validate_month);
        }
        private async Task Then_Organization(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Organization)));
            Assert.AreEqual(1, KpiProductGrouping_KpiProductGroupingDTO.OrganizationId);
        }
        private async Task Then_OldKpi(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Employee.Id)));
            Assert.AreEqual(42946, KpiProductGrouping_KpiProductGroupingDTO.Employees[0].Id);
        }
        private async Task Then_Status(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Status)));
            Assert.AreEqual(1, KpiProductGrouping_KpiProductGroupingDTO.StatusId);
        }
        private async Task Then_Item(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey("KpiProductGrouping.KpiProductGroupingContentItemMappings"));
            Assert.AreEqual(2313, KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingContents[0].KpiProductGroupingContentItemMappings[0].ItemId);
        }
        private async Task Then_Value(KpiProductGrouping_KpiProductGroupingDTO KpiProductGrouping_KpiProductGroupingDTO)
        {
            Assert.AreEqual(false, KpiProductGrouping_KpiProductGroupingDTO.Errors.ContainsKey(nameof(KpiProductGrouping_KpiProductGroupingDTO.Id)));

            bool flag = false;
            foreach (var content in KpiProductGrouping_KpiProductGroupingDTO.KpiProductGroupingContents)
            {
                foreach (var item in content.KpiProductGroupingContentCriteriaMappings)
                {
                    if (item.Value != null) flag = true;
                }
            }// Cần ít nhất một giá trị
            Assert.AreEqual(true, flag);
        }
    }
}
