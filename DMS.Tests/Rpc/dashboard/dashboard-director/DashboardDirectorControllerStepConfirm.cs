using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Enums;
using DMS.Rpc.store;
using DMS.Rpc.dashboards.store_information;
using DMS.Rpc.dashboards.director;
namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardDirectorControllerFeature
    {
        private async Task Then_Result(long quanlity)
        {
            Assert.AreEqual(quanlity, Result);
        }
        private async Task Then_DecimalResult(decimal quanlity)
        {
            Assert.AreEqual(quanlity, DecimalResult);
        }
        private async Task Then_StatisticDailyResult(decimal Revenue, long StoreCheckingCounter, long StoreHasCheckedCounter, long SalesOrderCounter)
        {
            Assert.AreEqual(Revenue, DashboardDirector_StatisticDailyDTO.Revenue);
            Assert.AreEqual(StoreCheckingCounter, DashboardDirector_StatisticDailyDTO.StoreCheckingCounter);
            Assert.AreEqual(StoreHasCheckedCounter, DashboardDirector_StatisticDailyDTO.StoreHasCheckedCounter);
            Assert.AreEqual(SalesOrderCounter, DashboardDirector_StatisticDailyDTO.IndirectSalesOrderCounter);
        }
        private async Task Then_StoreCoverage_NoFilter_Result()
        {
            Assert.AreEqual(97163, DashboardDirector_StoreDTOs.Count);
            Assert.AreEqual(20.992385700000000M, DashboardDirector_StoreDTOs[0].Latitude);
            Assert.AreEqual(105.944133200000000M, DashboardDirector_StoreDTOs[0].Longitude);
            Assert.AreEqual(76, DashboardDirector_StoreDTOs[1].Id);
        }
        private async Task Then_SaleEmployeeLocation_NoFilter_Result()
        {
            Assert.AreEqual(97163, DashboardDirector_AppUserDTOs.Count);
            Assert.AreEqual(20.992385700000000M, DashboardDirector_AppUserDTOs[0].Latitude);
            Assert.AreEqual(105.944133200000000M, DashboardDirector_AppUserDTOs[0].Longitude);
            Assert.AreEqual(76, DashboardDirector_AppUserDTOs[1].Id);
        }
        private async Task Then_ListIndirectSalesOrder_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_DirectSalesOrderDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_DirectSalesOrderDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].Id, DashboardDirector_DirectSalesOrderDTOs[i].Id);
                Assert.AreEqual(Expected[i].Code, DashboardDirector_DirectSalesOrderDTOs[i].Code);
                Assert.AreEqual(Expected[i].Total, DashboardDirector_DirectSalesOrderDTOs[i].Total);
                Assert.AreEqual(Expected[i].SaleEmployeeId, DashboardDirector_DirectSalesOrderDTOs[i].SaleEmployeeId);
            }
        }
        private async Task Then_Top5RevenueByProduct_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_Top5RevenueByProductDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_Top5RevenueByProductDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].ProductId, DashboardDirector_Top5RevenueByProductDTOs[i].ProductId);
                Assert.AreEqual(Expected[i].ProductCode, DashboardDirector_Top5RevenueByProductDTOs[i].ProductCode);
                Assert.AreEqual(Expected[i].ProductName, DashboardDirector_Top5RevenueByProductDTOs[i].ProductName);
                Assert.AreEqual(Expected[i].Revenue, DashboardDirector_Top5RevenueByEmployeeDTOs[i].Revenue);
            }
        }
        private async Task Then_Top5RevenueByEmployee_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_Top5RevenueByEmployeeDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_Top5RevenueByEmployeeDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].EmployeeId, DashboardDirector_Top5RevenueByEmployeeDTOs[i].EmployeeId);
                Assert.AreEqual(Expected[i].EmployeeName, DashboardDirector_Top5RevenueByEmployeeDTOs[i].EmployeeName);
                Assert.AreEqual(Expected[i].Revenue, DashboardDirector_Top5RevenueByEmployeeDTOs[i].Revenue);
            }
        }

        private async Task Then_RevenueFluctuation_ThisYear_Result(string path)
        {
            var Expected = ReadFileFromJson<DashboardDirector_RevenueFluctuationDTO>(path);

            Assert.AreEqual(Expected.RevenueFluctuationByMonths.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Count);
            Assert.AreEqual(Expected.RevenueFluctuationByQuaters.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Count);
            Assert.AreEqual(Expected.RevenueFluctuationByYears.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears.Count);

            for (int i = 0; i < Expected.RevenueFluctuationByMonths.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByMonths[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByMonths[i].Day, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths[i].Day);
            }

            for (int i = 0; i < Expected.RevenueFluctuationByQuaters.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByQuaters[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByQuaters[i].Month, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters[i].Month);
            }

            for (int i = 0; i < Expected.RevenueFluctuationByYears.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByYears[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByYears[i].Month, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears[i].Month);
            }
        }
        private async Task Then_IndirectSalesOrderFluctuation_ThisYear_Result(string path)
        {
            var Expected = ReadFileFromJson<DashboardDirector_IndirectSalesOrderFluctuationDTO>(path);

            Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByMonths.Count, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths.Count);
            Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByQuaters.Count, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters.Count);
            Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByYears.Count, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears.Count);

            for (int i = 0; i < Expected.IndirectSalesOrderFluctuationByMonths.Count; i++)
            {
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByMonths[i].IndirectSalesOrderCounter, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths[i].IndirectSalesOrderCounter);
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByMonths[i].Day, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByMonths[i].Day);
            }

            for (int i = 0; i < Expected.IndirectSalesOrderFluctuationByQuaters.Count; i++)
            {
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByQuaters[i].IndirectSalesOrderCounter, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters[i].IndirectSalesOrderCounter);
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByQuaters[i].Month, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByQuaters[i].Month);
            }

            for (int i = 0; i < Expected.IndirectSalesOrderFluctuationByYears.Count; i++)
            {
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByYears[i].IndirectSalesOrderCounter, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears[i].IndirectSalesOrderCounter);
                Assert.AreEqual(Expected.IndirectSalesOrderFluctuationByYears[i].Month, DashboardDirector_IndirectSalesOrderFluctuationDTO.IndirectSalesOrderFluctuationByYears[i].Month);
            }
        }
        private async Task Then_ListDirectSalesOrder_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_DirectSalesOrderDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_DirectSalesOrderDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].Id, DashboardDirector_DirectSalesOrderDTOs[i].Id);
                Assert.AreEqual(Expected[i].Code, DashboardDirector_DirectSalesOrderDTOs[i].Code);
                Assert.AreEqual(Expected[i].Total, DashboardDirector_DirectSalesOrderDTOs[i].Total);
                Assert.AreEqual(Expected[i].SaleEmployeeId, DashboardDirector_DirectSalesOrderDTOs[i].SaleEmployeeId);
            }
        }
        private async Task Then_Top5DirectRevenueByProduct_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_Top5RevenueByProductDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_Top5RevenueByProductDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].ProductId, DashboardDirector_Top5RevenueByProductDTOs[i].ProductId);
                Assert.AreEqual(Expected[i].ProductCode, DashboardDirector_Top5RevenueByProductDTOs[i].ProductCode);
                Assert.AreEqual(Expected[i].ProductName, DashboardDirector_Top5RevenueByProductDTOs[i].ProductName);
                Assert.AreEqual(Expected[i].Revenue, DashboardDirector_Top5RevenueByEmployeeDTOs[i].Revenue);
            }
        }
        private async Task Then_Top5DirectRevenueByEmployee_NoFilter_Result(string path)
        {
            var Expected = ReadFileFromJson<List<DashboardDirector_Top5RevenueByEmployeeDTO>>(path);

            Assert.AreEqual(Expected.Count, DashboardDirector_Top5RevenueByEmployeeDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].EmployeeId, DashboardDirector_Top5RevenueByEmployeeDTOs[i].EmployeeId);
                Assert.AreEqual(Expected[i].EmployeeName, DashboardDirector_Top5RevenueByEmployeeDTOs[i].EmployeeName);
                Assert.AreEqual(Expected[i].Revenue, DashboardDirector_Top5RevenueByEmployeeDTOs[i].Revenue);
            }
        }
        private async Task Then_DirectRevenueFluctuation_ThisYear_Result(string path)
        {
            var Expected = ReadFileFromJson<DashboardDirector_RevenueFluctuationDTO>(path);

            Assert.AreEqual(Expected.RevenueFluctuationByMonths.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Count);
            Assert.AreEqual(Expected.RevenueFluctuationByQuaters.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Count);
            Assert.AreEqual(Expected.RevenueFluctuationByYears.Count, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears.Count);

            for (int i = 0; i < Expected.RevenueFluctuationByMonths.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByMonths[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByMonths[i].Day, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths[i].Day);
            }

            for (int i = 0; i < Expected.RevenueFluctuationByQuaters.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByQuaters[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByQuaters[i].Month, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters[i].Month);
            }

            for (int i = 0; i < Expected.RevenueFluctuationByYears.Count; i++)
            {
                Assert.AreEqual(Expected.RevenueFluctuationByYears[i].Revenue, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears[i].Revenue);
                Assert.AreEqual(Expected.RevenueFluctuationByYears[i].Month, DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears[i].Month);
            }
        }
        private async Task Then_DirectSalesOrderFluctuation_ThisYear_Result(string path)
        {
            var Expected = ReadFileFromJson<DashboardDirector_DirectSalesOrderFluctuationDTO>(path);

            Assert.AreEqual(Expected.DirectSalesOrderFluctuationByMonths.Count, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths.Count);
            Assert.AreEqual(Expected.DirectSalesOrderFluctuationByQuaters.Count, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters.Count);
            Assert.AreEqual(Expected.DirectSalesOrderFluctuationByYears.Count, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears.Count);

            for (int i = 0; i < Expected.DirectSalesOrderFluctuationByMonths.Count; i++)
            {
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByMonths[i].DirectSalesOrderCounter, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths[i].DirectSalesOrderCounter);
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByMonths[i].Day, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths[i].Day);

            }

            for (int i = 0; i < Expected.DirectSalesOrderFluctuationByQuaters.Count; i++)
            {
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByQuaters[i].DirectSalesOrderCounter, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters[i].DirectSalesOrderCounter);
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByQuaters[i].Month, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters[i].Month);

            }

            for (int i = 0; i < Expected.DirectSalesOrderFluctuationByYears.Count; i++)
            {
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByYears[i].DirectSalesOrderCounter, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears[i].DirectSalesOrderCounter);
                Assert.AreEqual(Expected.DirectSalesOrderFluctuationByYears[i].Month, DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears[i].Month);

            }
        }
        private async Task Then_DirectStatisticDailyResult(decimal Revenue, long StoreCheckingCounter, long StoreHasCheckedCounter, long SalesOrderCounter)
        {
            Assert.AreEqual(Revenue, DashboardDirector_StatisticDailyDirectSalesOrderDTO.Revenue);
            Assert.AreEqual(StoreCheckingCounter, DashboardDirector_StatisticDailyDirectSalesOrderDTO.StoreCheckingCounter);
            Assert.AreEqual(StoreHasCheckedCounter, DashboardDirector_StatisticDailyDirectSalesOrderDTO.StoreHasCheckedCounter);
            Assert.AreEqual(SalesOrderCounter, DashboardDirector_StatisticDailyDirectSalesOrderDTO.DirectSalesOrderCounter);
        }
    }
}
