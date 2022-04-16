using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_IndirectSalesOrderDTO
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
        public long RequestStateId { get; set; }
        public long EditedPriceStatusId { get; set; }
        public string Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public MobileSync_StoreDTO BuyerStore { get; set; }
        public MobileSync_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public MobileSync_RequestStateDTO RequestState { get; set; }
        public MobileSync_AppUserDTO SaleEmployee { get; set; }
        public MobileSync_OrganizationDTO Organization { get; set; }
        public MobileSync_StoreDTO SellerStore { get; set; }
        public List<MobileSync_IndirectSalesOrderContentDTO> IndirectSalesOrderContents { get; set; }
        public List<MobileSync_IndirectSalesOrderPromotionDTO> IndirectSalesOrderPromotions { get; set; }
        public List<MobileSync_RequestWorkflowStepMappingDTO> RequestWorkflowStepMappings { get; set; }
        public MobileSync_IndirectSalesOrderDTO() { }
        public MobileSync_IndirectSalesOrderDTO(IndirectSalesOrder IndirectSalesOrder)
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
            this.DeliveryDate = IndirectSalesOrder.DeliveryDate;
            this.RequestStateId = IndirectSalesOrder.RequestStateId;
            this.EditedPriceStatusId = IndirectSalesOrder.EditedPriceStatusId;
            this.Note = IndirectSalesOrder.Note;
            this.SubTotal = Math.Round(IndirectSalesOrder.SubTotal, 0);
            this.GeneralDiscountPercentage = IndirectSalesOrder.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = IndirectSalesOrder.GeneralDiscountAmount;
            this.Total = Math.Round(IndirectSalesOrder.Total, 0);
            this.CreatedAt = IndirectSalesOrder.CreatedAt;
            this.UpdatedAt = IndirectSalesOrder.UpdatedAt;
            this.BuyerStore = IndirectSalesOrder.BuyerStore == null ? null : new MobileSync_StoreDTO(IndirectSalesOrder.BuyerStore);
            this.EditedPriceStatus = IndirectSalesOrder.EditedPriceStatus == null ? null : new MobileSync_EditedPriceStatusDTO(IndirectSalesOrder.EditedPriceStatus);
            this.RequestState = IndirectSalesOrder.RequestState == null ? null : new MobileSync_RequestStateDTO(IndirectSalesOrder.RequestState);
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new MobileSync_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.Organization = IndirectSalesOrder.Organization == null ? null : new MobileSync_OrganizationDTO(IndirectSalesOrder.Organization);
            this.SellerStore = IndirectSalesOrder.SellerStore == null ? null : new MobileSync_StoreDTO(IndirectSalesOrder.SellerStore);
            this.IndirectSalesOrderContents = IndirectSalesOrder.IndirectSalesOrderContents?.Select(x => new MobileSync_IndirectSalesOrderContentDTO(x)).ToList();
            this.IndirectSalesOrderPromotions = IndirectSalesOrder.IndirectSalesOrderPromotions?.Select(x => new MobileSync_IndirectSalesOrderPromotionDTO(x)).ToList();

        }
    }
}
