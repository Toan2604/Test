using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Repositories;
using DMS.Services.MAppUser;
using DMS.Services.MBrand;
using DMS.Services.MCategory;
using DMS.Services.MCheckState;
using DMS.Services.MDirectSalesOrder;
using DMS.Services.MDirectSalesOrderSourceType;
using DMS.Services.MEditedPriceStatus;
using DMS.Services.MErpApprovalState;
using DMS.Services.MExportTemplate;
using DMS.Services.MGeneralApprovalState;
using DMS.Services.MOrganization;
using DMS.Services.MProduct;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MStore;
using DMS.Services.MStoreApprovalState;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreStatus;
using DMS.Services.MStoreType;
using DMS.Services.MSupplier;
using DMS.Services.MSystemConfiguration;
using DMS.Services.MTaxType;
using DMS.Services.MUnitOfMeasure;
using DMS.Services.MUnitOfMeasureGrouping;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public partial class DirectSalesOrderController : RpcController
    {
        private IEditedPriceStatusService EditedPriceStatusService;
        private IStoreService StoreService;
        private IAppUserService AppUserService;
        private ICheckStateService CheckStateService;
        private IDirectSalesOrderSourceTypeService DirectSalesOrderSourceTypeService;
        private IExportTemplateService ExportTemplateService;
        private IOrganizationService OrganizationService;
        private IUnitOfMeasureService UnitOfMeasureService;
        private IUnitOfMeasureGroupingService UnitOfMeasureGroupingService;
        private IItemService ItemService;
        private IDirectSalesOrderService DirectSalesOrderService;
        private IProductGroupingService ProductGroupingService;
        private IProductTypeService ProductTypeService;
        private IProductService ProductService;
        private IRequestStateService RequestStateService;
        private ISupplierService SupplierService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreStatusService StoreStatusService;
        private IStoreTypeService StoreTypeService;
        private ISystemConfigurationService SystemConfigurationService;
        private ITaxTypeService TaxTypeService;
        private IBrandService BrandService;
        private ICategoryService CategoryService;
        private IErpApprovalStateService ErpApprovalStateService;
        private IStoreApprovalStateService StoreApprovalStateService;
        private IGeneralApprovalStateService GeneralApprovalStateService;
        private DataContext DataContext;
        private ICurrentContext CurrentContext;
        private IUOW UOW;
        public DirectSalesOrderController(
            IOrganizationService OrganizationService,
            IEditedPriceStatusService EditedPriceStatusService,
            ICheckStateService CheckStateService,
            IStoreService StoreService,
            IAppUserService AppUserService,
            IExportTemplateService ExportTemplateService,
            IUnitOfMeasureService UnitOfMeasureService,
            IUnitOfMeasureGroupingService UnitOfMeasureGroupingService,
            IItemService ItemService,
            IDirectSalesOrderService DirectSalesOrderService,
            IDirectSalesOrderSourceTypeService DirectSalesOrderSourceTypeService,
            IProductGroupingService ProductGroupingService,
            IProductTypeService ProductTypeService,
            IProductService ProductService,
            IRequestStateService RequestStateService,
            ISupplierService SupplierService,
            IStoreGroupingService StoreGroupingService,
            IStoreStatusService StoreStatusService,
            IStoreTypeService StoreTypeService,
            ISystemConfigurationService SystemConfigurationService,
            ITaxTypeService TaxTypeService,
            IBrandService BrandService,
            ICategoryService CategoryService,
            IErpApprovalStateService ErpApprovalStateService,
            IStoreApprovalStateService StoreApprovalStateService,
            IGeneralApprovalStateService GeneralApprovalStateService,
            DataContext DataContext,
            ICurrentContext CurrentContext,
            IUOW UOW
        )
        {
            this.OrganizationService = OrganizationService;
            this.EditedPriceStatusService = EditedPriceStatusService;
            this.StoreService = StoreService;
            this.AppUserService = AppUserService;
            this.CheckStateService = CheckStateService;
            this.DirectSalesOrderSourceTypeService = DirectSalesOrderSourceTypeService;
            this.ExportTemplateService = ExportTemplateService;
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.UnitOfMeasureGroupingService = UnitOfMeasureGroupingService;
            this.ItemService = ItemService;
            this.DirectSalesOrderService = DirectSalesOrderService;
            this.ProductGroupingService = ProductGroupingService;
            this.ProductTypeService = ProductTypeService;
            this.ProductService = ProductService;
            this.RequestStateService = RequestStateService;
            this.SupplierService = SupplierService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreStatusService = StoreStatusService;
            this.StoreTypeService = StoreTypeService;
            this.SystemConfigurationService = SystemConfigurationService;
            this.TaxTypeService = TaxTypeService;
            this.BrandService = BrandService;
            this.CategoryService = CategoryService;
            this.ErpApprovalStateService = ErpApprovalStateService;
            this.StoreApprovalStateService = StoreApprovalStateService;
            this.GeneralApprovalStateService = GeneralApprovalStateService;
            this.DataContext = DataContext;
            this.CurrentContext = CurrentContext;
            this.UOW = UOW;
        }

        [Route(DirectSalesOrderRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.Count(DirectSalesOrderFilter);
            return count;
        }

        [Route(DirectSalesOrderRoute.List), HttpPost]
        public async Task<ActionResult<List<DirectSalesOrder_DirectSalesOrderDTO>>> List([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.List(DirectSalesOrderFilter);
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new DirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return DirectSalesOrder_DirectSalesOrderDTOs;
        }


        [Route(DirectSalesOrderRoute.CountNew), HttpPost]
        public async Task<ActionResult<int>> CountNew([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.CountNew(DirectSalesOrderFilter);
            return count;
        }

        [Route(DirectSalesOrderRoute.ListNew), HttpPost]
        public async Task<ActionResult<List<DirectSalesOrder_DirectSalesOrderDTO>>> ListNew([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.ListNew(DirectSalesOrderFilter);
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new DirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return DirectSalesOrder_DirectSalesOrderDTOs;
        }


        [Route(DirectSalesOrderRoute.CountPending), HttpPost]
        public async Task<ActionResult<int>> CountPending([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.CountPending(DirectSalesOrderFilter);
            return count;
        }

        [Route(DirectSalesOrderRoute.ListPending), HttpPost]
        public async Task<ActionResult<List<DirectSalesOrder_DirectSalesOrderDTO>>> ListPending([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.ListPending(DirectSalesOrderFilter);
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new DirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return DirectSalesOrder_DirectSalesOrderDTOs;
        }


        [Route(DirectSalesOrderRoute.CountCompleted), HttpPost]
        public async Task<ActionResult<int>> CountCompleted([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.CountCompleted(DirectSalesOrderFilter);
            return count;
        }

        [Route(DirectSalesOrderRoute.ListCompleted), HttpPost]
        public async Task<ActionResult<List<DirectSalesOrder_DirectSalesOrderDTO>>> ListCompleted([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.ListCompleted(DirectSalesOrderFilter);
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new DirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return DirectSalesOrder_DirectSalesOrderDTOs;
        }

        [Route(DirectSalesOrderRoute.GetDetail), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> GetDetail([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = await DirectSalesOrderService.GetDetail(DirectSalesOrder_DirectSalesOrderDTO.Id);
            List<TaxType> TaxTypes = await TaxTypeService.List(new TaxTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = TaxTypeSelect.ALL
            });
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            foreach (var DirectSalesOrderContent in DirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
            {
                TaxType TaxType = TaxTypes.Where(x => x.Percentage == DirectSalesOrderContent.TaxPercentage).FirstOrDefault(); // phần trăm thuế ở content có thể null
                if (TaxType != null)
                {
                    DirectSalesOrderContent.TaxType = new DirectSalesOrder_TaxTypeDTO(TaxType);
                }
            }
            return DirectSalesOrder_DirectSalesOrderDTO;
        }
        [Route(DirectSalesOrderRoute.Get), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Get([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = await DirectSalesOrderService.Get(DirectSalesOrder_DirectSalesOrderDTO.Id);
            List<TaxType> TaxTypes = await TaxTypeService.List(new TaxTypeFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = TaxTypeSelect.ALL
            });
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            foreach (var DirectSalesOrderContent in DirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
            {
                TaxType TaxType = TaxTypes.Where(x => x.Percentage == DirectSalesOrderContent.TaxPercentage).FirstOrDefault();
                if (TaxType != null)
                {
                    DirectSalesOrderContent.TaxType = new DirectSalesOrder_TaxTypeDTO(TaxType);
                }
            }
            return DirectSalesOrder_DirectSalesOrderDTO;
        }

        [Route(DirectSalesOrderRoute.ApplyPromotionCode), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> ApplyPromotionCode([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.ApplyPromotionCode(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Create), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Create([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Create(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Update), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Update([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Update(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Send), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Send([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Send(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Approve), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Approve([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Approve(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Reject), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Reject([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Reject(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Delete), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> Delete([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(DirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Delete(DirectSalesOrder);
            DirectSalesOrder_DirectSalesOrderDTO = new DirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return DirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(DirectSalesOrderRoute.Print), HttpGet]
        public async Task<ActionResult> Print([FromQuery] long Id)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var DirectSalesOrder = await DirectSalesOrderService.Get(Id);
            if (DirectSalesOrder == null)
                return Content("Đơn hàng không tồn tại");
            DirectSalesOrder_PrintDTO DirectSalesOrder_PrintDTO = new DirectSalesOrder_PrintDTO(DirectSalesOrder);
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var STT = 1;
            foreach (var DirectSalesOrderContent in DirectSalesOrder_PrintDTO.Contents)
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
            foreach (var DirectSalesOrderPromotion in DirectSalesOrder_PrintDTO.Promotions)
            {
                DirectSalesOrderPromotion.STT = STT++;
                DirectSalesOrderPromotion.QuantityString = DirectSalesOrderPromotion.Quantity.ToString("N0", culture);
                DirectSalesOrderPromotion.RequestedQuantityString = DirectSalesOrderPromotion.RequestedQuantity.ToString("N0", culture);
            }

            DirectSalesOrder_PrintDTO.SubTotalString = DirectSalesOrder_PrintDTO.SubTotal.ToString("N0", culture);
            DirectSalesOrder_PrintDTO.Discount = DirectSalesOrder_PrintDTO.GeneralDiscountAmount.HasValue ? DirectSalesOrder_PrintDTO.GeneralDiscountAmount.Value.ToString("N0", culture) : "";
            DirectSalesOrder_PrintDTO.Tax = DirectSalesOrder_PrintDTO.TotalTaxAmount.ToString("N0", culture);
            DirectSalesOrder_PrintDTO.sOrderDate = DirectSalesOrder_PrintDTO.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            DirectSalesOrder_PrintDTO.sDeliveryDate = DirectSalesOrder_PrintDTO.DeliveryDate.HasValue ? DirectSalesOrder_PrintDTO.DeliveryDate.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy") : string.Empty;
            DirectSalesOrder_PrintDTO.TotalString = DirectSalesOrder_PrintDTO.Total.ToString("N0", culture);
            DirectSalesOrder_PrintDTO.TotalText = Utils.ConvertAmountTostring((long)DirectSalesOrder_PrintDTO.Total);

            ExportTemplate ExportTemplate = await ExportTemplateService.Get(ExportTemplateEnum.PRINT_DIRECT.Id);
            if (ExportTemplate == null)
                return BadRequest("Chưa có mẫu in đơn hàng");

            dynamic Data = new ExpandoObject();
            Data.Order = DirectSalesOrder_PrintDTO;
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
            request.AddFile("file", output.ToArray(), $"Don-hang-truc-tiep-{DirectSalesOrder.Code}.docx");
            byte[] PdfByteArray = client.DownloadData(request);
            return File(PdfByteArray, "application/pdf", $"Don-hang-truc-tiep-{DirectSalesOrder.Code}.pdf");
        }
        [Route(DirectSalesOrderRoute.PrintDeliveryNote), HttpGet]
        public async Task<ActionResult> PrintDeliveryNote([FromQuery] long Id)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var DirectSalesOrder = await DirectSalesOrderService.Get(Id);
            if (DirectSalesOrder == null)
                return Content("Đơn hàng không tồn tại");
            DirectSalesOrder_PrintDTO DirectSalesOrder_PrintDTO = new DirectSalesOrder_PrintDTO(DirectSalesOrder);
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var STT = 1;
            decimal TotalQuantity = 0;
            foreach (var DirectSalesOrderContent in DirectSalesOrder_PrintDTO.Contents)
            {
                DirectSalesOrderContent.STT = STT++;
                DirectSalesOrderContent.QuantityString = DirectSalesOrderContent.Quantity.ToString("N0", culture);
                DirectSalesOrderContent.RequestedQuantityString = DirectSalesOrderContent.RequestedQuantity.ToString("N0", culture);
                TotalQuantity += DirectSalesOrderContent.Quantity;
            }
            foreach (var DirectSalesOrderPromotion in DirectSalesOrder_PrintDTO.Promotions)
            {
                DirectSalesOrderPromotion.STT = STT++;
                DirectSalesOrderPromotion.QuantityString = DirectSalesOrderPromotion.Quantity.ToString("N0", culture);
                DirectSalesOrderPromotion.RequestedQuantityString = DirectSalesOrderPromotion.RequestedQuantity.ToString("N0", culture);
                TotalQuantity += DirectSalesOrderPromotion.Quantity;

            }
            DirectSalesOrder_PrintDTO.TotalQuantityString = TotalQuantity.ToString("N0", culture);
            DirectSalesOrder_PrintDTO.sOrderDate = DirectSalesOrder_PrintDTO.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            DirectSalesOrder_PrintDTO.sDeliveryDate = DirectSalesOrder_PrintDTO.DeliveryDate.HasValue ? DirectSalesOrder_PrintDTO.DeliveryDate.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy") : string.Empty;
            DirectSalesOrder_PrintDTO.TotalString = DirectSalesOrder_PrintDTO.Total.ToString("N0", culture);

            ExportTemplate ExportTemplate = await ExportTemplateService.Get(ExportTemplateEnum.PRINT_DELIVERY_NOTE.Id);
            if (ExportTemplate == null)
                return BadRequest("Chưa có mẫu in đơn hàng");

            dynamic Data = new ExpandoObject();
            Data.Order = DirectSalesOrder_PrintDTO;
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
            request.AddFile("file", output.ToArray(), $"Phieu-xuat-kho-{DirectSalesOrder.Code}.docx");
            byte[] PdfByteArray = client.DownloadData(request);
            return File(PdfByteArray, "application/pdf", $"Phieu-xuat-kho-{DirectSalesOrder.Code}.pdf");
        }

        [Route(DirectSalesOrderRoute.CalculatePrice), HttpPost]
        public async Task<ActionResult<DirectSalesOrder_DirectSalesOrderDTO>> CalculatePrice([FromBody] DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE == true)
            {
                string LinkApi = SystemConfiguration.URL_API_FOR_ERP_CALCULATE_SALES_ORDER;
                IRestClient RestClient = new RestClient(LinkApi);
                IRestRequest RestRequest = new RestRequest();
                RestRequest.AddHeader("Cookie", "Token=" + CurrentContext.Token);
                RestRequest.AddJsonBody(DirectSalesOrder_DirectSalesOrderDTO);
                IRestResponse<DirectSalesOrder_DirectSalesOrderDTO> response = RestClient.Post<DirectSalesOrder_DirectSalesOrderDTO>(RestRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount = response.Data.GeneralDiscountAmount;
                    DirectSalesOrder_DirectSalesOrderDTO.SubTotal = response.Data.SubTotal;
                    DirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount = response.Data.TotalTaxAmount;
                    DirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax = response.Data.TotalAfterTax;
                    DirectSalesOrder_DirectSalesOrderDTO.Total = response.Data.TotalAfterTax;
                    foreach(DirectSalesOrder_DirectSalesOrderContentDTO DirectSalesOrder_DirectSalesOrderContentDTO in DirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
                    {
                        var content = response.Data.DirectSalesOrderContents.Where(x => x.Item.ERPCode == DirectSalesOrder_DirectSalesOrderContentDTO.Item.ERPCode).FirstOrDefault();
                        DirectSalesOrder_DirectSalesOrderContentDTO.Amount = content.Amount;
                        DirectSalesOrder_DirectSalesOrderContentDTO.Quantity = content.Quantity;
                        DirectSalesOrder_DirectSalesOrderContentDTO.SalePrice = content.SalePrice;
                        DirectSalesOrder_DirectSalesOrderContentDTO.DiscountPercentage = content.DiscountPercentage;
                        DirectSalesOrder_DirectSalesOrderContentDTO.DiscountAmount = content.DiscountAmount;
                    }
                }
                else
                {
                    return BadRequest(DirectSalesOrder_DirectSalesOrderDTO);
                }
            }
            return DirectSalesOrder_DirectSalesOrderDTO;
        }

        [Route(DirectSalesOrderRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.LessEqual.Value;

            DirectSalesOrder_DirectSalesOrderFilterDTO.Skip = 0;
            DirectSalesOrder_DirectSalesOrderFilterDTO.Take = int.MaxValue;
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = (await List(DirectSalesOrder_DirectSalesOrderFilterDTO)).Value;

            var OrganizationIds = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => x.OrganizationId).Distinct().ToList();
            var Organizations = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).Select(x => new Organization
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            List<DirectSalesOrder_ExportDTO> Exports = Organizations.Select(x => new DirectSalesOrder_ExportDTO
            {
                OrganizationId = x.Id,
                OrganizationName = x.Name,
            }).ToList();

            long stt = 1;
            decimal SubTotal = 0;
            decimal GeneralDiscountAmount = 0;
            decimal TotalTaxAmount = 0;
            decimal Total = 0;
            foreach (DirectSalesOrder_ExportDTO DirectSalesOrder_ExportDTO in Exports)
            {
                DirectSalesOrder_ExportDTO.Contents = DirectSalesOrder_DirectSalesOrderDTOs
                    .Where(x => x.OrganizationId == DirectSalesOrder_ExportDTO.OrganizationId)
                    .Select(x => new DirectSalesOrder_ExportContentDTO(x))
                    .ToList();
                foreach (var content in DirectSalesOrder_ExportDTO.Contents)
                {
                    content.STT = stt++;
                    content.OrderDateString = content.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    content.DeliveryDateString = content.DeliveryDate?.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                    content.CreatedAtString = content.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
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

            string path = "Templates/Direct_Order_Export.xlsx";
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
            Data.Root.Name = Data.Root.Name.ToUpper();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };

            return File(output.ToArray(), "application/octet-stream", "ListDirectSalesOrder.xlsx");
        }


        [Route(DirectSalesOrderRoute.ExportDetail), HttpPost]
        public async Task<ActionResult> ExportDetail([FromBody] DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DateTime Start = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.GreaterEqual == null ?
                    LocalStartDay(CurrentContext) :
                    DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.GreaterEqual.Value;

            DateTime End = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate?.LessEqual == null ?
                    LocalEndDay(CurrentContext) :
                    DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate.LessEqual.Value;
            
            DirectSalesOrder_DirectSalesOrderFilterDTO.Skip = 0;
            DirectSalesOrder_DirectSalesOrderFilterDTO.Take = int.MaxValue;
            List<DirectSalesOrder_DirectSalesOrderDTO> DirectSalesOrder_DirectSalesOrderDTOs = (await List(DirectSalesOrder_DirectSalesOrderFilterDTO)).Value;
            var Ids = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => x.Id).ToList();
            var RowIds = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => x.RowId).ToList();

            var queryContent = DataContext.DirectSalesOrderContent.AsNoTracking();
            queryContent = queryContent.Where(x => x.DirectSalesOrderId, new IdFilter { In = Ids });
            var DirectSalesOrder_DirectSalesOrderContentDTOs = await queryContent.Select(c => new DirectSalesOrder_DirectSalesOrderContentDTO
            {
                Id = c.Id,
                DirectSalesOrderId = c.DirectSalesOrderId,
                ItemId = c.ItemId,
                UnitOfMeasureId = c.UnitOfMeasureId,
                SalePrice = c.SalePrice,
                RequestedQuantity = c.RequestedQuantity,
                Quantity = c.Quantity,
                Amount = c.Amount,
                TaxAmount = c.TaxAmount,
                InventoryCheckState = new DirectSalesOrder_InventoryCheckStateDTO
                {
                    Name = c.InventoryCheckState.Name,
                },
                UnitOfMeasure = new DirectSalesOrder_UnitOfMeasureDTO
                {
                    Name = c.UnitOfMeasure.Name
                },
                Item = new DirectSalesOrder_ItemDTO
                {
                    Code = c.Item.Code,
                    Name = c.Item.Name,
                    Product = new DirectSalesOrder_ProductDTO
                    {
                        Category = new DirectSalesOrder_CategoryDTO
                        {
                            Name = c.Item.Product.Category.Name,
                        }
                    }
                }
            }).ToListWithNoLockAsync();
            var queryPromotion = DataContext.DirectSalesOrderPromotion.AsNoTracking();
            queryPromotion = queryPromotion.Where(x => x.DirectSalesOrderId, new IdFilter { In = Ids });
            var DirectSalesOrder_DirectSalesOrderPromotionDTOs = await queryPromotion.Select(c => new DirectSalesOrder_DirectSalesOrderPromotionDTO
            {
                Id = c.Id,
                DirectSalesOrderId = c.DirectSalesOrderId,
                ItemId = c.ItemId,
                UnitOfMeasureId = c.UnitOfMeasureId,
                RequestedQuantity = c.RequestedQuantity,
                Quantity = c.Quantity,
                InventoryCheckState = new DirectSalesOrder_InventoryCheckStateDTO
                {
                    Name = c.InventoryCheckState.Name,
                },
                UnitOfMeasure = new DirectSalesOrder_UnitOfMeasureDTO
                {
                    Name = c.UnitOfMeasure.Name
                },
                Item = new DirectSalesOrder_ItemDTO
                {
                    Code = c.Item.Code,
                    Name = c.Item.Name,
                    Product = new DirectSalesOrder_ProductDTO
                    {
                        Category = new DirectSalesOrder_CategoryDTO
                        {
                            Name = c.Item.Product.Category.Name,
                        }
                    }
                }
            }).ToListWithNoLockAsync();

            var queryRequestWorkflowStepMapping = DataContext.RequestWorkflowStepMapping.AsNoTracking();
            queryRequestWorkflowStepMapping = queryRequestWorkflowStepMapping.Where(x => x.RequestId, new GuidFilter { In = RowIds });
            var DirectSalesOrder_RequestWorkflowStepMappingDTOs = await queryRequestWorkflowStepMapping.Select(r => new DirectSalesOrder_RequestWorkflowStepMappingDTO
            {
                AppUserId = r.AppUserId,
                UpdatedAt = r.UpdatedAt,
                RequestId = r.RequestId,
                WorkflowStateId = r.WorkflowStateId,
                AppUser = new DirectSalesOrder_AppUserDTO
                {
                    Username = r.AppUser.Username,
                    DisplayName = r.AppUser.DisplayName,
                }
            }).ToListWithNoLockAsync();

            var ItemIds = DirectSalesOrder_DirectSalesOrderContentDTOs.Select(x => x.ItemId).ToList();
            ItemIds.AddRange(DirectSalesOrder_DirectSalesOrderPromotionDTOs.Select(x => x.ItemId).ToList());
            var Items = await ItemService.List(new ItemFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = ItemSelect.Code | ItemSelect.Product,
                Id = new IdFilter { In = ItemIds }
            });

            var queryProductGrouping = from c in DataContext.DirectSalesOrderContent
                                       join i in DataContext.Item on c.ItemId equals i.Id
                                       join p in DataContext.Product on i.ProductId equals p.Id
                                       join ppg in DataContext.ProductProductGroupingMapping on p.Id equals ppg.ProductId
                                       join pg in DataContext.ProductGrouping on ppg.ProductGroupingId equals pg.Id
                                       select new
                                       {
                                           ItemCode = i.Code,
                                           ProductGrouping = new DirectSalesOrder_ProductGroupingDTO
                                           {
                                               Name = pg.Name,
                                               Code = pg.Code
                                           }
                                       };
            var productGroupings = await queryProductGrouping.Distinct().ToListWithNoLockAsync();

            var ItemProductGroupings = productGroupings.GroupBy(x => x.ItemCode).Select(group => new { ItemCode = group.Key, ProductGroupings = String.Join(";", group.Select(x => x.ProductGrouping.Name).ToList()) }).ToList();

            var OrganizationIds = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => x.OrganizationId).Distinct().ToList();
            var Organizations = await DataContext.Organization.Where(x => OrganizationIds.Contains(x.Id)).Select(x => new Organization
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            List<DirectSalesOrder_ExportDetailDTO> Exports = DirectSalesOrder_DirectSalesOrderDTOs.Select(x => new DirectSalesOrder_ExportDetailDTO
            {
                OrganizationId = x.OrganizationId,
                Id = x.Id,
                RequestStateId = x.RequestStateId,
                Code = x.Code,
                Note = x.Note,
                Discount = x.GeneralDiscountAmount.GetValueOrDefault(0),
                OrderDate = x.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy"),
                CreatedAt = x.CreatedAt.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy"),
                DeliveryDate = x.DeliveryDate?.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy"),
                BuyerStoreAddress = x.BuyerStore.Address,
                BuyerStoreCode = x.BuyerStore.Code,
                BuyerStoreName = x.BuyerStore.Name,
                BuyerStoreProvinceName = x.BuyerStore.Province?.Name,
                BuyerStoreDistrictName = x.BuyerStore.District?.Name,
                BuyerStoreGroupingName = string.Join(";", x.BuyerStore.StoreStoreGroupingMappings?.Select(x => x.StoreGrouping.Name).ToList()),
                BuyerStoreTypeName = x.BuyerStore.StoreType.Name,
                SalesEmployeeName = x.SaleEmployee.DisplayName,
                SalesEmployeeUserName = x.SaleEmployee.Username,
                GeneralApprovalStateName = x.GeneralApprovalState.Name,
                ErpApprovalStateName = x.ErpApprovalState == null ? "" : x.ErpApprovalState.Name,
                StoreBalanceCheckStateName = x.StoreBalanceCheckState != null ? x.StoreBalanceCheckState.Name : "",
                InventoryCheckStateName = x.InventoryCheckState != null ? x.InventoryCheckState.Name : "",
                DirectSalesOrderSourceTypeCheck = (x.DirectSalesOrderSourceType !=null && x.DirectSalesOrderSourceType.Id == DirectSalesOrderSourceTypeEnum.FROM_STORE.Id) ? "x" : "",
                EditedPriceStatusName = x.EditedPriceStatus.Name,
                SubTotal = x.SubTotal,
                Total = x.Total,
                RowId = x.RowId,
                TotalTaxAmount = x.TotalTaxAmount,

                Contents = new List<DirectSalesOrder_ExportDetailContentDTO>()
            }).ToList();

            foreach (var Export in Exports)
            {
                var Organization = Organizations.Where(x => x.Id == Export.OrganizationId).FirstOrDefault();
                if (Organization != null)
                    Export.OrganizationName = Organization.Name;

                if (Export.RequestStateId == RequestStateEnum.APPROVED.Id)
                {
                    var RequestWorkflowStepMapping = DirectSalesOrder_RequestWorkflowStepMappingDTOs
                    .Where(x => x.RequestId == Export.RowId)
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault();
                    if (RequestWorkflowStepMapping != null)
                    {
                        Export.ApprovedAt = RequestWorkflowStepMapping.UpdatedAt.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
                        Export.MonitorName = RequestWorkflowStepMapping.AppUser.DisplayName;
                        Export.MonitorUserName = RequestWorkflowStepMapping.AppUser.Username;
                    }
                }

                var Contents = DirectSalesOrder_DirectSalesOrderContentDTOs.Where(x => x.DirectSalesOrderId == Export.Id).ToList();
                var Promotions = DirectSalesOrder_DirectSalesOrderPromotionDTOs.Where(x => x.DirectSalesOrderId == Export.Id).ToList();

                foreach (var Content in Contents)
                {
                    DirectSalesOrder_ExportDetailContentDTO DirectSalesOrder_ExportDetailContentDTO = new DirectSalesOrder_ExportDetailContentDTO
                    {
                        Amount = Content.Amount,
                        CategoryName = Content.Item.Product.Category.Name,
                        ItemCode = Content.Item.Code,
                        ItemName = Content.Item.Name,
                        UnitOfMeasureName = Content.UnitOfMeasure.Name,
                        ItemOrderType = "Sản phẩm bán",
                        Quantity = Content.Quantity,
                        SalePrice = Content.SalePrice,
                        TaxAmount = Content.TaxAmount ?? 0,
                        InventoryCheckStateName = Content.InventoryCheckState.Name,
                        ProductGroupings = ItemProductGroupings.Where(x => x.ItemCode == Content.Item.Code).Select(x => x.ProductGroupings).FirstOrDefault()
                    };
                    Export.Contents.Add(DirectSalesOrder_ExportDetailContentDTO);
                }

                foreach (var Promotion in Promotions)
                {
                    DirectSalesOrder_ExportDetailContentDTO DirectSalesOrder_ExportDetailContentDTO = new DirectSalesOrder_ExportDetailContentDTO
                    {
                        CategoryName = Promotion.Item.Product.Category.Name,
                        ItemCode = Promotion.Item.Code,
                        ItemName = Promotion.Item.Name,
                        UnitOfMeasureName = Promotion.UnitOfMeasure.Name,
                        ItemOrderType = "Sản phẩm khuyến mại",
                        Quantity = Promotion.Quantity,
                        Amount = 0,
                        SalePrice = 0,
                        TaxAmount = 0,
                        ProductGroupings = ItemProductGroupings.Where(x => x.ItemCode == Promotion.Item.Code).Select(x => x.ProductGroupings).FirstOrDefault()
                    };
                    Export.Contents.Add(DirectSalesOrder_ExportDetailContentDTO);
                }
            }

            long stt = 1;
            var Datas = new List<DirectSalesOrder_ExportDetailContentDTO>();
            foreach (var Export in Exports)
            {
                foreach (var Content in Export.Contents)
                {
                    DirectSalesOrder_ExportDetailContentDTO DirectSalesOrder_ExportDetailContentDTO = new DirectSalesOrder_ExportDetailContentDTO
                    {
                        STT = stt++,
                        OrganizationId = Export.OrganizationId,
                        Id = Export.Id,
                        RequestStateId = Export.RequestStateId,
                        Code = Export.Code,
                        Note = Export.Note,
                        Discount = Export.Discount,
                        OrderDate = Export.OrderDate,
                        CreatedAt = Export.CreatedAt,
                        DeliveryDate = Export.DeliveryDate,
                        BuyerStoreAddress = Export.BuyerStoreAddress,
                        BuyerStoreCode = Export.BuyerStoreCode,
                        BuyerStoreName = Export.BuyerStoreName,
                        BuyerStoreProvinceName = Export.BuyerStoreProvinceName,
                        BuyerStoreDistrictName = Export.BuyerStoreDistrictName,
                        BuyerStoreGroupingName = Export.BuyerStoreGroupingName,
                        BuyerStoreTypeName = Export.BuyerStoreTypeName,
                        SalesEmployeeName = Export.SalesEmployeeName,
                        SalesEmployeeUserName = Export.SalesEmployeeUserName,
                        GeneralApprovalStateName = Export.GeneralApprovalStateName,
                        ErpApprovalStateName = Export.ErpApprovalStateName,
                        SubTotal = Export.SubTotal,
                        Total = Export.Total,
                        RowId = Export.RowId,
                        ApprovedAt = Export.ApprovedAt,
                        ERouteCode = Export.ERouteCode,
                        ERouteName = Export.ERouteName,
                        MonitorName = Export.MonitorName,
                        MonitorUserName = Export.MonitorUserName,
                        OrganizationName = Export.OrganizationName,
                        Amount = Content.Amount,
                        CategoryName = Content.CategoryName,
                        ItemCode = Content.ItemCode,
                        ItemName = Content.ItemName,
                        ProductGroupings = Content.ProductGroupings,
                        UnitOfMeasureName = Content.UnitOfMeasureName,
                        ItemOrderType = Content.ItemOrderType,
                        Quantity = Content.Quantity,
                        SalePrice = Content.SalePrice,
                        TaxAmount = Export.TotalTaxAmount,
                        StoreBalanceCheckStateName = Export.StoreBalanceCheckStateName,
                        InventoryCheckStateName = Export.InventoryCheckStateName,
                        ItemInventoryCheckStateName = Content.InventoryCheckStateName,
                        EditedPriceStatusName = Export.EditedPriceStatusName,
                        DirectSalesOrderSourceTypeCheck = Export.DirectSalesOrderSourceTypeCheck,
                        
                    };
                    Datas.Add(DirectSalesOrder_ExportDetailContentDTO);
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

            SystemConfiguration SystemConfiguration = await SystemConfigurationService.Get();
            string path = "Templates/Direct_Order_Detail_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            dynamic Data = new ExpandoObject();
            dynamic TempData = new ExpandoObject();
            Data.Start = Start == LocalStartDay(CurrentContext) ? "" : Start.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.End = End.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            Data.Exports = Datas;
            Data.SubTotal = Exports.Sum(x => x.SubTotal);
            Data.Discount = Exports.Sum(x => x.Discount);
            Data.Total = Exports.Sum(x => x.Total);
            Data.Root = OrgRoot;
            Data.Root.Name = Data.Root.Name.ToUpper();
            #region Xây dựng template
            List<List<string>> Headers = new List<List<string>>();
            List<List<string>> DataContents = new List<List<string>>();

            List<string> head = new List<string>();
            List<string> content = new List<string>();

            head.Add("Trạng thái duyệt");
            content.Add("[[Exports.GeneralApprovalStateName]]");
            if (SystemConfiguration.SYNC_DIRECT_SALES_ORDER_WITH_ERP)
            {
                head.Add("Trạng thái ERP");
                content.Add("[[Exports.ErpApprovalStateName]]");
            }

            if (SystemConfiguration.ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER)
            {
                head.Add("Sửa giá");
                content.Add("[[Exports.EditedPriceStatusName]]");
            }
            Headers.Add(head);
            DataContents.Add(content);
            TempData.Header = Headers;
            TempData.Content = DataContents;
            MemoryStream template_input = new MemoryStream(arr);
            MemoryStream template_output = new MemoryStream();
            using (var document = StaticParams.DocumentFactory.Open(template_input, template_output, "xlsx"))
            {
                document.Process(TempData);
            }
            #endregion
            MemoryStream input = new MemoryStream(template_output.ToArray());
            MemoryStream output = new MemoryStream();
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            }
            return File(output.ToArray(), "application/octet-stream", "Danh-muc-chi-tiet-don-truc-tiep.xlsx");
        }
        private async Task<bool> HasPermission(long Id)
        {
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter();
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            if (Id == 0)
            {

            }
            else
            {
                DirectSalesOrderFilter.Id = new IdFilter { Equal = Id };
                int count = await DirectSalesOrderService.Count(DirectSalesOrderFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private DirectSalesOrder ConvertDTOToEntity(DirectSalesOrder_DirectSalesOrderDTO DirectSalesOrder_DirectSalesOrderDTO)
        {
            DirectSalesOrder_DirectSalesOrderDTO.TrimString();
            DirectSalesOrder DirectSalesOrder = new DirectSalesOrder();
            DirectSalesOrder.Id = DirectSalesOrder_DirectSalesOrderDTO.Id;
            DirectSalesOrder.Code = DirectSalesOrder_DirectSalesOrderDTO.Code;
            DirectSalesOrder.BuyerStoreId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStoreId;
            DirectSalesOrder.PhoneNumber = DirectSalesOrder_DirectSalesOrderDTO.PhoneNumber;
            DirectSalesOrder.StoreAddress = DirectSalesOrder_DirectSalesOrderDTO.StoreAddress;
            DirectSalesOrder.DeliveryAddress = DirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress;
            DirectSalesOrder.SaleEmployeeId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployeeId;
            DirectSalesOrder.OrganizationId = DirectSalesOrder_DirectSalesOrderDTO.OrganizationId;
            DirectSalesOrder.OrderDate = DirectSalesOrder_DirectSalesOrderDTO.OrderDate;
            DirectSalesOrder.DeliveryDate = DirectSalesOrder_DirectSalesOrderDTO.DeliveryDate;
            DirectSalesOrder.RequestStateId = DirectSalesOrder_DirectSalesOrderDTO.RequestStateId;
            DirectSalesOrder.GeneralApprovalStateId = DirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalStateId;
            DirectSalesOrder.EditedPriceStatusId = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatusId;
            DirectSalesOrder.Note = DirectSalesOrder_DirectSalesOrderDTO.Note;
            DirectSalesOrder.SubTotal = DirectSalesOrder_DirectSalesOrderDTO.SubTotal;
            DirectSalesOrder.GeneralDiscountPercentage = DirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountPercentage;
            DirectSalesOrder.GeneralDiscountAmount = DirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount;
            DirectSalesOrder.TotalTaxAmount = DirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount;
            DirectSalesOrder.TotalAfterTax = DirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax;
            DirectSalesOrder.PromotionCode = DirectSalesOrder_DirectSalesOrderDTO.PromotionCode;
            DirectSalesOrder.PromotionValue = DirectSalesOrder_DirectSalesOrderDTO.PromotionValue;
            DirectSalesOrder.Total = DirectSalesOrder_DirectSalesOrderDTO.Total;
            DirectSalesOrder.ErpApprovalStateId = DirectSalesOrder_DirectSalesOrderDTO.ErpApprovalStateId;
            DirectSalesOrder.BuyerStore = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore == null ? null : new Store
            {
                Id = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Id,
                Code = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Code,
                Name = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Name,
                ParentStoreId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ParentStoreId,
                OrganizationId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OrganizationId,
                StoreTypeId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreTypeId,                
                Telephone = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Telephone,
                ProvinceId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ProvinceId,
                DistrictId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DistrictId,
                WardId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.WardId,
                Address = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Address,
                DeliveryAddress = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryAddress,
                Latitude = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Latitude,
                Longitude = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Longitude,
                DeliveryLatitude = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLatitude,
                DeliveryLongitude = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLongitude,
                OwnerName = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerName,
                OwnerPhone = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerPhone,
                OwnerEmail = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerEmail,
                TaxCode = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.TaxCode,
                LegalEntity = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.LegalEntity,
                StatusId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StatusId,
                StoreStatusId = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreStatusId,
                StoreStoreGroupingMappings = DirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreStoreGroupingMappings?.Select(x => new StoreStoreGroupingMapping
                {
                    StoreId = x.StoreId,
                    StoreGroupingId = x.StoreGroupingId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        Name = x.Store.Name
                    },
                    StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name
                    }
                }).ToList()
            };
            DirectSalesOrder.Organization = DirectSalesOrder_DirectSalesOrderDTO.Organization == null ? null : new Organization
            {
                Id = DirectSalesOrder_DirectSalesOrderDTO.Organization.Id,
                Code = DirectSalesOrder_DirectSalesOrderDTO.Organization.Code,
                Name = DirectSalesOrder_DirectSalesOrderDTO.Organization.Name,
                ParentId = DirectSalesOrder_DirectSalesOrderDTO.Organization.ParentId,
                Path = DirectSalesOrder_DirectSalesOrderDTO.Organization.Path,
                Level = DirectSalesOrder_DirectSalesOrderDTO.Organization.Level,
                StatusId = DirectSalesOrder_DirectSalesOrderDTO.Organization.StatusId,
                Phone = DirectSalesOrder_DirectSalesOrderDTO.Organization.Phone,
                Address = DirectSalesOrder_DirectSalesOrderDTO.Organization.Address,
                Email = DirectSalesOrder_DirectSalesOrderDTO.Organization.Email,
            };
            DirectSalesOrder.EditedPriceStatus = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus == null ? null : new EditedPriceStatus
            {
                Id = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Id,
                Code = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Code,
                Name = DirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Name,
            };
            DirectSalesOrder.SaleEmployee = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee == null ? null : new AppUser
            {
                Id = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Id,
                Username = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Username,
                DisplayName = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.DisplayName,
                Address = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Address,
                Email = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Email,
                Phone = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Phone,
                PositionId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.PositionId,
                Department = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Department,
                OrganizationId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.OrganizationId,
                SexId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.SexId,
                StatusId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.StatusId,
                Avatar = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Avatar,
                Birthday = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Birthday,
                ProvinceId = DirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.ProvinceId,
            };
            DirectSalesOrder.DirectSalesOrderContents = DirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents?
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
            DirectSalesOrder.DirectSalesOrderPromotions = DirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderPromotions?
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

        private DirectSalesOrderFilter ConvertFilterDTOToFilterEntity(DirectSalesOrder_DirectSalesOrderFilterDTO DirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter();
            DirectSalesOrderFilter.Selects = DirectSalesOrderSelect.ALL;
            DirectSalesOrderFilter.Skip = DirectSalesOrder_DirectSalesOrderFilterDTO.Skip;
            DirectSalesOrderFilter.Take = DirectSalesOrder_DirectSalesOrderFilterDTO.Take;
            DirectSalesOrderFilter.OrderBy = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderBy;
            DirectSalesOrderFilter.OrderType = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderType;

            DirectSalesOrderFilter.Id = DirectSalesOrder_DirectSalesOrderFilterDTO.Id;
            DirectSalesOrderFilter.OrganizationId = DirectSalesOrder_DirectSalesOrderFilterDTO.OrganizationId;
            DirectSalesOrderFilter.Code = DirectSalesOrder_DirectSalesOrderFilterDTO.Code;
            DirectSalesOrderFilter.BuyerStoreId = DirectSalesOrder_DirectSalesOrderFilterDTO.BuyerStoreId;
            DirectSalesOrderFilter.BuyerStoreCode = DirectSalesOrder_DirectSalesOrderFilterDTO.BuyerStoreCode;
            DirectSalesOrderFilter.PhoneNumber = DirectSalesOrder_DirectSalesOrderFilterDTO.PhoneNumber;
            DirectSalesOrderFilter.StoreAddress = DirectSalesOrder_DirectSalesOrderFilterDTO.StoreAddress;
            DirectSalesOrderFilter.DeliveryAddress = DirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryAddress;
            DirectSalesOrderFilter.AppUserId = DirectSalesOrder_DirectSalesOrderFilterDTO.AppUserId;
            DirectSalesOrderFilter.OrderDate = DirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate;
            DirectSalesOrderFilter.DeliveryDate = DirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryDate;
            DirectSalesOrderFilter.RequestStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.RequestStateId;
            DirectSalesOrderFilter.StoreApprovalStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.StoreApprovalStateId;
            DirectSalesOrderFilter.GeneralApprovalStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.GeneralApprovalStateId;
            DirectSalesOrderFilter.ErpApprovalStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.ErpApprovalStateId;
            DirectSalesOrderFilter.EditedPriceStatusId = DirectSalesOrder_DirectSalesOrderFilterDTO.EditedPriceStatusId;
            DirectSalesOrderFilter.Note = DirectSalesOrder_DirectSalesOrderFilterDTO.Note;
            DirectSalesOrderFilter.SubTotal = DirectSalesOrder_DirectSalesOrderFilterDTO.SubTotal;
            DirectSalesOrderFilter.GeneralDiscountPercentage = DirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountPercentage;
            DirectSalesOrderFilter.GeneralDiscountAmount = DirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountAmount;
            DirectSalesOrderFilter.TotalTaxAmount = DirectSalesOrder_DirectSalesOrderFilterDTO.TotalTaxAmount;
            DirectSalesOrderFilter.Total = DirectSalesOrder_DirectSalesOrderFilterDTO.Total;
            DirectSalesOrderFilter.StoreStatusId = DirectSalesOrder_DirectSalesOrderFilterDTO.StoreStatusId;
            DirectSalesOrderFilter.UpdatedAt = DirectSalesOrder_DirectSalesOrderFilterDTO.UpdatedAt;
            DirectSalesOrderFilter.StoreBalanceCheckStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.StoreBalanceCheckStateId;
            DirectSalesOrderFilter.InventoryCheckStateId = DirectSalesOrder_DirectSalesOrderFilterDTO.InventoryCheckStateId;
            DirectSalesOrderFilter.DirectSalesOrderSourceTypeId = DirectSalesOrder_DirectSalesOrderFilterDTO.DirectSalesOrderSourceTypeId;
            return DirectSalesOrderFilter;
        }
    }
}

