using DMS.Helpers;
using DMS.Models;
using DMS.Rpc.kpi_item;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.kpi.kpi_item
{
    public partial class Kpi_ItemControllerFeature
    {
        private async Task Then_ImportSuccess(int count)
        {
            int KpiItem_count = await DataContext.KpiItem.CountAsync();
            int KpiItemcontent_count = await DataContext.KpiItemContent.CountAsync();
            int KpiItemcontentkpiperiodmapping_count = await DataContext.KpiItemContentKpiCriteriaItemMapping.CountAsync();
            Assert.AreEqual(count, KpiItem_count);
            Assert.AreEqual(count * 1, KpiItemcontent_count); // có 1 item
            Assert.AreEqual(count * 1 * 4, KpiItemcontentkpiperiodmapping_count);// mỗi item có 4 chỉ tiêu
        }
        private async Task Then_Success()
        {
            await Then_Id(KpiItem_KpiItemDTO);
            await Then_Organization(KpiItem_KpiItemDTO);
            await Then_Employees(KpiItem_KpiItemDTO);
            await Then_Status(KpiItem_KpiItemDTO);
            await Then_Period(KpiItem_KpiItemDTO);
            await Then_KpiItemType(KpiItem_KpiItemDTO);
            await Then_KpiYear(KpiItem_KpiItemDTO);
            await Then_Item(KpiItem_KpiItemDTO);
            await Then_OldKpi(KpiItem_KpiItemDTO);
            await Then_Value(KpiItem_KpiItemDTO);
            await Then_KpiTime(KpiItem_KpiItemDTO);
        }

        private async Task Then_Id(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Id)));
            Assert.AreEqual(0, KpiItem_KpiItemDTO.Id);
        }
        private async Task Then_Employees(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Employees)));
            Assert.AreEqual(2, KpiItem_KpiItemDTO.Employees[0].Id);
        }
        private async Task Then_Period(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.KpiPeriod)));
            Assert.AreEqual(108, KpiItem_KpiItemDTO.KpiPeriodId);
        }
        private async Task Then_KpiYear(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.KpiYear)));
            Assert.AreEqual(2021, KpiItem_KpiItemDTO.KpiYearId);
        }
        private async Task Then_KpiItemType(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.KpiItemType)));
            Assert.AreEqual(2, KpiItem_KpiItemDTO.KpiItemTypeId);
        }
        private async Task Then_KpiTime(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.KpiYear)));

            bool validate_year = KpiItem_KpiItemDTO.KpiYearId >= StaticParams.DateTimeNow.Year;
            bool validate_month = (KpiItem_KpiItemDTO.KpiPeriodId - 200) * 3 >= StaticParams.DateTimeNow.Month || // Qúy * 3
                                  (KpiItem_KpiItemDTO.KpiPeriodId - 100) >= StaticParams.DateTimeNow.Month; // Tháng
            Assert.AreEqual(true, validate_year && validate_month);
        }
        private async Task Then_Organization(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Organization)));
            Assert.AreEqual(1, KpiItem_KpiItemDTO.OrganizationId);
        }
        private async Task Then_OldKpi(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Employee.Id)));
            Assert.AreEqual(2, KpiItem_KpiItemDTO.Employees[0].Id);
        }
        private async Task Then_Status(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Status)));
            Assert.AreEqual(1, KpiItem_KpiItemDTO.StatusId);
        }
        private async Task Then_Item(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey("KpiItem.KpiItemContent.Item"));
            Assert.AreEqual(2315, KpiItem_KpiItemDTO.KpiItemContents[0].ItemId);
        }
        private async Task Then_Value(KpiItem_KpiItemDTO KpiItem_KpiItemDTO)
        {
            Assert.AreEqual(false, KpiItem_KpiItemDTO.Errors.ContainsKey(nameof(KpiItem_KpiItemDTO.Id)));

            bool flag = false;
            foreach (var content in KpiItem_KpiItemDTO.KpiItemContents)
            {
                foreach (var item in content.KpiItemContentKpiCriteriaItemMappings)
                {
                    if (item.Value != null) flag = true;
                }
            }// Cần ít nhất một giá trị
            Assert.AreEqual(true, flag);
        }

    }
}
