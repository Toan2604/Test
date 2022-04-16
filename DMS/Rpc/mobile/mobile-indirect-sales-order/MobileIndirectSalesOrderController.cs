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
using DMS.Services.MIndirectSalesOrder;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public partial class MobileIndirectSalesOrderController : RpcController
    {
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        private IAppUserService AppUserService;
        private ICheckStateService CheckStateService;
        private IErpApprovalStateService ErpApprovalStateService;
        private IExportTemplateService ExportTemplateService;
        private IIndirectSalesOrderService IndirectSalesOrderService;
        private IItemService ItemService;
        private IRequestStateService RequestStateService;
        private IOrganizationService OrganizationService;
        private IStoreService StoreService;
        private IStoreCheckingService StoreCheckingService;
        private ISystemConfigurationService SystemConfigurationService;
        private IUOW UOW;
        public MobileIndirectSalesOrderController(
            ICurrentContext CurrentContext,
            DataContext DataContext,
            IAppUserService AppUserService,
            ICheckStateService CheckStateService,
            IExportTemplateService ExportTemplateService,
            IErpApprovalStateService ErpApprovalStateService,
            IItemService ItemService,
            IIndirectSalesOrderService IndirectSalesOrderService,
            IOrganizationService OrganizationService,
            IRequestStateService RequestStateService,
            IStoreService StoreService,
            IStoreCheckingService StoreCheckingService,
            ISystemConfigurationService SystemConfigurationService,
            IUOW UOW
        )
        {
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.AppUserService = AppUserService;
            this.CheckStateService = CheckStateService;
            this.IndirectSalesOrderService = IndirectSalesOrderService;
            this.ExportTemplateService = ExportTemplateService;
            this.ErpApprovalStateService = ErpApprovalStateService;
            this.ItemService = ItemService;
            this.OrganizationService = OrganizationService;
            this.RequestStateService = RequestStateService;
            this.StoreService = StoreService;
            this.StoreCheckingService = StoreCheckingService;
            this.SystemConfigurationService = SystemConfigurationService;
            this.UOW = UOW;
        }

        /// <summary>
        /// TẤT CẢ CÁC ĐƠN TRONG QUYỀN
        /// TAB "TẤT CẢ"
        /// </summary>
        /// <param name="MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO"></param>
        /// <returns></returns>
        [Route(MobileIndirectSalesOrderRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            int count = await IndirectSalesOrderService.Count(IndirectSalesOrderFilter);
            return count;
        }

        [Route(MobileIndirectSalesOrderRoute.List), HttpPost]
        public async Task<ActionResult<List<MobileIndirectSalesOrder_IndirectSalesOrderDTO>>> List([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            List<IndirectSalesOrder> IndirectSalesOrders = await IndirectSalesOrderService.List(IndirectSalesOrderFilter);
            List<MobileIndirectSalesOrder_IndirectSalesOrderDTO> MobileIndirectSalesOrder_IndirectSalesOrderDTOs = IndirectSalesOrders
                .Select(c => new MobileIndirectSalesOrder_IndirectSalesOrderDTO(c)).ToList();
            return MobileIndirectSalesOrder_IndirectSalesOrderDTOs;
        }
        /// <summary>
        /// Tất cả các đơn trừ các đơn đang đợi duyệt  
        /// TAB "CỦA TÔI"
        /// </summary>
        /// <param name="MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO"></param>
        /// <returns></returns>
        [Route(MobileIndirectSalesOrderRoute.CountInScoped), HttpPost]
        public async Task<ActionResult<int>> CountInScoped([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            int count = await IndirectSalesOrderService.CountInScoped(IndirectSalesOrderFilter);
            return count;
        }

        [Route(MobileIndirectSalesOrderRoute.ListInScoped), HttpPost]
        public async Task<ActionResult<List<MobileIndirectSalesOrder_IndirectSalesOrderDTO>>> ListInScoped([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            List<IndirectSalesOrder> IndirectSalesOrders = await IndirectSalesOrderService.ListInScoped(IndirectSalesOrderFilter);
            List<MobileIndirectSalesOrder_IndirectSalesOrderDTO> MobileIndirectSalesOrder_IndirectSalesOrderDTOs = IndirectSalesOrders
                .Select(c => new MobileIndirectSalesOrder_IndirectSalesOrderDTO(c)).ToList();
            return MobileIndirectSalesOrder_IndirectSalesOrderDTOs;
        }

        /// <summary>
        /// Tất cả các đơn đang đợi duyệt 
        /// TAB "TÔI DUYỆT"
        /// </summary>
        /// <param name="MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO"></param>
        /// <returns> Số lượng đơn đợi duyệt </returns>
        [Route(MobileIndirectSalesOrderRoute.CountPending), HttpPost]
        public async Task<ActionResult<int>> CountPending([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            int count = await IndirectSalesOrderService.CountPending(IndirectSalesOrderFilter);
            return count;
        }

        [Route(MobileIndirectSalesOrderRoute.ListPending), HttpPost]
        public async Task<ActionResult<List<MobileIndirectSalesOrder_IndirectSalesOrderDTO>>> ListPending([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrderFilter IndirectSalesOrderFilter = ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO);
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            List<IndirectSalesOrder> IndirectSalesOrders = await IndirectSalesOrderService.ListPending(IndirectSalesOrderFilter);
            List<MobileIndirectSalesOrder_IndirectSalesOrderDTO> MobileIndirectSalesOrder_IndirectSalesOrderDTOs = IndirectSalesOrders
                .Select(c => new MobileIndirectSalesOrder_IndirectSalesOrderDTO(c)).ToList();
            return MobileIndirectSalesOrder_IndirectSalesOrderDTOs;
        }


        [Route(MobileIndirectSalesOrderRoute.Get), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Get([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            IndirectSalesOrder IndirectSalesOrder = await IndirectSalesOrderService.Get(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id);
            return new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
        }


        [Route(MobileIndirectSalesOrderRoute.Create), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Create([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder.BaseLanguage = CurrentContext.Language;
            IndirectSalesOrder.StoreCheckingId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.StoreCheckingId;
            IndirectSalesOrder.SaleEmployeeId = CurrentContext.UserId;
            IndirectSalesOrder = await IndirectSalesOrderService.Create(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Update), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Update([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder.BaseLanguage = CurrentContext.Language;
            IndirectSalesOrder.StoreCheckingId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.StoreCheckingId;
            IndirectSalesOrder.SaleEmployeeId = CurrentContext.UserId;
            IndirectSalesOrder = await IndirectSalesOrderService.Update(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Send), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Send([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder.BaseLanguage = CurrentContext.Language;
            IndirectSalesOrder = await IndirectSalesOrderService.Send(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
            {
                MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.HasOrder = true;
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            }
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Approve), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Approve([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder = await IndirectSalesOrderService.Approve(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Reject), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Reject([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder = await IndirectSalesOrderService.Reject(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Delete), HttpPost]
        public async Task<ActionResult<MobileIndirectSalesOrder_IndirectSalesOrderDTO>> Delete([FromBody] MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id))
                return Forbid();

            IndirectSalesOrder IndirectSalesOrder = ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
            IndirectSalesOrder = await IndirectSalesOrderService.Delete(IndirectSalesOrder);
            MobileIndirectSalesOrder_IndirectSalesOrderDTO = new MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder);
            if (IndirectSalesOrder.IsValidated)
                return MobileIndirectSalesOrder_IndirectSalesOrderDTO;
            else
                return BadRequest(MobileIndirectSalesOrder_IndirectSalesOrderDTO);
        }

        [Route(MobileIndirectSalesOrderRoute.Print), HttpGet]
        public async Task<ActionResult> Print([FromQuery] long Id)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var IndirectSalesOrder = await IndirectSalesOrderService.Get(Id);
            if (IndirectSalesOrder == null)
                return Content("Đơn hàng không tồn tại");
            MobileIndirectSalesOrder_PrintIndirectOrderDTO MobileIndirectSalesOrder_PrintDTO = new MobileIndirectSalesOrder_PrintIndirectOrderDTO(IndirectSalesOrder);
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-EN");
            var STT = 1;
            if (MobileIndirectSalesOrder_PrintDTO.Contents != null)
            {
                foreach (var IndirectSalesOrderContent in MobileIndirectSalesOrder_PrintDTO.Contents)
                {
                    IndirectSalesOrderContent.STT = STT++;
                    IndirectSalesOrderContent.AmountString = IndirectSalesOrderContent.Amount.ToString("N0", culture);
                    IndirectSalesOrderContent.PrimaryPriceString = IndirectSalesOrderContent.PrimaryPrice.ToString("N0", culture);
                    IndirectSalesOrderContent.QuantityString = IndirectSalesOrderContent.Quantity.ToString("N0", culture);
                    IndirectSalesOrderContent.RequestedQuantityString = IndirectSalesOrderContent.RequestedQuantity.ToString("N0", culture);
                    IndirectSalesOrderContent.SalePriceString = IndirectSalesOrderContent.SalePrice.ToString("N0", culture);
                    IndirectSalesOrderContent.DiscountString = IndirectSalesOrderContent.DiscountPercentage.HasValue ? IndirectSalesOrderContent.DiscountPercentage.Value.ToString("N0", culture) + "%" : "";
                    IndirectSalesOrderContent.TaxPercentageString = IndirectSalesOrderContent.TaxPercentage.HasValue ? IndirectSalesOrderContent.TaxPercentage.Value.ToString("N0", culture) + "%" : "";
                }
            }
            if (MobileIndirectSalesOrder_PrintDTO.Promotions != null)
            {
                foreach (var IndirectSalesOrderPromotion in MobileIndirectSalesOrder_PrintDTO.Promotions)
                {
                    IndirectSalesOrderPromotion.STT = STT++;
                    IndirectSalesOrderPromotion.QuantityString = IndirectSalesOrderPromotion.Quantity.ToString("N0", culture);
                    IndirectSalesOrderPromotion.RequestedQuantityString = IndirectSalesOrderPromotion.RequestedQuantity.ToString("N0", culture);
                }
            }

            MobileIndirectSalesOrder_PrintDTO.SubTotalString = MobileIndirectSalesOrder_PrintDTO.SubTotal.ToString("N0", culture);
            MobileIndirectSalesOrder_PrintDTO.Discount = MobileIndirectSalesOrder_PrintDTO.GeneralDiscountAmount.HasValue ? MobileIndirectSalesOrder_PrintDTO.GeneralDiscountAmount.Value.ToString("N0", culture) : "";
            MobileIndirectSalesOrder_PrintDTO.sOrderDate = MobileIndirectSalesOrder_PrintDTO.OrderDate.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy");
            MobileIndirectSalesOrder_PrintDTO.sDeliveryDate = MobileIndirectSalesOrder_PrintDTO.DeliveryDate.HasValue ? MobileIndirectSalesOrder_PrintDTO.DeliveryDate.Value.AddHours(CurrentContext.TimeZone).ToString("dd-MM-yyyy") : string.Empty;
            MobileIndirectSalesOrder_PrintDTO.TotalString = MobileIndirectSalesOrder_PrintDTO.Total.ToString("N0", culture);
            MobileIndirectSalesOrder_PrintDTO.TotalText = Utils.ConvertAmountTostring((long)MobileIndirectSalesOrder_PrintDTO.Total);

            ExportTemplate ExportTemplate = await ExportTemplateService.Get(ExportTemplateEnum.PRINT_INDIRECT_MOBILE.Id);
            if (ExportTemplate == null)
                return BadRequest("Chưa có mẫu in đơn hàng");

            dynamic Data = new ExpandoObject();
            Data.Order = MobileIndirectSalesOrder_PrintDTO;
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
            request.AddFile("file", output.ToArray(), $"Don-hang-gian-tiep-{MobileIndirectSalesOrder_PrintDTO.Code}.docx");
            byte[] PdfByteArray = client.DownloadData(request);
            return File(PdfByteArray, "application/pdf", $"Don-hang-gian-tiep-{MobileIndirectSalesOrder_PrintDTO.Code}.pdf");
        }

        private IndirectSalesOrder ConvertDTOToEntity(MobileIndirectSalesOrder_IndirectSalesOrderDTO MobileIndirectSalesOrder_IndirectSalesOrderDTO)
        {
            IndirectSalesOrder IndirectSalesOrder = new IndirectSalesOrder();
            IndirectSalesOrder.Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Id;
            IndirectSalesOrder.Code = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Code;
            IndirectSalesOrder.BuyerStoreId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStoreId;
            IndirectSalesOrder.PhoneNumber = MobileIndirectSalesOrder_IndirectSalesOrderDTO.PhoneNumber;
            IndirectSalesOrder.StoreAddress = MobileIndirectSalesOrder_IndirectSalesOrderDTO.StoreAddress;
            IndirectSalesOrder.DeliveryAddress = MobileIndirectSalesOrder_IndirectSalesOrderDTO.DeliveryAddress;
            IndirectSalesOrder.SellerStoreId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStoreId;
            IndirectSalesOrder.SaleEmployeeId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployeeId;
            IndirectSalesOrder.OrganizationId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.OrganizationId;
            IndirectSalesOrder.OrderDate = MobileIndirectSalesOrder_IndirectSalesOrderDTO.OrderDate;
            IndirectSalesOrder.DeliveryDate = MobileIndirectSalesOrder_IndirectSalesOrderDTO.DeliveryDate;
            IndirectSalesOrder.RequestStateId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.RequestStateId;
            IndirectSalesOrder.EditedPriceStatusId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatusId;
            IndirectSalesOrder.Note = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Note;
            IndirectSalesOrder.SubTotal = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SubTotal;
            IndirectSalesOrder.GeneralDiscountPercentage = MobileIndirectSalesOrder_IndirectSalesOrderDTO.GeneralDiscountPercentage;
            IndirectSalesOrder.GeneralDiscountAmount = MobileIndirectSalesOrder_IndirectSalesOrderDTO.GeneralDiscountAmount;
            IndirectSalesOrder.Total = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Total;
            IndirectSalesOrder.StoreCheckingId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.StoreCheckingId;
            IndirectSalesOrder.BuyerStore = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore == null ? null : new Store
            {
                Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Id,
                Code = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Code,
                Name = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Name,
                ParentStoreId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.ParentStoreId,
                OrganizationId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.OrganizationId,
                StoreTypeId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.StoreTypeId,
                Telephone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Telephone,
                ProvinceId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.ProvinceId,
                DistrictId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.DistrictId,
                WardId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.WardId,
                Address = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Address,
                DeliveryAddress = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.DeliveryAddress,
                Latitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Latitude,
                Longitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.Longitude,
                DeliveryLatitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.DeliveryLatitude,
                DeliveryLongitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.DeliveryLongitude,
                OwnerName = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.OwnerName,
                OwnerPhone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.OwnerPhone,
                OwnerEmail = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.OwnerEmail,
                TaxCode = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.TaxCode,
                LegalEntity = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.LegalEntity,
                StatusId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.BuyerStore.StatusId,
            };
            IndirectSalesOrder.EditedPriceStatus = MobileIndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatus == null ? null : new EditedPriceStatus
            {
                Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatus.Id,
                Code = MobileIndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatus.Code,
                Name = MobileIndirectSalesOrder_IndirectSalesOrderDTO.EditedPriceStatus.Name,
            };
            IndirectSalesOrder.Organization = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization == null ? null : new Organization
            {
                Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Id,
                Code = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Code,
                Name = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Name,
                ParentId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.ParentId,
                Path = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Path,
                Level = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Level,
                StatusId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.StatusId,
                Phone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Phone,
                Address = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Address,
                Email = MobileIndirectSalesOrder_IndirectSalesOrderDTO.Organization.Email,
            };
            IndirectSalesOrder.SaleEmployee = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee == null ? null : new AppUser
            {
                Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Id,
                Username = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Username,
                DisplayName = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.DisplayName,
                Address = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Address,
                Email = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Email,
                Phone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Phone,
                PositionId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.PositionId,
                Department = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Department,
                OrganizationId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.OrganizationId,
                SexId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.SexId,
                StatusId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.StatusId,
                Avatar = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Avatar,
                Birthday = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.Birthday,
                ProvinceId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SaleEmployee.ProvinceId,
            };
            IndirectSalesOrder.SellerStore = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore == null ? null : new Store
            {
                Id = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Id,
                Code = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Code,
                Name = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Name,
                ParentStoreId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.ParentStoreId,
                OrganizationId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.OrganizationId,
                StoreTypeId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.StoreTypeId,
                Telephone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Telephone,
                ProvinceId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.ProvinceId,
                DistrictId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.DistrictId,
                WardId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.WardId,
                Address = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Address,
                DeliveryAddress = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.DeliveryAddress,
                Latitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Latitude,
                Longitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.Longitude,
                DeliveryLatitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.DeliveryLatitude,
                DeliveryLongitude = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.DeliveryLongitude,
                OwnerName = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.OwnerName,
                OwnerPhone = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.OwnerPhone,
                OwnerEmail = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.OwnerEmail,
                TaxCode = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.TaxCode,
                LegalEntity = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.LegalEntity,
                StatusId = MobileIndirectSalesOrder_IndirectSalesOrderDTO.SellerStore.StatusId,
            };
            IndirectSalesOrder.IndirectSalesOrderContents = MobileIndirectSalesOrder_IndirectSalesOrderDTO.IndirectSalesOrderContents?
                .Select(x => new IndirectSalesOrderContent
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
                        ProductId = x.Item.ProductId,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ScanCode = x.Item.ScanCode,
                        SalePrice = x.Item.SalePrice,
                        RetailPrice = x.Item.RetailPrice,
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
            IndirectSalesOrder.IndirectSalesOrderPromotions = MobileIndirectSalesOrder_IndirectSalesOrderDTO.IndirectSalesOrderPromotions?
                .Select(x => new IndirectSalesOrderPromotion
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
            return IndirectSalesOrder;
        }
        private IndirectSalesOrderFilter ConvertFilterDTOToFilterEntity(MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO)
        {
            IndirectSalesOrderFilter IndirectSalesOrderFilter = new IndirectSalesOrderFilter();
            IndirectSalesOrderFilter.Selects = IndirectSalesOrderSelect.ALL;
            IndirectSalesOrderFilter.Skip = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Skip;
            IndirectSalesOrderFilter.Take = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Take;
            IndirectSalesOrderFilter.OrderBy = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.OrderBy;
            IndirectSalesOrderFilter.OrderType = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.OrderType;
            IndirectSalesOrderFilter.Id = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Id;
            IndirectSalesOrderFilter.Code = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Code;
            IndirectSalesOrderFilter.BuyerStoreId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.BuyerStoreId;
            IndirectSalesOrderFilter.PhoneNumber = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.PhoneNumber;
            IndirectSalesOrderFilter.StoreAddress = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.StoreAddress;
            IndirectSalesOrderFilter.DeliveryAddress = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.DeliveryAddress;
            IndirectSalesOrderFilter.SellerStoreId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.SellerStoreId;
            IndirectSalesOrderFilter.OrderDate = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.OrderDate;
            IndirectSalesOrderFilter.AppUserId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.SaleEmployeeId;
            IndirectSalesOrderFilter.DeliveryDate = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.DeliveryDate;
            IndirectSalesOrderFilter.RequestStateId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.RequestStateId;
            IndirectSalesOrderFilter.EditedPriceStatusId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.EditedPriceStatusId;
            IndirectSalesOrderFilter.Note = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Note;
            IndirectSalesOrderFilter.Search = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.Search;
            IndirectSalesOrderFilter.StoreCheckingId = MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO.StoreCheckingId;
            return IndirectSalesOrderFilter;
        }
        private async Task<bool> HasPermission(long Id)
        {
            IndirectSalesOrderFilter IndirectSalesOrderFilter = new IndirectSalesOrderFilter();
            IndirectSalesOrderFilter = await IndirectSalesOrderService.ToFilter(IndirectSalesOrderFilter);
            if (Id == 0)
            {

            }
            else
            {
                IndirectSalesOrderFilter.Id = new IdFilter { Equal = Id };
                int count = await IndirectSalesOrderService.Count(IndirectSalesOrderFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

    }
}

