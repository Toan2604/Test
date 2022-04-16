using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_IndirectSalesOrderDTO : DataDTO
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
        public long? StoreCheckingId { get; set; }
        public Guid RowId { get; set; }
        public MobileIndirectSalesOrder_StoreDTO BuyerStore { get; set; }
        public MobileIndirectSalesOrder_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public MobileIndirectSalesOrder_RequestStateDTO RequestState { get; set; }
        public MobileIndirectSalesOrder_AppUserDTO SaleEmployee { get; set; }
        public MobileIndirectSalesOrder_OrganizationDTO Organization { get; set; }
        public MobileIndirectSalesOrder_StoreDTO SellerStore { get; set; }
        public List<MobileIndirectSalesOrder_IndirectSalesOrderContentDTO> IndirectSalesOrderContents { get; set; }
        public List<MobileIndirectSalesOrder_IndirectSalesOrderPromotionDTO> IndirectSalesOrderPromotions { get; set; }
        public MobileIndirectSalesOrder_IndirectSalesOrderDTO() { }
        public MobileIndirectSalesOrder_IndirectSalesOrderDTO(IndirectSalesOrder IndirectSalesOrder)
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
            this.SubTotal = IndirectSalesOrder.SubTotal;
            this.GeneralDiscountPercentage = IndirectSalesOrder.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = IndirectSalesOrder.GeneralDiscountAmount;
            this.Total = IndirectSalesOrder.Total;
            this.StoreCheckingId = IndirectSalesOrder.StoreCheckingId;
            this.RowId = IndirectSalesOrder.RowId;
            this.BuyerStore = IndirectSalesOrder.BuyerStore == null ? null : new MobileIndirectSalesOrder_StoreDTO(IndirectSalesOrder.BuyerStore);
            this.EditedPriceStatus = IndirectSalesOrder.EditedPriceStatus == null ? null : new MobileIndirectSalesOrder_EditedPriceStatusDTO(IndirectSalesOrder.EditedPriceStatus);
            this.RequestState = IndirectSalesOrder.RequestState == null ? null : new MobileIndirectSalesOrder_RequestStateDTO(IndirectSalesOrder.RequestState);
            this.SaleEmployee = IndirectSalesOrder.SaleEmployee == null ? null : new MobileIndirectSalesOrder_AppUserDTO(IndirectSalesOrder.SaleEmployee);
            this.Organization = IndirectSalesOrder.Organization == null ? null : new MobileIndirectSalesOrder_OrganizationDTO(IndirectSalesOrder.Organization);
            this.SellerStore = IndirectSalesOrder.SellerStore == null ? null : new MobileIndirectSalesOrder_StoreDTO(IndirectSalesOrder.SellerStore);
            this.IndirectSalesOrderContents = IndirectSalesOrder.IndirectSalesOrderContents?.Select(x => new MobileIndirectSalesOrder_IndirectSalesOrderContentDTO(x)).ToList();
            this.IndirectSalesOrderPromotions = IndirectSalesOrder.IndirectSalesOrderPromotions?.Select(x => new MobileIndirectSalesOrder_IndirectSalesOrderPromotionDTO(x)).ToList();
            this.Errors = IndirectSalesOrder.Errors;
        }
    }

    public class MobileIndirectSalesOrder_IndirectSalesOrderFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter BuyerStoreId { get; set; }
        public StringFilter PhoneNumber { get; set; }
        public StringFilter StoreAddress { get; set; }
        public StringFilter DeliveryAddress { get; set; }
        public IdFilter SellerStoreId { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public DateFilter OrderDate { get; set; }
        public DateFilter DeliveryDate { get; set; }
        public IdFilter RequestStateId { get; set; }
        public IdFilter EditedPriceStatusId { get; set; }
        public StringFilter Note { get; set; }
        public LongFilter SubTotal { get; set; }
        public LongFilter GeneralDiscountPercentage { get; set; }
        public LongFilter GeneralDiscountAmount { get; set; }
        public LongFilter Total { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public string Search { get; set; }
        public IndirectSalesOrderOrder OrderBy { get; set; }
    }
}
