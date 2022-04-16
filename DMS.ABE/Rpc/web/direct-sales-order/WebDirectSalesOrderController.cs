using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MAppUser;
using DMS.ABE.Services.MDirectSalesOrder;
using DMS.ABE.Services.MErpApprovalState;
using DMS.ABE.Services.MOrganization;
using DMS.ABE.Services.MProduct;
using DMS.ABE.Services.MStore;
using DMS.ABE.Services.MStoreApprovalState;
using DMS.ABE.Services.MStoreUser;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using System.Net.Mime;
using DMS.ABE.Services.MExportTemplate;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using DMS.ABE.Services.MCategory;
using DMS.ABE.Services.MGeneralApprovalState;

namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public partial class WebDirectSalesOrderController : SimpleController
    {
        private DataContext DataContext;
        private IAppUserService AppUserService;
        private ICategoryService CategoryService;
        private IDirectSalesOrderService DirectSalesOrderService;
        private ICurrentContext CurrentContext;
        private IErpApprovalStateService ErpApprovalStateService;
        private IExportTemplateService ExportTemplateService;
        private IGeneralApprovalStateService GeneralApprovalStateService;
        private IItemService ItemService;
        private IOrganizationService OrganizationService;
        private IProductService ProductService;
        private IStoreService StoreService;
        private IStoreUserService StoreUserService;
        private IStoreApprovalStateService StoreApprovalStateService;
        private IUOW UOW;
        public WebDirectSalesOrderController(
            DataContext DataContext,
            IAppUserService AppUserService,
            ICategoryService CategoryService,
            ICurrentContext CurrentContext,
            IDirectSalesOrderService DirectSalesOrderService,
            IErpApprovalStateService ErpApprovalStateService,
            IExportTemplateService ExportTemplateService,
            IGeneralApprovalStateService GeneralApprovalStateService,
            IItemService ItemService,
            IOrganizationService OrganizationService,
            IProductService ProductService,
            IStoreService StoreService,
            IStoreUserService StoreUserService,
            IStoreApprovalStateService StoreApprovalStateService,
            IUOW UOW
        )
        {
            this.AppUserService = AppUserService;
            this.CategoryService = CategoryService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.DirectSalesOrderService = DirectSalesOrderService;
            this.ErpApprovalStateService = ErpApprovalStateService;
            this.ExportTemplateService = ExportTemplateService;
            this.GeneralApprovalStateService = GeneralApprovalStateService;
            this.ItemService = ItemService;
            this.OrganizationService = OrganizationService;
            this.ProductService = ProductService;
            this.StoreService = StoreService;
            this.StoreApprovalStateService = StoreApprovalStateService;
            this.StoreUserService = StoreUserService;
            this.UOW = UOW;
        }
        [Route(WebDirectSalesOrderRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WebDirectSalesOrder_DirectSalesOrderFilterDTO WebDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(WebDirectSalesOrder_DirectSalesOrderFilterDTO);
            int count = await DirectSalesOrderService.WebCount(DirectSalesOrderFilter);
            return count;
        }
        [Route(WebDirectSalesOrderRoute.List), HttpPost]
        public async Task<ActionResult<List<WebDirectSalesOrder_DirectSalesOrderDTO>>> List([FromBody] WebDirectSalesOrder_DirectSalesOrderFilterDTO WebDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(WebDirectSalesOrder_DirectSalesOrderFilterDTO);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.WebList(DirectSalesOrderFilter);
            List<WebDirectSalesOrder_DirectSalesOrderDTO> WebDirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new WebDirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return WebDirectSalesOrder_DirectSalesOrderDTOs;
        }
        [Route(WebDirectSalesOrderRoute.Get), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Get([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = await DirectSalesOrderService.Get(WebDirectSalesOrder_DirectSalesOrderDTO.Id);
            return new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
        }
        [Route(WebDirectSalesOrderRoute.Create), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Create([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.WebCreate(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.Update), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Update([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.WebUpdate(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.Delete), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Delete([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Delete(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.Approve), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Approve([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Approve(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.Reject), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Reject([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Reject(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.Send), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> Send([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Send(DirectSalesOrder);
            WebDirectSalesOrder_DirectSalesOrderDTO = new WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return WebDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(WebDirectSalesOrderRoute.CalculatePrice), HttpPost]
        public async Task<ActionResult<WebDirectSalesOrder_DirectSalesOrderDTO>> CalculatePrice([FromBody] WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE == true)
            {
                var ProductIds = new List<long>();
                var ItemIds = new List<long>();
                if (WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents != null)
                {
                    ItemIds.AddRange(WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents.Select(x => x.ItemId).ToList());
                    ItemFilter SubItemFilter = new ItemFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Id = new IdFilter { In = ItemIds },
                        Selects = ItemSelect.ALL
                    };
                    List<Item> ListItems;
                    ListItems = await UOW.ItemRepository.List(SubItemFilter);
                    ProductIds.AddRange(ListItems.Select(x => x.ProductId).ToList());
                }
                ProductIds = ProductIds.Distinct().ToList();
                ItemIds = ItemIds.Distinct().ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = ItemIds },
                    Selects = ItemSelect.ALL,
                };
                List<Item> Items;
                Items = await UOW.ItemRepository.List(ItemFilter);
                ProductFilter ProductFilter = new ProductFilter
                {
                    Id = new IdFilter { In = ProductIds },
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ProductSelect.UnitOfMeasure | ProductSelect.UnitOfMeasureGrouping | ProductSelect.Id | ProductSelect.TaxType
                };
                List<Product> Products;
                Products = await UOW.ProductRepository.List(ProductFilter);
                var UOMGs = await UOW.UnitOfMeasureGroupingRepository.List(new UnitOfMeasureGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.UnitOfMeasure | UnitOfMeasureGroupingSelect.UnitOfMeasureGroupingContents
                });
                foreach (var DirectSalesOrderContent in WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
                {
                    var Item = Items.Where(x => x.Id == DirectSalesOrderContent.ItemId).FirstOrDefault();
                    var Product = Products.Where(x => Item.ProductId == x.Id).FirstOrDefault();
                    DirectSalesOrderContent.TaxPercentage = Product.TaxType.Percentage;
                    DirectSalesOrderContent.PrimaryUnitOfMeasureId = Product.UnitOfMeasureId;
                    DirectSalesOrderContent.UnitOfMeasureId = Product.UnitOfMeasureId; // đơn hàng tạo từ Mobile mặc định UOM của product
                    DirectSalesOrderContent.UnitOfMeasure = Product.UnitOfMeasure == null ? null : new WebDirectSalesOrder_UnitOfMeasureDTO
                    {
                        Id = Product.UnitOfMeasure.Id,
                        Code = Product.UnitOfMeasure.Code,
                        Name = Product.UnitOfMeasure.Name
                    };
                    UnitOfMeasure UOM = new UnitOfMeasure
                    {
                        Id = Product.UnitOfMeasure.Id,
                        Code = Product.UnitOfMeasure.Code,
                        Name = Product.UnitOfMeasure.Name,
                        Description = Product.UnitOfMeasure.Description,
                        StatusId = Product.UnitOfMeasure.StatusId,
                        Factor = 1
                    };
                }
                WebDirectSalesOrder_DirectSalesOrderDTO.OrderDate = DateTime.Today;
                string LinkApi = SystemConfiguration.URL_API_FOR_ERP_CALCULATE_SALES_ORDER;
                IRestClient RestClient = new RestClient(LinkApi);
                IRestRequest RestRequest = new RestRequest();
                RestRequest.AddJsonBody(WebDirectSalesOrder_DirectSalesOrderDTO);
                IRestResponse<WebDirectSalesOrder_DirectSalesOrderDTO> response = RestClient.Post<WebDirectSalesOrder_DirectSalesOrderDTO>(RestRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    WebDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount = response.Data.GeneralDiscountAmount;
                    WebDirectSalesOrder_DirectSalesOrderDTO.SubTotal = response.Data.SubTotal;
                    WebDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount = response.Data.TotalTaxAmount;
                    WebDirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax = response.Data.TotalAfterTax;
                    WebDirectSalesOrder_DirectSalesOrderDTO.Total = response.Data.TotalAfterTax;
                    foreach (WebDirectSalesOrder_DirectSalesOrderContentDTO WebDirectSalesOrder_DirectSalesOrderContentDTO in WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
                    {
                        var content = response.Data.DirectSalesOrderContents.Where(x => x.Item.ERPCode == WebDirectSalesOrder_DirectSalesOrderContentDTO.Item.ERPCode).FirstOrDefault();
                        WebDirectSalesOrder_DirectSalesOrderContentDTO.Amount = content.Amount;
                        WebDirectSalesOrder_DirectSalesOrderContentDTO.Quantity = content.Quantity;
                        WebDirectSalesOrder_DirectSalesOrderContentDTO.SalePrice = content.SalePrice;
                        WebDirectSalesOrder_DirectSalesOrderContentDTO.DiscountPercentage = content.DiscountPercentage;
                        WebDirectSalesOrder_DirectSalesOrderContentDTO.DiscountAmount = content.DiscountAmount;
                    }
                }
                else
                {
                    return BadRequest(WebDirectSalesOrder_DirectSalesOrderDTO);
                }
            }
            return WebDirectSalesOrder_DirectSalesOrderDTO;
        }
        [Route(WebDirectSalesOrderRoute.PrintDirectOrder), HttpGet]
        public async Task<ActionResult> PrintDirectOrder([FromQuery] long Id)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var DirectSalesOrder = await DirectSalesOrderService.Get(Id);
            if (DirectSalesOrder == null)
                return Content("Đơn hàng không tồn tại");
            WebDirectSalesOrder_PrintDirectOrderDTO WebDirectSalesOrder_PrintDTO = new WebDirectSalesOrder_PrintDirectOrderDTO(DirectSalesOrder);
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var STT = 1;
            if (WebDirectSalesOrder_PrintDTO.Contents != null)
            {
                foreach (var DirectSalesOrderContent in WebDirectSalesOrder_PrintDTO.Contents)
                {
                    DirectSalesOrderContent.STT = STT++;
                    DirectSalesOrderContent.AmountString = DirectSalesOrderContent.Amount.ToString("N0", culture);
                    DirectSalesOrderContent.PrimaryPriceString = DirectSalesOrderContent.PrimaryPrice.ToString("N0", culture);
                    DirectSalesOrderContent.QuantityString = DirectSalesOrderContent.Quantity.ToString("N0", culture);
                    DirectSalesOrderContent.RequestedQuantityString = DirectSalesOrderContent.RequestedQuantity.ToString("N0", culture);
                    DirectSalesOrderContent.SalePriceString = DirectSalesOrderContent.SalePrice.ToString("N0", culture);
                    DirectSalesOrderContent.DiscountString = DirectSalesOrderContent.DiscountPercentage.HasValue ? DirectSalesOrderContent.DiscountPercentage.Value.ToString("N0", culture) + "%" : "";
                    DirectSalesOrderContent.TaxPercentageString = DirectSalesOrderContent.TaxPercentage.HasValue ? DirectSalesOrderContent.TaxPercentage.Value.ToString("N0", culture) + "%" : "";
                }
            }
            if (WebDirectSalesOrder_PrintDTO.Promotions != null)
            {
                foreach (var DirectSalesOrderPromotion in WebDirectSalesOrder_PrintDTO.Promotions)
                {
                    DirectSalesOrderPromotion.STT = STT++;
                    DirectSalesOrderPromotion.QuantityString = DirectSalesOrderPromotion.Quantity.ToString("N0", culture);
                    DirectSalesOrderPromotion.RequestedQuantityString = DirectSalesOrderPromotion.RequestedQuantity.ToString("N0", culture);
                }
            }
            WebDirectSalesOrder_PrintDTO.SubTotalString = WebDirectSalesOrder_PrintDTO.SubTotal.ToString("N0", culture);
            WebDirectSalesOrder_PrintDTO.Discount = WebDirectSalesOrder_PrintDTO.GeneralDiscountAmount.HasValue ? WebDirectSalesOrder_PrintDTO.GeneralDiscountAmount.Value.ToString("N0", culture) : "";
            WebDirectSalesOrder_PrintDTO.sOrderDate = WebDirectSalesOrder_PrintDTO.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            WebDirectSalesOrder_PrintDTO.sDeliveryDate = WebDirectSalesOrder_PrintDTO.DeliveryDate.HasValue ? WebDirectSalesOrder_PrintDTO.DeliveryDate.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy") : string.Empty;
            WebDirectSalesOrder_PrintDTO.TotalString = WebDirectSalesOrder_PrintDTO.Total.ToString("N0", culture);
            WebDirectSalesOrder_PrintDTO.Tax = WebDirectSalesOrder_PrintDTO.TotalTaxAmount.ToString("N0", culture);
            WebDirectSalesOrder_PrintDTO.TotalText = Utils.ConvertAmountToString((long)WebDirectSalesOrder_PrintDTO.Total);
            ExportTemplate ExportTemplate = await ExportTemplateService.Get(ExportTemplateEnum.PRINT_DIRECT.Id);
            if (ExportTemplate == null)
                return BadRequest("Chưa có mẫu in đơn hàng");
            dynamic Data = new ExpandoObject();
            Data.Order = WebDirectSalesOrder_PrintDTO;
            MemoryStream MemoryStream = new MemoryStream();
            MemoryStream input = new MemoryStream(ExportTemplate.Content);
            MemoryStream output = new MemoryStream();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "docx"))
            {
                document.Process(Data);
            };
            IRestClient client = new RestClient("http://192.168.21.150:8093/rpc/converter/convert/");
            client.Timeout = -1;
            IRestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("file", output.ToArray(), $"Don-hang-truc-tiep-{WebDirectSalesOrder_PrintDTO.Code}.docx");
            byte[] PdfByteArray = client.DownloadData(request);
            return File(PdfByteArray, "application/pdf", $"Don-hang-truc-tiep-{WebDirectSalesOrder_PrintDTO.Code}.pdf");
        }
        [Route(WebDirectSalesOrderRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] WebDirectSalesOrder_DirectSalesOrderFilterDTO WebDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.LessEqual.Value;
            WebDirectSalesOrder_DirectSalesOrderFilterDTO.Skip = 0;
            WebDirectSalesOrder_DirectSalesOrderFilterDTO.Take = int.MaxValue;
            List<WebDirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = (await List(WebDirectSalesOrder_DirectSalesOrderFilterDTO)).Value;

            var OrganizationIds = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => x.OrganizationId).Distinct().ToList();
            var Organizations = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).Select(x => new Organization
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await StoreUserService.Get(StoreUserId);
            Store Store = new Store();
            if (StoreUser != null)
            {
                Store = await StoreService.Get(StoreUser.StoreId);
            }
            List<WebDirectSalesOrder_ExportDTO> Exports = Organizations.Select(x => new WebDirectSalesOrder_ExportDTO
            {
                OrganizationId = x.Id,
                OrganizationName = x.Name,
            }).ToList();

            long stt = 1;
            decimal SubTotal = 0;
            decimal GeneralDiscountAmount = 0;
            decimal TotalTaxAmount = 0;
            decimal Total = 0;
            foreach (WebDirectSalesOrder_ExportDTO DirectSalesOrder_ExportDTO in Exports)
            {
                DirectSalesOrder_ExportDTO.Contents = DirectSalesOrder_DirectSalesOrderDTOs
                    .Where(x => x.OrganizationId == DirectSalesOrder_ExportDTO.OrganizationId)
                    .Select(x => new WebDirectSalesOrder_ExportContentDTO(x))
                    .ToList();
                foreach (var content in DirectSalesOrder_ExportDTO.Contents)
                {
                    content.STT = stt++;
                    content.OrderDateString = content.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    content.DeliveryDateString = content.DeliveryDate?.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    if (content.EditedPriceStatus.Id == EditedPriceStatusEnum.ACTIVE.Id)
                        content.EditPrice = "x";
                }
                SubTotal += DirectSalesOrder_ExportDTO.Contents.Sum(x => x.SubTotal);
                GeneralDiscountAmount += DirectSalesOrder_ExportDTO.Contents.Where(x => x.GeneralDiscountAmount.HasValue).Sum(x => x.GeneralDiscountAmount.Value);
                TotalTaxAmount += DirectSalesOrder_ExportDTO.Contents.Sum(x => x.TotalTaxAmount);
                Total += DirectSalesOrder_ExportDTO.Contents.Sum(x => x.Total);
            }
            var OrgRoot = (await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = 1,
                Selects = OrganizationSelect.ALL,
                Level = new LongFilter { Equal = 1 },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            })).FirstOrDefault();

            string path = "Templates/DirectSalesOrder_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.Start = Start == LocalStartDay(CurrentContext) ? "" : Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Exports = Exports;
            Data.SubTotal = SubTotal;
            Data.GeneralDiscountAmount = GeneralDiscountAmount;
            Data.TotalTaxAmount = TotalTaxAmount;
            Data.Total = Total;
            Data.Root = OrgRoot;
            Data.StoreName = Store.Name;
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ListDirectSalesOrder.xlsx");
        }
        private DirectSalesOrder ConvertDTOToEntity(WebDirectSalesOrder_DirectSalesOrderDTO WebDirectSalesOrder_DirectSalesOrderDTO)
        {
            DirectSalesOrder DirectSalesOrder = new DirectSalesOrder();
            DirectSalesOrder.Id = WebDirectSalesOrder_DirectSalesOrderDTO.Id;
            DirectSalesOrder.Code = WebDirectSalesOrder_DirectSalesOrderDTO.Code;
            DirectSalesOrder.BuyerStoreId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStoreId;
            DirectSalesOrder.PhoneNumber = WebDirectSalesOrder_DirectSalesOrderDTO.PhoneNumber;
            DirectSalesOrder.StoreAddress = WebDirectSalesOrder_DirectSalesOrderDTO.StoreAddress;
            DirectSalesOrder.DeliveryAddress = WebDirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress;
            DirectSalesOrder.SaleEmployeeId = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployeeId;
            DirectSalesOrder.OrganizationId = WebDirectSalesOrder_DirectSalesOrderDTO.OrganizationId;
            DirectSalesOrder.OrderDate = WebDirectSalesOrder_DirectSalesOrderDTO.OrderDate;
            DirectSalesOrder.CreatedAt = StaticParams.DateTimeNow;
            DirectSalesOrder.UpdatedAt = StaticParams.DateTimeNow;
            DirectSalesOrder.DeliveryDate = WebDirectSalesOrder_DirectSalesOrderDTO.DeliveryDate;
            DirectSalesOrder.RequestStateId = WebDirectSalesOrder_DirectSalesOrderDTO.RequestStateId;
            DirectSalesOrder.EditedPriceStatusId = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatusId;
            DirectSalesOrder.Note = WebDirectSalesOrder_DirectSalesOrderDTO.Note;
            DirectSalesOrder.SubTotal = WebDirectSalesOrder_DirectSalesOrderDTO.SubTotal;
            DirectSalesOrder.GeneralDiscountPercentage = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountPercentage;
            DirectSalesOrder.GeneralDiscountAmount = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount;
            DirectSalesOrder.TotalTaxAmount = WebDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount;
            DirectSalesOrder.TotalAfterTax = WebDirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax;
            DirectSalesOrder.PromotionCode = WebDirectSalesOrder_DirectSalesOrderDTO.PromotionCode;
            DirectSalesOrder.PromotionValue = WebDirectSalesOrder_DirectSalesOrderDTO.PromotionValue;
            DirectSalesOrder.Total = WebDirectSalesOrder_DirectSalesOrderDTO.Total;
            DirectSalesOrder.GeneralApprovalStateId = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalStateId;
            DirectSalesOrder.StoreUserCreatorId = WebDirectSalesOrder_DirectSalesOrderDTO.StoreUserCreatorId; // người tạo là storeUser ứng với đại lý
            DirectSalesOrder.StoreApprovalStateId = WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalStateId; // người tạo là storeUser ứng với đại lý
            DirectSalesOrder.BuyerStore = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore == null ? null : new Store
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Id,
                Code = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Code,
                Name = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Name,
                ParentStoreId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ParentStoreId,
                OrganizationId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OrganizationId,
                StoreTypeId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreTypeId,
                Telephone = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Telephone,
                ProvinceId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ProvinceId,
                DistrictId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DistrictId,
                WardId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.WardId,
                Address = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Address,
                DeliveryAddress = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryAddress,
                Latitude = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Latitude,
                Longitude = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Longitude,
                DeliveryLatitude = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLatitude,
                DeliveryLongitude = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLongitude,
                OwnerName = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerName,
                OwnerPhone = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerPhone,
                OwnerEmail = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerEmail,
                TaxCode = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.TaxCode,
                LegalEntity = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.LegalEntity,
                StatusId = WebDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StatusId,
            };
            DirectSalesOrder.Organization = WebDirectSalesOrder_DirectSalesOrderDTO.Organization == null ? null : new Organization
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Id,
                Code = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Code,
                Name = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Name,
                ParentId = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.ParentId,
                Path = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Path,
                Level = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Level,
                StatusId = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.StatusId,
                Phone = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Phone,
                Address = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Address,
                Email = WebDirectSalesOrder_DirectSalesOrderDTO.Organization.Email,
            };
            DirectSalesOrder.EditedPriceStatus = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus == null ? null : new EditedPriceStatus
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Id,
                Code = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Code,
                Name = WebDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Name,
            };
            DirectSalesOrder.StoreApprovalState = WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalState == null ? null : new StoreApprovalState
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalState.Id,
                Code = WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalState.Code,
                Name = WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalState.Name,
            };
            DirectSalesOrder.GeneralApprovalState = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState == null ? null : new GeneralApprovalState
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState.Id,
                Code = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState.Code,
                Name = WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalState.Name,
            };
            DirectSalesOrder.SaleEmployee = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee == null ? null : new AppUser
            {
                Id = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Id,
                Username = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Username,
                DisplayName = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.DisplayName,
                Address = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Address,
                Email = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Email,
                Phone = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Phone,
                Department = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Department,
                OrganizationId = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.OrganizationId,
                SexId = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.SexId,
                StatusId = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.StatusId,
                Avatar = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Avatar,
                Birthday = WebDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Birthday,
            };
            DirectSalesOrder.DirectSalesOrderContents = WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents?
                .Select(x => new DirectSalesOrderContent
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Quantity = x.Quantity,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    RequestedQuantity = x.RequestedQuantity,
                    PrimaryPrice = x.PrimaryPrice,
                    SalePrice = x.SalePrice,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    Amount = x.Amount,
                    TaxPercentage = x.TaxPercentage,
                    TaxAmount = x.TaxAmount,
                    Factor = x.Factor,
                    Item = x.Item == null ? null : new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        RetailPrice = x.Item.RetailPrice,
                        SalePrice = x.Item.SalePrice,
                        SaleStock = x.Item.SaleStock,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                        Product = x.Item.Product == null ? null : new Product
                        {
                            Id = x.Item.Product.Id,
                            Code = x.Item.Product.Code,
                            SupplierCode = x.Item.Product.SupplierCode,
                            Name = x.Item.Product.Name,
                            Description = x.Item.Product.Description,
                            ScanCode = x.Item.Product.ScanCode,
                            ProductTypeId = x.Item.Product.ProductTypeId,
                            SupplierId = x.Item.Product.SupplierId,
                            BrandId = x.Item.Product.BrandId,
                            UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                            UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                            RetailPrice = x.Item.Product.RetailPrice,
                            TaxTypeId = x.Item.Product.TaxTypeId,
                            StatusId = x.Item.Product.StatusId,
                            ProductType = x.Item.Product.ProductType == null ? null : new ProductType
                            {
                                Id = x.Item.Product.ProductType.Id,
                                Code = x.Item.Product.ProductType.Code,
                                Name = x.Item.Product.ProductType.Name,
                                Description = x.Item.Product.ProductType.Description,
                                StatusId = x.Item.Product.ProductType.StatusId,
                            },
                            TaxType = x.Item.Product.TaxType == null ? null : new TaxType
                            {
                                Id = x.Item.Product.TaxType.Id,
                                Code = x.Item.Product.TaxType.Code,
                                StatusId = x.Item.Product.TaxType.StatusId,
                                Name = x.Item.Product.TaxType.Name,
                                Percentage = x.Item.Product.TaxType.Percentage,
                            },
                            UnitOfMeasure = x.Item.Product.UnitOfMeasure == null ? null : new UnitOfMeasure
                            {
                                Id = x.Item.Product.UnitOfMeasure.Id,
                                Code = x.Item.Product.UnitOfMeasure.Code,
                                Name = x.Item.Product.UnitOfMeasure.Name,
                                Description = x.Item.Product.UnitOfMeasure.Description,
                                StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                            },
                            UnitOfMeasureGrouping = x.Item.Product.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                            {
                                Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                                Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                                Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                                Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                                UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId,
                            },
                        }
                    },
                    PrimaryUnitOfMeasure = x.PrimaryUnitOfMeasure == null ? null : new UnitOfMeasure
                    {
                        Id = x.PrimaryUnitOfMeasure.Id,
                        Code = x.PrimaryUnitOfMeasure.Code,
                        Name = x.PrimaryUnitOfMeasure.Name,
                        Description = x.PrimaryUnitOfMeasure.Description,
                        StatusId = x.PrimaryUnitOfMeasure.StatusId,
                    },
                    UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).ToList();
            DirectSalesOrder.DirectSalesOrderPromotions = WebDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderPromotions?
              .Select(x => new DirectSalesOrderPromotion
              {
                  Id = x.Id,
                  ItemId = x.ItemId,
                  UnitOfMeasureId = x.UnitOfMeasureId,
                  Quantity = x.Quantity,
                  PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                  RequestedQuantity = x.RequestedQuantity,
                  Note = x.Note,
                  Factor = x.Factor,
                  Item = x.Item == null ? null : new Item
                  {
                      Id = x.Item.Id,
                      ProductId = x.Item.ProductId,
                      Code = x.Item.Code,
                      Name = x.Item.Name,
                      ScanCode = x.Item.ScanCode,
                      SalePrice = x.Item.SalePrice,
                      RetailPrice = x.Item.RetailPrice,
                      StatusId = x.Item.StatusId,
                      SaleStock = x.Item.SaleStock,
                      Product = x.Item.Product == null ? null : new Product
                      {
                          Id = x.Item.Product.Id,
                          Code = x.Item.Product.Code,
                          SupplierCode = x.Item.Product.SupplierCode,
                          Name = x.Item.Product.Name,
                          Description = x.Item.Product.Description,
                          ScanCode = x.Item.Product.ScanCode,
                          ProductTypeId = x.Item.Product.ProductTypeId,
                          SupplierId = x.Item.Product.SupplierId,
                          BrandId = x.Item.Product.BrandId,
                          UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                          UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                          RetailPrice = x.Item.Product.RetailPrice,
                          TaxTypeId = x.Item.Product.TaxTypeId,
                          StatusId = x.Item.Product.StatusId,
                          ProductType = x.Item.Product.ProductType == null ? null : new ProductType
                          {
                              Id = x.Item.Product.ProductType.Id,
                              Code = x.Item.Product.ProductType.Code,
                              Name = x.Item.Product.ProductType.Name,
                              Description = x.Item.Product.ProductType.Description,
                              StatusId = x.Item.Product.ProductType.StatusId,
                          },
                          TaxType = x.Item.Product.TaxType == null ? null : new TaxType
                          {
                              Id = x.Item.Product.TaxType.Id,
                              Code = x.Item.Product.TaxType.Code,
                              StatusId = x.Item.Product.TaxType.StatusId,
                              Name = x.Item.Product.TaxType.Name,
                              Percentage = x.Item.Product.TaxType.Percentage,
                          },
                          UnitOfMeasure = x.Item.Product.UnitOfMeasure == null ? null : new UnitOfMeasure
                          {
                              Id = x.Item.Product.UnitOfMeasure.Id,
                              Code = x.Item.Product.UnitOfMeasure.Code,
                              Name = x.Item.Product.UnitOfMeasure.Name,
                              Description = x.Item.Product.UnitOfMeasure.Description,
                              StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                          },
                          UnitOfMeasureGrouping = x.Item.Product.UnitOfMeasureGrouping == null ? null : new UnitOfMeasureGrouping
                          {
                              Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                              Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                              Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                              Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                              UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId,
                          },
                      }
                  },
                  PrimaryUnitOfMeasure = x.PrimaryUnitOfMeasure == null ? null : new UnitOfMeasure
                  {
                      Id = x.PrimaryUnitOfMeasure.Id,
                      Code = x.PrimaryUnitOfMeasure.Code,
                      Name = x.PrimaryUnitOfMeasure.Name,
                      Description = x.PrimaryUnitOfMeasure.Description,
                      StatusId = x.PrimaryUnitOfMeasure.StatusId,
                  },
                  UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                  {
                      Id = x.UnitOfMeasure.Id,
                      Code = x.UnitOfMeasure.Code,
                      Name = x.UnitOfMeasure.Name,
                      Description = x.UnitOfMeasure.Description,
                      StatusId = x.UnitOfMeasure.StatusId,
                  },
              }).ToList();
            DirectSalesOrder.BaseLanguage = CurrentContext.Language;
            return DirectSalesOrder;
        }
        private DirectSalesOrderFilter ConvertFilterDTOToFilterEntity(WebDirectSalesOrder_DirectSalesOrderFilterDTO WebDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter();
            DirectSalesOrderFilter.Selects = DirectSalesOrderSelect.ALL;
            DirectSalesOrderFilter.Skip = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Skip;
            DirectSalesOrderFilter.Take = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Take;
            DirectSalesOrderFilter.OrderBy = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderBy;
            DirectSalesOrderFilter.OrderType = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderType;
            DirectSalesOrderFilter.Id = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Id;
            DirectSalesOrderFilter.Code = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Code;
            DirectSalesOrderFilter.OrganizationId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrganizationId;
            DirectSalesOrderFilter.BuyerStoreId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.BuyerStoreId;
            DirectSalesOrderFilter.PhoneNumber = WebDirectSalesOrder_DirectSalesOrderFilterDTO.PhoneNumber;
            DirectSalesOrderFilter.StoreAddress = WebDirectSalesOrder_DirectSalesOrderFilterDTO.StoreAddress;
            DirectSalesOrderFilter.DeliveryAddress = WebDirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryAddress;
            DirectSalesOrderFilter.DeliveryDate = WebDirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryDate;
            DirectSalesOrderFilter.SaleEmployeeId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.SaleEmployeeId;
            DirectSalesOrderFilter.OrderDate = WebDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate;
            DirectSalesOrderFilter.DeliveryDate = WebDirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryDate;
            DirectSalesOrderFilter.ErpApprovalStateId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.ErpApprovalStateId;
            DirectSalesOrderFilter.StoreApprovalStateId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.StoreApprovalStateId;
            DirectSalesOrderFilter.RequestStateId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.RequestStateId;
            DirectSalesOrderFilter.EditedPriceStatusId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.EditedPriceStatusId;
            DirectSalesOrderFilter.Note = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Note;
            DirectSalesOrderFilter.SubTotal = WebDirectSalesOrder_DirectSalesOrderFilterDTO.SubTotal;
            DirectSalesOrderFilter.GeneralDiscountPercentage = WebDirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountPercentage;
            DirectSalesOrderFilter.GeneralDiscountAmount = WebDirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountAmount;
            DirectSalesOrderFilter.TotalTaxAmount = WebDirectSalesOrder_DirectSalesOrderFilterDTO.TotalTaxAmount;
            DirectSalesOrderFilter.TotalAfterTax = WebDirectSalesOrder_DirectSalesOrderFilterDTO.TotalAfterTax;
            DirectSalesOrderFilter.PromotionCode = WebDirectSalesOrder_DirectSalesOrderFilterDTO.PromotionCode;
            DirectSalesOrderFilter.PromotionValue = WebDirectSalesOrder_DirectSalesOrderFilterDTO.PromotionValue;
            DirectSalesOrderFilter.Total = WebDirectSalesOrder_DirectSalesOrderFilterDTO.Total;
            DirectSalesOrderFilter.RowId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.RowId;
            DirectSalesOrderFilter.StoreCheckingId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.StoreCheckingId;
            DirectSalesOrderFilter.CreatorId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.CreatorId;
            DirectSalesOrderFilter.DirectSalesOrderSourceTypeId = WebDirectSalesOrder_DirectSalesOrderFilterDTO.DirectSalesOrderSourceTypeId;
            DirectSalesOrderFilter.CreatedAt = WebDirectSalesOrder_DirectSalesOrderFilterDTO.CreatedAt;
            DirectSalesOrderFilter.UpdatedAt = WebDirectSalesOrder_DirectSalesOrderFilterDTO.UpdatedAt;
            return DirectSalesOrderFilter;
        }
    }
}
