using DMS.Common;
using DMS.DWModels;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public partial class DashboardDirectorController
    {

    
        [Route(DashboardDirectorRoute.CountDirectSalesOrder), HttpPost]
        public async Task<long> CountDirectSalesOrder([FromBody] DashboardDirector_DirectSalesOrderFluctuationFilterDTO DashboardDirector_DirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_DirectSalesOrderFluctuationFilterDTO.OrderDate;
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);

            return await DirectSalesOrderQuery.CountWithNoLockAsync();
        }

        [Route(DashboardDirectorRoute.DirectRevenueTotal), HttpPost]
        public async Task<decimal> DirectRevenueTotal([FromBody] DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - CurrentContext.TimeZone);
            DateTime End = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, 1).AddHours(0 - CurrentContext.TimeZone);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_SaledItemFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_SaledItemFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_SaledItemFluctuationFilterDTO.OrderDate;
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);

            var RevenueTotal = DirectSalesOrderQuery.Select(x => x.Total).Sum();
            return RevenueTotal;
        }

        [Route(DashboardDirectorRoute.DirectStatisticToday), HttpPost]
        public async Task<DashboardDirector_StatisticDailyDirectSalesOrderDTO> DirectStatisticToday([FromBody] DashboardDirector_StoreFilterDTO DashboardDirector_StoreFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_StoreFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, DateFilter);

            var RevenueTotal = await DirectSalesOrderQuery.Select(x => x.Total).SumAsync();
            var DirectSalesOrderCounter = await DirectSalesOrderQuery.CountWithNoLockAsync();
            var StoreHasCheckedCounter = await StoreCheckingQuery.Select(x => x.StoreId).Distinct().CountWithNoLockAsync();
            var StoreCheckingCounter = await StoreCheckingQuery.CountWithNoLockAsync();

            DashboardDirector_StatisticDailyDirectSalesOrderDTO DashboardDirector_StatisticDailyDirectSalesOrderDTO = new DashboardDirector_StatisticDailyDirectSalesOrderDTO()
            {
                Revenue = RevenueTotal,
                DirectSalesOrderCounter = DirectSalesOrderCounter,
                StoreHasCheckedCounter = StoreHasCheckedCounter,
                StoreCheckingCounter = StoreCheckingCounter
            };
            return DashboardDirector_StatisticDailyDirectSalesOrderDTO;
        }

        [Route(DashboardDirectorRoute.DirectStatisticYesterday), HttpPost]
        public async Task<DashboardDirector_StatisticDailyDirectSalesOrderDTO> DirectStatisticYesterday([FromBody] DashboardDirector_DirectSalesOrderFluctuationFilterDTO DashboardDirector_DirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Start = LocalStartDay(CurrentContext).AddDays(-1);
            DateTime End = LocalEndDay(CurrentContext).AddDays(-1);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);

            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.StoreId, StoreIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, DateFilter);

            var RevenueTotal = await DirectSalesOrderQuery.Select(x => x.Total).SumAsync();
            var DirectSalesOrderCounter = await DirectSalesOrderQuery.CountWithNoLockAsync();
            var StoreHasCheckedCounter = await StoreCheckingQuery.Select(x => x.StoreId).Distinct().CountWithNoLockAsync();
            var StoreCheckingCounter = await StoreCheckingQuery.CountWithNoLockAsync();

            DashboardDirector_StatisticDailyDirectSalesOrderDTO DashboardDirector_StatisticDailyDirectSalesOrderDTO = new DashboardDirector_StatisticDailyDirectSalesOrderDTO()
            {
                Revenue = RevenueTotal,
                DirectSalesOrderCounter = DirectSalesOrderCounter,
                StoreHasCheckedCounter = StoreHasCheckedCounter,
                StoreCheckingCounter = StoreCheckingCounter
            };
            return DashboardDirector_StatisticDailyDirectSalesOrderDTO;
        }

        [Route(DashboardDirectorRoute.ListDirectSalesOrder), HttpPost]
        public async Task<List<DashboardDirector_DirectSalesOrderDTO>> ListDirectSalesOrder([FromBody] DashboardDirector_DirectSalesOrderFluctuationFilterDTO DashboardDirector_DirectSalesOrderFluctuationFilterDTO)
        {
            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };
            var appUser = await AppUserService.Get(CurrentContext.UserId);

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.OrderByDescending(x => x.OrderDate);
            List<DashboardDirector_DirectSalesOrderDTO> DashboardUser_DirectSalesOrderDTOs = await DirectSalesOrderQuery.Select(i => new DashboardDirector_DirectSalesOrderDTO
            {
                Id = i.DirectSalesOrderId,
                Code = i.Code,
                OrderDate = i.OrderDate,
                GeneralApprovalStateId = i.GeneralApprovalStateId,
                SaleEmployeeId = i.SaleEmployeeId,
                Total = i.Total
            }).Skip(0).Take(5).ToListWithNoLockAsync();

            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser.Where(x => x.Id, AppUserIdFilter).ToListWithNoLockAsync();
            List<GeneralApprovalStateDAO> GeneralApprovalStateDAOs = await DataContext.GeneralApprovalState.Where(x => x.Id, GeneralApprovalStateIdFilter).ToListWithNoLockAsync();

            foreach (DashboardDirector_DirectSalesOrderDTO DashboardUser_DirectSalesOrderDTO in DashboardUser_DirectSalesOrderDTOs)
            {
                AppUserDAO AppUserDAO = AppUserDAOs.Where(x => x.Id == DashboardUser_DirectSalesOrderDTO.SaleEmployeeId).FirstOrDefault();
                if (AppUserDAO != null)
                {
                    DashboardUser_DirectSalesOrderDTO.SaleEmployee = new DashboardDirector_AppUserDTO
                    {
                        Id = AppUserDAO.Id,
                        DisplayName = AppUserDAO.DisplayName,
                        Username = AppUserDAO.Username
                    };
                }
                GeneralApprovalStateDAO GeneralApprovalStateDAO = GeneralApprovalStateDAOs.Where(r => r.Id == DashboardUser_DirectSalesOrderDTO.GeneralApprovalStateId).FirstOrDefault();
                if (GeneralApprovalStateDAO != null)
                {
                    DashboardUser_DirectSalesOrderDTO.GeneralApprovalState = new DashboardDirector_GeneralApprovalStateDTO
                    {
                        Id = GeneralApprovalStateDAO.Id,
                        Code = GeneralApprovalStateDAO.Code,
                        Name = GeneralApprovalStateDAO.Name
                    };
                }
            }
            return DashboardUser_DirectSalesOrderDTOs;
        }

        [Route(DashboardDirectorRoute.Top5DirectRevenueByProduct), HttpPost]
        public async Task<List<DashboardDirector_Top5RevenueByProductDTO>> Top5DirectRevenueByProduct([FromBody] DashboardDirector_Top5RevenueByProductFilterDTO DashboardDirector_Top5RevenueByProductFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_Top5RevenueByProductFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_Top5RevenueByProductFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_Top5RevenueByProductFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_Top5RevenueByProductFilterDTO.OrderDate;
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
            var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderIdFilter = new IdFilter { In = DirectSalesOrderIds };

            var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, OrderDateFilter);
            var RevenueByItemIds = await DirectSalesOrderTransactionQuery
                .GroupBy(x => x.ItemId)
                .Select(x => new
                {
                    ItemId = x.Key,
                    Revenue = (decimal)x.Sum(y => y.Amount) - x.Sum(y => y.GeneralDiscountAmount) + x.Sum(y => y.TaxAmount)
                }).Distinct().ToListWithNoLockAsync();

            var RevenueByProductIds = RevenueByItemIds.Select(x => new
            {
                ProductId = DataContext.Item.Where(i => i.Id == x.ItemId).FirstOrDefault(),
                Revenue = x.Revenue
            }); // convert ItemId to ProductId
            var RevenueByProduct = RevenueByProductIds.GroupBy(x => x.ProductId).Select(x => new DashboardDirector_Top5RevenueByProductDTO
            {
                ProductId = x.Key.ProductId,
                Revenue = (decimal)x.Sum(s => s.Revenue)
            });

            List<DashboardDirector_Top5RevenueByProductDTO> DashboardDirector_Top5RevenueByProductDTOs = RevenueByProduct.OrderByDescending(x => x.Revenue)
                .Skip(0).Take(5).ToList();
            foreach (var DashboardDirector_Top5RevenueByProductDTO in DashboardDirector_Top5RevenueByProductDTOs)
            {
                DashboardDirector_Top5RevenueByProductDTO.ProductName = DataContext.Product.Where(p => p.Id == DashboardDirector_Top5RevenueByProductDTO.ProductId)
                    .Select(p => p.Name).FirstOrDefault();
            } // Filter ProductName by ProductId
            return DashboardDirector_Top5RevenueByProductDTOs;
        }

        [Route(DashboardDirectorRoute.Top5DirectRevenueByEmployee), HttpPost]
        public async Task<List<DashboardDirector_Top5RevenueByEmployeeDTO>> Top5DirectRevenueByEmployee([FromBody] DashboardDirector_Top5RevenueByEmployeeFilterDTO DashboardDirector_Top5RevenueByEmployeeFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_Top5RevenueByEmployeeFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_Top5RevenueByEmployeeFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var OrderDateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            if (DashboardDirector_Top5RevenueByEmployeeFilterDTO.OrderDate.HasValue)
                OrderDateFilter = DashboardDirector_Top5RevenueByEmployeeFilterDTO.OrderDate;
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, OrderDateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
            List<DashboardDirector_Top5RevenueByEmployeeDTO> DashboardDirector_Top5RevenueByEmployeeDTOs = await DirectSalesOrderQuery
                .GroupBy(x => x.SaleEmployeeId)
                .Select(x => new DashboardDirector_Top5RevenueByEmployeeDTO
                {
                    EmployeeId = x.Key,
                    Revenue = (decimal)x.Sum(y => y.Total)
                }).OrderByDescending(x => x.Revenue).Skip(0).Take(5).ToListWithNoLockAsync();
            var AppUsers = await DataContext.AppUser.AsNoTracking().Select(x => new AppUserDAO
            {
                Id = x.Id,
                DisplayName = x.DisplayName
            }).ToListWithNoLockAsync();
            foreach (var DashboardDirector_Top5RevenueByEmployeeDTO in DashboardDirector_Top5RevenueByEmployeeDTOs)
            {
                DashboardDirector_Top5RevenueByEmployeeDTO.EmployeeName = AppUsers
                    .Where(a => a.Id == DashboardDirector_Top5RevenueByEmployeeDTO.EmployeeId)
                    .Select(a => a.DisplayName).FirstOrDefault();
            }
            return DashboardDirector_Top5RevenueByEmployeeDTOs;
        }

        [Route(DashboardDirectorRoute.DirectRevenueFluctuation), HttpPost]
        public async Task<DashboardDirector_RevenueFluctuationDTO> DirectRevenueFluctuation([FromBody] DashboardDirector_RevenueFluctuationFilterDTO DashboardDirector_RevenueFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_RevenueFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_RevenueFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
            var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderIdFilter = new IdFilter { In = DirectSalesOrderIds };

            var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);

            var DirectSalesOrderTransactionDAOs = await DirectSalesOrderTransactionQuery.Select(x => new DirectSalesOrderTransactionDAO
            {
                OrderDate = x.OrderDate,
                Revenue = x.Amount - x.GeneralDiscountAmount + x.TaxAmount
            }).ToListWithNoLockAsync();

            DashboardDirector_RevenueFluctuationDTO DashboardDirector_RevenueFluctuationDTO = new DashboardDirector_RevenueFluctuationDTO();

            if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths = new List<DashboardDirector_RevenueFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_RevenueFluctuationByMonthDTO RevenueFluctuationByMonth = new DashboardDirector_RevenueFluctuationByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Add(RevenueFluctuationByMonth);
                }

                foreach (var RevenueFluctuationByMonth in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).Month, (int)RevenueFluctuationByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByMonth.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {
                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths = new List<DashboardDirector_RevenueFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_RevenueFluctuationByMonthDTO RevenueFluctuationByMonth = new DashboardDirector_RevenueFluctuationByMonthDTO
                    {
                        Day = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths.Add(RevenueFluctuationByMonth);
                }

                foreach (var RevenueFluctuationByMonth in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByMonths)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, Now.AddHours(CurrentContext.TimeZone).AddMonths(-1).Month, (int)RevenueFluctuationByMonth.Day).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByMonth.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters = new List<DashboardDirector_RevenueFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_RevenueFluctuationByQuarterDTO RevenueFluctuationByQuarter = new DashboardDirector_RevenueFluctuationByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Add(RevenueFluctuationByQuarter);
                }

                foreach (var RevenueFluctuationByQuarter in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByQuarter.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters = new List<DashboardDirector_RevenueFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_RevenueFluctuationByQuarterDTO RevenueFluctuationByQuarter = new DashboardDirector_RevenueFluctuationByQuarterDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters.Add(RevenueFluctuationByQuarter);
                }

                foreach (var RevenueFluctuationByQuarter in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByQuaters)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByQuarter.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByQuarter.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            else if (DashboardDirector_RevenueFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {

                DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears = new List<DashboardDirector_RevenueFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_RevenueFluctuationByYearDTO RevenueFluctuationByYear = new DashboardDirector_RevenueFluctuationByYearDTO
                    {
                        Month = i,
                        Revenue = 0
                    };
                    DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears.Add(RevenueFluctuationByYear);
                }

                foreach (var RevenueFluctuationByYear in DashboardDirector_RevenueFluctuationDTO.RevenueFluctuationByYears)
                {
                    DateTime LocalStart = new DateTime(Now.AddHours(CurrentContext.TimeZone).Year, (int)RevenueFluctuationByYear.Month, 1).AddHours(0 - CurrentContext.TimeZone);
                    DateTime LocalEnd = LocalStart.AddHours(CurrentContext.TimeZone).AddMonths(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    RevenueFluctuationByYear.Revenue = DirectSalesOrderTransactionDAOs.Where(x => LocalStart <= x.OrderDate && x.OrderDate <= LocalEnd)
                        .Where(x => x.Revenue.HasValue)
                        .Select(x => x.Revenue.Value)
                        .DefaultIfEmpty(0)
                        .Sum();
                }

                return DashboardDirector_RevenueFluctuationDTO;
            }
            return new DashboardDirector_RevenueFluctuationDTO();
        }

        [Route(DashboardDirectorRoute.DirectSalesOrderFluctuation), HttpPost]
        public async Task<DashboardDirector_DirectSalesOrderFluctuationDTO> DirectSalesOrderFluctuation([FromBody] DashboardDirector_DirectSalesOrderFluctuationFilterDTO DashboardDirector_DirectSalesOrderFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_DirectSalesOrderFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_DirectSalesOrderFluctuationByMonthDTOs = await DirectSalesOrderQuery.GroupBy(x => x.OrderDate.Day).Select(x => new DashboardDirector_DirectSalesOrderFluctuationByMonthDTO
                {
                    Day = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO = new DashboardDirector_DirectSalesOrderFluctuationDTO();
                DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths = new List<DashboardDirector_DirectSalesOrderFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_DirectSalesOrderFluctuationByMonthDTO DirectSalesOrderFluctuationByMonth = new DashboardDirector_DirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = i,
                        DirectSalesOrderCounter = 0
                    };
                    DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths.Add(DirectSalesOrderFluctuationByMonth);
                }

                foreach (var DirectSalesOrderFluctuationByMonth in DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths)
                {
                    var data = DashboardDirector_DirectSalesOrderFluctuationByMonthDTOs.Where(x => x.Day == DirectSalesOrderFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderFluctuationByMonth.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return DashboardDirector_DirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {

                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_DirectSalesOrderFluctuationByMonthDTOs = await DirectSalesOrderQuery.GroupBy(x => x.OrderDate.Day).Select(x => new DashboardDirector_DirectSalesOrderFluctuationByMonthDTO
                {
                    Day = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO = new DashboardDirector_DirectSalesOrderFluctuationDTO();
                DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths = new List<DashboardDirector_DirectSalesOrderFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_DirectSalesOrderFluctuationByMonthDTO DirectSalesOrderFluctuationByMonth = new DashboardDirector_DirectSalesOrderFluctuationByMonthDTO
                    {
                        Day = i,
                        DirectSalesOrderCounter = 0
                    };
                    DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths.Add(DirectSalesOrderFluctuationByMonth);
                }

                foreach (var DirectSalesOrderFluctuationByMonth in DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByMonths)
                {
                    var data = DashboardDirector_DirectSalesOrderFluctuationByMonthDTOs.Where(x => x.Day == DirectSalesOrderFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderFluctuationByMonth.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return DashboardDirector_DirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));

                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_DirectSalesOrderFluctuationByQuarterDTOs = await DirectSalesOrderQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO = new DashboardDirector_DirectSalesOrderFluctuationDTO();
                DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters = new List<DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO DirectSalesOrderFluctuationByQuarter = new DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters.Add(DirectSalesOrderFluctuationByQuarter);
                }

                foreach (var DirectSalesOrderFluctuationByQuater in DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters)
                {
                    var data = DashboardDirector_DirectSalesOrderFluctuationByQuarterDTOs.Where(x => x.Month == DirectSalesOrderFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderFluctuationByQuater.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return DashboardDirector_DirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.AddHours(CurrentContext.TimeZone).Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_DirectSalesOrderFluctuationByQuarterDTOs = await DirectSalesOrderQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO = new DashboardDirector_DirectSalesOrderFluctuationDTO();
                DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters = new List<DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO DirectSalesOrderFluctuationByQuarter = new DashboardDirector_DirectSalesOrderFluctuationByQuarterDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters.Add(DirectSalesOrderFluctuationByQuarter);
                }

                foreach (var DirectSalesOrderFluctuationByQuater in DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByQuaters)
                {
                    var data = DashboardDirector_DirectSalesOrderFluctuationByQuarterDTOs.Where(x => x.Month == DirectSalesOrderFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderFluctuationByQuater.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return DashboardDirector_DirectSalesOrderFluctuationDTO;
            }
            else if (DashboardDirector_DirectSalesOrderFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {

                var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, AppUserIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_DirectSalesOrderFluctuationByYearDTOs = await DirectSalesOrderQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_DirectSalesOrderFluctuationByYearDTO
                {
                    Month = x.Key,
                    DirectSalesOrderCounter = x.Count()
                }).ToListWithNoLockAsync();

                DashboardDirector_DirectSalesOrderFluctuationDTO DashboardDirector_DirectSalesOrderFluctuationDTO = new DashboardDirector_DirectSalesOrderFluctuationDTO();
                DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears = new List<DashboardDirector_DirectSalesOrderFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_DirectSalesOrderFluctuationByYearDTO DirectSalesOrderFluctuationByYear = new DashboardDirector_DirectSalesOrderFluctuationByYearDTO
                    {
                        Month = i,
                        DirectSalesOrderCounter = 0
                    };
                    DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears.Add(DirectSalesOrderFluctuationByYear);
                }

                foreach (var DirectSalesOrderFluctuationByYear in DashboardDirector_DirectSalesOrderFluctuationDTO.DirectSalesOrderFluctuationByYears)
                {
                    var data = DashboardDirector_DirectSalesOrderFluctuationByYearDTOs.Where(x => x.Month == DirectSalesOrderFluctuationByYear.Month).FirstOrDefault();
                    if (data != null)
                        DirectSalesOrderFluctuationByYear.DirectSalesOrderCounter = data.DirectSalesOrderCounter;
                }

                return DashboardDirector_DirectSalesOrderFluctuationDTO;
            }
            return new DashboardDirector_DirectSalesOrderFluctuationDTO();
        }

        [Route(DashboardDirectorRoute.DirectSaledItemFluctuation), HttpPost]
        public async Task<DashboardDirector_SaledItemFluctuationDTO> DirectSaledItemFluctuation([FromBody] DashboardDirector_SaledItemFluctuationFilterDTO DashboardDirector_SaledItemFluctuationFilterDTO)
        {
            DateTime Now = StaticParams.DateTimeNow;
            DateTime Start = LocalStartDay(CurrentContext);
            DateTime End = LocalEndDay(CurrentContext);
            (Start, End) = TimeService.ConvertDashboardTime(DashboardDirector_SaledItemFluctuationFilterDTO.Time, CurrentContext);

            List<long> StoreIds, OrganizationIds, AppUserIds;
            (StoreIds, OrganizationIds, AppUserIds) = await DynamicFilter(DashboardDirector_SaledItemFluctuationFilterDTO);

            var StoreIdFilter = new IdFilter { In = StoreIds };
            var OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var AppUserIdFilter = new IdFilter { In = AppUserIds };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalStateIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };
            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalStateIdFilter);
            var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderIdFilter = new IdFilter { In = DirectSalesOrderIds };


            if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.HasValue == false
                || DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_MONTH.Id)
            {
                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_SaledItemFluctuationByMonthDTOs = await DirectSalesOrderTransactionQuery.GroupBy(x => x.OrderDate.Day).Select(x => new DashboardDirector_SaledItemFluctuationByMonthDTO
                {
                    Day = x.Key,
                    SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                }).ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths = new List<DashboardDirector_SaledItemFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_SaledItemFluctuationByMonthDTO SaledItemFluctuationByMonth = new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths.Add(SaledItemFluctuationByMonth);
                }

                foreach (var SaledItemFluctuationByMonth in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths)
                {
                    var data = DashboardDirector_SaledItemFluctuationByMonthDTOs.Where(x => x.Day == SaledItemFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByMonth.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_MONTH.Id)
            {

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_SaledItemFluctuationByMonthDTOs = await DirectSalesOrderTransactionQuery.GroupBy(x => x.OrderDate.Day).Select(x => new DashboardDirector_SaledItemFluctuationByMonthDTO
                {
                    Day = x.Key,
                    SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                }).ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths = new List<DashboardDirector_SaledItemFluctuationByMonthDTO>();
                var number_of_day_in_this_month = DateTime.DaysInMonth(Start.AddHours(CurrentContext.TimeZone).Year, Start.AddHours(CurrentContext.TimeZone).Month);
                for (int i = 1; i < number_of_day_in_this_month + 1; i++)
                {
                    DashboardDirector_SaledItemFluctuationByMonthDTO SaledItemFluctuationByMonth = new DashboardDirector_SaledItemFluctuationByMonthDTO
                    {
                        Day = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths.Add(SaledItemFluctuationByMonth);
                }

                foreach (var SaledItemFluctuationByMonth in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByMonths)
                {
                    var data = DashboardDirector_SaledItemFluctuationByMonthDTOs.Where(x => x.Day == SaledItemFluctuationByMonth.Day).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByMonth.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.THIS_QUARTER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.Month / 3m));

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_SaledItemFluctuationByQuarterDTOs = await DirectSalesOrderTransactionQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_SaledItemFluctuationByQuarterDTO
                {
                    Month = x.Key,
                    SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                }).ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters = new List<DashboardDirector_SaledItemFluctuationByQuarterDTO>();
                int start = 3 * (this_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_SaledItemFluctuationByQuarterDTO SaledItemFluctuationByQuarter = new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters.Add(SaledItemFluctuationByQuarter);
                }

                foreach (var SaledItemFluctuationByQuater in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters)
                {
                    var data = DashboardDirector_SaledItemFluctuationByQuarterDTOs.Where(x => x.Month == SaledItemFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByQuater.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.LAST_QUATER.Id)
            {
                var this_quarter = Convert.ToInt32(Math.Ceiling(Now.Month / 3m));
                var last_quarter = (this_quarter + 3) % 4;

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_SaledItemFluctuationByQuarterDTOs = await DirectSalesOrderTransactionQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_SaledItemFluctuationByQuarterDTO
                {
                    Month = x.Key,
                    SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                }).ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters = new List<DashboardDirector_SaledItemFluctuationByQuarterDTO>();
                int start = 3 * (last_quarter - 1) + 1;
                int end = start + 3;
                for (int i = start; i < end; i++)
                {
                    DashboardDirector_SaledItemFluctuationByQuarterDTO SaledItemFluctuationByQuarter = new DashboardDirector_SaledItemFluctuationByQuarterDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters.Add(SaledItemFluctuationByQuarter);
                }

                foreach (var SaledItemFluctuationByQuater in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByQuaters)
                {
                    var data = DashboardDirector_SaledItemFluctuationByQuarterDTOs.Where(x => x.Month == SaledItemFluctuationByQuater.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByQuater.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            else if (DashboardDirector_SaledItemFluctuationFilterDTO.Time.Equal.Value == DashboardPeriodTimeEnum.YEAR.Id)
            {

                var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.BuyerStoreId, StoreIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, AppUserIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrganizationId, OrganizationIdFilter);
                DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
                var DashboardDirector_SaledItemFluctuationByYearDTOs = await DirectSalesOrderTransactionQuery.GroupBy(x => x.OrderDate.Month).Select(x => new DashboardDirector_SaledItemFluctuationByYearDTO
                {
                    Month = x.Key,
                    SaledItemCounter = (decimal)x.Sum(x => x.RequestedQuantity)
                }).ToListWithNoLockAsync();

                DashboardDirector_SaledItemFluctuationDTO DashboardDirector_SaledItemFluctuationDTO = new DashboardDirector_SaledItemFluctuationDTO();
                DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears = new List<DashboardDirector_SaledItemFluctuationByYearDTO>();
                for (int i = 1; i <= 12; i++)
                {
                    DashboardDirector_SaledItemFluctuationByYearDTO SaledItemFluctuationByYear = new DashboardDirector_SaledItemFluctuationByYearDTO
                    {
                        Month = i,
                        SaledItemCounter = 0
                    };
                    DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears.Add(SaledItemFluctuationByYear);
                }

                foreach (var SaledItemFluctuationByYear in DashboardDirector_SaledItemFluctuationDTO.SaledItemFluctuationByYears)
                {
                    var data = DashboardDirector_SaledItemFluctuationByYearDTOs.Where(x => x.Month == SaledItemFluctuationByYear.Month).FirstOrDefault();
                    if (data != null)
                        SaledItemFluctuationByYear.SaledItemCounter = data.SaledItemCounter;
                }

                return DashboardDirector_SaledItemFluctuationDTO;
            }
            return new DashboardDirector_SaledItemFluctuationDTO();
        }

    }
}
