using DMS.Common;
using DMS.Models;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using Microsoft.AspNetCore.Mvc;
using System;
using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Enums;
using DMS.Helpers;
using DMS.Services.MIndirectSalesOrder;
using DMS.Services.MOrganization;
using Microsoft.EntityFrameworkCore;
using DMS.Repositories;
using System.IO;
using System.Dynamic;
using NGS.Templater;
using DMS.Rpc.direct_sales_order;
using DMS.Services.MAppUser;
using TrueSight.Common;
using DMS.Services.MStore;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using DMS.Services.MProvince;
using DMS.Services.MDistrict;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item
{
    public class ReportSalesOrderByItemController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IIndirectSalesOrderService IndirectSalesOrderService;
        private IItemService ItemService;
        private IStoreService StoreService;
        private IProductService ProductService;
        private IProductTypeService ProductTypeService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportSalesOrderByItemController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IItemService ItemService,
            IStoreService StoreService,
            IIndirectSalesOrderService IndirectSalesOrderService,
            IProductService ProductService,
            IProductTypeService ProductTypeService,
            IProductGroupingService ProductGroupingService,
            IProvinceService ProvinceService,
            IDistrictService DistrictService,
            ICurrentContext CurrentContext)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.IndirectSalesOrderService = IndirectSalesOrderService;
            this.ItemService = ItemService;
            this.ProductService = ProductService;
            this.ProductTypeService = ProductTypeService;
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
            this.StoreService = StoreService;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        #region Filter List
        [Route(ReportSalesOrderByItemRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportSalesOrderByItem_OrganizationDTO>> FilterListOrganization()
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
            List<ReportSalesOrderByItem_OrganizationDTO> ReportSalesOrderByItem_OrganizationDTO = Organizations
                .Select(x => new ReportSalesOrderByItem_OrganizationDTO(x)).ToList();
            return ReportSalesOrderByItem_OrganizationDTO;
        }

        [Route(ReportSalesOrderByItemRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<ReportSalesOrderByItem_ProductGroupingDTO>> FilterListProductGrouping([FromBody] ReportSalesOrderByItem_ProductGroupingFilterDTO ReportSalesOrderByItem_ProductGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter();
            ProductGroupingFilter.Skip = 0;
            ProductGroupingFilter.Take = int.MaxValue;
            ProductGroupingFilter.OrderBy = ProductGroupingOrder.Id;
            ProductGroupingFilter.OrderType = OrderType.ASC;
            ProductGroupingFilter.Selects = ProductGroupingSelect.Id | ProductGroupingSelect.Code
                | ProductGroupingSelect.Name | ProductGroupingSelect.Parent;
            ProductGroupingFilter.Code = ReportSalesOrderByItem_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = ReportSalesOrderByItem_ProductGroupingFilterDTO.Name;

            if (ProductGroupingFilter.Id == null) ProductGroupingFilter.Id = new IdFilter();
            ProductGroupingFilter.Id.In = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            List<ProductGrouping> ReportSalesOrderByItemGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<ReportSalesOrderByItem_ProductGroupingDTO> ReportSalesOrderByItem_ProductGroupingDTOs = ReportSalesOrderByItemGroupings
                .Select(x => new ReportSalesOrderByItem_ProductGroupingDTO(x)).ToList();
            return ReportSalesOrderByItem_ProductGroupingDTOs;
        }

        [Route(ReportSalesOrderByItemRoute.FilterListProductType), HttpPost]
        public async Task<List<ReportSalesOrderByItem_ProductTypeDTO>> FilterListProductType([FromBody] ReportSalesOrderByItem_ProductTypeFilterDTO ReportSalesOrderByItem_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = ReportSalesOrderByItem_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = ReportSalesOrderByItem_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = ReportSalesOrderByItem_ProductTypeFilterDTO.Name;
            ProductTypeFilter.StatusId = ReportSalesOrderByItem_ProductTypeFilterDTO.StatusId;

            if (ProductTypeFilter.Id == null) ProductTypeFilter.Id = new IdFilter();
            ProductTypeFilter.Id.In = await FilterProductType(ProductTypeService, CurrentContext);
            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<ReportSalesOrderByItem_ProductTypeDTO> ReportSalesOrderByItem_ProductTypeDTOs = ProductTypes
                .Select(x => new ReportSalesOrderByItem_ProductTypeDTO(x)).ToList();
            return ReportSalesOrderByItem_ProductTypeDTOs;
        }

        [Route(ReportSalesOrderByItemRoute.FilterListItem), HttpPost]
        public async Task<List<ReportSalesOrderByItem_ItemDTO>> FilterListItem([FromBody] ReportSalesOrderByItem_ItemFilterDTO ReportSalesOrderByItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = ReportSalesOrderByItem_ItemFilterDTO.Id;
            ItemFilter.Code = ReportSalesOrderByItem_ItemFilterDTO.Code;
            ItemFilter.Name = ReportSalesOrderByItem_ItemFilterDTO.Name;
            ItemFilter.StatusId = ReportSalesOrderByItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = ReportSalesOrderByItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<ReportSalesOrderByItem_ItemDTO> ReportSalesOrderByItem_ItemDTOs = Items
                .Select(x => new ReportSalesOrderByItem_ItemDTO(x)).ToList();
            return ReportSalesOrderByItem_ItemDTOs;
        }
        
        [Route(ReportSalesOrderByItemRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportSalesOrderByItem_ProvinceDTO>> FilterListProvince([FromBody] ReportSalesOrderByItem_ProvinceFilterDTO ReportSalesOrderByItem_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportSalesOrderByItem_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportSalesOrderByItem_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportSalesOrderByItem_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportSalesOrderByItem_ProvinceDTO> ReportSalesOrderByItem_ProvinceDTOs = Provinces
                .Select(x => new ReportSalesOrderByItem_ProvinceDTO(x)).ToList();
            return ReportSalesOrderByItem_ProvinceDTOs;
        }

        [Route(ReportSalesOrderByItemRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportSalesOrderByItem_DistrictDTO>> FilterListDistrict([FromBody] ReportSalesOrderByItem_DistrictFilterDTO ReportSalesOrderByItem_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportSalesOrderByItem_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportSalesOrderByItem_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportSalesOrderByItem_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportSalesOrderByItem_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportSalesOrderByItem_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportSalesOrderByItem_DistrictDTO> ReportSalesOrderByItem_DistrictDTOs = Districts
                .Select(x => new ReportSalesOrderByItem_DistrictDTO(x)).ToList();
            return ReportSalesOrderByItem_DistrictDTOs;
        }

        #endregion

        [Route(ReportSalesOrderByItemRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? ItemId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            List<long> ItemIds = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.DistrictId?.Equal;
            if (ItemId != null) ItemIds = new List<long> { ItemId.Value };

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            List<long> ProductTypeIds = await FilterProductType(ProductTypeService, CurrentContext);
            if (ProductTypeId.HasValue)
            {
                var listId = new List<long> { ProductTypeId.Value };
                ProductTypeIds = ProductTypeIds.Intersect(listId).ToList();
            }
            List<long> ProductGroupingIds = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            if (ProductGroupingId.HasValue)
            {
                var listId = new List<long> { ProductGroupingId.Value };
                ProductGroupingIds = ProductGroupingIds.Intersect(listId).ToList();
            }
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            //var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            //List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            //productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            //List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            //var product_query = DWContext.Dim_Product.AsNoTracking();
            //product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            //product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            //var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            //product_query = product_query.Union(productNoGrouping_query);
            //List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

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

            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            List<long> IndirectSalesOrderIds = await indirectsalesorder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

            var transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            transaction_query = transaction_query.Where(x => x.DeletedAt == null);
            transaction_query = transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            transaction_query = transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var query = transaction_query.GroupBy(x => new { x.OrganizationId, x.ItemId }).Select(x => new
            {
                OrganizationId = x.Key.OrganizationId,
                ItemId = x.Key.ItemId,
            });

            return await query.CountWithNoLockAsync();
        }

        [Route(ReportSalesOrderByItemRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>>> List([FromBody] ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.HasValue == false)
                return new List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>();

            DateTime Start = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO> ReportSalesOrderByItem_ReportSalesOrderByItemDTOs = await ListData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, Start, End);
            return ReportSalesOrderByItem_ReportSalesOrderByItemDTOs;
        }

        [Route(ReportSalesOrderByItemRoute.Total), HttpPost]
        public async Task<ReportSalesOrderByItem_TotalDTO> Total([FromBody] ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.HasValue == false)
                return new ReportSalesOrderByItem_TotalDTO();

            DateTime Start = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                   LocalStartDay(CurrentContext) :
                   ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return new ReportSalesOrderByItem_TotalDTO();

            ReportSalesOrderByItem_TotalDTO ReportSalesOrderByItem_TotalDTO = await TotalData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, Start, End);

            return ReportSalesOrderByItem_TotalDTO;
        }

        [Route(ReportSalesOrderByItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Skip = 0;
            ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Take = int.MaxValue;
            List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO> ReportSalesOrderByItem_ReportSalesOrderByItemDTOs = await ListData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, Start, End);

            ReportSalesOrderByItem_TotalDTO ReportSalesOrderByItem_TotalDTO = await TotalData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportSalesOrderByItem_ReportSalesOrderByItemDTO ReportSalesOrderByItem_ReportSalesOrderByItemDTO in ReportSalesOrderByItem_ReportSalesOrderByItemDTOs)
            {
                foreach (var Item in ReportSalesOrderByItem_ReportSalesOrderByItemDTO.ItemDetails)
                {
                    Item.STT = stt;
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

            string path = "Templates/Report_Sales_Order_By_Item.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderByItems = ReportSalesOrderByItem_ReportSalesOrderByItemDTOs;
            Data.Total = ReportSalesOrderByItem_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportSalesOrderByItem.xlsx");
        }

        private async Task<List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>> ListData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, DateTime Start, DateTime End)
        {
            long? ItemId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            List<long> ItemIds = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.In;
            long? ProductGroupingId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.DistrictId?.Equal;
            if (ItemId != null) ItemIds = new List<long> { ItemId.Value };

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            List<long> ProductTypeIds = await FilterProductType(ProductTypeService, CurrentContext);
            if (ProductTypeId.HasValue)
            {
                var listId = new List<long> { ProductTypeId.Value };
                ProductTypeIds = ProductTypeIds.Intersect(listId).ToList();
            }
            List<long> ProductGroupingIds = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            if (ProductGroupingId.HasValue)
            {
                var listId = new List<long> { ProductGroupingId.Value };
                ProductGroupingIds = ProductGroupingIds.Intersect(listId).ToList();
            }
            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            //var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            //List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            //productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            //List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            //var product_query = DWContext.Dim_Product.AsNoTracking();
            //product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            //product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            //var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            //product_query = product_query.Union(productNoGrouping_query);
            //List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

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

            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            List<long> IndirectSalesOrderIds = await indirectsalesorder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

            var transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            transaction_query = transaction_query.Where(x => x.DeletedAt == null);
            transaction_query = transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            transaction_query = transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var query = transaction_query.GroupBy(x => new { x.OrganizationId, x.ItemId }).Select(x => new
            {
                OrganizationId = x.Key.OrganizationId,
                ItemId = x.Key.ItemId,
            });

            var keys = await query
                .OrderBy(x => x.OrganizationId).ThenBy(x => x.ItemId)
                .Skip(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Skip)
                .Take(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.Take)
                .ToListWithNoLockAsync();

            var OrgIds = keys.Select(x => x.OrganizationId).Distinct().ToList();
            var OrganizationNames = await DataContext.Organization.Where(x => OrgIds.Contains(x.Id)).OrderBy(x => x.Id).Select(x => x.Name).ToListWithNoLockAsync();
            List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO> ReportSalesOrderByItem_ReportSalesOrderByItemDTOs = new List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>();
            foreach (var OrganizationName in OrganizationNames)
            {
                ReportSalesOrderByItem_ReportSalesOrderByItemDTO ReportSalesOrderByItem_ReportSalesOrderByItemDTO = new ReportSalesOrderByItem_ReportSalesOrderByItemDTO();
                ReportSalesOrderByItem_ReportSalesOrderByItemDTO.OrganizationName = OrganizationName;
                ReportSalesOrderByItem_ReportSalesOrderByItemDTO.ItemDetails = new List<ReportSalesOrderByItem_ItemDetailDTO>();
                ReportSalesOrderByItem_ReportSalesOrderByItemDTOs.Add(ReportSalesOrderByItem_ReportSalesOrderByItemDTO);
            }

            ItemIds = keys.Select(x => x.ItemId).Distinct().ToList();

            var DbTransaction_query = DataContext.IndirectSalesOrderTransaction.AsNoTracking();
            DbTransaction_query = DbTransaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            DbTransaction_query = DbTransaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var IndirectSalesOrderTransactions = await DbTransaction_query.Select(t => new IndirectSalesOrderTransactionDAO
            {
                Id = t.Id,
                ItemId = t.ItemId,
                Discount = t.Discount,
                IndirectSalesOrderId = t.IndirectSalesOrderId,
                OrganizationId = t.OrganizationId,
                Quantity = t.Quantity,
                Revenue = t.Revenue,
                TypeId = t.TypeId,
                UnitOfMeasureId = t.UnitOfMeasureId,
                IndirectSalesOrder = new IndirectSalesOrderDAO
                {
                    BuyerStoreId = t.IndirectSalesOrder.BuyerStoreId
                },
                Item = new ItemDAO
                {
                    Id = t.Item.Id,
                    Code = t.Item.Code,
                    Name = t.Item.Name,
                },
                Organization = new OrganizationDAO
                {
                    Name = t.Organization.Name
                },
                UnitOfMeasure = new UnitOfMeasureDAO
                {
                    Name = t.UnitOfMeasure.Name
                }
            }).ToListWithNoLockAsync();

            foreach (var ReportSalesOrderByItem_ReportSalesOrderByItemDTO in ReportSalesOrderByItem_ReportSalesOrderByItemDTOs)
            {
                var Transactions = IndirectSalesOrderTransactions.Where(x => x.Organization.Name == ReportSalesOrderByItem_ReportSalesOrderByItemDTO.OrganizationName);
                foreach (var Transaction in Transactions)
                {
                    var ItemDetail = ReportSalesOrderByItem_ReportSalesOrderByItemDTO.ItemDetails.Where(x => x.ItemId == Transaction.ItemId).FirstOrDefault();
                    if (ItemDetail == null)
                    {
                        ItemDetail = new ReportSalesOrderByItem_ItemDetailDTO();
                        ItemDetail.ItemId = Transaction.Item.Id;
                        ItemDetail.ItemCode = Transaction.Item.Code;
                        ItemDetail.ItemName = Transaction.Item.Name;
                        ItemDetail.UnitOfMeasureName = Transaction.UnitOfMeasure.Name;
                        ItemDetail.IndirectSalesOrderIds = new HashSet<long>();
                        ItemDetail.BuyerStoreIds = new HashSet<long>();
                        ReportSalesOrderByItem_ReportSalesOrderByItemDTO.ItemDetails.Add(ItemDetail);
                    }
                    if (Transaction.TypeId == TransactionTypeEnum.SALES_CONTENT.Id)
                    {
                        ItemDetail.SaleStock += Transaction.Quantity;
                    }
                    if (Transaction.TypeId == TransactionTypeEnum.PROMOTION.Id)
                    {
                        ItemDetail.PromotionStock += Transaction.Quantity;
                    }
                    ItemDetail.Discount += Transaction.Discount ?? 0;
                    ItemDetail.Revenue += Transaction.Revenue ?? 0;
                    ItemDetail.IndirectSalesOrderIds.Add(Transaction.IndirectSalesOrderId);
                    ItemDetail.BuyerStoreIds.Add(Transaction.IndirectSalesOrder.BuyerStoreId);
                }
            }
            //làm tròn số
            foreach (var ReportSalesOrderByItem_ReportSalesOrderByItemDTO in ReportSalesOrderByItem_ReportSalesOrderByItemDTOs)
            {
                foreach (var item in ReportSalesOrderByItem_ReportSalesOrderByItemDTO.ItemDetails)
                {
                    item.Discount = Math.Round(item.Discount, 0);
                    item.Revenue = Math.Round(item.Revenue, 0);
                }
            }

            return ReportSalesOrderByItem_ReportSalesOrderByItemDTOs;
        }

        private async Task<ReportSalesOrderByItem_TotalDTO> TotalData(ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO, DateTime Start, DateTime End)
        {
            long? ItemId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            long? ProductGroupingId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            List<long> ItemIds = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ItemId?.In;
            long? ProvinceId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.DistrictId?.Equal;
            if (ItemId != null) ItemIds = new List<long> { ItemId.Value };
            ReportSalesOrderByItem_TotalDTO ReportSalesOrderByItem_TotalDTO = new ReportSalesOrderByItem_TotalDTO();

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            List<long> ProductTypeIds = await FilterProductType(ProductTypeService, CurrentContext);
            if (ProductTypeId.HasValue)
            {
                var listId = new List<long> { ProductTypeId.Value };
                ProductTypeIds = ProductTypeIds.Intersect(listId).ToList();
            }
            List<long> ProductGroupingIds = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            if (ProductGroupingId.HasValue)
            {
                var listId = new List<long> { ProductGroupingId.Value };
                ProductGroupingIds = ProductGroupingIds.Intersect(listId).ToList();
            }

            List<long> StoreIds = await FilterStore(StoreService, AppUserService, OrganizationService, CurrentContext);

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.StoreId, new IdFilter { In = StoreIds });
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            //var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            //List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            //productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            //List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            //var product_query = DWContext.Dim_Product.AsNoTracking();
            //product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            //product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            //var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            //productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            //product_query = product_query.Union(productNoGrouping_query);
            //List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

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

            var indirectsalesorder_query = DWContext.Fact_IndirectSalesOrder.AsNoTracking();
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            indirectsalesorder_query = indirectsalesorder_query.Where(x => x.RequestStateId, new IdFilter { Equal = RequestStateEnum.APPROVED.Id });
            List<long> IndirectSalesOrderIds = await indirectsalesorder_query.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();

            var Transaction_query = DWContext.Fact_IndirectSalesOrderTransaction.AsNoTracking();
            Transaction_query = Transaction_query.Where(x => x.DeletedAt == null);
            Transaction_query = Transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
            if (ItemIds != null && ItemIds.Count > 0)
            {
                Transaction_query = Transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });
            }

            ReportSalesOrderByItem_TotalDTO.TotalSalesStock = Transaction_query
                .Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.SALES_CONTENT.Id }).Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportSalesOrderByItem_TotalDTO.TotalPromotionStock = Transaction_query
                .Where(x => x.TransactionTypeId, new IdFilter { Equal = TransactionTypeEnum.PROMOTION.Id }).Select(x => x.RequestedQuantity ?? 0).Sum();

            ReportSalesOrderByItem_TotalDTO.TotalRevenue = Math.Round(Transaction_query.Select(x => x.Amount).Sum()
                - Transaction_query.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum(), 0);

            ReportSalesOrderByItem_TotalDTO.TotalDiscount = Math.Round(Transaction_query.Where(x => x.GeneralDiscountAmount.HasValue).Select(x => x.GeneralDiscountAmount.Value).Sum()
                + Transaction_query.Where(x => x.DiscountAmount.HasValue).Select(x => x.DiscountAmount.Value).Sum(), 0);

            return ReportSalesOrderByItem_TotalDTO;
        }
    }
}
