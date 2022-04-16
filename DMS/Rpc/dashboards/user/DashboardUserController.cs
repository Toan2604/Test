using DMS.Common;
using DMS.DWModels;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MOrganization;
using DMS.Services.MStoreChecking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public partial class DashboardUserController : SimpleController
    {
        private ITimeService TimeService;
        private DWContext DWContext;
        private DataContext DataContext;
        private IAppUserService AppUserService;
        private ICurrentContext CurrentContext;
        private IOrganizationService OrganizationService;
        private IIndirectSalesOrderService IndirectSalesOrderService;
        private IStoreCheckingService StoreCheckingService;
        public DashboardUserController(
            ITimeService TimeService,
            DWContext DWContext,
            DataContext DataContext,
            IAppUserService AppUserService,
            ICurrentContext CurrentContext,
            IOrganizationService OrganizationService,
            IIndirectSalesOrderService IndirectSalesOrderService,
            IStoreCheckingService StoreCheckingService)
        {
            this.TimeService = TimeService;
            this.DWContext = DWContext;
            this.DataContext = DataContext;
            this.AppUserService = AppUserService;
            this.CurrentContext = CurrentContext;
            this.OrganizationService = OrganizationService;
            this.IndirectSalesOrderService = IndirectSalesOrderService;
            this.StoreCheckingService = StoreCheckingService;
        }

        [Route(DashboardUserRoute.FilterListTime), HttpPost]
        public List<DashboardUser_EnumList> FilterListTime()
        {
            List<DashboardUser_EnumList> Dashboard_EnumLists = new List<DashboardUser_EnumList>();
            Dashboard_EnumLists.Add(new DashboardUser_EnumList(DashboardPeriodTimeEnum.TODAY));
            Dashboard_EnumLists.Add(new DashboardUser_EnumList(DashboardPeriodTimeEnum.THIS_WEEK));
            Dashboard_EnumLists.Add(new DashboardUser_EnumList(DashboardPeriodTimeEnum.THIS_MONTH));
            Dashboard_EnumLists.Add(new DashboardUser_EnumList(DashboardPeriodTimeEnum.LAST_MONTH));
            return Dashboard_EnumLists;
        }

        [Route(DashboardUserRoute.ListIndirectSalesOrder), HttpPost]
        public async Task<List<DashboardUser_IndirectSalesOrderDTO>> ListIndirectSalesOrder()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var query = from i in DataContext.IndirectSalesOrder
                        join r in DataContext.RequestState on i.RequestStateId equals r.Id into rq
                        from r in rq.DefaultIfEmpty()
                        where i.SaleEmployeeId == CurrentContext.UserId &&
                        i.RequestStateId != RequestStateEnum.NEW.Id
                        orderby i.OrderDate descending
                        select new DashboardUser_IndirectSalesOrderDTO
                        {
                            Id = i.Id,
                            Code = i.Code,
                            OrderDate = i.OrderDate,
                            RequestStateId = r.Id,
                            SaleEmployeeId = i.SaleEmployeeId,
                            BuyerStoreId = i.BuyerStoreId,
                            SellerStoreId = i.SellerStoreId,
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
                            },
                            SellerStore = i.SellerStore == null ? null : new DashboardUser_StoreDTO
                            {
                                Id = i.SellerStore.Id,
                                Name = i.SellerStore.Name,
                                Address = i.SellerStore.Address,
                                OwnerEmail = i.SellerStore.OwnerEmail,
                            }
                        };

            return await query.Skip(0).Take(10).ToListWithNoLockAsync();
        }

        [Route(DashboardUserRoute.ListComment), HttpPost]
        public async Task<List<DashboardUser_CommentDTO>> ListComment()
        {
            DashboardUser_CommentFilterDTO DashboardUser_CommentFilterDTO = new DashboardUser_CommentFilterDTO
            {
                AppUserId = CurrentContext.UserId
            };

            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest("/rpc/utils/discussion/list-mentioned");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(DashboardUser_CommentFilterDTO);
            try
            {
                var response = restClient.Execute<List<DashboardUser_CommentDTO>>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Data;
                }
            }
            catch
            {
                return null;
            }
            return new List<DashboardUser_CommentDTO>();
        }
        [Route(DashboardUserRoute.SalesQuantity), HttpPost]
        public async Task<long> IndirectSalesQuantity([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var StartDateFilter = new DateFilter { GreaterEqual = Start };
            var EndDateFilter = new DateFilter { LessEqual = End };
            var RequestStateIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };
            var IndirectSalesOrderTransactionQuery = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.SalesEmployeeId, UserIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, StartDateFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.OrderDate, EndDateFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => x.RequestStateId, RequestStateIdFilter);
            IndirectSalesOrderTransactionQuery = IndirectSalesOrderTransactionQuery.Where(x => !x.DeletedAt.HasValue);

            var results = await IndirectSalesOrderTransactionQuery.ToListWithNoLockAsync();
            return (long)results.Select(x => x.RequestedQuantity).DefaultIfEmpty(0).Sum();
        }

        [Route(DashboardUserRoute.StoreChecking), HttpPost]
        public async Task<long> StoreChecking([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var StartDateFilter = new DateFilter { GreaterEqual = Start };
            var EndDateFilter = new DateFilter { LessEqual = End };
            var StoreCheckingQuery = DWContext.Fact_StoreChecking.AsNoTracking();
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.SaleEmployeeId, UserIdFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, StartDateFilter);
            StoreCheckingQuery = StoreCheckingQuery.Where(x => x.CheckOutAt, EndDateFilter);
            var count = await StoreCheckingQuery.CountWithNoLockAsync();
            return count;
        }

        [Route(DashboardUserRoute.Revenue), HttpPost]
        public async Task<decimal> IndirectRevenue([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var StartDateFilter = new DateFilter { GreaterEqual = Start };
            var EndDateFilter = new DateFilter { LessEqual = End };
            var ApprovalIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };
            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, UserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, ApprovalIdFilter);

            var results = await IndirectSalesOrderQuery.ToListWithNoLockAsync();
            return results.Select(x => x.Total).DefaultIfEmpty(0).Sum();
        }

        [Route(DashboardUserRoute.StatisticIndirectSalesOrder), HttpPost]
        public async Task<long> StatisticIndirectSalesOrder([FromBody] DashboardUser_DashboardUserFilterDTO DashboardUser_DashboardUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start, End;
            (Start, End) = TimeService.ConvertDashboardTime(DashboardUser_DashboardUserFilterDTO.Time, CurrentContext);
            var UserIdFilter = new IdFilter { Equal = CurrentContext.UserId };
            var StartDateFilter = new DateFilter { GreaterEqual = Start };
            var EndDateFilter = new DateFilter { LessEqual = End };
            var ApprovalIdFilter = new IdFilter { Equal = RequestStateEnum.APPROVED.Id };
            var IndirectSalesOrderQuery = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.SaleEmployeeId, UserIdFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, StartDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.OrderDate, EndDateFilter);
            IndirectSalesOrderQuery = IndirectSalesOrderQuery.Where(x => x.RequestStateId, ApprovalIdFilter);

            var count = await IndirectSalesOrderQuery.CountWithNoLockAsync();
            return count;
        }

    }
}
