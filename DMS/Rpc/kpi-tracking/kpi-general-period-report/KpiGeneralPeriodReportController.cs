using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MKpiCriteriaGeneral;
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

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReportController : RpcController
    {
        private DWContext DWContext;
        private DataContext DataContext;
        private IOrganizationService OrganizationService;
        private IAppUserService AppUserService;
        private IKpiYearService KpiYearService;
        private IKpiPeriodService KpiPeriodService;
        private ICurrentContext CurrentContext;
        private IKpiCriteriaGeneralService KpiCriteriaGeneralService;
        public KpiGeneralPeriodReportController(
            DWContext DWContext,
            DataContext DataContext,
            IOrganizationService OrganizationService,
            IAppUserService AppUserService,
            IKpiYearService KpiYearService,
            IKpiPeriodService KpiPeriodService,
            IKpiCriteriaGeneralService KpiCriteriaGeneralService,
            ICurrentContext CurrentContext)
        {
            this.DWContext = DWContext;
            this.DataContext = DataContext;
            this.OrganizationService = OrganizationService;
            this.AppUserService = AppUserService;
            this.KpiPeriodService = KpiPeriodService;
            this.KpiYearService = KpiYearService;
            this.KpiCriteriaGeneralService = KpiCriteriaGeneralService;
            this.CurrentContext = CurrentContext;
        }

        [Route(KpiGeneralPeriodReportRoute.ListKpiCriteriaGeneral), HttpPost]
        public async Task<List<KpiGeneralPeriodReport_KpiCriteriaGeneralDTO>> ListKpiCriteriaGeneral([FromBody] KpiGeneralPeriodReport_KpiCriteriaGeneralFilterDTO KpiGeneralPeriodReport_KpiCriteriaGeneralFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiCriteriaGeneralFilter KpiCriteriaGeneralFilter = new KpiCriteriaGeneralFilter();
            KpiCriteriaGeneralFilter.Skip = 0;
            KpiCriteriaGeneralFilter.Take = int.MaxValue;
            KpiCriteriaGeneralFilter.Selects = KpiCriteriaGeneralSelect.ALL;

            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(KpiCriteriaGeneralFilter);
            List<KpiGeneralPeriodReport_KpiCriteriaGeneralDTO> KpiGeneralPeriodReport_KpiCriteriaGeneralDTOs = KpiCriteriaGenerals
                .Select(x => new KpiGeneralPeriodReport_KpiCriteriaGeneralDTO(x)).ToList();
            return KpiGeneralPeriodReport_KpiCriteriaGeneralDTOs;
        }

        [Route(KpiGeneralPeriodReportRoute.FilterListAppUser), HttpPost]
        public async Task<List<KpiGeneralPeriodReport_AppUserDTO>> FilterListAppUser([FromBody] KpiGeneralPeriodReport_AppUserFilterDTO KpiGeneralPeriodReport_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = KpiGeneralPeriodReport_AppUserFilterDTO.Id;
            AppUserFilter.OrganizationId = KpiGeneralPeriodReport_AppUserFilterDTO.OrganizationId;
            AppUserFilter.Username = KpiGeneralPeriodReport_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = KpiGeneralPeriodReport_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<KpiGeneralPeriodReport_AppUserDTO> KpiGeneralPeriodReport_AppUserDTOs = AppUsers
                .Select(x => new KpiGeneralPeriodReport_AppUserDTO(x)).ToList();
            return KpiGeneralPeriodReport_AppUserDTOs;
        }

        [Route(KpiGeneralPeriodReportRoute.FilterListOrganization), HttpPost]
        public async Task<List<KpiGeneralPeriodReport_OrganizationDTO>> FilterListOrganization([FromBody] KpiGeneralPeriodReport_OrganizationFilterDTO KpiGeneralPeriodReport_OrganizationFilterDTO)
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
            List<KpiGeneralPeriodReport_OrganizationDTO> KpiGeneralPeriodReport_OrganizationDTOs = Organizations
                .Select(x => new KpiGeneralPeriodReport_OrganizationDTO(x)).ToList();
            return KpiGeneralPeriodReport_OrganizationDTOs;
        }
        [Route(KpiGeneralPeriodReportRoute.FilterListKpiPeriod), HttpPost]
        public async Task<List<KpiGeneralPeriodReport_KpiPeriodDTO>> FilterListKpiPeriod([FromBody] KpiGeneralPeriodReport_KpiPeriodFilterDTO KpiGeneralPeriodReport_KpiPeriodFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();
            KpiPeriodFilter.Skip = 0;
            KpiPeriodFilter.Take = 20;
            KpiPeriodFilter.OrderBy = KpiPeriodOrder.Id;
            KpiPeriodFilter.OrderType = OrderType.ASC;
            KpiPeriodFilter.Selects = KpiPeriodSelect.ALL;
            KpiPeriodFilter.Id = KpiGeneralPeriodReport_KpiPeriodFilterDTO.Id;
            KpiPeriodFilter.Code = KpiGeneralPeriodReport_KpiPeriodFilterDTO.Code;
            KpiPeriodFilter.Name = KpiGeneralPeriodReport_KpiPeriodFilterDTO.Name;

            List<KpiPeriod> KpiPeriods = await KpiPeriodService.List(KpiPeriodFilter);
            List<KpiGeneralPeriodReport_KpiPeriodDTO> KpiGeneralPeriodReport_KpiPeriodDTOs = KpiPeriods
                .Select(x => new KpiGeneralPeriodReport_KpiPeriodDTO(x)).ToList();
            return KpiGeneralPeriodReport_KpiPeriodDTOs;
        }

        [Route(KpiGeneralPeriodReportRoute.FilterListKpiYear), HttpPost]
        public async Task<List<KpiGeneralPeriodReport_KpiYearDTO>> FilterListKpiYear([FromBody] KpiGeneralPeriodReport_KpiYearFilterDTO KpiGeneralPeriodReport_KpiYearFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            KpiYearFilter KpiYearFilter = new KpiYearFilter();
            KpiYearFilter.Skip = 0;
            KpiYearFilter.Take = 20;
            KpiYearFilter.OrderBy = KpiYearOrder.Id;
            KpiYearFilter.OrderType = OrderType.ASC;
            KpiYearFilter.Selects = KpiYearSelect.ALL;
            KpiYearFilter.Id = KpiGeneralPeriodReport_KpiYearFilterDTO.Id;
            KpiYearFilter.Code = KpiGeneralPeriodReport_KpiYearFilterDTO.Code;
            KpiYearFilter.Name = KpiGeneralPeriodReport_KpiYearFilterDTO.Name;

            List<KpiYear> KpiYears = await KpiYearService.List(KpiYearFilter);
            List<KpiGeneralPeriodReport_KpiYearDTO> KpiGeneralPeriodReport_KpiYearDTOs = KpiYears
                .Select(x => new KpiGeneralPeriodReport_KpiYearDTO(x)).ToList();
            return KpiGeneralPeriodReport_KpiYearDTOs;
        }

        [Route(KpiGeneralPeriodReportRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState); // to do kpi year and period

            long? SaleEmployeeId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.AppUserId?.Equal;
            long KpiPeriodId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            var query = from k in DataContext.KpiGeneral
                        join kc in DataContext.KpiGeneralContent on k.Id equals kc.KpiGeneralId
                        join km in DataContext.KpiGeneralContentKpiPeriodMapping on kc.Id equals km.KpiGeneralContentId
                        where OrganizationIds.Contains(k.OrganizationId) &&
                        AppUserIds.Contains(k.EmployeeId) &&
                        (SaleEmployeeId == null || k.EmployeeId == SaleEmployeeId.Value) &&
                        k.KpiYearId == KpiYearId &&
                        km.KpiPeriodId == KpiPeriodId &&
                        km.Value.HasValue &&
                        k.StatusId == StatusEnum.ACTIVE.Id &&
                        k.DeletedAt == null
                        select k.Id;
            return await query.Distinct().CountWithNoLockAsync();
        }

        public async Task<ActionResult<List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO>>> List_Old([FromBody] KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId == null)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId == null)
                return BadRequest(new { message = "Chưa chọn năm KPI" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.AppUserId?.Equal;
            long KpiPeriodId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate != null)
            {
                StartDate = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual == null ? StartDate : KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual.Value;
                EndDate = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual == null ? EndDate : KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.LessEqual.Value
                    .AddDays(1).AddSeconds(-1);
            }

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            // list toan bo nhan vien trong organization do va cac con ma co kpi general
            var query = from k in DataContext.KpiGeneral
                        join kc in DataContext.KpiGeneralContent on k.Id equals kc.KpiGeneralId
                        join km in DataContext.KpiGeneralContentKpiPeriodMapping on kc.Id equals km.KpiGeneralContentId
                        where OrganizationIds.Contains(k.OrganizationId) &&
                        AppUserIds.Contains(k.EmployeeId) &&
                        (SaleEmployeeId == null || k.EmployeeId == SaleEmployeeId.Value) &&
                        k.KpiYearId == KpiYearId &&
                        km.KpiPeriodId == KpiPeriodId &&
                        km.Value.HasValue &&
                        k.StatusId == StatusEnum.ACTIVE.Id &&
                        k.DeletedAt == null
                        select new
                        {
                            EmployeeId = k.EmployeeId,
                            OrganizationId = k.OrganizationId
                        };
            var Ids = await query
                .Distinct()
                .OrderBy(x => x.OrganizationId)
                .Skip(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Skip)
                .Take(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Take)
                .ToListWithNoLockAsync();
            AppUserIds = Ids.Select(x => x.EmployeeId).Distinct().ToList();
            IdFilter AppUserIdFilter = new IdFilter { In = AppUserIds };
            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Id, AppUserIdFilter)
                .OrderBy(su => su.OrganizationId).ThenBy(x => x.DisplayName)
                .Select(x => new AppUserDAO
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    OrganizationId = x.OrganizationId
                })
                .ToListWithNoLockAsync();
            OrganizationIds = Ids.Select(x => x.OrganizationId).Distinct().ToList();
            IdFilter OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var Organizations = await DataContext.Organization
                .Where(x => x.Id, OrganizationIdFilter)
                .OrderBy(x => x.Id)
                .Select(x => new OrganizationDAO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO> KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs = new List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO>();
            foreach (var Organization in Organizations)
            {
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO = new KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO()
                {
                    OrganizationId = Organization.Id,
                    OrganizationName = Organization.Name,
                    SaleEmployees = new List<KpiGeneralPeriodReport_SaleEmployeeDTO>()
                };
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees = Ids.Where(x => x.OrganizationId == Organization.Id).Select(x => new KpiGeneralPeriodReport_SaleEmployeeDTO
                {
                    SaleEmployeeId = x.EmployeeId
                }).ToList();
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs.Add(KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO);

                foreach (var SaleEmployee in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    var Employee = AppUserDAOs.Where(x => x.Id == SaleEmployee.SaleEmployeeId).FirstOrDefault();
                    if (Employee != null)
                    {
                        SaleEmployee.Username = Employee.Username;
                        SaleEmployee.DisplayName = Employee.DisplayName;
                    }
                }
            } // nhóm saleEmp theo Organization, lấy ra UserName và DisplayName
            // list toan bo mapping value and criteria
            var query_detail = from kcm in DataContext.KpiGeneralContentKpiPeriodMapping
                               join kc in DataContext.KpiGeneralContent on kcm.KpiGeneralContentId equals kc.Id
                               join k in DataContext.KpiGeneral on kc.KpiGeneralId equals k.Id
                               where (AppUserIds.Contains(k.EmployeeId) &&
                               OrganizationIds.Contains(k.OrganizationId) &&
                               k.KpiYearId == KpiYearId &&
                               kcm.KpiPeriodId == KpiPeriodId &&
                               k.StatusId == StatusEnum.ACTIVE.Id &&
                               k.DeletedAt == null)
                               select new
                               {
                                   EmployeeId = k.EmployeeId,
                                   KpiCriteriaGeneralId = kc.KpiCriteriaGeneralId,
                                   Value = kcm.Value,
                               };
            List<KpiGeneralPeriodReport_SaleEmployeeDetailDTO> KpiGeneralPeriodReport_SaleEmployeeDetailDTOs = (await query_detail
                .Distinct()
                .ToListWithNoLockAsync())
                .Select(x => new KpiGeneralPeriodReport_SaleEmployeeDetailDTO
                {
                    SaleEmployeeId = x.EmployeeId,
                    KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                    Value = x.Value,
                }).ToList();

            var IndirectSalesOrderDAOs = await DataContext.IndirectSalesOrder
                .Where(x => AppUserIds.Contains(x.SaleEmployeeId) &&
                x.OrderDate >= StartDate && x.OrderDate <= EndDate &&
                x.RequestStateId == RequestStateEnum.APPROVED.Id)
                .Select(x => new IndirectSalesOrderDAO
                {
                    Id = x.Id,
                    Total = x.Total,
                    SaleEmployeeId = x.SaleEmployeeId,
                    OrderDate = x.OrderDate,
                    BuyerStore = x.BuyerStore == null ? null : new StoreDAO
                    {
                        StoreType = x.BuyerStore.StoreType == null ? null : new StoreTypeDAO
                        {
                            Code = x.BuyerStore.StoreType.Code
                        }
                    },
                    IndirectSalesOrderContents = x.IndirectSalesOrderContents.Select(c => new IndirectSalesOrderContentDAO
                    {
                        RequestedQuantity = c.RequestedQuantity,
                        ItemId = c.ItemId
                    }).ToList(),
                    IndirectSalesOrderPromotions = x.IndirectSalesOrderPromotions.Select(x => new IndirectSalesOrderPromotionDAO
                    {
                        RequestedQuantity = x.RequestedQuantity,
                        ItemId = x.ItemId
                    }).ToList()
                })
                .ToListWithNoLockAsync();

            var query_store_checking = from sc in DataContext.StoreChecking
                                       join s in DataContext.Store on sc.StoreId equals s.Id
                                       where AppUserIds.Contains(sc.SaleEmployeeId) &&
                                       (sc.CheckOutAt.HasValue && StartDate <= sc.CheckOutAt.Value && sc.CheckOutAt.Value <= EndDate) &&
                                       s.DeletedAt == null
                                       select sc;

            var StoreCheckingDAOs = await query_store_checking
                .Select(x => new StoreCheckingDAO
                {
                    SaleEmployeeId = x.SaleEmployeeId,
                    Id = x.Id,
                    CheckInAt = x.CheckInAt,
                    CheckOutAt = x.CheckOutAt,
                    StoreId = x.StoreId
                })
                .ToListWithNoLockAsync();

            var ProblemDAOs = await DWContext.Fact_Problem
                 .Where(x => x.CreatorId, new IdFilter { In = AppUserIds })
                 .Where(x => x.NoteAt, new DateFilter { GreaterEqual = StartDate, LessEqual = EndDate })
                 .ToListWithNoLockAsync();
            var StoreImages = await DWContext.Fact_Image
                .Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds })
                .Where(x => x.ShootingAt, new DateFilter { GreaterEqual = StartDate, LessEqual = EndDate })
                .ToListWithNoLockAsync();

            List<StoreDAO> Stores = await DataContext.Store.
                Where(x => x.CreatorId, new IdFilter { In = AppUserIds })
                .Where(x => x.CreatedAt, new DateFilter { GreaterEqual = StartDate, LessEqual = EndDate })
                .Select(x => new StoreDAO
                {
                    Id = x.Id,
                    CreatorId = x.CreatorId,
                    StoreType = x.StoreType == null ? null : new StoreTypeDAO
                    {
                        Code = x.StoreType.Code
                    }
                }).ToListWithNoLockAsync();
            foreach (var KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs)
            {
                foreach (var SaleEmployeeDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    SaleEmployeeDTO.OrganizationName = KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.OrganizationName;
                    //lấy tất cả đơn hàng được thực hiện bởi nhân viên đang xét
                    var IndirectSalesOrders = IndirectSalesOrderDAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();

                    #region Tổng doanh thu đơn hàng
                    //kế hoạch
                    SaleEmployeeDTO.TotalIndirectSalesAmountPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalIndirectSalesAmount = SaleEmployeeDTO.TotalIndirectSalesAmountPlanned == null ? null : (decimal?)IndirectSalesOrders.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.TotalIndirectSalesAmountRatio = SaleEmployeeDTO.TotalIndirectSalesAmountPlanned == null || SaleEmployeeDTO.TotalIndirectSalesAmount == null || SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalIndirectSalesAmount.Value / SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số đại lý ghé thăm
                    //kế hoạch
                    SaleEmployeeDTO.StoresVisitedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                           .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.STORE_VISITED.Id)
                           .Select(x => x.Value).FirstOrDefault();
                    //thực hiện
                    if (SaleEmployeeDTO.StoresVisitedPlanned.HasValue)
                    {
                        var StoreIds = StoreCheckingDAOs
                            .Where(sc => sc.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                            .Select(x => x.StoreId)
                            .ToList();
                        SaleEmployeeDTO.StoreIds = new HashSet<long>();
                        foreach (var StoreId in StoreIds)
                        {
                            SaleEmployeeDTO.StoreIds.Add(StoreId);
                        }
                        if (SaleEmployeeDTO.StoresVisitedPlanned == 0)
                            SaleEmployeeDTO.StoreIds = new HashSet<long>();
                        //tỉ lệ
                        SaleEmployeeDTO.StoresVisitedRatio = SaleEmployeeDTO.StoresVisitedPlanned == null || SaleEmployeeDTO.StoresVisited == null || SaleEmployeeDTO.StoresVisitedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.StoresVisited.Value / SaleEmployeeDTO.StoresVisitedPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng số đại lý mở mới
                    //kế hoạch
                    SaleEmployeeDTO.NewStoreCreatedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId
                             && x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NewStoreCreatedPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NewStoreCreated = SaleEmployeeDTO.NewStoreCreatedPlanned == null || SaleEmployeeDTO.NewStoreCreatedPlanned == 0
                            ? null
                            : (decimal?)Stores
                            .Where(st => st.CreatorId == SaleEmployeeDTO.SaleEmployeeId
                                    && st.DeletedAt == null)
                            .Count(); // lấy ra tất cả cửa hàng có người tạo là saleEmployee, trạng thái là dự thảo hoặc chính thức
                        //tỉ lệ
                        SaleEmployeeDTO.NewStoreCreatedRatio = SaleEmployeeDTO.NewStoreCreatedPlanned == null || SaleEmployeeDTO.NewStoreCreated == null || SaleEmployeeDTO.NewStoreCreatedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NewStoreCreated.Value / SaleEmployeeDTO.NewStoreCreatedPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Tổng số lượt ghé thăm
                    //kế hoạch
                    SaleEmployeeDTO.NumberOfStoreVisitsPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NumberOfStoreVisitsPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NumberOfStoreVisits = SaleEmployeeDTO.NumberOfStoreVisitsPlanned == null || SaleEmployeeDTO.NumberOfStoreVisitsPlanned.Value == 0
                            ? null
                            : (decimal?)StoreCheckingDAOs
                            .Where(sc => sc.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                            .Count();
                        //tỉ lệ
                        SaleEmployeeDTO.NumberOfStoreVisitsRatio = SaleEmployeeDTO.NumberOfStoreVisitsPlanned == null || SaleEmployeeDTO.NumberOfStoreVisits == null || SaleEmployeeDTO.NumberOfStoreVisitsPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NumberOfStoreVisits.Value / SaleEmployeeDTO.NumberOfStoreVisitsPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2 Trọng điểm
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2TDPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2TDPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2TD = SaleEmployeeDTO.RevenueC2TDPlanned == null ?
                        null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2TD).Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2TDRatio = SaleEmployeeDTO.RevenueC2TDPlanned == null || SaleEmployeeDTO.RevenueC2TD == null || SaleEmployeeDTO.RevenueC2TDPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2TD.Value / SaleEmployeeDTO.RevenueC2TDPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2 Siêu lớn
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2SLPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2SLPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2SL = SaleEmployeeDTO.RevenueC2SLPlanned == null ?
                        null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2SL).Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2SLRatio = SaleEmployeeDTO.RevenueC2SLPlanned == null || SaleEmployeeDTO.RevenueC2SL == null || SaleEmployeeDTO.RevenueC2SLPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2SL.Value / SaleEmployeeDTO.RevenueC2SLPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2Planned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2Planned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2 = SaleEmployeeDTO.RevenueC2Planned == null ?
                        null : (decimal?)IndirectSalesOrders.Where(x => x.BuyerStore.StoreType.Code == StaticParams.C2).Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2Ratio = SaleEmployeeDTO.RevenueC2Planned == null || SaleEmployeeDTO.RevenueC2 == null || SaleEmployeeDTO.RevenueC2Planned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2.Value / SaleEmployeeDTO.RevenueC2Planned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số đại lý trọng điểm mở mới
                    //kế hoạch
                    SaleEmployeeDTO.NewStoreC2CreatedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId
                             && x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NewStoreC2CreatedPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NewStoreC2Created = SaleEmployeeDTO.NewStoreC2CreatedPlanned == null || SaleEmployeeDTO.NewStoreC2CreatedPlanned == 0
                            ? null
                            : (decimal?)Stores
                            .Where(st => st.CreatorId == SaleEmployeeDTO.SaleEmployeeId &&
                            st.StoreType.Code == StaticParams.C2TD
                            && st.DeletedAt == null)
                            .Count(); // lấy ra tất cả cửa hàng có người tạo là saleEmployee, trạng thái là dự thảo hoặc chính thức
                        //tỉ lệ
                        SaleEmployeeDTO.NewStoreC2CreatedRatio = SaleEmployeeDTO.NewStoreC2CreatedPlanned == null || SaleEmployeeDTO.NewStoreC2Created == null || SaleEmployeeDTO.NewStoreC2CreatedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NewStoreC2Created.Value / SaleEmployeeDTO.NewStoreC2CreatedPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Số thông tin phản ánh
                    //kế hoạch
                    SaleEmployeeDTO.TotalProblemPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalProblemPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalProblem = SaleEmployeeDTO.TotalProblemPlanned == null ?
                        null : (decimal?)ProblemDAOs.Where(x => x.CreatorId == SaleEmployeeDTO.SaleEmployeeId).Count();
                        //tỉ lệ
                        SaleEmployeeDTO.TotalProblemRatio = SaleEmployeeDTO.TotalProblemPlanned == null || SaleEmployeeDTO.TotalProblem == null || SaleEmployeeDTO.TotalProblemPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalProblem.Value / SaleEmployeeDTO.TotalProblemPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số hình ảnh chụp
                    //kế hoạch
                    SaleEmployeeDTO.TotalImagePlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalImagePlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalImage = SaleEmployeeDTO.TotalImagePlanned == null ?
                        null : (decimal?)StoreImages.Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId).Count();
                        //tỉ lệ
                        SaleEmployeeDTO.TotalImageRatio = SaleEmployeeDTO.TotalImagePlanned == null || SaleEmployeeDTO.TotalImage == null || SaleEmployeeDTO.TotalImagePlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalImage.Value / SaleEmployeeDTO.TotalImagePlanned.Value) * 100, 2);
                    }
                    #endregion
                }
            };

            return KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs;
        }

        [Route(KpiGeneralPeriodReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId == null)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId == null)
                return BadRequest(new { message = "Chưa chọn năm KPI" });

            var KpiPeriod = KpiPeriodEnum.KpiPeriodEnumList.Where(x => x.Id == KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId.Equal.Value).FirstOrDefault();
            var KpiYear = KpiYearEnum.KpiYearEnumList.Where(x => x.Id == KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId.Equal.Value).FirstOrDefault();

            DateTime StartDate, EndDate;
            long KpiPeriodId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.HasValue)
            {
                StartDate = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual == null ? StartDate : KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual.Value;
                EndDate = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual == null ? EndDate : KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.LessEqual.Value;
            }

            KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Skip = 0;
            KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Take = int.MaxValue;
            List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO> KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs = (await List(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO)).Value;

            long stt = 1;
            foreach (KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs)
            {
                foreach (var SaleEmployee in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    SaleEmployee.STT = stt;
                    stt++;
                }
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            List<KpiGeneralPeriodReport_ExportDTO> KpiGeneralPeriodReport_ExportDTOs =
                await ConvertReportToExportDTO(KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs);

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
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Báo cáo KPI theo kỳ");
                ws.Cells.Style.Font.Name = "Times New Roman";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                ws.Cells["A1"].Value = OrgRoot.Name.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 11;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A4"].Value = "BÁO CÁO KPI THEO KỲ";
                ws.Cells["A4:U4"].Merge = true;
                ws.Cells["A4:U4"].Style.Font.Size = 20;
                ws.Cells["A4:U4"].Style.Font.Bold = true;
                ws.Cells["A4:U4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["I5:L6"].Style.Font.Size = 11;
                ws.Cells["I5:L6"].Style.Font.Italic = true;

                ws.Cells["I5"].Value = "Kỳ KPI";
                ws.Cells["I5"].Style.Font.Bold = true;
                ws.Cells["J5"].Value = KpiPeriod.Name;

                ws.Cells["K5"].Value = "Năm";
                ws.Cells["K5"].Style.Font.Bold = true;
                ws.Cells["L5"].Value = KpiYear.Name;

                ws.Cells["I6"].RichText.Add("Từ ngày: ").Bold = true;
                ws.Cells["I6"].RichText.Add(StartDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy")).Bold = false;
                ws.Cells["I6"].RichText.Add(" - Đến ngày: ").Bold = true;
                ws.Cells["I6"].RichText.Add(EndDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy")).Bold = false;
                ws.Cells["I6:L6"].Merge = true;
                #endregion

                #region Header bang ket qua
                List<string> headers = new List<string>
                {
                    "STT",
                    "Mã nhân viên",
                    "Tên nhân viên"
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
                if (endColumnNumber > 26) endColumnString = Char.ConvertFromUtf32(endColumnNumber / 26 + 64) + Char.ConvertFromUtf32(endColumnNumber % 26 + 64); ;

                string headerRange = $"A8:" + $"{endColumnString}9";
                List<string[]> Header = new List<string[]> { headers.ToArray(), headerLine2.ToArray() };
                ws.Cells[headerRange].LoadFromArrays(Header);
                ws.Cells[headerRange].Style.Font.Size = 11;
                ws.Cells[headerRange].Style.Font.Bold = true;
                ws.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                ws.Cells["A8:A9"].Merge = true;
                ws.Cells["B8:B9"].Merge = true;
                ws.Cells["C8:C9"].Merge = true;

                for (int i = 1; i <= endColumnNumber; i += 3)
                {
                    if (i == 1) continue;

                    string startMerge = Char.ConvertFromUtf32(i + 64);
                    if (i > 26) startMerge = Char.ConvertFromUtf32(i / 26 + 64) + Char.ConvertFromUtf32(i % 26 + 64);
                    string endMerge = Char.ConvertFromUtf32(i + 2 + 64);
                    if (i + 2 > 26) endMerge = Char.ConvertFromUtf32((i + 2) / 26 + 64) + Char.ConvertFromUtf32((i + 2) % 26 + 64);
                    ws.Cells[$"{startMerge}8:{endMerge}8"].Merge = true; // merge cai dong tren cua header
                }
                string headerRange2 = $"A9:" + $"{endColumnString}9";

                ws.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[headerRange].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells[headerRange2].AutoFitColumns();
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 20;
                #endregion

                #region Du lieu bao cao
                int startRow = 10;
                int startColumn = 1;
                int endColumn = startColumn;
                int endRow = startRow;
                if (KpiGeneralPeriodReport_ExportDTOs != null && KpiGeneralPeriodReport_ExportDTOs.Count > 0)
                {
                    foreach (var KpiGeneralPeriodReport_ExportDTO in KpiGeneralPeriodReport_ExportDTOs)
                    {
                        ws.Cells[$"A{startRow}"].Value = KpiGeneralPeriodReport_ExportDTO.OrganizationName;
                        ws.Cells[$"A{startRow}"].Style.Font.Bold = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Merge = true;
                        ws.Cells[$"A{startRow}:{endColumnString}{startRow}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        startRow++;

                        #region Cột STT
                        List<Object[]> SttData = new List<object[]>();
                        endRow = startRow;
                        foreach (var user in KpiGeneralPeriodReport_ExportDTO.Users)
                        {
                            SttData.Add(new object[]
                            {
                                user.STT,
                            });
                            endRow++;
                        }
                        string startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        string currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(SttData);
                        endColumn += 1;
                        startColumn = endColumn;
                        #endregion

                        #region Cột mã nhân viện, tên nhân viên
                        List<Object[]> KpiData = new List<object[]>();
                        endRow = startRow;
                        foreach (var user in KpiGeneralPeriodReport_ExportDTO.Users)
                        {
                            KpiData.Add(new object[]
                            {
                                user.Username,
                                user.DisplayName,
                            });
                            endRow++;
                        }
                        startColumnString = Char.ConvertFromUtf32(startColumn + 64);
                        if (startColumn > 26) startColumnString = Char.ConvertFromUtf32(startColumn / 26 + 64) + Char.ConvertFromUtf32(startColumn % 26 + 64);
                        currentColumnString = Char.ConvertFromUtf32(endColumn + 64);
                        if (endColumn > 26) currentColumnString = Char.ConvertFromUtf32(endColumn / 26 + 64) + Char.ConvertFromUtf32(endColumn % 26 + 64);

                        ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(KpiData);
                        endColumn += 2;
                        startColumn = endColumn;
                        #endregion

                        #region Các cột giá trị
                        for (int i = 0; i < KpiCriteriaGenerals.Count; i++)
                        {
                            List<Object[]> ValueData = new List<object[]>();
                            endRow = startRow; // gán lại dòng bắt đầu
                            foreach (var user in KpiGeneralPeriodReport_ExportDTO.Users)
                            {
                                KpiGeneralPeriodReport_CriteriaContentDTO criteriaContent = user
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


                            ws.Cells[$"{currentColumnString}{startRow}:{startColumnString}{endRow - 1}"].LoadFromArrays(ValueData); // fill dữ liệu

                            endColumn += 3; // Chiếm 3 cột cho mỗi criteria
                            startColumn = endColumn;
                        }
                        startColumn = 1;
                        endColumn = startColumn; // gán lại cột bắt đầu
                        startRow = endRow; // gán dòng bắt đầu cho org tiếp theo
                        #endregion
                    }
                }
                #endregion
                ws.Cells[$"D10:{endColumnString}{endRow}"].Style.Numberformat.Format = "#,##0.00"; // format number column value

                // All borders
                ws.Cells[$"A8:{endColumnString}{endRow}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A8:{endColumnString}{endRow}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A8:{endColumnString}{endRow}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[$"A8:{endColumnString}{endRow}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                excel.Save();
            }

            return File(output.ToArray(), "application/octet-stream", "KpiGeneralPeriodReport.xlsx");
        }

        private async Task<List<KpiGeneralPeriodReport_ExportDTO>> ConvertReportToExportDTO(List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO> KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs)
        {
            List<KpiGeneralPeriodReport_ExportDTO> KpiGeneralPeriodReport_ExportDTOs = new List<KpiGeneralPeriodReport_ExportDTO>();
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = await KpiCriteriaGeneralService.List(new KpiCriteriaGeneralFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = KpiCriteriaGeneralSelect.Id,
            });
            List<long> KpiCriteriaGeneralIds = KpiCriteriaGenerals.Select(x => x.Id).ToList();

            foreach (var KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs)
            {
                KpiGeneralPeriodReport_ExportDTO KpiGeneralPeriodReport_ExportDTO = new KpiGeneralPeriodReport_ExportDTO();
                KpiGeneralPeriodReport_ExportDTO.OrganizationName = KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.OrganizationName;
                KpiGeneralPeriodReport_ExportDTO.Users = new List<KpiGeneralPeriodReport_UserDTO>();

                foreach (var SaleEmployee in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    KpiGeneralPeriodReport_UserDTO KpiGeneralPeriodReport_LineDTO = new KpiGeneralPeriodReport_UserDTO();
                    KpiGeneralPeriodReport_LineDTO.STT = SaleEmployee.STT;
                    KpiGeneralPeriodReport_LineDTO.Username = SaleEmployee.Username;
                    KpiGeneralPeriodReport_LineDTO.DisplayName = SaleEmployee.DisplayName;
                    KpiGeneralPeriodReport_LineDTO.OrganizationName = SaleEmployee.OrganizationName;
                    KpiGeneralPeriodReport_LineDTO.CriteriaContents = new List<KpiGeneralPeriodReport_CriteriaContentDTO>();
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Name,
                            Planned = SaleEmployee.TotalIndirectSalesAmountPlanned,
                            Result = SaleEmployee.TotalIndirectSalesAmount,
                            Ratio = SaleEmployee.TotalIndirectSalesAmountRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2_TD.Name,
                            Planned = SaleEmployee.RevenueC2TDPlanned,
                            Result = SaleEmployee.RevenueC2TD,
                            Ratio = SaleEmployee.RevenueC2TDRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2_SL.Name,
                            Planned = SaleEmployee.RevenueC2SLPlanned,
                            Result = SaleEmployee.RevenueC2SL,
                            Ratio = SaleEmployee.RevenueC2SLRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.REVENUE_C2.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.REVENUE_C2.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.REVENUE_C2.Name,
                            Planned = SaleEmployee.RevenueC2Planned,
                            Result = SaleEmployee.RevenueC2,
                            Ratio = SaleEmployee.RevenueC2Ratio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Name,
                            Planned = SaleEmployee.TotalProblemPlanned,
                            Result = SaleEmployee.TotalProblem,
                            Ratio = SaleEmployee.TotalProblemRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_IMAGE.Name,
                            Planned = SaleEmployee.TotalImagePlanned,
                            Result = SaleEmployee.TotalImage,
                            Ratio = SaleEmployee.TotalImageRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.STORE_VISITED.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.STORE_VISITED.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.STORE_VISITED.Name,
                            Planned = SaleEmployee.StoresVisitedPlanned,
                            Result = SaleEmployee.StoresVisited,
                            Ratio = SaleEmployee.StoresVisitedRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Name,
                            Planned = SaleEmployee.NewStoreCreatedPlanned,
                            Result = SaleEmployee.NewStoreCreated,
                            Ratio = SaleEmployee.NewStoreCreatedRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Name,
                            Planned = SaleEmployee.NewStoreC2CreatedPlanned,
                            Result = SaleEmployee.NewStoreC2Created,
                            Ratio = SaleEmployee.NewStoreC2CreatedRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Name,
                            Planned = SaleEmployee.NumberOfStoreVisitsPlanned,
                            Result = SaleEmployee.NumberOfStoreVisits,
                            Ratio = SaleEmployee.NumberOfStoreVisitsRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Name,
                            Planned = SaleEmployee.TotalDirectOrdersPlanned,
                            Result = SaleEmployee.TotalDirectOrders,
                            Ratio = SaleEmployee.TotalDirectOrdersRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Name,
                            Planned = SaleEmployee.TotalDirectSalesAmountPlanned,
                            Result = SaleEmployee.TotalDirectSalesAmount,
                            Ratio = SaleEmployee.TotalDirectSalesAmountRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Name,
                            Planned = SaleEmployee.TotalDirectSalesQuantityPlanned,
                            Result = SaleEmployee.TotalDirectSalesQuantity,
                            Ratio = SaleEmployee.TotalDirectSalesQuantityRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Name,
                            Planned = SaleEmployee.TotalIndirectSalesQuantityPlanned,
                            Result = SaleEmployee.TotalIndirectSalesQuantity,
                            Ratio = SaleEmployee.TotalIndirectSalesQuantityRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Name,
                            Planned = SaleEmployee.DirectSalesBuyerStorePlanned,
                            Result = SaleEmployee.DirectSalesBuyerStore,
                            Ratio = SaleEmployee.DirectSalesBuyerStoreRatio
                        });
                    }
                    if (KpiCriteriaGeneralIds.Contains(KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id))
                    {
                        KpiGeneralPeriodReport_LineDTO.CriteriaContents.Add(new KpiGeneralPeriodReport_CriteriaContentDTO
                        {
                            CriteriaId = KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id,
                            CriteriaName = KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Name,
                            Planned = SaleEmployee.IndirectSalesBuyerStorePlanned,
                            Result = SaleEmployee.IndirectSalesBuyerStore,
                            Ratio = SaleEmployee.IndirectSalesBuyerStoreRatio
                        });
                    }

                    KpiGeneralPeriodReport_ExportDTO.Users.Add(KpiGeneralPeriodReport_LineDTO);
                }

                KpiGeneralPeriodReport_ExportDTOs.Add(KpiGeneralPeriodReport_ExportDTO);
            }

            return KpiGeneralPeriodReport_ExportDTOs;
        }

        private Tuple<DateTime, DateTime> DateTimeConvert(long KpiPeriodId, long KpiYearId)
        {
            DateTime startDate = StaticParams.DateTimeNow;
            DateTime endDate = StaticParams.DateTimeNow;
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
            startDate = startDate.AddHours(0 - CurrentContext.TimeZone);
            endDate = endDate.AddHours(0 - CurrentContext.TimeZone);
            return Tuple.Create(startDate, endDate);
        }

        [Route(KpiGeneralPeriodReportRoute.List), HttpPost]
        public async Task<ActionResult<List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO>>> List([FromBody] KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId == null)
                return BadRequest(new { message = "Chưa chọn kì KPI" });
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId == null)
                return BadRequest(new { message = "Chưa chọn năm KPI" });

            DateTime StartDate, EndDate;
            long? SaleEmployeeId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.AppUserId?.Equal;
            long KpiPeriodId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiPeriodId?.Equal ?? 100 + StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Month;
            long KpiYearId = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.KpiYearId?.Equal ?? StaticParams.DateTimeNow.AddHours(0 - CurrentContext.TimeZone).Year;
            (StartDate, EndDate) = DateTimeConvert(KpiPeriodId, KpiYearId);
            if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate != null)
            {
                if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual != null)
                {
                    var StartDateFilter = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.GreaterEqual.Value;
                    StartDate = StartDateFilter >= StartDate ? StartDateFilter : StartDate; // Don't allow get date out of Kpi period range
                }
                if (KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.LessEqual != null)
                {
                    var EndDateFilter = KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrderDate.LessEqual.Value;
                    EndDate = EndDateFilter <= EndDate ? EndDateFilter : EndDate; // Don't allow get date out of Kpi period range
                }
            }

            IdFilter SaleEmployeeIdFilter = new IdFilter() { Equal = SaleEmployeeId };
            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = StartDate, LessEqual = EndDate };

            List<long> AppUserIds, OrganizationIds;
            (AppUserIds, OrganizationIds) = await FilterOrganizationAndUser(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.OrganizationId,
                AppUserService, OrganizationService, CurrentContext, DataContext);

            #region KPI
            // list toan bo nhan vien trong organization do va cac con ma co kpi general
            ITempTableQuery<TempTable<long>> TempTableOrganizationQuery = await DataContext
                .BulkInsertValuesIntoTempTableAsync<long>(OrganizationIds);
            ITempTableQuery<TempTable<long>> TempTableAppUserQuery = await DataContext
                .BulkInsertValuesIntoTempTableAsync<long>(AppUserIds);
            var query = from k in DataContext.KpiGeneral
                        join kc in DataContext.KpiGeneralContent on k.Id equals kc.KpiGeneralId
                        join km in DataContext.KpiGeneralContentKpiPeriodMapping on kc.Id equals km.KpiGeneralContentId
                        join o in TempTableOrganizationQuery.Query on k.OrganizationId equals o.Column1
                        join au in TempTableAppUserQuery.Query on k.EmployeeId equals au.Column1
                        where
                        (SaleEmployeeId == null || k.EmployeeId == SaleEmployeeId.Value) &&
                        k.KpiYearId == KpiYearId &&
                        km.KpiPeriodId == KpiPeriodId &&
                        km.Value.HasValue &&
                        k.StatusId == StatusEnum.ACTIVE.Id &&
                        k.DeletedAt == null
                        select new
                        {
                            EmployeeId = k.EmployeeId,
                            OrganizationId = k.OrganizationId
                        };
            var Ids = await query
                .Distinct()
                .OrderBy(x => x.OrganizationId)
                .Skip(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Skip)
                .Take(KpiGeneralPeriodReport_KpiGeneralPeriodReportFilterDTO.Take)
                .ToListWithNoLockAsync();
            AppUserIds = Ids.Select(x => x.EmployeeId).Distinct().ToList();
            IdFilter AppUserIdFilter = new IdFilter { In = AppUserIds };
            List<AppUserDAO> AppUserDAOs = await DataContext.AppUser
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Id, AppUserIdFilter)
                .OrderBy(su => su.OrganizationId).ThenBy(x => x.DisplayName)
                .Select(x => new AppUserDAO
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    OrganizationId = x.OrganizationId
                })
                .ToListWithNoLockAsync();
            OrganizationIds = Ids.Select(x => x.OrganizationId).Distinct().ToList();
            IdFilter OrganizationIdFilter = new IdFilter { In = OrganizationIds };
            var Organizations = await DataContext.Organization
                .Where(x => x.Id, OrganizationIdFilter)
                .OrderBy(x => x.Id)
                .Select(x => new OrganizationDAO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO> KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs = new List<KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO>();
            foreach (var Organization in Organizations)
            {
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO = new KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO()
                {
                    OrganizationId = Organization.Id,
                    OrganizationName = Organization.Name,
                    SaleEmployees = new List<KpiGeneralPeriodReport_SaleEmployeeDTO>()
                };
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees = Ids.Where(x => x.OrganizationId == Organization.Id).Select(x => new KpiGeneralPeriodReport_SaleEmployeeDTO
                {
                    SaleEmployeeId = x.EmployeeId
                }).ToList();
                KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs.Add(KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO);

                foreach (var SaleEmployee in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    var Employee = AppUserDAOs.Where(x => x.Id == SaleEmployee.SaleEmployeeId).FirstOrDefault();
                    if (Employee != null)
                    {
                        SaleEmployee.Username = Employee.Username;
                        SaleEmployee.DisplayName = Employee.DisplayName;
                    }
                }
            } // nhóm saleEmp theo Organization, lấy ra UserName và DisplayName
            // list toan bo mapping value and criteria
            var query_detail = from kcm in DataContext.KpiGeneralContentKpiPeriodMapping
                               join kc in DataContext.KpiGeneralContent on kcm.KpiGeneralContentId equals kc.Id
                               join k in DataContext.KpiGeneral on kc.KpiGeneralId equals k.Id
                               join o in TempTableOrganizationQuery.Query on k.OrganizationId equals o.Column1
                               join au in TempTableAppUserQuery.Query on k.EmployeeId equals au.Column1
                               where
                               k.KpiYearId == KpiYearId &&
                               kcm.KpiPeriodId == KpiPeriodId &&
                               k.StatusId == StatusEnum.ACTIVE.Id &&
                               k.DeletedAt == null
                               select new
                               {
                                   EmployeeId = k.EmployeeId,
                                   KpiCriteriaGeneralId = kc.KpiCriteriaGeneralId,
                                   Value = kcm.Value,
                               };
            List<KpiGeneralPeriodReport_SaleEmployeeDetailDTO> KpiGeneralPeriodReport_SaleEmployeeDetailDTOs = (await query_detail
                .Distinct()
                .ToListWithNoLockAsync())
                .Select(x => new KpiGeneralPeriodReport_SaleEmployeeDetailDTO
                {
                    SaleEmployeeId = x.EmployeeId,
                    KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                    Value = x.Value,
                }).ToList();
            #endregion KPI

            ITempTableQuery<TempTable<long>> TempTableAppUserDWQuery = await DWContext
                .BulkInsertValuesIntoTempTableAsync<long>(AppUserIds);

            // store
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

            foreach (var KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs)
            {
                foreach (var SaleEmployeeDTO in KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.SaleEmployees)
                {
                    SaleEmployeeDTO.OrganizationName = KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.OrganizationName;
                    //lấy tất cả đơn hàng được thực hiện bởi nhân viên đang xét
                    var IndirectSalesOrders = IndirectSalesOrderDAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();
                    var SubIndirectSalesOrderIds = IndirectSalesOrders.Select(x => x.IndirectSalesOrderId).ToList();
                    var IndirectSalesOrdersTransaction = IndirectSalesOrderTransactionDAOs.Where(x => SubIndirectSalesOrderIds.Contains(x.IndirectSalesOrderId)).ToList();
                    var IndirectSalesOrdersC2TD = IndirectSalesOrderC2TDDAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();
                    var IndirectSalesOrdersC2SL = IndirectSalesOrderC2SLDAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();
                    var IndirectSalesOrdersC2 = IndirectSalesOrderC2DAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();
                    var DirectSalesOrders = DirectSalesOrderDAOs
                        .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                        .ToList();
                    var SubDirectSalesOrderIds = DirectSalesOrders.Select(x => x.DirectSalesOrderId).ToList();
                    var DirectSalesOrdersTransaction = DirectSalesOrderTransactionDAOs.Where(x => SubDirectSalesOrderIds.Contains(x.DirectSalesOrderId)).ToList();

                    #region Tổng doanh thu đơn hàng trực tiếp
                    //kế hoạch
                    SaleEmployeeDTO.TotalDirectSalesAmountPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_AMOUNT.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalDirectSalesAmountPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalDirectSalesAmount = SaleEmployeeDTO.TotalDirectSalesAmountPlanned == null ? null :
                            DirectSalesOrders == null ? 0 : (decimal?)DirectSalesOrders.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.TotalDirectSalesAmountRatio = SaleEmployeeDTO.TotalDirectSalesAmountPlanned == null || SaleEmployeeDTO.TotalDirectSalesAmount == null || SaleEmployeeDTO.TotalDirectSalesAmountPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalDirectSalesAmount.Value / SaleEmployeeDTO.TotalDirectSalesAmountPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số đại lý mua đơn hàng trực tiếp
                    //kế hoạch
                    SaleEmployeeDTO.DirectSalesBuyerStorePlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.DIRECT_SALES_BUYER_STORE.Id)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    if (SaleEmployeeDTO.DirectSalesBuyerStorePlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.DirectSalesBuyerStore = SaleEmployeeDTO.DirectSalesBuyerStorePlanned == null ? null : DirectSalesOrders == null ? null : (decimal?)DirectSalesOrders.Select(x => x .BuyerStoreId).Distinct().Count();
                        //tỉ lệ
                        SaleEmployeeDTO.DirectSalesBuyerStoreRatio = SaleEmployeeDTO.DirectSalesBuyerStorePlanned == null || SaleEmployeeDTO.DirectSalesBuyerStore == null || SaleEmployeeDTO.DirectSalesBuyerStorePlanned == 0 ? null :
                            (decimal?)
                            Math.Round((SaleEmployeeDTO.DirectSalesBuyerStore.Value / SaleEmployeeDTO.DirectSalesBuyerStorePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số đại lý mua đơn hàng gián tiếp
                    //kế hoạch
                    SaleEmployeeDTO.IndirectSalesBuyerStorePlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.INDIRECT_SALES_BUYER_STORE.Id)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    if (SaleEmployeeDTO.IndirectSalesBuyerStorePlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.IndirectSalesBuyerStore = SaleEmployeeDTO.IndirectSalesBuyerStorePlanned == null ? null : IndirectSalesOrders == null ? null : (decimal?)IndirectSalesOrders.Select(x => x.BuyerStoreId).Distinct().Count();
                        //tỉ lệ
                        SaleEmployeeDTO.IndirectSalesBuyerStoreRatio = SaleEmployeeDTO.IndirectSalesBuyerStorePlanned == null || SaleEmployeeDTO.IndirectSalesBuyerStore == null || SaleEmployeeDTO.IndirectSalesBuyerStorePlanned == 0 ? null :
                            (decimal?)
                            Math.Round((SaleEmployeeDTO.IndirectSalesBuyerStore.Value / SaleEmployeeDTO.IndirectSalesBuyerStorePlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng sản lượng đơn gián tiếp
                    // Kế hoạch
                    SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_QUANTITY.Id)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    if (SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalIndirectSalesQuantity = SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned == null ? null : IndirectSalesOrders == null ? 0 : (decimal?)IndirectSalesOrdersTransaction.Sum(x => x.Quantity);
                        //tỉ lệ
                        SaleEmployeeDTO.TotalIndirectSalesQuantityRatio = SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned == null || SaleEmployeeDTO.TotalIndirectSalesQuantity == null || SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned == 0 ? null :
                            (decimal?)
                            Math.Round((SaleEmployeeDTO.TotalIndirectSalesQuantity.Value / SaleEmployeeDTO.TotalIndirectSalesQuantityPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng sản lượng đơn trực tiếp
                    // Kế hoạch
                    SaleEmployeeDTO.TotalDirectSalesQuantityPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_QUANTITY.Id)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    if (SaleEmployeeDTO.TotalDirectSalesQuantityPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalDirectSalesQuantity = SaleEmployeeDTO.TotalDirectSalesQuantityPlanned == null ? null : DirectSalesOrders == null ? 0 : (decimal?)DirectSalesOrdersTransaction.Sum(x => x.Quantity);
                        //tỉ lệ
                        SaleEmployeeDTO.TotalDirectSalesQuantityRatio = SaleEmployeeDTO.TotalDirectSalesQuantityPlanned == null || SaleEmployeeDTO.TotalDirectSalesQuantity == null || SaleEmployeeDTO.TotalDirectSalesQuantityPlanned == 0 ? null :
                            (decimal?)
                            Math.Round((SaleEmployeeDTO.TotalDirectSalesQuantity.Value / SaleEmployeeDTO.TotalDirectSalesQuantityPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng số lượng đơn hàng trực tiếp
                    //kế hoạch
                    SaleEmployeeDTO.TotalDirectOrdersPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_DIRECT_SALES_ORDER.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalDirectOrdersPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalDirectOrders = SaleEmployeeDTO.TotalDirectOrdersPlanned == null ? null :
                            DirectSalesOrders == null ? 0 : (decimal?)DirectSalesOrders.Count();
                        //tỉ lệ
                        SaleEmployeeDTO.TotalDirectOrdersRatio = SaleEmployeeDTO.TotalDirectOrdersPlanned == null || SaleEmployeeDTO.TotalDirectOrders == null || SaleEmployeeDTO.TotalDirectOrdersPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalDirectOrders.Value / SaleEmployeeDTO.TotalDirectOrdersPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng doanh thu đơn hàng gián tiếp
                    //kế hoạch
                    SaleEmployeeDTO.TotalIndirectSalesAmountPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_INDIRECT_SALES_AMOUNT.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalIndirectSalesAmount = SaleEmployeeDTO.TotalIndirectSalesAmountPlanned == null ? null :
                            IndirectSalesOrders == null ? 0 : (decimal?)IndirectSalesOrders.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.TotalIndirectSalesAmountRatio = SaleEmployeeDTO.TotalIndirectSalesAmountPlanned == null || SaleEmployeeDTO.TotalIndirectSalesAmount == null || SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalIndirectSalesAmount.Value / SaleEmployeeDTO.TotalIndirectSalesAmountPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2 Trọng điểm
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2TDPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_TD.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2TDPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2TD = SaleEmployeeDTO.RevenueC2TDPlanned == null ?
                        null : IndirectSalesOrdersC2TD == null ? 0 : (decimal?)IndirectSalesOrdersC2TD.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2TDRatio = SaleEmployeeDTO.RevenueC2TDPlanned == null || SaleEmployeeDTO.RevenueC2TD == null || SaleEmployeeDTO.RevenueC2TDPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2TD.Value / SaleEmployeeDTO.RevenueC2TDPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2 Siêu lớn
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2SLPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2_SL.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2SLPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2SL = SaleEmployeeDTO.RevenueC2SLPlanned == null ?
                        null : IndirectSalesOrdersC2SL == null ? 0 : (decimal?)IndirectSalesOrdersC2SL.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2SLRatio = SaleEmployeeDTO.RevenueC2SLPlanned == null || SaleEmployeeDTO.RevenueC2SL == null || SaleEmployeeDTO.RevenueC2SLPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2SL.Value / SaleEmployeeDTO.RevenueC2SLPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Doanh thu C2
                    //kế hoạch
                    SaleEmployeeDTO.RevenueC2Planned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.REVENUE_C2.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.RevenueC2Planned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.RevenueC2 = SaleEmployeeDTO.RevenueC2Planned == null ?
                        null : IndirectSalesOrdersC2 == null ? 0 : (decimal?)IndirectSalesOrdersC2.Sum(iso => iso.Total);
                        //tỉ lệ
                        SaleEmployeeDTO.RevenueC2Ratio = SaleEmployeeDTO.RevenueC2Planned == null || SaleEmployeeDTO.RevenueC2 == null || SaleEmployeeDTO.RevenueC2Planned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.RevenueC2.Value / SaleEmployeeDTO.RevenueC2Planned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số đại lý ghé thăm
                    //kế hoạch
                    SaleEmployeeDTO.StoresVisitedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                           .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                            x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.STORE_VISITED.Id)
                           .Select(x => x.Value).FirstOrDefault();
                    //thực hiện
                    if (SaleEmployeeDTO.StoresVisitedPlanned.HasValue)
                    {
                        var StoreIds = StoreCheckingDAOs
                            .Where(sc => sc.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                            .Select(x => x.StoreId)
                            .ToList();
                        SaleEmployeeDTO.StoreIds = new HashSet<long>();
                        foreach (var StoreId in StoreIds)
                        {
                            SaleEmployeeDTO.StoreIds.Add(StoreId);
                        }
                        //if (SaleEmployeeDTO.StoresVisitedPlanned == 0)
                        //    SaleEmployeeDTO.StoreIds = new HashSet<long>();
                        //tỉ lệ
                        SaleEmployeeDTO.StoresVisitedRatio = SaleEmployeeDTO.StoresVisitedPlanned == null || SaleEmployeeDTO.StoresVisited == null || SaleEmployeeDTO.StoresVisitedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.StoresVisited.Value / SaleEmployeeDTO.StoresVisitedPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Tổng số đại lý mở mới
                    //kế hoạch
                    SaleEmployeeDTO.NewStoreCreatedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId
                             && x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_CREATED.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NewStoreCreatedPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NewStoreCreated = SaleEmployeeDTO.NewStoreCreatedPlanned == null
                            ? null
                            : StoreDAOs == null ? 0 : (decimal?)StoreDAOs
                            .Where(st => st.CreatorId == SaleEmployeeDTO.SaleEmployeeId
                                    && st.DeletedAt == null)
                            .Count(); // lấy ra tất cả cửa hàng có người tạo là saleEmployee, trạng thái là dự thảo hoặc chính thức
                        //tỉ lệ
                        SaleEmployeeDTO.NewStoreCreatedRatio = SaleEmployeeDTO.NewStoreCreatedPlanned == null || SaleEmployeeDTO.NewStoreCreated == null || SaleEmployeeDTO.NewStoreCreatedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NewStoreCreated.Value / SaleEmployeeDTO.NewStoreCreatedPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Tổng số lượt ghé thăm
                    //kế hoạch
                    SaleEmployeeDTO.NumberOfStoreVisitsPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NUMBER_OF_STORE_VISIT.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NumberOfStoreVisitsPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NumberOfStoreVisits = SaleEmployeeDTO.NumberOfStoreVisitsPlanned == null
                            ? null
                            : StoreCheckingDAOs == null ? 0 : (decimal?)StoreCheckingDAOs
                            .Where(sc => sc.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId)
                            .Count();
                        //tỉ lệ
                        SaleEmployeeDTO.NumberOfStoreVisitsRatio = SaleEmployeeDTO.NumberOfStoreVisitsPlanned == null || SaleEmployeeDTO.NumberOfStoreVisits == null || SaleEmployeeDTO.NumberOfStoreVisitsPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NumberOfStoreVisits.Value / SaleEmployeeDTO.NumberOfStoreVisitsPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Số đại lý trọng điểm mở mới
                    //kế hoạch
                    SaleEmployeeDTO.NewStoreC2CreatedPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId
                             && x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.NEW_STORE_C2_CREATED.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.NewStoreC2CreatedPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.NewStoreC2Created = SaleEmployeeDTO.NewStoreC2CreatedPlanned == null
                            ? null
                            : StoreDAOs == null ? 0 : (decimal?)StoreDAOs
                            .Where(st => st.CreatorId == SaleEmployeeDTO.SaleEmployeeId &&
                            st.StoreTypeId == C2TD_ID
                            && st.DeletedAt == null)
                            .Count(); // lấy ra tất cả cửa hàng có người tạo là saleEmployee, trạng thái là dự thảo hoặc chính thức
                        //tỉ lệ
                        SaleEmployeeDTO.NewStoreC2CreatedRatio = SaleEmployeeDTO.NewStoreC2CreatedPlanned == null || SaleEmployeeDTO.NewStoreC2Created == null || SaleEmployeeDTO.NewStoreC2CreatedPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round(SaleEmployeeDTO.NewStoreC2Created.Value / SaleEmployeeDTO.NewStoreC2CreatedPlanned.Value * 100, 2);
                    }
                    #endregion

                    #region Số thông tin phản ánh
                    //kế hoạch
                    SaleEmployeeDTO.TotalProblemPlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_PROBLEM.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalProblemPlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalProblem = SaleEmployeeDTO.TotalProblemPlanned == null ?
                        null : ProblemDAOs == null ? 0 : (decimal?)ProblemDAOs.Where(x => x.CreatorId == SaleEmployeeDTO.SaleEmployeeId).Count();
                        //tỉ lệ
                        SaleEmployeeDTO.TotalProblemRatio = SaleEmployeeDTO.TotalProblemPlanned == null || SaleEmployeeDTO.TotalProblem == null || SaleEmployeeDTO.TotalProblemPlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalProblem.Value / SaleEmployeeDTO.TotalProblemPlanned.Value) * 100, 2);
                    }
                    #endregion

                    #region Số hình ảnh chụp
                    //kế hoạch
                    SaleEmployeeDTO.TotalImagePlanned = KpiGeneralPeriodReport_SaleEmployeeDetailDTOs
                            .Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId &&
                             x.KpiCriteriaGeneralId == KpiCriteriaGeneralEnum.TOTAL_IMAGE.Id)
                            .Select(x => x.Value).FirstOrDefault();
                    if (SaleEmployeeDTO.TotalImagePlanned.HasValue)
                    {
                        //thực hiện
                        SaleEmployeeDTO.TotalImage = SaleEmployeeDTO.TotalImagePlanned == null ?
                        null : StoreImageDAOs == null ? 0 : (decimal?)StoreImageDAOs.Where(x => x.SaleEmployeeId == SaleEmployeeDTO.SaleEmployeeId).Count();
                        //tỉ lệ
                        SaleEmployeeDTO.TotalImageRatio = SaleEmployeeDTO.TotalImagePlanned == null || SaleEmployeeDTO.TotalImage == null || SaleEmployeeDTO.TotalImagePlanned.Value == 0
                            ? null
                            : (decimal?)Math.Round((SaleEmployeeDTO.TotalImage.Value / SaleEmployeeDTO.TotalImagePlanned.Value) * 100, 2);
                    }
                    #endregion
                }
            };

            return KpiGeneralPeriodReport_KpiGeneralPeriodReportDTOs;
        }


    }
}
