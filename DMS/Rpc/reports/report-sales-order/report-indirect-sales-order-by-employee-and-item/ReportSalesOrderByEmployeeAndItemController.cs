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
using DMS.Services.MAppUser;
using System.IO;
using System.Dynamic;
using NGS.Templater;
using System.Diagnostics;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using DMS.Services.MProductGrouping;
using TrueSight.Common;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_employee_and_item
{
    public class ReportSalesOrderByEmployeeAndItemController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IItemService ItemService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        public ReportSalesOrderByEmployeeAndItemController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IItemService ItemService,
            IOrganizationService OrganizationService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            IProductGroupingService ProductGroupingService,
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
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
        }

        #region Filter List
        [Route(ReportSalesOrderByEmployeeAndItemRoute.FilterListAppUser), HttpPost]
        public async Task<List<ReportSalesOrderByEmployeeAndItem_AppUserDTO>> FilterListAppUser([FromBody] ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO.Id;
            AppUserFilter.Username = ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO.DisplayName;
            AppUserFilter.OrganizationId = ReportSalesOrderByEmployeeAndItem_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<ReportSalesOrderByEmployeeAndItem_AppUserDTO> ReportSalesOrderByEmployeeAndItem_AppUserDTOs = AppUsers
                .Select(x => new ReportSalesOrderByEmployeeAndItem_AppUserDTO(x)).ToList();
            return ReportSalesOrderByEmployeeAndItem_AppUserDTOs;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportSalesOrderByEmployeeAndItem_OrganizationDTO>> FilterListOrganization([FromBody] ReportSalesOrderByEmployeeAndItem_OrganizationFilterDTO ReportSalesOrderByEmployeeAndItem_OrganizationFilterDTO)
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
            OrganizationFilter.Id = ReportSalesOrderByEmployeeAndItem_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = ReportSalesOrderByEmployeeAndItem_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = ReportSalesOrderByEmployeeAndItem_OrganizationFilterDTO.Name;
            OrganizationFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.Id == null) OrganizationFilter.Id = new IdFilter();
            OrganizationFilter.Id.In = await FilterOrganization(OrganizationService, CurrentContext);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<ReportSalesOrderByEmployeeAndItem_OrganizationDTO> ReportSalesOrderByEmployeeAndItem_OrganizationDTOs = Organizations
                .Select(x => new ReportSalesOrderByEmployeeAndItem_OrganizationDTO(x)).ToList();
            return ReportSalesOrderByEmployeeAndItem_OrganizationDTOs;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.FilterListItem), HttpPost]
        public async Task<List<ReportSalesOrderByEmployeeAndItem_ItemDTO>> FilterListItem([FromBody] ReportSalesOrderByEmployeeAndItem_ItemFilterDTO ReportSalesOrderByEmployeeAndItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = ReportSalesOrderByEmployeeAndItem_ItemFilterDTO.Id;
            ItemFilter.Code = ReportSalesOrderByEmployeeAndItem_ItemFilterDTO.Code;
            ItemFilter.Name = ReportSalesOrderByEmployeeAndItem_ItemFilterDTO.Name;
            ItemFilter.StatusId = ReportSalesOrderByEmployeeAndItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = ReportSalesOrderByEmployeeAndItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<ReportSalesOrderByEmployeeAndItem_ItemDTO> ReportSalesOrderByEmployeeAndItem_ItemDTOs = Items
                .Select(x => new ReportSalesOrderByEmployeeAndItem_ItemDTO(x)).ToList();
            return ReportSalesOrderByEmployeeAndItem_ItemDTOs;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO>> FilterListProductGrouping([FromBody] ReportSalesOrderByEmployeeAndItem_ProductGroupingFilterDTO ReportSalesOrderByEmployeeAndItem_ProductGroupingFilterDTO)
        {
            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO> ReportSalesOrderByEmployeeAndItem_ProductGroupingDTOs = ProductGroupings
                .Select(x => new ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO(x)).ToList();
            return ReportSalesOrderByEmployeeAndItem_ProductGroupingDTOs;
        }
        #endregion

        [Route(ReportSalesOrderByEmployeeAndItemRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            var SaleEmployeeId = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.AppUserId?.Equal;
            List<long> ItemIds = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.ItemId?.In;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();

            var orderQuery = from i in DataContext.IndirectSalesOrder
                             where i.OrderDate >= Start && i.OrderDate <= End &&
                             AppUserIds.Contains(i.SaleEmployeeId) &&
                             (SaleEmployeeId.HasValue == false || i.SaleEmployeeId == SaleEmployeeId.Value) &&
                             OrganizationIds.Contains(i.OrganizationId) &&
                             i.RequestStateId == RequestStateEnum.APPROVED.Id
                             select i.Id;

            var IndirectSalesOrderIds = await orderQuery.ToListWithNoLockAsync();
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                        .BulkInsertValuesIntoTempTableAsync<long>(IndirectSalesOrderIds);

            var transactionQuery = from t in DataContext.IndirectSalesOrderTransaction
                                   join tt in tempTableQuery.Query on t.IndirectSalesOrderId equals tt.Column1
                                   where (ItemIds == null || ItemIds.Count == 0 || ItemIds.Contains(t.ItemId))
                                   select new
                                   {
                                       OrganizationId = t.OrganizationId,
                                       SalesEmployeeId = t.SalesEmployeeId
                                   };
            int count = await transactionQuery.Distinct().CountWithNoLockAsync();
            return count;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>>> List([FromBody] ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.HasValue == false)
                return new List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>();

            DateTime Start = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                     LocalStartDay(CurrentContext) :
                     ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            var ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs = await ListData(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO, Start, End);

            return ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.Total), HttpPost]
        public async Task<ReportSalesOrderByEmployeeAndItem_TotalDTO> Total([FromBody] ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.HasValue == false)
                return new ReportSalesOrderByEmployeeAndItem_TotalDTO();

            DateTime Start = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                     LocalStartDay(CurrentContext) :
                     ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportSalesOrderByEmployeeAndItem_TotalDTO();

            ReportSalesOrderByEmployeeAndItem_TotalDTO ReportSalesOrderByEmployeeAndItem_TotalDTO = await TotalData(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO, Start, End);

            return ReportSalesOrderByEmployeeAndItem_TotalDTO;
        }

        [Route(ReportSalesOrderByEmployeeAndItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrderDate.LessEqual.Value;

            ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.Skip = 0;
            ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.Take = int.MaxValue;
            var ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs = await ListData(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO, Start, End);

            ReportSalesOrderByEmployeeAndItem_TotalDTO ReportSalesOrderByEmployeeAndItem_TotalDTO = await TotalData(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs)
            {
                foreach (var SaleEmployee in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO.SaleEmployees)
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

            string path = "Templates/Report_Sales_Order_By_Employee_And_Item.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderByEmployeeAndItems = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs;
            Data.Total = ReportSalesOrderByEmployeeAndItem_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportSalesOrderByEmployeeAndItem.xlsx");
        }

        private async Task<List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>> ListData(
            ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.HasValue == false)
                return new List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>();

            long? SaleEmployeeId = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.AppUserId?.Equal;
            List<long> ItemIds = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.ProductGroupingId?.Equal;

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();

            var InderectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            InderectSalesOrder_query = InderectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            InderectSalesOrder_query = InderectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            InderectSalesOrder_query = InderectSalesOrder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            InderectSalesOrder_query = InderectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });

            var IndirectSalesOrderIds = await InderectSalesOrder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

            var Transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            Transaction_query = Transaction_query.Where(x => x.DeletedAt == null);
            Transaction_query = Transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            Transaction_query = Transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds});

            var Ids = await Transaction_query.Select(t => new
            {
                OrganizationId = t.OrganizationId,
                SalesEmployeeId = t.SalesEmployeeId
            })
                .Distinct()
                .OrderBy(x => x.OrganizationId)
                .Skip(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.Skip)
                .Take(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.Take)
                .ToListWithNoLockAsync();
            AppUserIds = Ids.Select(x => x.SalesEmployeeId).Distinct().ToList();

            var AppUserQuery = DWContext.Dim_AppUser.AsNoTracking();
            AppUserQuery = AppUserQuery.Where(x => x.DeletedAt == null);
            AppUserQuery = AppUserQuery.Where(au => au.AppUserId, new IdFilter { In = AppUserIds });
            List<Dim_AppUserDAO> AppUserDAOs = await AppUserQuery.OrderBy(su => su.OrganizationId).ThenBy(x => x.DisplayName)
                .ToListWithNoLockAsync();
            OrganizationIds = Ids.Select(x => x.OrganizationId).Distinct().ToList();
            var Organizations = await DataContext.Organization
                .Where(x => x.Id, new IdFilter { In = OrganizationIds })
                .OrderBy(x => x.Id)
                .Select(x => new OrganizationDAO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListWithNoLockAsync();

            var Transaction_query2 = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            Transaction_query2 = Transaction_query2.Where(x => x.DeletedAt == null);
            Transaction_query2 = Transaction_query2.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            Transaction_query2 = Transaction_query2.Where(x => x.SalesEmployeeId, new IdFilter { In = AppUserIds });
            Transaction_query2 = Transaction_query2.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            Transaction_query2 = Transaction_query2.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            IndirectSalesOrderIds = await Transaction_query2.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs = await DataContext.IndirectSalesOrder.AsNoTracking()
                .Where(x => x.Id, new IdFilter { In = IndirectSalesOrderIds })
                .ToListWithNoLockAsync();
            var IndirectSalesOrderContentQuery = DataContext.IndirectSalesOrderContent.AsNoTracking();
            IndirectSalesOrderContentQuery = IndirectSalesOrderContentQuery.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            if (ItemIds != null && ItemIds.Count > 0)
                IndirectSalesOrderContentQuery = IndirectSalesOrderContentQuery.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            List<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs = await IndirectSalesOrderContentQuery.Select(x => new IndirectSalesOrderContentDAO
            {
                Id = x.Id,
                Amount = x.Amount,
                DiscountAmount = x.DiscountAmount,
                GeneralDiscountAmount = x.GeneralDiscountAmount,
                PrimaryPrice = x.PrimaryPrice,
                RequestedQuantity = x.RequestedQuantity,
                SalePrice = x.SalePrice,
                TaxAmount = x.TaxAmount,
                IndirectSalesOrderId = x.IndirectSalesOrderId,
                ItemId = x.ItemId,
                PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                IndirectSalesOrder = x.IndirectSalesOrder == null ? null : new IndirectSalesOrderDAO
                {
                    BuyerStoreId = x.IndirectSalesOrder.BuyerStoreId,
                }
            })
            .ToListWithNoLockAsync();
            var IndirectSalesOrderPromotionQuery = DataContext.IndirectSalesOrderPromotion.AsNoTracking();
            IndirectSalesOrderPromotionQuery = IndirectSalesOrderPromotionQuery.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            if (ItemIds != null && ItemIds.Count > 0)
            {
                IndirectSalesOrderContentQuery = IndirectSalesOrderContentQuery.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            }
            List<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionDAOs = await IndirectSalesOrderPromotionQuery.Select(x => new IndirectSalesOrderPromotionDAO
            {
                    Id = x.Id,
                    RequestedQuantity = x.RequestedQuantity,
                    IndirectSalesOrderId = x.IndirectSalesOrderId,
                    ItemId = x.ItemId,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    IndirectSalesOrder = x.IndirectSalesOrder == null ? null : new IndirectSalesOrderDAO
                    {
                        BuyerStoreId = x.IndirectSalesOrder.BuyerStoreId,
                    }
                })
                .ToListWithNoLockAsync();
            ItemIds = new List<long>();
            ItemIds.AddRange(IndirectSalesOrderContentDAOs.Select(x => x.ItemId));
            ItemIds.AddRange(IndirectSalesOrderPromotionDAOs.Select(x => x.ItemId));
            ItemIds = ItemIds.Distinct().ToList();

            List<Item> Items = await ItemService.List(new ItemFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { In = ItemIds },
                Selects = ItemSelect.Id | ItemSelect.Code | ItemSelect.Name | ItemSelect.SalePrice
            });

            List<UnitOfMeasureDAO> UnitOfMeasureDAOs = await DataContext.UnitOfMeasure.ToListWithNoLockAsync();
            List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO> ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs = new List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>();
            foreach (var Organization in Organizations)
            {
                ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO = new ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO()
                {
                    OrganizationId = Organization.Id,
                    OrganizationName = Organization.Name,
                    SaleEmployees = new List<ReportSalesOrderByEmployeeAndItem_SaleEmployeeDTO>()
                };
                ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO.SaleEmployees = Ids
                        .Where(x => x.OrganizationId == Organization.Id)
                        .Select(x => new ReportSalesOrderByEmployeeAndItem_SaleEmployeeDTO
                        {
                            SaleEmployeeId = x.SalesEmployeeId
                        }).ToList();
                ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs.Add(ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO);
            }
            // khởi tạo khung dữ liệu
            foreach (var ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs)
            {

                foreach (var SalesEmployee in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO.SaleEmployees)
                {
                    var Employee = AppUserDAOs.Where(x => x.AppUserId == SalesEmployee.SaleEmployeeId).FirstOrDefault();
                    if (Employee != null)
                    {
                        SalesEmployee.Username = Employee.Username;
                        SalesEmployee.DisplayName = Employee.DisplayName;
                    }

                    //lấy tất cả đơn hàng được thực hiện bởi nhân viên đang xét
                    var IndirectSalesOrders = IndirectSalesOrderDAOs.Where(x => x.SaleEmployeeId == SalesEmployee.SaleEmployeeId).ToList();
                    var SalesOrderIds = IndirectSalesOrders.Select(x => x.Id).ToList();

                    SalesEmployee.Items = new List<ReportSalesOrderByEmployeeAndItem_ItemDTO>();
                    foreach (IndirectSalesOrderContentDAO IndirectSalesOrderContentDAO in IndirectSalesOrderContentDAOs)
                    {
                        if (SalesOrderIds.Contains(IndirectSalesOrderContentDAO.IndirectSalesOrderId))
                        {
                            ReportSalesOrderByEmployeeAndItem_ItemDTO ReportSalesOrderByEmployeeAndItem_ItemDTO = SalesEmployee.Items.Where(i => i.Id == IndirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                            if (ReportSalesOrderByEmployeeAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == IndirectSalesOrderContentDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == IndirectSalesOrderContentDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportSalesOrderByEmployeeAndItem_ItemDTO = new ReportSalesOrderByEmployeeAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    StoreIds = new HashSet<long>(),
                                    IndirectSalesOrderIds = new HashSet<long>(),
                                };
                                SalesEmployee.Items.Add(ReportSalesOrderByEmployeeAndItem_ItemDTO);
                            }
                            var BuyerStoreId = IndirectSalesOrderContentDAO.IndirectSalesOrder.BuyerStoreId;
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.StoreIds.Add(BuyerStoreId);
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.IndirectSalesOrderIds.Add(IndirectSalesOrderContentDAO.IndirectSalesOrderId);
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.SaleStock += IndirectSalesOrderContentDAO.RequestedQuantity;
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.SalePriceAverage += (IndirectSalesOrderContentDAO.PrimaryPrice * IndirectSalesOrderContentDAO.RequestedQuantity);
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.Revenue += (IndirectSalesOrderContentDAO.Amount - (IndirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0));
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.Discount += ((IndirectSalesOrderContentDAO.DiscountAmount ?? 0) + (IndirectSalesOrderContentDAO.GeneralDiscountAmount ?? 0));
                        }
                    }

                    foreach (var item in SalesEmployee.Items)
                    {
                        item.SalePriceAverage = item.SalePriceAverage / item.SaleStock;
                    }

                    foreach (IndirectSalesOrderPromotionDAO IndirectSalesOrderPromotionDAO in IndirectSalesOrderPromotionDAOs)
                    {
                        if (SalesOrderIds.Contains(IndirectSalesOrderPromotionDAO.IndirectSalesOrderId))
                        {
                            ReportSalesOrderByEmployeeAndItem_ItemDTO ReportSalesOrderByEmployeeAndItem_ItemDTO = SalesEmployee.Items.Where(i => i.Id == IndirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                            if (ReportSalesOrderByEmployeeAndItem_ItemDTO == null)
                            {
                                var item = Items.Where(x => x.Id == IndirectSalesOrderPromotionDAO.ItemId).FirstOrDefault();
                                if (item == null)
                                    continue;
                                var UOMName = UnitOfMeasureDAOs.Where(x => x.Id == IndirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId).Select(x => x.Name).FirstOrDefault();
                                ReportSalesOrderByEmployeeAndItem_ItemDTO = new ReportSalesOrderByEmployeeAndItem_ItemDTO
                                {
                                    Id = item.Id,
                                    Code = item.Code,
                                    Name = item.Name,
                                    UnitOfMeasureName = UOMName,
                                    StoreIds = new HashSet<long>(),
                                    IndirectSalesOrderIds = new HashSet<long>(),
                                };
                                SalesEmployee.Items.Add(ReportSalesOrderByEmployeeAndItem_ItemDTO);
                            }
                            var BuyerStoreId = IndirectSalesOrderPromotionDAO.IndirectSalesOrder.BuyerStoreId;
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.StoreIds.Add(BuyerStoreId);
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.IndirectSalesOrderIds.Add(IndirectSalesOrderPromotionDAO.IndirectSalesOrderId);
                            ReportSalesOrderByEmployeeAndItem_ItemDTO.PromotionStock += IndirectSalesOrderPromotionDAO.RequestedQuantity;
                        }
                    }
                }
            }
            //});

            //làm tròn số
            foreach(var ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs)
            {
                foreach (var SaleEmployee in ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO.SaleEmployees)
                {
                    foreach (var item in SaleEmployee.Items)
                    {
                        item.Discount = Math.Round(item.Discount, 0);
                        item.Revenue = Math.Round(item.Revenue, 0);
                        item.SalePriceAverage = Math.Round(item.SalePriceAverage, 0);
                    }
                }
            }

            return ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTOs;
        }

        private async Task<ReportSalesOrderByEmployeeAndItem_TotalDTO> TotalData(
            ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? SaleEmployeeId = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.AppUserId?.Equal;
            List<long> ItemIds = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.ProductGroupingId?.Equal;

            if (ProductGroupingId.HasValue)
            {
                var ItemDAOs = await ItemService.List(new ItemFilter
                {
                    ProductGroupingId = new IdFilter { Equal = ProductGroupingId.Value },
                    Selects = ItemSelect.Id
                });
                if (ItemIds != null)
                {
                    ItemIds = ItemIds.Union(ItemDAOs.Select(x => x.Id).ToList())
                            .Distinct()
                            .ToList();
                }
                else
                {
                    ItemIds = ItemDAOs.Select(x => x.Id).ToList();
                }
            }

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                OrganizationId = new IdFilter { In = OrganizationIds },
                Id = new IdFilter { },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.DisplayName | AppUserSelect.Organization
            };
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            var AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            if (SaleEmployeeId != null)
                AppUserIds = AppUserIds.Intersect(new List<long> { SaleEmployeeId.Value }).ToList();

            var IndirectSalesOrder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            IndirectSalesOrder_query = IndirectSalesOrder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });

            var IndirectSalesOrderIds = await IndirectSalesOrder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

            var Transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            Transaction_query = Transaction_query.Where(x => x.DeletedAt == null);
            Transaction_query = Transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            if (ItemIds != null && ItemIds.Count > 0)
            {
                Transaction_query = Transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            }

            ReportSalesOrderByEmployeeAndItem_TotalDTO ReportSalesOrderByEmployeeAndItem_TotalDTO = new ReportSalesOrderByEmployeeAndItem_TotalDTO();
            ReportSalesOrderByEmployeeAndItem_TotalDTO.TotalSalesStock = Transaction_query
                .Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id }).Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportSalesOrderByEmployeeAndItem_TotalDTO.TotalPromotionStock = Transaction_query
                .Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.PROMOTION.Id }).Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportSalesOrderByEmployeeAndItem_TotalDTO.TotalRevenue = Math.Round(Transaction_query.Select(x => x.Amount).Sum()
                - Transaction_query.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum(), 0);

            ReportSalesOrderByEmployeeAndItem_TotalDTO.TotalDiscount = Math.Round(Transaction_query.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + Transaction_query.Where(x => x.DiscountAmount.HasValue).Select(x => x.DiscountAmount.Value).Sum(), 0);

            return ReportSalesOrderByEmployeeAndItem_TotalDTO;
        }

    }
}