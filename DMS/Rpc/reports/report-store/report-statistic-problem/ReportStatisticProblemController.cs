using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Services.MOrganization;
using Microsoft.AspNetCore.Mvc;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using System;
using DMS.Helpers;
using Microsoft.EntityFrameworkCore;
using DMS.Services.MProduct;
using System.IO;
using System.Dynamic;
using NGS.Templater;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using System.Diagnostics;
using DMS.Services.MStoreStatus;
using TrueSight.Common;
using DMS.Services.MAppUser;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_store.report_statistic_problem
{
    public class ReportStatisticProblemController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IItemService ItemService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IStoreStatusService StoreStatusService;
        private ICurrentContext CurrentContext;
        public ReportStatisticProblemController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IItemService ItemService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IStoreStatusService StoreStatusService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.ItemService = ItemService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.StoreStatusService = StoreStatusService;
            this.CurrentContext = CurrentContext;
        }

        #region Filter List
        [Route(ReportStatisticProblemRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportStatisticProblem_OrganizationDTO>> FilterListOrganization()
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
            List<ReportStatisticProblem_OrganizationDTO> StoreCheckerReport_OrganizationDTOs = Organizations
                .Select(x => new ReportStatisticProblem_OrganizationDTO(x)).ToList();
            return StoreCheckerReport_OrganizationDTOs;
        }

        [Route(ReportStatisticProblemRoute.FilterListStore), HttpPost]
        public async Task<List<ReportStatisticProblem_StoreDTO>> FilterListStore([FromBody] ReportStatisticProblem_StoreFilterDTO ReportStatisticProblem_StoreFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreFilter StoreFilter = new StoreFilter();
            StoreFilter.Skip = 0;
            StoreFilter.Take = 20;
            StoreFilter.OrderBy = StoreOrder.Id;
            StoreFilter.OrderType = OrderType.ASC;
            StoreFilter.Selects = StoreSelect.ALL;
            StoreFilter.Id = ReportStatisticProblem_StoreFilterDTO.Id;
            StoreFilter.Code = ReportStatisticProblem_StoreFilterDTO.Code;
            StoreFilter.Name = ReportStatisticProblem_StoreFilterDTO.Name;
            StoreFilter.CodeDraft = ReportStatisticProblem_StoreFilterDTO.CodeDraft;
            StoreFilter.OrganizationId = ReportStatisticProblem_StoreFilterDTO.OrganizationId;
            StoreFilter.StoreTypeId = ReportStatisticProblem_StoreFilterDTO.StoreTypeId;
            StoreFilter.StoreGroupingId = ReportStatisticProblem_StoreFilterDTO.StoreGroupingId;
            StoreFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreFilter.Id == null) StoreFilter.Id = new IdFilter();
            StoreFilter.Id.In = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<Store> Stores = await StoreService.List(StoreFilter);
            List<ReportStatisticProblem_StoreDTO> ReportStatisticProblem_StoreDTOs = Stores
                .Select(x => new ReportStatisticProblem_StoreDTO(x)).ToList();
            return ReportStatisticProblem_StoreDTOs;
        }

        [Route(ReportStatisticProblemRoute.FilterListStoreGrouping), HttpPost]
        public async Task<List<ReportStatisticProblem_StoreGroupingDTO>> FilterListStoreGrouping([FromBody] ReportStatisticProblem_StoreGroupingFilterDTO ReportStatisticProblem_StoreGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreGroupingFilter StoreGroupingFilter = new StoreGroupingFilter();
            StoreGroupingFilter.Skip = 0;
            StoreGroupingFilter.Take = 99999;
            StoreGroupingFilter.OrderBy = StoreGroupingOrder.Id;
            StoreGroupingFilter.OrderType = OrderType.ASC;
            StoreGroupingFilter.Selects = StoreGroupingSelect.ALL;
            StoreGroupingFilter.Code = ReportStatisticProblem_StoreGroupingFilterDTO.Code;
            StoreGroupingFilter.Name = ReportStatisticProblem_StoreGroupingFilterDTO.Name;
            StoreGroupingFilter.Level = ReportStatisticProblem_StoreGroupingFilterDTO.Level;
            StoreGroupingFilter.Path = ReportStatisticProblem_StoreGroupingFilterDTO.Path;
            StoreGroupingFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreGroupingFilter.Id == null) StoreGroupingFilter.Id = new IdFilter();
            StoreGroupingFilter.Id.In = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(StoreGroupingFilter);
            List<ReportStatisticProblem_StoreGroupingDTO> ReportStatisticProblem_StoreGroupingDTOs = StoreGroupings
                .Select(x => new ReportStatisticProblem_StoreGroupingDTO(x)).ToList();
            return ReportStatisticProblem_StoreGroupingDTOs;
        }
        [Route(ReportStatisticProblemRoute.FilterListStoreType), HttpPost]
        public async Task<List<ReportStatisticProblem_StoreTypeDTO>> FilterListStoreType([FromBody] ReportStatisticProblem_StoreTypeFilterDTO ReportStatisticProblem_StoreTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreTypeFilter StoreTypeFilter = new StoreTypeFilter();
            StoreTypeFilter.Skip = 0;
            StoreTypeFilter.Take = 20;
            StoreTypeFilter.OrderBy = StoreTypeOrder.Id;
            StoreTypeFilter.OrderType = OrderType.ASC;
            StoreTypeFilter.Selects = StoreTypeSelect.ALL;
            StoreTypeFilter.Id = ReportStatisticProblem_StoreTypeFilterDTO.Id;
            StoreTypeFilter.Code = ReportStatisticProblem_StoreTypeFilterDTO.Code;
            StoreTypeFilter.Name = ReportStatisticProblem_StoreTypeFilterDTO.Name;
            StoreTypeFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (StoreTypeFilter.Id == null) StoreTypeFilter.Id = new IdFilter();
            StoreTypeFilter.Id.In = await FilterStoreType(StoreTypeService, CurrentContext);
            List<StoreType> StoreTypes = await StoreTypeService.List(StoreTypeFilter);
            List<ReportStatisticProblem_StoreTypeDTO> ReportStatisticProblem_StoreTypeDTOs = StoreTypes
                .Select(x => new ReportStatisticProblem_StoreTypeDTO(x)).ToList();
            return ReportStatisticProblem_StoreTypeDTOs;
        }


        [Route(ReportStatisticProblemRoute.FilterListStoreStatus), HttpPost]
        public async Task<List<ReportStatisticProblem_StoreStatusDTO>> FilterListStoreStatus([FromBody] ReportStatisticProblem_StoreStatusFilterDTO ReportStatisticProblem_StoreStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreStatusFilter StoreStatusFilter = new StoreStatusFilter();
            StoreStatusFilter.Skip = 0;
            StoreStatusFilter.Take = 20;
            StoreStatusFilter.OrderBy = StoreStatusOrder.Id;
            StoreStatusFilter.OrderType = OrderType.ASC;
            StoreStatusFilter.Selects = StoreStatusSelect.ALL;
            StoreStatusFilter.Id = ReportStatisticProblem_StoreStatusFilterDTO.Id;
            StoreStatusFilter.Code = ReportStatisticProblem_StoreStatusFilterDTO.Code;
            StoreStatusFilter.Name = ReportStatisticProblem_StoreStatusFilterDTO.Name;

            List<StoreStatus> StoreStatuses = await StoreStatusService.List(StoreStatusFilter);
            List<ReportStatisticProblem_StoreStatusDTO> ReportStatisticProblem_StoreStatusDTOs = StoreStatuses
                .Select(x => new ReportStatisticProblem_StoreStatusDTO(x)).ToList();
            return ReportStatisticProblem_StoreStatusDTOs;
        }
        #endregion

        [Route(ReportStatisticProblemRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? StoreId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreStatusId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            //ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
            //           .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            //var query = from p in DataContext.Problem
            //            join s in DataContext.Store on p.StoreId equals s.Id
            //            join tt in tempTableQuery.Query on s.Id equals tt.Column1
            //            join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
            //            from q in subquery.DefaultIfEmpty()
            //            where p.NoteAt >= Start && p.NoteAt <= End &&
            //            StoreTypeIds.Contains(s.StoreTypeId) &&
            //            (
            //                (
            //                    StoreGroupingId.HasValue == false &&
            //                    (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
            //                ) ||
            //                (
            //                    StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
            //                )
            //            ) &&
            //            (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
            //            OrganizationIds.Contains(s.OrganizationId) &&
            //            AppUserIds.Contains(p.CreatorId) &&
            //            s.DeletedAt == null
            //            select s;

            var StoreStoreGroupingMappingQuery = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            var MappedStoreIds = await StoreStoreGroupingMappingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            StoreQuery = StoreQuery.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeId });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id)
                StoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            if (!StoreGroupingId.HasValue)
            {
                var sub1 = StoreQuery.Where(x => x.StoreId, new IdFilter { NotIn = MappedStoreIds });
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds }).Select(x => x.StoreId).ToListWithNoLockAsync();
                var sub2 = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
                StoreQuery = sub1.Union(sub2);
            }
            else
            {
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingId.Value }).Select(x => x.StoreId).ToListWithNoLockAsync();
                StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            }
            StoreIds = await StoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();        

            var ProblemQuery = DWContext.Fact_Problem.AsNoTracking();
            ProblemQuery = ProblemQuery.Where(x => x.CreatorId, new IdFilter { In = AppUserIds });
            ProblemQuery = ProblemQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            ProblemQuery = ProblemQuery.Where(x => x.NoteAt, new DateFilter { LessEqual = End, GreaterEqual = Start });
            int count = await ProblemQuery.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportStatisticProblemRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportStatisticProblem_ReportStatisticProblemDTO>>> List([FromBody] ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.HasValue == false)
                return new List<ReportStatisticProblem_ReportStatisticProblemDTO>();

            DateTime Start = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportStatisticProblem_ReportStatisticProblemDTO> ReportStatisticProblem_ReportStatisticProblemDTOs = await ListData(ReportStatisticProblem_ReportStatisticProblemFilterDTO, Start, End);
            return ReportStatisticProblem_ReportStatisticProblemDTOs;
        }

        [Route(ReportStatisticProblemRoute.Total), HttpPost]
        public async Task<ReportStatisticProblem_TotalDTO> Total([FromBody] ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.HasValue == false)
                return new ReportStatisticProblem_TotalDTO();

            DateTime Start = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportStatisticProblem_TotalDTO();

            ReportStatisticProblem_TotalDTO ReportStatisticProblem_TotalDTO = await TotalData(ReportStatisticProblem_ReportStatisticProblemFilterDTO, Start, End);
            return ReportStatisticProblem_TotalDTO;
        }

        [Route(ReportStatisticProblemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.GreaterEqual == null ?
               LocalStartDay(CurrentContext) :
               ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportStatisticProblem_ReportStatisticProblemFilterDTO.Date.LessEqual.Value;

            ReportStatisticProblem_ReportStatisticProblemFilterDTO.Skip = 0;
            ReportStatisticProblem_ReportStatisticProblemFilterDTO.Take = int.MaxValue;
            List<ReportStatisticProblem_ReportStatisticProblemDTO> ReportStatisticProblem_ReportStatisticProblemDTOs = await ListData(ReportStatisticProblem_ReportStatisticProblemFilterDTO, Start, End);

            ReportStatisticProblem_TotalDTO ReportStatisticProblem_TotalDTO = await TotalData(ReportStatisticProblem_ReportStatisticProblemFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportStatisticProblem_ReportStatisticProblemDTO ReportStatisticProblem_ReportStatisticProblemDTO in ReportStatisticProblem_ReportStatisticProblemDTOs)
            {
                foreach (var Store in ReportStatisticProblem_ReportStatisticProblemDTO.Stores)
                {
                    Store.STT = stt;
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

            string path = "Templates/Report_Statistic_Problem.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportStatisticProblems = ReportStatisticProblem_ReportStatisticProblemDTOs;
            Data.Total = ReportStatisticProblem_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportStatisticProblem.xlsx");
        }

        private async Task<List<ReportStatisticProblem_ReportStatisticProblemDTO>> ListData(
            ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreStatusId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            //ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
            //           .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            //var query = from p in DataContext.Problem
            //            join s in DataContext.Store on p.StoreId equals s.Id
            //            join tt in tempTableQuery.Query on s.Id equals tt.Column1
            //            join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
            //            from q in subquery.DefaultIfEmpty()
            //            where p.NoteAt >= Start && p.NoteAt <= End &&
            //            AppUserIds.Contains(p.CreatorId) &&
            //            StoreTypeIds.Contains(s.StoreTypeId) &&
            //            (
            //                (
            //                    StoreGroupingId.HasValue == false &&
            //                    (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
            //                ) ||
            //                (
            //                    StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
            //                )
            //            ) &&
            //            (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
            //            (OrganizationIds.Contains(s.OrganizationId)) &&
            //            s.DeletedAt == null
            //            select new Store
            //            {
            //                Id = s.Id,
            //                Code = s.Code,
            //                CodeDraft = s.CodeDraft,
            //                Name = s.Name,
            //                Address = s.Address,
            //                Telephone = s.Telephone,
            //                OrganizationId = s.OrganizationId,
            //                StoreStatusId = s.StoreStatusId
            //            };
            var StoreStoreGroupingMappingQuery = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            var MappedStoreIds = await StoreStoreGroupingMappingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            StoreQuery = StoreQuery.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeId });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id)
                StoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            if (!StoreGroupingId.HasValue)
            {
                var sub1 = StoreQuery.Where(x => x.StoreId, new IdFilter { NotIn = MappedStoreIds });
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds }).Select(x => x.StoreId).ToListWithNoLockAsync();
                var sub2 = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
                StoreQuery = sub1.Union(sub2);
            }
            else
            {
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingId.Value }).Select(x => x.StoreId).ToListWithNoLockAsync();
                StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            }
            StoreIds = await StoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var ProblemQuery = DWContext.Fact_Problem.AsNoTracking();
            ProblemQuery = ProblemQuery.Where(x => x.CreatorId, new IdFilter { In = AppUserIds });
            ProblemQuery = ProblemQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            ProblemQuery = ProblemQuery.Where(x => x.NoteAt, new DateFilter { LessEqual = End, GreaterEqual = Start });
            StoreIds = await ProblemQuery.Select(x => x.StoreId).ToListWithNoLockAsync();
            StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            var query = StoreQuery.Select(s => new Store
            {
                Id = s.StoreId,
                Code = s.Code,
                CodeDraft = s.CodeDraft,
                Name = s.Name,
                Address = s.Address,
                Telephone = s.Telephone,
                OrganizationId = s.OrganizationId,
                StoreStatusId = s.StoreStatusId
            });
            List<Store> Stores = await query.Distinct().OrderBy(x => x.OrganizationId).ThenBy(x => x.Name)
                .Skip(ReportStatisticProblem_ReportStatisticProblemFilterDTO.Skip)
                .Take(ReportStatisticProblem_ReportStatisticProblemFilterDTO.Take)
                .ToListWithNoLockAsync();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();

            OrganizationDAOs = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();
            List<ReportStatisticProblem_ReportStatisticProblemDTO> ReportStatisticProblem_ReportStatisticProblemDTOs = OrganizationDAOs.Select(on => new ReportStatisticProblem_ReportStatisticProblemDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach(var row in ReportStatisticProblem_ReportStatisticProblemDTOs)
            {
                row.Stores = Stores
                    .Where(x => x.OrganizationId == row.OrganizationId)
                    .Select(x => new ReportStatisticProblem_StoreDTO
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CodeDraft = x.CodeDraft,
                        Name = x.Name,
                        Address = x.Address,
                        Phone = x.Telephone,
                        OrganizationId = x.OrganizationId,
                        StoreTypeId = x.StoreTypeId,
                        StoreStatusId = x.StoreStatusId,
                        StoreStatus = new ReportStatisticProblem_StoreStatusDTO
                        {
                            Name = StoreStatusEnum.StoreStatusEnumList.Where(y => y.Id == x.StoreStatusId).Select(z => z.Name).FirstOrDefault()
                        }
                    })
                    .ToList();
            }
            StoreIds = Stores.Select(s => s.Id).ToList();
            List<ProblemDAO> ProblemDAOs = await DataContext.Problem
                .Where(x => StoreIds.Contains(x.StoreId) &&
                Start <= x.NoteAt && x.NoteAt <= End &&
                AppUserIds.Contains(x.CreatorId))
                .ToListWithNoLockAsync();
            List<ProblemTypeDAO> ProblemTypeDAOs = await DataContext.ProblemType
                .Where(x => x.DeletedAt == null)
                .ToListWithNoLockAsync();

            // khởi tạo khung dữ liệu
            foreach (ReportStatisticProblem_ReportStatisticProblemDTO ReportStatisticProblem_ReportStatisticProblemDTO in ReportStatisticProblem_ReportStatisticProblemDTOs)
            {
                foreach(var Store in ReportStatisticProblem_ReportStatisticProblemDTO.Stores)
                {
                    var Problems = ProblemDAOs.Where(x => x.StoreId == Store.Id).ToList();
                    var ProblemTypeIds = Problems.Select(x => x.ProblemTypeId).Distinct();
                    if (Store.Contents == null)
                        Store.Contents = new List<ReportStatisticProblem_ContentDTO>();

                    foreach (var ProblemTypeId in ProblemTypeIds)
                    {
                        var ProblemType = ProblemTypeDAOs.Where(x => x.Id == ProblemTypeId).FirstOrDefault();
                        if (ProblemType == null)
                            continue;

                        ReportStatisticProblem_ContentDTO ReportStatisticProblem_ContentDTO = new ReportStatisticProblem_ContentDTO();
                        ReportStatisticProblem_ContentDTO.ProblemTypeId = ProblemTypeId;
                        ReportStatisticProblem_ContentDTO.ProblemTypeName = ProblemType.Name;
                        ReportStatisticProblem_ContentDTO.WaitingCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.WAITING.Id)
                            .Count();
                        ReportStatisticProblem_ContentDTO.ProcessCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.PROCESSING.Id)
                            .Count();
                        ReportStatisticProblem_ContentDTO.CompletedCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.DONE.Id)
                            .Count();
                        Store.Contents.Add(ReportStatisticProblem_ContentDTO);
                    }
                }
            }

            return ReportStatisticProblem_ReportStatisticProblemDTOs;
        }

        private async Task<ReportStatisticProblem_TotalDTO> TotalData(
            ReportStatisticProblem_ReportStatisticProblemFilterDTO ReportStatisticProblem_ReportStatisticProblemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? StoreId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreId?.Equal;
            long? StoreTypeId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreGroupingId?.Equal;
            long? StoreStatusId = ReportStatisticProblem_ReportStatisticProblemFilterDTO.StoreStatusId?.Equal;

            ReportStatisticProblem_TotalDTO ReportStatisticProblem_TotalDTO = new ReportStatisticProblem_TotalDTO();
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportStatisticProblem_ReportStatisticProblemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            if (StoreId.HasValue)
            {
                var listId = new List<long> { StoreId.Value };
                StoreIds = StoreIds.Intersect(listId).ToList();
            }
            List<long> StoreTypeIds = await FilterStoreType(StoreTypeService, CurrentContext);
            if (StoreTypeId.HasValue)
            {
                var listId = new List<long> { StoreTypeId.Value };
                StoreTypeIds = StoreTypeIds.Intersect(listId).ToList();
            }
            List<long> StoreGroupingIds = await FilterStoreGrouping(StoreGroupingService, CurrentContext);
            if (StoreGroupingId.HasValue)
            {
                var listId = new List<long> { StoreGroupingId.Value };
                StoreGroupingIds = StoreGroupingIds.Intersect(listId).ToList();
            }
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            //ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
            //           .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            //var query = from p in DataContext.Problem
            //            join s in DataContext.Store on p.StoreId equals s.Id
            //            join tt in tempTableQuery.Query on s.Id equals tt.Column1
            //            join sg in DataContext.StoreStoreGroupingMapping on s.Id equals sg.StoreId into subquery
            //            from q in subquery.DefaultIfEmpty()
            //            where p.NoteAt >= Start && p.NoteAt <= End &&
            //            AppUserIds.Contains(p.CreatorId) &&
            //            StoreTypeIds.Contains(s.StoreTypeId) &&
            //            (
            //                (
            //                    StoreGroupingId.HasValue == false &&
            //                    (q == null || StoreGroupingIds.Contains(q.StoreGroupingId))
            //                ) ||
            //                (
            //                    StoreGroupingId.HasValue && StoreGroupingId.Value == q.StoreGroupingId
            //                )
            //            ) &&
            //            (StoreStatusId.HasValue == false || StoreStatusId.Value == StoreStatusEnum.ALL.Id || s.StoreStatusId == StoreStatusId.Value) &&
            //            (OrganizationIds.Contains(s.OrganizationId)) &&
            //            s.DeletedAt == null
            //            select new Store
            //            {
            //                Id = s.Id,
            //                Code = s.Code,
            //                CodeDraft = s.CodeDraft,
            //                Name = s.Name,
            //                Address = s.Address,
            //                OrganizationId = s.OrganizationId,
            //            };

            var StoreStoreGroupingMappingQuery = DataContext.StoreStoreGroupingMapping.AsNoTracking();
            var MappedStoreIds = await StoreStoreGroupingMappingQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.DeletedAt == null);
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            StoreQuery = StoreQuery.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeId });
            StoreQuery = StoreQuery.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            if (StoreStatusId.HasValue && StoreStatusId != StoreStatusEnum.ALL.Id)
                StoreQuery = StoreQuery.Where(x => x.StoreStatusId, new IdFilter { Equal = StoreStatusId });
            if (!StoreGroupingId.HasValue)
            {
                var sub1 = StoreQuery.Where(x => x.StoreId, new IdFilter { NotIn = MappedStoreIds });
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { In = StoreGroupingIds }).Select(x => x.StoreId).ToListWithNoLockAsync();
                var sub2 = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
                StoreQuery = sub1.Union(sub2);
            }
            else
            {
                var storeIds = await StoreStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingId.Value }).Select(x => x.StoreId).ToListWithNoLockAsync();
                StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = storeIds });
            }
            StoreIds = await StoreQuery.Select(x => x.StoreId).ToListWithNoLockAsync();

            var ProblemQuery = DWContext.Fact_Problem.AsNoTracking();
            ProblemQuery = ProblemQuery.Where(x => x.CreatorId, new IdFilter { In = AppUserIds });
            ProblemQuery = ProblemQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            ProblemQuery = ProblemQuery.Where(x => x.NoteAt, new DateFilter { LessEqual = End, GreaterEqual = Start });
            StoreIds = await ProblemQuery.Select(x => x.StoreId).ToListWithNoLockAsync();
            StoreQuery = DWContext.Dim_Store.AsNoTracking();
            StoreQuery = StoreQuery.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            var query = StoreQuery.Select(s => new Store
            {
                Id = s.StoreId,
                Code = s.Code,
                CodeDraft = s.CodeDraft,
                Name = s.Name,
                Address = s.Address,
                OrganizationId = s.OrganizationId,
            });
            List<Store> Stores = await query.Distinct().ToListWithNoLockAsync();
            OrganizationIds = Stores.Select(x => x.OrganizationId).Distinct().ToList();

            OrganizationDAOs = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).ToListWithNoLockAsync();
            List<ReportStatisticProblem_ReportStatisticProblemDTO> ReportStatisticProblem_ReportStatisticProblemDTOs = OrganizationDAOs.Select(on => new ReportStatisticProblem_ReportStatisticProblemDTO
            {
                OrganizationId = on.Id,
                OrganizationName = on.Name,
            }).ToList();
            foreach(var row in ReportStatisticProblem_ReportStatisticProblemDTOs)
            {
                row.Stores = Stores
                    .Where(x => x.OrganizationId == row.OrganizationId)
                    .Select(x => new ReportStatisticProblem_StoreDTO
                    {
                        Id = x.Id,
                        OrganizationId = x.OrganizationId,
                    })
                    .ToList();
            }

            StoreIds = Stores.Select(s => s.Id).ToList();
            List<ProblemDAO> ProblemDAOs = await DataContext.Problem
                .Where(x => StoreIds.Contains(x.StoreId) &&
                Start <= x.NoteAt && x.NoteAt <= End &&
                AppUserIds.Contains(x.CreatorId))
                .ToListWithNoLockAsync();
            List<ProblemTypeDAO> ProblemTypeDAOs = await DataContext.ProblemType
                .Where(x => x.DeletedAt == null)
                .ToListWithNoLockAsync();

            // khởi tạo khung dữ liệu
            foreach (ReportStatisticProblem_ReportStatisticProblemDTO ReportStatisticProblem_ReportStatisticProblemDTO in ReportStatisticProblem_ReportStatisticProblemDTOs)
            {
                foreach(var Store in ReportStatisticProblem_ReportStatisticProblemDTO.Stores)
                {
                    var Problems = ProblemDAOs.Where(x => x.StoreId == Store.Id).ToList();
                    var ProblemTypeIds = Problems.Select(x => x.ProblemTypeId).Distinct();
                    if (Store.Contents == null)
                        Store.Contents = new List<ReportStatisticProblem_ContentDTO>();

                    foreach (var ProblemTypeId in ProblemTypeIds)
                    {
                        var ProblemType = ProblemTypeDAOs.Where(x => x.Id == ProblemTypeId).FirstOrDefault();
                        if (ProblemType == null)
                            continue;

                        ReportStatisticProblem_ContentDTO ReportStatisticProblem_ContentDTO = new ReportStatisticProblem_ContentDTO();
                        ReportStatisticProblem_ContentDTO.ProblemTypeId = ProblemTypeId;
                        ReportStatisticProblem_ContentDTO.ProblemTypeName = ProblemType.Name;
                        ReportStatisticProblem_ContentDTO.WaitingCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.WAITING.Id)
                            .Count();
                        ReportStatisticProblem_ContentDTO.ProcessCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.PROCESSING.Id)
                            .Count();
                        ReportStatisticProblem_ContentDTO.CompletedCounter = Problems
                            .Where(x => x.ProblemTypeId == ProblemTypeId)
                            .Where(x => x.ProblemStatusId == ProblemStatusEnum.DONE.Id)
                            .Count();
                        Store.Contents.Add(ReportStatisticProblem_ContentDTO);
                    }
                }
            }

            ReportStatisticProblem_TotalDTO.WaitingCounter = ReportStatisticProblem_ReportStatisticProblemDTOs.SelectMany(x => x.Stores)
                .SelectMany(x => x.Contents).Sum(x => x.WaitingCounter);
            ReportStatisticProblem_TotalDTO.ProcessCounter = ReportStatisticProblem_ReportStatisticProblemDTOs.SelectMany(x => x.Stores)
                .SelectMany(x => x.Contents).Sum(x => x.ProcessCounter);
            ReportStatisticProblem_TotalDTO.CompletedCounter = ReportStatisticProblem_ReportStatisticProblemDTOs.SelectMany(x => x.Stores)
                .SelectMany(x => x.Contents).Sum(x => x.CompletedCounter);
            return ReportStatisticProblem_TotalDTO;
        }

    }
}
