using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MShowingItem;
using DMS.Services.MShowingOrder;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public partial class POSMReportController : RpcController
    {
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private IShowingItemService ShowingItemService;
        private IShowingOrderService ShowingOrderService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private DataContext DataContext;
        private DWContext DWContext;
        private ICurrentContext CurrentContext;
        public POSMReportController(
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            IShowingItemService ShowingItemService,
            IShowingOrderService ShowingOrderService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            DataContext DataContext,
            DWContext DWContext,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.ShowingItemService = ShowingItemService;
            this.ShowingOrderService = ShowingOrderService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.CurrentContext = CurrentContext;
        }

        [Route(POSMReportRoute.Count), HttpPost]
        public async Task<ActionResult<long>> Count([FromBody] POSMReport_POSMReportFilterDTO POSMReport_ShowingOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (POSMReport_ShowingOrderFilterDTO.HasValue == false)
                return 0;

            DateTime Start = POSMReport_ShowingOrderFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    POSMReport_ShowingOrderFilterDTO.Date.GreaterEqual.Value;
            DateTime End = POSMReport_ShowingOrderFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    POSMReport_ShowingOrderFilterDTO.Date.LessEqual.Value;
            var DateIds = await DWContext.Dim_Date.Where(x => x.Date >= Start && x.Date <= End)
                    .Select(x => x.DateId).ToListWithNoLockAsync();
            if (End.Subtract(Start).Days > 31)
                return 0;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (POSMReport_ShowingOrderFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == POSMReport_ShowingOrderFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            List<long> AppUserFilter = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<long> AppUserIds = await DWContext.Dim_AppUser.Where(x => OrganizationIds.Contains(x.OrganizationId))
                .Select(x => x.AppUserId).ToListWithNoLockAsync();
            AppUserIds = AppUserIds.Intersect(AppUserFilter).ToList();

            List<long> StoreFilter = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            List<long> StoreTypeFilter = await FilterStoreType(StoreTypeService, CurrentContext);
            List<long> StoreGroupingFilter = await FilterStoreGrouping(StoreGroupingService, CurrentContext);

            long? StoreId = POSMReport_ShowingOrderFilterDTO.StoreId?.Equal;
            long? StoreTypeId = POSMReport_ShowingOrderFilterDTO.StoreTypeId?.Equal;
            long? StoreGroupingId = POSMReport_ShowingOrderFilterDTO.StoreGroupingId?.Equal;
            List<long> ShowingItemIds = POSMReport_ShowingOrderFilterDTO.ShowingItemId?.In;

            if (!StoreId.HasValue || !StoreFilter.Contains(StoreId.Value)) StoreId = null;
            if (!StoreTypeId.HasValue || !StoreTypeFilter.Contains(StoreTypeId.Value)) StoreId = null;
            if (!StoreGroupingId.HasValue || !StoreGroupingFilter.Contains(StoreGroupingId.Value)) StoreId = null;

            List<long> StoreIds = await (from sm in DWContext.Dim_StoreMapping
                                         where (!StoreId.HasValue || sm.StoreId == StoreId)
                                         && (!StoreTypeId.HasValue || sm.StoreTypeId == StoreTypeId)
                                         && (!StoreGroupingId.HasValue || sm.StoreGroupingId == StoreGroupingId)
                                         && OrganizationIds.Contains(sm.OrganizationId.Value)
                                         select sm.StoreId)
                                     .Distinct().ToListWithNoLockAsync();

            StoreIds = StoreIds.Intersect(StoreFilter).ToList();

            ITempTableQuery<TempTable<long>> StoreTemp = await DWContext
                       .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            ShowingItemIds = await DWContext.Dim_ShowingItem
                .Where(x => ShowingItemIds == null || ShowingItemIds.Count == 0 || ShowingItemIds.Contains(x.ShowingItemId))
                .Select(x => x.ShowingItemId).ToListWithNoLockAsync();

            var query = await (from t in DWContext.Fact_POSMTransaction
                               join s in StoreTemp.Query on t.StoreId equals s.Column1
                               select t)
                        .Where(x => ShowingItemIds.Contains(x.ShowingItemId))
                        .Where(x => x.Date >= Start && x.Date <= End)
                        .ToListWithNoLockAsync();
            return query.Count;
        }

        [Route(POSMReportRoute.List), HttpPost]
        public async Task<ActionResult<List<POSMReport_POSMReportDTO>>> List([FromBody] POSMReport_POSMReportFilterDTO POSMReport_POSMReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (POSMReport_POSMReportFilterDTO.HasValue == false)
                return new List<POSMReport_POSMReportDTO>();

            DateTime Start = POSMReport_POSMReportFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    POSMReport_POSMReportFilterDTO.Date.GreaterEqual.Value;

            DateTime End = POSMReport_POSMReportFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    POSMReport_POSMReportFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<POSMReport_POSMReportDTO> POSMReport_POSMReportDTOs = (await ListReportData(POSMReport_POSMReportFilterDTO, Start, End)).Value;

            return POSMReport_POSMReportDTOs;
        }

        [Route(POSMReportRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] POSMReport_POSMReportFilterDTO POSMReport_POSMReportFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = POSMReport_POSMReportFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    POSMReport_POSMReportFilterDTO.Date.GreaterEqual.Value;

            DateTime End = POSMReport_POSMReportFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    POSMReport_POSMReportFilterDTO.Date.LessEqual.Value;

            POSMReport_POSMReportFilterDTO.Skip = 0;
            POSMReport_POSMReportFilterDTO.Take = int.MaxValue;
            List<POSMReport_POSMReportDTO> POSMReport_POSMReportDTOs = (await ListReportData(POSMReport_POSMReportFilterDTO, Start, End)).Value;

            long stt = 1;
            foreach (POSMReport_POSMReportDTO POSMReport_POSMReportDTO in POSMReport_POSMReportDTOs)
            {
                foreach (var Store in POSMReport_POSMReportDTO.Stores)
                {
                    Store.STT = stt;
                    stt++;
                }
            }

            var Total = POSMReport_POSMReportDTOs.SelectMany(x => x.Stores).Select(x => x.Total).DefaultIfEmpty(0).Sum();
            var SumQuantity = POSMReport_POSMReportDTOs.SelectMany(x => x.Stores).SelectMany(x => x.Contents).Select(x => x.OrderQuantity).DefaultIfEmpty(0).Sum();

            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/POSM_Report.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Data = POSMReport_POSMReportDTOs;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            Data.Total = Total;
            Data.SumQuantity = SumQuantity;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "POSMReport.xlsx");
        }

        private async Task<ActionResult<List<POSMReport_POSMReportDTO>>> ListReportData(
            POSMReport_POSMReportFilterDTO POSMReport_ShowingOrderFilterDTO, DateTime Start, DateTime End)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (POSMReport_ShowingOrderFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == POSMReport_ShowingOrderFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            IdFilter OrganizationIdFilter = new IdFilter() { In = OrganizationIds };

            long? StoreId = POSMReport_ShowingOrderFilterDTO.StoreId?.Equal;
            IdFilter StoreIdFilter = new IdFilter() { Equal = StoreId };
            long? StoreTypeId = POSMReport_ShowingOrderFilterDTO.StoreTypeId?.Equal;
            IdFilter StoreTypeIdFilter = new IdFilter() { Equal = StoreTypeId };
            long? StoreGroupingId = POSMReport_ShowingOrderFilterDTO.StoreGroupingId?.Equal;
            IdFilter StoreGroupingIdFilter = new IdFilter() { Equal = StoreGroupingId };
            List<long> ShowingItemIds = POSMReport_ShowingOrderFilterDTO.ShowingItemId?.In;

            DateFilter OrderDateFilter = new DateFilter() { GreaterEqual = Start, LessEqual = End };
            DateFilter PreOrderDateFilter = new DateFilter() { LessEqual = Start.AddSeconds(-1) };

            var storeid_query = DWContext.Dim_StoreMapping.AsNoTracking();
            storeid_query = storeid_query.Where(x => x.StoreId, StoreIdFilter);
            storeid_query = storeid_query.Where(x => x.OrganizationId, OrganizationIdFilter);
            storeid_query = storeid_query.Where(x => x.StoreTypeId, StoreTypeIdFilter);
            storeid_query = storeid_query.Where(x => x.StoreGroupingId, StoreGroupingIdFilter);

            List<long> StoreIds = await storeid_query.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();

            List<long> StoreFilter = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);
            StoreIds = StoreIds.Intersect(StoreFilter).ToList();
            StoreIdFilter = new IdFilter() { In = StoreIds };

            ITempTableQuery<TempTable<long>> StoreTemp = await DWContext
                       .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);
            ShowingItemIds = await DWContext.Dim_ShowingItem
                .Where(x => ShowingItemIds == null || ShowingItemIds.Count == 0 || ShowingItemIds.Contains(x.ShowingItemId))
                .Select(x => x.ShowingItemId).ToListWithNoLockAsync();
            IdFilter ShowingItemIdFilter = new IdFilter() { In = ShowingItemIds };

            var POSMTransaction_query = DWContext.Fact_POSMTransaction.AsNoTracking();
            POSMTransaction_query = POSMTransaction_query.Where(x => x.StoreId, StoreIdFilter);
            POSMTransaction_query = POSMTransaction_query.Where(x => x.ShowingItemId, ShowingItemIdFilter);

            var POSMTransactionDAOs = await POSMTransaction_query.ToListWithNoLockAsync();
            //var POSMTransactionDAO_query = from transaction in POSMTransaction_query
            //            join shw in DWContext.Dim_ShowingItem on transaction.ShowingItemId equals shw.ShowingItemId
            //            join shwm in DWContext.Dim_ShowingItemMapping on transaction.ShowingItemId equals shwm.ShowingItemId
            //            join uom in DWContext.Dim_UnitOfMeasure on shwm.UnitOfMeasureId equals uom.UnitOfMeasureId
            //            select new
            //            {
            //                OrganizationId = transaction.OrganizationId,
            //                StoreId = transaction.StoreId,
            //                ShowingItemId = transaction.ShowingItemId,
            //                UnitOfMeasureId = uom.UnitOfMeasureId,
            //                ShowingItemName = shw.Name,
            //                ShowingItemCode = shw.Code,
            //                UnitOfMeasureName = uom.Name,
            //                SalePrice = shw.SalePrice,
            //                Quantity = transaction.Quantity,
            //                Amount = transaction.Amount,
            //                TransactionTypeId = transaction.TransactionTypeId,
            //                Date = transaction.Date,
            //            };

            var Ids = await POSMTransaction_query.Where(x => x.Date, OrderDateFilter)
                .Select(x => new
                {
                    StoreId = x.StoreId,
                    OrganizationId = x.OrganizationId,
                    ShowingItemId = x.ShowingItemId
                })
                .Distinct()
                .OrderBy(x => x.OrganizationId)
                .Skip(POSMReport_ShowingOrderFilterDTO.Skip)
                .Take(POSMReport_ShowingOrderFilterDTO.Take)
                .ToListWithNoLockAsync(); // phân trang báo cáo theo Org và Store
            OrganizationIds = Ids.Select(x => x.OrganizationId)
                .Distinct()
                .ToList();
            OrganizationIdFilter = new IdFilter() { In = OrganizationIds };
            StoreIds = Ids.Select(x => x.StoreId)
                .Distinct()
                .ToList();
            StoreIdFilter = new IdFilter() { In = StoreIds };

            ShowingItemIds = Ids.Select(x => x.ShowingItemId)
                .Distinct()
                .ToList();

            // lấy ra toàn bộ Org trong danh sách phân trang
            var dim_organizations_query = DWContext.Dim_Organization.AsNoTracking().Where(x => x.OrganizationId, OrganizationIdFilter);
            var Dim_Organizations = await dim_organizations_query
                .OrderBy(x => x.OrganizationId)
                .Select(x => new Dim_OrganizationDAO
                {
                    OrganizationId = x.OrganizationId,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            // lấy ra toàn bộ store trong danh sách phân trang
            var dim_stores_query = DWContext.Dim_Store.AsNoTracking().Where(x => x.StoreId, StoreIdFilter);
            var Dim_Stores = await dim_stores_query
                .Select(x => new Dim_StoreDAO
                {
                    StoreId = x.StoreId,
                    Code = x.Code,
                    CodeDraft = x.CodeDraft,
                    Name = x.Name,
                    Address = x.Address,
                }).ToListWithNoLockAsync();

            // Lấy ra toàn bộ item và đơn vị tính tương ứng
            var showingItemContent_query = from item in DWContext.Dim_ShowingItemMapping
                                     join uom in DWContext.Dim_UnitOfMeasure on item.UnitOfMeasureId equals uom.UnitOfMeasureId
                                     join shw in DWContext.Dim_ShowingItem on item.ShowingItemId equals shw.ShowingItemId
                                     select new
                                     {
                                         ShowingItemId = item.ShowingItemId,
                                         ShowingItemCode = shw.Code,
                                         ShowingItemName = shw.Name,
                                         SalePrice = shw.SalePrice,
                                         UnitOfMeasureId = shw.UnitOfMeasureId,
                                         UnitOfMeasureCode = uom.Code,
                                         UnitOfMeasureName = uom.Name
                                     };
            var ShowingItemContentDAOs = await showingItemContent_query.Distinct().ToListWithNoLockAsync();

            // tạo group cửa hàng bởi Organization
            List<POSMReport_POSMReportDTO> POSMReport_POSMReportDTOs = new List<POSMReport_POSMReportDTO>();
            foreach (var Dim_Organization in Dim_Organizations)
            {
                POSMReport_POSMReportDTO POSMReport_POSMReportDTO = new POSMReport_POSMReportDTO()
                {
                    OrganizationId = Dim_Organization.OrganizationId,
                    OrganizationName = Dim_Organization.Name,
                    Stores = new List<POSMReport_POSMStoreDTO>()
                };
                var storeids = Ids.Where(x => x.OrganizationId == Dim_Organization.OrganizationId).Select(x => x.StoreId).Distinct().ToList();
                POSMReport_POSMReportDTO.Stores = storeids
                        .Select(storeid => new POSMReport_POSMStoreDTO
                        {
                            Id = storeid,
                            OrganizationId = Dim_Organization.OrganizationId
                        }).Distinct().ToList();
                POSMReport_POSMReportDTOs.Add(POSMReport_POSMReportDTO);
            }

            foreach (POSMReport_POSMReportDTO POSMReport_POSMReportDTO in POSMReport_POSMReportDTOs)
            {
                foreach (POSMReport_POSMStoreDTO POSMReport_POSMStoreDTO in POSMReport_POSMReportDTO.Stores)
                {
                    var Dim_Store = Dim_Stores.Where(x => x.StoreId == POSMReport_POSMStoreDTO.Id).FirstOrDefault();
                    if (Dim_Store != null)
                    {
                        POSMReport_POSMStoreDTO.Code = Dim_Store.Code;
                        POSMReport_POSMStoreDTO.CodeDraft = Dim_Store.CodeDraft;
                        POSMReport_POSMStoreDTO.Name = Dim_Store.Name;
                        POSMReport_POSMStoreDTO.Address = Dim_Store.Address;
                    }

                    var LineTransaction_query = POSMTransaction_query.Where(x => x.Date, OrderDateFilter);
                    LineTransaction_query = LineTransaction_query.Where(x => x.OrganizationId, new IdFilter() { Equal = POSMReport_POSMStoreDTO.OrganizationId });
                    LineTransaction_query = LineTransaction_query.Where(x => x.StoreId, new IdFilter() { Equal = POSMReport_POSMStoreDTO.Id } );
                    var LineTransaction = await LineTransaction_query.ToListWithNoLockAsync(); // Get all Order in order date

                    var PreLineTransactinon_query = POSMTransaction_query.Where(x => x.Date, PreOrderDateFilter);
                    PreLineTransactinon_query = PreLineTransactinon_query.Where(x => x.OrganizationId, new IdFilter() { Equal = POSMReport_POSMStoreDTO.OrganizationId });
                    PreLineTransactinon_query = PreLineTransactinon_query.Where(x => x.StoreId, new IdFilter() { Equal = POSMReport_POSMStoreDTO.Id });
                    var PreLineTransactinon = await PreLineTransactinon_query.ToListWithNoLockAsync(); // Get all Order in pre order date
                    
                    var OrderTransaction = LineTransaction
                        .Where(x => x.TransactionTypeId == POSMTransactionTypeEnum.ORDER.Id)
                        .ToList(); // lấy ra toàn bộ đơn cấp mới
                    var OrderWithDrawTransaction = LineTransaction
                        .Where(x => x.TransactionTypeId == POSMTransactionTypeEnum.ORDER_WITHDRAW.Id)
                        .ToList(); // lấy ra toàn bộ đơn thu hồi

                    decimal OrderTotal = OrderTransaction.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
                    decimal OrderWithDrawTotal = OrderWithDrawTransaction.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
                    POSMReport_POSMStoreDTO.Total = OrderTotal - OrderWithDrawTotal; // tổng giá trị trưng bày
                    POSMReport_POSMStoreDTO.Contents = new List<POSMReport_POSMReportContentDTO>();
                    foreach (var TransactionItem in LineTransaction)
                    {
                        var TransactionItemContent = ShowingItemContentDAOs.Where(x => x.ShowingItemId == TransactionItem.ShowingItemId).FirstOrDefault();
                        POSMReport_POSMReportContentDTO ShowingItemContent = POSMReport_POSMStoreDTO.Contents
                            .Where(x => x.ShowingItemId == TransactionItem.ShowingItemId)
                            .Where(x => x.UnitOfMeasureId == TransactionItemContent.UnitOfMeasureId)
                            .Where(x => x.SalePrice == TransactionItemContent.SalePrice)
                            .FirstOrDefault(); // tìm dòng giống nhau item, đơn vị và giá
                        if (ShowingItemContent != null)
                        {
                            if (TransactionItem.TransactionTypeId == POSMTransactionTypeEnum.ORDER.Id)
                            {
                                ShowingItemContent.OrderQuantity += TransactionItem.Quantity;
                                ShowingItemContent.Amount += TransactionItem.Amount;
                                ShowingItemContent.CurrentOrderQuantity += TransactionItem.Quantity;
                            } // nếu cấp mới thì thêm vào OrderQuantity và cộng thêm giá trị
                            if (TransactionItem.TransactionTypeId == POSMTransactionTypeEnum.ORDER_WITHDRAW.Id)
                            {
                                ShowingItemContent.OrderWithDrawQuantity += TransactionItem.Quantity;
                                ShowingItemContent.Amount -= TransactionItem.Amount;
                                ShowingItemContent.CurrentOrderQuantity -= TransactionItem.Quantity;
                            } // nếu thu hồi thì thêm vào OrderWithDrawQuantity và trừ vào giá trị
                        } // nếu dòng theo ShowingItem, UOM, SalePrice có sẵn
                        else
                        {
                            long totalPreOrderQuantity = PreLineTransactinon.
                                Where(x => x.ShowingItemId == TransactionItem.ShowingItemId && x.TransactionTypeId == POSMTransactionTypeEnum.ORDER.Id)
                                .Sum(x => x.Quantity);
                            long totalPreOrderWithDrawQuantity = PreLineTransactinon.
                                Where(x => x.ShowingItemId == TransactionItem.ShowingItemId && x.TransactionTypeId == POSMTransactionTypeEnum.ORDER_WITHDRAW.Id)
                                .Sum(x => x.Quantity);
                            ShowingItemContent = new POSMReport_POSMReportContentDTO
                            {
                                ShowingItemId = TransactionItem.ShowingItemId,
                                ShowingItemCode = TransactionItemContent.ShowingItemCode,
                                ShowingItemName = TransactionItemContent.ShowingItemName,
                                SalePrice = TransactionItemContent.SalePrice,
                                UnitOfMeasureId = TransactionItemContent.UnitOfMeasureId,
                                UnitOfMeasure = TransactionItemContent.UnitOfMeasureName,
                                PreOrderQuantity = totalPreOrderQuantity - totalPreOrderWithDrawQuantity,
                                CurrentOrderQuantity = totalPreOrderQuantity - totalPreOrderWithDrawQuantity,
                            };
                            if (TransactionItem.TransactionTypeId == POSMTransactionTypeEnum.ORDER.Id)
                            {
                                ShowingItemContent.OrderQuantity = TransactionItem.Quantity;
                                ShowingItemContent.Amount += TransactionItem.Amount;
                                ShowingItemContent.CurrentOrderQuantity += TransactionItem.Quantity;
                            }
                            if (TransactionItem.TransactionTypeId == POSMTransactionTypeEnum.ORDER_WITHDRAW.Id)
                            {
                                ShowingItemContent.OrderWithDrawQuantity = TransactionItem.Quantity;
                                ShowingItemContent.Amount -= TransactionItem.Amount;
                                ShowingItemContent.CurrentOrderQuantity -= TransactionItem.Quantity;
                            }
                            
                            POSMReport_POSMStoreDTO.Contents.Add(ShowingItemContent);
                        } // nếu chưa có dòng nào theo ShowingItem, UOM, SalePrice
                    }
                }
            }

            return POSMReport_POSMReportDTOs;
        }
    }
}

