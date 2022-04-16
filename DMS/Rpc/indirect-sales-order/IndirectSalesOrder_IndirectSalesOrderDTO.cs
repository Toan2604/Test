using DMS.Common;
using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_IndirectSalesOrderDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long BuyerStoreId { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public long SellerStoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal Total { get; set; }
        public long RequestStateId { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? StoreCheckingId { get; set; }
        public IndirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public IndirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public IndirectSalesOrder_RequestStateDTO RequestState { get; set; }
        public IndirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public IndirectSalesOrder_OrganizationDTO Organization { get; set; }
        public IndirectSalesOrder_StoreDTO SellerStore { get; set; }
        public List<IndirectSalesOrder_IndirectSalesOrderContentDTO> IndirectSalesOrderContents { get; set; }
        public List<IndirectSalesOrder_IndirectSalesOrderPromotionDTO> IndirectSalesOrderPromotions { get; set; }
        public List<IndirectSalesOrder_RequestWorkflowStepMappingDTO> RequestWorkflowStepMappings { get; set; }
        public IndirectSalesOrder_IndirectSalesOrderDTO() { }
        public IndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder IndirectSalesOrder)
        {
            this.Id = IndirectSalesOrder.Id;
            this.Code = IndirectSalesOrder.Code;
            this.BuyerStoreId = IndirectSalesOrder.BuyerStoreId;
            this.PhoneNumber = IndirectSalesOrder.PhoneNumber;
            this.StoreAddress = IndirectSalesOrder.StoreAddress;
            this.DeliveryAddress = IndirectSalesOrder.DeliveryAddress;
            this.SellerStoreId = IndirectSalesOrder.SellerStoreId;
            this.SaleEmployeeId = IndirectSalesOrder.SaleEmployeeId;
            this.OrganizationId = IndirectSalesOrder.OrganizationId;
            this.OrderDate = IndirectSalesOrder.OrderDate;
            this.CreatedAt = IndirectSalesOrder.CreatedAt;
            this.DeliveryDate = IndirectSalesOrder.DeliveryDate;
            this.RequestStateId = IndirectSalesOrder.RequestStateId;
            this.EditedPriceStatusId = IndirectSalesOrder.EditedPriceStatusId;
            this.Note = IndirectSalesOrder.Note;
            this.SubTotal = IndirectSalesOrder.SubTotal;
            this.GeneralDiscountPercentage = IndirectSalesOrder.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = IndirectSalesOrder.GeneralDiscountAmount;
            this.Total = Math.Round(IndirectSalesOrder.Total, 0);
            this.CreatedAt = IndirectSalesOrder.CreatedAt;
            this.UpdatedAt = IndirectSalesOrder.UpdatedAt;
            this.RowId = IndirectSalesOrder.RowId;
            this.StoreCheckingId = IndirectSalesOrder.StoreCheckingId;
            this.BuyerStore = IndirectSalesOrder.BuyerStore == null ? null : new IndirectSalesOrder_StoreDTO(IndirectSalesOrder.BuyerStore);
            this.EditedPriceStatus = IndirectSalesOrder.EditedPriceStatus == null ? null : new IndirectSalesOrder_EditedPriceStatusDTO(IndirectSalesOrder.EditedPriceStatus);
            this.RequestState = IndirectSalesOrder.RequestState == null ? null : new IndirectSalesOrder_RequestStateDTO(IndirectSalesOrder.RequestState);
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new IndirectSalesOrder_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.Organization = IndirectSalesOrder.Organization == null ? null : new IndirectSalesOrder_OrganizationDTO(IndirectSalesOrder.Organization);
            this.SellerStore = IndirectSalesOrder.SellerStore == null ? null : new IndirectSalesOrder_StoreDTO(IndirectSalesOrder.SellerStore);
            this.IndirectSalesOrderContents = IndirectSalesOrder.IndirectSalesOrderContents?.Select(x => new IndirectSalesOrder_IndirectSalesOrderContentDTO(x)).ToList();
            this.IndirectSalesOrderPromotions = IndirectSalesOrder.IndirectSalesOrderPromotions?.Select(x => new IndirectSalesOrder_IndirectSalesOrderPromotionDTO(x)).ToList();
            this.RequestWorkflowStepMappings = IndirectSalesOrder.RequestWorkflowStepMappings?
                .Where(x => x.WorkflowStateId != WorkflowStateEnum.NEW.Id)
                .Select(x => new IndirectSalesOrder_RequestWorkflowStepMappingDTO(x)).ToList();
            this.Errors = IndirectSalesOrder.Errors;
        }
    }

    public class IndirectSalesOrder_IndirectSalesOrderFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public StringFilter BuyerStoreCode { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter SellerStoreId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public StringFilter Note { get; set; }
        public DecimalFilter SubTotal { get; set; }
        public DecimalFilter GeneralDiscountPercentage { get; set; }
        public DecimalFilter GeneralDiscountAmount { get; set; }
        public DecimalFilter Total { get; set; }
        public IdFilter StoreStatusId { get; set; }
        public IndirectSalesOrderOrder OrderBy { get; set; }
    }
}
