using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_DirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long OrganizationId { get; set; }
        public long BuyerStoreId { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ErpApprovalStateId { get; set; }
        public long? positionId { get; set; }
        public long? provinceId { get; set; }
        public long? StoreApprovalStateId { get; set; }
        public long RequestStateId { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public string PromotionCode { get; set; }
        public decimal? PromotionValue { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public long? StoreCheckingId { get; set; }
        public long? CreatorId { get; set; }
        public long? StoreUserCreatorId { get; set; }
        public long? DirectSalesOrderSourceTypeId { get; set; }
        public long GeneralApprovalStateId { get; set; }
        public WebDirectSalesOrder_GeneralApprovalStateDTO GeneralApprovalState { get; set; }
        public WebDirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public WebDirectSalesOrder_AppUserDTO Creator { get; set; }
        public WebDirectSalesOrder_StoreUserDTO StoreUserCreator { get; set; }
        public WebDirectSalesOrder_DirectSalesOrderSourceTypeDTO DirectSalesOrderSourceType { get; set; }
        public WebDirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public WebDirectSalesOrder_ErpApprovalStateDTO ErpApprovalState { get; set; }
        public WebDirectSalesOrder_OrganizationDTO Organization { get; set; }
        public List<WebDirectSalesOrder_DirectSalesOrderContentDTO> DirectSalesOrderContents { get; set; }
        public List<WebDirectSalesOrder_DirectSalesOrderPromotionDTO> DirectSalesOrderPromotions { get; set; }
        public WebDirectSalesOrder_RequestStateDTO RequestState { get; set; }
        public WebDirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public WebDirectSalesOrder_StoreApprovalStateDTO StoreApprovalState { get; set; }
        public WebDirectSalesOrder_StoreCheckingDTO StoreChecking { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WebDirectSalesOrder_DirectSalesOrderDTO() { }
        public WebDirectSalesOrder_DirectSalesOrderDTO(DirectSalesOrder DirectSalesOrder)
        {
            this.Id = DirectSalesOrder.Id;
            this.Code = DirectSalesOrder.Code;
            this.OrganizationId = DirectSalesOrder.OrganizationId;
            this.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
            this.PhoneNumber = DirectSalesOrder.PhoneNumber;
            this.StoreAddress = DirectSalesOrder.StoreAddress;
            this.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
            this.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
            this.OrderDate = DirectSalesOrder.OrderDate;
            this.DeliveryDate = DirectSalesOrder.DeliveryDate;
            this.ErpApprovalStateId = DirectSalesOrder.ErpApprovalStateId;
            this.StoreApprovalStateId = DirectSalesOrder.StoreApprovalStateId;
            this.RequestStateId = DirectSalesOrder.RequestStateId;
            this.EditedPriceStatusId = DirectSalesOrder.EditedPriceStatusId;
            this.Note = DirectSalesOrder.Note;
            this.SubTotal = DirectSalesOrder.SubTotal;
            this.GeneralDiscountPercentage = DirectSalesOrder.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
            this.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
            this.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
            this.PromotionCode = DirectSalesOrder.PromotionCode;
            this.PromotionValue = DirectSalesOrder.PromotionValue;
            this.Total = DirectSalesOrder.Total;
            this.RowId = DirectSalesOrder.RowId;
            this.StoreCheckingId = DirectSalesOrder.StoreCheckingId;
            this.CreatorId = DirectSalesOrder.CreatorId;
            this.GeneralApprovalStateId = DirectSalesOrder.GeneralApprovalStateId;
            this.GeneralApprovalState = DirectSalesOrder.GeneralApprovalState == null ? null : new WebDirectSalesOrder_GeneralApprovalStateDTO(DirectSalesOrder.GeneralApprovalState);
            this.DirectSalesOrderContents = DirectSalesOrder.DirectSalesOrderContents == null ? null : DirectSalesOrder.DirectSalesOrderContents
                                                                                                .Select(x => new WebDirectSalesOrder_DirectSalesOrderContentDTO(x))
                                                                                                .ToList();
            this.DirectSalesOrderPromotions = DirectSalesOrder.DirectSalesOrderPromotions == null ? null : DirectSalesOrder.DirectSalesOrderPromotions
                                                                                    .Select(x => new WebDirectSalesOrder_DirectSalesOrderPromotionDTO(x))
                                                                                    .ToList();
            this.DirectSalesOrderSourceTypeId = DirectSalesOrder.DirectSalesOrderSourceTypeId;
            this.BuyerStore = DirectSalesOrder.BuyerStore == null ? null : new WebDirectSalesOrder_StoreDTO(DirectSalesOrder.BuyerStore);
            this.Creator = DirectSalesOrder.Creator == null ? null : new WebDirectSalesOrder_AppUserDTO(DirectSalesOrder.Creator);
            this.StoreUserCreator = DirectSalesOrder.StoreUserCreator == null ? null : new WebDirectSalesOrder_StoreUserDTO(DirectSalesOrder.StoreUserCreator);
            this.DirectSalesOrderSourceType = DirectSalesOrder.DirectSalesOrderSourceType == null ? null : new WebDirectSalesOrder_DirectSalesOrderSourceTypeDTO(DirectSalesOrder.DirectSalesOrderSourceType);
            this.EditedPriceStatus = DirectSalesOrder.EditedPriceStatus == null ? null : new WebDirectSalesOrder_EditedPriceStatusDTO(DirectSalesOrder.EditedPriceStatus);
            this.ErpApprovalState = DirectSalesOrder.ErpApprovalState == null ? null : new WebDirectSalesOrder_ErpApprovalStateDTO(DirectSalesOrder.ErpApprovalState);
            this.Organization = DirectSalesOrder.Organization == null ? null : new WebDirectSalesOrder_OrganizationDTO(DirectSalesOrder.Organization);
            this.RequestState = DirectSalesOrder.RequestState == null ? null : new WebDirectSalesOrder_RequestStateDTO(DirectSalesOrder.RequestState);
            this.SaleEmployee = DirectSalesOrder.SaleEmployee == null ? null : new WebDirectSalesOrder_AppUserDTO(DirectSalesOrder.SaleEmployee);
            this.StoreApprovalState = DirectSalesOrder.StoreApprovalState == null ? null : new WebDirectSalesOrder_StoreApprovalStateDTO(DirectSalesOrder.StoreApprovalState);
            this.StoreChecking = DirectSalesOrder.StoreChecking == null ? null : new WebDirectSalesOrder_StoreCheckingDTO(DirectSalesOrder.StoreChecking);
            this.CreatedAt = DirectSalesOrder.CreatedAt;
            this.UpdatedAt = DirectSalesOrder.UpdatedAt;
            this.Errors = DirectSalesOrder.Errors;
        }
    }
    public class WebDirectSalesOrder_DirectSalesOrderFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter StoreUserCreatorId { get; set; }
        public IdFilter PositionId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter ErpApprovalStateId { get; set; }
        public IdFilter StoreApprovalStateId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public StringFilter Note { get; set; }
        public DecimalFilter SubTotal { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter TotalTaxAmount { get; set; }
        public DecimalFilter TotalAfterTax { get; set; }
        public StringFilter PromotionCode { get; set; }
        public DecimalFilter PromotionValue { get; set; }
        public DecimalFilter Total { get; set; }
        public GuidFilter RowId { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter DirectSalesOrderSourceTypeId { get; set; }
        public IdFilter GeneralApprovalStateId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public DirectSalesOrderOrder OrderBy { get; set; }
    }
}
