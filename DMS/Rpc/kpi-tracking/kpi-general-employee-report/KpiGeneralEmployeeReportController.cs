using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MKpiCriteriaGeneral;
using DMS.Services.MKpiGeneral;
using DMS.Services.MKpiPeriod;
using DMS.Services.MKpiYear;
using DMS.Services.MOrganization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_employee_report
{
    public class KpiGeneralEmployeeReportController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IOrganizationService OrganizationService;
        private IAppUserService AppUserService;
        private IKpiYearService KpiYearService;
        private IKpiGeneralService KpiGeneralService;
        private IKpiPeriodService KpiPeriodService;
        private ICurrentContext CurrentContext;
        private IKpiCriteriaGeneralService KpiCriteriaGeneralService;
        public KpiGeneralEmployeeReportController(DataContext DataContext,
            DWContext DWContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IKpiGeneralService KpiGeneralService,
            IKpiYearService KpiYearService,
            IKpiPeriodService KpiPeriodService,
            IKpiCriteriaGeneralService KpiCriteriaGeneralService,
            ICurrentContext CurrentContext)
        {
            this.KpiCriteriaGeneralService = KpiCriteriaGeneralService;
            this.DWContext = DWContext;
            this.DataContext = DataContext;
            this.OrganizationService = OrganizationService;
            this.AppUserService = AppUserService;
            this.KpiGeneralService = KpiGeneralService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiYearService = KpiYearService;
            this.CurrentContext = CurrentContext;
        }

        [Route(KpiGeneralEmployeeReportRoute.ListKpiCriteriaGeneral), HttpPost]
        public async Task<List<KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO>> ListKpiCriteriaGeneral([FromBody] KpiGeneralEmployeeReport_KpiCriteriaGeneralFilterDTO KpiGeneralEmployeeReport_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter = new KpiCriteriaGeneralFilter();
            KpiCriteriaGeneralFilter.Skip = 0;
            KpiCriteriaGeneralFilter.Take = int.MaxValue;
            KpiCriteriaGeneralFilter.Selects = KpiCriteriaGeneralSelect.ALL;

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(KpiCriteriaGeneralFilter);
            List<KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO> KpiGeneralEmployeeReport_KpiCriteriaGeneralDTOs = KpiCriteriaGenerals
                .Select(x => new KpiGeneralEmployeeReport_KpiCriteriaGeneralDTO(x)).ToList();
            return KpiGeneralEmployeeReport_KpiCriteriaGeneralDTOs;
        }

        [Route(KpiGeneralEmployeeReportRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiGeneralEmployeeReport_AppUserDTO>> FilterListAppUser([FromBody] KpiGeneralEmployeeReport_AppUserFilterDTO KpiGeneralEmployeeReport_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = KpiGeneralEmployeeReport_AppUserFilterDTO.Id;
            AppUserFilter.OrganizationId = KpiGeneralEmployeeReport_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Username = KpiGeneralEmployeeReport_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneralEmployeeReport_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);


            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiGeneralEmployeeReport_AppUserDTO> KpiGeneralEmployeeReport_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneralEmployeeReport_AppUserDTO(x)).ToList();
            return KpiGeneralEmployeeReport_AppUserDTOs;
        }

        [Route(KpiGeneralEmployeeReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiGeneralEmployeeReport_OrganizationDTO>> FilterListOrganization([FromBody] KpiGeneralEmployeeReport_OrganizationFilterDTO KpiGeneralEmployeeReport_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<KpiGeneralEmployeeReport_OrganizationDTO> KpiGeneralEmployeeReport_OrganizationDTOs = Organizations
                .Select(x => new KpiGeneralEmployeeReport_OrganizationDTO(x)).ToList();
            return KpiGeneralEmployeeReport_OrganizationDTOs;
        }
        [Route(KpiGeneralEmployeeReportRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiGeneralEmployeeReport_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiGeneralEmployeeReport_KpiPeriodFilterDTO KpiGeneralEmployeeReport_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiGeneralEmployeeReport_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiGeneralEmployeeReport_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiGeneralEmployeeReport_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiGeneralEmployeeReport_KpiPeriodDTO> KpiGeneralEmployeeReport_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiGeneralEmployeeReport_KpiPeriodDTO(x)).ToList();
            return KpiGeneralEmployeeReport_KpiPeriodDTOs;
        }

        [Route(KpiGeneralEmployeeReportRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiGeneralEmployeeReport_KpiYearDTO>> FilterListKpiYear([FromBody] KpiGeneralEmployeeReport_KpiYearFilterDTO KpiGeneralEmployeeReport_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiGeneralEmployeeReport_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiGeneralEmployeeReport_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiGeneralEmployeeReport_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiGeneralEmployeeReport_KpiYearDTO> KpiGeneralEmployeeReport_KpiYearDTOs = KpiYears
                .Select(x => new KpiGeneralEmployeeReport_KpiYearDTO(x)).ToList();
            return KpiGeneralEmployeeReport_KpiYearDTOs;
        }

        [Route(KpiGeneralEmployeeReportRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            long? SaleEmployeeId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId.Equal;
            if (SaleEmployeeId == null)
                return 0;
            long? KpiPeriodId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiPeriodId?.Equal;
            long? KpiYearId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiYearId?.Equal;


            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            var query_detail = from a in DataContext.KpiGeneralContentKpiPeriodMapping
                               join b in DataContext.KpiGeneralContent on a.KpiGeneralContentId equals b.Id
                               join c in DataContext.KpiGeneral on b.KpiGeneralId equals c.Id
                               where OrganizationIds.Contains(c.OrganizationId) &&
                               c.EmployeeId == SaleEmployeeId.Value &&
                               (KpiYearId == null || c.KpiYearId == KpiYearId) &&
                               (KpiPeriodId == null || a.KpiPeriodId == KpiPeriodId) &&
                               c.StatusId == StatusEnum.ACTIVE.Id &&
                               c.DeletedAt == null
                               select new
                               {
                                   SaleEmployeeId = c.EmployeeId,
                                   KpiYearId = c.KpiYearId,
                                   KpiPeriodId = a.KpiPeriodId,
                               };
            return await query_detail.Distinct().CountWithNoLockAsync();
        }

        public async Task<ActionResult<List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>>> List_Old([FromBody] KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId == null)
                return BadRequest(new { message = "Chưa chọn nhân viên" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId.Equal.Value;
            long? KpiPeriodId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiPeriodId?.Equal;
            long? KpiYearId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiYearId?.Equal;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate != null)
            {
                if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual != null)
                {
                    var StartDateFilter = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual.Value.AddHours(0 - CurrentContext.TimeZone);
                    StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                }
                if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.LessEqual != null)
                {
                    var EndDateFilter = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.LessEqual.Value.AddDays(1).AddSeconds(-1).AddHours(0 - CurrentContext.TimeZone);
                    EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                }
            }

            var KpiGeneralId = await DataContext.KpiGeneral
                .Where(x => x.EmployeeId == SaleEmployeeId.Value &&
                (KpiYearId.HasValue == false || x.KpiYearId == KpiYearId.Value) &&
                x.StatusId == StatusEnum.ACTIVE.Id &&
                (KpiYearId == null || x.KpiYearId == KpiYearId) &&
                x.DeletedAt == null)
                .Select(x => x.Id)
                .FirstOrDefaultWithNoLockAsync();
            var KpiGeneral = await KpiGeneralService.Get(KpiGeneralId);

            if (KpiGeneral == null)
                return new List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>();
            var KpiPeriodIds = KpiGeneral.KpiGeneralContents
                .SelectMany(x => x.KpiGeneralContentKpiPeriodMappings)
                .Where(x => KpiPeriodId.HasValue == false || x.KpiPeriodId == KpiPeriodId.Value)
                .Select(x => x.KpiPeriodId)
                .Distinct()
                .Skip(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Skip)
                .Take(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Take)
                .ToList();
            var KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
                Id = new IdFilter { In = KpiPeriodIds },
                OrderBy = KpiPeriodOrder.Id,
                OrderType = OrderType.ASC
            });

            var KpiGeneralContentKpiPeriodMappings = KpiGeneral.KpiGeneralContents
                .SelectMany(x => x.KpiGeneralContentKpiPeriodMappings)
                .Where(x => KpiPeriodIds.Contains(x.KpiPeriodId))
                .ToList();
            List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs = new List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>();
            foreach (KpiPeriod KpiPeriod in KpiPeriods)
            {
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO
                    = new KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO();
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiPeriodId = KpiPeriod.Id;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiPeriodName = KpiPeriod.Name;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiYearId = KpiGeneral.KpiYearId;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiYearName = KpiGeneral.KpiYear.Name;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.SaleEmployeeId = SaleEmployeeId.Value;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs.Add(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO);
            }
            var IndirectSalesOrderDAOs = await DataContext.IndirectSalesOrder
                .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                x.OrderDate >= StartDate && x.OrderDate <= EndDate &&
                x.RequestStateId == RequestStateEnum.APPROVED.Id)
                .Select(x => new IndirectSalesOrderDAO
                {
                    Id = x.Id,
                    Total = x.Total,
                    SaleEmployeeId = x.SaleEmployeeId,
                    OrderDate = x.OrderDate,
                    BuyerStoreId = x.BuyerStoreId,
                    BuyerStore = x.BuyerStore == null ? null : new StoreDAO
                    {
                        StoreType = x.BuyerStore.StoreType == null ? null : new StoreTypeDAO
                        {
                            Code = x.BuyerStore.StoreType.Code
                        }
                    },
                    IndirectSalesOrderContents = x.IndirectSalesOrderContents.Select(c => new IndirectSalesOrderContentDAO
                    {
                        IndirectSalesOrderId = x.Id,
                        RequestedQuantity = c.RequestedQuantity,
                        ItemId = c.ItemId,
                    }).ToList(),
                })
                .ToListWithNoLockAsync();

            var StoreCheckingDAOs = await DWContext.Fact_StoreChecking
                .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                x.CheckOutAt.HasValue && x.CheckOutAt.Value >= StartDate && x.CheckOutAt.Value <= EndDate)
                .Select(x => new StoreCheckingDAO
                {
                    StoreId = x.StoreId,
                    SaleEmployeeId = x.SaleEmployeeId,
                    Id = x.StoreCheckingId,
                    CheckInAt = x.CheckInAt,
                    CheckOutAt = x.CheckOutAt
                })
                .ToListWithNoLockAsync();

            var StoreDAOs = await DataContext.Store
                    .Where(x => x.CreatorId == SaleEmployeeId &&
                    x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                    .Select(x => new StoreDAO
                    {
                        CreatorId = x.CreatorId,
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        StoreType = x.StoreType == null ? null : new StoreTypeDAO
                        {
                            Code = x.StoreType.Code
                        }
                    })
                    .ToListWithNoLockAsync();

            var ProblemDAOs = await DataContext.Problem
                 .Where(x => x.CreatorId == SaleEmployeeId &&
                x.NoteAt >= StartDate && x.NoteAt <= EndDate)
                 .ToListWithNoLockAsync();
            var StoreImages = await DWContext.Fact_Image
                .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                x.ShootingAt >= StartDate && x.ShootingAt <= EndDate)
                .ToListWithNoLockAsync();
            foreach (var Period in KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs)
            {
                foreach (var KpiPeriod in KpiPeriodEnum.KpiPeriodEnumList)
                {
                    if (KpiPeriod.Id == Period.KpiPeriodId)
                        Period.KpiPeriodName = KpiPeriod.Name;
                }
                DateTime Start, End;
                (Start, End) = DateTimeConvert(Period.KpiPeriodId, Period.KpiYearId);

                //lấy tất cả đơn hàng được đặt trong kì đang xét
                var IndirectSalesOrders = IndirectSalesOrderDAOs
                    .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                    x.OrderDate >= Start && x.OrderDate <= End)
                    .ToList();

                //var DirectSalesOrders = DirectSalesOrderDAOs
                //    .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                //    x.OrderDate >= Start && x.OrderDate <= End)
                //    .ToList();

                //lấy tất cả lượt checking trong kì đang xét
                var StoreCheckings = StoreCheckingDAOs
                    .Where(x => x.SaleEmployeeId == Period.SaleEmployeeId &&
                    x.CheckOutAt.HasValue && x.CheckOutAt.Value >= Start && x.CheckOutAt.Value <= End)
                    .ToList();

                #region các chỉ tiêu tạm ẩn
                //#region Số đơn hàng gián tiếp
                ////kế hoạch
                //Period.TotalIndirectOrdersPLanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_ORDER.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.TotalIndirectOrdersPLanned.HasValue)
                //{
                //    //thực hiện
                //    Period.TotalIndirectOrders = Period.TotalIndirectOrdersPLanned == null ? null : (decimal?)IndirectSalesOrders.Count();
                //    //tỉ lệ
                //    Period.TotalIndirectOrdersRatio = Period.TotalIndirectOrdersPLanned == null || Period.TotalIndirectOrders == null || Period.TotalIndirectOrdersPLanned == 0 ? null :
                //        (decimal?)
                //        Math.Round((Period.TotalIndirectOrders.Value / Period.TotalIndirectOrdersPLanned.Value) * 100, 2);
                //}
                //#endregion

                //#region Tổng sản lượng theo đơn gián tiếp
                ////kế hoạch
                //Period.TotalIndirectQuantityPlanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.TotalIndirectQuantityPlanned.HasValue)
                //{
                //    //thực hiện
                //    Period.TotalIndirectQuantity = 0;
                //    foreach (var IndirectSalesOrder in IndirectSalesOrders)
                //    {
                //        foreach (var content in IndirectSalesOrder.IndirectSalesOrderContents)
                //        {
                //            Period.TotalIndirectQuantity += content.RequestedQuantity;
                //        }
                //    }
                //    //tỉ lệ
                //    Period.TotalIndirectQuantityRatio = Period.TotalIndirectQuantityPlanned == null || Period.TotalIndirectQuantity == null || Period.TotalIndirectQuantityPlanned == 0 ? null :
                //        (decimal?)
                //        Math.Round((Period.TotalIndirectQuantity.Value / Period.TotalIndirectQuantityPlanned.Value) * 100, 2);
                //}
                //#endregion

                //#region SKU/Đơn hàng gián tiếp
                ////kế hoạch
                //Period.SkuIndirectOrderPlanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.SKU_INDIRECT_SALES_ORDER.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.SkuIndirectOrderPlanned.HasValue)
                //{
                //    //thực hiện
                //    Period.SKUIndirectItems = new List<long>();
                //    foreach (var IndirectSalesOrder in IndirectSalesOrders)
                //    {
                //        var itemIds = IndirectSalesOrder.IndirectSalesOrderContents.Select(x => x.ItemId).Distinct().ToList();
                //        Period.SKUIndirectItems.AddRange(itemIds);
                //    }
                //    Period.SkuIndirectOrder = IndirectSalesOrders.Count() == 0 || Period.SKUIndirectItems == null ? 0 :
                //        Math.Round((decimal)Period.SKUIndirectItems.Count() / IndirectSalesOrders.Count(), 2);
                //    //tỉ lệ
                //    Period.SkuIndirectOrderRatio = Period.SkuIndirectOrderPlanned == null || Period.SkuIndirectOrder == null || Period.SkuIndirectOrderPlanned == 0 ? 0 :
                //        Math.Round((Period.SkuIndirectOrder.Value / Period.SkuIndirectOrderPlanned.Value) * 100, 2);
                //}

                //#endregion

                //#region Số đơn hàng trực tiếp
                ////kế hoạch
                //Period.TotalDirectOrdersPLanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.TotalDirectOrdersPLanned.HasValue)
                //{
                //    //thực hiện
                //    Period.TotalDirectOrders = Period.TotalDirectOrdersPLanned == null ? null : (decimal?)DirectSalesOrders.Count();
                //    //tỉ lệ
                //    Period.TotalDirectOrdersRatio = Period.TotalDirectOrdersPLanned == null || Period.TotalDirectOrders == null || Period.TotalDirectOrdersPLanned == 0 ? null :
                //        (decimal?)
                //        Math.Round((Period.TotalDirectOrders.Value / Period.TotalDirectOrdersPLanned.Value) * 100, 2);
                //}
                //#endregion

                //#region Tổng sản lượng theo đơn trực tiếp
                ////kế hoạch
                //Period.TotalDirectQuantityPlanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.TotalDirectQuantityPlanned.HasValue)
                //{
                //    //thực hiện
                //    Period.TotalDirectQuantity = 0;
                //    foreach (var DirectSalesOrder in DirectSalesOrders)
                //    {
                //        foreach (var content in DirectSalesOrder.DirectSalesOrderContents)
                //        {
                //            Period.TotalDirectQuantity += content.RequestedQuantity;
                //        }
                //    }
                //    //tỉ lệ
                //    Period.TotalDirectQuantityRatio = Period.TotalDirectQuantityPlanned == null || Period.TotalDirectQuantity == null || Period.TotalDirectQuantityPlanned == 0 ? null :
                //        (decimal?)
                //        Math.Round((Period.TotalDirectQuantity.Value / Period.TotalDirectQuantityPlanned.Value) * 100, 2);
                //}
                //#endregion

                //#region Doanh thu theo đơn hàng trực tiếp
                ////kế hoạch
                //Period.TotalDirectSalesAmountPlanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                //if (Period.TotalDirectSalesAmountPlanned.HasValue)
                //{
                //    //thực hiện
                //    Period.TotalDirectSalesAmount = Period.TotalDirectSalesAmountPlanned == null ? null : (decimal?)DirectSalesOrders.Sum(x => x.Total);
                //    //tỉ lệ
                //    Period.TotalDirectSalesAmountRatio = Period.TotalDirectSalesAmountPlanned == null || Period.TotalDirectSalesAmount == null || Period.TotalDirectSalesAmountPlanned == 0 ? null :
                //        (decimal?)
                //        Math.Round((Period.TotalDirectSalesAmount.Value / Period.TotalDirectSalesAmountPlanned.Value) * 100, 2);
                //}
                //#endregion

                //#region SKU/Đơn hàng trực tiếp
                ////kế hoạch
                //Period.SkuDirectOrderPlanned = KpiGeneralContentKpiPeriodMappings
                //        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                //        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.SKU_DIRECT_SALES_ORDER.Id)
                //        .Select(x => x.Value)
                //        .FirstOrDefault();
                ////thực hiện
                //if (Period.SkuDirectOrderPlanned.HasValue)
                //{
                //    Period.SKUDirectItems = new List<long>();
                //    foreach (var DirectSalesOrder in DirectSalesOrders)
                //    {
                //        var itemIds = DirectSalesOrder.DirectSalesOrderContents.Select(x => x.ItemId).Distinct().ToList();
                //        Period.SKUDirectItems.AddRange(itemIds);
                //    }
                //    Period.SkuDirectOrder = DirectSalesOrders.Count() == 0 || Period.SKUDirectItems == null ? 0 :
                //        Math.Round((decimal)Period.SKUDirectItems.Count() / DirectSalesOrders.Count(), 2);
                //    //tỉ lệ
                //    Period.SkuDirectOrderRatio = Period.SkuDirectOrderPlanned == null || Period.SkuDirectOrder == null || Period.SkuDirectOrderPlanned == 0 ? 0 :
                //        Math.Round((Period.SkuDirectOrder.Value / Period.SkuDirectOrderPlanned.Value) * 100, 2);
                //}
                //#endregion
                #endregion

                #region Tổng doanh thu đơn hàng
                //kế hoạch
                Period.TotalIndirectSalesAmountPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalIndirectSalesAmountPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalIndirectSalesAmount = Period.TotalIndirectSalesAmountPlanned == null ? null : (decimal?)IndirectSalesOrders.Sum(x => x.Total);
                    //tỉ lệ
                    Period.TotalIndirectSalesAmountRatio = Period.TotalIndirectSalesAmountPlanned == null || Period.TotalIndirectSalesAmount == null || Period.TotalIndirectSalesAmountPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalIndirectSalesAmount.Value / Period.TotalIndirectSalesAmountPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý ghé thăm
                //kế hoạch
                Period.StoresVisitedPlanned = KpiGeneralContentKpiPeriodMappings
                       .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                       x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.STORE_VISITED.Id)
                       .Select(x => x.Value)
                       .FirstOrDefault();
                //thực hiện
                if (Period.StoresVisitedPlanned.HasValue)
                {
                    Period.StoreIds = new HashSet<long>();
                    foreach (var StoreChecking in StoreCheckings)
                    {
                        Period.StoreIds.Add(StoreChecking.StoreId);
                    }
                    if (Period.StoresVisitedPlanned == 0)
                        Period.StoreIds = null;
                    //tỉ lệ
                    Period.StoresVisitedRatio = Period.StoresVisitedPlanned == null || Period.StoresVisited == null || Period.StoresVisitedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.StoresVisited.Value / Period.StoresVisitedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng số đại lý mở mới
                //kế hoạch
                Period.NewStoreCreatedPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NewStoreCreatedPlanned.HasValue)
                {
                    //thực hiện
                    Period.NewStoreCreated = Period.NewStoreCreatedPlanned == null ? null :
                        (decimal?)
                        StoreDAOs
                        .Where(sc => sc.CreatorId == Period.SaleEmployeeId &&
                        sc.CreatedAt >= Start && sc.CreatedAt <= End)
                        .Count();
                    //tỉ lệ
                    Period.NewStoreCreatedRatio = Period.NewStoreCreatedPlanned == null || Period.NewStoreCreated == null || Period.NewStoreCreatedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NewStoreCreated.Value / Period.NewStoreCreatedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng số lượt ghé thăm
                //kế hoạch
                Period.NumberOfStoreVisitsPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NumberOfStoreVisitsPlanned.HasValue)
                {
                    //thực hiện
                    Period.NumberOfStoreVisits = Period.NumberOfStoreVisitsPlanned == null ? null :
                        (decimal?)
                        StoreCheckings.Count();
                    //tỉ lệ
                    Period.NumberOfStoreVisitsRatio = Period.NumberOfStoreVisitsPlanned == null || Period.NumberOfStoreVisits == null || Period.NumberOfStoreVisitsPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NumberOfStoreVisits.Value / Period.NumberOfStoreVisitsPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2 Trọng điểm
                //kế hoạch
                Period.RevenueC2TDPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2TDPlanned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2TD = Period.RevenueC2TDPlanned == null ?
                    null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2TD).Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2TDRatio = Period.RevenueC2TDPlanned == null || Period.RevenueC2TD == null || Period.RevenueC2TDPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2TD.Value / Period.RevenueC2TDPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2 Siêu lớn
                //kế hoạch
                Period.RevenueC2SLPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2SLPlanned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2SL = Period.RevenueC2SLPlanned == null ?
                    null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2SL).Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2SLRatio = Period.RevenueC2SLPlanned == null || Period.RevenueC2SL == null || Period.RevenueC2SLPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2SL.Value / Period.RevenueC2SLPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2
                //kế hoạch
                Period.RevenueC2Planned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2Planned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2 = Period.RevenueC2Planned == null ?
                    null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2).Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2Ratio = Period.RevenueC2Planned == null || Period.RevenueC2 == null || Period.RevenueC2Planned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2.Value / Period.RevenueC2Planned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý trọng điểm mở mới
                //kế hoạch
                Period.NewStoreC2CreatedPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NewStoreC2CreatedPlanned.HasValue)
                {
                    //thực hiện
                    Period.NewStoreC2Created = Period.NewStoreC2CreatedPlanned == null ? null :
                        (decimal?)
                        StoreDAOs
                        .Where(sc => sc.CreatorId == Period.SaleEmployeeId &&
                        sc.CreatedAt >= Start && sc.CreatedAt <= End)
                        .Where(x => x.StoreType.Code == StaticParams.C2TD)
                        .Count();
                    //tỉ lệ
                    Period.NewStoreC2CreatedRatio = Period.NewStoreC2CreatedPlanned == null || Period.NewStoreC2Created == null || Period.NewStoreC2CreatedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NewStoreC2Created.Value / Period.NewStoreC2CreatedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số thông tin phản ảnh
                //kế hoạch
                Period.TotalProblemPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalProblemPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalProblem = Period.TotalProblemPlanned == null ?
                    null : (decimal?)ProblemDAOs.Where(x => x.CreatorId == Period.SaleEmployeeId &&
                    Start <= x.NoteAt && x.NoteAt <= End)
                    .Count();
                    //tỉ lệ
                    Period.TotalProblemRatio = Period.TotalProblemPlanned == null || Period.TotalProblem == null || Period.TotalProblemPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalProblem.Value / Period.TotalProblemPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số hình ảnh chụp
                //kế hoạch
                Period.TotalImagePlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalImagePlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalImage = Period.TotalImagePlanned == null ?
                    null : (decimal?)StoreImages.Where(x => x.SaleEmployeeId == Period.SaleEmployeeId &&
                    Start <= x.ShootingAt && x.ShootingAt <= End)
                    .Count();
                    //tỉ lệ
                    Period.TotalImageRatio = Period.TotalImagePlanned == null || Period.TotalImage == null || Period.TotalImagePlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalImage.Value / Period.TotalImagePlanned.Value) * 100, 2);
                }
                #endregion
            };

            return KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs.OrderBy(x => x.KpiPeriodId).ThenBy(x => x.KpiYearId).ToList();
        }

        [Route(KpiGeneralEmployeeReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId == null)
                return BadRequest(new { message = "Chưa chọn nhân viên" });

            DateTime StartDate, EndDate;
            long? KpiPeriodId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiPeriodId?.Equal;
            long? KpiYearId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiYearId?.Equal;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.HasValue)
            {
                StartDate = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual == null ? StartDate : KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual.Value;
                EndDate = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual == null ? EndDate : KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.LessEqual.Value;
            }

            KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Skip = 0;
            KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Take = int.MaxValue;
            List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO> KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs = (await List(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO)).Value;
            var SaleEmployee = await AppUserService.Get(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId.Equal.Value);
            long stt = 1;
            foreach (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO in KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs)
            {
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.STT = stt;
                stt++;
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            List<KpiGeneralEmployeeReport_ExportDTO> KpiGeneralEmployeeReport_ExportDTOs = await ConvertReportToExportDTO(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs);
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.ALL,
            });


            MemoryStream output = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(output))
            {
                #region Tieu de bao cao
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Báo cáo KPI theo nhân viên");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 11;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A3"].Value = "BÁO CÁO KPI THEO NHÂN VIÊN";
                ws.Cells["A3:U3"].Merge = true;
                ws.Cells["A3:U3"].Style.Font.Size = 20;
                ws.Cells["A3:U3"].Style.Font.Bold = true;
                ws.Cells["A3:U3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["I4:L5"].Style.Font.Size = 11;
                ws.Cells["I4:L5"].Style.Font.Italic = true;

                ws.Cells["I4"].Value = "Mã nhân viên";
                ws.Cells["I4"].Style.Font.Bold = true;
                ws.Cells["J4"].Value = SaleEmployee.Username;

                ws.Cells["K4"].Value = "Tên nhân viên";
                ws.Cells["K4"].Style.Font.Bold = true;
                ws.Cells["L4"].Value = SaleEmployee.DisplayName;

                ws.Cells["I5"].Value = "Từ ngày";
                ws.Cells["J5"].Value = StartDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                ws.Cells["K5"].Value = "Đến ngày";
                ws.Cells["L5"].Value = EndDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");

                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "STT",
                    "Kỳ KPI",
                    "Năm KPI"
                };
                List<string> headerLine2 = new List<string>
                {
                    "", "", ""
                };

                KpiCriteriaGenerals = KpiCriteriaGenerals.OrderBy(x => x.Id).ToList();
                for (int i = 0; i < KpiCriteriaGenerals.Count; i++)
                {
                    headers.Add(KpiCriteriaGenerals[i].Name);
                    headers.Add("");
                    headers.Add(""); // thêm ô trống vào để merge 3 ô dòng trên lại
                    headerLine2.Add("Kế hoạch");
                    headerLine2.Add("Thực hiện");
                    headerLine2.Add("Tỷ lệ(%)");
                }
                int endColumnNumber = headers.Count;
                string endColumnString = Char.ConvertFromUtf32(endColumnNumber + 64);
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64);

                string headerRange = $"A7:" + $"{endColumnString}8";
                List<string[]> Header = new List<string[]> { headers.ToArray(), headerLine2.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Size = 11;
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ws.Cells["A7:A8"].Merge = true;
                ws.Cells["B7:B8"].Merge = true;
                ws.Cells["C7:C8"].Merge = true;

                for (int i = 1; i <= endColumnNumber; i += 3)
                {
                    if (i == 1) continue;

                    string startMerge = Char.ConvertFromUtf32(i + 64);
                    if (i > 26) startMerge = Char.ConvertFromUtf32(i / 26 + 64) + Char.ConvertFromUtf32(i % 26 + 64);
                    string endMerge = Char.ConvertFromUtf32(i + 2 + 64);
                    if (i + 2 > 26) endMerge = Char.ConvertFromUtf32((i + 2) / 26 + 64) + Char.ConvertFromUtf32((i + 2) % 26 + 64);
                    ws.Cells[$"{startMerge}7:{endMerge}7"].Merge = true; // merge cai dong tren cua header
                }
                string headerRange2 = $"A8:" + $"{endColumnString}8";

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells[headerRange2].AutoFitColumns();
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                #endregion

                #region Du lieu bao cao
                int startRow = 9;
                int startColumn = 1;
                int endColumn = startColumn;
                int endRow = startRow;
                if (KpiGeneralEmployeeReport_ExportDTOs != null && KpiGeneralEmployeeReport_ExportDTOs.Count > 0)
                {
                    #region Cột STT
                    List<Object[]> SttData = new List<object[]>();
                    endRow = startRow;
                    foreach (var KpiGeneralEmployeeReport_ExportDTO in KpiGeneralEmployeeReport_ExportDTOs)
                    {
                        SttData.Add(new object[]
                        {
                                KpiGeneralEmployeeReport_ExportDTO.STT,
                        });
                        endRow++;
                    }
                    string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                    if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                    string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                    if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                    ws.Cells[$"{startColumnString}{startRow}:{currentColumnString}{endRow - 1}"].LoadFromArrays(SttData);
                    endColumn += 1;
                    startColumn = endColumn;
                    #endregion

                    #region Cột Kỳ KPI, năm KPI
                    List<Object[]> KpiData = new List<object[]>();
                    endRow = startRow;
                    foreach (var KpiGeneralEmployeeReport_ExportDTO in KpiGeneralEmployeeReport_ExportDTOs)
                    {
                        KpiData.Add(new object[]
                        {
                                KpiGeneralEmployeeReport_ExportDTO.KpiPeriod,
                                KpiGeneralEmployeeReport_ExportDTO.KpiYear,
                        });
                        endRow++;
                    }
                    startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                    if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                    currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                    if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                    ws.Cells[$"{startColumnString}{startRow}:{currentColumnString}{endRow - 1}"].LoadFromArrays(KpiData);
                    endColumn += 2;
                    startColumn = endColumn;
                    #endregion

                    #region Các cột giá trị
                    for (int i = 0; i < KpiCriteriaGenerals.Count; i++)
                    {
                        List<Object[]> ValueData = new List<object[]>();
                        endRow = startRow; // gán lại dòng bắt đầu
                        foreach (var KpiGeneralEmployeeReport_ExportDTO in KpiGeneralEmployeeReport_ExportDTOs)
                        {
                            KpiGeneralEmployeeReport_CriteriaContentDTO criteriaContent = KpiGeneralEmployeeReport_ExportDTO
                                .CriteriaContents.Where(x => x.CriteriaId == KpiCriteriaGenerals[i].Id).FirstOrDefault(); // Lấy ra giá trị của criteria tương ứng
                            ValueData.Add(new object[]
                            {
                                criteriaContent.Planned,
                                criteriaContent.Result,
                                criteriaContent.Ratio,
                            });
                            endRow++;
                        }
                        startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);


                        ws.Cells[$"{startColumnString}{startRow}:{currentColumnString}{endRow - 1}"].LoadFromArrays(ValueData); // fill dữ liệu

                        endColumn += 3; // Chiếm 3 cột cho mỗi criteria
                        startColumn = endColumn;
                    }
                    #endregion
                    //foreach (var KpiGeneralEmployeeReport_ExportDTO in KpiGeneralEmployeeReport_ExportDTOs)
                    //{
                    //    List<KpiGeneralEmployeeReport_CriteriaContentDTO> criteriaContents = KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.OrderBy(x => x.CriteriaId).ToList();
                    //    List<string> lineData = new List<string>
                    //    {
                    //        KpiGeneralEmployeeReport_ExportDTO.STT.ToString(),
                    //        KpiGeneralEmployeeReport_ExportDTO.KpiPeriod,
                    //        KpiGeneralEmployeeReport_ExportDTO.KpiYear
                    //    };
                    //    for (int i = 0; i < criteriaContents.Count; i++)
                    //    {
                    //        lineData.Add(criteriaContents[i].Planned);
                    //        lineData.Add(criteriaContents[i].Result);
                    //        lineData.Add(criteriaContents[i].Ratio);
                    //    }
                    //    Contents.Add(lineData.ToArray());

                    //    currentRow++; // Gán lại start row cho org tiếp theo
                    //}
                    //ws.Cells[$"A{startRow}:{endColumnString}{currentRow - 1}"].LoadFromArrays(Contents);

                }
                ws.Cells[$"D9:{endColumnString}{endRow - 1}"].Style.Numberformat.Format = "#,##0.00"; // format number column value
                // All borders
                ws.Cells[$"A7:{endColumnString}{endRow - 1}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow - 1}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow - 1}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A7:{endColumnString}{endRow - 1}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion

                excel.Save();
            }
            return File(output.ToArray(), "application/octet-stream", "KpiGeneralEmployeeReport.xlsx");
        }

        private async Task<List<KpiGeneralEmployeeReport_ExportDTO>> ConvertReportToExportDTO(List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO> KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs)
        {
            List<KpiGeneralEmployeeReport_ExportDTO> KpiGeneralEmployeeReport_ExportDTOs = new List<KpiGeneralEmployeeReport_ExportDTO>();
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.Id,
            });
            List<long> KpiCriteriaGeneralIds = KpiCriteriaGenerals.Select(x => x.Id).ToList();

            foreach (var KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO in KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs)
            {
                KpiGeneralEmployeeReport_ExportDTO KpiGeneralEmployeeReport_ExportDTO = new KpiGeneralEmployeeReport_ExportDTO();
                KpiGeneralEmployeeReport_ExportDTO.STT = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.STT;
                KpiGeneralEmployeeReport_ExportDTO.KpiPeriod = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiPeriodName;
                KpiGeneralEmployeeReport_ExportDTO.KpiYear = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiYearName;
                KpiGeneralEmployeeReport_ExportDTO.CriteriaContents = new List<KpiGeneralEmployeeReport_CriteriaContentDTO>();


                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesAmountPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesAmount,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesAmountRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2_TD.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2TDPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2TD,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2TDRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2_SL.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2SLPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2SL,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2SLRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2Planned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.RevenueC2Ratio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalProblemPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalProblem,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalProblemRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_IMAGE.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalImagePlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalImage,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalImageRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.STORE_VISITED.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.STORE_VISITED.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.STORE_VISITED.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.StoresVisitedPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.StoresVisited,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.StoresVisitedRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreCreatedPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreCreated,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreCreatedRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreC2CreatedPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreC2Created,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NewStoreC2CreatedRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NumberOfStoreVisitsPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NumberOfStoreVisits,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.NumberOfStoreVisitsRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectOrdersPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectOrders,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectOrdersRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesAmountPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesAmount,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesAmountRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesQuantityPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesQuantity,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalDirectSalesQuantityRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesQuantityPlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesQuantity,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.TotalIndirectSalesQuantityRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.DirectSalesBuyerStorePlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.DirectSalesBuyerStore,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.DirectSalesBuyerStoreRatio
                    });
                }
                if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id))
                {
                    KpiGeneralEmployeeReport_ExportDTO.CriteriaContents.Add(new KpiGeneralEmployeeReport_CriteriaContentDTO
                    {
                        CriteriaId = KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id,
                        CriteriaName = KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Name,
                        Planned = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.IndirectSalesBuyerStorePlanned,
                        Result = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.IndirectSalesBuyerStore,
                        Ratio = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.IndirectSalesBuyerStoreRatio
                    });
                }

                KpiGeneralEmployeeReport_ExportDTOs.Add(KpiGeneralEmployeeReport_ExportDTO);
            }

            return KpiGeneralEmployeeReport_ExportDTOs;
        }

        private Tuple<DateTime, DateTime> DateTimeConvert(long? KpiPeriodId, long? KpiYearId)
        {
            DateTime startDate = StaticParams.DateTimeNow;
            DateTime endDate = StaticParams.DateTimeNow;
            if (KpiYearId == null) KpiYearId = startDate.Year;
            if (KpiPeriodId == null)
            {
                //startDate = new DateTime(2019, 1, 1);
                //endDate = new DateTime(2040, 12, 12);
                startDate = new DateTime((int)KpiYearId, 1, 1);
                endDate = startDate.AddYears(1).AddSeconds(-1);
            }
            else
            if (KpiPeriodId <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
            {
                startDate = new DateTime((int)KpiYearId, (int)(KpiPeriodId % 100), 1);
                endDate = startDate.AddMonths(1).AddSeconds(-1);
            }
            else
            {
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER02.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 4, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER03.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 7, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 10, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_YEAR01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                }
            }

            return Tuple.Create(startDate.AddHours(0 - CurrentContext.TimeZone), endDate.AddHours(0 - CurrentContext.TimeZone));
        }

        [Route(KpiGeneralEmployeeReportRoute.List), HttpPost]
        public async Task<ActionResult<List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>>> List([FromBody] KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId == null)
                return BadRequest(new { message = "Chưa chọn nhân viên" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.AppUserId.Equal.Value;
            long? KpiPeriodId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiPeriodId?.Equal;
            long? KpiYearId = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.KpiYearId?.Equal;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate != null)
            {
                if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual != null)
                {
                    var StartDateFilter = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.GreaterEqual.Value;
                    StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                }
                if (KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.LessEqual != null)
                {
                    var EndDateFilter = KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.OrderDate.LessEqual.Value;
                    EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                }
            }

            IdFilter SaleEmployeeIdFilter = new IdFilter() { Equal = SaleEmployeeId };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = StartDate, LessEqual = EndDate };

            var KpiGeneralId = await DataContext.KpiGeneral
                .Where(x => x.EmployeeId == SaleEmployeeId.Value &&
                (KpiYearId.HasValue == false || x.KpiYearId == KpiYearId.Value) &&
                x.StatusId == StatusEnum.ACTIVE.Id &&
                (KpiYearId == null || x.KpiYearId == KpiYearId) &&
                x.DeletedAt == null)
                .Select(x => x.Id)
                .FirstOrDefaultWithNoLockAsync();
            var KpiGeneral = await KpiGeneralService.Get(KpiGeneralId);

            if (KpiGeneral == null)
                return new List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>();
            var KpiPeriodIds = KpiGeneral.KpiGeneralContents
                .SelectMany(x => x.KpiGeneralContentKpiPeriodMappings)
                .Where(x => KpiPeriodId.HasValue == false || x.KpiPeriodId == KpiPeriodId.Value)
                .Select(x => x.KpiPeriodId)
                .Distinct()
                .Skip(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Skip)
                .Take(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportFilterDTO.Take)
                .ToList();
            var KpiPeriods = await KpiPeriodService.List(new KpiPeriodFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiPeriodSelect.ALL,
                Id = new IdFilter { In = KpiPeriodIds },
                OrderBy = KpiPeriodOrder.Id,
                OrderType = OrderType.ASC
            });

            var KpiGeneralContentKpiPeriodMappings = KpiGeneral.KpiGeneralContents
                .SelectMany(x => x.KpiGeneralContentKpiPeriodMappings)
                .Where(x => KpiPeriodIds.Contains(x.KpiPeriodId))
                .ToList();
            List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs = new List<KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO>();
            foreach (KpiPeriod KpiPeriod in KpiPeriods)
            {
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO
                    = new KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO();
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiPeriodId = KpiPeriod.Id;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiPeriodName = KpiPeriod.Name;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiYearId = KpiGeneral.KpiYearId;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.KpiYearName = KpiGeneral.KpiYear.Name;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO.SaleEmployeeId = SaleEmployeeId.Value;
                KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs.Add(KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTO);
            }

            // all store
            var allstore_query = DWContext.Dim_Store.AsNoTracking();
            var AllStores = await allstore_query.ToListWithNoLockAsync();

            // store in checking
            var store_query = allstore_query.Where(s => s.CreatedAt, OrderDateFilter);
            store_query = store_query.Where(s => s.DeletedAt == null);
            store_query = store_query.Where(s => s.CreatorId, SaleEmployeeIdFilter);
            var StoreDAOs = await store_query.ToListWithNoLockAsync();

            // store type
            var StoreTypeDAOs = await DWContext.Dim_StoreType.ToListWithNoLockAsync();
            long C2TD_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2TD).Select(x => x.StoreTypeId).FirstOrDefault();
            long C2SL_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2SL).Select(x => x.StoreTypeId).FirstOrDefault();
            long C2_ID = StoreTypeDAOs.Where(x => x.Code == StaticParams.C2).Select(x => x.StoreTypeId).FirstOrDefault();

            // DirectSalesOrder
            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(q => q.SaleEmployeeId, SaleEmployeeIdFilter);
            directsalesorder_query = directsalesorder_query.Where(q => q.OrderDate, OrderDateFilter);
            directsalesorder_query = directsalesorder_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
            {
                In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
            });
            var DirectSalesOrderDAOs = await directsalesorder_query.ToListWithNoLockAsync();
            List<long> DirectSalesOrderIds = DirectSalesOrderDAOs.Select(x => x.DirectSalesOrderId).ToList();

            // DirectSalesOrder transaction
            var DirectSalesOrderTransaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DirectSalesOrderTransaction_query = DirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            var DirectSalesOrderTransactionDAOs = await DirectSalesOrderTransaction_query.Select(x => new
            {
                x.DirectSalesOrderId,
                x.Quantity
            }).ToListWithNoLockAsync();

            // IndirectSalesOrder
            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.SaleEmployeeId, SaleEmployeeIdFilter);
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.OrderDate, OrderDateFilter);
            indirectsalesorder_query = indirectsalesorder_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });

            var IndirectSalesOrderDAOs = await indirectsalesorder_query.ToListWithNoLockAsync();
            List<long> IndirectSalesOrderIds = IndirectSalesOrderDAOs.Select(x => x.IndirectSalesOrderId).ToList();

            // IndirectSalesOrder transaction
            var IndirectSalesOrderTransaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.DeletedAt == null);
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            IndirectSalesOrderTransaction_query = IndirectSalesOrderTransaction_query.Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id });
            var IndirectSalesOrderTransactionDAOs = await IndirectSalesOrderTransaction_query.Select(x => new
            {
                x.IndirectSalesOrderId,
                x.Quantity
            }).ToListWithNoLockAsync();

            // IndirectSalesOrder C2TD
            var IndirectSalesOrderC2TDDAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreTypeId, new IdFilter { Equal = C2TD_ID }).ToListWithNoLockAsync();

            // IndirectSalesOrder C2SL
            var IndirectSalesOrderC2SLDAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreTypeId, new IdFilter { Equal = C2SL_ID }).ToListWithNoLockAsync();

            // IndirectSalesOrder C2
            var IndirectSalesOrderC2DAOs = await indirectsalesorder_query.Where(q => q.BuyerStoreTypeId, new IdFilter { Equal = C2_ID }).ToListWithNoLockAsync();

            // StoreChecking
            var query_store_checking = DWContext.Fact_StoreChecking.AsNoTracking();
            query_store_checking = query_store_checking.Where(q => q.CheckOutAt, OrderDateFilter);
            query_store_checking = query_store_checking.Where(q => q.SaleEmployeeId, SaleEmployeeIdFilter);
            query_store_checking = from sc in query_store_checking
                                   join s in allstore_query on sc.StoreId equals s.StoreId
                                   where s.DeletedAt == null
                                   select sc;

            var StoreCheckingDAOs = await query_store_checking.ToListWithNoLockAsync();

            // Image
            var store_image_query = DWContext.Fact_Image.AsNoTracking();
            store_image_query = store_image_query.Where(q => q.SaleEmployeeId, SaleEmployeeIdFilter);
            store_image_query = store_image_query.Where(q => q.ShootingAt, OrderDateFilter);
            var StoreImageDAOs = await store_image_query.ToListWithNoLockAsync();

            // Problem
            var query_problem = DWContext.Fact_Problem.AsNoTracking();
            query_problem = query_problem.Where(q => q.CreatorId, SaleEmployeeIdFilter);
            query_problem = query_problem.Where(q => q.NoteAt, OrderDateFilter);
            var ProblemDAOs = await query_problem.ToListWithNoLockAsync();
            foreach (var Period in KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs)
            {
                foreach (var KpiPeriod in KpiPeriodEnum.KpiPeriodEnumList)
                {
                    if (KpiPeriod.Id == Period.KpiPeriodId)
                        Period.KpiPeriodName = KpiPeriod.Name;
                }
                DateTime Start, End;
                (Start, End) = DateTimeConvert(Period.KpiPeriodId, Period.KpiYearId);
                Start = Start < StartDate ? StartDate : Start;
                End = End > EndDate ? EndDate : End;

                //lấy tất cả đơn hàng được đặt trong kì đang xét
                var IndirectSalesOrders = IndirectSalesOrderDAOs
                    .Where(x => x.OrderDate >= Start && x.OrderDate <= End)
                    .ToList();
                var SubIndirectSalesOrderIds = IndirectSalesOrders.Select(x => x.IndirectSalesOrderId).ToList();
                var IndirectSalesOrdersTransaction = IndirectSalesOrderTransactionDAOs.Where(x => SubIndirectSalesOrderIds.Contains(x.IndirectSalesOrderId)).ToList();
                var IndirectSalesOrdersC2TD = IndirectSalesOrderC2TDDAOs
                        .Where(x => x.OrderDate >= Start && x.OrderDate <= End)
                        .ToList();
                var IndirectSalesOrdersC2SL = IndirectSalesOrderC2SLDAOs
                    .Where(x => x.OrderDate >= Start && x.OrderDate <= End)
                    .ToList();
                var IndirectSalesOrdersC2 = IndirectSalesOrderC2DAOs
                    .Where(x => x.OrderDate >= Start && x.OrderDate <= End)
                    .ToList();

                // Lấy tất cả cửa hàng được tạo trong kỳ đang xét
                var Stores = StoreDAOs.Where(sc => sc.CreatedAt >= Start && sc.CreatedAt <= End);

                // Lấy tất cả Problem trong kỳ đang xét
                var Problems = ProblemDAOs.Where(p => p.NoteAt >= Start && p.NoteAt <= End);

                //lấy tất cả lượt checking trong kì đang xét
                var StoreCheckings = StoreCheckingDAOs
                    .Where(x => x.SaleEmployeeId == Period.SaleEmployeeId &&
                    x.CheckOutAt.HasValue && x.CheckOutAt.Value >= Start && x.CheckOutAt.Value <= End)
                    .ToList();

                // Lấy ra tất cả hình ảnh chụp trong kỳ đang xét
                var StoreImages = StoreImageDAOs.Where(s => s.ShootingAt >= Start && s.ShootingAt <= End);

                // Lấy ra tất cả đơn hàng trực tiếp trong kỳ đang xét
                var DirectSalesOrders = DirectSalesOrderDAOs
                    .Where(x => x.SaleEmployeeId == SaleEmployeeId &&
                    x.OrderDate >= Start && x.OrderDate <= End)
                    .ToList();
                var SubDirectSalesOrderIds = DirectSalesOrders.Select(x => x.DirectSalesOrderId).ToList();
                var DirectSalesOrdersTransaction = DirectSalesOrderTransactionDAOs.Where(x => SubDirectSalesOrderIds.Contains(x.DirectSalesOrderId)).ToList();

                #region Số đơn hàng trực tiếp
                //kế hoạch
                Period.TotalDirectOrdersPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalDirectOrdersPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalDirectOrders = Period.TotalDirectOrdersPlanned == null ? null : DirectSalesOrders == null ? null : (decimal?)DirectSalesOrders.Count();
                    //tỉ lệ
                    Period.TotalDirectOrdersRatio = Period.TotalDirectOrdersPlanned == null || Period.TotalDirectOrders == null || Period.TotalDirectOrdersPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalDirectOrders.Value / Period.TotalDirectOrdersPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý mua đơn hàng trực tiếp
                //kế hoạch
                Period.DirectSalesBuyerStorePlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.DirectSalesBuyerStorePlanned.HasValue)
                {
                    //thực hiện
                    Period.DirectSalesBuyerStore = Period.DirectSalesBuyerStorePlanned == null ? null : DirectSalesOrders == null ? null : (decimal?)DirectSalesOrders.Select(x => x.BuyerStoreId).Distinct().Count();
                    //tỉ lệ
                    Period.DirectSalesBuyerStoreRatio = Period.DirectSalesBuyerStorePlanned == null || Period.DirectSalesBuyerStore == null || Period.DirectSalesBuyerStorePlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.DirectSalesBuyerStore.Value / Period.DirectSalesBuyerStorePlanned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý mua đơn hàng gián tiếp
                //kế hoạch
                Period.IndirectSalesBuyerStorePlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.IndirectSalesBuyerStorePlanned.HasValue)
                {
                    //thực hiện
                    Period.IndirectSalesBuyerStore = Period.IndirectSalesBuyerStorePlanned == null ? null : IndirectSalesOrders == null ? null : (decimal?)IndirectSalesOrders.Select(x => x.BuyerStoreId).Distinct().Count();
                    //tỉ lệ
                    Period.IndirectSalesBuyerStoreRatio = Period.IndirectSalesBuyerStorePlanned == null || Period.IndirectSalesBuyerStore == null || Period.IndirectSalesBuyerStorePlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.IndirectSalesBuyerStore.Value / Period.IndirectSalesBuyerStorePlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu theo đơn hàng trực tiếp
                //kế hoạch
                Period.TotalDirectSalesAmountPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalDirectSalesAmountPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalDirectSalesAmount = Period.TotalDirectSalesAmountPlanned == null ? null : DirectSalesOrders == null ? 0 : (decimal?)DirectSalesOrders.Sum(x => x.Total);
                    //tỉ lệ
                    Period.TotalDirectSalesAmountRatio = Period.TotalDirectSalesAmountPlanned == null || Period.TotalDirectSalesAmount == null || Period.TotalDirectSalesAmountPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalDirectSalesAmount.Value / Period.TotalDirectSalesAmountPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng sản lượng đơn gián tiếp
                // Kế hoạch
                Period.TotalIndirectSalesQuantityPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalIndirectSalesQuantityPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalIndirectSalesQuantity = Period.TotalIndirectSalesQuantityPlanned == null ? null : IndirectSalesOrders == null ? 0 : (decimal?)IndirectSalesOrdersTransaction.Sum(x => x.Quantity);
                    //tỉ lệ
                    Period.TotalIndirectSalesQuantityRatio = Period.TotalIndirectSalesQuantityPlanned == null || Period.TotalIndirectSalesQuantity == null || Period.TotalIndirectSalesQuantityPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalIndirectSalesQuantity.Value / Period.TotalIndirectSalesQuantityPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng sản lượng đơn trực tiếp
                // Kế hoạch
                Period.TotalDirectSalesQuantityPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalDirectSalesQuantityPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalDirectSalesQuantity = Period.TotalDirectSalesQuantityPlanned == null ? null : DirectSalesOrders == null ? 0 : (decimal?)DirectSalesOrdersTransaction.Sum(x => x.Quantity);
                    //tỉ lệ
                    Period.TotalDirectSalesQuantityRatio = Period.TotalDirectSalesQuantityPlanned == null || Period.TotalDirectSalesQuantity == null || Period.TotalDirectSalesQuantityPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalDirectSalesQuantity.Value / Period.TotalDirectSalesQuantityPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng doanh thu đơn hàng gián tiếp
                //kế hoạch
                Period.TotalIndirectSalesAmountPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalIndirectSalesAmountPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalIndirectSalesAmount = Period.TotalIndirectSalesAmountPlanned == null ? null : IndirectSalesOrders == null ? 0 : (decimal?)IndirectSalesOrders.Sum(x => x.Total);
                    //tỉ lệ
                    Period.TotalIndirectSalesAmountRatio = Period.TotalIndirectSalesAmountPlanned == null || Period.TotalIndirectSalesAmount == null || Period.TotalIndirectSalesAmountPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalIndirectSalesAmount.Value / Period.TotalIndirectSalesAmountPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2 Trọng điểm
                //kế hoạch
                Period.RevenueC2TDPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2TDPlanned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2TD = Period.RevenueC2TDPlanned == null ?
                    null : IndirectSalesOrdersC2TD == null ? 0 : (decimal?)IndirectSalesOrdersC2TD.Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2TDRatio = Period.RevenueC2TDPlanned == null || Period.RevenueC2TD == null || Period.RevenueC2TDPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2TD.Value / Period.RevenueC2TDPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2 Siêu lớn
                //kế hoạch
                Period.RevenueC2SLPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2SLPlanned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2SL = Period.RevenueC2SLPlanned == null ?
                    null : IndirectSalesOrdersC2SL == null ? 0 : (decimal?)IndirectSalesOrdersC2SL.Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2SLRatio = Period.RevenueC2SLPlanned == null || Period.RevenueC2SL == null || Period.RevenueC2SLPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2SL.Value / Period.RevenueC2SLPlanned.Value) * 100, 2);
                }
                #endregion

                #region Doanh thu C2
                //kế hoạch
                Period.RevenueC2Planned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.RevenueC2Planned.HasValue)
                {
                    //thực hiện
                    Period.RevenueC2 = Period.RevenueC2Planned == null ?
                    null : IndirectSalesOrdersC2 == null ? 0 : (decimal?)IndirectSalesOrdersC2.Sum(x => x.Total);
                    //tỉ lệ
                    Period.RevenueC2Ratio = Period.RevenueC2Planned == null || Period.RevenueC2 == null || Period.RevenueC2Planned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.RevenueC2.Value / Period.RevenueC2Planned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý ghé thăm
                //kế hoạch
                Period.StoresVisitedPlanned = KpiGeneralContentKpiPeriodMappings
                       .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                       x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.STORE_VISITED.Id)
                       .Select(x => x.Value)
                       .FirstOrDefault();
                //thực hiện
                if (Period.StoresVisitedPlanned.HasValue)
                {
                    Period.StoreIds = new HashSet<long>();
                    foreach (var StoreChecking in StoreCheckings)
                    {
                        Period.StoreIds.Add(StoreChecking.StoreId);
                    }
                    //if (Period.StoresVisitedPlanned == 0)
                    //    Period.StoreIds = null;
                    //tỉ lệ
                    Period.StoresVisitedRatio = Period.StoresVisitedPlanned == null || Period.StoresVisited == null || Period.StoresVisitedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.StoresVisited.Value / Period.StoresVisitedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng số đại lý mở mới
                //kế hoạch
                Period.NewStoreCreatedPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NewStoreCreatedPlanned.HasValue)
                {
                    //thực hiện
                    Period.NewStoreCreated = Period.NewStoreCreatedPlanned == null ? null :
                        Stores == null ? 0 : (decimal?)
                        Stores.Count();
                    //tỉ lệ
                    Period.NewStoreCreatedRatio = Period.NewStoreCreatedPlanned == null || Period.NewStoreCreated == null || Period.NewStoreCreatedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NewStoreCreated.Value / Period.NewStoreCreatedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Tổng số lượt ghé thăm
                //kế hoạch
                Period.NumberOfStoreVisitsPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NumberOfStoreVisitsPlanned.HasValue)
                {
                    //thực hiện
                    Period.NumberOfStoreVisits = Period.NumberOfStoreVisitsPlanned == null ? null :
                        StoreCheckings == null ? 0 : (decimal?)
                        StoreCheckings.Count();
                    //tỉ lệ
                    Period.NumberOfStoreVisitsRatio = Period.NumberOfStoreVisitsPlanned == null || Period.NumberOfStoreVisits == null || Period.NumberOfStoreVisitsPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NumberOfStoreVisits.Value / Period.NumberOfStoreVisitsPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số đại lý trọng điểm mở mới
                //kế hoạch
                Period.NewStoreC2CreatedPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.NewStoreC2CreatedPlanned.HasValue)
                {
                    //thực hiện
                    Period.NewStoreC2Created = Period.NewStoreC2CreatedPlanned == null ? null :
                        Stores == null ? 0 : (decimal?)
                        Stores
                        .Where(sc => sc.CreatorId == Period.SaleEmployeeId &&
                        sc.StoreTypeId == C2TD_ID && sc.DeletedAt == null)
                        .Count();
                    //tỉ lệ
                    Period.NewStoreC2CreatedRatio = Period.NewStoreC2CreatedPlanned == null || Period.NewStoreC2Created == null || Period.NewStoreC2CreatedPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.NewStoreC2Created.Value / Period.NewStoreC2CreatedPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số thông tin phản ảnh
                //kế hoạch
                Period.TotalProblemPlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalProblemPlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalProblem = Period.TotalProblemPlanned == null ?
                    null : Problems == null ? 0 : (decimal?)Problems.Where(x => x.CreatorId == Period.SaleEmployeeId).Count();
                    //tỉ lệ
                    Period.TotalProblemRatio = Period.TotalProblemPlanned == null || Period.TotalProblem == null || Period.TotalProblemPlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalProblem.Value / Period.TotalProblemPlanned.Value) * 100, 2);
                }
                #endregion

                #region Số hình ảnh chụp
                //kế hoạch
                Period.TotalImagePlanned = KpiGeneralContentKpiPeriodMappings
                        .Where(x => x.KpiPeriodId == Period.KpiPeriodId &&
                        x.KpiGeneralContent.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                if (Period.TotalImagePlanned.HasValue)
                {
                    //thực hiện
                    Period.TotalImage = Period.TotalImagePlanned == null ?
                    null : StoreImages == null ? 0 : (decimal?)StoreImages.Where(x => x.SaleEmployeeId == Period.SaleEmployeeId).Count();
                    //tỉ lệ
                    Period.TotalImageRatio = Period.TotalImagePlanned == null || Period.TotalImage == null || Period.TotalImagePlanned == 0 ? null :
                        (decimal?)
                        Math.Round((Period.TotalImage.Value / Period.TotalImagePlanned.Value) * 100, 2);
                }
                #endregion
            };

            return KpiGeneralEmployeeReport_KpiGeneralEmployeeReportDTOs.OrderBy(x => x.KpiPeriodId).ThenBy(x => x.KpiYearId).ToList();
        }

    }
}
