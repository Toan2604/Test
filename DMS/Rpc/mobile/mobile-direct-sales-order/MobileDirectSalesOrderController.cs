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
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Services.MStoreChecking;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public partial class MobileDirectSalesOrderController : RpcController
    {
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        private IAppUserService AppUserService;
        private ICheckStateService CheckStateService;
        private IDirectSalesOrderService DirectSalesOrderService;
        private IDirectSalesOrderSourceTypeService DirectSalesOrderSourceTypeService;
        private IErpApprovalStateService ErpApprovalStateService;
        private IExportTemplateService ExportTemplateService;
        private IGeneralApprovalStateService GeneralApprovalStateService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreCheckingService StoreCheckingService;
        private ISystemConfigurationService SystemConfigurationService;
        private IStoreApprovalStateService StoreApprovalStateService;
        private IUOW UOW;
        public MobileDirectSalesOrderController(
            ICurrentContext CurrentContext,
            DataContext DataContext,
            IAppUserService AppUserService,
            ICheckStateService CheckStateService,
            IDirectSalesOrderService DirectSalesOrderService,
            IDirectSalesOrderSourceTypeService DirectSalesOrderSourceTypeService,
            IExportTemplateService ExportTemplateService,
            IErpApprovalStateService ErpApprovalStateService,
            IGeneralApprovalStateService GeneralApprovalStateService,
            IOrganizationService OrganizationService,
            IStoreCheckingService StoreCheckingService,
            IStoreService StoreService,
            ISystemConfigurationService SystemConfigurationService,
            IStoreApprovalStateService StoreApprovalStateService,
            IUOW UOW
        )
        {
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.AppUserService = AppUserService;
            this.CheckStateService = CheckStateService;
            this.DirectSalesOrderService = DirectSalesOrderService;
            this.DirectSalesOrderSourceTypeService = DirectSalesOrderSourceTypeService;
            this.ExportTemplateService = ExportTemplateService;
            this.ErpApprovalStateService = ErpApprovalStateService;
            this.GeneralApprovalStateService = GeneralApprovalStateService;
            this.OrganizationService = OrganizationService;
            this.StoreService = StoreService;
            this.StoreCheckingService = StoreCheckingService;
            this.StoreApprovalStateService = StoreApprovalStateService;
            this.SystemConfigurationService = SystemConfigurationService;
            this.UOW = UOW;
        }

        [Route(MobileDirectSalesOrderRoute.Get), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Get([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrder DirectSalesOrder = await DirectSalesOrderService.Get(MobileDirectSalesOrder_DirectSalesOrderDTO.Id);
            return new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
        }

        /// <summary>
        /// TẤT CẢ CÁC ĐƠN TRONG QUYỀN
        /// TAB "TẤT CẢ"
        /// </summary>
        /// <param name="MobileDirectSalesOrder_DirectSalesOrderFilterDTO"></param>
        /// <returns></returns>
        [Route(MobileDirectSalesOrderRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.Count(DirectSalesOrderFilter);
            return count;
        }

        [Route(MobileDirectSalesOrderRoute.List), HttpPost]
        public async Task<ActionResult<List<MobileDirectSalesOrder_DirectSalesOrderDTO>>> List([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.List(DirectSalesOrderFilter);
            List<MobileDirectSalesOrder_DirectSalesOrderDTO> MobileDirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new MobileDirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return MobileDirectSalesOrder_DirectSalesOrderDTOs;
        }

        /// <summary>
        /// Tất cả các đơn trừ các đơn đang đợi duyệt  
        /// TAB "CỦA TÔI"
        /// </summary>
        /// <param name="MobileDirectSalesOrder_DirectSalesOrderFilterDTO"></param>
        /// <returns></returns>
        [Route(MobileDirectSalesOrderRoute.CountInScoped), HttpPost]
        public async Task<ActionResult<int>> CountInScoped([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.CountInScoped(DirectSalesOrderFilter);
            return count;
        }
        [Route(MobileDirectSalesOrderRoute.ListInScoped), HttpPost]
        public async Task<ActionResult<List<MobileDirectSalesOrder_DirectSalesOrderDTO>>> ListInScoped([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.ListInScoped(DirectSalesOrderFilter);
            List<MobileDirectSalesOrder_DirectSalesOrderDTO> MobileDirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new MobileDirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return MobileDirectSalesOrder_DirectSalesOrderDTOs;
        }

        /// <summary>
        /// Tất cả các đơn đang đợi duyệt 
        /// TAB "TÔI DUYỆT"
        /// </summary>
        /// <param name="MobileDirectSalesOrder_DirectSalesOrderFilterDTO"></param>
        /// <returns> Số lượng đơn đợi duyệt </returns>
        /// <exception cref="BindException"></exception>
        [Route(MobileDirectSalesOrderRoute.CountPending), HttpPost]
        public async Task<ActionResult<int>> CountPending([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            int count = await DirectSalesOrderService.CountPending(DirectSalesOrderFilter);
            return count;
        }

        [Route(MobileDirectSalesOrderRoute.ListPending), HttpPost]
        public async Task<ActionResult<List<MobileDirectSalesOrder_DirectSalesOrderDTO>>> ListPending([FromBody] MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            DirectSalesOrderFilter DirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO);
            DirectSalesOrderFilter = await DirectSalesOrderService.ToFilter(DirectSalesOrderFilter);
            List<DirectSalesOrder> DirectSalesOrders = await DirectSalesOrderService.ListPending(DirectSalesOrderFilter);
            List<MobileDirectSalesOrder_DirectSalesOrderDTO> MobileDirectSalesOrder_DirectSalesOrderDTOs = DirectSalesOrders
                .Select(c => new MobileDirectSalesOrder_DirectSalesOrderDTO(c)).ToList();
            return MobileDirectSalesOrder_DirectSalesOrderDTOs;
        }

        [Route(MobileDirectSalesOrderRoute.Create), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Create([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder.BaseLanguage = CurrentContext.Language;
            DirectSalesOrder.SaleEmployeeId = CurrentContext.UserId;
            DirectSalesOrder = await DirectSalesOrderService.Create(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(MobileDirectSalesOrderRoute.Update), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Update([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder.BaseLanguage = CurrentContext.Language;
            DirectSalesOrder.SaleEmployeeId = CurrentContext.UserId;
            DirectSalesOrder = await DirectSalesOrderService.Update(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(MobileDirectSalesOrderRoute.CalculatePrice), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> CalculatePrice([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            if (SystemConfiguration.ERP_CALCULATE_SALES_ORDER_PRICE == true)
            {
                string LinkApi = SystemConfiguration.URL_API_FOR_ERP_CALCULATE_SALES_ORDER;
                IRestClient RestClient = new RestClient(LinkApi);
                IRestRequest RestRequest = new RestRequest();
                RestRequest.AddJsonBody(MobileDirectSalesOrder_DirectSalesOrderDTO);
                IRestResponse<MobileDirectSalesOrder_DirectSalesOrderDTO> response = RestClient.Post<MobileDirectSalesOrder_DirectSalesOrderDTO>(RestRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MobileDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount = response.Data.GeneralDiscountAmount;
                    MobileDirectSalesOrder_DirectSalesOrderDTO.SubTotal = response.Data.SubTotal;
                    MobileDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount = response.Data.TotalTaxAmount;
                    MobileDirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax = response.Data.TotalAfterTax;
                    MobileDirectSalesOrder_DirectSalesOrderDTO.Total = response.Data.TotalAfterTax;
                    foreach (MobileDirectSalesOrder_DirectSalesOrderContentDTO MobileDirectSalesOrder_DirectSalesOrderContentDTO in MobileDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents)
                    {
                        var content = response.Data.DirectSalesOrderContents.Where(x => x.Item.ERPCode == MobileDirectSalesOrder_DirectSalesOrderContentDTO.Item.ERPCode).FirstOrDefault();
                        MobileDirectSalesOrder_DirectSalesOrderContentDTO.Amount = content.Amount;
                        MobileDirectSalesOrder_DirectSalesOrderContentDTO.Quantity = content.Quantity;
                        MobileDirectSalesOrder_DirectSalesOrderContentDTO.SalePrice = content.SalePrice;
                        MobileDirectSalesOrder_DirectSalesOrderContentDTO.DiscountPercentage = content.DiscountPercentage;
                        MobileDirectSalesOrder_DirectSalesOrderContentDTO.DiscountAmount = content.DiscountAmount;
                    }
                }
                else
                {
                    return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
                }
            }
            return MobileDirectSalesOrder_DirectSalesOrderDTO;

        }

        [Route(MobileDirectSalesOrderRoute.Send), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Send([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder.BaseLanguage = CurrentContext.Language;
            DirectSalesOrder = await DirectSalesOrderService.Send(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(MobileDirectSalesOrderRoute.Approve), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Approve([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Approve(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);

        }

        [Route(MobileDirectSalesOrderRoute.Reject), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Reject([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Reject(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
        }
        [Route(MobileDirectSalesOrderRoute.Delete), HttpPost]
        public async Task<ActionResult<MobileDirectSalesOrder_DirectSalesOrderDTO>> Delete([FromBody] MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileDirectSalesOrder_DirectSalesOrderDTO.Id))
                return Forbid();

            DirectSalesOrder DirectSalesOrder = ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO);
            DirectSalesOrder = await DirectSalesOrderService.Delete(DirectSalesOrder);
            MobileDirectSalesOrder_DirectSalesOrderDTO = new MobileDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder);
            if (DirectSalesOrder.IsValidated)
                return MobileDirectSalesOrder_DirectSalesOrderDTO;
            else
                return BadRequest(MobileDirectSalesOrder_DirectSalesOrderDTO);
        }

        [Route(MobileDirectSalesOrderRoute.Print), HttpGet]
        public async Task<ActionResult> PrintDirectOrder([FromQuery] long Id)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var DirectSalesOrder = await DirectSalesOrderService.Get(Id);
            if (DirectSalesOrder == null)
                return Content("Đơn hàng không tồn tại");
            MobileDirectSalesOrder_PrintDirectOrderDTO MobileDirectSalesOrder_PrintDTO = new MobileDirectSalesOrder_PrintDirectOrderDTO(DirectSalesOrder);
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var STT = 1;
            if (MobileDirectSalesOrder_PrintDTO.Contents != null)
            {
                foreach (var DirectSalesOrderContent in MobileDirectSalesOrder_PrintDTO.Contents)
                {
                    DirectSalesOrderContent.STT = STT++;
                    DirectSalesOrderContent.AmountString = DirectSalesOrderContent.Amount.ToString("N0", culture);
                    DirectSalesOrderContent.PrimaryPriceString = DirectSalesOrderContent.PrimaryPrice.ToString("N0", culture);
                    DirectSalesOrderContent.QuantityString = DirectSalesOrderContent.Quantity.ToString("N0", culture);
                    DirectSalesOrderContent.RequestedQuantityString = DirectSalesOrderContent.RequestedQuantity.ToString("N0", culture);
                    DirectSalesOrderContent.SalePriceString = DirectSalesOrderContent.SalePrice.ToString("N0", culture);
                    DirectSalesOrderContent.DiscountAmountString = DirectSalesOrderContent.DiscountPercentage.HasValue ? DirectSalesOrderContent.DiscountPercentage.Value.ToString("N0", culture) + "%" : "";
                    DirectSalesOrderContent.TaxPercentageString = DirectSalesOrderContent.TaxPercentage.HasValue ? DirectSalesOrderContent.TaxPercentage.Value.ToString("N0", culture) + "%" : "";
                }
            }
            if (MobileDirectSalesOrder_PrintDTO.Promotions != null)
            {
                foreach (var DirectSalesOrderPromotion in MobileDirectSalesOrder_PrintDTO.Promotions)
                {
                    DirectSalesOrderPromotion.STT = STT++;
                    DirectSalesOrderPromotion.QuantityString = DirectSalesOrderPromotion.Quantity.ToString("N0", culture);
                    DirectSalesOrderPromotion.RequestedQuantityString = DirectSalesOrderPromotion.RequestedQuantity.ToString("N0", culture);
                }
            }

            MobileDirectSalesOrder_PrintDTO.SubTotalString = MobileDirectSalesOrder_PrintDTO.SubTotal.ToString("N0", culture);
            MobileDirectSalesOrder_PrintDTO.Discount = MobileDirectSalesOrder_PrintDTO.GeneralDiscountAmount.HasValue ? MobileDirectSalesOrder_PrintDTO.GeneralDiscountAmount.Value.ToString("N0", culture) : "";
            MobileDirectSalesOrder_PrintDTO.sOrderDate = MobileDirectSalesOrder_PrintDTO.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            MobileDirectSalesOrder_PrintDTO.sDeliveryDate = MobileDirectSalesOrder_PrintDTO.DeliveryDate.HasValue ? MobileDirectSalesOrder_PrintDTO.DeliveryDate.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy") : string.Empty;
            MobileDirectSalesOrder_PrintDTO.TotalString = MobileDirectSalesOrder_PrintDTO.Total.ToString("N0", culture);
            MobileDirectSalesOrder_PrintDTO.TotalTaxString = MobileDirectSalesOrder_PrintDTO.TotalTaxAmount.ToString("N0", culture);
            MobileDirectSalesOrder_PrintDTO.TotalText = Utils.ConvertAmountTostring((long)MobileDirectSalesOrder_PrintDTO.Total);

            ExportTemplate ExportTemplate = await ExportTemplateService.Get(ExportTemplateEnum.PRINT_DIRECT_MOBILE.Id);
            if (ExportTemplate == null)
                return BadRequest("Chưa có mẫu in đơn hàng");


            dynamic Data = new ExpandoObject();
            Data.Order = MobileDirectSalesOrder_PrintDTO;
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
            request.AddFile("file", output.ToArray(), $"Don-hang-truc-tiep-{MobileDirectSalesOrder_PrintDTO.Code}.docx");
            byte[] PdfByteArray = client.DownloadData(request);
            return File(PdfByteArray, "application/pdf", $"Don-hang-truc-tiep-{MobileDirectSalesOrder_PrintDTO.Code}.pdf");
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

        private DirectSalesOrder ConvertDTOToEntity(MobileDirectSalesOrder_DirectSalesOrderDTO MobileDirectSalesOrder_DirectSalesOrderDTO)
        {
            MobileDirectSalesOrder_DirectSalesOrderDTO.TrimString();
            DirectSalesOrder DirectSalesOrder = new DirectSalesOrder();
            DirectSalesOrder.Id = MobileDirectSalesOrder_DirectSalesOrderDTO.Id;
            DirectSalesOrder.Code = MobileDirectSalesOrder_DirectSalesOrderDTO.Code;
            DirectSalesOrder.BuyerStoreId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStoreId;
            DirectSalesOrder.PhoneNumber = MobileDirectSalesOrder_DirectSalesOrderDTO.PhoneNumber;
            DirectSalesOrder.StoreAddress = MobileDirectSalesOrder_DirectSalesOrderDTO.StoreAddress;
            DirectSalesOrder.DeliveryAddress = MobileDirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress;
            DirectSalesOrder.SaleEmployeeId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployeeId;
            DirectSalesOrder.OrganizationId = MobileDirectSalesOrder_DirectSalesOrderDTO.OrganizationId;
            DirectSalesOrder.OrderDate = MobileDirectSalesOrder_DirectSalesOrderDTO.OrderDate;
            DirectSalesOrder.DeliveryDate = MobileDirectSalesOrder_DirectSalesOrderDTO.DeliveryDate;
            DirectSalesOrder.RequestStateId = MobileDirectSalesOrder_DirectSalesOrderDTO.RequestStateId;
            DirectSalesOrder.GeneralApprovalStateId = MobileDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalStateId;
            DirectSalesOrder.EditedPriceStatusId = MobileDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatusId;
            DirectSalesOrder.Note = MobileDirectSalesOrder_DirectSalesOrderDTO.Note;
            DirectSalesOrder.SubTotal = MobileDirectSalesOrder_DirectSalesOrderDTO.SubTotal;
            DirectSalesOrder.GeneralDiscountPercentage = MobileDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountPercentage;
            DirectSalesOrder.GeneralDiscountAmount = MobileDirectSalesOrder_DirectSalesOrderDTO.GeneralDiscountAmount;
            DirectSalesOrder.TotalTaxAmount = MobileDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount;
            DirectSalesOrder.TotalAfterTax = MobileDirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax;
            DirectSalesOrder.PromotionCode = MobileDirectSalesOrder_DirectSalesOrderDTO.PromotionCode;
            DirectSalesOrder.PromotionValue = MobileDirectSalesOrder_DirectSalesOrderDTO.PromotionValue;
            DirectSalesOrder.Total = MobileDirectSalesOrder_DirectSalesOrderDTO.Total;
            DirectSalesOrder.ErpApprovalStateId = MobileDirectSalesOrder_DirectSalesOrderDTO.ErpApprovalStateId;
            DirectSalesOrder.BuyerStore = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore == null ? null : new Store
            {
                Id = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Id,
                Code = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Code,
                Name = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Name,
                ParentStoreId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ParentStoreId,
                OrganizationId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OrganizationId,
                StoreTypeId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreTypeId,
                Telephone = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Telephone,
                ProvinceId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.ProvinceId,
                DistrictId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DistrictId,
                WardId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.WardId,
                Address = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Address,
                DeliveryAddress = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryAddress,
                Latitude = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Latitude,
                Longitude = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.Longitude,
                DeliveryLatitude = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLatitude,
                DeliveryLongitude = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.DeliveryLongitude,
                OwnerName = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerName,
                OwnerPhone = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerPhone,
                OwnerEmail = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.OwnerEmail,
                TaxCode = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.TaxCode,
                LegalEntity = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.LegalEntity,
                StatusId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StatusId,
                StoreStatusId = MobileDirectSalesOrder_DirectSalesOrderDTO.BuyerStore.StoreStatusId,
            };
            DirectSalesOrder.Organization = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization == null ? null : new Organization
            {
                Id = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Id,
                Code = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Code,
                Name = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Name,
                ParentId = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.ParentId,
                Path = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Path,
                Level = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Level,
                StatusId = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.StatusId,
                Phone = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Phone,
                Address = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Address,
                Email = MobileDirectSalesOrder_DirectSalesOrderDTO.Organization.Email,
            };
            DirectSalesOrder.EditedPriceStatus = MobileDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus == null ? null : new EditedPriceStatus
            {
                Id = MobileDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Id,
                Code = MobileDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Code,
                Name = MobileDirectSalesOrder_DirectSalesOrderDTO.EditedPriceStatus.Name,
            };
            DirectSalesOrder.SaleEmployee = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee == null ? null : new AppUser
            {
                Id = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Id,
                Username = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Username,
                DisplayName = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.DisplayName,
                Address = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Address,
                Email = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Email,
                Phone = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Phone,
                PositionId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.PositionId,
                Department = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Department,
                OrganizationId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.OrganizationId,
                SexId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.SexId,
                StatusId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.StatusId,
                Avatar = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Avatar,
                Birthday = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.Birthday,
                ProvinceId = MobileDirectSalesOrder_DirectSalesOrderDTO.SaleEmployee.ProvinceId,
            };
            DirectSalesOrder.DirectSalesOrderContents = MobileDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderContents?
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
            DirectSalesOrder.DirectSalesOrderPromotions = MobileDirectSalesOrder_DirectSalesOrderDTO.DirectSalesOrderPromotions?
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

        private DirectSalesOrderFilter ConvertFilterDTOToFilterEntity(MobileDirectSalesOrder_DirectSalesOrderFilterDTO MobileDirectSalesOrder_DirectSalesOrderFilterDTO)
        {
            DirectSalesOrderFilter DirectSalesOrderFilter = new DirectSalesOrderFilter();
            DirectSalesOrderFilter.Selects = DirectSalesOrderSelect.ALL;
            DirectSalesOrderFilter.Skip = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Skip;
            DirectSalesOrderFilter.Take = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Take;
            DirectSalesOrderFilter.OrderBy = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.OrderBy;
            DirectSalesOrderFilter.OrderType = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.OrderType;

            DirectSalesOrderFilter.Id = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Id;
            DirectSalesOrderFilter.OrganizationId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.OrganizationId;
            DirectSalesOrderFilter.Code = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Code;
            DirectSalesOrderFilter.BuyerStoreId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.BuyerStoreId;
            DirectSalesOrderFilter.PhoneNumber = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.PhoneNumber;
            DirectSalesOrderFilter.StoreAddress = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.StoreAddress;
            DirectSalesOrderFilter.DeliveryAddress = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryAddress;
            DirectSalesOrderFilter.AppUserId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.SaleEmployeeId;
            DirectSalesOrderFilter.OrderDate = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.OrderDate;
            DirectSalesOrderFilter.DeliveryDate = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.DeliveryDate;
            DirectSalesOrderFilter.RequestStateId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.RequestStateId;
            DirectSalesOrderFilter.StoreApprovalStateId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.StoreApprovalStateId;
            DirectSalesOrderFilter.GeneralApprovalStateId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.GeneralApprovalStateId;
            DirectSalesOrderFilter.ErpApprovalStateId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.ErpApprovalStateId;
            DirectSalesOrderFilter.EditedPriceStatusId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.EditedPriceStatusId;
            DirectSalesOrderFilter.Note = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Note;
            DirectSalesOrderFilter.SubTotal = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.SubTotal;
            DirectSalesOrderFilter.GeneralDiscountPercentage = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountPercentage;
            DirectSalesOrderFilter.GeneralDiscountAmount = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.GeneralDiscountAmount;
            DirectSalesOrderFilter.TotalTaxAmount = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.TotalTaxAmount;
            DirectSalesOrderFilter.Total = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Total;
            DirectSalesOrderFilter.StoreStatusId = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.StoreStatusId;
            DirectSalesOrderFilter.Search = MobileDirectSalesOrder_DirectSalesOrderFilterDTO.Search;
            return DirectSalesOrderFilter;
        }
    }
}

