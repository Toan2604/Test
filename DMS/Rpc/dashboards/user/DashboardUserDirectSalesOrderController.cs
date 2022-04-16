using DMS.Common;
using DMS.DWModels;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MOrganization;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public partial class DashboardUserController
    {
        //private const long TODAY = 0;
        //private const long THIS_WEEK = 1;
        //private const long THIS_MONTH = 2;
        //private const long LAST_MONTH = 3;

        //private DWContext DWContext;
        //private DataContext DataContext;
        //private IAppUserService AppUserService;
        //private ICurrentContext CurrentContext;
        //private IOrganizationService OrganizationService;
        //private IIndirectSalesOrderService IndirectSalesOrderService;
        //private IStoreCheckingService StoreCheckingService;
        //public DashboardUserDirectSalesOrderController(
        //    DWContext DWContext,
        //    DataContext DataContext,
        //    IAppUserService AppUserService,
        //    ICurrentContext CurrentContext,
        //    IOrganizationService OrganizationService,
        //    IIndirectSalesOrderService IndirectSalesOrderService,
        //    IStoreCheckingService StoreCheckingService)
        //{
        //    this.DWContext = DWContext;
        //    this.DataContext = DataContext;
        //    this.AppUserService = AppUserService;
        //    this.CurrentContext = CurrentContext;
        //    this.OrganizationService = OrganizationService;
        //    this.IndirectSalesOrderService = IndirectSalesOrderService;
        //    this.StoreCheckingService = StoreCheckingService;
        //}

        //[Route(DashboardUserRoute.DirectSalesQuantity), HttpPost]
        //public async Task<long> SalesQuantity([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    DateTime Start, End;
        //    (Start, End) = ConvertDateTime(DashboardUser_DashboardUserFilterDTO);

        //    var query = from i in DWContext.Fact_DirectSalesOrderTransaction
        //                where i.SalesEmployeeId == CurrentContext.UserId &&
        //                i.OrderDate >= Start && i.OrderDate <= End &&
        //                i.RequestStateId == RequestStateEnum.APPROVED.Id
        //                select i;

        //    var results = await query.ToListWithNoLockAsync();
        //    return (long)results.Select(x => x.RequestedQuantity).DefaultIfEmpty(0).Sum();
        //}

        //[Route(DashboardUserRoute.DirectRevenue), HttpPost]
        //public async Task<decimal> Revenue([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    DateTime Start, End;
        //    (Start, End) = ConvertDateTime(DashboardUser_DashboardUserFilterDTO);

        //    var query = from i in DWContext.Fact_DirectSalesOrder
        //                where i.SaleEmployeeId == CurrentContext.UserId &&
        //                i.OrderDate >= Start && i.OrderDate <= End &&
        //                (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
        //                select i;

        //    var results = await query.ToListWithNoLockAsync();
        //    return results.Select(x => x.Total).DefaultIfEmpty(0).Sum();
        //}

        //[Route(DashboardUserRoute.StatisticDirectSalesOrder), HttpPost]
        //public async Task<long> StatisticIndirectSalesOrder([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    DateTime Start, End;
        //    (Start, End) = ConvertDateTime(DashboardUser_DashboardUserFilterDTO);

        //    var query = from i in DWContext.Fact_DirectSalesOrder
        //                where i.SaleEmployeeId == CurrentContext.UserId &&
        //                i.OrderDate >= Start && i.OrderDate <= End &&
        //                (i.GeneralApprovalStateId == GeneralApprovalStateEnum.APPROVED.Id || i.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_APPROVED.Id)
        //                select i;

        //    var count = await query.CountWithNoLockAsync();
        //    return count;
        //}

        [Route(DashboardUserRoute.ListDirectSalesOrder), HttpPost]
        public async Task<List<DashboardUser_DirectSalesOrderDTO>> ListDirectSalesOrder()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var query = from i in DataContext.DirectSalesOrder
                        join r in DataContext.RequestState on i.RequestStateId equals r.Id into rq
                        from r in rq.DefaultIfEmpty()
                        where i.SaleEmployeeId == CurrentContext.UserId &&
                        i.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id
                        orderby i.OrderDate descending
                        select new DashboardUser_DirectSalesOrderDTO
                        {
                            Id = i.Id,
                            Code = i.Code,
                            OrderDate = i.OrderDate,
                            RequestStateId = r.Id,
                            SaleEmployeeId = i.SaleEmployeeId,
                            BuyerStoreId = i.BuyerStoreId,
                            Total = i.Total,
                            SaleEmployee = i.SaleEmployee == null ? null : new DashboardUser_AppUserDTO
                            {
                                Id = i.SaleEmployee.Id,
                                DisplayName = i.SaleEmployee.DisplayName,
                                Username = i.SaleEmployee.Username,
                            },
                            RequestState = new DashboardUser_RequestStateDTO
                            {
                                Id = r.Id,
                                Code = r.Code,
                                Name = r.Name,
                            },
                            BuyerStore = i.BuyerStore == null ? null : new DashboardUser_StoreDTO
                            {
                                Id = i.BuyerStore.Id,
                                Name = i.BuyerStore.Name,
                                Address = i.BuyerStore.Address,
                                OwnerEmail = i.BuyerStore.OwnerEmail,
                            }
                        };

            return await query.Skip(0).Take(10).ToListWithNoLockAsync();
        }
        [Route(DashboardUserRoute.DirectSalesQuantity), HttpPost]
        public async Task<long> SalesQuantity([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };
            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalIdFilter);
            var DirectSalesOrderIds = await DirectSalesOrderQuery.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();
            var DirectSalesOrderIdFilter = new IdFilter { In = DirectSalesOrderIds };

            var DirectSalesOrderTransactionQuery = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, UserIdFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.OrderDate, DateFilter);
            DirectSalesOrderTransactionQuery = DirectSalesOrderTransactionQuery.Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter);
            var results = await DirectSalesOrderTransactionQuery.ToListWithNoLockAsync();
            return (long)results.Select(x => x.RequestedQuantity).DefaultIfEmpty(0).Sum();
        }

        [Route(DashboardUserRoute.DirectRevenue), HttpPost]
        public async Task<decimal> Revenue([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalIdFilter = new IdFilter { In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };

            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, UserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalIdFilter);
            var results = await DirectSalesOrderQuery.ToListWithNoLockAsync();
            return results.Select(x => x.Total).DefaultIfEmpty(0).Sum();
        }

        [Route(DashboardUserRoute.StatisticDirectSalesOrder), HttpPost]
        public async Task<long> StatisticDirectSalesOrder([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var DateFilter = new DateFilter { GreaterEqual = Start, LessEqual = End };
            var GeneralApprovalIdFilter = new IdFilter { In = new List<long>{ GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id } };
            var DirectSalesOrderQuery = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.SaleEmployeeId, UserIdFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.OrderDate, DateFilter);
            DirectSalesOrderQuery = DirectSalesOrderQuery.Where(x => x.GeneralApprovalStateId, GeneralApprovalIdFilter);
            var count = await DirectSalesOrderQuery.CountWithNoLockAsync();
            return count;
        }
    }
}
