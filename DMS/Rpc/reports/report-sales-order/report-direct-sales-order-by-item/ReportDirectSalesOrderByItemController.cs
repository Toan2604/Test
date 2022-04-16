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
using DMS.Services.MDirectSalesOrder;
using DMS.Services.MOrganization;
using Microsoft.EntityFrameworkCore;
using DMS.Repositories;
using System.IO;
using System.Dynamic;
using NGS.Templater;
using DMS.Rpc.direct_sales_order;
using DMS.Services.MAppUser;
using TrueSight.Common;
using DMS.Services.MDistrict;
using DMS.Services.MProvince;
using Thinktecture.EntityFrameworkCore.TempTables;
using Thinktecture;
using DMS.DWModels;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_item
{
    public class ReportDirectSalesOrderByItemController : RpcController
    {
        private DataContext DataContext;
        private DWContext DWContext;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IDirectSalesOrderService DirectSalesOrderService;
        private IItemService ItemService;
        private IProductService ProductService;
        private IProductTypeService ProductTypeService;
        private IProductGroupingService ProductGroupingService;
        private ICurrentContext CurrentContext;
        private IProvinceService ProvinceService;
        private IDistrictService DistrictService;
        public ReportDirectSalesOrderByItemController(
            DataContext DataContext,
            DWContext DWContext,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IItemService ItemService,
            IDirectSalesOrderService DirectSalesOrderService,
            IProductService ProductService,
            IProductTypeService ProductTypeService,
            IProductGroupingService ProductGroupingService,
            ICurrentContext CurrentContext,
            IProvinceService ProvinceService,
            IDistrictService DistrictService)
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.DirectSalesOrderService = DirectSalesOrderService;
            this.ItemService = ItemService;
            this.ProductService = ProductService;
            this.ProductTypeService = ProductTypeService;
            this.ProductGroupingService = ProductGroupingService;
            this.CurrentContext = CurrentContext;
            this.ProvinceService = ProvinceService;
            this.DistrictService = DistrictService;
        }

        #region Filter List
        [Route(ReportDirectSalesOrderByItemRoute.FilterListOrganization), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_OrganizationDTO>> FilterListOrganization()
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
            List<ReportDirectSalesOrderByItem_OrganizationDTO> ReportDirectSalesOrderByItem_OrganizationDTO = Organizations
                .Select(x => new ReportDirectSalesOrderByItem_OrganizationDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_OrganizationDTO;
        }

        [Route(ReportDirectSalesOrderByItemRoute.FilterListProductGrouping), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_ProductGroupingDTO>> FilterListProductGrouping([FromBody] ReportDirectSalesOrderByItem_ProductGroupingFilterDTO ReportDirectSalesOrderByItem_ProductGroupingFilterDTO)
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
            ProductGroupingFilter.Code = ReportDirectSalesOrderByItem_ProductGroupingFilterDTO.Code;
            ProductGroupingFilter.Name = ReportDirectSalesOrderByItem_ProductGroupingFilterDTO.Name;

            if (ProductGroupingFilter.Id == null) ProductGroupingFilter.Id = new IdFilter();
            ProductGroupingFilter.Id.In = await FilterProductGrouping(ProductGroupingService, CurrentContext);
            List<ProductGrouping> ReportDirectSalesOrderByItemGroupings = await ProductGroupingService.List(ProductGroupingFilter);
            List<ReportDirectSalesOrderByItem_ProductGroupingDTO> ReportDirectSalesOrderByItem_ProductGroupingDTOs = ReportDirectSalesOrderByItemGroupings
                .Select(x => new ReportDirectSalesOrderByItem_ProductGroupingDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_ProductGroupingDTOs;
        }

        [Route(ReportDirectSalesOrderByItemRoute.FilterListProductType), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_ProductTypeDTO>> FilterListProductType([FromBody] ReportDirectSalesOrderByItem_ProductTypeFilterDTO ReportDirectSalesOrderByItem_ProductTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ProductTypeFilter ProductTypeFilter = new ProductTypeFilter();
            ProductTypeFilter.Skip = 0;
            ProductTypeFilter.Take = 20;
            ProductTypeFilter.OrderBy = ProductTypeOrder.Id;
            ProductTypeFilter.OrderType = OrderType.ASC;
            ProductTypeFilter.Selects = ProductTypeSelect.ALL;
            ProductTypeFilter.Id = ReportDirectSalesOrderByItem_ProductTypeFilterDTO.Id;
            ProductTypeFilter.Code = ReportDirectSalesOrderByItem_ProductTypeFilterDTO.Code;
            ProductTypeFilter.Name = ReportDirectSalesOrderByItem_ProductTypeFilterDTO.Name;
            ProductTypeFilter.StatusId = ReportDirectSalesOrderByItem_ProductTypeFilterDTO.StatusId;

            if (ProductTypeFilter.Id == null) ProductTypeFilter.Id = new IdFilter();
            ProductTypeFilter.Id.In = await FilterProductType(ProductTypeService, CurrentContext);
            List<ProductType> ProductTypes = await ProductTypeService.List(ProductTypeFilter);
            List<ReportDirectSalesOrderByItem_ProductTypeDTO> ReportDirectSalesOrderByItem_ProductTypeDTOs = ProductTypes
                .Select(x => new ReportDirectSalesOrderByItem_ProductTypeDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_ProductTypeDTOs;
        }

        [Route(ReportDirectSalesOrderByItemRoute.FilterListItem), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_ItemDTO>> FilterListItem([FromBody] ReportDirectSalesOrderByItem_ItemFilterDTO ReportDirectSalesOrderByItem_ItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            ItemFilter ItemFilter = new ItemFilter();
            ItemFilter.Skip = 0;
            ItemFilter.Take = 20;
            ItemFilter.OrderBy = ItemOrder.Id;
            ItemFilter.OrderType = OrderType.ASC;
            ItemFilter.Selects = ItemSelect.ALL;
            ItemFilter.Id = ReportDirectSalesOrderByItem_ItemFilterDTO.Id;
            ItemFilter.Code = ReportDirectSalesOrderByItem_ItemFilterDTO.Code;
            ItemFilter.Name = ReportDirectSalesOrderByItem_ItemFilterDTO.Name;
            ItemFilter.StatusId = ReportDirectSalesOrderByItem_ItemFilterDTO.StatusId;
            ItemFilter.Search = ReportDirectSalesOrderByItem_ItemFilterDTO.Search;

            List<Item> Items = await ItemService.List(ItemFilter);
            List<ReportDirectSalesOrderByItem_ItemDTO> ReportDirectSalesOrderByItem_ItemDTOs = Items
                .Select(x => new ReportDirectSalesOrderByItem_ItemDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_ItemDTOs;
        }

        [Route(ReportDirectSalesOrderByItemRoute.FilterListProvince), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_ProvinceDTO>> FilterListProvince([FromBody] ReportDirectSalesOrderByItem_ProvinceFilterDTO ReportDirectSalesOrderByItem_ProvinceFilterDTO)
        {
            ProvinceFilter ProvinceFilter = new ProvinceFilter();
            ProvinceFilter.Skip = 0;
            ProvinceFilter.Take = 20;
            ProvinceFilter.OrderBy = ProvinceOrder.Priority;
            ProvinceFilter.OrderType = OrderType.ASC;
            ProvinceFilter.Selects = ProvinceSelect.ALL;
            ProvinceFilter.Id = ReportDirectSalesOrderByItem_ProvinceFilterDTO.Id;
            ProvinceFilter.Name = ReportDirectSalesOrderByItem_ProvinceFilterDTO.Name;
            ProvinceFilter.StatusId = ReportDirectSalesOrderByItem_ProvinceFilterDTO.StatusId;

            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            List<ReportDirectSalesOrderByItem_ProvinceDTO> ReportDirectSalesOrderByItem_ProvinceDTOs = Provinces
                .Select(x => new ReportDirectSalesOrderByItem_ProvinceDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_ProvinceDTOs;
        }

        [Route(ReportDirectSalesOrderByItemRoute.FilterListDistrict), HttpPost]
        public async Task<List<ReportDirectSalesOrderByItem_DistrictDTO>> FilterListDistrict([FromBody] ReportDirectSalesOrderByItem_DistrictFilterDTO ReportDirectSalesOrderByItem_DistrictFilterDTO)
        {
            DistrictFilter DistrictFilter = new DistrictFilter();
            DistrictFilter.Skip = 0;
            DistrictFilter.Take = 20;
            DistrictFilter.OrderBy = DistrictOrder.Priority;
            DistrictFilter.OrderType = OrderType.ASC;
            DistrictFilter.Selects = DistrictSelect.ALL;
            DistrictFilter.Id = ReportDirectSalesOrderByItem_DistrictFilterDTO.Id;
            DistrictFilter.Name = ReportDirectSalesOrderByItem_DistrictFilterDTO.Name;
            DistrictFilter.Priority = ReportDirectSalesOrderByItem_DistrictFilterDTO.Priority;
            DistrictFilter.ProvinceId = ReportDirectSalesOrderByItem_DistrictFilterDTO.ProvinceId;
            DistrictFilter.StatusId = ReportDirectSalesOrderByItem_DistrictFilterDTO.StatusId;

            List<District> Districts = await DistrictService.List(DistrictFilter);
            List<ReportDirectSalesOrderByItem_DistrictDTO> ReportDirectSalesOrderByItem_DistrictDTOs = Districts
                .Select(x => new ReportDirectSalesOrderByItem_DistrictDTO(x)).ToList();
            return ReportDirectSalesOrderByItem_DistrictDTOs;
        }
        #endregion

        [Route(ReportDirectSalesOrderByItemRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.HasValue == false)
                return 0;

            DateTime Start = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return 0;

            long? ItemId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            long? ProductGroupingId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var product_query = DWContext.Dim_Product.AsNoTracking();
            product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            if (ProductGroupingId.HasValue)
                productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            product_query = product_query.Union(productNoGrouping_query);
            List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var item_query = DWContext.Dim_Item.AsNoTracking();
            item_query = item_query.Where(x => x.ItemId, new IdFilter { Equal = ItemId });
            item_query = item_query.Where(x => x.ProductId, new IdFilter { In = ProductIds });
            var ItemIds = await item_query.Select(x => x.ItemId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });
            List<long> DirectSalesOrderIds = await directsalesorder_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();

            var transaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            transaction_query = transaction_query.Where(x => x.DeletedAt == null);
            transaction_query = transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            transaction_query = transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var count = await transaction_query.GroupBy(x => new { x.OrganizationId, x.ItemId }).Select(x => new
            {
                OrganizationId = x.Key.OrganizationId,
                ItemId = x.Key.ItemId,
            }).CountWithNoLockAsync();
            return count;
        }

        [Route(ReportDirectSalesOrderByItemRoute.List), HttpPost]
        public async Task<ActionResult<List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>>> List([FromBody] ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.HasValue == false)
                return new List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>();

            DateTime Start = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            if (End.Subtract(Start).Days > 31)
                return BadRequest(new { message = "Chỉ được phép xem tối đa trong vòng 31 ngày" });

            List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO> ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs = await ListData(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO, Start, End);
            return ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs;
        }

        [Route(ReportDirectSalesOrderByItemRoute.Total), HttpPost]
        public async Task<ReportDirectSalesOrderByItem_TotalDTO> Total([FromBody] ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.HasValue == false)
                return new ReportDirectSalesOrderByItem_TotalDTO();

            DateTime Start = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                   LocalStartDay(CurrentContext) :
                   ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            long? ItemId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            long? ProductGroupingId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;

            if (End.Subtract(Start).Days > 31)
                return new ReportDirectSalesOrderByItem_TotalDTO();

            ReportDirectSalesOrderByItem_TotalDTO ReportDirectSalesOrderByItem_TotalDTO = await TotalData(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO, Start, End);
            return ReportDirectSalesOrderByItem_TotalDTO;
        }

        [Route(ReportDirectSalesOrderByItemRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.GreaterEqual.Value;

            DateTime End = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Date.LessEqual.Value;

            ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Skip = 0;
            ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Take = int.MaxValue;
            List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO> ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs = await ListData(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO, Start, End);

            ReportDirectSalesOrderByItem_TotalDTO ReportDirectSalesOrderByItem_TotalDTO = await TotalData(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO, Start, End);
            long stt = 1;
            foreach (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO in ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs)
            {
                foreach (var Item in ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.ItemDetails)
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

            string path = "Templates/Report_Direct_Sales_Order_By_Item.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.ReportSalesOrderByItems = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs;
            Data.Total = ReportDirectSalesOrderByItem_TotalDTO;
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ReportDirectSalesOrderByItem.xlsx");
        }

        private async Task<List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>> ListData(
            ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? ItemId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            long? ProductGroupingId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var product_query = DWContext.Dim_Product.AsNoTracking();
            product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            if (ProductGroupingId.HasValue) 
                productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            product_query = product_query.Union(productNoGrouping_query);
            List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var item_query = DWContext.Dim_Item.AsNoTracking();
            item_query = item_query.Where(x => x.ItemId, new IdFilter { Equal = ItemId });
            item_query = item_query.Where(x => x.ProductId, new IdFilter { In = ProductIds });
            var ItemIds = await item_query.Select(x => x.ItemId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });

            List<long> DirectSalesOrderIds = await directsalesorder_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();

            var transaction_query = DWContext.Fact_DirectSalesOrderTransaction.AsNoTracking();
            transaction_query = transaction_query.Where(x => x.DeletedAt == null);
            transaction_query = transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            transaction_query = transaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var query = transaction_query.GroupBy(x => new { x.OrganizationId, x.ItemId }).Select(x => new
            {
                OrganizationId = x.Key.OrganizationId,
                ItemId = x.Key.ItemId,
            });

            var keys = await query
                .OrderBy(x => x.OrganizationId).ThenBy(x => x.ItemId)
                .Skip(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Skip)
                .Take(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.Take)
                .ToListWithNoLockAsync();

            var OrgIds = keys.Select(x => x.OrganizationId).Distinct().ToList();
            var OrganizationNames = await DataContext.Organization.Where(x => OrgIds.Contains(x.Id)).OrderBy(x => x.Id).Select(x => x.Name).ToListWithNoLockAsync();
            List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO> ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs = new List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>();
            foreach (var OrganizationName in OrganizationNames)
            {
                ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO = new ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO();
                ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.OrganizationName = OrganizationName;
                ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.ItemDetails = new List<ReportDirectSalesOrderByItem_ItemDetailDTO>();
                ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs.Add(ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO);
            }

            ItemIds = keys.Select(x => x.ItemId).Distinct().ToList();

            var DbTransaction_query = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            DbTransaction_query = DbTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DbTransaction_query = DbTransaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var DirectSalesOrderTransactions = await DbTransaction_query.Select(t => new DirectSalesOrderTransactionDAO
            {
                Id = t.Id,
                ItemId = t.ItemId,
                Discount = t.Discount,
                DirectSalesOrderId = t.DirectSalesOrderId,
                OrganizationId = t.OrganizationId,
                Quantity = t.Quantity,
                Revenue = t.Revenue,
                TypeId = t.TypeId,
                UnitOfMeasureId = t.UnitOfMeasureId,
                DirectSalesOrder = new DirectSalesOrderDAO
                {
                    BuyerStoreId = t.BuyerStoreId
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

            foreach (var ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO in ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs)
            {
                var Transactions = DirectSalesOrderTransactions.Where(x => x.Organization.Name == ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.OrganizationName);
                foreach (var Transaction in Transactions)
                {
                    var ItemDetail = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.ItemDetails.Where(x => x.ItemId == Transaction.ItemId).FirstOrDefault();
                    if (ItemDetail == null)
                    {
                        ItemDetail = new ReportDirectSalesOrderByItem_ItemDetailDTO();
                        ItemDetail.ItemId = Transaction.Item.Id;
                        ItemDetail.ItemCode = Transaction.Item.Code;
                        ItemDetail.ItemName = Transaction.Item.Name;
                        ItemDetail.UnitOfMeasureName = Transaction.UnitOfMeasure.Name;
                        ItemDetail.DirectSalesOrderIds = new HashSet<long>();
                        ItemDetail.BuyerStoreIds = new HashSet<long>();
                        ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.ItemDetails.Add(ItemDetail);
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
                    ItemDetail.DirectSalesOrderIds.Add(Transaction.DirectSalesOrderId);
                    ItemDetail.BuyerStoreIds.Add(Transaction.DirectSalesOrder.BuyerStoreId);
                }
            }
            //làm tròn số
            foreach (var ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO in ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs)
            {
                foreach (var item in ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO.ItemDetails)
                {
                    item.Discount = Math.Round(item.Discount, 0);
                    item.Revenue = Math.Round(item.Revenue, 0);
                }
            }

            return ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTOs;
        }

        private async Task<ReportDirectSalesOrderByItem_TotalDTO> TotalData(
            ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO,
            DateTime Start, DateTime End)
        {
            long? ItemId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ItemId?.Equal;
            long? ProductTypeId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductTypeId?.Equal;
            long? ProductGroupingId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProductGroupingId?.Equal;
            long? ProvinceId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.ProvinceId?.Equal;
            long? DistrictId = ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.DistrictId?.Equal;

            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && OrganizationIds.Contains(o.Id)).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemFilterDTO.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
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

            var Store_query = DWContext.Dim_Store.AsNoTracking();
            Store_query = Store_query.Where(x => x.ProvinceId, new IdFilter { Equal = ProvinceId });
            Store_query = Store_query.Where(x => x.DistrictId, new IdFilter { Equal = DistrictId });
            var StoreIds = await Store_query.Select(x => x.StoreId).ToListWithNoLockAsync();

            var productGrouping_query = DataContext.ProductProductGroupingMapping.AsNoTracking();
            List<long> ProductHasGroupingIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();
            productGrouping_query = productGrouping_query.Where(x => x.ProductGroupingId, new IdFilter { In = ProductGroupingIds });
            List<long> ProductHasGroupingFilterIds = await productGrouping_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var product_query = DWContext.Dim_Product.AsNoTracking();
            product_query = product_query.Where(x => x.ProductId, new IdFilter { In = ProductHasGroupingFilterIds });
            product_query = product_query.Where(x => x.ProductTypeId, new IdFilter { In = ProductTypeIds });

            var productNoGrouping_query = DWContext.Dim_Product.AsNoTracking();
            productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { NotIn = ProductHasGroupingIds });
            if (ProductGroupingId.HasValue)
                productNoGrouping_query = productNoGrouping_query.Where(x => x.ProductId, new IdFilter { In = ProductGroupingIds });
            product_query = product_query.Union(productNoGrouping_query);
            List<long> ProductIds = await product_query.Select(x => x.ProductId).Distinct().ToListWithNoLockAsync();

            var item_query = DWContext.Dim_Item.AsNoTracking();
            item_query = item_query.Where(x => x.ItemId, new IdFilter { Equal = ItemId });
            item_query = item_query.Where(x => x.ProductId, new IdFilter { In = ProductIds });
            var ItemIds = await item_query.Select(x => x.ItemId).ToListWithNoLockAsync();

            var directsalesorder_query = DWContext.Fact_DirectSalesOrder.AsNoTracking();
            directsalesorder_query = directsalesorder_query.Where(x => x.BuyerStoreId, new IdFilter { In = StoreIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrganizationId, new IdFilter { In = OrganizationIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.SaleEmployeeId, new IdFilter { In = AppUserIds });
            directsalesorder_query = directsalesorder_query.Where(x => x.OrderDate, new DateFilter { GreaterEqual = Start, LessEqual = End });
            directsalesorder_query = directsalesorder_query.Where(x => x.GeneralApprovalStateId, new IdFilter
            {
                In = new List<long>
            {
                GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id
            }
            });

            List<long> DirectSalesOrderIds = await directsalesorder_query.Select(x => x.DirectSalesOrderId).ToListWithNoLockAsync();

            var DbTransaction_query = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            DbTransaction_query = DbTransaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
            DbTransaction_query = DbTransaction_query.Where(x => x.ItemId, new IdFilter { In = ItemIds });

            var DirectSalesOrderTransactions = await DbTransaction_query.Select(t => new DirectSalesOrderTransactionDAO
            {
                Id = t.Id,
                ItemId = t.ItemId,
                Discount = t.Discount,
                DirectSalesOrderId = t.DirectSalesOrderId,
                OrganizationId = t.OrganizationId,
                Quantity = t.Quantity,
                Revenue = t.Revenue,
                TypeId = t.TypeId,
                UnitOfMeasureId = t.UnitOfMeasureId,
            }).ToListWithNoLockAsync();

            ReportDirectSalesOrderByItem_TotalDTO ReportDirectSalesOrderByItem_TotalDTO = new ReportDirectSalesOrderByItem_TotalDTO();
            ReportDirectSalesOrderByItem_TotalDTO.TotalDiscount = DirectSalesOrderTransactions
                .Where(x => x.Discount.HasValue)
                .Select(x => x.Discount.Value)
                .DefaultIfEmpty(0)
                .Sum();
            ReportDirectSalesOrderByItem_TotalDTO.TotalRevenue = DirectSalesOrderTransactions
                .Where(x => x.Revenue.HasValue)
                .Select(x => x.Revenue.Value)
                .DefaultIfEmpty(0)
                .Sum();
            ReportDirectSalesOrderByItem_TotalDTO.TotalPromotionStock = DirectSalesOrderTransactions
                .Where(x => x.TypeId == TransactionTypeEnum.PROMOTION.Id)
                .Select(x => x.Quantity)
                .Sum();
            ReportDirectSalesOrderByItem_TotalDTO.TotalSalesStock = DirectSalesOrderTransactions
                .Where(x => x.TypeId == TransactionTypeEnum.SALES_CONTENT.Id)
                .Select(x => x.Quantity)
                .Sum();

            return ReportDirectSalesOrderByItem_TotalDTO;
        }

    }
}
